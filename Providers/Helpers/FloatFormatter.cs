using System.Globalization;

namespace IN12B8_WindowsService.Providers.Helpers;

public static class FloatFormatter
{
    public static string FormatFloat3Digits(float? value)
    {
        if (value == null || value < 0 || value >= 1000)
        {
            return "---";
        }

        string s = ((float)value).ToString(CultureInfo.InvariantCulture);

        if (value < 10)
        {
            return "--" + s.Substring(0, 1);
        }
        
        if (value < 100)
        {
            return "-" + s.Substring(0, 2);
        }
        
        return s.Substring(0, 3);
    }
}