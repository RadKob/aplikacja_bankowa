using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BankAppKO
{
    class MainModel
    {
        public string AccountNumber { get; set; }
        public string AccountPin { get; set; }
        // limit ilości posiadanych kont.
        public const int Limit = 3;
        public void AccountCreator(string loginUserName, string loginUserSurname)
        {
            // string łączenia z bazą danych.
            string connection = @"Data Source=DESKTOP-KG6NVAH\SQLTEST;Initial Catalog=bank;Persist Security Info=True;Integrated Security=SSPI;MultipleActiveResultSets=true";
            //string connection = @"Data Source=S213-019\SQLEXPRESS;Initial Catalog=bank;Persist Security Info=True;Integrated Security=SSPI;MultipleActiveResultSets=true";
            SqlConnection sqlc = new SqlConnection(connection);
            // generator numeru konta i zerowe saldo początkowe.
            var accountN = new Random();
            int number = Int32.Parse(accountN.Next(1, 999999).ToString("D6"));
            long balance_new_account = 0;
            try
            {
                sqlc.Open();
                // zliczamy ile kont ma zalogowany użytkownik.
                string query1 = "Select count(1) from [Bank_account] left join [User] on [User].pesel = [Bank_account].pesel where name=@name and surname=@surname";
                SqlCommand command1 = new SqlCommand(query1, sqlc);
                command1.Parameters.AddWithValue("@name", loginUserName);
                command1.Parameters.AddWithValue("@surname", loginUserSurname);
                int limit = Convert.ToInt32(command1.ExecuteScalar());
                if(limit < Limit)// jeśli liczba kont jest mniejsza od 3.
                {
                    // pobieramy pesel zalogowanego użytkownika.
                    string pesel = "";
                    string query2 = "Select pesel from [User] where name=@name and surname=@surname";
                    SqlCommand command2 = new SqlCommand(query2, sqlc);
                    command2.Parameters.AddWithValue("@name", loginUserName);
                    command2.Parameters.AddWithValue("@surname", loginUserSurname);
                    using (SqlDataReader rdr = command2.ExecuteReader())
                    {
                        if (rdr.Read() && rdr.HasRows)
                        {
                            pesel = rdr["pesel"].ToString();
                        }
                    }
                    using (SqlDataReader rdr2 = command2.ExecuteReader())
                    {
                        // tworzymy kolejne konto.
                        string query3 = "Insert into [Bank_account] ([account_number],balance,pesel,pin) values (@number,@balance,@pesel,@pin)";
                        SqlCommand command3 = new SqlCommand(query3, sqlc);
                        command3.Parameters.AddWithValue("@number", number);
                        command3.Parameters.AddWithValue("@balance", balance_new_account);
                        command3.Parameters.AddWithValue("@pesel", pesel);
                        command3.Parameters.AddWithValue("@pin", number.ToString().Substring(0, 4));
                        command3.ExecuteNonQuery();
                    }
                    // zamykamy połączenie.
                    MessageBox.Show("Succes.");
                    sqlc.Close();
                }
                // jeśli liczba kont jest większ niż 3.
                else
                {
                    MessageBox.Show("Sorry but you can have only 3 bank accounts.");
                }
            }
            catch (Exception ex) { MessageBox.Show("Not succes."); }
            finally { sqlc.Close(); }
        }
        public void DeleteAccount()
        {
            // string łączenia z bazą danych.
            string connection = @"Data Source=DESKTOP-KG6NVAH\SQLTEST;Initial Catalog=bank;Persist Security Info=True;Integrated Security=SSPI;MultipleActiveResultSets=true";
            //string connection = @"Data Source=S213-019\SQLEXPRESS;Initial Catalog=bank;Persist Security Info=True;Integrated Security=SSPI;MultipleActiveResultSets=true";
            SqlConnection sqlc = new SqlConnection(connection);
            try
            {
                sqlc.Open();
                // usuwamy konto po podaniu jego pinu.
                string query = "Delete from [Bank_account] where pin=@pin";
                SqlCommand command = new SqlCommand(query, sqlc);
                command.Parameters.AddWithValue("@pin", AccountPin);
                if (AccountNumber != "" && AccountPin != "")
                {
                    using (SqlDataReader reader = command.ExecuteReader()) { MessageBox.Show("Succes."); }
                }
                else
                {
                    MessageBox.Show("Pin number can't be empty.");
                }
                sqlc.Close();
            }
            catch (Exception ex) { MessageBox.Show("Not succes."); }
            finally { sqlc.Close(); }
        }
        public void GotoAccountPage()
        {
            int pin = 0;
            string connection = @"Data Source=DESKTOP-KG6NVAH\SQLTEST;Initial Catalog=bank;Persist Security Info=True;Integrated Security=SSPI";
            //string connection = @"Data Source=S213-019\SQLEXPRESS;Initial Catalog=bank;Persist Security Info=True;Integrated Security=SSPI";
            SqlConnection sqlc = new SqlConnection(connection);
            try
            {
                sqlc.Open();
                string query = "Select account_number, balance from [Bank_account] where pin=@pin";
                SqlCommand command = new SqlCommand(query, sqlc);
                command.Parameters.AddWithValue("@pin", AccountPin);
                SqlDataReader rdr = command.ExecuteReader();
                pin = Int32.Parse(AccountPin);
                if (rdr.Read() && rdr.HasRows)
                {
                    AccountPage accountPage = new AccountPage(pin, rdr["account_number"].ToString());
                    accountPage.Show();
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
