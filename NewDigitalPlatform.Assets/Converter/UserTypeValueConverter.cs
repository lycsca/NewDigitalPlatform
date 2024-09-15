using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace NewDigitalPlatform.Assets.Converter
{
    public class UserTypeValueConverter : IValueConverter
    {
        //转换器，
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return "(null)";
            else if (value.ToString() == "0") return "操作员";
            else if (value.ToString() == "1") return "技术员";
            else if (value.ToString() == "10") return "信息管理员";

            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    //public enum UserType
    //{
    //    操作员 = 0,
    //    技术员 = 1,
    //    信息管理员 = 10
    //}
}
