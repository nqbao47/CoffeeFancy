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
    }
}
