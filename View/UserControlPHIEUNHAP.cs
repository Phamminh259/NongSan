using NongSanXanh.Controller;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using NongSanXanh.Models;

namespace NongSanXanh.View
{
    public partial class UserControlPHIEUNHAP : UserControl, IView
    {
        private readonly PHIEUNHAPController _controller;

        public event EventHandler DataChanged;

        public UserControlPHIEUNHAP()
        {
            InitializeComponent();
            SetupDataGridView();
            _controller = new PHIEUNHAPController(this);
            LoadData();
            btnSave.Enabled = false;
            btnDelete.Enabled = false;
            txtSoPhieu.TextChanged += TxtSoPhieu_TextChanged;
            txtMaNCC.TextChanged += TxtMaNCC_TextChanged;
            dtpNgayNhap.ValueChanged += DtpNgayNhap_ValueChanged;
            
        }
        private void SetupDataGridView()
        {
            // Xóa tất cả cột hiện tại (nếu có)
            dataGridView.Columns.Clear();

            // Thêm cột cho DataGridView
            dataGridView.Columns.Add("SoPhieu", "Số Phiếu");
            dataGridView.Columns.Add("MaNCC", "Mã NCC");
            dataGridView.Columns.Add("NgayNhap", "Ngày Nhập");
        }


        private void TxtSoPhieu_TextChanged(object sender, EventArgs e)
        {
            // Gọi phương thức UpdateView để cập nhật trạng thái nút Save
            UpdateView();
        }
        private void TxtMaNCC_TextChanged(object sender, EventArgs e)
        {
            UpdateView();
        }

        // Phương thức để xử lý sự kiện ValueChanged cho dtpNgayNhap
        private void DtpNgayNhap_ValueChanged(object sender, EventArgs e)
        {
            UpdateView();
        }
      

        private void LoadData()
        {
            var phieuNhaps = _controller.Items.OfType<PhieuNhap>().ToList();

            // Chuyển đổi List<PhieuNhap> thành List<IModel>
            List<IModel> items = phieuNhaps.Cast<IModel>().ToList();

            UpdateDataGridView(items); //
            //_controller.Load();
        }

        public void UpdateView()
        {
            btnSave.Enabled = ValidateInputs();
            btnDelete.Enabled = !string.IsNullOrEmpty(txtSoPhieu.Text);
        }


        private bool ValidateInputs()
        {
            return !string.IsNullOrEmpty(txtSoPhieu.Text) &&
           !string.IsNullOrEmpty(txtMaNCC.Text) &&
           dtpNgayNhap.Value <= DateTime.Now; // Thêm kiểm tra cho ngày nhập nếu cần
        }
        private void dataGridViewPhieunhap_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm tra nếu hàng được nhấp là hợp lệ
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView.Rows[e.RowIndex];

                // Tạo một đối tượng PhieuNhap từ dữ liệu của hàng được chọn
                var phieuNhap = new PhieuNhap
                {
                    SoPhieu = Convert.ToInt32(row.Cells["SoPhieu"].Value),
                    MaNCC = Convert.ToInt32(row.Cells["MaNCC"].Value),
                    NgayNhap = row.Cells["NgayNhap"].Value as DateTime? ?? DateTime.Now // Xử lý giá trị nullable
                };

                // Gọi SetDataToText để cập nhật các điều khiển
                SetDataToText(phieuNhap);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var phieuNhap = GetDataFromText();

            // Kiểm tra nếu dữ liệu nhập vào là hợp lệ
            if (phieuNhap == null)
            {
                MessageBox.Show("Số phiếu và Mã NCC phải là số hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Chuyển đổi phieuNhap về kiểu cụ thể
            var phieuNhapModel = phieuNhap as PhieuNhap; // Thực hiện phép chuyển đổi

            if (phieuNhapModel == null)
            {
                MessageBox.Show("Dữ liệu không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Kiểm tra nếu mã NCC tồn tại trong cơ sở dữ liệu
            if (!_controller.IsValidMaNCC(phieuNhapModel.MaNCC))
            {
                MessageBox.Show("Mã Nhà Cung Cấp không đúng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Kiểm tra xem mục đã tồn tại trong cơ sở dữ liệu chưa
            if (_controller.IsExist(phieuNhapModel.SoPhieu))
            {
                MessageBox.Show("Số phiếu đã tồn tại. Vui lòng nhập số phiếu khác.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                _controller.Create(phieuNhapModel);
                MessageBox.Show("Thêm phiếu nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView.CurrentRow != null) // Kiểm tra nếu có dòng hiện tại
            {
                // Lấy số phiếu từ dòng hiện tại
                int soPhieu = Convert.ToInt32(dataGridView.CurrentRow.Cells["SoPhieu"].Value);

                // Gọi phương thức xóa
                if (_controller.Delete(soPhieu)) // Truyền số phiếu vào
                {
                    MessageBox.Show("Xóa phiếu nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData(); // Cập nhật lại DataGridView

                    // Xóa dữ liệu trên các TextBox
                    ClearTextBoxes(); // Gọi phương thức để xóa dữ liệu
                }
                else
                {
                    MessageBox.Show("Đã xảy ra lỗi!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn phiếu nhập để xóa.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ClearTextBoxes()
        {
            txtSoPhieu.Clear();
            dtpNgayNhap.Value = DateTime.Now; // Đặt lại giá trị của DateTimePicker về ngày hiện tại
            txtMaNCC.Clear();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

       
        public void UpdateDataGridView(List<IModel> items)
        {
            dataGridView.Rows.Clear();

            // Thêm dữ liệu mới vào DataGridView
            foreach (var item in items)
            {
                var model = item as PhieuNhap; // Ép kiểu
                if (model != null)
                {
                    // Thêm dòng dữ liệu vào DataGridView
                    dataGridView.Rows.Add(model.SoPhieu, model.MaNCC, model.NgayNhap);
                }
            }
        }

        public void SetDataToText(object item)
        {
            if (item is PhieuNhap phieuNhap)
            {
                txtSoPhieu.Text = phieuNhap.SoPhieu.ToString();
                txtMaNCC.Text = phieuNhap.MaNCC.ToString();

                // Kiểm tra nếu NgayNhap có giá trị, nếu không thì gán giá trị mặc định
                dtpNgayNhap.Value = phieuNhap.NgayNhap ?? DateTime.Now; // Hoặc gán một giá trị mặc định khác
            }
            else
            {
                throw new ArgumentException("Invalid item type");
            }
        }

        public object GetDataFromText()
        {
            // Try to parse the input values and return a new PhieuNhap object
            if (int.TryParse(txtSoPhieu.Text, out int soPhieu) && int.TryParse(txtMaNCC.Text, out int maNCC))
            {
                return new PhieuNhap
                {
                    SoPhieu = soPhieu,
                    MaNCC = maNCC,
                    NgayNhap = dtpNgayNhap.Value
                };
            }
            return null; // Return null if parsing fails
        }

        private void UserControlPHIEUNHAP_Load(object sender, EventArgs e)
        {

        }
    }
}
