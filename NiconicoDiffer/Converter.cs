using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace NiconicoDiffer
{
    /// <summary>
    /// 比較によってDataTrigger判定するためのコンバータ
    /// </summary>
    public class IsGreaterThanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var v = System.Convert.ToDouble(value);
            var compareValue = double.Parse(parameter as string);
            return v > compareValue;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    ///// <summary>
    ///// 比較によってDataTrigger判定するためのコンバータ
    ///// parameter（定数）よりvalue（変数）が小さければtrue
    ///// </summary>
    //public class IsLessThanConverter : IValueConverter
    //{
    //    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    //    {
    //        Console.WriteLine("p:" + parameter.ToString() + " v:" + value.ToString());
    //        var v = System.Convert.ToDouble(value);
    //        var compareValue = double.Parse(parameter as string);
    //        return v < compareValue;
    //    }
    //    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
