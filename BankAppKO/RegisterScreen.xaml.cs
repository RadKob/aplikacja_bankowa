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
    /// Logika interakcji dla klasy RegisterScreen.xaml
    /// </summary>
    public partial class RegisterScreen : Window
    {
        RegisterModel regM = new RegisterModel();
        public RegisterScreen()
        {
            InitializeComponent();
            DataContext = regM;
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            regM.RegisterAction();
            txtPesel.Clear();
            txtName.Clear();
            txtSurname.Clear();
            txtMail.Clear();
        }
        private void LabelRegister_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            LoginScreen logs = new LoginScreen();
            logs.Show();
            this.Close();
        }
    }
}
