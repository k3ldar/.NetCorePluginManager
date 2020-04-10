using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ErrorManager.Plugin.Pages
{
#pragma warning disable CS1591
    public class AboutModel : PageModel
    {
        public string Message { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "I deem it to be valid in this context!")]
        public void OnGet()
        {
            Message = "Your application description page.";
        }
    }
#pragma warning restore CS1591
}
