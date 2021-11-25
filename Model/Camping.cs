namespace Model
{
    public class Camping
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        
        public Address Address { get; private set; }
        public CampingOwner CampingOwner { get; private set; }

        public Camping(int id, string name, Address address, CampingOwner campingOwner)
        {
            this.Id = id;
            this.Name = name;
            this.Address = address;
            this.CampingOwner = campingOwner;
        }
    }
}