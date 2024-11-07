using NongSanXanh.Models;
using NongSanXanh.View;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace NongSanXanh.Controller
{
    public class NHACHUNGCAPController : IController
    {
        private List<IModel> _items = new List<IModel>();
        private UserControlNHACUNGCAP _view;
        private MyDatabase _context;

        public List<IModel> Items => _items;

        public NHACHUNGCAPController(UserControlNHACUNGCAP view)
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
                _items = _context.NhaCungCaps.ToList<IModel>(); // Giả sử bạn có DbSet cho NHACUNGCAP
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
                var newNCC = (NhaCungCap)model;
                _context.NhaCungCaps.Add(newNCC);
                _context.SaveChanges();
                Load();
                return true; // Thành công
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding item: {ex.Message}");
                return false; // Thất bại
            }
        }

        public bool Update(IModel model)
        {
            try
            {
                var existingNCC = (NhaCungCap)model;
                var item = _context.NhaCungCaps.Find(existingNCC.MaNCC);
                if (item != null)
                {
                    item.TenNCC = existingNCC.TenNCC;
                    item.DiaChi = existingNCC.DiaChi;
                    item.DienThoai = existingNCC.DienThoai;
                    item.STK = existingNCC.STK;
                    item.TenNH = existingNCC.TenNH;
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
                var item = _context.NhaCungCaps.Find(id); // Ensure id is the correct type (should match the database ID type)
                if (item != null)
                {
                    _context.NhaCungCaps.Remove(item);
                    _context.SaveChanges();
                    Load(); // Reload data after deletion
                    return true; // Deletion successful
                }
                return false; // Item not found
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting item: {ex.Message}");
                return false; // Deletion failed
            }
        }

        public IModel Read(object id)
        {
            return _context.NhaCungCaps.Find(id) as IModel; // Trả về IModel
        }

        public bool IsExist(object id)
        {
            return _context.NhaCungCaps.Any(ncc => ncc.MaNCC == (int)id);
        }

        public bool IsExist(IModel model)
        {
            return IsExist(((NhaCungCap)model).MaNCC);
        }

        public bool IsValidMaNCC(int maNCC)
        {
            return _context.NhaCungCaps.Any(ncc => ncc.MaNCC == maNCC);
        }

        public bool Load(object id)
        {
            throw new NotImplementedException(); // Chưa triển khai
        }
    }
}
