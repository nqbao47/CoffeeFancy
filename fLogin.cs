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
            fTableManager f = new fTableManager();
            this.Hide();
            f.ShowDialog(); // Sử dụng Dialog vì tính chất của nó là đợi để hiển thị lên
            this.Show();
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
    }
}
