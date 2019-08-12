using SqlSugar;
using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    /// <summary>
    /// MVVM角色模型
    /// </summary>
    public class Role : ModelDalBase
    {

        #region 角色名称（唯一）
        private string name;
        /// <summary>
        /// Desc:角色名称（唯一）
        /// Default:
        /// Nullable:False
        /// </summary>           
        [Required(ErrorMessage = "必填项，不能重复，长度小于50个字符"),MaxLength(50, ErrorMessage = "长度小于50个字符")]
        [SugarColumn(ColumnDataType = "Nvarchar(50)")]//custom
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }
        #endregion

    }
}
