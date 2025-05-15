using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace CLientApp.View.Convertors
{
    public class DateToBackGroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateOnly expirationDate)
            {
                var now = DateTime.Now.Date;
                var end = new DateTime(expirationDate, TimeOnly.MinValue);
                var delta = end - now;

                if (delta.TotalDays <= 7)
                    return Brushes.MediumVioletRed; // Одна неделя
                else if (delta.TotalDays <= 30)
                    return Brushes.Green; // Один месяц
                else if (delta.TotalDays <= 90)
                    return Brushes.Yellow; // Три месяца
            }

            return Brushes.Transparent; // По умолчанию
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
