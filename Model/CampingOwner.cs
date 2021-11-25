namespace Model
{
    public class CampingOwner
    {
        public int Id { get; private set; }
        
        public string Name { get; private set; }

        public CampingOwner(string id, string name)
        {
            this.Id = int.Parse(id);
            this.Name = name;
        }
        
    }
}