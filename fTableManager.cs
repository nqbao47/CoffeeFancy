﻿using CoffeeFancy.DAO;
using CoffeeFancy.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoffeeFancy
{
    public partial class fTableManager : Form
    {
        public fTableManager()
        {
            InitializeComponent();

            LoadTable();
        }

        #region METHOD
        void LoadTable()
        {
            List<Table> tableList =  TableDAO.Instance.LoadTableList();

            foreach (Table item in tableList)
            {
               Button btn = new Button() { Width = TableDAO.TableWidth, Height = TableDAO.TableHeight };
               btn.Text = item.Name + Environment.NewLine + item.Status;
               
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
        #endregion

        #region EVENTS
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
        #endregion
    }
}
