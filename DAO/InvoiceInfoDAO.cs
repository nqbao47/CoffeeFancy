using CoffeeFancy.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeFancy.DAO
{
    public class InvoiceInfoDAO
    {
        private static InvoiceInfoDAO instance;

        public static InvoiceInfoDAO Instance
        {
            get { if (instance == null) instance = new InvoiceInfoDAO(); return InvoiceInfoDAO.instance; }
            private set { InvoiceInfoDAO.instance = value; }
        } 
        private InvoiceInfoDAO() { }

        // Bất cứ InvoiceInfo nào dính với FoodID đang xoá đều phải xoá hết
        public void DeleteInvoiceInfoByFoodID(int id) {
            DataProvider.Instance.ExecuteQuery("delete dbo.InvoiceInfo WHERE idFood =" + id);
        }

        public List<InvoiceInfo> GetListInvoiceInfo(int id) { 
            List<InvoiceInfo> listInvoiceInfo = new List<InvoiceInfo>();

            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT * FROM dbo.InvoiceInfo WHERE idInvoice =" + id);

            foreach (DataRow item in data.Rows)
            {
                InvoiceInfo info = new InvoiceInfo(item);
                listInvoiceInfo.Add(info);
            }

            return listInvoiceInfo;
        }

        /* Case khi insert Invoice
         * 1 - If table ko tồn tại Invoice thì => tạo ra Invoice mới => insert info vô Invoice
         * 2 - If table tồn tại Invoice rồi => insert info vô Invoice (!)Case Food CHƯA tồn tại
         * 3 - If table tồn tại Invoice rồi => insert info vô Invoice (!)Case Food ĐÃ tồn tại => update lại data (count, price,...)
         * 
         */
        public void InsertInvoiceInfo(int idInvoice, int idFood, int count )
        {
            DataProvider.Instance.ExecuteNonQuery("exec USP_InsertInvoiceInfo @idInvoice , @idFood , @idCount ", new object[] { idInvoice, idFood, count });
        }
    }
}
