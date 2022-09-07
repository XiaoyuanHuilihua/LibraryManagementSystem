using LibraryManagementSystem.Models.ManagementBooksModules;
using LibraryManagementSystem.Models.ReaderModule;
using LibraryManagementSystem.Models.UserManagementModules;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text.Encodings.Web;

namespace MvcMovie.Controllers
{
    public class ManagementBooksController : Controller
    {
        /*
        ----需要的API----
        1，书籍管理子系统
        图书录入功能：
        图书检索功能：
        + 查看所有图书功能：
        图书注销功能：
        预约超期处理功能：
        借书超期处理功能：
        超期受理功能：
        挂失受理功能：
        还书受理功能：OK
        图书损坏处理功能：
        借书受理功能：OK
        */

        private ManagementBooksModule _managementBooksModule = new ManagementBooksModule();
        private ReaderModule _readerModule = new ReaderModule();
        private AdministratorModule _administratorModule = new AdministratorModule();

        /// <summary>
        /// 还书受理功能的API
        /// </summary>
        /// <returns></returns>
        [Route("/report_return")]
        [HttpPost]
        public string ReturnAccep()
        {
            Dictionary<string, Object> dataReturn = new Dictionary<string, Object>();
            string adminId = HttpContext.Request.Form["adminId"];
            string readerId = HttpContext.Request.Form["readerId"];
            string bookId = HttpContext.Request.Form["bookId"];

            if (_administratorModule.CheckAdmin(adminId))
            {
                _managementBooksModule.ReturnAccep(readerId, bookId);
                dataReturn.Add("status", true);
                return JsonConvert.SerializeObject(dataReturn);
            }

            dataReturn.Add("status", false);
            return JsonConvert.SerializeObject(dataReturn);
        }

        /// <summary>
        /// 借书受理功能的API
        /// </summary>
        /// <returns></returns>
        [Route("/report_borrow")]
        [HttpPost]
        public string BorrowAccep()
        {
            Dictionary<string, Object> dataReturn = new Dictionary<string, Object>();
            string adminId = HttpContext.Request.Form["adminId"];
            string readerId = HttpContext.Request.Form["readerId"];
            string bookId = HttpContext.Request.Form["bookId"];

            if (_administratorModule.CheckAdmin(adminId))
            {
                _managementBooksModule.BorrowAccep(readerId, bookId);
                dataReturn.Add("status", true);
                return JsonConvert.SerializeObject(dataReturn);
            }

            dataReturn.Add("status", false);
            return JsonConvert.SerializeObject(dataReturn);
        }

        /// <summary>
        /// 图书检索功能的API
        /// </summary>
        /// <returns></returns>
        [Route("/user_login")]
        [HttpPost]
        public Boolean SearchBook()
        {
            //string phone = HttpContext.Request.Form["phone"];
            //string pwd = HttpContext.Request.Form["pwd"];
            //if (_readerModule.readerLogin(phone, pwd))
            //    return true;
            //return false;
        }

        /// <summary>
        /// 图书注销功能的API
        /// </summary>
        /// <returns></returns>
        [Route("/user_info")]
        [HttpGet]
        public string CancelBook()
        {
            //Dictionary<string, Object> dataReturn = new Dictionary<string, Object>();
            //List<Object> userList = new List<object>();
            //Dictionary<string, string> userInfo = new Dictionary<string, string>();

            //string readerId = HttpContext.Request.Query["readerId"];
            //if (_readerModule.readerCheck(readerId))
            //{
            //    dataReturn.Add("status", "true");
            //    DataRow info = _readerModule.ViewReaderInformation(readerId);
            //    userInfo.Add("readerId", info[0].ToString());
            //    userInfo.Add("name", info[1].ToString());
            //    userInfo.Add("phone", info[3].ToString());
            //    userInfo.Add("IdCard", info[4].ToString());
            //    userList.Add(userInfo);
            //    dataReturn.Add("userData", userList);
            //    return JsonConvert.SerializeObject(dataReturn);
            //}
            //dataReturn.Add("status", "false");
            //dataReturn.Add("userData", userList);
            //return JsonConvert.SerializeObject(dataReturn);
        }

        /// <summary>
        /// 预约超期处理功能的API
        /// </summary>
        [Route("/user_logout")]
        [HttpGet]
        public void ProcessOverdue()
        {
            //string phone = HttpContext.Request.Query["phone"];
            //_readerModule.readerLogout(phone);
        }
    }
}
