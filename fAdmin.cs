using CoffeeFancy.DAO;
using CoffeeFancy.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace CoffeeFancy
{
    public partial class fAdmin : Form
    {
        // Xử lý bị thay đổi dataSource khi sử dụng Binding (mất kết nối Binding khi refresh)
        BindingSource foodList = new BindingSource();

        BindingSource categoryList = new BindingSource();

        BindingSource tableFoodList = new BindingSource();

        // Lấy ra account
        BindingSource accountList = new BindingSource();

        // Không cho phép xoá Account đang login
        public Account loginAccount;
        public fAdmin()
        {
            InitializeComponent();
            LoadMain();
        }

        #region Methods
        List<Food> SearchFoodByName(string name)
        {
            List<Food> listFood = FoodDAO.Instance.SearchFoodByName(name);

            return listFood;
        }

        List<Category> SearchCategoryByName(string name)
        {
            List<Category> listCategory = CategoryDAO.Instance.SearchCategoryByName(name);

            return listCategory;
        }

        void LoadMain()
        {
            dtgvFood.DataSource = foodList;
            dtgvAccount.DataSource = accountList;
            dtgvCategory.DataSource = categoryList;
            dtgvTableFood.DataSource = tableFoodList;

            LoadDateTimePickerInvoice();
            LoadListInvoiceByDate(dtpkFromDate.Value, dtpkToDate.Value);
            LoadlistFood();
            LoadAccount();
            LoadCategoryIntoCbb(cbbFoodCategory);
            AddFoodBinding();
            AddAccountBinding();
            LoadlistCategory();
            LoadlistTableFood();
        }


        void LoadDateTimePickerInvoice()
        {
            DateTime today = DateTime.Now;
            dtpkFromDate.Value = new DateTime(today.Year, today.Month, 1);
            dtpkToDate.Value = dtpkFromDate.Value.AddMonths(1).AddDays(-1);
        }
        void LoadListInvoiceByDate(DateTime checkIn, DateTime checkOut)
        {
            dtgvInvoice.DataSource = InvoiceDAO.Instance.GetListInvoiceByDate(checkIn, checkOut);
        }

        void AddAccountBinding()
        {
            txtUserName.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "UserName"));
            txtShowName.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "ShowName"));
            nmAccountType.DataBindings.Add(new Binding("Value", dtgvAccount.DataSource, "Type"));
            // Chuyển về thành "Admin" hoặc "Staff"
        }

        void LoadAccount()
        {
            accountList.DataSource = AccountDAO.Instance.GetListAccount();
        }

        void AddFoodBinding()
        {
            txtFoodID.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "Id", true, DataSourceUpdateMode.Never));
            txtFoodName.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "Name", true, DataSourceUpdateMode.Never));
            //cbbFoodCategory.DataBindings.Add(new Binding("Value", dtgvCategory.DataSource, "Name"));
            nmFoodPrice.DataBindings.Add(new Binding("Value", dtgvFood.DataSource, "Price", true, DataSourceUpdateMode.Never));
        }

        void LoadCategoryIntoCbb(ComboBox cb)
        {
            cb.DataSource = CategoryDAO.Instance.GetListCategory();
            cb.DisplayMember = "Name";
        }

        void LoadlistFood()
        {
            foodList.DataSource = FoodDAO.Instance.GetListFood();
        }

        void LoadlistCategory()
        {
            categoryList.DataSource = CategoryDAO.Instance.GetListCategory();
        }

        void LoadlistTableFood()
        {
            tableFoodList.DataSource = TableDAO.Instance.LoadTableList();
        }

        void AddAccount(string userName, string showName, int type)
        {
           if(AccountDAO.Instance.InsertAccount(userName, showName, type))
            {
                MessageBox.Show("Thêm account thành công!");
            } else
            {
                MessageBox.Show("Thêm account thất bại");
            }
            LoadAccount();
        }

        void EditAccount(string userName, string showName, int type)
        {
            if (AccountDAO.Instance.UpdateAccount(userName, showName, type))
            {
                MessageBox.Show("Cập nhật account thành công!");
            }
            else
            {
                MessageBox.Show("Cập nhật account thất bại");
            }
            LoadAccount();
        }

        void DeleteAccount(string userName)
        {
            if (loginAccount.UserName.Equals(userName))
            {
                MessageBox.Show("Không thể xoá ! Vì Account đang được Login");
                return;
            }
                
            if (AccountDAO.Instance.DeleteAccount(userName))
            {
                MessageBox.Show("Xoá account thành công!");
            }
            else
            {
                MessageBox.Show("Xoá nhật account thất bại");
            }
            LoadAccount();
        }

        void ResetPass(string userName)
        {
            if (AccountDAO.Instance.ResetPassword(userName))
            {
                MessageBox.Show("Reset password thành công!");
            }
            else
            {
                MessageBox.Show("Reset password thất bại");
            }
        }

        #endregion

        #region Events
        private void btnShowTableFood_Click(object sender, EventArgs e)
        {
            LoadlistTableFood();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult exitHome = MessageBox.Show(
                "Bạn có chắc chắn muốn thoát ?",
                "Thông báo",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if (exitHome == DialogResult.Yes)
                Close();
        }

        private void btnShowCategory_Click(object sender, EventArgs e)
        {
            LoadlistCategory();
        }
        private void btnAddAccount_Click(object sender, EventArgs e)
        {
            string userName = txtUserName.Text;
            string showName = txtShowName.Text;
            int type = (int)nmAccountType.Value;

            AddAccount(userName, showName, type);
        }

        private void btnDeleteAccount_Click(object sender, EventArgs e)
        {
            string userName = txtUserName.Text;

            DeleteAccount(userName);
        }

        private void btnEditAccount_Click(object sender, EventArgs e)
        {
            string userName = txtUserName.Text;
            string showName = txtShowName.Text;
            int type = (int)nmAccountType.Value;

            EditAccount(userName, showName, type);
        }

        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            string userName = txtUserName.Text;

            ResetPass(userName);
        }
        private void btnShowAccount_Click(object sender, EventArgs e)
        {
            LoadAccount();
        }

        private void btnSearchFood_Click(object sender, EventArgs e)
        {
            foodList.DataSource = SearchFoodByName(txtSearchFoodName.Text);
        }

        private void btnSearchCategory_Click(object sender, EventArgs e)
        {
            categoryList.DataSource = SearchCategoryByName(txtSearchCategory.Text);
        }


        private void btnShowFood_Click(object sender, EventArgs e)
        {
            LoadlistFood();
        }

        private void txtFoodID_TextChanged(object sender, EventArgs e)
        {
            
                if (dtgvFood.SelectedCells.Count > 0)
                {
                    int id = (int)dtgvFood.SelectedCells[0].OwningRow.Cells["idCategory"].Value;  // Các lấy dữ liệu từ dataGridView ra

                    Category category = CategoryDAO.Instance.GetCategoryByID(id);

                    cbbFoodCategory.SelectedItem = category;

                    int index = -1;
                    int i = 0;
                    foreach (Category item in cbbFoodCategory.Items)
                    {
                        if (item.ID == category.ID)
                        {
                            index = i;
                            break;
                        }
                        i++;
                    }

                    cbbFoodCategory.SelectedIndex = index;
                }
            
        }

        private void btnAddFood_Click(object sender, EventArgs e)
        {
            string name = txtFoodName.Text;
            int categoryID = (cbbFoodCategory.SelectedItem as Category).ID;
            float price = (float)nmFoodPrice.Value;

            if (FoodDAO.Instance.InsertFood(name, categoryID, price))
            {
                MessageBox.Show("Thêm món thành công!");
                LoadlistFood();
                if (insertFood != null)
                    insertFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Đã có lỗi khi thêm món!");
            }
        }

        private void btnDeleteFood_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txtFoodID.Text);

            if (FoodDAO.Instance.DeleteFood(id))
            {
                MessageBox.Show("Xoá món thành công!");
                LoadlistFood();
                if (deleteFood != null)
                    deleteFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Đã có lỗi khi Xoá món!");
            }
        }

        private void btnEditFood_Click(object sender, EventArgs e)
        {
            string name = txtFoodName.Text;
            int categoryID = (cbbFoodCategory.SelectedItem as Category).ID;
            float price = (float)nmFoodPrice.Value;
            int id = Convert.ToInt32(txtFoodID.Text);

            if (FoodDAO.Instance.UpdateFood(id, name, categoryID, price))
            {
                MessageBox.Show("Cập nhật món thành công!");
                LoadlistFood();
                if (updateFood != null)
                    updateFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Đã có lỗi khi cập nhật món!");
            }
        }
        private void btnViewInvoice_Click(object sender, EventArgs e)
        {
            LoadListInvoiceByDate(dtpkFromDate.Value, dtpkToDate.Value);
        }

        // Event cho món

        private event EventHandler insertFood;
        public event EventHandler InsertFood
        {
            add { insertFood += value; }
            remove { insertFood -= value;}
        }

        private event EventHandler deleteFood;
        public event EventHandler DeleteFood
        {
            add { deleteFood += value; }
            remove { deleteFood -= value; }
        }

        private event EventHandler updateFood;
        public event EventHandler UpdateFood
        {
            add { updateFood += value; }
            remove { updateFood -= value; }
        }
        #endregion

    }
}
