using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop.VisionModule
{
    //如果好用，请收藏地址，帮忙分享。
    public class VisionResult
    {
        /// <summary>
        /// 
        /// </summary>
        public uint Label { get; set; }
        ///// <summary>
        ///// 
        ///// </summary>
        //public string imageSource { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string PredictedLabel { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<float> Score { get; set; }
    }
}
