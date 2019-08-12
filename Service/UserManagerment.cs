using Microsoft.AspNet.Identity;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Service
{

    //    public class SolutionManager : UserManager<User, Guid>
    //    {
    //        public SolutionManager(IUserStore<User, Guid> store) : base(store)
    //        {
    //        }

    //        //测试地址   http://localhost:13800/api/user/date
    //        [Authorize]
    //#if DEBUG
    //        [HttpGet]
    //#endif 
    //        public string Login(User user)
    //        {

    //            Bll.UserBll userBll = new Bll.UserBll();
    //            var count = userBll.Count(a => a.Name == "yuxianye");
    //            dynamic[] ids = { "B6675ACA-8CAA-4E58-9819-74456EB700C9" };
    //            var user2 = userBll.GetByIds(ids);
    //            return DateTime.Today.ToString("yyyy/MM/dd3333333333" + user.NikeName + count);

    //            //var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, Module.RememberMe, shouldLockout: false);

    //        }
    //    }


}
