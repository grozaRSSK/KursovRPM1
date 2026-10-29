using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace KursovRPM
{
    /// <summary>
    /// Логика взаимодействия для Fromblank.xaml
    /// </summary>
    public partial class Fromblank : Window
    {
        public Fromblank()
        {
            InitializeComponent();
            Window_Loaded();
        }

        Task _task;

        private void Window_Loaded()
        {
            if (Data.task == null)
            {
                WindowAddEditTask.Title = "Добавление записи";
                addButton.Content = "Добавить";
                _task = new Task();
            }
            else
            {
                WindowAddEditTask.Title = "Редактировние записи";
                addButton.Content = "Сохранить";
            }
            WindowAddEditTask.DataContext = _task;
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(TitleTextBox.Text))
                errors.AppendLine("Введите название");
            if (string.IsNullOrWhiteSpace(DescriptionTextBox.Text))
                errors.AppendLine("Введите описание");
            if (FilterComboBox.SelectedItem == null)
                errors.AppendLine("Выберите категорию");
            if (TaskDatePicker.SelectedDate == null)
                errors.AppendLine("Выберите дату");
            if (HighPriorityRadio.IsChecked != true && LowPriorityRadio.IsChecked != true)
                errors.AppendLine("Выберите статус");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            this.Close();
        }

        private void resizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
