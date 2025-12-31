// ═══════════════════════════════════════════════════════════════════════════════
//  Live x Shop Pro - Boolean Converters
//  Value Converters สำหรับ Boolean ต่างๆ
//  พัฒนาโดย Xman Studio
// ═══════════════════════════════════════════════════════════════════════════════

using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using MaterialDesignThemes.Wpf;

namespace LiveXShopPro.Desktop.Converters;

/// <summary>
/// แปลง Boolean เป็น Visibility
/// True = Visible, False = Collapsed
/// </summary>
public class BoolToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            // ถ้ามี parameter "Invert" ให้กลับค่า
            if (parameter?.ToString() == "Invert")
                return boolValue ? Visibility.Collapsed : Visibility.Visible;

            return boolValue ? Visibility.Visible : Visibility.Collapsed;
        }
        return Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Visibility visibility)
            return visibility == Visibility.Visible;
        return false;
    }
}

/// <summary>
/// แปลง Boolean เป็น Visibility (Hidden แทน Collapsed)
/// True = Visible, False = Hidden
/// </summary>
public class BoolToHiddenConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            if (parameter?.ToString() == "Invert")
                return boolValue ? Visibility.Hidden : Visibility.Visible;

            return boolValue ? Visibility.Visible : Visibility.Hidden;
        }
        return Visibility.Hidden;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Visibility visibility)
            return visibility == Visibility.Visible;
        return false;
    }
}

/// <summary>
/// แปลง Boolean เป็น Arrow Icon (ขึ้น/ลง)
/// True = ArrowUp, False = ArrowDown
/// </summary>
public class BoolToArrowIconConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            return boolValue ? PackIconKind.ArrowUp : PackIconKind.ArrowDown;
        }
        return PackIconKind.Minus;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// แปลง Boolean เป็นสี (เขียว/แดง)
/// True = Success (เขียว), False = Error (แดง)
/// </summary>
public class BoolToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            // หาสีจาก Application Resources
            var successColor = Application.Current.FindResource("Success") as SolidColorBrush;
            var errorColor = Application.Current.FindResource("Error") as SolidColorBrush;

            return boolValue ? (successColor ?? Brushes.Green) : (errorColor ?? Brushes.Red);
        }
        return Brushes.Gray;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// กลับค่า Boolean
/// True -> False, False -> True
/// </summary>
public class InverseBoolConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
            return !boolValue;
        return true;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
            return !boolValue;
        return false;
    }
}

/// <summary>
/// แปลง Null เป็น Visibility
/// Null = Collapsed, Not Null = Visible
/// </summary>
public class NullToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var isNull = value == null;

        if (parameter?.ToString() == "Invert")
            return isNull ? Visibility.Visible : Visibility.Collapsed;

        return isNull ? Visibility.Collapsed : Visibility.Visible;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// แปลงจำนวนเป็น Visibility
/// 0 = Collapsed, > 0 = Visible
/// </summary>
public class CountToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        int count = 0;

        if (value is int intValue)
            count = intValue;
        else if (value is long longValue)
            count = (int)longValue;
        else if (value is double doubleValue)
            count = (int)doubleValue;

        var hasItems = count > 0;

        if (parameter?.ToString() == "Invert")
            return hasItems ? Visibility.Collapsed : Visibility.Visible;

        return hasItems ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// แปลง String ว่างเป็น Visibility
/// Empty/Null = Collapsed, Has Value = Visible
/// </summary>
public class StringToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var hasValue = !string.IsNullOrEmpty(value?.ToString());

        if (parameter?.ToString() == "Invert")
            return hasValue ? Visibility.Collapsed : Visibility.Visible;

        return hasValue ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
