using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeFancy.DAO
{
    public class DataProvider
    {
        #region Design patern Singleton
        private static DataProvider instance;
        public static DataProvider Instance
        {
            get { if (instance == null) instance = new DataProvider(); return DataProvider.instance; }
            private set { DataProvider.instance = value; }
        }

        // Đảm bảo bên ngoài ko tác động đc, chỉ lấy ra
        // Chỉ duy I có 1 thể hiện của DataProvider tồn tại trong chương trình
        private DataProvider() {}
        #endregion

        private string connectionSTR = "Data Source=QUOCBAO\\BRIANSQLSERVER;Initial Catalog=CoffeeFancy;Integrated Security=True";

        public DataTable ExecuteQuery(string query, object[] parameter = null)
        {
            // Khong su dung command de excute truc tiep
            DataTable data = new DataTable();

            // Using giup giai phong bo nho cho du co loi trong khoi lenh
            using (SqlConnection connection = new SqlConnection(connectionSTR))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);

                if (parameter != null )
                {
                    string[] ListPara = query.Split(' ');

                    int i = 0;
                    foreach (string item in ListPara)
                    {
                        if (item.Contains('@'))
                        {
                            command.Parameters.AddWithValue(item, parameter[i]);
                            i++;
                        }
                    }
                }

                SqlDataAdapter adapter = new SqlDataAdapter(command);

                adapter.Fill(data);

                connection.Close();

            }

            return data;

        }

        // Tra ve trang thai so dong thanh cong khi (INSERT - UPDATE - DELETE)
        public int ExecuteNonQuery(string query, object[] parameter = null)
        {

            int data = 0;

            // Using giup giai phong bo nho cho du co loi trong khoi lenh
            using (SqlConnection connection = new SqlConnection(connectionSTR))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);

                if (parameter != null)
                {
                    string[] ListPara = query.Split(' ');

                    int i = 0;
                    foreach (string item in ListPara)
                    {
                        if (item.Contains('@'))
                        {
                            command.Parameters.AddWithValue(item, parameter[i]);
                            i++;
                        }
                    }
                }
                
                data = command.ExecuteNonQuery();

                connection.Close();

            }

            return data;

        }

        // Dem so luong khi (SELECT * ...) o dau tien trong dataset
        public object ExecuteScalar(string query, object[] parameter = null)
        {

            object data = 0;

            // Using giup giai phong bo nho cho du co loi trong khoi lenh
            using (SqlConnection connection = new SqlConnection(connectionSTR))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);

                if (parameter != null)
                {
                    string[] ListPara = query.Split(' ');

                    int i = 0;
                    foreach (string item in ListPara)
                    {
                        if (item.Contains('@'))
                        {
                            command.Parameters.AddWithValue(item, parameter[i]);
                            i++;
                        }
                    }
                }

                data = command.ExecuteScalar();

                connection.Close();

            }

            return data;

        }

    }
}
