using System.Globalization;

namespace MyPhamUsa.Client.Extensions
{
    public static class MoneyExtension
    {
        public static string ToMoney(this string unformattedString)
        {

            var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
            nfi.NumberGroupSeparator = " ";
            var number = long.Parse(unformattedString.Trim());
            string formatted = number.ToString("#,0.00", nfi); // "1 234 897.11"
            return formatted;
        }
    }
}
