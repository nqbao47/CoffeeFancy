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
            LoadDateTimePickerInvoice();
            LoadListInvoiceByDate(dtpkFromDate.Value, dtpkToDate.Value);
        }

        #region Methods
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
        #endregion

        #region Events
        private void btnViewInvoice_Click(object sender, EventArgs e)
        {
            LoadListInvoiceByDate(dtpkFromDate.Value, dtpkToDate.Value);
        }

        #endregion

    }
}
