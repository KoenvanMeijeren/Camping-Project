﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <inheritdoc/>
    public class CampingPlace : ModelBase<CampingPlace>
    {
        public const string
            TableName = "CampingPlace",
            ColumnId = "CampingPlaceID",
            ColumnType = "CampingPlaceTypeID",
            ColumnNumber = "CampingPlaceNumber",
            ColumnSurface = "CampingPlaceSurface",
            ColumnExtraNightPrice = "CampingPlaceExtraNightPrice";
        
        public int Number { get; private set; }
        public float Surface { get; private set; }
        public string SurfaceReadable { get; private set; }
        public float ExtraNightPrice { get; private set; }
        public string Location { get; private set; }
        public float TotalPrice { get; private set; }
        public string TotalPriceReadable { get; private set; }
        public CampingPlaceType Type { get; private set; }

        public CampingPlace(): base(TableName, ColumnId)
        {
        }
        
        public CampingPlace(string number, string surface, string extraNightPrice, CampingPlaceType campingPlaceType): this("-1", number, surface, extraNightPrice, campingPlaceType)
        {
        }
        
        public CampingPlace(string id, string number, string surface, string extraNightPrice, CampingPlaceType campingPlaceType): base(TableName, ColumnId)
        {
            bool successId = int.TryParse(id, out int numericId);
            bool successNumber = int.TryParse(number, out int numericNumber);
            bool successSurface = float.TryParse(surface, out float numericSurface);
            bool successExtraNightPrice = float.TryParse(extraNightPrice, out float numericExtraNightPrice);

            float standardNightPrice = 0;
            if (campingPlaceType != null)
            {
                standardNightPrice = campingPlaceType.StandardNightPrice;
            }

            this.Id = successId ? numericId : -1;
            this.Number = successNumber ? numericNumber : 0;
            this.Surface = successSurface ? numericSurface : 0;
            this.ExtraNightPrice = successExtraNightPrice ? numericExtraNightPrice : 0;
            this.Type = campingPlaceType;
            this.Location = this.GetLocation();
            this.TotalPrice = this.ExtraNightPrice + standardNightPrice;
            this.TotalPriceReadable = $"€ {this.TotalPrice}";
            this.SurfaceReadable = $"{this.Surface} m3";
        }

        /// <summary>
        /// Gets the location of this camping place.
        /// </summary>
        /// <returns>The location build on camping place number and accommodation prefix.</returns>
        public string GetLocation()
        {
            return this.Type == null ? this.Number.ToString() : $"{this.Type.Accommodation.Prefix}-{this.Number}";
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return this.GetLocation();
        }

        public bool Update(int number, float surface, float extraNightPrice, CampingPlaceType campingPlaceType)
        {
            this.Number = number;
            this.Surface = surface;
            this.ExtraNightPrice = extraNightPrice;
            this.Type = campingPlaceType;
            
            return base.Update(CampingPlace.ToDictionary(number, surface, extraNightPrice, campingPlaceType));
        }

        /// <inheritdoc/>
        protected override CampingPlace ToModel(Dictionary<string, string> dictionary)
        {
            dictionary.TryGetValue(ColumnId, out string campingPlaceId);
            dictionary.TryGetValue(ColumnType, out string campingPlaceTypeId);
            dictionary.TryGetValue(ColumnNumber, out string placeNumber);
            dictionary.TryGetValue(ColumnSurface, out string surface);
            dictionary.TryGetValue(ColumnExtraNightPrice, out string extraNightPrice);
            
            dictionary.TryGetValue(Accommodation.ColumnId, out string accommodationId);
            dictionary.TryGetValue(Accommodation.ColumnPrefix, out string prefix);
            dictionary.TryGetValue(Accommodation.ColumnName, out string accommodationName);
            
            dictionary.TryGetValue(CampingPlaceType.ColumnGuestLimit, out string guestLimit);
            dictionary.TryGetValue(CampingPlaceType.ColumnNightPrice, out string standardNightPrice);

            Accommodation accommodation = new Accommodation(accommodationId, prefix, accommodationName);
            CampingPlaceType campingPlaceType = new CampingPlaceType(campingPlaceTypeId, guestLimit, standardNightPrice, accommodation);

            return new CampingPlace(campingPlaceId, placeNumber, surface, extraNightPrice, campingPlaceType);
        }

        /// <inheritdoc/>
        protected override Dictionary<string, string> ToDictionary()
        {
            return CampingPlace.ToDictionary(this.Number, this.Surface, this.ExtraNightPrice, this.Type);
        }

        private static Dictionary<string, string> ToDictionary(int number, float surface, float extraNightPrice, CampingPlaceType campingPlaceType)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>
            {
                {ColumnType, campingPlaceType.Id.ToString()},
                {ColumnNumber, number.ToString()},
                {ColumnSurface, surface.ToString(CultureInfo.InvariantCulture)},
                {ColumnExtraNightPrice, extraNightPrice.ToString(CultureInfo.InvariantCulture)}
            };

            return dictionary;
        }
        
        /// <inheritdoc/>
        protected override string BaseSelectQuery()
        {
            string query = base.BaseSelectQuery();
            query += $" INNER JOIN {CampingPlaceType.TableName} CPT ON CPT.{CampingPlaceType.ColumnId} = BT.{ColumnType}";
            query += $" INNER JOIN {Accommodation.TableName} ACM ON ACM.{Accommodation.ColumnId} = CPT.{CampingPlaceType.ColumnAccommodation}";

            return query;
        }

    }
}
