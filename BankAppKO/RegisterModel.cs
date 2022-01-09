using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BankAppKO
{
    class RegisterModel
    {
        public long Pesel { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Mail { get; set; }

        public void RegisterAction()
        {
            string connection = @"Data Source=DESKTOP-KG6NVAH\SQLTEST;Initial Catalog=bank;Persist Security Info=True;Integrated Security=SSPI";
            //string connection = @"Data Source=S213-019\SQLEXPRESS;Initial Catalog=bank;Persist Security Info=True;Integrated Security=SSPI";
            SqlConnection sqlc = new SqlConnection(connection);
            try
            {
                sqlc.Open();
                string query = "Insert into [User] (pesel,name,surname,[e-mail],login,haslo) values (@pesel,@name,@surname,@mail,@login,@haslo)";
                SqlCommand command = new SqlCommand(query,sqlc);
                if (Pesel.ToString().Length == 11 && Name != null && Surname != null)
                { 
                    command.Parameters.AddWithValue("@pesel",Pesel);
                    command.Parameters.AddWithValue("@name",Name);
                    command.Parameters.AddWithValue("@surname",Surname);
                    command.Parameters.AddWithValue("@mail",Mail);
                    // generowanie loginu i hasla
                    string login = Name.Substring(0, 3) + Surname.Substring(0, 3) + Pesel.ToString().Substring(8, 3);
                    long haslo = Pesel;
                    command.Parameters.AddWithValue("@login",login);
                    command.Parameters.AddWithValue("@haslo",haslo);
                }
                else
                {
                    MessageBox.Show("Name and Surname can't be empty and Pesel must have 11 characters.");
                }
                using (SqlDataReader reader = command.ExecuteReader()){}
                MessageBox.Show("Succes.");
                sqlc.Close();
            }
            catch (Exception ex) { MessageBox.Show("Not succes."); }
            finally { sqlc.Close(); }
        }
    }
}
