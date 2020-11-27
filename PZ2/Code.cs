using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PZ2
{
    class Program
    {
        static string studentPath = @"C:\Users\hardb\Desktop\Students.txt";
        static string teacherPath = @"C:\Users\hardb\Desktop\Teachers.txt";

        static void Main(string[] args)
        {
            University university = new University();

            #region Enter
            foreach (var student in File.ReadAllLines(studentPath).Select(student => Student.Parse(student)))
            {
                university.Add(student);
            }

            foreach (var teacher in File.ReadAllLines(teacherPath).Select(teacher => Teacher.Parse(teacher)))
            {
                university.Add(teacher);
            }
            #endregion

            bool isAnswered = false;
            bool isReverse = false;

            Console.WriteLine("Сортровать в порядке возрастания?");

            while (!isAnswered)
            {
                switch (Console.ReadLine())
                {
                    case "да":
                        isReverse = false;
                        isAnswered = true;
                        break;
                    case "нет":
                        isReverse = true;
                        isAnswered = true;
                        break;
                    default:
                        Console.WriteLine("Повторите");
                        break;
                }
            }
            university.Sort(isReverse);

            Console.WriteLine("<0> - ВЫХОД ");
            Console.WriteLine("<1> - Вывести студентов");
            Console.WriteLine("<2> - Вывести преподавателей");
            Console.WriteLine("<3> - Вывести всех");
            Console.WriteLine("<4> - Вывести всех преподавателей, название кафедры которых содержит заданный текст");
            Console.WriteLine("<5> - Поиск по фамилии");
            Console.WriteLine("<6> - удалить человека");
            Console.WriteLine("<7> - очистить консоль");

            int a = Convert.ToInt32(Console.ReadLine());

            while (a != 0)
            {
                switch (a)
                {
                    case 1:
                        {
                            if (university.Students.Any())
                                foreach (Student student in university.Students)
                                    Console.WriteLine(student);

                            else
                                Console.WriteLine("Список студентов пуст");
                            break;
                        }
                    case 2:
                        {
                            if (university.Teachers.Any())
                                foreach (Teacher teacher in university.Teachers)
                                    Console.WriteLine(teacher);

                            else
                                Console.WriteLine("Список преподавателей пуст");
                            break;
                        }
                    case 3:
                        {
                            if (university.Persons.Any())
                                foreach (IPerson pers in university.Persons)
                                    Console.WriteLine(pers);

                            else
                                Console.WriteLine("Список людей пуст");
                            break;
                        }
                    case 4:
                        {
                            Console.WriteLine("Введите ключ");
                            string key = Console.ReadLine();
                            IEnumerable<Teacher> teachers = university.FindByDepartment(key);

                            if (teachers.Any())
                                foreach (var person in teachers)
                                    Console.WriteLine(person);

                            else
                                Console.WriteLine("Соответствий не найдено");
                            break;
                        }
                    case 5:
                        {
                            Console.WriteLine("Введите фамилию для поиска");
                            string key = Console.ReadLine();
                            IEnumerable<IPerson> peoples = university.FindByLastName(key);

                            if (peoples.Any())
                                foreach (IPerson person in peoples)
                                    Console.WriteLine(person);

                            else
                                Console.WriteLine("В списке не существует людей с такой фамилией");
                            break;
                        }
                    case 6:
                        {
                            if (university.Persons.Any())
                            {
                                Console.WriteLine("Введите ФИО человека, которого вы хотите удалить из списка");
                                string namePerson = Console.ReadLine();
                                try
                                {
                                    string[] key = namePerson.Split(new char[] { ' ' });
                                    string lastName = key[0], name = key[1], patronomic = key[2];
                                    IPerson itemToDelete = university.Persons.Where(x => x.Name == name).Where(y => y.Patronomic == patronomic).Where(z => z.Lastname == lastName).First();
                                    university.Remove(itemToDelete);
                                }
                                catch
                                {
                                    Console.WriteLine("Ошибка удаления. Человека с та-ким именем в списке не существует.");
                                }
                            }
                            else
                                Console.WriteLine("Ошибка удаления. Список людей пуст.");
                            break;
                        }
                    case 7:
                        {
                            Console.Clear();
                            Console.WriteLine("<0> - ВЫХОД ");
                            Console.WriteLine("<1> - Вывести студентов");
                            Console.WriteLine("<2> - Вывести преподавателей");
                            Console.WriteLine("<3> - Вывести всех");
                            Console.WriteLine("<4> - Вывести всех преподавателей, название кафедры которых содержит заданный текст");
                            Console.WriteLine("<5> - Поиск по фамилии");
                            Console.WriteLine("<6> - удалить человека");
                            Console.WriteLine("<7> - очистить консоль");
                            break;
                        }
                    default:
                        {
                            Console.WriteLine("Ошибка ввода");
                            break;
                        }
                }
                a = Convert.ToInt32(Console.ReadLine());
            }
        }
    }
}

interface IPerson
{
    string Name { get; }
    string Patronomic { get; }
    string Lastname { get; }
    DateTime Date { get; }
    int Age { get; }
}

interface IUniversity
{
    IEnumerable<IPerson> Persons { get; }   // отсортировать в соответствии с вариантом 1-й лабы
    IEnumerable<Student> Students { get; }  // отсортировать в соответствии с вариантом 1-й лабы
    IEnumerable<Teacher> Teachers { get; }  // отсортировать в соответствии с вариантом 1-й лабы

    void Add(IPerson person);
    void Remove(IPerson person);

    IEnumerable<IPerson> FindByLastName(string lastName);
    IEnumerable<Teacher> FindByDepartment(string text);
}

public class Person
{
    protected int AgeCalculation(DateTime Date)
    {
        var today = DateTime.Today;
        int age = today.Year - Date.Year;
        if (Date > today.AddYears(-age)) age--;
        return age;
    }
}


public class Student : Person, IPerson
{

    Student(string name, string patronomic, string lastname, int year, string group, float average_rate, DateTime date)
    {
        Name = name;
        Patronomic = patronomic;
        Lastname = lastname;
        Year = year;
        Group = group;
        Average_rate = average_rate;
        Date = date;
    }

    #region inherited properties
    public string Name { get; }
    public string Patronomic { get; }
    public string Lastname { get; }
    public DateTime Date { get; }
    public int Age
    {
        get
        {
            return AgeCalculation(Date);
        }
    }
    #endregion

    #region properties
    int Year { get; }
    string Group { get; }
    float Average_rate { get; }
    #endregion

    public static Student Parse(string text)
    {
        string[] data = text.Split(' ');
        Student student = new Student(data[0], data[1], data[2], int.Parse(data[3]), data[4], float.Parse(data[5]), DateTime.Parse(data[6]));
        return student;
    }

    public override string ToString()
    {
        string data;
        data = $"{Name} {Patronomic} {Lastname}, {Date.ToString(@"dd\ MMM\ yyyy")}, {Age} y.o., {Year}, {Group}, {Average_rate}";
        return data;
    }

}
class Teacher : Person, IPerson
{
    Teacher(string name, string patronomic, string lastname, DateTime date, string departament, int experience, Position position)
    {
        Name = name;
        Patronomic = patronomic;
        Lastname = lastname;
        Date = date;
        Departament = departament;
        Experience = experience;
        this.position = position;
    }


    #region inherited properties
    public string Name { get; }
    public string Patronomic { get; }
    public string Lastname { get; }
    public DateTime Date { get; }
    public int Age
    {
        get
        {
            return AgeCalculation(Date);
        }
    }
    #endregion
    public string Departament { get; }
    public int Experience { get; }
    Position position { get; }

    public static Teacher Parse(string text)
    {
        string[] data = data = text.Split(' ');
        Teacher teacher = new Teacher(data[0], data[1], data[2], DateTime.Parse(data[3]), data[4], int.Parse(data[5]),
            (Position)Enum.Parse(typeof(Position), data[6]));
        return teacher;
    }

    public override string ToString()
    {
        string data;
        data = $"{Name} {Patronomic} {Lastname}, {Date.ToString(@"dd\ MMM\ yyyy")}, {Age} y.o., {Departament}, {Experience}, {position}.";
        return data;
    }


    enum Position
    {
        Assistant,
        Teacher,
        HeadTeacher,
        AssistantProfessor,
        Professor,
    }
}

class University : IUniversity
{
    List<IPerson> persons = new List<IPerson>();

    public IEnumerable<IPerson> Persons
    {
        get
        {
            return persons;
        }
    }
    public IEnumerable<Student> Students
    {
        get
        {
            return persons.Where(person => person.GetType() == typeof(Student)).Cast<Student>();
        }
    }
    public IEnumerable<Teacher> Teachers
    {
        get
        {
            return persons.Where(person => person.GetType() == typeof(Teacher)).Cast<Teacher>(); ;
        }
    }

    public void Add(IPerson person)
    {
        persons.Add(person);
    }
    public void Remove(IPerson person)
    {
        persons.Remove(person);
    }


    public IEnumerable<IPerson> FindByLastName(string lastName)
    {
        foreach (var person in Persons)
        {
            if (person.Lastname == lastName)
                yield return person;
        }
    }

    public IEnumerable<Teacher> FindByDepartment(string text)
    {
        foreach (var teacher in Teachers)
        {
            if (teacher.Departament.Contains(text))
                yield return teacher;
        }
    }

    public void Sort(bool isReverse)
    {
        if (isReverse)
            persons = persons.OrderByDescending(person => person.Date).ToList();
        else
            persons = persons.OrderBy(person => person.Date).ToList();
    }
}
