using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace consoleTaskList 
{
    internal class program 
    {
        static TaskList taskList {get; set;} = new TaskList();
        public static void Main(string[] args)
        {
            if (File.Exists("taskList.json")) 
            {
                using (var jsonSR = new StreamReader("taskList.json"))
                {   
                string jsonstr = jsonSR.ReadToEnd();
                taskList = JsonConvert.DeserializeObject<TaskList>(jsonstr);
                }
            }
            else{}
            bool isRunning = true;
            Console.Clear();
            while (isRunning == true) 
            {
                Console.WriteLine("Список задач версия 1.0 Copyright Павел Мельник 11-А \nСписок команд:\n1. Показать активные задачи\n2. Создать новую задачу\n0. Выйти из программы и сохранить задачи");
                char input = Console.ReadKey().KeyChar;
                switch (input)
                {
                    case '1':
                        Console.Clear();
                        if (taskList.tasks.Any())
                        {
                            SelectTask();
                        }
                        else
                        {
                            Console.WriteLine("Ваш список задач пуст");
                            ReturnToMain();
                        }
                    break;

                    case '2':
                    {
                        bool success = true;
                        Console.Clear();
                        do 
                        {
                            
                            Console.WriteLine("\tСоздание новой задачи\nВведите название для новой задачи:");
                            string newTaskName = Console.ReadLine();
                            Console.WriteLine("Введите описание для новой задачи:");
                            string newTaskContent = Console.ReadLine();
                            if (newTaskName != "" && newTaskContent != "")
                            {
                                CreateTask(taskList, newTaskName, newTaskContent);
                                Console.WriteLine("Задча успешно создана");
                                success = true;
                                ReturnToMain();
                            }
                            else
                            {
                                Console.Clear();
                                Console.WriteLine("Пожалуйста, заполните все поля. Чтобы отменить создание задачи, введите 0");
                                if (Console.ReadLine() == "0")
                                {
                                    Console.Clear();
                                    break;
                                }
                                else Console.Clear();                            
                            }
                        } while (success == false);
                        
                    }
                    break;
                    case '0':
                        isRunning = false;
                        Console.Clear();
                        using (StreamWriter jsonSW = new StreamWriter("taskList.json"))
                        {
                            jsonSW.WriteLine(JsonConvert.SerializeObject(taskList));
                        }
                    break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Неверная команда");
                        ReturnToMain();
                    break;
                }    
            }
        }
        
        public static void CreateTask(TaskList _taskList, string name, string content)
        {
            task newTask = new task(name, content);
            _taskList.tasks.Add(newTask);
        }
        public static string BoolToString(bool theBool)
        {
            if (theBool == true) return "Да";
            else return "Нет";
        }
        public static string PriorityToString(int priority)
        {
            switch (priority)
            {
                case -1: return "Высокий";
                case 0: return "Обычный";
                case 1: return "Низкий";
                default: return "Задача выполнена";
            }
        }
        public static int CharToPriority(char priority)
        {
            switch (priority)
            {
                case '1': return -1 ;
                case '2': return 0 ;
                case '3': return 1;
                default: return 0;
            }
        }
        public static void ReturnToMain()
        {
            Console.WriteLine("Нажмите любую клавишу, чтобы вернуться назад");
            Console.ReadKey();
            Console.Clear();
        }
        public static void ReturnToMain(string arg)
        {
            switch (arg)
            {
                case "silent":
                    Console.ReadKey();
                    Console.Clear();
                break;
                case "success":
                    Console.WriteLine("Операция успешно завершена! Нажмите любую клавишу, чтобы вернуться назад");
                    Console.ReadKey();
                    Console.Clear();
                break;
            }
        }
        public static void SelectAction(int taskNum)
        {
            bool end = false;
            while (end == false) 
            {
                Console.Clear();
                task _task = taskList.tasks[taskNum];
                Console.WriteLine("ВЫбранная задача:\n");
                Console.WriteLine($"Имя: {_task.name}");
                Console.WriteLine($"Описание: {_task.content}");
                Console.WriteLine($"Выполнена ли задача: {BoolToString(_task.isDone)}");
                Console.WriteLine($"Приоритет:{PriorityToString(_task.priority)}\n");

                Console.WriteLine("Доступные действия:\n");
                Console.WriteLine("1. Изменить название");
                Console.WriteLine("2. Изменить описание");
                Console.WriteLine("3. Изменить приоритет");
                Console.WriteLine("4. Изменить статус");
                Console.WriteLine("5. Удалить задачу");
                Console.WriteLine("0. Вернуться назад");
                char input = Console.ReadKey().KeyChar;
                    switch (input)
                    {
                        case '1':
                            EditTaskName(_task);
                        break;
                        case '2':
                            EditTaskContent(_task);
                        break;
                        case '3':
                            EditTaskPriority(_task);
                        break;
                        case '4':
                            SwitchTaskStatus(_task);
                        break;
                        case '5':
                            RemoveTask(_task);
                            Console.Clear();
                            end = true;
                        break;
                        case '0':
                            Console.Clear();
                            end = true;
                        break;
                        default:
                            end = true; 
                        break;
                    } 
            }
        }
        public static void SelectTask()
        {
            bool end = false;
            while(end == false)
            {
                int i = 1;
                taskList.tasks = taskList.tasks.OrderBy(a => a.priority).ToList<task>();
                foreach (task _task in taskList.tasks)
                {
                    Console.WriteLine($"{i}.\tИмя: {_task.name}");
                    Console.WriteLine($"\tОписание: {_task.content}");
                    Console.WriteLine($"\tВыполнена ли задача: {BoolToString(_task.isDone)}");
                    Console.WriteLine($"\tПриоритет:{PriorityToString(_task.priority)}\n");
                    i++;
                }
                Console.WriteLine("Введите номер задачи, чтобы с ней взаимодействовать");
                Console.WriteLine("Введите 0, чтобы вернуться назад");
                string inputTaskNum = Console.ReadLine();
                int intInputTaskNum = Convert.ToInt32(inputTaskNum);
                if (inputTaskNum == "0") 
                {
                    end = true;
                    Console.Clear();
                }
                else if (intInputTaskNum > 0 && intInputTaskNum <= taskList.tasks.Count)
                {
                    SelectAction(intInputTaskNum - 1);
                }
                else
                {
                    end = ReturnOrCancel();
                }
            } 
        }
        public static void EditTaskName(task _task)
        {
            bool end = true;
            Console.Clear();
            do {
            Console.WriteLine("\tРедактирование названия\n");
            Console.WriteLine($"Текущее название: {_task.name}");
            Console.WriteLine("Введите новое название");
            string newProp = Console.ReadLine();
            if (newProp != "")
            {
                _task.name = newProp;
                ReturnToMain("success");
            }
            else 
            {
                end = ReturnOrCancel();
            }
            } while (end == false);
        }
        public static void EditTaskContent(task _task)
        {
            bool end = true;
            Console.Clear();
            do {
            Console.WriteLine("\tРедактирование описания\n");
            Console.WriteLine($"Текущее описание: {_task.content}");
            Console.WriteLine("Введите новое описание");
            string newProp = Console.ReadLine();
            if (newProp != "")
            {
                _task.content = newProp;
                ReturnToMain("success");
            }
            else 
            {
                end = ReturnOrCancel();
            }
            } while (end == false);
        }
        public static void EditTaskPriority(task _task)
        {
            bool end = true;
            Console.Clear();
            do {
            Console.WriteLine("\tРедактирование приоритета\n");
            Console.WriteLine($"Текущий приоритет: {PriorityToString(_task.priority)}");
            Console.WriteLine("Выберите новый приоритет:\n");
            Console.WriteLine("1. Высокий");
            Console.WriteLine("2. Обычный");
            Console.WriteLine("3. Низкий");
            char newProp = Console.ReadKey().KeyChar;
            List<char> keys = new List<char>(){'1', '2', '3'};
            if (keys.Contains(newProp))
            {
                _task.priority = CharToPriority(newProp);
                _task.isDone = false;
                ReturnToMain("success");
            }
            else 
            {
                end = ReturnOrCancel();
            }
            } while (end == false);
        }
        public static void SwitchTaskStatus(task _task)
        {
            _task.isDone = !_task.isDone;
            if (_task.isDone == true)
            {
                _task.priority = 2;
            }
            else
            {
                _task.priority = 0;
            }
        }
        public static void RemoveTask(task _task)
        {
            taskList.tasks.Remove(_task);
        }
        public static bool ReturnOrCancel()
        {
            Console.Clear();
            Console.WriteLine("Пожалуйста, введите возможное значение. Чтобы отменить, нажмите 0. Чтобы вернуться назад, нажмите любую клавишу");
            if (Console.ReadKey().KeyChar == '0')
            {
                Console.Clear();
                return true;
            }
            else 
            {
                Console.Clear();
                return false;
            }
        }

    }
}