using System.Collections.Generic;
using SystemCore;

namespace Model
{
    /// <inheritdoc/>
    public class Address : ModelBase<Address>
    {
        public const string
            TableName = "Address",
            ColumnId = "AddressID",
            ColumnAddress = "Address",
            ColumnPostalCode = "AddressPostalCode",
            ColumnPlace = "AddressPlace";
        
        public string Street { get; private set; }
        
        public string PostalCode { get; private set; }
        
        public string Place { get; private set; }

        public Address(): base(TableName, ColumnId)
        {
            
        }
        
        public Address(string address, string postalCode, string place): this(null, address, postalCode, place)
        {
            
        }
        
        public Address(string id, string address, string postalCode, string place): base(TableName, ColumnId)
        {
            bool success = int.TryParse(id, out int idNumeric);
            
            this.Id = success ? idNumeric : -1;
            this.Street = address;
            this.PostalCode = postalCode;
            this.Place = place;
        }

        /// <summary>
        /// Fetches Address with given parameters, if address already exists in database
        /// </summary>
        /// <param name="address">given address</param>
        /// <param name="postalCode">given postalcode</param>
        /// <returns>Address object</returns>
        private Address SelectByParameters(string address, string postalCode)
        {
            Query query = new Query($"{this.BaseSelectQuery()} WHERE {ColumnAddress} = @Address AND {ColumnPostalCode} = @AddressPostalCode");
            query.AddParameter(ColumnAddress, address);
            query.AddParameter(ColumnPostalCode, postalCode);
            
            var result = query.SelectFirst();
            if (result == null)
            {
                return null;
            }
            
            // Updates the selected address with the current place.
            result.Remove(ColumnPlace);
            result.Add(ColumnPlace, this.Place);

            result.TryGetValue(ColumnId, out string id);
            bool successId = int.TryParse(id, out int numericId);
            
            this.Id = successId ? numericId : this.Id;
            
            this.Update(address, postalCode, this.Place);

            return this.ToModel(result);
        }

        /// <summary>
        /// Returns Address object based on if it already exists in database. If it doesn't exist it creates one and
        /// returns that one. An existing address is always updated.
        /// </summary>
        /// <returns>Address object</returns>
        public Address FirstAndUpdateOrInsert()
        {
            var result = this.SelectByParameters(this.Street, this.PostalCode);
            if (result != null)
            {
                return result;
            }

            this.Insert();
            return this.SelectByParameters(this.Street, this.PostalCode);
        }

        public bool Update(string street, string postalCode, string place)
        {
            if (this.Street.Equals(street) && this.PostalCode.Equals(postalCode))
            {
                this.Place = place;
                
                return base.Update(Address.ToDictionary(street, postalCode, place));
            }
            
            // Reset all data, in order to prepare updating the address.
            this.Id = -1;
            this.Street = street;
            this.PostalCode = postalCode;
            this.Place = place;

            var address = this.FirstAndUpdateOrInsert();
            this.Id = address.Id;
            this.Street = address.Street;
            this.PostalCode = address.PostalCode;
            this.Place = address.Place;
            
            return true;
        }

        /// <inheritdoc/>
        protected override Address ToModel(Dictionary<string, string> dictionary)
        {
            if (dictionary == null)
            {
                return null;
            }
            
            dictionary.TryGetValue(ColumnId, out string id);
            dictionary.TryGetValue(ColumnAddress, out string address);
            dictionary.TryGetValue(ColumnPostalCode, out string postalCode);
            dictionary.TryGetValue(ColumnPlace, out string place);

            return new Address(id, address, postalCode, place);
        }

        /// <inheritdoc/>
        protected override Dictionary<string, string> ToDictionary()
        {
            return Address.ToDictionary(this.Street, this.PostalCode, this.Place);
        }

        private static Dictionary<string, string> ToDictionary(string address, string postalCode, string place)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>
            {
                {ColumnAddress, address},
                {ColumnPostalCode, postalCode},
                {ColumnPlace, place}
            };

            return dictionary;
        }
        
    }
}