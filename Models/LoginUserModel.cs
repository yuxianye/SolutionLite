using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    /// <summary>
    /// MVVM登陆用户模型
    /// </summary>
    public class LoginUserModel : ModelBase
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

        #region 安全密码
        private string securityPassword;
        /// <summary>
        /// Desc:安全密码
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string SecurityPassword
        {
            get { return securityPassword; }
            set { SetProperty(ref securityPassword, value); }
        }
        #endregion
    }
}
