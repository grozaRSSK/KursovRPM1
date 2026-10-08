using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;

namespace KursovRPM
{
    public class TaskExport
    {
        /// <summary>
        /// Экспортирует список задач в файл JSON
        /// </summary>
        /// <param name="tasks">Список задач для экспорта</param>
        public static void ExportToJson(List<Task> tasks)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.DefaultExt = ".json";
            save.Filter = "JSON файлы (*.json)|*.json|Все файлы (*.*)|*.*";
            save.FilterIndex = 2;
            save.Title = "Экспорт задач в JSON";

            if (save.ShowDialog() == true)
            {
                try
                {
                    JsonSerializerOptions options = new JsonSerializerOptions();

                    options.WriteIndented = true; // отступы
                    options.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping; // русские буквы

                    string json = JsonSerializer.Serialize(tasks, options);

                    File.WriteAllText(save.FileName, json, Encoding.UTF8); // сохранение в UTF8

                    MessageBox.Show($"Всё успешно экспортировано:\n{save.FileName}", "Успех!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при экспорте: {ex.Message}", "Неудача!");
                }
            }
        }

        /// <summary>
        /// Импортирует список задач из файла JSON
        /// </summary>
        /// <param name="filePath">Путь к файлу JSON</param>
        /// <param name="tasks">Сюда будет записан список задач</param>
        /// <returns>True если успешно, False если ошибка</returns>
        public static bool ImportFromJson(string filePath, out List<Task> tasks)
        {
            tasks = null; // Выходной параметр null по умолчанию

            try
            {
                // Читаем весь файл
                string json = File.ReadAllText(filePath, Encoding.UTF8);

                // Десериализуем JSON в список задач
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true // Игнорировать регистр свойств
                };

                tasks = JsonSerializer.Deserialize<List<Task>>(json, options);
                return true; // Если успешно - true
            }
            catch
            {
                return false; // При любой ошибке - false
            }
        }
    }
}
