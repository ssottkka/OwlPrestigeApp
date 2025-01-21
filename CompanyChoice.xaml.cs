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

namespace OwlPrestigeApp
{
    /// <summary>
    /// Логика взаимодействия для CompanyChoice.xaml
    /// </summary>
    public partial class CompanyChoice : Window
    {
        private int _currentUserId; // ID авторизованного пользователя

        // Конструктор с параметром userId
        public CompanyChoice(int userId)
        {
            InitializeComponent();
            _currentUserId = userId;
            LoadUserCompanies(); // Загружаем компании, в которых состоит пользователь
        }

        // Метод для загрузки компаний пользователя
        private void LoadUserCompanies()
        {
            using (var db = new OwlPrestigeEntities())
            {
                // Получаем список компаний, в которых состоит пользователь
                var userCompanies = db.ПользователиКомпании
                    .Where(uc => uc.IDПользователя == _currentUserId) // Фильтруем по ID пользователя
                    .Select(uc => uc.Компании) // Выбираем компании
                    .ToList();

                // Скрываем кнопки для компаний, в которых пользователь не состоит
                Owl.Visibility = userCompanies.Any(c => c.НазваниеКомпании == "СОВА") ? Visibility.Visible : Visibility.Collapsed;
                Prestige.Visibility = userCompanies.Any(c => c.НазваниеКомпании == "Престиж") ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void OwlButton_Click(object sender, RoutedEventArgs e)
        {
            // Передаем ID пользователя и ID компании "СОВА" в окно задач
            PRESTIGE.EmployPrestigeWindow employPrestigeWindow = new PRESTIGE.EmployPrestigeWindow(_currentUserId, 1); // 1 - ID компании "СОВА"
            this.Close();
            employPrestigeWindow.Show();
        }

        private void PrestigeButton_Click(object sender, RoutedEventArgs e)
        {
            // Передаем ID пользователя и ID компании "Престиж" в окно задач
            PRESTIGE.EmployPrestigeWindow employPrestigeWindow = new PRESTIGE.EmployPrestigeWindow(_currentUserId, 2); // 2 - ID компании "Престиж"
            this.Close();
            employPrestigeWindow.Show();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            this.Close();
            main.Show();
        }
    }
}
