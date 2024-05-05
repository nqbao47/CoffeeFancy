using CoffeeFancy.DAO;
using CoffeeFancy.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Menu = CoffeeFancy.DTO.Menu;

namespace CoffeeFancy
{
    public partial class fTableManager : Form
    {
        public fTableManager()
        {
            InitializeComponent();

            LoadTable();
            LoadCategory();
        }

        #region METHOD
        void LoadCategory()
        {
            List<Category> listCategory = CategoryDAO.Instance.GetListCategory();
            cbbCategory.DataSource = listCategory;
            cbbCategory.DisplayMember = "Name";
        }

        void LoadFoodListByCategoryID(int id)
        {
            List<Food> listFood = FoodDAO.Instance.GetFoodByCategoryID(id);
            cbbFood.DataSource = listFood;
            cbbFood.DisplayMember = "Name";
        }

        void LoadTable()
        {
            flpTable.Controls.Clear(); 
            List<Table> tableList = TableDAO.Instance.LoadTableList();

            foreach (Table item in tableList)
            {
                Button btn = new Button() { Width = TableDAO.TableWidth, Height = TableDAO.TableHeight };
                btn.Text = item.Name + Environment.NewLine + item.Status;
                btn.Click += Btn_Click;
                btn.Tag = item; // Dựa vào btnClick để xác định đc tableID

                switch (item.Status) {
                    case "Trống":
                        btn.BackColor = Color.White;
                        break;
                    default:
                        btn.BackColor = Color.LightPink;
                        break;
                }

                flpTable.Controls.Add(btn);
            }
        }

        void ShowInvoice(int id)
        {
            lsvInvoice.Items.Clear();
            List<Menu> listInvoiceInfos = MenuDAO.Instance.GetListMenuByTable(id);
            float totalPrice = 0;
            foreach (Menu item in listInvoiceInfos)
            {
                ListViewItem lsvItem = new ListViewItem(item.FoodName.ToString());
                lsvItem.SubItems.Add((item.Count.ToString()));
                lsvItem.SubItems.Add((item.Price.ToString()));
                lsvItem.SubItems.Add((item.TotalPrice.ToString()));
                totalPrice += item.TotalPrice;

                lsvInvoice.Items.Add(lsvItem);
            }
            CultureInfo culture = new CultureInfo("vi-VN");

            // Thread.CurrentThread.CurrentCulture = culture;

            txtTotalPrice.Text = totalPrice.ToString("c", culture);
        }

        #endregion

        #region EVENTS
        private void Btn_Click(object sender, EventArgs e)
        {
            int tableID = ((sender as Button).Tag as Table).ID;
            lsvInvoice.Tag = (sender as Button).Tag;
            ShowInvoice(tableID);
        }
        private void nmFoodCount_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void thôngTinCáNhânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAccountProfile f = new fAccountProfile();
            f.ShowDialog();
        }

        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAdmin f = new fAdmin();
            f.ShowDialog();
        }

        private void cbbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = 0;

            ComboBox cb = sender as ComboBox;

            if (cb.SelectedItem == null)
                return;

            Category selected = cb.SelectedItem as Category;
            id = selected.ID;

            LoadFoodListByCategoryID(id);
        }

        private void btnAddFood_Click(object sender, EventArgs e)
        {
            Table table = lsvInvoice.Tag as Table;

            int idInvoice = InvoiceDAO.Instance.GetUncheckInvoiceIDbyTableID(table.ID);
            int foodID = (cbbFood.SelectedItem as Food).ID;
            int count = (int)nmCount.Value;

            if(idInvoice == - 1) // Case: 1
            {
                InvoiceDAO.Instance.InsertInvoice(table.ID);
                InvoiceInfoDAO.Instance.InsertInvoiceInfo(InvoiceDAO.Instance.GetMaxIdInvoice(), foodID, count);
            }
            else // Case: 2
            {
                InvoiceInfoDAO.Instance.InsertInvoiceInfo(idInvoice, foodID, count);
            }

            // Reload
            ShowInvoice(table.ID);
            LoadTable();
        }
        private void btnPay_Click(object sender, EventArgs e)
        {
            // Lấy ra table
            Table table = lsvInvoice.Tag as Table;

            // Lấy ra id
            int idInvoice = InvoiceDAO.Instance.GetUncheckInvoiceIDbyTableID(table.ID);

            /* Trigger cho việc CheckOut (Xác định trạng thái của bàn sau khi CheckOut)
             * 1 - Tạo trigger Update or Insert cho InvoiceInfo (Update thông tin thành `Có người`)
             * 2 - Tạo trigger Update cho Invoice (Update thông tin thành `Trống`)
             */

            // Kiểm tra xem invoice đang ở trạng thái nào (Chưa có - Đã thanh toán - Chưa thanh toán)
            if (idInvoice != -1)
            {
                if(MessageBox.Show("Xác nhận thanh toán cho: " + table.Name, "Thông báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    InvoiceDAO.Instance.CheckOut(idInvoice);
                    ShowInvoice(table.ID);
                    LoadTable();
                } 
                    
            }
        }

        #endregion

    }
}
