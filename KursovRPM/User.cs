using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KursovRPM
{
    public class User
    {
        public string Username { get; set; } // имя пользователя
        public string Password { get; set; } // пароль пользователя
        public List<Task> Tasks { get; set; } = new List<Task>(); // задачи 
    }
}
