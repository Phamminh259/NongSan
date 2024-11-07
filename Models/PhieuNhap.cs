using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NongSanXanh.Models
{
    // File: Models/PhieuNhap.cs
    public class PhieuNhap : IModel
    {
        [Key]
        public int SoPhieu { get; set; }
        public int MaNCC { get; set; }
        public DateTime? NgayNhap { get; set; }
    }
}
