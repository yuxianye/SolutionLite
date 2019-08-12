using System;
using System.Web.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;
using Utility;

namespace Service
{
    public class DeviceController : BaseApiController
    {
        /// <summary>
        /// 读取设备节点
        /// </summary>
        /// <param name="seviceUri"></param>
        /// <param name="node"></param>
        /// <returns></returns>
#if DEBUG
        [HttpGet]
#endif

        public async Task<IHttpActionResult> Read(string seviceUri, string node)
        {
            //IList<OpcUaDataItem>
            var opcUaClientHelper = SolutionService.OpcUaClientHelperList?.FirstOrDefault(a => a.ServerUri == seviceUri);
            var result = await opcUaClientHelper?.Read(new OpcUaHelper.OpcUaDataItem() { Name = node });
            return Json(result);
        }

        /// <summary>
        /// 写设备节点
        /// </summary>
        /// <param name="seviceUri"></param>
        /// <param name="node"></param>
        /// <param name="value"></param>
        /// <returns></returns>
#if DEBUG
        [HttpGet]
#endif
        public async Task<IHttpActionResult> Write(string seviceUri, string node, string value)
        {

            var opcUaClientHelper = SolutionService.OpcUaClientHelperList?.FirstOrDefault(a => a.ServerUri == seviceUri);
            var opcUaDataItem = new OpcUaHelper.OpcUaDataItem()
            {
                Name = node,
                NewValue = 0,
                OldValue = 0,
                ValueType = ((TypeCode)DataType.UInt16).ToType(),
            };

            var result = await opcUaClientHelper?.Write(opcUaDataItem, UInt16.Parse(value));
            LogHelper.Logger.Info($"WebApi Device Write:{node} Value:{value }");

            return Json(result);
        }



        //#if DEBUG
        //        [HttpGet]
        //#endif
        //        public IHttpActionResult Index(string aa)
        //        {

        //            return Json("欢迎使用！");
        //        }


        //        //测试地址   http://localhost:13800/api/user/date
        //#if DEBUG
        //        [HttpGet]
        //#endif 
        //        public string Login(User user)
        //        {
        //            var v = ControllerContext.RequestContext.Url.ToString();

        //            Bll.UserBll userBll = new Bll.UserBll();
        //            var count = userBll.Count(a => a.UserName == "yuxianye");
        //            dynamic[] ids = { "B6675ACA-8CAA-4E58-9819-74456EB700C9" };
        //            //var user2 = userBll.GetByIds(ids);
        //            return DateTime.Today.ToString("yyyy/MM/dd3333333333" + "user2.FirstOrDefault().NikeName + v");

        //            //var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, Module.RememberMe, shouldLockout: false);

        //        }
        //#if DEBUG
        //        [HttpGet]
        //#endif
        //        public IHttpActionResult date2()
        //        {
        //            return Json(DateTime.Now.ToString());
        //        }

        //#if DEBUG
        //        [HttpGet]
        //#endif
        //public IHttpActionResult GetUser()
        //{
        //    return Json(DateTime.Now.ToString());
        //}

        //public async Task<IHttpActionResult> GetUser2(string id)
        //{
        //    //Bll.Bll userBll = new Bll.Bll();
        //    //bool aa = userBll.UserDb.Insert(new User() { UserName = "yuxianye", NickName = "yuxianye", PasswordHash = "123" });
        //    var write = await SolutionService.OpcUaClientHelperList.FirstOrDefault(a => a.ServerUri == "").Write(new OpcUaHelper.OpcUaDataItem() { Name = "ns=2;s=TestChannel.TestDevice.Agv_SettingSpeed2", NewValue = 0, OldValue = 0, ValueType = ((TypeCode)DataType.UInt16).ToType(), }, (UInt16)12);
        //    var v = await SolutionService.OpcUaClientHelperList[0].Read(new OpcUaHelper.OpcUaDataItem() { Name = "ns=2;s=TestChannel.TestDevice.Agv_TaskStatus2", NewValue = 0, OldValue = 0, ValueType = ((TypeCode)DataType.UInt16).ToType(), });
        //    int cc = 132;// userBll.Count(a => a.Id != -1);
        //    return Json(cc + DateTime.Now.ToString() + write + v + id);

        //}


        //// GET api/values
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/values/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/values
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/values/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/values/5
        //public void Delete(int id)
        //{
        //}





    }
}



