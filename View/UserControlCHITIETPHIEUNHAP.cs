using NongSanXanh.Controller;
using NongSanXanh.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace NongSanXanh.View
{
    public partial class UserControlCHITIETPHIEUNHAP : UserControl, IView
    {
        private readonly CHITIETPHIEUNHAPController _controller;
        private BindingList<ChiTietPhieuNhap> _bindingList;
        
        public event EventHandler DataChanged;
        public UserControlCHITIETPHIEUNHAP()
        {
            InitializeComponent();
            _controller = new CHITIETPHIEUNHAPController(this);
            _bindingList = new BindingList<ChiTietPhieuNhap>();
            
           
            LoadData();
            btnSave.Enabled = false;
            btnDelete.Enabled = false;
           


        }
        private void InitializeDataGridView()
        {
            dataGridView.Columns.Clear();

            // Cấu hình cho DataGridView cũ
            dataGridView.Columns.Clear();
            dataGridView.Columns.Add("IDPN", "ID Phiếu Nhập");
            dataGridView.Columns.Add("SoPhieu", "Số Phiếu");
            dataGridView.Columns.Add("MaHangHoa", "Mã Hàng Hóa");
            dataGridView.Columns.Add("SoLuongNhap", "Số Lượng Nhập");
            dataGridView.Columns.Add("NgaySanXuat", "Ngày Sản Xuất");
            dataGridView.Columns.Add("GiaNhap", "Giá Nhập");
            dataGridView.Columns.Add("HangSuDung", "Hạn Sử Dụng");


        }

        private void InitializeDataGridViewNew()
        {
            dataGridViewNew.Columns.Clear();
            dataGridViewNew.Columns.Add("IDPN", "ID Phiếu Nhập");
            dataGridViewNew.Columns.Add("SoPhieu", "Số Phiếu");
            dataGridViewNew.Columns.Add("MaHangHoa", "Mã Hàng Hóa");
            dataGridViewNew.Columns.Add("SoLuongNhap", "Số Lượng Nhập");
            dataGridViewNew.Columns.Add("NgaySanXuat", "Ngày Sản Xuất");
            dataGridViewNew.Columns.Add("GiaNhap", "Giá Nhập");
            dataGridViewNew.Columns.Add("HangSuDung", "Hạn Sử Dụng");
        }
        private void LoadData()
        {
            //var items = _controller.Items.OfType<ChiTietPhieuNhap>().ToList();
            //_bindingList = new BindingList<ChiTietPhieuNhap>(items);
            //dataGridView.DataSource = _bindingList;
            //Lấy lại danh sách dữ liệu mới từ nguồn





            //var items = _controller.Items.OfType<ChiTietPhieuNhap>().ToList();

            //// Kiểm tra xem dữ liệu có thay đổi không
            //if (items.Count > 0)
            //{
            //    // Cập nhật lại BindingList nếu có thay đổi
            //    _bindingList = new BindingList<ChiTietPhieuNhap>(items);
            //    dataGridView.DataSource = _bindingList;
            //}
            //else
            //{
            //    MessageBox.Show("Không có dữ liệu mới để hiển thị", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //}



        }
        private void UpdateTongTien()
        {
            decimal tongTien = 0;
            foreach (DataGridViewRow row in dataGridViewNew.Rows)
            {
                if (row.Cells["GiaNhap"].Value != null && row.Cells["SoLuongNhap"].Value != null)
                {
                    decimal giaNhap = Convert.ToDecimal(row.Cells["GiaNhap"].Value);
                    int soLuongNhap = Convert.ToInt32(row.Cells["SoLuongNhap"].Value);
                    tongTien += giaNhap * soLuongNhap;
                }
            }
            txtTongTien.Text = tongTien.ToString("N2"); // Hiển thị tổng tiền
        }


        public void UpdateDataGridView(List<IModel> items)
        {
            if (items == null || items.Count == 0)
                return;

            if (_bindingList == null)
                _bindingList = new BindingList<ChiTietPhieuNhap>();

            _bindingList.Clear();
            foreach (var item in items.OfType<ChiTietPhieuNhap>())
                _bindingList.Add(item);

            dataGridView.DataSource = null;
            dataGridView.DataSource = _bindingList;

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            UpdateView();
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            UpdateView();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtSoPhieu.Text, out int soPhieu) || !_controller.IsSoPhieuValid(soPhieu))
            {
                MessageBox.Show("Số phiếu không hợp lệ. Vui lòng kiểm tra lại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var chiTiet = (ChiTietPhieuNhap)GetDataFromText();

            if (_controller.IsExist(chiTiet.IDPN))
            {
                // Cập nhật nếu mục đã tồn tại
                if (_controller.Update(chiTiet))
                {
                    MessageBox.Show("Cập nhật chi tiết phiếu nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Có lỗi xảy ra khi cập nhật chi tiết phiếu nhập. Vui lòng thử lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                // Tạo mới nếu mục chưa tồn tại
                if (_controller.Create(chiTiet))
                {
                    MessageBox.Show("Thêm chi tiết phiếu nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Có lỗi xảy ra khi thêm chi tiết phiếu nhập. Vui lòng thử lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            DataChanged?.Invoke(this, EventArgs.Empty);
            LoadData();


        }
        private void CalculateTotalAmount()
        {
            decimal total = 0;
            foreach (DataGridViewRow row in dataGridViewNew.Rows)
            {
                if (row.IsNewRow) continue;

                decimal giaNhap = decimal.Parse(row.Cells["GiaNhap"].Value.ToString());
                int soLuongNhap = int.Parse(row.Cells["SoLuongNhap"].Value.ToString());
                total += giaNhap * soLuongNhap;
            }
            txtTongTien.Text = total.ToString("N0"); // Hiển thị tổng tiền có định dạng
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //// Xóa dòng đã chọn khỏi cả 2 DataGridView
            //if (dataGridView.SelectedRows.Count > 0) dataGridView.Rows.Remove(dataGridView.SelectedRows[0]);
            //if (dataGridViewNew.SelectedRows.Count > 0) dataGridViewNew.Rows.Remove(dataGridViewNew.SelectedRows[0]);
            try
            {
                bool deletedFromGrid1 = false;
                bool deletedFromGrid2 = false;

                // Kiểm tra và xóa dòng đã chọn từ dataGridView
                if (dataGridView.SelectedRows.Count > 0)
                {
                    foreach (DataGridViewRow row in dataGridView.SelectedRows)
                    {
                        if (!row.IsNewRow)
                        {
                            dataGridView.Rows.Remove(row); // Xóa dòng đã chọn
                            deletedFromGrid1 = true;
                        }
                    }
                }

                // Kiểm tra và xóa dòng đã chọn từ dataGridViewNew
                if (dataGridViewNew.SelectedRows.Count > 0)
                {
                    foreach (DataGridViewRow row in dataGridViewNew.SelectedRows)
                    {
                        if (!row.IsNewRow)
                        {
                            dataGridViewNew.Rows.Remove(row); // Xóa dòng đã chọn
                            deletedFromGrid2 = true;
                        }
                    }
                }

                // Kiểm tra xem có dòng nào bị xóa hay không, rồi hiển thị thông báo tương ứng
                if (deletedFromGrid1 || deletedFromGrid2)
                {
                    MessageBox.Show("Dữ liệu đã được xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Không có dòng nào được chọn để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                // Thông báo lỗi nếu có
                MessageBox.Show($"Có lỗi xảy ra khi xóa dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void ClearTextBoxes()
        {
            // Đặt tất cả các TextBox về trạng thái rỗng
            txtIDPN.Clear();
            txtSoPhieu.Clear();
            txtMaHangHoa.Clear();
            txtSoLuongNhap.Clear();
            txtGiaNhap.Clear();
            dtpNgaySanXuat.Value = DateTime.Now; // Hoặc giá trị mặc định bạn muốn
            dtpHangSuDung.Value = DateTime.Now; // Hoặc giá trị mặc định bạn muốn
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }
        private bool ValidateInputs()
        {
            // Kiểm tra tất cả các textbox cần thiết
            return !string.IsNullOrEmpty(txtSoPhieu.Text) &&
                   !string.IsNullOrEmpty(txtMaHangHoa.Text) &&
                   !string.IsNullOrEmpty(txtSoLuongNhap.Text) &&
                   !string.IsNullOrEmpty(txtGiaNhap.Text) && // Có thể thêm kiểm tra cho giá nhập
                   int.TryParse(txtSoPhieu.Text, out _) && // Đảm bảo giá trị hợp lệ
                   int.TryParse(txtMaHangHoa.Text, out _) &&
                   int.TryParse(txtSoLuongNhap.Text, out _) &&
                   decimal.TryParse(txtGiaNhap.Text, out _);
        }
        public void UpdateView()
        {
            btnSave.Enabled = ValidateInputs(); // Kích hoạt nút "Save" nếu tất cả đều hợp lệ
            btnDelete.Enabled = !string.IsNullOrEmpty(txtIDPN.Text); // Kích hoạt nút "Delete" nếu IDPN không rỗng
        }

        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridView.SelectedRows[0];

                // Kiểm tra các cột "GiaNhap" và "HangSuDung" có giá trị không
                txtIDPN.Text = selectedRow.Cells["IDPN"].Value?.ToString();
                txtSoPhieu.Text = selectedRow.Cells["SoPhieu"].Value?.ToString();
                txtMaHangHoa.Text = selectedRow.Cells["MaHangHoa"].Value?.ToString();
                txtSoLuongNhap.Text = selectedRow.Cells["SoLuongNhap"].Value?.ToString();

                // Kiểm tra giá trị của "NgaySanXuat" và "HangSuDung"
                if (selectedRow.Cells["NgaySanXuat"].Value != DBNull.Value)
                {
                    dtpNgaySanXuat.Value = (DateTime)selectedRow.Cells["NgaySanXuat"].Value;
                }
                else
                {
                    // Nếu giá trị null thì thiết lập giá trị mặc định
                    dtpNgaySanXuat.Value = DateTime.Now;
                }

                // Kiểm tra giá trị của "GiaNhap"
                if (selectedRow.Cells["GiaNhap"].Value != DBNull.Value)
                {
                    txtGiaNhap.Text = selectedRow.Cells["GiaNhap"].Value?.ToString();
                }
                else
                {
                    // Nếu giá trị null thì thiết lập giá trị mặc định
                    txtGiaNhap.Clear();
                }

                // Kiểm tra giá trị của "HangSuDung"
                if (selectedRow.Cells["HangSuDung"].Value != DBNull.Value)
                {
                    dtpHangSuDung.Value = (DateTime)selectedRow.Cells["HangSuDung"].Value;
                }
                else
                {
                    // Nếu giá trị null thì thiết lập giá trị mặc định
                    dtpHangSuDung.Value = DateTime.Now;
                }
            }
        }


        private void txtIDPN_TextChanged(object sender, EventArgs e)
        {
            UpdateView();
        }

        private void txtSoPhieu_TextChanged(object sender, EventArgs e)
        {
            UpdateView();
        }

        private void txtSoLuongNhap_TextChanged(object sender, EventArgs e)
        {
            UpdateView();
        }

        private void dtpNgaySanXuat_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dtpHangSuDung_ValueChanged(object sender, EventArgs e)
        {

        }

        public void SetDataToText(object item)
        {
            if (item is ChiTietPhieuNhap chiTiet)
            {
                txtIDPN.Text = chiTiet.IDPN.ToString();
                txtSoPhieu.Text = chiTiet.SoPhieu.ToString();
                txtMaHangHoa.Text = chiTiet.MaHangHoa.ToString();
                txtSoLuongNhap.Text = chiTiet.SoLuongNhap.ToString();

                // Kiểm tra nếu NgaySanXuat không null
                if (chiTiet.NgaySanXuat.HasValue)
                {
                    dtpNgaySanXuat.Value = chiTiet.NgaySanXuat.Value;
                }
                else
                {
                    dtpNgaySanXuat.Value = DateTime.Now; // Giá trị mặc định nếu null
                }

                txtGiaNhap.Text = chiTiet.GiaNhap.ToString();

                // Kiểm tra nếu HangSuDung không null
                if (chiTiet.HangSuDung.HasValue)
                {
                    dtpHangSuDung.Value = chiTiet.HangSuDung.Value;
                }
                else
                {
                    dtpHangSuDung.Value = DateTime.Now; // Giá trị mặc định nếu null
                }
            }
        }








        public object GetDataFromText()
        {
            return new ChiTietPhieuNhap
            {
                IDPN = int.TryParse(txtIDPN.Text, out int idpn) ? idpn : 0,
                SoPhieu = int.TryParse(txtSoPhieu.Text, out int soPhieu) ? soPhieu : 0,
                MaHangHoa = int.TryParse(txtMaHangHoa.Text, out int maHangHoa) ? maHangHoa : 0,
                SoLuongNhap = int.TryParse(txtSoLuongNhap.Text, out int soLuongNhap) ? soLuongNhap : 0,
                NgaySanXuat = dtpNgaySanXuat.Value,
                GiaNhap = decimal.TryParse(txtGiaNhap.Text, out decimal giaNhap) ? giaNhap : 0,
                HangSuDung = dtpHangSuDung.Value
            };
        }

       

        private void txtTongTien_TextChanged(object sender, EventArgs e)
        {

        }
        
        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Kiểm tra tính hợp lệ của số phiếu
            if (!int.TryParse(txtSoPhieu.Text, out int soPhieu) || !_controller.IsSoPhieuValid(soPhieu))
            {
                MessageBox.Show("Số phiếu không hợp lệ. Vui lòng kiểm tra lại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Lấy dữ liệu từ form
            var chiTiet = (ChiTietPhieuNhap)GetDataFromText();

            // Kiểm tra nếu có dòng được chọn
            if (dataGridViewNew.SelectedRows.Count > 0)
            {
                // Lấy dòng đã chọn (chỉ có 1 dòng được chọn)
                var selectedRow = dataGridViewNew.SelectedRows[0];

                // Cập nhật dữ liệu của dòng đã chọn
                selectedRow.Cells["IDPN"].Value = chiTiet.IDPN;
                selectedRow.Cells["SoPhieu"].Value = chiTiet.SoPhieu;
                selectedRow.Cells["MaHangHoa"].Value = chiTiet.MaHangHoa;
                selectedRow.Cells["SoLuongNhap"].Value = chiTiet.SoLuongNhap;
                selectedRow.Cells["NgaySanXuat"].Value = chiTiet.NgaySanXuat is DateTime ? ((DateTime)chiTiet.NgaySanXuat).ToString("dd/MM/yyyy") : "";
                selectedRow.Cells["GiaNhap"].Value = chiTiet.GiaNhap;
                selectedRow.Cells["HangSuDung"].Value = chiTiet.HangSuDung is DateTime ? ((DateTime)chiTiet.HangSuDung).ToString("dd/MM/yyyy") : "";

                // Hiển thị thông báo thành công
                MessageBox.Show("Cập nhật chi tiết phiếu nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                // Nếu không có dòng nào được chọn, thêm mới
                // Nếu DataGridView chưa có cột, thêm các cột vào
                if (dataGridViewNew.Columns.Count == 0)
                {
                    dataGridViewNew.Columns.Add("IDPN", "ID Phiếu Nhập");
                    dataGridViewNew.Columns.Add("SoPhieu", "Số Phiếu");
                    dataGridViewNew.Columns.Add("MaHangHoa", "Mã Hàng Hóa");
                    dataGridViewNew.Columns.Add("SoLuongNhap", "Số Lượng Nhập");
                    dataGridViewNew.Columns.Add("NgaySanXuat", "Ngày Sản Xuất");
                    dataGridViewNew.Columns.Add("GiaNhap", "Giá Nhập");
                    dataGridViewNew.Columns.Add("HangSuDung", "Hạn Sử Dụng");
                }

                // Thêm chi tiết vào DataGridView
                dataGridViewNew.Rows.Add(
                    chiTiet.IDPN,
                    chiTiet.SoPhieu,
                    chiTiet.MaHangHoa,
                    chiTiet.SoLuongNhap,
                    chiTiet.NgaySanXuat is DateTime ? ((DateTime)chiTiet.NgaySanXuat).ToString("dd/MM/yyyy") : "",
                    chiTiet.GiaNhap,
                    chiTiet.HangSuDung is DateTime ? ((DateTime)chiTiet.HangSuDung).ToString("dd/MM/yyyy") : ""
                );

                // Hiển thị thông báo thành công
                MessageBox.Show("Thêm chi tiết phiếu nhập vào danh sách thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            // Tính tổng tiền sau khi thêm/sửa chi tiết
            CalculateTotalAmount();

            //test1
            //// Kiểm tra tính hợp lệ của số phiếu
            //if (!int.TryParse(txtSoPhieu.Text, out int soPhieu) || !_controller.IsSoPhieuValid(soPhieu))
            //{
            //    MessageBox.Show("Số phiếu không hợp lệ. Vui lòng kiểm tra lại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}

            //// Lấy dữ liệu từ form
            //var chiTiet = (ChiTietPhieuNhap)GetDataFromText();

            //// Nếu DataGridView chưa có cột, thêm các cột vào
            //if (dataGridViewNew.Columns.Count == 0)
            //{
            //    dataGridViewNew.Columns.Add("IDPN", "ID Phiếu Nhập");
            //    dataGridViewNew.Columns.Add("SoPhieu", "Số Phiếu");
            //    dataGridViewNew.Columns.Add("MaHangHoa", "Mã Hàng Hóa");
            //    dataGridViewNew.Columns.Add("SoLuongNhap", "Số Lượng Nhập");
            //    dataGridViewNew.Columns.Add("NgaySanXuat", "Ngày Sản Xuất");
            //    dataGridViewNew.Columns.Add("GiaNhap", "Giá Nhập");
            //    dataGridViewNew.Columns.Add("HangSuDung", "Hạn Sử Dụng");
            //}

            //// Thêm chi tiết vào DataGridView
            //dataGridViewNew.Rows.Add(
            //    chiTiet.IDPN,
            //    chiTiet.SoPhieu,
            //    chiTiet.MaHangHoa,
            //    chiTiet.SoLuongNhap,
            //    chiTiet.NgaySanXuat is DateTime ? ((DateTime)chiTiet.NgaySanXuat).ToString("dd/MM/yyyy") : "",
            //    chiTiet.GiaNhap,
            //    chiTiet.HangSuDung is DateTime ? ((DateTime)chiTiet.HangSuDung).ToString("dd/MM/yyyy") : ""
            //);

            //// Tính tổng tiền sau khi thêm chi tiết
            //CalculateTotalAmount();

            //// Hiển thị thông báo thành công
            //MessageBox.Show("Thêm chi tiết phiếu nhập vào danh sách thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);





            //case1

            //        // Kiểm tra tính hợp lệ của số phiếu
            //        if (!int.TryParse(txtSoPhieu.Text, out int soPhieu) || !_controller.IsSoPhieuValid(soPhieu))
            //        {
            //            MessageBox.Show("Số phiếu không hợp lệ. Vui lòng kiểm tra lại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //            return;
            //        }

            //        // Lấy dữ liệu từ form
            //        var chiTiet = (ChiTietPhieuNhap)GetDataFromText();

            //        // Kiểm tra xem mã phiếu nhập đã tồn tại trong DataGridView chưa
            //        bool isExistInGrid = false;
            //        foreach (DataGridViewRow row in dataGridViewNew.Rows)
            //        {
            //            if (row.Cells["IDPN"].Value?.ToString() == chiTiet.IDPN.ToString())
            //            {
            //                isExistInGrid = true;
            //                break;
            //            }
            //        }

            //        // Nếu đã tồn tại, thông báo lỗi và dừng lại
            //        if (isExistInGrid)
            //        {
            //            MessageBox.Show("Chi tiết phiếu nhập đã tồn tại trong danh sách. Vui lòng kiểm tra lại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //            return;
            //        }
            //        if (dataGridViewNew.Columns.Count == 0)
            //        {
            //            dataGridViewNew.Columns.Add("IDPN", "ID Phiếu Nhập");
            //            dataGridViewNew.Columns.Add("SoPhieu", "Số Phiếu");
            //            dataGridViewNew.Columns.Add("MaHangHoa", "Mã Hàng Hóa");
            //            dataGridViewNew.Columns.Add("SoLuongNhap", "Số Lượng Nhập");
            //            dataGridViewNew.Columns.Add("NgaySanXuat", "Ngày Sản Xuất");
            //            dataGridViewNew.Columns.Add("GiaNhap", "Giá Nhập");
            //            dataGridViewNew.Columns.Add("HangSuDung", "Hạn Sử Dụng");
            //        }

            //        // Thêm chi tiết vào DataGridView nếu chưa tồn tại
            //        dataGridViewNew.Rows.Add(
            //chiTiet.IDPN,
            //chiTiet.SoPhieu,
            //chiTiet.MaHangHoa,
            //chiTiet.SoLuongNhap,
            //chiTiet.NgaySanXuat is DateTime ? ((DateTime)chiTiet.NgaySanXuat).ToString("dd/MM/yyyy") : "",
            //chiTiet.GiaNhap,
            //chiTiet.HangSuDung is DateTime ? ((DateTime)chiTiet.HangSuDung).ToString("dd/MM/yyyy") : ""
            //    );

            //        // Tính tổng tiền sau khi thêm chi tiết
            //        CalculateTotalAmount();

            //        // Hiển thị thông báo thành công
            //        MessageBox.Show("Thêm chi tiết phiếu nhập vào danh sách thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        private void UpdateTotalPrice()
        {
            decimal total = 0;
            foreach (DataGridViewRow row in dataGridViewNew.Rows)
            {
                if (row.Cells["GiaNhap"].Value != null)
                {
                    total += Convert.ToDecimal(row.Cells["GiaNhap"].Value);
                }
            }
            txtTongTien.Text = total.ToString("N0");
        }
        private decimal CalculateTotalPrice(decimal soLuong, decimal giaNhap)
        {
            return soLuong * giaNhap; // Tính tổng tiền cho 1 sản phẩm
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = @"C:\Users\Admin\Videos\Hoadon"; // Đặt thư mục mặc định
            saveFileDialog.Filter = "CSV Files (*.csv)|*.csv";
            saveFileDialog.FileName = "ChiTietPhieuNhap.csv";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    ExportDataToCSV(saveFileDialog.FileName);
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Có lỗi xảy ra khi xuất dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            LoadData();
        }



        private void ExportDataToCSV(string filePath)
        {
            try
            {
                // Kiểm tra nếu DataGridView không có dữ liệu
                if (dataGridViewNew.Rows.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                decimal tongTienToanBo = 0; // Biến lưu tổng tiền của tất cả các dòng

                // Tạo StreamWriter để ghi vào file CSV
                using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
                {
                    // Ghi header (tên các cột)
                    writer.WriteLine("ID Phiếu Nhập,Số Phiếu,Mã Hàng Hóa,Số Lượng Nhập,Ngày Sản Xuất,Giá Nhập,Hạn Sử Dụng,Tổng Tiền");

                    // Duyệt qua các dòng dữ liệu trong DataGridView và ghi vào file
                    foreach (DataGridViewRow row in dataGridViewNew.Rows)
                    {
                        if (row.IsNewRow) continue; // Bỏ qua dòng mới (chưa có dữ liệu)

                        // Lấy giá trị các cột và đảm bảo dữ liệu đầu ra không bị lỗi hoặc thiếu
                        string idPN = row.Cells["IDPN"].Value?.ToString().Replace(",", "") ?? ""; // Xóa dấu phẩy nếu có
                        string soPhieu = row.Cells["SoPhieu"].Value?.ToString().Replace(",", "") ?? "";
                        string maHangHoa = row.Cells["MaHangHoa"].Value?.ToString().Replace(",", "") ?? "";
                        string soLuongNhap = row.Cells["SoLuongNhap"].Value?.ToString().Replace(",", "") ?? "0";
                        string giaNhap = row.Cells["GiaNhap"].Value?.ToString().Replace(",", "") ?? "0";

                        // Kiểm tra và chuyển đổi ngày sản xuất
                        string ngaySanXuat = "";
                        if (row.Cells["NgaySanXuat"].Value != DBNull.Value && DateTime.TryParse(row.Cells["NgaySanXuat"].Value.ToString(), out DateTime parsedNgaySanXuat))
                        {
                            ngaySanXuat = parsedNgaySanXuat.ToString("dd/MM/yyyy");
                        }

                        // Kiểm tra và chuyển đổi hạn sử dụng
                        string hangSuDung = "";
                        if (row.Cells["HangSuDung"].Value != DBNull.Value && DateTime.TryParse(row.Cells["HangSuDung"].Value.ToString(), out DateTime parsedHangSuDung))
                        {
                            hangSuDung = parsedHangSuDung.ToString("dd/MM/yyyy");
                        }

                        // Chuyển đổi số lượng nhập và giá nhập thành dạng số cho tính toán
                        if (!decimal.TryParse(soLuongNhap, out decimal soLuong))
                        {
                            soLuong = 0;
                        }
                        if (!decimal.TryParse(giaNhap, out decimal giaNhapDecimal))
                        {
                            giaNhapDecimal = 0;
                        }

                        // Tính toán tổng tiền cho dòng hiện tại
                        decimal tongTien = soLuong * giaNhapDecimal;
                        tongTienToanBo += tongTien; // Cộng dồn vào tổng tiền toàn bộ

                        // Định dạng lại các giá trị cho đầu ra CSV
                        string giaNhapFormatted = giaNhapDecimal.ToString("#,##0");
                        string tongTienFormatted = tongTien.ToString("#,##0");

                        // Ghi dòng dữ liệu vào file CSV với định dạng chính xác
                        writer.WriteLine($"{idPN},{soPhieu},{maHangHoa},{soLuongNhap},{ngaySanXuat},{giaNhapFormatted},{hangSuDung},{tongTienFormatted}");

                        // Lưu dữ liệu vào cơ sở dữ liệu
                        SaveDataToDatabase(idPN, soPhieu, maHangHoa, soLuongNhap, ngaySanXuat, giaNhapDecimal, hangSuDung);
                    }

                    // Ghi dòng tổng tiền vào cuối file
                    writer.WriteLine($",,,,,,Tổng Tiền,{tongTienToanBo.ToString("#,##0")}");

                    // Thông báo thành công
                    MessageBox.Show("Xuất dữ liệu thành công và lưu vào cơ sở dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                // Thông báo lỗi
                MessageBox.Show($"Có lỗi xảy ra khi xuất dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        // Helper method to parse decimal values safely
       
        private void SaveDataToDatabase(string idPN, string soPhieu, string maHangHoa, string soLuongNhap, string ngaySanXuat, decimal giaNhap, string hangSuDung)
        {
            try
            {
                using (var context = new MyDatabase())
                {
                    var chiTiet = new ChiTietPhieuNhap
                    {
                        IDPN = Convert.ToInt32(idPN),
                        SoPhieu = Convert.ToInt32(soPhieu),
                        MaHangHoa = Convert.ToInt32(maHangHoa),
                        SoLuongNhap = Convert.ToInt32(soLuongNhap),
                        NgaySanXuat = DateTime.Parse(ngaySanXuat),
                        GiaNhap = giaNhap,
                        HangSuDung = DateTime.Parse(hangSuDung)
                    };

                    context.ChiTietPhieuNhaps.Add(chiTiet);
                    context.SaveChanges(); // Lưu dữ liệu vào cơ sở dữ liệu
                }

                MessageBox.Show("Dữ liệu đã được lưu vào cơ sở dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi xảy ra khi lưu dữ liệu vào cơ sở dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    



        private void txtTongTien_TextChanged_1(object sender, EventArgs e)
        {
            // Cập nhật tổng tiền khi có thay đổi trong TextBox tổng tiền
            
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            // Khởi tạo đối tượng MyDatabase để truy vấn cơ sở dữ liệu
            using (var dbContext = new MyDatabase())
            {
                // Sử dụng Eager Loading để tải dữ liệu liên quan (ví dụ: PhieuNhap)
                var chiTietList = dbContext.ChiTietPhieuNhaps
                                           .Include("PhieuNhap") // Tải trước đối tượng PhieuNhap dưới dạng chuỗi
                                           .ToList(); // Hoặc có thể áp dụng điều kiện nếu cần

                // Gán dữ liệu vào DataGridView thông qua DataBinding
                dataGridView.DataSource = chiTietList;
            }
        }
        private void AddEmptyRow()
        {
            // Thêm một dòng trống vào DataGridView
            dataGridViewNew.Rows.Add("", "", "", "", "", "", "");  // Dòng trống với các giá trị trống
        }

        private void dataGridViewNew_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm tra nếu người dùng click vào một dòng hợp lệ (không phải dòng tiêu đề)
            if (e.RowIndex >= 0)
            {
                // Lấy dòng dữ liệu đã chọn
                DataGridViewRow selectedRow = dataGridViewNew.Rows[e.RowIndex];

                // Kiểm tra nếu dòng là dòng trống (các giá trị trong dòng là trống)
                if (selectedRow.Cells.Cast<DataGridViewCell>().All(cell => cell.Value == null || string.IsNullOrEmpty(cell.Value.ToString())))
                {
                    // Nếu là dòng trống, xóa tất cả giá trị trong các TextBox
                    txtIDPN.Clear();
                    txtSoPhieu.Clear();
                    txtMaHangHoa.Clear();
                    txtSoLuongNhap.Clear();
                    txtGiaNhap.Clear();
                    dtpNgaySanXuat.Value = DateTime.Now; // Hoặc giá trị mặc định bạn muốn
                    dtpHangSuDung.Value = DateTime.Now; // Hoặc giá trị mặc định bạn muốn
                }
                else
                {
                    // Cập nhật TextBox với dữ liệu của dòng đã chọn
                    txtIDPN.Text = selectedRow.Cells["IDPN"].Value?.ToString() ?? "";
                    txtSoPhieu.Text = selectedRow.Cells["SoPhieu"].Value?.ToString() ?? "";
                    txtMaHangHoa.Text = selectedRow.Cells["MaHangHoa"].Value?.ToString() ?? "";
                    txtSoLuongNhap.Text = selectedRow.Cells["SoLuongNhap"].Value?.ToString() ?? "";
                    txtGiaNhap.Text = selectedRow.Cells["GiaNhap"].Value?.ToString() ?? "";

                    // Cập nhật ngày sản xuất nếu có giá trị
                    if (selectedRow.Cells["NgaySanXuat"].Value != DBNull.Value)
                    {
                        DateTime parsedNgaySanXuat;
                        if (DateTime.TryParse(selectedRow.Cells["NgaySanXuat"].Value.ToString(), out parsedNgaySanXuat))
                        {
                            dtpNgaySanXuat.Value = parsedNgaySanXuat;
                        }
                    }

                    // Cập nhật hạn sử dụng nếu có giá trị
                    if (selectedRow.Cells["HangSuDung"].Value != DBNull.Value)
                    {
                        DateTime parsedHangSuDung;
                        if (DateTime.TryParse(selectedRow.Cells["HangSuDung"].Value.ToString(), out parsedHangSuDung))
                        {
                            dtpHangSuDung.Value = parsedHangSuDung;
                        }
                    }
                }
            }
        }
    }
    
}