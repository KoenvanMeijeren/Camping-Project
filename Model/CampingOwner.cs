namespace Model
{
    public class CampingOwner
    {
        public int Id { get; private set; }
        
        public string Name { get; private set; }

        public CampingOwner(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
        
    }
}