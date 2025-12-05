using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Lab02_04
{
    public partial class QLTAIKHOAN : Form
    {
        private List<Account> _accounts = new List<Account>();

        public QLTAIKHOAN()
        {
            InitializeComponent();
        }

        private void QLTAIKHOAN_Load(object sender, EventArgs e)
        {
            // Thêm 10 tài khoản mẫu
            _accounts = new List<Account>
            {
                new Account { AccountNumber = "ACC001", CustomerName = "Nguyễn Văn A", Address = "Quận 1", Balance = 1500000 },
                new Account { AccountNumber = "ACC002", CustomerName = "Trần Thị B", Address = "Quận 2", Balance = 2500000 },
                new Account { AccountNumber = "ACC003", CustomerName = "Lê Văn C", Address = "Quận 3", Balance = 3000000 },
                new Account { AccountNumber = "ACC004", CustomerName = "Phạm Thị D", Address = "Quận 4", Balance = 1200000 },
                new Account { AccountNumber = "ACC005", CustomerName = "Hoàng Văn E", Address = "Quận 5", Balance = 5000000 },
                new Account { AccountNumber = "ACC006", CustomerName = "Vũ Thị F", Address = "Quận 6", Balance = 800000 },
                new Account { AccountNumber = "ACC007", CustomerName = "Đỗ Văn G", Address = "Quận 7", Balance = 2200000 },
                new Account { AccountNumber = "ACC008", CustomerName = "Bùi Thị H", Address = "Quận 8", Balance = 3500000 },
                new Account { AccountNumber = "ACC009", CustomerName = "Đặng Văn I", Address = "Quận 9", Balance = 1800000 },
                new Account { AccountNumber = "ACC010", CustomerName = "Ngô Thị K", Address = "Quận 10", Balance = 4000000 }
            };

            LoadListView();
            UpdateTotalBalance();
        }

        private class Account
        {
            public string AccountNumber { get; set; }
            public string CustomerName { get; set; }
            public string Address { get; set; }
            public decimal Balance { get; set; }
        }

        private void LoadListView()
        {
            lvAccount.Items.Clear();
            for (int i = 0; i < _accounts.Count; i++)
            {
                var acc = _accounts[i];
                var item = new ListViewItem((i + 1).ToString()); // STT
                item.SubItems.Add(acc.AccountNumber);
                item.SubItems.Add(acc.CustomerName);
                item.SubItems.Add(acc.Address);
                item.SubItems.Add(acc.Balance.ToString("N0") + "đ");
                lvAccount.Items.Add(item);
            }
        }

        private void UpdateTotalBalance()
        {
            decimal total = _accounts.Sum(a => a.Balance);
            lblTotalBalance.Text = $"Tổng số tiền: {total:N0}đ";
        }

        private bool IsValidInput()
        {
            if (string.IsNullOrWhiteSpace(txtAccountNumber.Text) ||
                string.IsNullOrWhiteSpace(txtCustomerName.Text) ||
                string.IsNullOrWhiteSpace(txtAddress.Text) ||
                string.IsNullOrWhiteSpace(txtBalance.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!decimal.TryParse(txtBalance.Text, out decimal balance) || balance < 0)
            {
                MessageBox.Show("Số tiền phải là số dương!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private int FindAccountIndex(string accountNumber)
        {
            for (int i = 0; i < _accounts.Count; i++)
            {
                if (_accounts[i].AccountNumber == accountNumber) return i;
            }
            return -1;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!IsValidInput()) return;

            string accNum = txtAccountNumber.Text.Trim();
            string name = txtCustomerName.Text.Trim();
            string address = txtAddress.Text.Trim();
            decimal balance = decimal.Parse(txtBalance.Text);

            int idx = FindAccountIndex(accNum);

            if (idx == -1)
            {
                _accounts.Add(new Account
                {
                    AccountNumber = accNum,
                    CustomerName = name,
                    Address = address,
                    Balance = balance
                });
                MessageBox.Show("Thêm mới dữ liệu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                _accounts[idx].CustomerName = name;
                _accounts[idx].Address = address;
                _accounts[idx].Balance = balance;
                MessageBox.Show("Cập nhật dữ liệu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            ClearInputs();
            LoadListView();
            UpdateTotalBalance();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string accNum = txtAccountNumber.Text.Trim();
            int idx = FindAccountIndex(accNum);

            if (idx == -1)
            {
                MessageBox.Show("Không tìm thấy số tài khoản cần xóa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Bạn có chắc muốn xóa tài khoản này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _accounts.RemoveAt(idx);
                MessageBox.Show("Xóa tài khoản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearInputs();
                LoadListView();
                UpdateTotalBalance();
            }
        }

        private void lvAccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvAccount.SelectedItems.Count > 0)
            {
                var item = lvAccount.SelectedItems[0];
                txtAccountNumber.Text = item.SubItems[1].Text; // cột 1 = số tài khoản
                txtCustomerName.Text = item.SubItems[2].Text;
                txtAddress.Text = item.SubItems[3].Text;
                txtBalance.Text = item.SubItems[4].Text.Replace("đ", "").Replace(",", "");
            }
        }

        private void ClearInputs()
        {
            txtAccountNumber.Clear();
            txtCustomerName.Clear();
            txtAddress.Clear();
            txtBalance.Clear();
            txtAccountNumber.Focus();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string search = txtSearch.Text.Trim().ToLower();

            lvAccount.Items.Clear();

            for (int i = 0; i < _accounts.Count; i++)
            {
                var acc = _accounts[i];
                if (acc.AccountNumber.ToLower().Contains(search) ||
                    acc.CustomerName.ToLower().Contains(search))
                {
                    var item = new ListViewItem((i + 1).ToString());
                    item.SubItems.Add(acc.AccountNumber);
                    item.SubItems.Add(acc.CustomerName);
                    item.SubItems.Add(acc.Address);
                    item.SubItems.Add(acc.Balance.ToString("N0") + "đ");
                    lvAccount.Items.Add(item);
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn thoát chương trình?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close();
            }
        }
    }
}