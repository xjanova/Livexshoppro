// ═══════════════════════════════════════════════════════════════════════════════
//  Live x Shop Pro - Currency & Number Converters
//  Value Converters สำหรับแสดงผลเงินและตัวเลข
//  พัฒนาโดย Xman Studio
// ═══════════════════════════════════════════════════════════════════════════════

using System.Globalization;
using System.Windows.Data;

namespace LiveXShopPro.Desktop.Converters;

/// <summary>
/// แปลงตัวเลขเป็นสกุลเงินบาท
/// </summary>
public class CurrencyConverter : IValueConverter
{
    private static readonly CultureInfo ThaiCulture = new("th-TH");

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        decimal amount = 0;

        if (value is decimal decimalValue)
            amount = decimalValue;
        else if (value is double doubleValue)
            amount = (decimal)doubleValue;
        else if (value is int intValue)
            amount = intValue;
        else if (value is long longValue)
            amount = longValue;

        // รูปแบบ: ฿1,234.00 หรือ ฿1,234
        var format = parameter?.ToString() ?? "N0";
        return $"฿{amount.ToString(format, ThaiCulture)}";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string str)
        {
            // ลบสัญลักษณ์เงินและ comma ออก
            str = str.Replace("฿", "").Replace(",", "").Trim();
            if (decimal.TryParse(str, out decimal result))
                return result;
        }
        return 0m;
    }
}

/// <summary>
/// แปลงตัวเลขเป็น Compact Format (K, M, B)
/// 1,000 -> 1K, 1,000,000 -> 1M
/// </summary>
public class CompactNumberConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        decimal number = 0;

        if (value is decimal decimalValue)
            number = decimalValue;
        else if (value is double doubleValue)
            number = (decimal)doubleValue;
        else if (value is int intValue)
            number = intValue;
        else if (value is long longValue)
            number = longValue;

        // แปลงเป็น Compact
        if (number >= 1_000_000_000)
            return $"{number / 1_000_000_000:N1}B";
        if (number >= 1_000_000)
            return $"{number / 1_000_000:N1}M";
        if (number >= 1_000)
            return $"{number / 1_000:N1}K";

        return number.ToString("N0");
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// แปลง Decimal เป็นเปอร์เซ็นต์
/// </summary>
public class PercentConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        decimal number = 0;

        if (value is decimal decimalValue)
            number = decimalValue;
        else if (value is double doubleValue)
            number = (decimal)doubleValue;
        else if (value is int intValue)
            number = intValue;

        var sign = number >= 0 ? "+" : "";
        return $"{sign}{number:N1}%";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// แปลง DateTime เป็น "กี่นาทีที่แล้ว" แบบไทย
/// </summary>
public class TimeAgoConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is DateTime dateTime)
        {
            var diff = DateTime.Now - dateTime;

            if (diff.TotalSeconds < 60)
                return "เมื่อสักครู่";
            if (diff.TotalMinutes < 60)
                return $"{(int)diff.TotalMinutes} นาทีที่แล้ว";
            if (diff.TotalHours < 24)
                return $"{(int)diff.TotalHours} ชม.ที่แล้ว";
            if (diff.TotalDays < 7)
                return $"{(int)diff.TotalDays} วันที่แล้ว";
            if (diff.TotalDays < 30)
                return $"{(int)(diff.TotalDays / 7)} สัปดาห์ที่แล้ว";
            if (diff.TotalDays < 365)
                return $"{(int)(diff.TotalDays / 30)} เดือนที่แล้ว";

            return dateTime.ToString("d MMM yyyy", new CultureInfo("th-TH"));
        }

        return "-";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// แปลง DateTime เป็นรูปแบบไทยสั้นๆ
/// </summary>
public class ThaiDateConverter : IValueConverter
{
    private static readonly CultureInfo ThaiCulture = new("th-TH");

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is DateTime dateTime)
        {
            var format = parameter?.ToString() ?? "d MMM yyyy";
            return dateTime.ToString(format, ThaiCulture);
        }
        if (value is DateOnly dateOnly)
        {
            var format = parameter?.ToString() ?? "d MMM yyyy";
            return dateOnly.ToString(format, ThaiCulture);
        }

        return "-";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// แปลง TimeSpan เป็นรูปแบบ HH:MM:SS
/// </summary>
public class TimeSpanConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is TimeSpan timeSpan)
        {
            var format = parameter?.ToString() ?? @"hh\:mm\:ss";
            return timeSpan.ToString(format);
        }

        return "00:00:00";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
