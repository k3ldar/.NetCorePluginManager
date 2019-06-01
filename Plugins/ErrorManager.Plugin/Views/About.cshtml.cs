using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ErrorManager.Plugin.Pages
{
#pragma warning disable CS1591
    public class AboutModel : PageModel
    {
        public string Message { get; set; }

        public void OnGet()
        {
            Message = "Your application description page.";
        }
    }
#pragma warning restore CS1591
}
