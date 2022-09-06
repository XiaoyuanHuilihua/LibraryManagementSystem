using LibraryManagementSystem.Models.ReaderModule;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text.Encodings.Web;

namespace MvcMovie.Controllers
{
    public class ReaderController : Controller
    {
        private ReaderModule _readerModule = new ReaderModule();

        [Route("/user_register")]
        [HttpPost]
        public Boolean readerRegister()
        {
            string name = HttpContext.Request.Form["name"];
            long phone = Convert.ToInt64(HttpContext.Request.Form["phone"]);
            string pwd = HttpContext.Request.Form["pwd"];
            string IdCard = HttpContext.Request.Form["IdCard"];
            if (_readerModule.readerResister(name, pwd, phone, IdCard))
                return true;
            return false;
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
