using LibraryManagementSystem.Controllers;
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
    public class UserController : Controller
    {
        /*
        ----需要的API----
        读者注册：OK
        读者登录：OK
        修改密码功能：OK
        查看个人信息功能：OK
        书籍预约功能：OK
        读者留言功能：OK
        编辑个人信息功能：
        查看借阅历史功能：
        查看当前借阅功能：
        查看座位功能：
        预约座位功能：
        */

        private ReaderModule _readerModule = new ReaderModule();

        /// <summary>
        /// 用户(读者)注册的API
        /// </summary>
        /// <returns></returns>
        [Route("/user_register")]
        [HttpPost]
        public Boolean userRegister()
        {
            string name = HttpContext.Request.Form["name"];
            string phone = HttpContext.Request.Form["phone"];
            string pwd = HttpContext.Request.Form["pwd"];
            string IdCard = HttpContext.Request.Form["IdCard"];
            if (_readerModule.userRegister(name, pwd, phone, IdCard))
                return true;
            return false;
        }

        /// <summary>
        /// 用户(读者)登录的API
        /// </summary>
        /// <returns></returns>
        [Route("/user_login")]
        [HttpPost]
        public string userLogin()
        {
            Dictionary<string, Object> dataReturn = new Dictionary<string, Object>();
            List<Object> userList = new List<object>();

            string phone = HttpContext.Request.Form["phone"];
            string pwd = HttpContext.Request.Form["pwd"];
            List<Object> reader = _readerModule.userLogin(phone, pwd);
            if (reader.Count != 0)
            {
                DataRow info = (DataRow)reader[0];
                dataReturn.Add("status", true);
                Dictionary<string, string> userInfo = new Dictionary<string, string>();
                userInfo.Add("readerId", info[0].ToString());
                userInfo.Add("name", info[1].ToString());
                userInfo.Add("phone", info[3].ToString());
                userInfo.Add("IdCard", info[4].ToString());
                userList.Add(userInfo);
                dataReturn.Add("userData", userList);
                return JsonConvert.SerializeObject(dataReturn);
            }
            dataReturn.Add("status", false);
            dataReturn.Add("userData", userList);
            return JsonConvert.SerializeObject(dataReturn);
        }

        /// <summary>
        /// 查看个人信息的API
        /// </summary>
        /// <returns></returns>
        [Route("/user_info")]
        [HttpGet]
        public string viewUserInfo()
        {
            Dictionary<string, Object> dataReturn = new Dictionary<string, Object>();
            List<Object> userList = new List<object>();

            string readerId = HttpContext.Request.Query["readerId"];
            if (_readerModule.userCheck(readerId))
            {
                dataReturn.Add("status", true);
                DataRow info = _readerModule.ViewReaderInformation(readerId);
                Dictionary<string, string> userInfo = new Dictionary<string, string>();
                userInfo.Add("readerId", info[0].ToString());
                userInfo.Add("name", info[1].ToString());
                userInfo.Add("phone", info[3].ToString());
                userInfo.Add("IdCard", info[4].ToString());
                userList.Add(userInfo);
                dataReturn.Add("userData", userList);
                return JsonConvert.SerializeObject(dataReturn);
            }
            dataReturn.Add("status", false);
            dataReturn.Add("userData", userList);
            return JsonConvert.SerializeObject(dataReturn);
        }

        /// <summary>
        /// 查看所有座位功能的API
        /// </summary>
        /// <returns></returns>
        [Route("/view_seats")]
        [HttpGet]
        public string viewSeats()
        {
            Dictionary<string, Object> dataReturn = new Dictionary<string, Object>();
            List<Object> seatList = new List<object>();

            string readerId = HttpContext.Request.Query["readerId"];
            if (_readerModule.userCheck(readerId))
            {
                dataReturn.Add("status", true);
                DataRowCollection seats = _readerModule.ViewSeats();
                foreach (DataRow seat in seats)
                {
                    Dictionary<string, string> seatInfo = new Dictionary<string, string>();
                    seatInfo.Add("seatId", seat[0].ToString());
                    seatInfo.Add("seatState", seat[1].ToString());
                    seatInfo.Add("seatDeposit", seat[2].ToString());
                    seatList.Add(seatInfo);
                }
                dataReturn.Add("userData", seatList);
                return JsonConvert.SerializeObject(dataReturn);
            }
            dataReturn.Add("status", false);
            dataReturn.Add("seatData", seatList);
            return JsonConvert.SerializeObject(dataReturn);
        }

        /// <summary>
        /// 预约座位功能的API
        /// </summary>
        /// <returns></returns>
        [Route("/reserve_seat")]
        [HttpGet]
        public Boolean reserveSeat()
        {
            string readerId = HttpContext.Request.Query["readerId"];
            string seatId = HttpContext.Request.Query["seatId"];
            if (_readerModule.userCheck(readerId))
            {
                _readerModule.ReserveSeat(readerId, seatId, DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")),
                    DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")).AddDays(14));
                return true;
            }
            return false;
        }

        /// <summary>
        /// 查看所有图书的API
        /// </summary>
        /// <returns></returns>
        [Route("/user_view_books")]
        [HttpGet]
        public string viewBooks()
        {
            Dictionary<string, Object> dataReturn = new Dictionary<string, Object>();
            List<Object> bookList = new List<object>();

            string readerId = HttpContext.Request.Query["readerId"];
            if (_readerModule.userCheck(readerId))
            {
                dataReturn.Add("status", true);
                DataRowCollection books = _readerModule.ViewBooks();
                foreach (DataRow book in books)
                {
                    Dictionary<string, string> bookInfo = new Dictionary<string, string>();
                    bookInfo.Add("bookId", book[0].ToString());
                    bookInfo.Add("ISBN", book[1].ToString());
                    bookInfo.Add("bookName", book[2].ToString());
                    bookInfo.Add("author", book[3].ToString());
                    bookInfo.Add("publisher", book[4].ToString());
                    bookInfo.Add("publishDate", book[5].ToString());
                    bookInfo.Add("lendReserve", book[6].ToString());
                    bookInfo.Add("bookDetail", book[7].ToString());
                    bookInfo.Add("price", book[8].ToString());
                    bookList.Add(bookInfo);
                }
                dataReturn.Add("userData", bookList);
                return JsonConvert.SerializeObject(dataReturn);
            }
            dataReturn.Add("status", false);
            dataReturn.Add("seatData", bookList);
            return JsonConvert.SerializeObject(dataReturn);
        }

        /// <summary>
        /// 书籍预约功能的API
        /// </summary>
        /// <returns></returns>
        [Route("/reserve_book")]
        [HttpPost]
        public string reserveBook()
        {
            Dictionary<string, Object> dataReturn = new Dictionary<string, Object>();
            string readerId = HttpContext.Request.Form["readerId"];
            string bookId = HttpContext.Request.Form["bookId"];

            if (_readerModule.userCheck(readerId))
            {
                _readerModule.ReserveBook(readerId, bookId);
                dataReturn.Add("status", true);
                return JsonConvert.SerializeObject(dataReturn);
            }

            dataReturn.Add("status", false);
            return JsonConvert.SerializeObject(dataReturn);
        }

        /// <summary>
        /// 读者留言功能的API
        /// </summary>
        /// <returns></returns>
        [Route("/user_comment")]
        [HttpPost]
        public string userComment()
        {
            Dictionary<string, Object> dataReturn = new Dictionary<string, Object>();
            string readerId = HttpContext.Request.Form["readerId"];
            string bookId = HttpContext.Request.Form["bookId"];
            string content = HttpContext.Request.Form["content"];

            if (_readerModule.userCheck(readerId))
            {
                _readerModule.ReaderComment(readerId, bookId, content);
                dataReturn.Add("status", true);
                return JsonConvert.SerializeObject(dataReturn);
            }

            dataReturn.Add("status", false);
            return JsonConvert.SerializeObject(dataReturn);
        }

        /// <summary>
        /// 用户(读者)退出的API
        /// </summary>
        [Route("/user_logout")]
        [HttpGet]
        public void userLogout()
        {
            string phone = HttpContext.Request.Query["phone"];
            _readerModule.userLogout(phone);
        }

        /// <summary>
        /// 用户修改密码功能的API
        /// </summary>
        /// <returns></returns>
        [Route("/user_alter_password")]
        [HttpPost]
        public string UserAlterPassword()
        {
            Dictionary<string, Object> dataReturn = new Dictionary<string, Object>();
            string readerId = HttpContext.Request.Form["readerId"];
            string oldPwd = HttpContext.Request.Form["oldpwd"];
            string newPwd = HttpContext.Request.Form["newpwd"];

            if (_readerModule.userCheck(readerId))
            {
                _readerModule.AlterPassword(readerId, oldPwd, newPwd);
                dataReturn.Add("status", true);
                return JsonConvert.SerializeObject(dataReturn);
            }

            dataReturn.Add("status", false);
            return JsonConvert.SerializeObject(dataReturn);
        }
    }
}
