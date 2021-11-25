namespace Model
{
    public class Camping
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        
        public Address Address { get; private set; }
        public CampingOwner CampingOwner { get; private set; }

        public Camping(string id, string name, Address address, CampingOwner campingOwner)
        {
            this.Id = int.Parse(id);
            this.Name = name;
            this.Address = address;
            this.CampingOwner = campingOwner;
        }
    }
}