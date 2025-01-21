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
    /// Логика взаимодействия для EmployPrestigeWindow.xaml
    /// </summary>
    public partial class EmployPrestigeWindow : Window
    {
        private int _currentUserId; // ID авторизованного пользователя
        private int _currentCompanyId; // ID выбранной компании
        private List<Задачи> _tasks; // Список задач
        private List<ТипыОбъектов> _objectTypes; // Список типов объектов

        public EmployPrestigeWindow(int userId, int companyId)
        {
            InitializeComponent();
            _currentUserId = userId;
            _currentCompanyId = companyId;
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                using (var db = new OwlPrestigeEntities())
                {
                    // Загрузка задач для текущего пользователя и выбранной компании
                    _tasks = db.ИсполнителиЗадач
                        .Include("Задачи") // Включаем связанные данные о задачах
                        .Include("Задачи.Компании") // Включаем данные о компании
                        .Include("Задачи.ТипыОбъектов") // Включаем данные о типе объекта
                        .Where(exec => exec.IDПользователя == _currentUserId && exec.Задачи.IDКомпании == _currentCompanyId) // Фильтруем по ID пользователя и компании
                        .Select(exec => exec.Задачи)
                        .ToList();

                    // Загрузка типов объектов для ComboBox
                    _objectTypes = db.ТипыОбъектов.ToList();
                    TypeFilterComboBox.ItemsSource = _objectTypes;

                    // Привязка данных к DataGrid
                    TasksDataGrid.ItemsSource = _tasks.Select(task => new
                    {
                        task.IDЗадачи,
                        task.НазваниеЗадачи,
                        НазваниеОбъекта = task.Название_объекта ?? "Нет данных",
                        task.СрокВыполнения,
                        Компания = task.Компании?.НазваниеКомпании ?? "Нет данных",
                        Исполнители = task.ИсполнителиЗадач != null
                            ? string.Join(", ", task.ИсполнителиЗадач
                                .Where(exec => exec.Пользователи != null)
                                .Select(exec => $"{exec.Пользователи.Имя} {exec.Пользователи.Фамилия}"))
                            : "Нет данных"
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}");
            }
        }



        private void ClearFiltersButton_Click(object sender, RoutedEventArgs e)
        {
            // Очистка фильтров
            TypeFilterComboBox.SelectedIndex = -1;
            SearchTextBox.Text = string.Empty;
            TasksDataGrid.ItemsSource = _tasks.Select(task => new
            {
                task.IDЗадачи,
                task.НазваниеЗадачи,
                НазваниеОбъекта = task.Название_объекта ?? "Нет данных",
                task.СрокВыполнения,
                Компания = task.Компании?.НазваниеКомпании ?? "Нет данных",
                Исполнители = task.ИсполнителиЗадач != null
                    ? string.Join(", ", task.ИсполнителиЗадач
                        .Where(exec => exec.Пользователи != null)
                        .Select(exec => $"{exec.Пользователи.Имя} {exec.Пользователи.Фамилия}"))
                    : "Нет данных"
            }).ToList();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                string searchText = SearchTextBox.Text.ToLower();

                // Фильтрация задач
                var filteredTasks = _tasks
                    .Where(task => (task.НазваниеЗадачи != null && task.НазваниеЗадачи.ToLower().Contains(searchText)) ||
                                   (task.Название_объекта != null && task.Название_объекта.ToLower().Contains(searchText)) ||
                                   (task.Компании != null && task.Компании.НазваниеКомпании != null && task.Компании.НазваниеКомпании.ToLower().Contains(searchText)) ||
                                   (task.ИсполнителиЗадач != null && task.ИсполнителиЗадач.Any(exec => exec.Пользователи != null &&
                                                                                                      exec.Пользователи.Имя != null &&
                                                                                                      exec.Пользователи.Фамилия != null &&
                                                                                                      (exec.Пользователи.Имя.ToLower().Contains(searchText) ||
                                                                                                       exec.Пользователи.Фамилия.ToLower().Contains(searchText)))))
                    .ToList();

                // Обновление DataGrid
                TasksDataGrid.ItemsSource = filteredTasks.Select(task => new
                {
                    task.IDЗадачи,
                    task.НазваниеЗадачи,
                    НазваниеОбъекта = task.Название_объекта ?? "Нет данных",
                    task.СрокВыполнения,
                    Компания = task.Компании?.НазваниеКомпании ?? "Нет данных",
                    Исполнители = task.ИсполнителиЗадач != null
                        ? string.Join(", ", task.ИсполнителиЗадач
                            .Where(exec => exec.Пользователи != null)
                            .Select(exec => $"{exec.Пользователи.Имя} {exec.Пользователи.Фамилия}"))
                        : "Нет данных"
                }).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при поиске: {ex.Message}");
            }
        }

        private void TypeFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (TypeFilterComboBox.SelectedItem == null)
                {
                    // Если ничего не выбрано, показываем все задачи
                    TasksDataGrid.ItemsSource = _tasks.Select(task => new
                    {
                        task.IDЗадачи,
                        task.НазваниеЗадачи,
                        НазваниеОбъекта = task.Название_объекта ?? "Нет данных",
                        task.СрокВыполнения,
                        Компания = task.Компании?.НазваниеКомпании ?? "Нет данных",
                        Исполнители = task.ИсполнителиЗадач != null
                            ? string.Join(", ", task.ИсполнителиЗадач
                                .Where(exec => exec.Пользователи != null)
                                .Select(exec => $"{exec.Пользователи.Имя} {exec.Пользователи.Фамилия}"))
                            : "Нет данных"
                    }).ToList();
                    return;
                }

                // Получаем выбранный тип объекта
                var selectedType = (ТипыОбъектов)TypeFilterComboBox.SelectedItem;

                // Фильтруем задачи по выбранному типу объекта
                var filteredTasks = _tasks
                    .Where(task => task.ТипыОбъектов != null && task.ТипыОбъектов.IDТипаОбъекта == selectedType.IDТипаОбъекта)
                    .ToList();

                // Обновляем DataGrid
                TasksDataGrid.ItemsSource = filteredTasks.Select(task => new
                {
                    task.IDЗадачи,
                    task.НазваниеЗадачи,
                    НазваниеОбъекта = task.Название_объекта ?? "Нет данных",
                    task.СрокВыполнения,
                    Компания = task.Компании?.НазваниеКомпании ?? "Нет данных",
                    Исполнители = task.ИсполнителиЗадач != null
                        ? string.Join(", ", task.ИсполнителиЗадач
                            .Where(exec => exec.Пользователи != null)
                            .Select(exec => $"{exec.Пользователи.Имя} {exec.Пользователи.Фамилия}"))
                        : "Нет данных"
                }).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при фильтрации: {ex.Message}");
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            CompanyChoice companyChoice = new CompanyChoice(_currentUserId);
            this.Close();
            companyChoice.Show();
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            MenuPrestigeEmploye menuPrestigeEmp = new MenuPrestigeEmploye(_currentUserId, _currentCompanyId);
            this.Close();   
            menuPrestigeEmp.Show();
        }
    }
}
