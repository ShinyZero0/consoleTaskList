namespace consoleTaskList
{
    public class task
    {
        public string name {get; set;}
        public string content {get; set;}
        public bool isDone {get; set;}
        public int priority {get; set;}
        public task(string n, string c) 
        {
            name = n;
            content = c;
            isDone = false;
            priority = 0;
        }
    }
}