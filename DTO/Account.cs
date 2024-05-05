using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeFancy.DTO
{
    public class Account
    {
        public Account(string userName, string showName, int type, string passWord = null) { 
            this.UserName = userName;
            this.ShowName = showName;
            this.Type = type;
            this.PassWord = passWord;
        }

        public Account(DataRow row)
        {
            this.UserName = row["userName"].ToString();
            this.ShowName = row["showName"].ToString();
            this.Type = (int)row["type"];
            this.PassWord = row["passWord"].ToString();
        }

        private int type;
        private string passWord;
        private string showName;
        private string userName;

        public string UserName { get => userName; set => userName = value; }
        public string ShowName { get => showName; set => showName = value; }
        public string PassWord { get => passWord; set => passWord = value; }
        public int Type { get => type; set => type = value; }
    }
}
