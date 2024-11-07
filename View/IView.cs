using NongSanXanh.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NongSanXanh.View
{
    public interface IView
    {
        // Phương thức để cập nhật giao diện dựa trên dữ liệu hiện tại
        //void UpdateView();

        // Sự kiện thông báo khi dữ liệu thay đổi
        //event EventHandler DataChanged;

        // Đặt dữ liệu vào giao diện từ một đối tượng
        void SetDataToText(object item);

        // Lấy dữ liệu từ giao diện và trả về đối tượng chứa dữ liệu
        object GetDataFromText();
    }
}
