using System.IO;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
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
            Window_Initialized1();
            LoadDBInListView();
        }

        private void LoadDBInListView()
        {

            using (StreamReader rdr = new StreamReader("users.json"))
            {
                int selectedIndex = lwZadach.SelectedIndex;

                string json = rdr.ReadToEnd();
                List<User> allUsers = JsonSerializer.Deserialize<List<User>>(json);

                List<Task> _tsk = new List<Task>();
                if (allUsers != null && Data.TekUser != null)
                {
                    User me = allUsers.FirstOrDefault(u => u.Username == Data.TekUser.Username);
                    if (me != null && me.Tasks != null)
                    {
                        _tsk = me.Tasks;
                    }
                }

                lwZadach.ItemsSource = _tsk;

                if (selectedIndex != -1)
                {
                    if (selectedIndex >= lwZadach.Items.Count) selectedIndex--;
                    lwZadach.SelectedIndex = selectedIndex;
                    lwZadach.ScrollIntoView(lwZadach.SelectedItem);
                }
                lwZadach.Focus();
            }
        }

        private void Window_Initialized1()
        {
            Avtorizacia f1 = new Avtorizacia();
            f1.ShowDialog();
            if (Data.Login == false) Close();
        }



        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы точно хотите выйти?", "Выход",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                SaveCurrentUserTasks();

                Data.TekUser = null;
                Data.Login = false;

                _tasks.Clear();
                lwZadach.ItemsSource = null;

                MessageBox.Show("Вы вышли из аккаунта", "Выход");

                this.Hide();

                Avtorizacia f1 = new Avtorizacia();
                f1.ShowDialog();

                if (Data.Login == true)
                {
                    LoadDBInListView();
                    this.Show();
                }
                else
                {
                    this.Close();
                }
            }
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<Task> listItem = (List<Task>)lwZadach.ItemsSource;

            var filtered = listItem.Where(p => p.Title.Contains(tbSearch.Text));

            if (filtered.Count() > 0)
            {
                var item = filtered.First();
                lwZadach.SelectedItem = item;
                lwZadach.ScrollIntoView(item);
                //lwZadach.Focus();
            }
        }

        private void tbCategory_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbCategory.Text != "")
            {
                string json = File.ReadAllText("users.json", Encoding.UTF8);
                var allUsers = JsonSerializer.Deserialize<List<User>>(json);

                var me = allUsers.FirstOrDefault(u => u.Username == Data.TekUser.Username);

                var filtered = me.Tasks.Where(p => p.Title.Contains(tbCategory.Text));

                lwZadach.ItemsSource = filtered.ToList();
            }
            else
            {
                LoadDBInListView();
            }
        }

        private void Info_Click(object sender, RoutedEventArgs e)
        {
            Info f = new Info();
            f.Owner = this;
            f.ShowDialog();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Open_Click(object sender, RoutedEventArgs e)
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
            Data.task = null;
            Fromblank f = new Fromblank();
            f.Owner = this;
            f.ShowDialog();
            LoadDBInListView();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (lwZadach.SelectedItem != null)
            {
                Data.task = (Task)lwZadach.SelectedItem;
                Fromblank f = new Fromblank();
                f.Owner = this;
                f.ShowDialog();
                LoadDBInListView();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result;
            result = MessageBox.Show("Удалить запись?", "Удаление записи",
                MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    Task row = (Task)lwZadach.SelectedItem;
                    if (row != null)
                    {
                        string json = File.ReadAllText("users.json");
                        List<User> allUsers = JsonSerializer.Deserialize<List<User>>(json);

                        for (int i = 0; i < allUsers.Count; i++)
                        {
                            if (allUsers[i].Username == Data.TekUser.Username)
                            {
                                allUsers[i].Tasks.Remove(row);
                                break;
                            }
                        }
                        string updatedJson = JsonSerializer.Serialize(allUsers);
                        File.WriteAllText("users.json", updatedJson);

                        LoadDBInListView();
                    }
                    else
                    {
                        MessageBox.Show("Выберите запись для удаления!", "Внимание");
                    }
                }
                catch
                {
                    MessageBox.Show("Ошибка удаления");
                }
            }
            else lwZadach.Focus();
        }


        private void SaveCurrentUserTasks()
        {
            if (Data.TekUser != null)
            {
                try
                {
                    List<User> allUsers = new List<User>();

                    if (File.Exists("users.json"))
                    {
                        string jsonText = File.ReadAllText("users.json", Encoding.UTF8);
                        allUsers = JsonSerializer.Deserialize<List<User>>(jsonText);

                        if (allUsers == null)
                        {
                            allUsers = new List<User>();
                        }
                    }

                    bool found = false;
                    for (int i = 0; i < allUsers.Count; i++)
                    {
                        if (allUsers[i].Username == Data.TekUser.Username)
                        {
                            allUsers[i].Tasks = Data.TekUser.Tasks; // сохраняем актуальные задачи
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        allUsers.Add(Data.TekUser);
                    }

                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        WriteIndented = true,
                        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                    };

                    string newJson = JsonSerializer.Serialize(allUsers, options);
                    File.WriteAllText("users.json", newJson, Encoding.UTF8);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка");
                }
            }
        }
    }
}