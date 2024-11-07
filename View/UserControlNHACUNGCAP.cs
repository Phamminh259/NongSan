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
using NongSanXanh.Controller;
using NongSanXanh.Models;


namespace NongSanXanh.View
{
    public partial class UserControlNHACUNGCAP : UserControl, IView
    {
        private readonly NHACHUNGCAPController _controller;

        public event EventHandler DataChanged;

        public UserControlNHACUNGCAP()
        {
            InitializeComponent();
            _controller = new NHACHUNGCAPController(this);
            SetupDataGridViewColumns(); // Thiết lập cột một lần
            LoadData();
            btnSave.Enabled = false;
            btnDelete.Enabled = false;
            // Gán sự kiện cho các TextBox
            txtTenNCC.TextChanged += (s, e) => UpdateView();
            txtDienThoai.TextChanged += (s, e) => UpdateView();
            txtDiaChi1.TextChanged += (s, e) => UpdateView();
            txtSTK.TextChanged += (s, e) => UpdateView();
            txtTenNH.TextChanged += (s, e) => UpdateView();
        }
       
            private void LoadData()
            {
            var items = _controller.Items.OfType<NhaCungCap>().Select(ncc => (IModel)ncc).ToList();
            UpdateDataGridView(items);
        }


            private bool ValidateInputs()
            {
            return !string.IsNullOrEmpty(txtTenNCC.Text) && !string.IsNullOrEmpty(txtDienThoai.Text);
        }


            private void dataGridViewNhacungcap_CellContentClick(object sender, DataGridViewCellEventArgs e)
            {
            if (e.RowIndex >= 0) // Ensure the selected row is valid
            {
                DataGridViewRow row = this.dataGridView.Rows[e.RowIndex];

                // Fill the textboxes with the selected row's data
                txtMaNCC.Text = row.Cells["maNCC"].Value?.ToString() ?? string.Empty;
                txtTenNCC.Text = row.Cells["tenNCC"].Value?.ToString() ?? string.Empty;
                txtDiaChi1.Text = row.Cells["diaChi"].Value?.ToString() ?? string.Empty;
                txtDienThoai.Text = row.Cells["dienThoai"].Value?.ToString() ?? string.Empty;
                txtSTK.Text = row.Cells["STK"].Value?.ToString() ?? string.Empty;
                txtTenNH.Text = row.Cells["tenNH"].Value?.ToString() ?? string.Empty;

                // Update the view to enable or disable buttons accordingly
                UpdateView();
            }
        }

            private void btnSave_Click(object sender, EventArgs e)
            {
            var nhaCungCap = GetDataFromText();

            if (_controller.IsExist(nhaCungCap.MaNCC))
            {
                // Nếu nhà cung cấp đã tồn tại, cập nhật
                if (_controller.Update(nhaCungCap))
                {
                    MessageBox.Show("Cập nhật nhà cung cấp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Có lỗi xảy ra khi cập nhật nhà cung cấp. Vui lòng thử lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                // Nếu nhà cung cấp chưa tồn tại, tạo mới
                if (_controller.Create(nhaCungCap)) // Giả định rằng phương thức Create trả về true/false
                {
                    MessageBox.Show("Thêm nhà cung cấp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Có lỗi xảy ra khi thêm nhà cung cấp. Vui lòng thử lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            DataChanged?.Invoke(this, EventArgs.Empty);
            LoadData();

        }

            private void btnDelete_Click(object sender, EventArgs e)
            {
            if (int.TryParse(txtMaNCC.Text, out int maNCC))
            {
                // Use the ID directly, ensure that maNCC is the correct identifier
                if (_controller.Delete(maNCC)) // Pass the ID directly
                {
                    MessageBox.Show("Xóa nhà cung cấp thành công!"); // Notify success
                    DataChanged?.Invoke(this, EventArgs.Empty);
                    LoadData(); // Reload the data

                    // Clear the textboxes after successful deletion
                    ClearTextBoxes();
                }
                else
                {
                    MessageBox.Show("Không tìm thấy nhà cung cấp để xóa!"); // Notify failure
                }
            }
            else
            {
                MessageBox.Show("Mã NCC không hợp lệ!"); // Invalid ID input
            }
        }
        private void ClearTextBoxes()
        {
            txtMaNCC.Clear();
            txtTenNCC.Clear();
            txtDiaChi1.Clear();
            txtDienThoai.Clear();
            txtSTK.Clear();
            txtTenNH.Clear();
        }

        private void btnClose_Click(object sender, EventArgs e)
            {
                this.Visible = false;
            }
            private bool ValidateInput()
            {
            return !string.IsNullOrEmpty(txtTenNCC.Text) &&
        !string.IsNullOrEmpty(txtDienThoai.Text) &&
        !string.IsNullOrEmpty(txtDiaChi1.Text) &&
        !string.IsNullOrEmpty(txtSTK.Text) &&
        !string.IsNullOrEmpty(txtTenNH.Text);
        }
        private void SetupDataGridViewColumns()
        {
            // Xóa tất cả cột cũ (nếu có)
            // Clear existing columns if any
            dataGridView.Columns.Clear();

            // Add new columns
            dataGridView.Columns.Add("maNCC", "Mã NCC");
            dataGridView.Columns.Add("tenNCC", "Tên NCC");
            dataGridView.Columns.Add("diaChi", "Địa Chỉ");
            dataGridView.Columns.Add("dienThoai", "Điện Thoại");
            dataGridView.Columns.Add("STK", "Số Tài Khoản");
            dataGridView.Columns.Add("tenNH", "Tên Ngân Hàng");
        }



        public void UpdateDataGridView(List<IModel> items)
        {
            // Thiết lập cột chỉ một lần nếu chưa có
            if (dataGridView.Columns.Count == 0)
            {
                SetupDataGridViewColumns();
            }

            // Gỡ ràng buộc DataSource
            dataGridView.DataSource = null;

            // Xóa các hàng cũ
            dataGridView.Rows.Clear();

            // Thêm dữ liệu mới vào DataGridView
            foreach (var item in items)
            {
                var model = item as NhaCungCap; // Ép kiểu
                if (model != null)
                {
                    dataGridView.Rows.Add(model.MaNCC, model.TenNCC, model.DiaChi, model.DienThoai, model.STK, model.TenNH);
                }
            }

        }

        public void UpdateView()
        {
            btnSave.Enabled = ValidateInputs();
            btnDelete.Enabled = !string.IsNullOrEmpty(txtMaNCC.Text);
        }

        private void txtMaNCC_TextChanged(object sender, EventArgs e)
        {
            UpdateView();
        }

        private void txtTenNCC_TextChanged(object sender, EventArgs e)
        {
            UpdateView();
        }

        private void txtDiaChi1_TextChanged(object sender, EventArgs e)
        {
            UpdateView();
        }

        private void txtDienThoai_TextChanged(object sender, EventArgs e)
        {
            UpdateView();
        }

        private void txtSTK_TextChanged(object sender, EventArgs e)
        {
            UpdateView();
        }

        private void txtTenNH_TextChanged(object sender, EventArgs e)
        {
            UpdateView();
        }

        public void SetDataToText(object item)
        {
            if (item is NhaCungCap nhaCungCap)
            {
                txtMaNCC.Text = nhaCungCap.MaNCC.ToString();
                txtTenNCC.Text = nhaCungCap.TenNCC;
                txtDiaChi1.Text = nhaCungCap.DiaChi;
                txtDienThoai.Text = nhaCungCap.DienThoai;
                txtSTK.Text = nhaCungCap.STK;
                txtTenNH.Text = nhaCungCap.TenNH;
            }
            else
            {
                throw new ArgumentException("Item must be of type NhaCungCap", nameof(item));
            }
        }

        public NhaCungCap GetDataFromText()
        {
            return new NhaCungCap
            {
                MaNCC = int.TryParse(txtMaNCC.Text, out int maNCC) ? maNCC : 0,
                TenNCC = txtTenNCC.Text,
                DiaChi = txtDiaChi1.Text,
                DienThoai = txtDienThoai.Text,
                STK = txtSTK.Text,
                TenNH = txtTenNH.Text
            };
        }

        object IView.GetDataFromText()
        {
            return GetDataFromText();
        }
    }
}
