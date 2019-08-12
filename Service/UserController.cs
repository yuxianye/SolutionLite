using System;
using System.Web.Http;

namespace Service
{
    public class UserController : BaseApiController
    {
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
#if DEBUG
        [HttpGet]
#endif
        public IHttpActionResult date()
        {
            return Json(DateTime.Now.ToString());
        }

#if DEBUG
        [HttpGet]
#endif
        public IHttpActionResult GetUser()
        {
            return Json(DateTime.Now.ToString());
        }

        public IHttpActionResult GetUser(string id)
        {
            //Bll.Bll userBll = new Bll.Bll();
            //bool aa = userBll.UserDb.Insert(new User() { UserName = "yuxianye", NickName = "yuxianye", PasswordHash = "123" });
            int cc = 132;// userBll.Count(a => a.Id != -1);
            return Json(cc + DateTime.Now.ToString() + id);
        }


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
