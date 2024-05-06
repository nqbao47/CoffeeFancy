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
        public fAdmin()
        {
            InitializeComponent();
            LoadMain();
        }

        #region Methods
        void LoadMain()
        {
            dtgvFood.DataSource = foodList;

            LoadDateTimePickerInvoice();
            LoadListInvoiceByDate(dtpkFromDate.Value, dtpkToDate.Value);
            LoadlistFood();
            LoadCategoryIntoCbb(cbbFoodCategory);
            AddFoodBinding();
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
        #endregion

        #region Events
        private void btnViewInvoice_Click(object sender, EventArgs e)
        {
            LoadListInvoiceByDate(dtpkFromDate.Value, dtpkToDate.Value);
        }

        #endregion

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
                int i = 0 ;
                foreach (Category item in cbbFoodCategory.Items)
                {
                    if(item.ID == category.ID)
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
            
            if(FoodDAO.Instance.InsertFood(name, categoryID, price))
            {
                MessageBox.Show("Thêm món thành công!");
                LoadlistFood();
            } else
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
            }
            else
            {
                MessageBox.Show("Đã có lỗi khi cập nhật món!");
            }
        }
    }
}
