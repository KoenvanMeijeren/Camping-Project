namespace Model
{
    // todo: henk
    public class CampingPlaceType : IModel
    {
        public int Id { get; private set; }
        public int GuestLimit { get; private set; }
        public float StandardNightPrice { get; private set; }
        
        public Accommodation Accommodation { get; private set; }

        public CampingPlaceType(string id, string guestLimit, string standardNightPrice, Accommodation accommodation)
        {
            this.Id = int.Parse(id);
            this.GuestLimit = int.Parse(guestLimit);
            this.StandardNightPrice = float.Parse(standardNightPrice);
            this.Accommodation = accommodation;
        }
        
    }
}