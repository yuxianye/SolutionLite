using SqlSugar;
using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    /// <summary>
    /// MVVM用户模型
    /// </summary>
    public class User : ModelDalBase
    {
        #region 用户名
        private string name;
        /// <summary>
        /// Desc:用户名
        /// Default:
        /// Nullable:False
        /// </summary>           
        [Required(ErrorMessage = "必填项，不能重复，长度小于50个字符"), MaxLength(50, ErrorMessage = "长度小于50个字符")]
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }
        #endregion

        #region 昵称、中文名
        private string nickName;
        /// <summary>
        /// Desc:昵称、中文名
        /// Default:
        /// Nullable:True
        /// </summary>           
        [Required(ErrorMessage = "必填项，不能重复，长度小于50个字符"), MaxLength(50, ErrorMessage = "长度小于50个字符")]
        public string NickName
        {
            get { return nickName; }
            set { SetProperty(ref nickName, value); }
        }
        #endregion

        #region 安全密码
        private string securityPassword;
        /// <summary>
        /// Desc:安全密码
        /// Default:
        /// Nullable:False
        /// </summary>           
        [Required(ErrorMessage = "必填项，长度6-50个字符"), MinLength(6, ErrorMessage = "长度大于6个字符"), MaxLength(50, ErrorMessage = "长度小于50个字符")]

        public string SecurityPassword
        {
            get { return securityPassword; }
            set { SetProperty(ref securityPassword, value); }
        }
        #endregion

        #region 安全密码
        private string confirmSecurityPassword;
        /// <summary>
        /// Desc:安全密码
        /// Default:
        /// Nullable:False
        /// </summary>           
        [Required(ErrorMessage = "必填项，长度6-50个字符"), MinLength(6, ErrorMessage = "长度大于6个字符"), MaxLength(50, ErrorMessage = "长度小于50个字符")]
        [SugarColumn(IsIgnore = true)]
        public string ConfirmSecurityPassword
        {
            get { return confirmSecurityPassword; }
            set { SetProperty(ref confirmSecurityPassword, value); }
        }
        #endregion
    }
}
