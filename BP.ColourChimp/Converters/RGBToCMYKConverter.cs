﻿using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using BP.ColourChimp.Classes;
using BP.ColourChimp.Extensions;

namespace BP.ColourChimp.Converters
{
    [ValueConversion(typeof(Color), typeof(CMYKColor))]
    public class RGBToCMYKConverter : IValueConverter
    {
        #region Implementation of IValueConverter

        /// <summary>Converts a value. </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns <see langword="null" />, the valid null value is used.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is System.Windows.Media.Color color)
                return color.ToCMYK();

            return new CMYKColor(0, 0, 0, 0);
        }

        /// <summary>Converts a value. </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns <see langword="null" />, the valid null value is used.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is CMYKColor color)
                return color.ToColor();

            return Color.FromArgb(0, 0, 0, 0);
        }

        #endregion
    }
}