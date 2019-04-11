using System.Collections.Generic;

using SharedPluginFeatures;

namespace HelpdeskPlugin.Models
{
    public class IndexViewModel : BaseModel
    {
        #region Construtors

        public IndexViewModel(in List<BreadcrumbItem> breadcrumbs, in ShoppingCartSummary cartSummary, 
            in bool showTickets, in bool showFaq, in bool showFeedback, in string growlMessage)
            : base (breadcrumbs, cartSummary)
        {
            ShowFaq = showFaq;
            ShowFeedback = showFeedback;
            ShowTickets = showTickets;
            GrowlMessage = growlMessage ?? string.Empty;
        }

        #endregion Construtors

        #region Properties

        public bool ShowTickets { get; private set; }

        public bool ShowFaq { get; private set; }

        public bool ShowFeedback { get; private set; }

        public string GrowlMessage { get; private set; }

        #endregion Properties
    }
}
