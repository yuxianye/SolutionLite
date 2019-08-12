using System;

namespace LogHelper
{
    /// <summary>
    /// 日志帮助类
    /// </summary>
    public static class Logger
    {
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 记录调试信息
        /// </summary>
        /// <param name="message"></param>
        public static void Debug(string message)
        {
            log.Debug(message);
        }

        /// <summary>
        /// 记录带异常的调试信息
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="message"></param>
        public static void Debug(string message, Exception exception)
        {
            log.Debug(exception, message);
        }

        /// <summary>
        /// 记录一般信息
        /// </summary>
        /// <param name="message"></param>
        public static void Info(object message)
        {
            log.Info(message);
        }

        /// <summary>
        /// 记录带异常的一般信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Info(string message, Exception exception)
        {
            log.Info(exception, message);
        }

        /// <summary>
        /// 记录警告信息
        /// </summary>
        /// <param name="message"></param>
        public static void Warn(string message)
        {
            log.Warn(message);
        }

        /// <summary>
        /// 记录带异常的警告信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Warn(string message, Exception exception)
        {
            log.Warn(exception, message);
        }

        /// <summary>
        /// 记录错误信息
        /// </summary>
        /// <param name="message"></param>
        public static void Error(string message)
        {
            log.Error(message);
        }

        /// <summary>
        /// 记录带异常的错误信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Error(string message, Exception exception)
        {
            log.Error(exception, message);
        }

        /// <summary>
        /// 记录严重错误
        /// </summary>
        /// <param name="message"></param>
        public static void Fatal(object message)
        {
            log.Fatal(message);
        }

        /// <summary>
        /// 记录带异常的严重错误
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Fatal(string message, Exception exception)
        {
            log.Fatal(exception, message);
        }
    }
}
