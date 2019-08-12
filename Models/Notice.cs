using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    /// <summary>
    /// MVVM通知模型
    /// </summary>
    public class Notice : ModelDalBase
    {
        #region 内容
        private string content;
        /// <summary>
        /// Desc:内容
        /// Default:
        /// Nullable:False
        /// </summary>           
        [Required(ErrorMessage = "必填项"), MinLength(10, ErrorMessage = "长度大于10个字符"), MaxLength(5000, ErrorMessage = "长度小于5000个字符")]
        public string Content
        {
            get { return content; }
            set { SetProperty(ref content, value); }
        }
        #endregion

        #region 排序码，默认1
        private double orderCode;
        /// <summary>
        /// Desc:排序码，默认1
        /// Default:1
        /// Nullable:False    
        [Required(ErrorMessage = "必填项"), Range(0, int.MaxValue, ErrorMessage = "大于0小于65535")]
        public double OrderCode
        {
            get { return orderCode; }
            set { SetProperty(ref orderCode, value); }
        }
        #endregion

        #region 是否启用，默认启用1
        private bool isEnable = true;
        /// <summary>
        /// Desc:是否启用，默认启用1
        /// Default:1
        /// Nullable:False    
        public bool IsEnable
        {
            get { return isEnable; }
            set { SetProperty(ref isEnable, value); }
        }
        #endregion
    }
}
