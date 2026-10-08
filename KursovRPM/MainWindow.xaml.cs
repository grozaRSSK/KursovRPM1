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
using Microsoft.Win32;

namespace KursovRPM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Task> _tasks = new List<Task>();
        public MainWindow()
        {
            InitializeComponent();
            Window_Initialized();
        }

        private void Window_Initialized()
        {
            Avtorizacia f = new Avtorizacia();
            f.ShowDialog();
            if (Data.Login == false) Close();
        }

        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {

        }


        private void Info_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (_tasks.Count == 0)
            {
                MessageBox.Show("Нет задач для экспорта.", "!ОБРАТИТЕ ВНИМАНИЕ!");
                return;
            }
            TaskExport.ExportToJson(_tasks);
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Fromblank f = new Fromblank();
            f.Owner = this;
            f.ShowDialog();

        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.DefaultExt = ".json";
            open.Filter = "JSON файлы (*.json)|*.json|Все файлы (*.*)|*.*";
            open.FilterIndex = 2;
            open.Title = "Импорт задач из JSON";

            if (open.ShowDialog() == true)
            {
                try
                {
                    if (TaskExport.ImportFromJson(open.FileName, out List<Task> loadedTasks))
                    {
                        _tasks = loadedTasks;
                        MessageBox.Show($"Задачи успешно загружены из файла:\n{open.FileName}", "Успех");
                    }
                    else
                    {
                        MessageBox.Show("Не удалось загрузить файл. Возможно, файл поврежден или имеет неверный формат.", "Ошибка");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при загрузке файла: {ex.Message}", "Ошибка");
                }
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}