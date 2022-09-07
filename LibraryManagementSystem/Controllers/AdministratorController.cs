using LibraryManagementSystem.Controllers;
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
        /*
        ----需要的API----
        管理员登录：OK
        管理员退出：OK
        搜索帐户(读者)功能：->搜索所有读者信息OK，用js实现这功能？
        编辑帐户(读者)功能：
        查看读者借阅历史功能：
        发送通知功能：
        查看过期未还书的读者名单功能：OK
        编辑座位功能：
        */

        private AdministratorModule _administratorModule = new AdministratorModule();

        /// <summary>
        /// 管理员登录的API
        /// </summary>
        /// <returns></returns>
        [Route("/admin_login")]
        [HttpPost]
        public Boolean adminLogin()
        {
            string adminId = HttpContext.Request.Form["adminId"];
            string pwd = HttpContext.Request.Form["pwd"];
            if (_administratorModule.adminLogin(adminId, pwd))
                return true;
            return false;
        }

        /// <summary>
        /// 管理员退出的API
        /// </summary>
        [Route("/admin_logout")]
        [HttpGet]
        public void userLogout()
        {
            string adminId = HttpContext.Request.Query["adminId"];
            _administratorModule.AdminLogout(adminId);
        }

        /// <summary>
        /// 搜索所有用户的API
        /// </summary>
        /// <returns></returns>
        [Route("/views_readers")]
        [HttpGet]
        public string ViewAllReaderInfo()
        {
            Dictionary<string, Object> dataReturn = new Dictionary<string, Object>();
            List<Object> readersList = new List<object>();

            dataReturn.Add("status", true);
            DataRowCollection infos = Sql.Read("SELECT * FROM READER");
            foreach (DataRow info in infos)
            {
                Dictionary<string, string> readerInfo = new Dictionary<string, string>();
                readerInfo.Add("readerId", info[0].ToString());
                readerInfo.Add("name", info[1].ToString());
                readerInfo.Add("phone", info[3].ToString());
                readerInfo.Add("IdCard", info[4].ToString());
                readersList.Add(readerInfo);
            }
            dataReturn.Add("userData", readersList);
            return JsonConvert.SerializeObject(dataReturn);
        }

        /// <summary>
        /// 查看过期未还书的读者名的API
        /// </summary>
        /// <returns></returns>
        [Route("/view_unreturned")]
        [HttpGet]
        public string ViewUnreturnedList()
        {
            Dictionary<string, Object> dataReturn = new Dictionary<string, Object>();
            List<Object> unreturnedList = new List<object>();

            dataReturn.Add("status", true);
            DataRowCollection infos = _administratorModule.SearchTheListOfReadersWhoHaveNotReturnedBooks();
            foreach (DataRow info in infos)
            {
                Dictionary<string, string> unreturnInfo = new Dictionary<string, string>();
                unreturnInfo.Add("readerId", info[0].ToString());
                unreturnInfo.Add("bookId", info[1].ToString());
                unreturnInfo.Add("borrowDate", info[3].ToString());
                unreturnInfo.Add("latestReturnDate", info[4].ToString());
                unreturnInfo.Add("borrowId", info[4].ToString());
                unreturnInfo.Add("continuedState", info[4].ToString());
                unreturnedList.Add(unreturnInfo);
            }
            dataReturn.Add("nameList", unreturnedList);
            return JsonConvert.SerializeObject(dataReturn);
        }
    }
}
