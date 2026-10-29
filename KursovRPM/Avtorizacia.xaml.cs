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

        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            string login = UsernameTextBox.Text.Trim();
            string password = PasswordBox.Password;

            if (login == "" || password == "")
            {
                MessageBox.Show("Введите логин и пароль!", "Ошибка");
                return;
            }

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

            Data.Login = true;
            Data.TekUser = locate;

            Close();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            var f = new Auntificaqia();
            f.Owner = this.Owner;
            f.ShowDialog();

            if (Data.Login == true)
            {
                this.Close();              
            }
            else
            {
                this.Show();               
            }
        }
    }
}