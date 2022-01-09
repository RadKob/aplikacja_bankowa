using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BankAppKO
{
    class AccountModel
    {
        public string Account_Number_One { set; get; }
        public string Balance_in { set; get; }
        public void Przelew(string account_number, string account_NO, string balance_in)
        {
            string connection = @"Data Source=DESKTOP-KG6NVAH\SQLTEST;Initial Catalog=bank;Persist Security Info=True;Integrated Security=SSPI";
            //string connection = @"Data Source=S213-019\SQLEXPRESS;Initial Catalog=bank;Persist Security Info=True;Integrated Security=SSPI";
            SqlConnection sqlc = new SqlConnection(connection);
            try
            {
                sqlc.Open();
                string query = @"Declare @OldBalance as decimal;
                                set @OldBalance = (select balance from [Bank_account] where account_number=@account_number)
                                Declare @OldBalance2 as decimal;
                                set @OldBalance2 = (select balance from [Bank_account] where account_number=@account_NO)
                                IF @new_balance<=@OldBalance
                                BEGIN
	                                Update [Bank_account] set balance = @OldBalance - @new_balance where account_number=@account_number
	                                Update [Bank_account] set balance = @OldBalance2 + @new_balance where account_number=@account_NO
                                    Insert History(account_number,balance,account_number_two) values (@account_number,@new_balance,@account_NO)
                                END";
                SqlCommand command = new SqlCommand(query, sqlc);
                command.Parameters.AddWithValue("@account_number", account_number);
                command.Parameters.AddWithValue("@new_balance", balance_in);
                command.Parameters.AddWithValue("@account_NO", account_NO);
                SqlDataReader rdr = command.ExecuteReader();
                sqlc.Close();
            }
            catch (Exception ex) { MessageBox.Show("Not succes."); }
            finally { sqlc.Close(); }
        }
    }
}
