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
    /// Logika interakcji dla klasy MainScreen.xaml
    /// </summary>
    public partial class MainScreen : Window
    {
        public string Login { private set; get; }
        public string DisplayUsername { private set; get; }
        public string DisplayUsersurname { private set; get; }

        private MainModel mainM = new MainModel();
        public MainScreen(string login, string displayUsername, string displayUsersurname)
        {
            Login = login;
            DisplayUsername = displayUsername;
            DisplayUsersurname = displayUsersurname;

            InitializeComponent();

            // Set values on the screen.
            txtDisplayUsername.Text = DisplayUsername;
            txtDisplayUsersurname.Text = DisplayUsersurname;

            DataGrid_SelectionChanged();
            SetTimer();
        }
        public void btnCreateAccount_Click(object sender, RoutedEventArgs e)
        {
            mainM.AccountCreator(txtDisplayUsername.Text.ToString(), txtDisplayUsersurname.Text.ToString());
        }
        public void btnDeleteAccount_Click(object sender, RoutedEventArgs e)
        {
            mainM.AccountPin = txtAccountPin.Text.ToString();
            mainM.DeleteAccount();
            txtAccountPin.Clear();
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
                command.CommandText = "Select [account_number],balance from [Bank_account] left join [User] on [Bank_account].pesel=[User].pesel where login like @login";
                command.Parameters.AddWithValue("@login", Login);
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
        private void btnMoreAccount_Click(object sender, RoutedEventArgs e)
        {
            mainM.AccountPin = txtAccountPin.Text.ToString();
            mainM.GotoAccountPage();
            txtAccountPin.Clear();
        }
    }
}
