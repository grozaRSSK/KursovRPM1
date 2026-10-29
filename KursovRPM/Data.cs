using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KursovRPM
{
    public static class Data
    {
        //public static List<Task> Tasks = new List<Task>();

        //// Счётчик для Id (чтобы новые записи получали уникальный номер)
        //public static int NextId = 1;

        public static Task? task;
        public static bool Login = false;
        public static User TekUser { get; set; }
    }
}
