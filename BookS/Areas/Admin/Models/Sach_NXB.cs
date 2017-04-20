using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookS.Models;


namespace BookS.Areas.Admin.Models
{
    public class Sach_NXB
    {
        public SACH sach { get; set; }
        public string ten_NXB { get; set; }
        public Sach_NXB(SACH sach_temp,string ten_NXB_temp)
        {
            this.sach = sach_temp;
            this.ten_NXB = ten_NXB_temp;
        }  
    }
}