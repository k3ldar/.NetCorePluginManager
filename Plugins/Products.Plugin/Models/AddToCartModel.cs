using System;

#pragma warning disable CS1591

namespace ProductPlugin.Models
{
    public class AddToCartModel
    {
        #region Constructors

        public AddToCartModel()
        {
            Quantity = 1;
        }

        public AddToCartModel(in int id, in decimal retailPrice, in decimal discount, in uint stockAvailability)
            : this ()
        {
            if (retailPrice <= 0)
                throw new ArgumentOutOfRangeException(nameof(retailPrice));

            if (discount < 0 || discount > 100)
                throw new ArgumentOutOfRangeException(nameof(discount));

            Id = id;
            RetailPrice = retailPrice;
            Discount = discount;
            StockAvailability = stockAvailability;
        }

        #endregion Constructors

        #region Properties

        public int Id { get; set; }

        public int Quantity { get; set; }

        public decimal RetailPrice { get; set; }

        public decimal Discount { get; set; }

        public uint StockAvailability { get; private set; }

        #endregion Properties
    }
}

#pragma warning restore CS1591