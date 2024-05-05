using CoffeeFancy.DAO;
using CoffeeFancy.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
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
        private Account loginAccount;

        public Account LoginAccount { get { return loginAccount; } set { loginAccount = value; ChangeAccount(LoginAccount.Type); }  }

        public fTableManager(Account acc)
        {
            InitializeComponent();

            this.LoginAccount = acc;

            LoadTable();
            LoadCategory();
            LoadComboBoxTable(cbbSwitchTable);
        }

        #region METHOD
        void ChangeAccount(int type)
        {
            adminToolStripMenuItem.Enabled = type == 1;
        }
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

        void LoadComboBoxTable(ComboBox cb)
        {
            cb.DataSource = TableDAO.Instance.LoadTableList();
            cb.DisplayMember = "Name";
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
            int discount = (int)nmDiscount.Value;

            /* Trigger cho việc CheckOut (Xác định trạng thái của bàn sau khi CheckOut)
             * 1 - Tạo trigger Update or Insert cho InvoiceInfo (Update thông tin thành `Có người`)
             * 2 - Tạo trigger Update cho Invoice (Update thông tin thành `Trống`)
             */

            // Tính kết quả
            double totalPrice = Convert.ToDouble(txtTotalPrice.Text.Split(',')[0].Replace(".", ""));
            double finalTotalPrice = totalPrice - (totalPrice/100) * discount;


            // Kiểm tra xem invoice đang ở trạng thái nào (Chưa có - Đã thanh toán - Chưa thanh toán)
            if (idInvoice != -1)
            {
                if(MessageBox.Show(string.Format(
                    "Xác nhận thanh toán cho: {0}\nTổng tiền - (Tổng tiền / 100) x Giảm giá \n{1} - ({1} / 100) x {2} = {3} ", table.Name, totalPrice, discount, finalTotalPrice), 
                    "Thông báo", 
                    MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    InvoiceDAO.Instance.CheckOut(idInvoice, discount, (float)finalTotalPrice);
                    ShowInvoice(table.ID);
                    LoadTable();
                } 
                    
            }
        }



        /*private void btnSwitchTable_Click(object sender, EventArgs e)
        {
            int id1 = (lsvInvoice.Tag as Table).ID; //lấy ra id table đang chọn
            int id2 = (cbbSwitchTable.SelectedItem as Table).ID; //lấy ra id table muốn chuyển bàn
            if (MessageBox.Show(string.Format("Bạn có muốn chuyển bàn {0} qua bàn {1} không?", (lsvInvoice.Tag as Table).Name, (cbbSwitchTable.SelectedItem as Table).Name), "Thông báo", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                TableDAO.Instance.SwitchTable(id1, id2);
                //chuyển xong sẽ update lại table ban đầu thành trống
                string query1 = "update TableFood set status = N'Trống' where id=" + id1;
                //update table chuyển qua thành có người
                string query2 = "update TableFood set status = N'Có người' where id=" + id2;
                SqlCommand cmd = new SqlCommand(query1, sql);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand(query2, sql);
                cmd.ExecuteNonQuery();
                LoadTable();
            }
        }*/

        private void btnSwitchTable_Click(object sender, EventArgs e)
        {
            int id1 = (lsvInvoice.Tag as Table).ID;
            int id2 = (cbbSwitchTable.SelectedItem as Table).ID;

            if (MessageBox.Show(string.Format(
                "Bạn có chắc muốn chuyển {0} sang {1} không ?", (lsvInvoice.Tag as Table).Name, (cbbSwitchTable.SelectedItem as Table).Name),
                "Thông báo",
                MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                TableDAO.Instance.SwitchTable(id1, id2);
                LoadTable();
            }
        }

        #endregion


    }
}
