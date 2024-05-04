using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeFancy.DTO
{
    public class InvoiceInfo
    {
        public InvoiceInfo(int id, int invoiceId, int foodID, int count) { 
            this.ID = id;
            this.InvoiceID = invoiceId;
            this.FoodID = foodID;
            this.Count = count;
        }

        public InvoiceInfo(DataRow row)
        {
            this.ID = (int)row["id"];
            this.InvoiceID = (int)row["idinvoice"];
            this.FoodID = (int)row["idfood"];
            this.Count = (int)row["count"];
        }

        private int count;
        private int foodID;
        private int invoiceID;
        private int iD;

        public int ID { get => iD; set => iD = value; }
        public int InvoiceID { get => invoiceID; set => invoiceID = value; }
        public int FoodID { get => foodID; set => foodID = value; }
        public int Count { get => count; set => count = value; }
    }
}
