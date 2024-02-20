using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Models
{
    ///<summary>
    ///
    ///</summary>
    public partial class Equipment : ModelDalBase
    {
        public Equipment()
        {

        }

        #region Code（唯一）
        private string code;
        /// <summary>
        /// Desc:Code（唯一）
        /// Default:Equipment
        /// Nullable:False
        /// </summary>               
        [Required(ErrorMessage = "必填项，不能重复，长度小于50个字符"), MaxLength(50, ErrorMessage = "长度小于50个字符")]
        public string Code
        {
            get { return code; }
            set { SetProperty(ref code, value); }
        }
        #endregion


        #region 名称（唯一）
        private string name;
        /// <summary>
        /// Desc:名称（唯一）
        /// Default:Equipment
        /// Nullable:False
        /// </summary>               
        [Required(ErrorMessage = "必填项，不能重复，长度小于50个字符"), MaxLength(50, ErrorMessage = "长度小于50个字符")]
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }
        #endregion

    }
}
