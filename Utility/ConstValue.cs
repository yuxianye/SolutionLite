using System.Reflection;

namespace Utility
{
    /// <summary>
    /// 常量
    /// </summary>
    public class ConstValue
    {
        /// <summary>
        /// AppPath(包含末尾的斜线)，应用程序的exe路径
        /// </summary>
        public static readonly string AppPath = Assembly.GetEntryAssembly().Location.Substring(0, Assembly.GetEntryAssembly().Location.LastIndexOf('\\') + 1);

        public static readonly string DefaultIconPath = "pack://application:,,,/Desktop.Resource;component/Images/Logo_16.ico";

        #region 日期时间格式化字符串

        /// <summary>
        /// 日期时间格式：yyyyMMddHHmmssffff
        /// </summary>
        public const string Datetime_yyyyMMddHHmmssffff = @"yyyyMMddHHmmssffff";


        /// <summary>
        /// 日期时间格式：yyyyMMddHHmmssfff
        /// </summary>
        public const string Datetime_yyyyMMddHHmmssfff = @"yyyyMMddHHmmssfff";


        /// <summary>
        /// 日期时间格式：yyyyMMddHHmmss
        /// </summary>
        public const string Datetime_yyyyMMddHHmmss = @"yyyyMMddHHmmss";

        /// <summary>
        /// 日期时间格式：yyyyMMdd
        /// </summary>
        public const string Datetime_yyyyMMdd = @"yyyyMMdd";

        /// <summary>
        /// 日期时间格式：yyyy/MM/dd
        /// </summary>
        public const string Datetime_yyyyMMdd2 = @"yyyy/MM/dd";

        /// <summary>
        /// 日期时间格式：yyyy-MM-dd
        /// </summary>
        public const string Datetime_yyyyMMdd3 = @"yyyy-MM-dd";

        /// <summary>
        /// 日期时间格式：HH:mm:ss
        /// </summary>
        public const string Datetime_HHmmss2 = @"HH:mm:ss";


        /// <summary>
        /// 日期时间格式：HHmmss
        /// </summary>
        public const string Datetime_HHmmss = @"HHmmss";

        /// <summary>
        /// 日期时间格式：HH:mm
        /// </summary>
        public const string DeteHHmm = @"HH:mm";

        /// <summary>
        /// 日期时间格式：yyMMddHHmmss
        /// </summary>
        public const string DeteyyMMddHHmmss = @"yyMMddHHmmss";

        #endregion

        #region 数值格式化字符串

        /// <summary>
        /// 数值格式化字符串，精确到0位小数
        /// </summary>
        public const string NumericFormatString0 = @"{0:N0}";

        /// <summary>
        /// 数值格式化字符串，精确到1位小数
        /// </summary>
        public const string NumericFormatString1 = @"{0:N1}";

        /// <summary>
        /// 数值格式化字符串，精确到2位小数
        /// </summary>
        public const string NumericFormatString2 = @"{0:N2}";

        /// <summary>
        /// 数值格式化字符串，精确到2位小数
        /// </summary>
        public const string NumericFormatString3 = @"{0:N3}";

        #endregion

    }
}
