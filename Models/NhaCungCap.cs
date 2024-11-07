using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NongSanXanh.Models
{
    public class NhaCungCap : IModel
    {
        [Key] // Đánh dấu thuộc tính này là Primary Key
        public int MaNCC { get; set; }

        [Required] // Bắt buộc
        [StringLength(100)] // Độ dài tối đa cho tên nhà cung cấp
        public string TenNCC { get; set; }

        [StringLength(200)] // Độ dài tối đa cho địa chỉ
        public string DiaChi { get; set; }

        [StringLength(15)] // Độ dài tối đa cho số điện thoại
        public string DienThoai { get; set; }

        [StringLength(20)] // Độ dài tối đa cho số tài khoản
        public string STK { get; set; }

        [StringLength(100)] // Độ dài tối đa cho tên ngân hàng
        public string TenNH { get; set; }
    }
}
