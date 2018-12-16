using System;
using System.Collections.Generic;
using System.Text;

namespace Middleware
{
    public sealed class DeliveryAddress : Address
    {
        #region Constructors

        public DeliveryAddress()
        {
        }

        public DeliveryAddress(in int addressId, in string businessName, in string addressLine1, 
            in string addressLine2, in string addressLine3, in string city, in string county, 
            in string postcode, in string country, in decimal postageCost)
            : base (businessName, addressLine1, addressLine2, addressLine3, city, county, postcode, country)
        {
            AddressId = addressId;
            PostageCost = postageCost;
        }

        #endregion Constructors

        #region Properties

        public int AddressId { get; set; }

        public decimal PostageCost { get; set; }

        #endregion Properties
    }
}
