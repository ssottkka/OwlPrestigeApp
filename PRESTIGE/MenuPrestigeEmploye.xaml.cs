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

namespace OwlPrestigeApp.PRESTIGE
{
    /// <summary>
    /// Логика взаимодействия для MenuPrestigeEmploye.xaml
    /// </summary>
    public partial class MenuPrestigeEmploye : Window
    {
        private int _currentUserId;
        private int _currentCompanyId;
        public MenuPrestigeEmploye(int userId, int companyId)
        {
            InitializeComponent();
            _currentUserId = userId;
            _currentCompanyId = companyId;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(_currentCompanyId.ToString());
            MessageBox.Show(_currentUserId.ToString());
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MessagesButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CompletedTasksButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MyObjectsButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
