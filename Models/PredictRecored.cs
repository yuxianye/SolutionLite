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
    public partial class PredictRecored : ModelDalBase
    {

        #region StationId
        private Guid stationId;
        /// <summary>
        /// StationId
        /// </summary>                  
        public Guid StationId
        {
            get { return stationId; }
            set { SetProperty(ref stationId, value); }
        }
        #endregion

        #region StationName
        private string stationName;
        /// <summary>
        /// StationName,用于查询后的显示
        /// </summary>                  
        [SugarColumn(IsIgnore = true)]
        public string StationName
        {
            get { return stationName; }
            set { SetProperty(ref stationName, value); }
        }
        #endregion

        #region 名称
        private string name;
        /// <summary>
        /// Desc:名称（唯一）
        /// Default:OPC Server
        /// Nullable:False
        /// </summary>               
        [Required(ErrorMessage = "必填项，不能重复，长度小于100个字符"), MaxLength(100, ErrorMessage = "长度小于50个字符")]
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }
        #endregion

        #region VehicleType
        private VehicleType vehicleType;
        /// <summary>
        /// Desc:是否启用
        /// Default:unknow
        /// Nullable:False
        /// </summary>          
        public VehicleType VehicleType
        {
            get { return vehicleType; }
            set { SetProperty(ref vehicleType, value); }
        }
        #endregion

        #region ExteriorColor
        private ExteriorColor exteriorColor;
        /// <summary>
        /// Desc:是否启用
        /// Default:unknow
        /// Nullable:False
        /// </summary>          
        public ExteriorColor ExteriorColor
        {
            get { return exteriorColor; }
            set { SetProperty(ref exteriorColor, value); }
        }
        #endregion

        #region ImageUri地址
        private string imageUri;
        /// <summary>
        /// Desc:ImageUri地址
        /// Default:null
        /// Nullable:False
        /// </summary>          
        [Required(ErrorMessage = "必填项，长度小于100个字符"), MaxLength(100, ErrorMessage = "长度小于100个字符")]
        public string ImageUri
        {
            get { return imageUri; }
            set { SetProperty(ref imageUri, value); }
        }
        #endregion
    }

    /// <summary>
    /// VehicleType
    /// </summary>
    public enum VehicleType
    {
        /// <summary>
        /// 未知
        /// </summary>
        [Description("Unknow")]
        Unknow = 0,

        /// <summary>
        /// Opc经典
        /// </summary>
        [Description("Q6L")]
        Q6L = 1,

        /// <summary>
        /// OpcUa统一架构
        /// </summary>
        [Description("Q6L SB")]
        Q6L_SB = 2,

        /// <summary>
        /// OpcUa统一架构
        /// </summary>
        [Description("E6L")]
        E6L = 3,


    }

    /// <summary>
    /// ExteriorColor
    /// </summary>
    public enum ExteriorColor
    {
        /// <summary>
        /// 未知
        /// </summary>
        [Description("Unknow")]
        Unknow = 0,

        /// <summary>
        /// Roast Tea
        /// </summary>
        [Description("Roast Tea")]
        _0I0I = 1,

        /// <summary>
        /// Jasmine White
        /// </summary>
        [Description("Jasmine White")]
        X0X0 = 2,

        /// <summary>
        /// Magnet Grey
        /// </summary>
        [Description("Magnet Grey")]
        G5G5 = 3,
        /// <summary>
        /// Arkona White
        /// </summary>
        [Description("Arkona White")]
        Z9Z9 = 4,
        /// <summary>
        /// Typhoon Gray
        /// </summary>
        [Description("Typhoon Gray")]
        _2L2L = 5,
        /// <summary>
        /// Lilac Gray
        /// </summary>
        [Description("Lilac Gray")]
        _6B6B = 6,
        /// <summary>
        /// Mythos Black
        /// </summary>
        [Description("Mythos Black")]
        _0E0E = 7,

        /// <summary>
        /// Ascari Blue
        /// </summary>
        [Description("Ascari Blue")]
        _9W9W = 8,



    }

}
