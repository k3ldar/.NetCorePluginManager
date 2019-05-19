using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SharedPluginFeatures;

using Shared.Docs;

namespace DocumentationPlugin.Models
{
    public sealed class IndexViewModel : BaseModel
    {
        #region Constructors

        public IndexViewModel(in List<BreadcrumbItem> breadcrumbs, in ShoppingCartSummary cartSummary)
            : base (breadcrumbs, cartSummary)
        {
            AssemblyNames = new Dictionary<string, string>();
        }

        #endregion Constructors

        #region Properties

        public Dictionary<string, string> AssemblyNames { get; private set; }

        #endregion Properties
    }
}
