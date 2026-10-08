using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KursovRPM
{
    public class Task
    {
        public string Title { get; set; } // название задачи
        public string Description { get; set; } // описание задачки
        public string Category { get; set; } // категория
        public List<string> Tags { get; set; } = new List<string>(); // список тэгов
        public DateTime Date { get; set; }  // дата
        public string Status { get; set; } // статус
    }
}
