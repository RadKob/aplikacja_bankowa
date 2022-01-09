using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace BankAppKO
{
    /// <summary>
    /// Logika interakcji dla klasy AccountPage.xaml
    /// </summary>
    public partial class AccountPage : Window
    {
        public int Pin { private set; get; }
        public string Account_number { private set; get; }

        private AccountModel accountModel = new AccountModel();
        public AccountPage(int pin, string account_number)
        {
            Pin = pin;
            Account_number = account_number;
            InitializeComponent();
            DataContext = accountModel;
            txtAccountNumber.Text = Account_number;
            DataGrid_SelectionChanged();
            SetTimer();
        }
        public void DataGrid_SelectionChanged()
        {
            try
            {
                string connection = @"Data Source=DESKTOP-KG6NVAH\SQLTEST;Initial Catalog=bank;Persist Security Info=True;Integrated Security=SSPI";
                //string connection = @"Data Source=S213-019\SQLEXPRESS;Initial Catalog=bank;Persist Security Info=True;Integrated Security=SSPI";
                SqlConnection sqlc = new SqlConnection(connection);
                sqlc.Open();
                SqlCommand command = new SqlCommand();
                command.CommandText = "Select balance from [Bank_account] where account_number=@account_number";
                command.Parameters.AddWithValue("@account_number", Account_number);
                command.Connection = sqlc;
                SqlDataAdapter da = new SqlDataAdapter(command);
                DataTable dt = new DataTable("Bank_account");
                da.Fill(dt);
                g1.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public void Timer_Tick(object sender, EventArgs e)
        {
            DataGrid_SelectionChanged();
        }
        public void SetTimer()
        {
            DispatcherTimer Timer = new DispatcherTimer();
            Timer.Tick += new EventHandler(Timer_Tick);
            Timer.Interval = new TimeSpan(0, 0, 5);
            Timer.Start();
        }

        private void btnPrzelew_Click(object sender, RoutedEventArgs e)
        {
            accountModel.Account_Number_One = txtAccount_number_one.Text.ToString();
            accountModel.Balance_in = txtBalance_in.Text.ToString();
            accountModel.Przelew(Account_number,accountModel.Account_Number_One,accountModel.Balance_in);
            txtAccount_number_one.Clear();
            txtBalance_in.Clear();
        }
    }
}
