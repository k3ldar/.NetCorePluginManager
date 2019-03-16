using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SharedPluginFeatures;

namespace ProductPlugin.Models
{
    public class AddToCartModel
    {
        #region Constructors

        public AddToCartModel()
        {
            Quantity = 1;
        }

        public AddToCartModel(in int id)
            : this ()
        {
            Id = id;
        }

        #endregion Constructors

        #region Properties

        public int Id { get; set; }

        public int Quantity { get; set; }

        #endregion Properties
    }
}
