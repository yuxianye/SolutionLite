using SqlSugar;
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
    public partial class DeviceNode : ModelDalBase
    {
        #region OPC服务器GUID主键（与Name组合唯一）
        private Guid opcServerId;
        /// <summary>
        /// Desc:OPC服务器GUID主键（与Name组合唯一）
        /// Default:
        /// Nullable:False
        /// </summary>                  
        public Guid OpcServerId
        {
            get { return opcServerId; }
            set { SetProperty(ref opcServerId, value); }
        }
        #endregion

        #region OPC服务器名称
        private string opcServerName;
        /// <summary>
        /// OPC服务器名称,用于查询后的显示
        /// </summary>                  
        [SugarColumn(IsIgnore = true)]
        public string OpcServerName
        {
            get { return opcServerName; }
            set { SetProperty(ref opcServerName, value); }
        }
        #endregion

        #region 节点名称（与OPC ServerId组合唯一）
        private string name;
        /// <summary>
        /// Desc:节点名称（与OPC ServerId组合唯一）
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

        #region 数据类型与KepServer一致
        private DataType dataType;
        /// <summary>
        /// Desc:更新频率
        /// Default:100
        /// Nullable:False
        /// </summary>           
        public DataType DataType
        {
            get { return dataType; }
            set { SetProperty(ref dataType, value); }
        }
        #endregion

        #region 更新频率 Default:100
        private int updateRate = 100;
        /// <summary>
        /// Desc:更新频率
        /// Default:100
        /// Nullable:False
        /// </summary>           
        [Range(100, 10000, ErrorMessage = "更新频率100-10000毫秒")]
        public int UpdateRate
        {
            get { return updateRate; }
            set { SetProperty(ref updateRate, value); }
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
    /// 数据类型，与KepServer一致
    /// </summary>
    public enum DataType : int
    {
        /// <summary>
        /// 一个空引用
        /// </summary>
        [Description("空")]
        Empty = TypeCode.Empty,

        //Object = TypeCode.Object,

        //DBNull = TypeCode.DBNull,

        /// <summary>
        ///  布尔型：true 或 false 的二进制值
        /// </summary>
        [Description("布尔型：true 或 false 的二进制值")]
        Boolean = TypeCode.Boolean,

        /// <summary>
        ///  字符：有符号的 8 位整数数据
        /// </summary>
        [Description("字符：有符号的 8 位整数数据")]
        Char = TypeCode.Char,

        SByte = TypeCode.SByte,

        /// <summary>
        ///  字节：无符号的 8 位整数数据
        /// </summary>
        [Description("字节：无符号的 8 位整数数据")]
        Byte = TypeCode.Byte,

        /// <summary>
        /// 短整型：有符号的 16 位整数数据
        /// </summary>
        [Description("短整型：有符号的 16 位整数数据")]
        Int16 = TypeCode.Int16,

        /// <summary>
        /// 字：无符号的 16 位整数数据
        /// </summary>
        [Description("字：无符号的 16 位整数数据")]
        UInt16 = TypeCode.UInt16,

        /// <summary>
        /// 长整型:有符号的 32 位整数数据
        /// </summary>
        [Description("长整型:有符号的 32 位整数数据")]
        Int32 = TypeCode.Int32,

        /// <summary>
        /// 双字型：无符号的 32 位整数数据
        /// </summary>
        [Description("双字型：无符号的 32 位整数数据")]
        UInt32 = TypeCode.UInt32,

        /// <summary>
        /// 64位有符号整型
        /// </summary>
        [Description("双长整型：有符号的 64 位整数数据")]
        Int64 = TypeCode.Int64,

        /// <summary>
        /// 四字型：无符号的 64 位整数数据
        /// </summary>
        [Description("四字型：无符号的 64 位整数数据")]
        UInt64 = TypeCode.UInt64,

        /// <summary>
        /// 浮点型：32 位实数值 IEEE-754 标准定义
        /// </summary>
        [Description("浮点型：32 位实数值 IEEE-754 标准定义")]
        Single = TypeCode.Single,

        /// <summary>
        /// 16位有符号整型
        /// </summary>
        [Description("双精度：64 位实数值 IEEE-754 标准定义")]
        Double = TypeCode.Double,

        Decimal = TypeCode.Decimal,

        DateTime = TypeCode.DateTime,

        /// <summary>
        /// 字符串：空终止 Unicode 字符串
        /// </summary>
        [Description("字符串：空终止 Unicode 字符串")]
        String = TypeCode.String,

        ///// <summary>
        ///// BCD：两个字节封装的 BCD 值的范围是 0-9999
        ///// </summary>
        //[Description("BCD：两个字节封装的 BCD 值的范围是 0-9999")]
        //BCD = 19,

        ///// <summary>
        ///// LBCD：压缩为四个字节的 BCD 值的范围是 0-99999999
        ///// </summary>
        //[Description("LBCD：压缩为四个字节的 BCD 值的范围是 0-99999999")]
        //LBCD = 12,
    }






}
