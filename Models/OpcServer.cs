using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Models
{
    ///<summary>
    ///
    ///</summary>
    public partial class OpcServer : ModelDalBase
    {
        #region OPC Server名称（唯一）
        private string name;
        /// <summary>
        /// Desc:名称（唯一）
        /// Default:OPC Server
        /// Nullable:False
        /// </summary>               
        [Required(ErrorMessage = "必填项，不能重复，长度小于50个字符"), MaxLength(50, ErrorMessage = "长度小于50个字符")]
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }
        #endregion

        #region Uri地址（唯一）
        private string uri;
        /// <summary>
        /// Desc:Uri地址（唯一）
        /// Default:Kepware.KEPServerEX.V6
        /// Nullable:False
        /// </summary>          
        [Required(ErrorMessage = "必填项，不能重复，长度小于100个字符"), MaxLength(50, ErrorMessage = "长度小于100个字符")]
        public string Uri
        {
            get { return uri; }
            set { SetProperty(ref uri, value); }
        }
        #endregion

        #region 是否启用
        private OpcType opcType;
        /// <summary>
        /// Desc:是否启用
        /// Default:1
        /// Nullable:False
        /// </summary>          
        public OpcType OpcType
        {
            get { return opcType; }
            set { SetProperty(ref opcType, value); }
        }
        #endregion

        #region 是否启用
        private bool isEnable = true;
        /// <summary>
        /// Desc:是否启用
        /// Default:1
        /// Nullable:False
        /// </summary>          
        public bool IsEnable
        {
            get { return isEnable; }
            set { SetProperty(ref isEnable, value); }
        }
        #endregion
    }

    /// <summary>
    /// Opc类型
    /// </summary>
    public enum OpcType
    {
        /// <summary>
        /// 未知
        /// </summary>
        [Description("未知")]
        Unknow = 0,

        /// <summary>
        /// Opc经典
        /// </summary>
        [Description("Opc经典")]
        OpcClassics = 1,

        /// <summary>
        /// OpcUa统一架构
        /// </summary>
        [Description("OpcUa统一架构")]
        OpcUa = 2,
    }




}
