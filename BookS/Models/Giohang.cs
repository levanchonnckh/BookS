using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookS.Models;

namespace BookS.Models
{
    public class Giohang
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        public int iMasach { set; get; }
        public string sTensach { set; get; }
        public string sAnhbia { set; get; }
        public double dDongia { set; get; }
        public int iSoluong { set; get; }
        public Double dThanhtien
        {
            get { return iSoluong * dDongia; }
        }
        public Giohang(int Masach)
        {
            this.iMasach = Masach;
            SACH sach = db.SACHes.SingleOrDefault(n => n.MaSach == Masach);
            this.sTensach = sach.TenSach;
            this.sAnhbia = sach.Anhbia;
            this.dDongia = double.Parse(sach.GiaBan.ToString());
            this.iSoluong = 1;
        }
    }
}