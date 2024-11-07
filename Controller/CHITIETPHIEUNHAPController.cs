using NongSanXanh.Models;
using NongSanXanh.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration; // Thêm không gian tên này
using NongSanXanh; // Nếu cần thiết

namespace NongSanXanh.Controller
{
    public class CHITIETPHIEUNHAPController : IController
    {
        private List<IModel> _items = new List<IModel>();
        private UserControlCHITIETPHIEUNHAP _view;
        private MyDatabase _context;

        public List<IModel> Items => _items;

        public CHITIETPHIEUNHAPController(UserControlCHITIETPHIEUNHAP view)
        {
            _view = view;
            string connectionString = ConfigurationManager.ConnectionStrings["MyDatabase"].ConnectionString;
            _context = new MyDatabase(); // Sử dụng chuỗi kết nối
            Load();
        }

        public bool Load()
        {
            try
            {
                _items = _context.ChiTietPhieuNhaps.ToList<IModel>(); // Giả sử bạn có DbSet cho CHITIETPHIEUNHAP
                Console.WriteLine($"Loaded {_items.Count} items.");
                _view.UpdateDataGridView(_items); // Gọi phương thức cập nhật
                return true; // Thành công
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading data: {ex.Message}");
                return false; // Thất bại
            }
        }

        public bool Create(IModel model)
        {
            try
            {
                var newCTPN = (ChiTietPhieuNhap)model;
                _context.ChiTietPhieuNhaps.Add(newCTPN);
                _context.SaveChanges();
                Load();
                return true; // Thành công
            }
            catch
            {
                return false; // Thất bại
            }
        }

        public bool Update(IModel model)
        {
            try
            {
                var existingCTPN = (ChiTietPhieuNhap)model;
                var item = _context.ChiTietPhieuNhaps.Find(existingCTPN.IDPN);
                if (item != null)
                {
                    item.SoPhieu = existingCTPN.SoPhieu;
                    item.MaHangHoa = existingCTPN.MaHangHoa;
                    item.SoLuongNhap = existingCTPN.SoLuongNhap;
                    item.NgaySanXuat = existingCTPN.NgaySanXuat;
                    item.GiaNhap = existingCTPN.GiaNhap;
                    item.HangSuDung = existingCTPN.HangSuDung;
                    _context.SaveChanges();
                    Load();
                    return true; // Thành công
                }
                return false; // Không tìm thấy mục
            }
            catch
            {
                return false; // Thất bại
            }
        }

        public bool Delete(object id)
        {
            try
            {
                var item = _context.ChiTietPhieuNhaps.Find(id);
                if (item != null)
                {
                    _context.ChiTietPhieuNhaps.Remove(item);
                    _context.SaveChanges();
                    Load(); // Make sure this method updates any necessary data if needed
                    return true; // Success
                }
                return false; // Item not found
            }
            catch
            {
                return false; // Failure
            }
        }

        public IModel Read(object id)
        {
            return _context.ChiTietPhieuNhaps.Find(id) as IModel; // Trả về IModel
        }

        public bool IsExist(object id)
        {
            return _context.ChiTietPhieuNhaps.Any(ctpn => ctpn.IDPN == (int)id);
        }

        public bool IsSoPhieuValid(int soPhieu)
        {
            // Kiểm tra xem số phiếu có tồn tại trong cơ sở dữ liệu hay không
            using (var context = new MyDatabase())
            {
                return context.PhieuNhaps.Any(p => p.SoPhieu == soPhieu);
            }
        }

        public bool Load(object id)
        {
            throw new NotImplementedException();
        }

        public bool IsExist(IModel model)
        {
            throw new NotImplementedException();
        }
    }
}
