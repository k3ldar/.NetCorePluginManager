using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SharedPluginFeatures;

namespace ShoppingCartPlugin.Models
{
    public class BasketModel : BaseModel
    {
        #region Constructors

        public BasketModel(in List<BreadcrumbItem> breadcrumbs, in ShoppingCartSummary cartSummary)
            : base(breadcrumbs, cartSummary)
        {

        }

        #endregion Constructors
    }
}
