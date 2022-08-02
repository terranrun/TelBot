using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot
{
    
    public class Employee
    {
        private DBEmployee _employee = new DBEmployee();
        private Dictionary<string,List<DateTime>> _dic;
        private List<DateTime> _dateOtpusk = new List<DateTime>();

        public Employee()
        {
            _dic = _employee.GetAllEmployee();
        }

        //public void AddEmployee(string name, string data)
        //{
        //    if(!_dic.ContainsKey(name))
        //    {
        //        oDate = DateTime.ParseExact(data, "dd.MM.yyyy", null);
        //        _dic.Add(name, oDate);
        //        _employee.AddEmployee(name, data);
        //    }
        //}

        public string ChangeEmployee(string name, string data1, string data2)
        {
            if (_dic.ContainsKey(name))
            {
                _dateOtpusk = DateSum(data1,data2);
                _dic[name] = _dateOtpusk;
                _employee.ChangeEmployee(_dic, name);
            }
            else
                return $"дата {name} отпуска не была изменена";

            return $"дата {name[1]} отпуска была изменена";
        }

        public List<DateTime> DateSum(string data1, string data2)
        {
            List<DateTime> dateTimes = new List<DateTime>();
            DateTime xDate;
            Console.WriteLine("когда вы хотите начать свой отпуск?");
            
            while(!DateTime.TryParseExact(data1, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out xDate))
            {
                Console.WriteLine("введите корректную дату dd.mm.yyyy ");
               // data1 = Console.ReadLine();
            }
            Console.WriteLine($"Ваш отпуск начинается с {xDate.ToShortDateString()}");

            dateTimes.Add(xDate);
            Console.WriteLine("сколько вы хотите потратить дней на отпуск?");

            Int32.TryParse(data2, out int day);

            if (day < 14)
            {
                Console.WriteLine("нельзя ставить отпуск меньше 14 дней");
            }
            else if(day>= 14 && day<28)
            {
                DateTime yDate = xDate.AddDays(day);
                dateTimes.Add(yDate);
                Console.WriteLine($"Ваш отпуск продлится с {xDate.ToShortDateString()} по {yDate.ToShortDateString()}");

            }
            else
                Console.WriteLine("некорректный ввод");
            return dateTimes;
        }

        public string InfoEmployeeWeekend(string[] name)
        {
            if (name.Length != 2)
                return "Неправильное количество аргументов. Их должно быть 2";
            else
            {
             return $"Отпуск сотрудника {name[1]} продлится с {InfoEmployee(name[1])[0].ToShortDateString()} по {InfoEmployee(name[1])[1].ToShortDateString()}";
            }          
        }

        public List<DateTime> InfoEmployee(string name)
        {
            if (_dic.ContainsKey(name))
            {
                return _dic[name];
            }
            else
                return _dateOtpusk;
        }

    }
}
