using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot
{
    public class DBEmployee
    {
        private const string _path = "info.txt";
       
        public Dictionary<string, List<DateTime>> GetAllEmployee()//получаем список из файла
        {
            try
            {
                var dic = new Dictionary<string,List<DateTime>>();
                DateTime xDate;
                DateTime yDate;
                
                if (File.Exists(_path))
                {
                    foreach(var line in File.ReadAllLines(_path))
                    {
                        var words = line.Split('|');
                        if(words.Length ==3)
                        {
                            List<DateTime> dateTimes = new List<DateTime>();
                            xDate = DateTime.ParseExact(words[1], "dd.MM.yyyy", null);
                            dateTimes.Add(xDate);
                            yDate = DateTime.ParseExact(words[2], "dd.MM.yyyy", null);
                            dateTimes.Add(yDate);
                            dic.Add(words[0], dateTimes);
                        }
                    }
                }
                return dic;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Не удалось считать файл{ex.Message}");
                return new Dictionary<string,List <DateTime>>();
            }
        }

        public void AddEmployee(string name, string data) 
        {
            try
            {
                using(var writer = new StreamWriter(_path, true))
                {
                    writer.WriteLine($"{name}|{data}");
                }
            }
            catch
            {
                Console.WriteLine($"Не удалось добавить сотрудника {name} в базу");
            }
        
        } 

        public void ChangeEmployee(Dictionary<string,List<DateTime>> dic, string name)
        {
            try
            {
                using (var writer = new StreamWriter(_path, false))
                {
                    foreach (var item in dic)
                    {

                        writer.WriteLine($"{item.Key}|{item.Value[0].ToShortDateString()}|{item.Value[1].ToShortDateString()}");
                    }
                }
            }
            catch
            {
                Console.WriteLine($"Не удалось изменить данные {name} в базе");
            }
        }
    }
}
