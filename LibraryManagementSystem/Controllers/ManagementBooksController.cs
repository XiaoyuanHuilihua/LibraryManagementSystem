using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Controllers
{
    public class ManagementBooksController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
