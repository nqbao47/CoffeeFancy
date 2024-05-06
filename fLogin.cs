using CoffeeFancy.DAO;
using CoffeeFancy.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoffeeFancy
{
    public partial class fLogin : Form
    {
        public fLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string userName = txtUserName.Text;
            string passWord = txtPassWord.Text;

            if(Login(userName, passWord))
            {
                Account loginAccount = AccountDAO.Instance.GetAccountByUsername(userName);

                fTableManager f = new fTableManager(loginAccount);
                this.Hide();
                f.ShowDialog(); // Sử dụng Dialog vì tính chất của nó là đợi để hiển thị lên
                this.Show();
            } else
            {
                MessageBox.Show("Sai tên tài khoản hoặc mật khẩu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Cus Style
        }

        bool Login(string userName, string passWord)
        {
            return AccountDAO.Instance.Login(userName, passWord);    
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void fLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(MessageBox.Show(
                "Bạn có chắc muốn thoát ?", 
                "Thông báo",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question                
                ) != System.Windows.Forms.DialogResult.OK)
            {
                e.Cancel = true;
            } 
                
        }

        private void txtUserName_TextChanged(object sender, EventArgs e)
        {
            //
        }

        private void txtPassWord_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {
            //
        }

        private void btnExit_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Tính năng đang được phát triển ...","Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
