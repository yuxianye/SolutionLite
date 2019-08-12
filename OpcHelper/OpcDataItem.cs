using System;
using System.Text;

namespace OpcHelper
{
    /// <summary>
    /// Opc数据项
    /// </summary>
    public class OpcDataItem : ICloneable
    {
        public OpcDataItem(string name, int updateRate, string oldValue, string newValue, OpcResult quality)
        {
            this.Name = name;
            this.UpdateRate = updateRate;
            this.OldValue = oldValue;
            this.NewValue = newValue;
            this.Quality = quality;
        }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 更新频率（毫秒）
        /// </summary>
        public int UpdateRate { get; set; } //= 101;

        /// <summary>
        /// 旧值
        /// </summary>
        public object OldValue { get; set; }

        /// <summary>
        /// 新值
        /// </summary>
        public object NewValue { get; set; }

        /// <summary>
        /// 数据质量 
        /// </summary>
        public OpcResult Quality { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Name=");
            sb.Append(Name);
            sb.Append(";");

            sb.Append("UpdateRate=");
            sb.Append(UpdateRate);
            sb.Append(";");

            sb.Append("OldValue=");
            sb.Append(OldValue);
            sb.Append(";");

            sb.Append("NewValue=");
            sb.Append(NewValue);
            sb.Append(";");

            sb.Append("Quality=");
            sb.Append(Quality);
            sb.Append(";");

            string result = sb.ToString();
            sb.Clear();
            sb = null;
            return result;

        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }

}
//public struct OpcDataItem
//{


//    public OpcDataItem(string name, int updateRate, string oldValue, string newValue, OpcResult quality)
//    {
//        this.Name = name;
//        this.UpdateRate = updateRate;
//        this.OldValue = oldValue;
//        this.NewValue = newValue;
//        this.Quality = quality;
//    }
//    /// <summary>
//    /// 名称
//    /// </summary>
//    public string Name;// { get; set; }

//    /// <summary>
//    /// 更新频率（毫秒）
//    /// </summary>
//    public int UpdateRate;//{ get; set; } //= 101;

//    /// <summary>
//    /// 旧值
//    /// </summary>
//    public string OldValue;//{ get; set; }

//    /// <summary>
//    /// 新值
//    /// </summary>
//    public string NewValue;//{ get; set; }

//    /// <summary>
//    /// 数据质量 
//    /// </summary>
//    public OpcResult Quality;//{ get; set; }

//    public override string ToString()
//    {
//        StringBuilder sb = new StringBuilder();
//        sb.Append("Name=");
//        sb.Append(Name);
//        sb.Append(";");

//        sb.Append("UpdateRate=");
//        sb.Append(UpdateRate);
//        sb.Append(";");

//        sb.Append("OldValue=");
//        sb.Append(OldValue);
//        sb.Append(";");

//        sb.Append("NewValue=");
//        sb.Append(NewValue);
//        sb.Append(";");

//        sb.Append("Quality=");
//        sb.Append(Quality);
//        sb.Append(";");

//        string result = sb.ToString();
//        sb.Clear();
//        sb = null;
//        return result;

//    }
//}

