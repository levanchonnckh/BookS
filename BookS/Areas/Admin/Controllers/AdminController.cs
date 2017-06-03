using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookS.Models;
using PagedList;
using System.IO;
using BookS.Areas.admin.Models;

namespace BookS.Areas.admin.Controllers
{
    public class AdminController : Controller
    {

        DataClasses1DataContext db = new DataClasses1DataContext();
        // GET: admin/Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection f)
        {
            var tenDn = f["email"];
            var mk = f["password"];
            if (String.IsNullOrEmpty(tenDn) || String.IsNullOrEmpty(mk))
            {
                ViewData["loi"] = "Email hoặc mất khẩu không đúng";
            }
            else
            {
                ADMIN ad = db.ADMINs.SingleOrDefault(n => n.USER_ADMIN == tenDn && n.PASSWORD == mk);
                if (ad != null)
                {
                    Session["email"] = tenDn;
                    return RedirectToAction("admin_home","Admin");
                }
            }
            return this.Index();
        }

        public ActionResult admin_home()
        {
            if (!string.IsNullOrEmpty(Session["email"] as string))
            {
                return RedirectToAction("device","Admin");
            }
            else
            {
                ViewData["loi"] = "bạn không có quyền truy cập";
            }

            return Content("bạn không có quyền truy cập");
        }

        public ActionResult device(int? page)
        {
            int pageSize = 5;
            int pageNum = (page ?? 1);


            var deviceL = db.DEVICEs.ToList().OrderBy(n=>n.MaDevice);

            return View(deviceL.ToPagedList(pageNum, pageSize));
            
        }

        [HttpGet]
        public ActionResult CreatNew()
        {
            ViewBag.MaCD = new SelectList(db.CHU_DEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");
            ViewBag.MaNSX = new SelectList(db.NHA_SAN_XUATs.ToList().OrderBy(n => n.TenNSX), "MaNSX", "TenNSX");
            return View();
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreatNew(DEVICE dev,HttpPostedFileBase fileUpload)
        {
            

            ViewBag.MaCD = new SelectList(db.CHU_DEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");
            ViewBag.MaNSX = new SelectList(db.NHA_SAN_XUATs.ToList().OrderBy(n => n.TenNSX), "MaNSX", "TenNSX");

            if (fileUpload == null)
            {
                ViewBag.thongbao = "vui lòng chọn hình ảnh";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    //luu ten file
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    //luu duong dan file
                    var path = Path.Combine(Server.MapPath("~/image/DEVICE"), fileName);
                    //kiem tra file ton tai chu
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.thongbao = "hình ảnh đã tồn tại";
                    }
                    else
                    {
                        fileUpload.SaveAs(path);
                    }

                    dev.AnhBia = fileName;
                    db.DEVICEs.InsertOnSubmit(dev);
                    db.SubmitChanges();
                }

                return View();
            }


           
        }

        

        public ActionResult Detail(int id)
        {
            deviceDetail devD = new deviceDetail();
            devD.sheet = new List<Sheet1_>();
            
            devD.detailS = new Sheet1_();
            devD.detailD = new DEVICE();
            //get color
            var dev = db.Sheet1_s.ToList();
            foreach(var i in dev)
            {
                if (i.MaDev == id)
                {
                    //
                    devD.sheet.Add(i);
                }
            }

           

            devD.detailD = db.DEVICEs.SingleOrDefault(n=>n.MaDevice == id);
            var temp = db.Sheet1_s.ToList();

            foreach(var i in temp)
            {
                if (i.MaDev == id)
                {
                    devD.detailS = i;
                    break;
                }
            }


            if (devD == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            return View(devD);
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            DEVICE dev = db.DEVICEs.SingleOrDefault(n => n.MaDevice == id);
            if (dev == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.DEVICEs.DeleteOnSubmit(dev);
            db.SubmitChanges();
            return RedirectToAction("device");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            DEVICE dev = db.DEVICEs.SingleOrDefault(n => n.MaDevice == id);
            if (dev == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaCD = new SelectList(db.CHU_DEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");
            ViewBag.MaNSX = new SelectList(db.NHA_SAN_XUATs.ToList().OrderBy(n => n.TenNSX), "MaNSX", "TenNSX");
            return View(dev);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(DEVICE dev, HttpPostedFileBase fileUpload)
        {

            ViewBag.MaCD = new SelectList(db.CHU_DEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");
            ViewBag.MaNSX = new SelectList(db.NHA_SAN_XUATs.ToList().OrderBy(n => n.TenNSX), "MaNSX", "TenNSX");
            DEVICE devS = db.DEVICEs.Single(n => n.MaDevice == dev.MaDevice);

            if (ModelState.IsValid)
            {
                if (fileUpload != null)
                {
                    //luu ten file
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    //luu duong dan file
                    var path = Path.Combine(Server.MapPath("~/image/DEVICE"), fileName);
                    //kiem tra file ton tai chu
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.thongbao = "hình ảnh đã tồn tại";
                    }
                    else
                    {
                        fileUpload.SaveAs(path);
                    }
                    dev.AnhBia = fileName;
                }
                else
                {
                    dev.AnhBia = devS.AnhBia;
                }
                

                
                
                db.DEVICEs.DeleteOnSubmit(devS);
                db.DEVICEs.InsertOnSubmit(dev);
                db.SubmitChanges();
            }

            return RedirectToAction("device");
        }

        public ActionResult user()
        {
            var kh = db.KHACH_HANGs.ToList();
            return View(kh);
        }

        //user
        [HttpGet]
        public ActionResult DeleteUser(int id)
        {
            KHACH_HANG dev = db.KHACH_HANGs.SingleOrDefault(n => n.MaKH == id);
            if (dev == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.KHACH_HANGs.DeleteOnSubmit(dev);
            db.SubmitChanges();
            return RedirectToAction("user");
        }
        //

        public ActionResult donHang()
        {
            
            var dh = db.DON_DAT_HANGs.ToList();
            return View(dh);
        }

        [HttpGet]
        public ActionResult deleteDonHang(int id)
        {
            CT_DON_HANG dev = db.CT_DON_HANGs.SingleOrDefault(n => n.MaDH == id);
            DON_DAT_HANG don = db.DON_DAT_HANGs.SingleOrDefault(n => n.MaDH == id);
            if (dev == null||don==null)
            {
                Response.StatusCode = 404;
                return null;
            }

            try
            {
                db.CT_DON_HANGs.DeleteOnSubmit(dev);
                db.DON_DAT_HANGs.DeleteOnSubmit(don);
            }
            catch ( Exception e)
            {
                Response.Write(e.Message);
            }
            





            db.SubmitChanges();
            return RedirectToAction("donHang");
        }

        public ActionResult chitiet()
        {
            var chitiet = db.DON_DAT_HANGs.SingleOrDefault(n=>n.MaDH==14);
            return View(chitiet);
        }

       


    }
}