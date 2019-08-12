using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    /// <summary>
    /// MVVM修改密码模型
    /// </summary>
    public class ModifyPasswordModel : ModelBase
    {
        #region 用户名
        private string name;
        /// <summary>
        /// Desc:用户名
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }
        #endregion

        #region 旧密码
        private string oldPassword;
        /// <summary>
        /// Desc:旧密码
        /// Default:
        /// Nullable:False
        /// </summary>           
        [Required(ErrorMessage = "必填项，长度6-50个字符"), MinLength(6, ErrorMessage = "长度大于6个字符"), MaxLength(50, ErrorMessage = "长度小于50个字符")]
        public string OldPassword
        {
            get { return oldPassword; }
            set { SetProperty(ref oldPassword, value); }
        }
        #endregion

        #region 新密码
        private string newPassword;
        /// <summary>
        /// Desc:新密码
        /// Default:
        /// Nullable:False
        /// </summary>           
        [Required(ErrorMessage = "必填项，长度6-50个字符"), MinLength(6, ErrorMessage = "长度大于6个字符"), MaxLength(50, ErrorMessage = "长度小于50个字符")]
        public string NewPassword
        {
            get { return newPassword; }
            set { SetProperty(ref newPassword, value); }
        }
        #endregion

        #region 确认新密码
        private string confirmNewPassword;
        /// <summary>
        /// Desc:确认新密码
        /// Default:
        /// Nullable:False
        /// </summary>           
        [Required(ErrorMessage = "必填项，长度6-50个字符"), MinLength(6, ErrorMessage = "长度大于6个字符"), MaxLength(50, ErrorMessage = "长度小于50个字符")]
        public string ConfirmNewPassword
        {
            get { return confirmNewPassword; }
            set { SetProperty(ref confirmNewPassword, value); }
        }
        #endregion
    }
}
