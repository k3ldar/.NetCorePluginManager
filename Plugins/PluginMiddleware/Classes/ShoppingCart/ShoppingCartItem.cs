using System;
using System.Collections.Generic;
using System.Text;

namespace Middleware.ShoppingCart
{
    public sealed class ShoppingCartItem
    {
        #region Constructors

        public ShoppingCartItem()
        {

        }

        #endregion Constructors

        #region Properties

        public int Id { get; private set; }

        public decimal Count { get; private set; }

        public decimal Cost { get; private set; }



        #endregion Properties
    }
}
