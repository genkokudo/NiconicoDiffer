using Prism.Mvvm;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiconicoDiffer
{
    /// <summary>
    /// 動画データ1件
    /// </summary>
    class VideoData : BindableBase
    {
        /// <summary>
        /// 動画Id
        /// </summary>
        private string smId;
        public string SmId
        {
            get { return smId; }
            set { SetProperty(ref smId, value); }
        }

        /// <summary>
        /// ランク
        /// </summary>
        private string rank;
        public string Rank
        {
            get { return rank; }
            set { SetProperty(ref rank, value); }
        }

        /// <summary>
        /// タイトル
        /// </summary>
        private string title;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        /// <summary>
        /// 動画URL
        /// </summary>
        private string url;
        public string Url
        {
            get { return url; }
            set { SetProperty(ref url, value); }
        }

        /// <summary>
        /// サムネイルURL
        /// </summary>
        private string thumbnail;
        public string Thumbnail
        {
            get { return thumbnail; }
            set { SetProperty(ref thumbnail, value); }
        }

        /// <summary>
        /// 投稿時間
        /// </summary>
        private DateTime pubDate;
        public DateTime PubDate
        {
            get { return pubDate; }
            set { SetProperty(ref pubDate, value); }
        }

        /// <summary>
        /// 再生時間
        /// </summary>
        private string length;
        public string Length
        {
            get { return length; }
            set { SetProperty(ref length, value); }
        }

        /// <summary>
        /// 再生回数
        /// </summary>
        private string viewCounter;
        public string ViewCounter
        {
            get { return viewCounter; }
            set { SetProperty(ref viewCounter, value); }
        }

        /// <summary>
        /// コメント数
        /// </summary>
        private string commentNum;
        public string CommentNum
        {
            get { return commentNum; }
            set { SetProperty(ref commentNum, value); }
        }

        /// <summary>
        /// マイリス数
        /// </summary>
        private string mylistCounter;
        public string MylistCounter
        {
            get { return mylistCounter; }
            set { SetProperty(ref mylistCounter, value); }
        }

        /// <summary>
        /// 最終レス
        /// </summary>
        private string lastRes;
        public string LastRes
        {
            get { return lastRes; }
            set { SetProperty(ref lastRes, value); }
        }

        /// <summary>
        /// 投稿者ID
        /// </summary>
        private string userId;
        public string UserId
        {
            get { return userId; }
            set { SetProperty(ref userId, value); }
        }

        /// <summary>
        /// 投稿者
        /// </summary>
        private string userNickname;
        public string UserNickname
        {
            get { return userNickname; }
            set { SetProperty(ref userNickname, value); }
        }

        /// <summary>
        /// タグ
        /// </summary>
        private List<string> tags;
        public List<string> Tags
        {
            get { return tags; }
            set { SetProperty(ref tags, value); }
        }

        /// <summary>
        /// フィルタ対象
        /// </summary>
        private bool isFiltered;
        public bool IsFiltered
        {
            get { return isFiltered; }
            set { SetProperty(ref isFiltered, value); }
        }

        /// <summary>
        /// 表示用
        /// 固定表示なのでデータを取った後生成する
        /// </summary>
        private string displayHead;
        public string DisplayHead
        {
            get { return displayHead; }
            set { SetProperty(ref displayHead, value); }
        }

        /// <summary>
        /// 表示用
        /// 固定表示なのでデータを取った後生成する
        /// </summary>
        private string displayFoot;
        public string DisplayFoot
        {
            get { return displayFoot; }
            set { SetProperty(ref displayFoot, value); }
        }

        public void CreateDisplay()
        {
            var tag = string.Empty;
            foreach (var item in tags)
            {
                tag += item + " ";
            }

            DisplayHead = $"{rank} {pubDate} {SmId} {userNickname}({userId})";
            DisplayFoot = $"再生時間：{length} 再生数：{viewCounter} コメント数：{commentNum} マイリスト数：{mylistCounter}\n{lastRes}\nタグ：{tag}";

        }
    }
}
