using CoffeeFancy.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeFancy.DAO
{
    public class AccountDAO
    {
        #region 
        private static AccountDAO instance;

        public static AccountDAO Instance
        {
            get { if (instance == null) instance = new AccountDAO(); return instance; }
            private set { instance = value; }
        }

        private AccountDAO() { }
        #endregion

        #region Login func
        public bool Login(string userName, string passWord)
        {
            string query = "USP_Login @userName , @passWord";

            DataTable result = DataProvider.Instance.ExecuteQuery(query, new object[] {userName, passWord});

            return result.Rows.Count > 0;
        }

        public Account GetAccountByUsername(string userName)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("select * from Account where userName = '" + userName + "'");
            
            foreach (DataRow item in data.Rows)
            {
                return new Account(item);

            }

            return null;
        }

        #endregion

    }
}
