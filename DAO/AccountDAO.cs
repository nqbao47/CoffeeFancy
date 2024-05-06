using CoffeeFancy.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            // Mã hoá password sử dụng MD5
            byte[] temp = ASCIIEncoding.ASCII.GetBytes(passWord); // chuyển password thành mảng byte
            byte[] hasData = new MD5CryptoServiceProvider().ComputeHash(temp); // Tạo ra mảng kw sau khi mã hoá

            string hasPass = "";

            foreach(byte item in hasData)
            {
                hasPass += item;    
            }
            /*var list = hasData.ToString(); // Chuyển sang ToString để sử dụng reverse() để tăng sercurity
            list.Reverse();*/

            string query = "USP_Login @userName , @passWord";

            DataTable result = DataProvider.Instance.ExecuteQuery(query, new object[] {userName, hasPass /*list*/});

            return result.Rows.Count > 0;
        }

        public bool UpdateAccount(string userName, string displayName, string pass, string newPass)
        {
            int result = DataProvider.Instance.ExecuteNonQuery("exec USP_UpdateAccount @userName , @displayName , @password , @newPassword", new object[] {userName, displayName, pass, newPass});
            
            return result > 0;
        }

        public DataTable GetListAccount()
        {
            return DataProvider.Instance.ExecuteQuery("select UserName, ShowName, Type from Account");
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

        public bool InsertAccount(string name, string showName , int type)
        {
            string query = string.Format("insert dbo.Account (UserName , ShowName, Type, PassWord) values (N'{0}', N'{1}', {2}, N'{3}')", name, showName, type, "1962026656160185351301320480154111117132155");


            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool UpdateAccount(string name, string showName, int type)
        {
            string query = string.Format("update dbo.Account set ShowName = N'{1}', Type = {2} where UserName = N'{0}'", name, showName, type);

            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool DeleteAccount(string name)
        {
            string query = string.Format("Delete Account where UserName = N'{0}'", name);

            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool ResetPassword(string name)
        {
            string query = string.Format("update Account set PassWord = N'1962026656160185351301320480154111117132155' where UserName = N'{0}'", name);

            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        #endregion

    }
}
