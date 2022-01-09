using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BankAppKO
{
    class LoginModel
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public void LoginAction()
        {
            string connection = @"Data Source=DESKTOP-KG6NVAH\SQLTEST;Initial Catalog=bank;Persist Security Info=True;Integrated Security=SSPI";
            //string connection = @"Data Source=S213-019\SQLEXPRESS;Initial Catalog=bank;Persist Security Info=True;Integrated Security=SSPI";
            SqlConnection sqlc = new SqlConnection(connection);
            try
            {
                sqlc.Open();
                string query = "Select name, surname from [User] where login=@login and haslo=@password";
                SqlCommand command = new SqlCommand(query, sqlc);
                command.Parameters.AddWithValue("@login", Login);
                command.Parameters.AddWithValue("@password", Password);
                SqlDataReader rdr = command.ExecuteReader();
                if(rdr.Read() && rdr.HasRows)
                {
                    MainScreen main = new MainScreen(Login, rdr["name"].ToString(), rdr["surname"].ToString());
                    main.Show();
                }
                else
                {
                    MessageBox.Show("Username or password is incorrect.");
                }
                sqlc.Close();
            }
            catch (Exception ex) { MessageBox.Show("Not succes."); }
            finally { sqlc.Close(); }
        }
    }
}
