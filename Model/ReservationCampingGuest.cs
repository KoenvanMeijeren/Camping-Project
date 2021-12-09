using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemCore;

namespace Model
{
    /// <inheritdoc/>
    public class ReservationCampingGuest : ModelBase<ReservationCampingGuest>
    {
        public const string
            TableName = "ReservationCampingGuest",
            ColumnId = "ReservationCampingGuestID",
            ColumnReservation = "ReservationID",
            ColumnGuest = "CampingGuestID";
        
        public Reservation Reservation { get; private set; }
        public CampingGuest CampingGuest { get; private set; }

        public ReservationCampingGuest(): base(TableName, ColumnId)
        {

        }

        public ReservationCampingGuest(Reservation reservation, CampingGuest campingGuest): this("-1", reservation, campingGuest)
        {

        }
        
        public ReservationCampingGuest(string id, Reservation reservation, CampingGuest campingGuest): base(TableName, ColumnId)
        {
            bool successId = int.TryParse(id, out int numericId);
            
            this.Id = successId ? numericId : -1;
            this.Reservation = reservation;
            this.CampingGuest = campingGuest;
        }

        /// <summary>
        /// Selects all camping guests by the given reservation.
        /// </summary>
        /// <param name="reservation">The reservation.</param>
        /// <returns>The camping guests.</returns>
        public List<ReservationCampingGuest> SelectByReservation(Reservation reservation)
        {
            Query query = new Query(this.BaseSelectQuery() + $" WHERE BT.{ColumnReservation} = @{ColumnReservation}");
            query.AddParameter(ColumnReservation, reservation.Id);
         
            this.Collection = new List<ReservationCampingGuest>();
            foreach (var reservationCampingGuest in query.Select())
            {
                this.Collection.Add(this.ToModel(reservationCampingGuest));
            }
            
            return this.Collection;
        }

        public bool Update(Reservation reservation, CampingGuest campingGuest)
        {
            this.Reservation = reservation;
            this.CampingGuest = campingGuest;

            return base.Update(ReservationCampingGuest.ToDictionary(reservation, campingGuest));
        }

        /// <inheritdoc/>
        protected override ReservationCampingGuest ToModel(Dictionary<string, string> dictionary)
        {
            if(dictionary == null)
            {
                return null;
            }

            dictionary.TryGetValue(ColumnId, out string reservationCampingGuestId);
            dictionary.TryGetValue(ColumnReservation, out string reservationId);
            dictionary.TryGetValue(Reservation.ColumnPlace, out string campingPlaceId);
            dictionary.TryGetValue(Reservation.ColumnPeople, out string numberOfPeople);
            dictionary.TryGetValue(Reservation.ColumnCustomer, out string campingCustomerId);
            dictionary.TryGetValue(Reservation.ColumnDuration, out string reservationDurationId);
            
            dictionary.TryGetValue(ColumnGuest, out string campingGuestId);
            dictionary.TryGetValue(CampingGuest.ColumnFirstName, out string guestFirstName);
            dictionary.TryGetValue(CampingGuest.ColumnLastName, out string guestLastName);
            dictionary.TryGetValue(CampingGuest.ColumnBirthdate, out string guestBirthdate);

            dictionary.TryGetValue(CampingPlaceType.ColumnId, out string campingPlaceTypeId);
            dictionary.TryGetValue(CampingPlaceType.ColumnGuestLimit, out string guestLimit);
            dictionary.TryGetValue(CampingPlaceType.ColumnNightPrice, out string standardNightPrice);
            
            dictionary.TryGetValue(Accommodation.ColumnId, out string accommodationId);
            dictionary.TryGetValue(Accommodation.ColumnPrefix, out string prefix);
            dictionary.TryGetValue(Accommodation.ColumnName, out string name);
            
            dictionary.TryGetValue(CampingPlace.ColumnId, out string placeNumber);
            dictionary.TryGetValue(CampingPlace.ColumnSurface, out string surface);
            dictionary.TryGetValue(CampingPlace.ColumnExtraNightPrice, out string extraNightPrice);
            
            dictionary.TryGetValue(Address.ColumnId, out string addressId);
            dictionary.TryGetValue(Address.ColumnAddress, out string address);
            dictionary.TryGetValue(Address.ColumnPostalCode, out string postalCode);
            dictionary.TryGetValue(Address.ColumnPlace, out string place);

            dictionary.TryGetValue(Account.ColumnId, out string accountId);
            dictionary.TryGetValue(Account.ColumnEmail, out string email);
            dictionary.TryGetValue(Account.ColumnPassword, out string password);
            dictionary.TryGetValue(Account.ColumnRights, out string rights);

            dictionary.TryGetValue(CampingCustomer.ColumnBirthdate, out string customerBirthdate);
            dictionary.TryGetValue(CampingCustomer.ColumnPhoneNumber, out string customerPhoneNumber);
            dictionary.TryGetValue(CampingCustomer.ColumnFirstName, out string customerFirstName);
            dictionary.TryGetValue(CampingCustomer.ColumnLastName, out string customerLastName);
            
            dictionary.TryGetValue(ReservationDuration.ColumnCheckInDate, out string checkInDateTime);
            dictionary.TryGetValue(ReservationDuration.ColumnCheckOutDate, out string checkOutDateTime);

            Address customerAddress = new Address(addressId, address, postalCode, place);
            Accommodation accommodation = new Accommodation(accommodationId, prefix, name);
            CampingPlaceType campingPlaceType = new CampingPlaceType(campingPlaceTypeId, guestLimit, standardNightPrice, accommodation);
            CampingPlace campingPlace = new CampingPlace(campingPlaceId, placeNumber, surface, extraNightPrice, campingPlaceType);
            Account account = new Account(accountId, email, password, rights);
            CampingCustomer campingCustomer = new CampingCustomer(campingCustomerId, account, customerAddress, customerBirthdate, customerPhoneNumber, customerFirstName, customerLastName);
            ReservationDuration reservationDuration = new ReservationDuration(reservationDurationId, checkInDateTime, checkOutDateTime);

            Reservation reservation = new Reservation(reservationId, numberOfPeople, campingCustomer, campingPlace, reservationDuration);
            CampingGuest campingGuest = new CampingGuest(campingGuestId, guestFirstName, guestLastName, guestBirthdate);

            return new ReservationCampingGuest(reservationCampingGuestId, reservation, campingGuest);
        }
        
        /// <inheritdoc/>
        protected override Dictionary<string, string> ToDictionary()
        {
            return ReservationCampingGuest.ToDictionary(this.Reservation, this.CampingGuest);
        }

        private static Dictionary<string, string> ToDictionary(Reservation reservation, CampingGuest campingGuest)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>
            {
                {ColumnReservation, reservation.Id.ToString()},
                {ColumnGuest, campingGuest.Id.ToString()}
            };

            return dictionary;
        }

        /// <inheritdoc/>
        protected override string BaseSelectQuery()
        {
            string query = base.BaseSelectQuery();
            query += $" INNER JOIN {Reservation.TableName} R ON BT.{ColumnReservation} = R.{Reservation.ColumnId}";
            query += $" INNER JOIN {CampingPlace.TableName} CP ON CP.{CampingPlace.ColumnId} = R.{Reservation.ColumnPlace} ";
            query += $" INNER JOIN {CampingPlaceType.TableName} CPT ON CPT.{CampingPlaceType.ColumnId} = CP.{CampingPlace.ColumnType} ";
            query += $" INNER JOIN {Accommodation.TableName} AM ON AM.{Accommodation.ColumnId} = CPT.{CampingPlaceType.ColumnAccommodation} ";
            query += $" INNER JOIN {CampingCustomer.TableName} CC ON CC.{CampingCustomer.ColumnId} = R.{Reservation.ColumnCustomer} ";
            query += $" LEFT JOIN {Account.TableName} AC on CC.{CampingCustomer.ColumnAccount} = AC.{Account.ColumnId}";
            query += $" INNER JOIN {Address.TableName} CCA ON CCA.{Address.ColumnId} = CC.{CampingCustomer.ColumnAddress} ";
            query += $" INNER JOIN {ReservationDuration.TableName} RD ON RD.{ReservationDuration.ColumnId} = R.{Reservation.ColumnDuration} ";
            query += $" INNER JOIN {CampingGuest.TableName} CG ON BT.{ColumnGuest} = CG.{CampingGuest.ColumnId}";

            return query;
        }
    }
}
