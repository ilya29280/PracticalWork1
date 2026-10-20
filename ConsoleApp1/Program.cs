using System;
using PW1.Model;
using PW1.BusinessLogic;     
using PW1.DataAccessLayer;   

namespace PW1.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Logic logic = new Logic(new EntityRepository<Gnome>());

            

            // Тест база
            logic.AddGnome("Гимли", "Длиннобороды", "Воин", 139, 45);
            logic.AddGnome("Торин", "Длиннобороды", "Король", 195, 60);
            logic.AddGnome("Балин", "Огнебороды", "Шахтер", 210, 55);

            while (true)
            {
                Console.WriteLine("МЕНЮ УПРАВЛЕНИЯ ГНОМАМИ");
                Console.WriteLine("1. Добавить гнома");
                Console.WriteLine("2. Удалить гнома");
                Console.WriteLine("3. Показать всех гномов");
                Console.WriteLine("4. Редактировать гнома");
                Console.WriteLine("5. [Бизнес] Группировка по кланам");
                Console.WriteLine("6. [Бизнес] Поиск по профессии");
                Console.WriteLine("0. Выход");
                Console.Write("Выберите действие: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("Имя: "); string name = Console.ReadLine();
                        Console.Write("Клан: "); string clan = Console.ReadLine();
                        Console.Write("Профессия: "); string prof = Console.ReadLine();
                        Console.Write("Возраст: "); int age = int.Parse(Console.ReadLine());
                        Console.Write("Длина бороды (см): "); int beard = int.Parse(Console.ReadLine());
                        logic.AddGnome(name, clan, prof, age, beard);
                        Console.WriteLine("Гном добавлен!");
                        break;

                    case "2":
                        Console.Write("Введите ID гнома для удаления: ");
                        int delId = int.Parse(Console.ReadLine());
                        if (logic.DeleteGnome(delId)) Console.WriteLine("Удалено.");
                        else Console.WriteLine("Не найден.");
                        break;

                    case "3":
                        var all = logic.GetAllGnomes();
                        if (all.Count == 0) Console.WriteLine("Список пуст.");
                        else foreach (var g in all) Console.WriteLine(g.ToString());
                        break;

                    case "4":
                        Console.Write("ID гнома: "); int upId = int.Parse(Console.ReadLine());
                        Console.Write("Новое имя: "); string uName = Console.ReadLine();
                        Console.Write("Новый клан: "); string uClan = Console.ReadLine();
                        Console.Write("Новая профессия: "); string uProf = Console.ReadLine();
                        Console.Write("Новый возраст: "); int uAge = int.Parse(Console.ReadLine());
                        Console.Write("Новая борода: "); int uBeard = int.Parse(Console.ReadLine());
                        if (logic.UpdateGnome(upId, uName, uClan, uProf, uAge, uBeard)) Console.WriteLine("Обновлено!");
                        else Console.WriteLine("Не найден.");
                        break;

                    case "5":
                        Console.WriteLine("Группировка по кланам");
                        var grouped = logic.GroupGnomesByClan();
                        foreach (var group in grouped)
                        {
                            Console.WriteLine($"\nКлан: {group.Key}");
                            foreach (var g in group.Value) Console.WriteLine($"  - {g.Name} ({g.Profession})");
                        }
                        break;

                    case "6":
                        Console.Write("Профессия для поиска: ");
                        string searchProf = Console.ReadLine();
                        var filtered = logic.GetGnomesByProfession(searchProf);
                        Console.WriteLine($"\nНайдено: {filtered.Count}");
                        foreach (var g in filtered) Console.WriteLine(g.ToString());
                        break;

                    case "0": return;
                    default: Console.WriteLine("Неверный ввод."); break;
                }
            }
        }
    }
}