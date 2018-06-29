using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace NiconicoDiffer
{
    /// <summary>
    /// ボタンに表示されたテキストを、ターゲットに指定したテキストにうつす
    /// </summary>
    class SetTextButtonAction : TargetedTriggerAction<TextBox>
    {
        // Actionが実行されたときの処理
        protected override void Invoke(object o)
        {
            if (AssociatedObject is Button)
            {
                Target.Text = ((Button)AssociatedObject).Content.ToString();
            }
        }
    }
}
