using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Lab02_02
{
    public partial class QLSINHVIEN : Form
    {
        private List<StudentItem> _allStudents = new List<StudentItem>();

        public QLSINHVIEN()
        {
            InitializeComponent();
        }

        private void QLSINHVIEN_Load(object sender, EventArgs e)
        {
            // Khởi tạo ComboBox nhập liệu
            cmbFaculty.Items.AddRange(new string[] { "QTKD", "CNTT", "NNA" });
            cmbFaculty.SelectedIndex = 0;

            // Khởi tạo ComboBox lọc
            cmbFilterFaculty.Items.AddRange(new string[] { "Tất cả", "QTKD", "CNTT", "NNA" });
            cmbFilterFaculty.SelectedIndex = 0;

            cmbFilterRank.Items.AddRange(new string[] { "Tất cả", "Xuất sắc", "Giỏi", "Khá", "Trung bình", "Yếu", "Kém" });
            cmbFilterRank.SelectedIndex = 0;

            radFemale.Checked = true;

            // Cấu hình DataGridView
            dgvStudent.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvStudent.MultiSelect = false;
            dgvStudent.AllowUserToAddRows = false;

            dgvStudent.Columns.Clear();
            dgvStudent.Columns.Add("colStudentID", "Mã số SV");
            dgvStudent.Columns.Add("colFullName", "Họ tên");
            dgvStudent.Columns.Add("colGender", "Giới tính");
            dgvStudent.Columns.Add("colAverageScore", "Điểm TB");
            dgvStudent.Columns.Add("colFaculty", "Khoa");

            // 10 sinh viên mẫu
            _allStudents = new List<StudentItem>
            {
                new StudentItem { ID = "SV001", Name = "Nguyễn Văn A", Gender = "Nam", Score = 7.5f, Faculty = "CNTT" },
                new StudentItem { ID = "SV002", Name = "Trần Thị B", Gender = "Nữ", Score = 8.2f, Faculty = "CNTT" },
                new StudentItem { ID = "SV003", Name = "Lê Văn C", Gender = "Nam", Score = 6.0f, Faculty = "QTKD" },
                new StudentItem { ID = "SV004", Name = "Phạm Thị D", Gender = "Nữ", Score = 9.0f, Faculty = "NNA" },
                new StudentItem { ID = "SV005", Name = "Hoàng Văn E", Gender = "Nam", Score = 5.5f, Faculty = "CNTT" },
                new StudentItem { ID = "SV006", Name = "Vũ Thị F", Gender = "Nữ", Score = 7.8f, Faculty = "QTKD" },
                new StudentItem { ID = "SV007", Name = "Đỗ Văn G", Gender = "Nam", Score = 8.5f, Faculty = "CNTT" },
                new StudentItem { ID = "SV008", Name = "Bùi Thị H", Gender = "Nữ", Score = 6.7f, Faculty = "NNA" },
                new StudentItem { ID = "SV009", Name = "Đặng Văn I", Gender = "Nam", Score = 9.2f, Faculty = "CNTT" },
                new StudentItem { ID = "SV010", Name = "Ngô Thị K", Gender = "Nữ", Score = 7.0f, Faculty = "QTKD" }
            };

            ApplyFilter();
        }

        private class StudentItem
        {
            public string ID { get; set; }
            public string Name { get; set; }
            public string Gender { get; set; }
            public float Score { get; set; }
            public string Faculty { get; set; }

            public string GetRank()
            {
                if (Score >= 9.0f) return "Xuất sắc";
                if (Score >= 8.0f) return "Giỏi";
                if (Score >= 7.0f) return "Khá";
                if (Score >= 5.0f) return "Trung bình";
                if (Score >= 4.0f) return "Yếu";
                return "Kém";
            }
        }

        private void ApplyFilter()
        {
            string search = txtSearch.Text.Trim().ToLower();
            string faculty = cmbFilterFaculty.SelectedItem?.ToString() ?? "Tất cả";
            string rank = cmbFilterRank.SelectedItem?.ToString() ?? "Tất cả";

            dgvStudent.Rows.Clear();

            foreach (var s in _allStudents)
            {
                bool matchSearch = string.IsNullOrEmpty(search) ||
                                   s.ID.ToLower().Contains(search) ||
                                   s.Name.ToLower().Contains(search);

                bool matchFaculty = faculty == "Tất cả" || s.Faculty == faculty;
                bool matchRank = rank == "Tất cả" || s.GetRank() == rank;

                if (matchSearch && matchFaculty && matchRank)
                {
                    dgvStudent.Rows.Add(s.ID, s.Name, s.Gender, s.Score, s.Faculty);
                }
            }

            UpdateGenderCounts();
        }

        private bool IsValidInput()
        {
            if (string.IsNullOrWhiteSpace(txtStudentID.Text) ||
                string.IsNullOrWhiteSpace(txtFullName.Text) ||
                string.IsNullOrWhiteSpace(txtAverageScore.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!float.TryParse(txtAverageScore.Text, out float score) || score < 0 || score > 10)
            {
                MessageBox.Show("Điểm trung bình phải là số từ 0 đến 10!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private int FindStudentIndex(string id)
        {
            for (int i = 0; i < _allStudents.Count; i++)
                if (_allStudents[i].ID == id) return i;
            return -1;
        }

        private void btnAddUpdate_Click(object sender, EventArgs e)
        {
            if (!IsValidInput()) return;

            string id = txtStudentID.Text.Trim();
            string name = txtFullName.Text.Trim();
            float score = float.Parse(txtAverageScore.Text);
            string gender = radFemale.Checked ? "Nữ" : "Nam";
            string faculty = cmbFaculty.SelectedItem.ToString();

            int idx = FindStudentIndex(id);

            if (idx == -1)
            {
                _allStudents.Add(new StudentItem { ID = id, Name = name, Gender = gender, Score = score, Faculty = faculty });
                MessageBox.Show("Thêm mới dữ liệu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                _allStudents[idx].Name = name;
                _allStudents[idx].Gender = gender;
                _allStudents[idx].Score = score;
                _allStudents[idx].Faculty = faculty;
                MessageBox.Show("Cập nhật dữ liệu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            ClearInputs();
            ApplyFilter();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string id = txtStudentID.Text.Trim();
            int idx = FindStudentIndex(id);

            if (idx == -1)
            {
                MessageBox.Show("Không tìm thấy MSSV cần xóa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Bạn có chắc muốn xóa sinh viên này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _allStudents.RemoveAt(idx);
                MessageBox.Show("Xóa sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearInputs();
                ApplyFilter();
            }
        }

        private void dgvStudent_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dgvStudent.Rows[e.RowIndex];
            txtStudentID.Text = row.Cells["colStudentID"].Value?.ToString() ?? "";
            txtFullName.Text = row.Cells["colFullName"].Value?.ToString() ?? "";
            txtAverageScore.Text = row.Cells["colAverageScore"].Value?.ToString() ?? "";

            string gender = row.Cells["colGender"].Value?.ToString();
            if (gender == "Nam") radMale.Checked = true;
            else radFemale.Checked = true;

            cmbFaculty.SelectedItem = row.Cells["colFaculty"].Value?.ToString();
        }

        private void UpdateGenderCounts()
        {
            int nam = 0, nu = 0;
            foreach (DataGridViewRow row in dgvStudent.Rows)
            {
                string g = row.Cells["colGender"].Value?.ToString();
                if (g == "Nam") nam++;
                else if (g == "Nữ") nu++;
            }
            lblTotalMale.Text = $"Tổng SV Nam: {nam}";
            lblTotalFemale.Text = $"Tổng SV Nữ: {nu}";
        }

        private void ClearInputs()
        {
            txtStudentID.Clear();
            txtFullName.Clear();
            txtAverageScore.Clear();
            radFemale.Checked = true;
            cmbFaculty.SelectedIndex = 0;
            txtStudentID.Focus();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e) => ApplyFilter();
        private void cmbFilterFaculty_SelectedIndexChanged(object sender, EventArgs e) => ApplyFilter();
        private void cmbFilterRank_SelectedIndexChanged(object sender, EventArgs e) => ApplyFilter();
    }
}