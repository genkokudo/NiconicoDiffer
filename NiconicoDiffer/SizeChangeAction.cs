using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;

namespace NiconicoDiffer
{
    // ここの依存プロパティに対してバインドする（SizeChangeAction -> View）方法が分からない（できない？）
    /// <summary>
    /// ウィンドウサイズを変更したとき
    /// ViewModelにそのサイズを伝える
    /// </summary>
    //class SizeChangeAction : TargetedTriggerAction<Window>
    class SizeChangeAction : TriggerAction<Window>
    {
        #region 依存プロパティ
        /// <summary>
        /// メニューグリッドの幅（固定）
        /// </summary>
        public double MenuGridWidth
        {
            get { return (double)GetValue(MenuGridWidthProperty); }
            set { SetValue(MenuGridWidthProperty, value); }
        }

        public static readonly DependencyProperty MenuGridWidthProperty =
            DependencyProperty.Register("MenuGridWidth", typeof(double), typeof(SizeChangeAction), new UIPropertyMetadata(null));

        /// <summary>
        /// アイテムテンプレートの幅（固定）
        /// </summary>
        public double ItemTemplateWidth
        {
            get { return (double)GetValue(ItemTemplateWidthProperty); }
            set { SetValue(ItemTemplateWidthProperty, value); }
        }

        public static readonly DependencyProperty ItemTemplateWidthProperty =
            DependencyProperty.Register("ItemTemplateWidth", typeof(double), typeof(SizeChangeAction), new UIPropertyMetadata(null));
        #endregion

        // Actionが実行されたときの処理
        protected override void Invoke(object o)
        {
            var viewModel = (ViewModel)AssociatedObject.DataContext;
            var column = (int)((AssociatedObject.RenderSize.Width - MenuGridWidth) / ItemTemplateWidth);
            viewModel.ListColumn.Value = column;
        }
    }
}
