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
        还书受理功能：OK
        借书受理功能：OK
        图书录入功能：OK
        挂失受理功能：OK
        图书注销功能：OK
        图书损坏处理功能：OK
        图书检索功能： - (用js来实现)
        + 查看所有图书功能：UserContraller
        预约超期处理功能：
        借书超期处理功能：
        超期受理功能：
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

            if (_administratorModule.AdminCheck(adminId))
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

            if (_administratorModule.AdminCheck(adminId))
            {
                _managementBooksModule.BorrowAccep(readerId, bookId);
                dataReturn.Add("status", true);
                return JsonConvert.SerializeObject(dataReturn);
            }

            dataReturn.Add("status", false);
            return JsonConvert.SerializeObject(dataReturn);
        }

        /// <summary>
        /// 图书录入功能的API
        /// </summary>
        /// <returns></returns>
        [Route("/add_book")]
        [HttpPost]
        public string AddBook()
        {
            Dictionary<string, Object> dataReturn = new Dictionary<string, Object>();
            string adminId = HttpContext.Request.Form["adminId"];
            string bookId = HttpContext.Request.Form["bookId"];
            string bookName = HttpContext.Request.Form["bookName"];
            string isbn = HttpContext.Request.Form["ISBN"];
            string author = HttpContext.Request.Form["author"];
            string publisher = HttpContext.Request.Form["publisher"];
            DateTime publishDate = Convert.ToDateTime(HttpContext.Request.Form["publishDate"]);
            string detail = HttpContext.Request.Form["detail"];
            int price = Convert.ToInt32(HttpContext.Request.Form["price"]);

            if (_administratorModule.AdminCheck(adminId))
            {
                _managementBooksModule.ResisterBook(bookId, bookName, isbn, author, publisher, publishDate, detail, price);
                dataReturn.Add("status", true);
                return JsonConvert.SerializeObject(dataReturn);
            }

            dataReturn.Add("status", false);
            return JsonConvert.SerializeObject(dataReturn);
        }

        /// <summary>
        /// 挂失受理功能的API
        /// </summary>
        /// <returns></returns>
        [Route("/report_loss")]
        [HttpPost]
        public string ReportLoss()
        {
            Dictionary<string, Object> dataReturn = new Dictionary<string, Object>();
            string adminId = HttpContext.Request.Form["adminId"];
            string bookId = HttpContext.Request.Form["bookId"];
            string readerId = HttpContext.Request.Form["readerId"];
            string fine = HttpContext.Request.Form["fine"];

            if (_administratorModule.AdminCheck(adminId))
            {
                _managementBooksModule.LossAccep(bookId, readerId);
                dataReturn.Add("status", true);
                return JsonConvert.SerializeObject(dataReturn);
            }

            dataReturn.Add("status", false);
            return JsonConvert.SerializeObject(dataReturn);
        }

        /// <summary>
        /// 图书注销功能的API
        /// </summary>
        /// <returns></returns>
        [Route("/delete_book")]
        [HttpPost]
        public string DeleteBook()
        {
            Dictionary<string, Object> dataReturn = new Dictionary<string, Object>();
            string adminId = HttpContext.Request.Form["adminId"];
            string bookId = HttpContext.Request.Form["bookId"];
            string reason = HttpContext.Request.Form["reason"];

            if (_administratorModule.AdminCheck(adminId))
            {
                _managementBooksModule.BookCancellation(bookId, reason);
                dataReturn.Add("status", true);
                return JsonConvert.SerializeObject(dataReturn);
            }

            dataReturn.Add("status", false);
            return JsonConvert.SerializeObject(dataReturn);
        }

        /// <summary>
        /// 预约超期处理功能的API
        /// </summary>
        [Route("/report_damage")]
        [HttpGet]
        public string ReportDamage()
        {
            Dictionary<string, Object> dataReturn = new Dictionary<string, Object>();
            string adminId = HttpContext.Request.Form["adminId"];
            string readerId = HttpContext.Request.Form["readerId"];
            string bookId = HttpContext.Request.Form["bookId"];
            string fine = HttpContext.Request.Form["fine"];
            string reason = HttpContext.Request.Form["reason"];

            if (_administratorModule.AdminCheck(adminId))
            {
                _managementBooksModule.DealWithDamagedBooks(bookId, readerId, reason);
                dataReturn.Add("status", true);
                return JsonConvert.SerializeObject(dataReturn);
            }

            dataReturn.Add("status", false);
            return JsonConvert.SerializeObject(dataReturn);
        }
    }
}
