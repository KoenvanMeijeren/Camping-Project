namespace Model
{
    public class CampingPlaceType
    {
        public int Id { get; private set; }
        public int guestLimit { get; private set; }
        public float StandardNightPrice { get; private set; }
        
        public Accommodation Accommodation { get; private set; }

        public CampingPlaceType(int id, int guestLimit, float standardNightPrice, Accommodation accommodation)
        {
            this.Id = id;
            this.guestLimit = guestLimit;
            this.StandardNightPrice = standardNightPrice;
            this.Accommodation = accommodation;
        }
        
    }
}