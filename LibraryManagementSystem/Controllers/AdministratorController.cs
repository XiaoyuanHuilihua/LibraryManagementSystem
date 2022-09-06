using LibraryManagementSystem.Models.UserManagementModules;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text.Encodings.Web;

namespace MvcMovie.Controllers
{
    public class AdministratorController : Controller
    {
        private AdministratorModule _administratorModule = new AdministratorModule();

        /// <summary>
        /// 搜索所有用户
        /// </summary>
        /// <returns></returns>
        [Route("/views_readers")]
        [HttpGet]
        public Boolean ViewAllReaderInfo()
        {
            Dictionary<string, Object> dataReturn = new Dictionary<string, Object>();
            List<Object> datasReturn = new List<object>();
            List<Object> readersList = new List<object>();
            Dictionary<string, string> readerInfo = new Dictionary<string, string>();

            string adminId = HttpContext.Request.Form["adminId"];
            if (_administratorModule.CheckReturnReaderInfo(adminId))
            {
                dataReturn.Add("status", "true");
                DataRowCollection infos = _administratorModule.ViewAllReaderInformation();
                foreach (DataRow info in infos)
                {
                    readerInfo.Add("readerId", info[0].ToString());
                    readerInfo.Add("name", info[1].ToString());
                    readerInfo.Add("phone", info[3].ToString());
                    readerInfo.Add("IdCard", info[4].ToString());
                    readersList.Add(readerInfo);
                }
                dataReturn.Add("userData", readersList);
                datasReturn.Add(dataReturn);
            }
        }

        [Route("/user_login")]
        [HttpPost]
        public Boolean readerLogin()
        {
            string phone = HttpContext.Request.Form["phone"];
            string pwd = HttpContext.Request.Form["pwd"];
            if (_readerModule.readerLogin(phone, pwd))
                return true;
            return false;
        }

        [Route("/user_info")]
        [HttpGet]
        public string viewUserInfo()
        {
            Dictionary<string, Object> dataReturn = new Dictionary<string, Object>();
            List<Object> userList = new List<object>();
            Dictionary<string, string> userInfo = new Dictionary<string, string>();

            string readerId = HttpContext.Request.Query["readerId"];
            if (_readerModule.readerCheck(readerId))
            {
                dataReturn.Add("status", "true");
                DataRow info = _readerModule.ViewReaderInformation(readerId);
                userInfo.Add("readerId", info[0].ToString());
                userInfo.Add("name", info[1].ToString());
                userInfo.Add("phone", info[3].ToString());
                userInfo.Add("IdCard", info[4].ToString());
                userList.Add(userInfo);
                dataReturn.Add("userData", userList);
                return JsonConvert.SerializeObject(dataReturn);
            }
            dataReturn.Add("status", "false");
            dataReturn.Add("userData", userList);
            return JsonConvert.SerializeObject(dataReturn);
        }

        [Route("/user_logout")]
        [HttpGet]
        public void userLogout()
        {
            string phone = HttpContext.Request.Query["phone"];
            _readerModule.readerLogout(phone);
        }
    }
}
