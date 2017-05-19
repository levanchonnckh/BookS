using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookS.Models;
using PagedList;


namespace BookS.Controllers
{
    public class NewsController : Controller
    {

        DataClasses1DataContext _data = new DataClasses1DataContext();
        // GET: News
        public ActionResult Index(int? page)
        {
            int pageSize = 5;
            int pageNum = (page ?? 1);


            var sachL = LaySach(7);

            return View(sachL.ToPagedList(pageNum, pageSize));
        }

        public List<SACH> LaySach(int count)
        {
            return _data.SACHes.Take(count).ToList();
        }
    }
}