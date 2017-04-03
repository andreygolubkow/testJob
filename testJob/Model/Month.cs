using SQLite;

namespace testJob.Model
{
    public class Month
    {
        [Unique]
        public int id { get; set; }

        public string name { get; set; }

        public Month(int id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public Month()
        {
            
        }
    }
}