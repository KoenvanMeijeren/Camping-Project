namespace Model
{
    public class Accommodation
    {
        public int Id { get; private set; }
        public string Prefix { get; private set; }
        public string Name { get; private set; }

        public Accommodation(string id, string prefix, string name)
        {
            this.Id = int.Parse(id);
            this.Prefix = prefix;
            this.Name = name;
        }
        
    }
}