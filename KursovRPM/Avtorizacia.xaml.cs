using System.IO;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

namespace KursovRPM
{
    /// <summary>
    /// Логика взаимодействия для Avtorizacia.xaml
    /// </summary>
    public partial class Avtorizacia : Window
    {
        private string _usersFile = "users.json";

        private List<User> _users = new List<User>();

        public Avtorizacia()
        {
            InitializeComponent();
            Window_Activated();
            LoadUsers();
        }

        private void Window_Activated()
        {
            UsernameTextBox.Focus();
            Data.Login = false;
        }

        private void LoadUsers()
        {
            try
            {
                //создание пустого списка если нет файла 
                if (File.Exists(_usersFile) == false) 
                {
                    _users = new List<User>();
                    return;
                }

                // открытие файла json и считка всего текста в переменную 
                string text = "";
                using (StreamReader reader = new StreamReader(_usersFile, Encoding.UTF8))
                {
                    text = reader.ReadToEnd();
                }
                // превращаем текст в список объектов
                _users = JsonSerializer.Deserialize<List<User>>(text);

            }
            catch
            {
                _users = new List<User>();
            }
        }

        private void SaveUsers()
        {
            try
            {
                // нормальное форматирование с отступами
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                };
                // cпиcок => текст
                string text = JsonSerializer.Serialize(_users, options);

                // открытие и записываем в файл информацию 
                using (StreamWriter writer = new StreamWriter(_usersFile, false, Encoding.UTF8))
                {
                    writer.Write(text);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось сохранить: {ex.Message}", "Ошибка");
            }
        }

        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            string login = UsernameTextBox.Text.Trim();
            string password = PasswordBox.Password;

            if (login == "" || password == "")
            {
                MessageBox.Show("Введите логин и пароль!", "Ошибка");
                return;
            }

            //создание пустой переменной перебираем всех пока не найдём нужного
            User locate = null;
            for (int i = 0; i < _users.Count; i++)
            {
                if (_users[i].Username == login && _users[i].Password == password)
                {
                    locate = _users[i];
                    break;
                }
            }

            if (locate == null)
            {
                MessageBox.Show("Логин или пароль неверны! Повторите ввод.",
                                "Ошибка авторизации");
                PasswordBox.Clear();
                UsernameTextBox.Focus();
                return;
            }

            // вошёл, запомнили, закрыли успешно, закрываем
            Data.Login = true;
            Data.TekUser = locate;

            DialogResult = true;
            Close();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string login = UsernameTextBox.Text.Trim();
            string password = PasswordBox.Password;

            if (login == "" || password == "")
            {
                MessageBox.Show("Введите логин и пароль!", "Ошибка");
                return;
            }
            // проверка на повторение логина 
            bool est = false;
            for (int i = 0; i < _users.Count; i++)
            {
                if (_users[i].Username == login)
                {
                    est = true;
                    break;
                }
            }

            if (est == true)
            {
                MessageBox.Show("Такой логин уже есть!");
                return;
            }

            if (password.Length < 4)
            {
                MessageBox.Show("Пароль должен быть минимум 4 символа!", "Ошибка");
                return;
            }
            // создание нового пользователя
            User recent = new User();
            recent.Username = login;
            recent.Password = password;

            //добавление и сохранение 
            _users.Add(recent);
            SaveUsers();

            // вошёл, запомнили, закрыли успешно, закрываем
            Data.Login = true;
            Data.TekUser = recent;

            DialogResult = true;
            Close();
        }
    }
}