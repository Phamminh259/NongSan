using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NongSanXanh.Models
{
    public class ChiTietPhieuNhap : IModel
    {
        [Key]
        public int IDPN { get; set; }
        public int SoPhieu { get; set; }
        public int MaHangHoa { get; set; }
        public int SoLuongNhap { get; set; }
        public DateTime? NgaySanXuat { get; set; }
        public decimal? GiaNhap { get; set; }
        //public int MaNCC { get; set; }
        public DateTime? HangSuDung { get; set; }

        [ForeignKey("SoPhieu")]
        public virtual PhieuNhap PhieuNhap { get; set; }  // Giả sử bạn có lớp PhieuNhap để ánh xạ mối quan hệ
    }
}



