using System.Text.RegularExpressions;

namespace Utility
{
    /// <summary>
    /// 数字帮助类
    /// </summary>
    public static class NumberHelper
    {
        /// <summary>
        /// 判断字符串是否是数字
        /// </summary>
        public static bool IsNumber(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return false;
            const string pattern = "^[0-9]*$";
            Regex rx = new Regex(pattern);
            return rx.IsMatch(s);
        }


        public static bool IsNumber(string s, bool b)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                return b;
            }
            //
            const string pattern = "^[0-9]*$";
            Regex rx = new Regex(pattern);
            return rx.IsMatch(s);
        }
    }
}
