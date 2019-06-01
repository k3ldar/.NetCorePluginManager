using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ErrorManager.Plugin.Pages
{
#pragma warning disable CS1591
    public class ContactModel : PageModel
    {
        public string Message { get; set; }

        public void OnGet()
        {
            Message = "Your contact page.";
        }
    }
#pragma warning restore CS1591
}
