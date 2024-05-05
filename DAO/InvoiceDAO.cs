using CoffeeFancy.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeFancy.DAO
{
    public class InvoiceDAO
    {
        private static InvoiceDAO instance;

        public static InvoiceDAO Instance
        {
            get { if (instance == null) instance = new InvoiceDAO(); return instance; }
            private set { InvoiceDAO.instance = value; }
        }
        private InvoiceDAO() { }

        /// <summary>
        /// Thành công: invoiceID
        /// Thất bại: -1
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int GetUncheckInvoiceIDbyTableID(int id)
        {
            /* idea
             * - Ko sử dụng đc Scalar vì trường hợp 1 (Đã thanh toán) sẽ ko tìm đc id
             * - Lấy ra số lượng - thành công hay ko? => chuyển thành 1 cái invoice
             * - Từ Invoice đó thì lấy id ra
             */
            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT * FROM dbo.Invoice WHERE idTable = " + id +" and status = 0");
            
            if(data.Rows.Count > 0)
            {
                Invoice invoice = new Invoice(data.Rows[0]);
                return invoice.ID;
            }

            return -1;
        }

        public void InsertInvoice(int id)
        {
            DataProvider.Instance.ExecuteNonQuery("exec USP_InsertInvocie @idTable", new object[] {id});

        }

        public int GetMaxIdInvoice ()
        {
            try
            {
                return (int)DataProvider.Instance.ExecuteScalar("select MAX(id) from dbo.Invoice");
            } catch
            {
                return 1;
            }
        }
    }
}
