using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookS.Models;

namespace BookS.Controllers
{
    public class GiohangController : Controller
    {
        // GET: Giohang
        public ActionResult Index()
        {
            return View();
        }

        DataClasses1DataContext db = new DataClasses1DataContext();
        public List<Giohang> Laygiohang()
        {
            List<Giohang> lstGioHang = Session["Giohang"] as List<Giohang>;
            if(lstGioHang == null)
            {
                lstGioHang = new List<Giohang>();
                Session["Giohang"] = lstGioHang;
            }
            return lstGioHang;
        }

        public ActionResult ThemGiohang(int iMasach, string strURL)
        {
            List<Giohang> lstGiohang = Laygiohang();
            Giohang sanpham = lstGiohang.Find(n => n.iMasach == iMasach);
            if (sanpham == null)
            {
                sanpham = new Giohang(iMasach);
                lstGiohang.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                sanpham.iSoluong++;
                return Redirect(strURL);
            }
        }
        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<Giohang> lstGiohang = Session["Giohang"] as List<Giohang>;
            if (lstGiohang != null)
            {
                iTongSoLuong = lstGiohang.Sum(n => n.iSoluong);

            }
            return iTongSoLuong;
        }

        private double TongTien()
        {
            double dTongTien = 0;
            List<Giohang> lstGiohang = Session["Giohang"] as List<Giohang>;
            if (lstGiohang != null)
            {
                dTongTien = lstGiohang.Sum(n => n.dThanhtien);

            }
            return dTongTien;
        }

        public ActionResult GioHang()
        {
            List<Giohang> lstGiohang = Laygiohang();
            if (lstGiohang.Count == 0)
            {
                return RedirectToAction("Index", "BookS");

            }
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return View(lstGiohang);
        }

        public ActionResult GiohangPartial()
        {
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return PartialView();
        }

        public ActionResult XoaGiohang(int iMasp)
        {
            List<Giohang> lstGiohang = Laygiohang();
            Giohang sanpham = lstGiohang.SingleOrDefault(n => n.iMasach == iMasp);
            if (sanpham != null)
            {
                lstGiohang.RemoveAll(n => n.iMasach == iMasp);
            }
            if(lstGiohang.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("GioHang");
        }

        public ActionResult CapnhatGiohang(int iMasp, FormCollection f)
        {
            List<Giohang> lstGiohang = Laygiohang();
            Giohang sanpham = lstGiohang.SingleOrDefault(n => n.iMasach == iMasp);
            if (sanpham != null)
            {
                sanpham.iSoluong = int.Parse(f["txtSoluong"].ToString());
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult XoaAll()
        {
            List<Giohang> lstGiohang = Laygiohang();
            lstGiohang.Clear();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Dathang()
        {
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("Dangnhap", "NguoiDung");
            }

            if (Session["Giohang"] == null)
            {
                return RedirectToAction("index", "BookS");
            }
            List<Giohang> lstGiohang = Laygiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return View(lstGiohang);
        }

        [HttpPost]
        public ActionResult Dathang(FormCollection collection)
        {
            DON_DAT_HANG ddh = new DON_DAT_HANG();
            KHACH_HANG kh = (KHACH_HANG)Session["Taikhoan"];
            List<Giohang> gh = Laygiohang();
            ddh.MaKH = kh.MaKH;
            ddh.NgayDH = DateTime.Now;
            var ngaygiao = String.Format("{0:MM/dd/yyyy}", collection["Ngaygiao"]);
            ddh.Ngaygiao = DateTime.Parse(ngaygiao);
            ddh.TinhTrangGiaoHang = false;
            ddh.DaThanhToan = false;
            db.DON_DAT_HANGs.InsertOnSubmit(ddh);
            db.SubmitChanges();
            //Them chi tiet don hang
            foreach (var item in gh)
            {
                CT_DON_HANG ctdh = new CT_DON_HANG();
                int temp = db.DON_DAT_HANGs.ToList().Last().MaDH;
                ctdh.MaDH = ddh.MaDH;
                ctdh.Masach = item.iMasach;
                ctdh.Soluong = item.iSoluong;
                ctdh.Dongia = (decimal)item.dDongia;
                db.CT_DON_HANGs.InsertOnSubmit(ctdh);
              

            }
            db.SubmitChanges();
            Session["Giohang"] = null;
            return RedirectToAction("Xacnhandonhang", "Giohang");
        }

        public ActionResult Xacnhandonhang()
        {
            return View();
        }
    }
}