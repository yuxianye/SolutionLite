using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Models
{
    ///<summary>
    /// 工序/工站
    ///</summary>
    public partial class Station : ModelDalBase
    {
        public Station()
        {

        }

        #region Code（唯一）
        private string code;
        /// <summary>
        /// Desc:Code（唯一）
        /// Default:Station
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
        /// Default:Station
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
