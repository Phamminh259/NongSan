using NongSanXanh.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NongSanXanh.Controller
{
    public interface IController
    {
         List<IModel> Items { get; }

        // Thay đổi phương thức Add thành Create
        bool Create(IModel model);

        // Phương thức Update giữ nguyên
        bool Update(IModel model);

        // Thay đổi phương thức Delete để nhận id
        bool Delete(object id);

        // Thay đổi phương thức Load để trả về IModel
        IModel Read(object id);

        // Phương thức Load được cập nhật
        bool Load();

        // Phương thức Load nhận tham số id
        bool Load(object id);

        // Kiểm tra sự tồn tại của id
        bool IsExist(object id);

        // Kiểm tra sự tồn tại của model
        bool IsExist(IModel model);
    }
}
