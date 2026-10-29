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

namespace KursovRPM
{
    /// <summary>
    /// Логика взаимодействия для Info.xaml
    /// </summary>
    public partial class Info : Window
    {
        public Info()
        {
            InitializeComponent();

            InfoTextBlock.Text =
               "📋  Управление задачами\n" +
               "─────────────────────────────\n\n" +
               "Приложение для ведения списка задач:\n" +
               "  • название и текстовое описание задачи\n" +
               "  • добавление тегов к задачам\n" +
               "  • экспорт задач в формате JSON\n" +
               "  • регистрация и аутентификация пользователей\n" +
               "  • фильтрация и поиск по задачам\n\n" +
               "─────────────────────────────\n" +
               "👤  Автор: Громов Андрей ИСП-41\n" +
               "📅  Год: 2026";
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
