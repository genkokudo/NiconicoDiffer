using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using AngleSharp.Parser.Html;
using System.IO;
using AngleSharp.Dom.Html;

namespace NiconicoDiffer
{
    /// <summary>
    /// Model
    /// </summary>
    class BaseModel : BindableBase
    {
        /// <summary>
        /// 動画のリスト
        /// </summary>
        public ObservableCollection<VideoData> DataList { get; } = new ObservableCollection<VideoData>();

        /// <summary>
        /// フィルタリングしているタグリスト
        /// </summary>
        private List<string> RemoveTagList { get; set; }

        /// <summary>
        /// フィルタリングしているユーザリスト
        /// </summary>
        private List<string> RemoveUserList { get; set; }

        // メニューグリッドの幅
        private int listColumn = 1;
        public int ListColumn
        {
            get { return listColumn; }
            set { SetProperty(ref listColumn, value); }
        }

        public BaseModel()
        {
            RemoveTagList = ReadData("tag");
            RemoveUserList = ReadData("userid");
        }

        /// <summary>
        /// 動画一覧を取得する
        /// </summary>
        public async void GetVideos()
        {
            DataList.Clear();

            #region 動画ランキングカテゴリ総合

            // データを取得し、XMLを解析する
            var doc = await GetData("http://www.nicovideo.jp/ranking/fav/hourly/all?rss=2.0");

            // 繰り返し項目
            var listItems = doc.GetElementsByTagName("item")
            .Select(n =>
            {
                // 動画のタイトル
                var title = n.QuerySelector("title").TextContent.Trim();

                // 動画URL
                // どういうわけか、<link>だけ取得できないのでguidで代用する
                var guid = n.QuerySelector("guid").TextContent.Trim();
                // 動画ID
                var sub = guid.Split('/');
                var smId = sub[sub.Length - 1];
                var link = @"http://www.nicovideo.jp/watch/" + smId;

                // 動画の投稿時間
                var pubDate = DateTime.Parse(n.QuerySelector("pubDate").TextContent.Trim());

                // 動画のサムネイル
                var thumbnailImg = n.QuerySelectorAll("img");
                var thumbnail = string.Empty;
                foreach (var item in thumbnailImg)
                {
                    thumbnail = item.Attributes["src"].Value;
                }

                return new { Title = title, Link = link, SmId = smId, PubDate = pubDate, Thumbnail = thumbnail };
            });

            foreach (var item in listItems)
            {
                var titleStr = item.Title.Split('：');
                var vData = new VideoData
                {
                    Rank = titleStr[0],
                    Title = titleStr[1],
                    Url = item.Link,
                    SmId = item.SmId,
                    PubDate = item.PubDate,
                    Thumbnail = item.Thumbnail,
                    IsFiltered = false
                };
                DataList.Add(vData);
            }
            #endregion

            // IDでフィルタリングする
            FilterById("smid");
            FilterById("smidAuto");

            #region 動画詳細

            foreach (var item in DataList)
            {
                // データを取得し、XMLを解析する
                var detailDoc = await GetData($"http://ext.nicovideo.jp/api/getthumbinfo/{item.SmId}");

                if (detailDoc.QuerySelector("error") == null)
                {

                    // 再生時間
                    item.Length = detailDoc.QuerySelector("length").TextContent.Trim();

                    // 再生回数
                    item.ViewCounter = detailDoc.QuerySelector("view_counter").TextContent.Trim();

                    // コメント数
                    item.CommentNum = detailDoc.QuerySelector("comment_num").TextContent.Trim();

                    // マイリス数
                    item.MylistCounter = detailDoc.QuerySelector("mylist_counter").TextContent.Trim();

                    // 最終レス
                    item.LastRes = detailDoc.QuerySelector("last_res_body").TextContent.Trim();
                    if (item.LastRes.Contains("\n"))
                    {
                        // AngleSharpのバグで<last_res_body/>の時バグるので、改行コードがあったらクリア
                        item.LastRes = string.Empty;
                    }

                    // 投稿者ID
                    var tempId = detailDoc.QuerySelector("user_id");
                    if (tempId == null)
                    {
                        // 公式
                        item.UserId = detailDoc.QuerySelector("ch_id").TextContent.Trim();
                        // 投稿者
                        item.UserNickname = detailDoc.QuerySelector("ch_name").TextContent.Trim();
                    }
                    else
                    {
                        item.UserId = tempId.TextContent.Trim();
                        // 投稿者
                        item.UserNickname = detailDoc.QuerySelector("user_nickname").TextContent.Trim();
                    }

                    // タグ
                    item.Tags = detailDoc.QuerySelectorAll("tag").Select(tag => tag.TextContent.Trim()).ToList();

                    // 表示用情報を作成
                    item.CreateDisplay();
                }
            }

            #endregion

            // 以下はフィルタリングした動画IDを自動フィルタIDファイルに入れる
            // タグでフィルタリングする
            foreach (var item in RemoveTagList)
            {
                BlockTag(item, false);
            }
            // 投稿者でフィルタリングする
            foreach (var item in RemoveUserList)
            {
                BlockUser(item, false);
            }
        }

        private async Task<IHtmlDocument> GetData(string url)
        {
            HttpClient client = new HttpClient();
            string data = await client.GetStringAsync(url);

            var parser = new HtmlParser();
            var doc = await parser.ParseAsync(data);

            return doc;
        }

        private void FilterById(string filename)
        {
            var removeList = ReadData(filename);
            var delList = new List<VideoData>();
            foreach (var item in DataList)
            {
                if (removeList.Contains(item.SmId))
                {
                    delList.Add(item);
                }
            }
            foreach (var item in delList)
            {
                DataList.Remove(item);
            }
        }

        /// <summary>
        /// 選択した動画を非表示にする
        /// </summary>
        public void BlockSmId(string smId)
        {
            // ファイルに追加
            AddData("smid", smId);

            // リストから削除
            VideoData delData = null;
            foreach (var item in DataList)
            {
                if (item.SmId == smId)
                {
                    delData = item;
                    break;
                }
            }
            if (delData != null)
            {
                DataList.Remove(delData);
            }
        }

        /// <summary>
        /// 選択した動画のユーザを非表示にする
        /// </summary>
        public void BlockUser(string userId, bool addData)
        {
            // ファイルに追加
            if (addData && !RemoveUserList.Contains(userId))
            {
                AddData("userid", userId);
            }

            // リストから削除
            VideoData delData = null;
            foreach (var item in DataList)
            {
                if (item.UserId != null && item.UserId == userId)
                {
                    delData = item;
                    // ファイルに追加
                    AddData("smidauto", item.SmId);
                }
            }
            if (delData != null)
            {
                DataList.Remove(delData);
            }
        }

        /// <summary>
        /// 入力したタグを非表示にする
        /// </summary>
        public void BlockTag(string tag, bool addData)
        {
            // ファイルに追加
            if (addData && !RemoveTagList.Contains(tag))
            {
                AddData("tag", tag);
            }

            // リストから削除
            VideoData delData = null;
            foreach (var item in DataList)
            {
                if (item.Tags != null && item.Tags.Contains(tag))
                {
                    delData = item;
                    // ファイルに追加
                    AddData("smidauto", item.SmId);
                }
            }
            if (delData != null)
            {
                DataList.Remove(delData);
            }
        }

        /// <summary>
        /// 動画をブラウザで開く
        /// </summary>
        public void ViewVideo(string url)
        {
            System.Diagnostics.Process.Start(url);
        }

        #region データ関係
        /// <summary>
        /// データディレクトリ
        /// </summary>
        private const string DataDirectry = "./data";

        /// <summary>
        /// データを追加する
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="data"></param>
        private void AddData(string filename, string data)
        {
            SafeCreateDirectory(DataDirectry);
            File.AppendAllText($"{DataDirectry}/{filename}.dat", data + Environment.NewLine);
        }

        /// <summary>
        /// データを読み込む
        /// </summary>
        /// <param name="filename"></param>
        private List<string> ReadData(string filename)
        {
            var filepath = $"{DataDirectry}/{filename}.dat";
            SafeCreateDirectory(DataDirectry);
            List<string> list = new List<string>();

            if (File.Exists(filepath))
            {
                using (StreamReader file = new StreamReader(filepath, Encoding.UTF8))
                {
                    string line = "";

                    while ((line = file.ReadLine()) != null)
                    {
                        list.Add(line);
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 指定したパスにディレクトリが存在しない場合
        /// すべてのディレクトリとサブディレクトリを作成します
        /// </summary>
        private DirectoryInfo SafeCreateDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                return null;
            }
            return Directory.CreateDirectory(path);
        }
        #endregion
    }
}
