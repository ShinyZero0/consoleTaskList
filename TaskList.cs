using System;
using System.Collections.Generic;
using System.Text.Json;

namespace consoleTaskList
{
    public class TaskList
    {  
        public List<task>? tasks {get; set; }
        public TaskList(List<task> _tasks)
        {
            tasks = _tasks;
        }
        public TaskList(){
            tasks = new List<task>();
        }
    }
}