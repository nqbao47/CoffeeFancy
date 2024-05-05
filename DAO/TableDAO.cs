﻿using CoffeeFancy.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeFancy.DAO
{
    public class TableDAO
    {
        private static TableDAO instance;

        public static TableDAO Instance
        {
            get { if(instance == null ) instance = new TableDAO(); return instance; }
            private set { TableDAO.instance = value; }   
        }

        public static int TableWidth = 90;
        public static int TableHeight = 90;
        private TableDAO() { }

        public List<Table> LoadTableList()
        {
            // Trả ra danh sách table (Phải tạo ra 1 class table trong DTO)

            List<Table> tableList = new List<Table>();

            DataTable data = DataProvider.Instance.ExecuteQuery("USP_GetTableList");

            /* Cần chuyển từ DataTable -> List<Table> 
             * + Chuyển từng rows thành list 
             * => tạo hàm dựng để xử lý từ db trong class Table
             */

            foreach ( DataRow item in data.Rows )
            {
                Table table = new Table(item);
                tableList.Add(table);
            }

            return tableList;
        }

    }
}