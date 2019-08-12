using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Service
{
    //#if DEBUG
    //    [HttpGet]
    //#endif
    //#if RELEASE
    //    [Authorize]
    //#endif
    /// <summary>
    /// Web Api的基类
    /// </summary>
    public class BaseApiController : ApiController
    {

        //测试地址   http://localhost:13800/api/user/date
        //#if DEBUG
        //        [HttpGet]
        //#endif
        //        public string Date()
        //        {
        //            Bll.UserBll userBll = new Bll.UserBll();

        //            var count = userBll.Count(a => a.Name == "yuxianye");
        //            dynamic[] ids = { "B6675ACA-8CAA-4E58-9819-74456EB700C9" };
        //            var user = userBll.GetByIds(ids);
        //            return DateTime.Today.ToString("yyyy/MM/dd3333333333" + user.FirstOrDefault().NikeName + count);

        //            //var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, Module.RememberMe, shouldLockout: false);
        //            //UserMangerment

        //        }




    }


}
