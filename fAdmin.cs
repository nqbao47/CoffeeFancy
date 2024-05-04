using CoffeeFancy.DAO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoffeeFancy
{
    public partial class fAdmin : Form
    {
        public fAdmin()
        {
            InitializeComponent();
            LoadAccountList();
            LoadFoodList();
        }

        void LoadFoodList()
        {
            string query = "SELECT * FROM Food";

            dtgvFood.DataSource = DataProvider.Instance.ExecuteQuery(query);
        }
        void LoadAccountList()
        {

            //string query = "SELECT * FROM dbo.Account";
            // string query = "SELECT ShowName as [Ten hien thi] FROM dbo.Account";
            string query = "EXEC dbo.USP_GetAccountByUserName @username";

            dtgvAccount.DataSource = DataProvider.Instance.ExecuteQuery(query, new object[] {"Staff"});

        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
