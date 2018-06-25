using Prism.Commands;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace NiconicoDiffer
{
    class ViewModel : INotifyPropertyChanged
    {
#pragma warning disable 0067
        /// <summary>
        /// INotifyPropertyChangedを継承してPropertyChangedを実装しないとメモリリークする
        /// 警告が出るので無視設定する
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore 0067

        /// <summary>
        /// POCO
        /// </summary>
        private BaseModel model;

        // Modelに関連したプロパティ
        public ReadOnlyReactiveCollection<VideoData> DataList { get; private set; }

        /// <summary>
        /// ボタンにバインドするコマンド定義
        /// </summary>
        public ReactiveCommand GetVideos { get; }

        /// <summary>
        /// ボタンにバインドするコマンド定義
        /// 選択したSmIdを削除対象にする
        /// </summary>
        public ReactiveCommand BlockSmId { get; }

        /// <summary>
        /// ボタンにバインドするコマンド定義
        /// 選択した動画の投稿者を削除対象にする
        /// </summary>
        public ReactiveCommand BlockUser { get; }

        /// <summary>
        /// ボタンにバインドするコマンド定義
        /// 入力したタグを削除対象にする
        /// </summary>
        public ReactiveCommand BlockTag { get; }

        /// <summary>
        /// リンクにバインドするコマンド定義
        /// </summary>
        public ReactiveCommand ViewVideo { get; }
        
        // 他のオブジェクトに関連したプロパティ
        // リストの列数
        public ReactiveProperty<int> ListColumn { get; }

        public ViewModel()
        {
            // Modelクラスを初期化
            model = new BaseModel();

            // リストの連動設定
            DataList = model.DataList.ToReadOnlyReactiveCollection();

            // コマンド
            // 実行の許可/不許可を制御するIObservable<bool>
            // このValueがtrueかfalseかで制御される
            ReactiveProperty<bool> gate = new ReactiveProperty<bool>(true);

            // gateを使って実行制御する
            GetVideos = new ReactiveCommand(gate);
            GetVideos.Subscribe(
                d =>
                {
                    // 実行可能時に実行する処理
                    model.GetVideos();
                }
            );

            BlockSmId = new ReactiveCommand(gate);
            BlockSmId.Subscribe(
                d =>
                {
                    // 実行可能時に実行する処理
                    model.BlockSmId(d as string);
                }
            );

            BlockUser = new ReactiveCommand(gate);
            BlockUser.Subscribe(
                d =>
                {
                    // 実行可能時に実行する処理
                    model.BlockUser(d as string, true);
                }
            );

            BlockTag = new ReactiveCommand(gate);
            BlockTag.Subscribe(
                d =>
                {
                    // 実行可能時に実行する処理
                    TextBox textBox = (TextBox)d;
                    model.BlockTag(textBox.Text, true);
                }
            );

            ViewVideo = new ReactiveCommand(gate);
            ViewVideo.Subscribe(
                url =>
                {
                    // 実行可能時に実行する処理
                    model.ViewVideo(url as string);
                }
            );

            // 他のオブジェクトに関連したプロパティ
            // リストの列数
            ListColumn = model.ToReactivePropertyAsSynchronized(
                m => m.ListColumn,   // BaseModel.Xに対応付け
                x => x,
                s => s,   // doubleに変換
                ReactivePropertyMode.DistinctUntilChanged       // 同じ値が設定されても変更イベントを実行しない
                    | ReactivePropertyMode.RaiseLatestValueOnSubscribe // ReactivePropertyをSubscribeしたタイミングで現在の値をOnNextに発行する
                );
        }

        // 表示列数を変更する
        public void SetColumn(int column)
        {
            model.ListColumn = column;
        }
    }
}
