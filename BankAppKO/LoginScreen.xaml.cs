using System;
using System.Collections.Generic;
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

namespace BankAppKO
{
    /// <summary>
    /// Logika interakcji dla klasy LoginScreen.xaml
    /// </summary>
    public partial class LoginScreen : Window
    {
        LoginModel logM = new LoginModel();
        public LoginScreen()
        {
            InitializeComponent();
            DataContext = logM;
        }
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            logM.LoginAction();
            txtUsername.Clear();
            txtPassword.Clear();
        }
        private void LabelLogin_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            RegisterScreen regs = new RegisterScreen();
            regs.Show();
            this.Close();
        }
    }
}
