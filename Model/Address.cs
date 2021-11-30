namespace Model
{
    public class Address : IModel
    {
        public int Id { get; private set; }

        public string address { get; private set; }
        
        public string postalCode { get; private set; }
        
        public string place { get; private set; }

        public Address(string id, string address, string postalCode, string place)
        {
            this.Id = int.Parse(id);
            this.address = address;
            this.postalCode = postalCode;
            this.place = place;
        }

    }
}