using BookS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookS.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        DataClasses1DataContext _data = new DataClasses1DataContext();
        // GET: Admin/Home
        
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(FormCollection fie)
        {
            string user = Request.Form["userName"];
            string pass = Request.Form["passWord"];
            var _user = (from c in _data.ADMINs where user == c.USER_NAME select c);
            if ((user.Equals(_user.First().USER_NAME)&&user.Equals(_user.First().PASSWORD))==true)
            {
                //neu dung tai khoan
                return RedirectToAction("mBooks");
            }
                return this.Index();
        }


        public ActionResult mBooks()
        {

            var lSach = LaySach(8);
            return View(lSach);
        }

        public List<SACH> LaySach(int count)
        {
            return _data.SACHes.Take(count).ToList();
        }




    }
}