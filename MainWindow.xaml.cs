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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OwlPrestigeApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        OwlPrestigeEntities db = new OwlPrestigeEntities();

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text;
            string password = PasswordTextBox.Password;

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Пожалуйста, введите логин и пароль.");
                return;
            }
            var user =  db.Пользователи.FirstOrDefault(x => x.Пароль == password && x.Логин == login);
            if (user == null)
            {
                MessageBox.Show("Неверный логин или пароль.");
                return;
            }
            CompanyChoice companyChoice = new CompanyChoice(user.IDПользователя);



            switch (user.IDРоли)
            {

                case 1:
                    this.Close();
                    companyChoice.Show();
                    break;
                case 2:
                    this.Close();
                    companyChoice.Show();
                    break;
                case 3:
                    this.Close();
                    companyChoice.Show();
                    break;
                default:
                    MessageBox.Show("Роль не определена.");
                    break;
            }
        }
    }
}
