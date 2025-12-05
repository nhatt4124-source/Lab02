using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Lab02_03
{
    public partial class frmRapchieuphim : Form
    {
        // Biến lưu trữ các nút ghế (Button) để dễ quản lý
        private List<Button> seatButtons = new List<Button>();

        public frmRapchieuphim()
        {
            InitializeComponent();
            InitializeSeats();
        }

        // Khởi tạo danh sách các nút ghế
        private void InitializeSeats()
        {
            seatButtons.Add(btnSeat1);
            seatButtons.Add(btnSeat2);
            seatButtons.Add(btnSeat3);
            seatButtons.Add(btnSeat4);
            seatButtons.Add(btnSeat5);
            seatButtons.Add(btnSeat6);
            seatButtons.Add(btnSeat7);
            seatButtons.Add(btnSeat8);
            seatButtons.Add(btnSeat9);
            seatButtons.Add(btnSeat10);
            seatButtons.Add(btnSeat11);
            seatButtons.Add(btnSeat12);
            seatButtons.Add(btnSeat13);
            seatButtons.Add(btnSeat14);
            seatButtons.Add(btnSeat15);
            seatButtons.Add(btnSeat16);
            seatButtons.Add(btnSeat17);
            seatButtons.Add(btnSeat18);
            seatButtons.Add(btnSeat19);
            seatButtons.Add(btnSeat20);
        }

        // Sự kiện khi người dùng click vào bất kỳ ghế nào
        private void btnChooseASeat(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;

            // Nếu ghế đã được bán (màu vàng) -> thông báo
            if (btn.BackColor == Color.Yellow)
            {
                MessageBox.Show("Ghế đã được bán!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Nếu ghế đang chọn (màu xanh) -> hủy chọn (trở về trắng)
            if (btn.BackColor == Color.Blue)
            {
                btn.BackColor = Color.White;
            }
            // Nếu ghế chưa chọn (màu trắng) -> chọn (đổi sang xanh)
            else if (btn.BackColor == Color.White)
            {
                btn.BackColor = Color.Blue;
            }
        }

        // Hàm lấy giá vé theo số ghế
        private int GetSeatPrice(int seatNumber)
        {
            if (seatNumber >= 1 && seatNumber <= 5) return 30000;   // Dãy 1
            if (seatNumber >= 6 && seatNumber <= 10) return 40000;  // Dãy 2
            if (seatNumber >= 11 && seatNumber <= 15) return 50000; // Dãy 3
            if (seatNumber >= 16 && seatNumber <= 20) return 80000; // Dãy 4
            return 0;
        }

        // Sự kiện nút CHỌN
        private void btnChoose_Click(object sender, EventArgs e)
        {
            int totalPrice = 0;

            foreach (Button btn in seatButtons)
            {
                // Nếu ghế đang được chọn (màu xanh) -> đổi thành đã bán (vàng) và cộng tiền
                if (btn.BackColor == Color.Blue)
                {
                    btn.BackColor = Color.Yellow;
                    int seatNumber = int.Parse(btn.Text);
                    totalPrice += GetSeatPrice(seatNumber);
                }
            }

            lblTotalPrice.Text = totalPrice.ToString("N0") + " đ";
            MessageBox.Show($"Đặt vé thành công!\nTổng tiền: {totalPrice:N0} đ", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Sự kiện nút HỦY BỎ
        private void btnCancel_Click(object sender, EventArgs e)
        {
            foreach (Button btn in seatButtons)
            {
                // Nếu ghế đang chọn (màu xanh) -> trả về màu trắng
                if (btn.BackColor == Color.Blue)
                {
                    btn.BackColor = Color.White;
                }
            }

            lblTotalPrice.Text = "0 đ";
            MessageBox.Show("Đã hủy bỏ việc chọn ghế.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn thoát chương trình?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close(); // hoặc Application.Exit() nếu là form chính
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}