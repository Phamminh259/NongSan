using NongSanXanh.Models;
using NongSanXanh.View;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace NongSanXanh.Controller
{
    public class PHIEUNHAPController : IController
    {
        private List<IModel> _items = new List<IModel>();
        private UserControlPHIEUNHAP _view;
        private MyDatabase _context;

        public List<IModel> Items => _items;

        public PHIEUNHAPController(UserControlPHIEUNHAP view)
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
                _items = _context.PhieuNhaps.ToList<IModel>();
                if (_items.Count == 0)
                {
                    Console.WriteLine("Không có dữ liệu để tải.");
                }
                _view.UpdateDataGridView(_items);
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
                var newPN = (PhieuNhap)model;
                if (!_context.PhieuNhaps.Any(p => p.SoPhieu == newPN.SoPhieu))
                {
                    _context.PhieuNhaps.Add(newPN);
                    _context.SaveChanges();
                    Load(); // Tải lại dữ liệu để cập nhật
                    return true; // Thành công
                }
                Console.WriteLine("Số phiếu đã tồn tại. Vui lòng nhập số phiếu khác.");
                return false; // Thất bại
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Có lỗi xảy ra khi thêm phiếu nhập: {ex.Message}");
                return false; // Thất bại
            }
        }

        public bool Update(IModel model)
        {
            try
            {
                var existingPN = (PhieuNhap)model;
                var item = _context.PhieuNhaps.Find(existingPN.SoPhieu);
                if (item != null)
                {
                    item.MaNCC = existingPN.MaNCC;
                    item.NgayNhap = existingPN.NgayNhap;
                    _context.SaveChanges();
                    Load();
                    return true; // Thành công
                }
                return false; // Không tìm thấy mục
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating item: {ex.Message}");
                return false; // Thất bại
            }
        }

        public bool Delete(object id)
        {
            try
            {
                var item = _context.PhieuNhaps.Find(id);
                if (item != null)
                {
                    _context.PhieuNhaps.Remove(item);
                    _context.SaveChanges();
                    Load(); // Nếu bạn có phương thức Load() để cập nhật danh sách
                    return true; // Thành công
                }
                return false; // Không tìm thấy mục
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting item: {ex.Message}");
                return false; // Thất bại
            }
        }

        public IModel Read(object id)
        {
            return _context.PhieuNhaps.Find(id) as IModel; // Trả về IModel
        }

        public bool IsExist(object id)
        {
            return _context.PhieuNhaps.Any(pn => pn.SoPhieu == (int)id);
        }

        public bool IsValidMaNCC(int maNCC)
        {
            return _context.NhaCungCaps.Any(ncc => ncc.MaNCC == maNCC);
        }

        public bool Load(object id)
        {
            try
            {
                // Tải tất cả các phiếu nhập từ cơ sở dữ liệu
                _items = _context.PhieuNhaps.ToList<IModel>();

                // Kiểm tra xem có dữ liệu hay không
                if (_items.Count == 0)
                {
                    Console.WriteLine("Không có dữ liệu để tải.");
                }

                // Gọi phương thức cập nhật DataGridView trong UserControl
                _view.UpdateDataGridView(_items);
                return true; // Thành công
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading data: {ex.Message}");
                return false; // Thất bại
            }
        }

        public bool IsExist(IModel model)
        {
            throw new NotImplementedException();
        }
    }
}
