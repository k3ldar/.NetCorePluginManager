using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Shared.Classes;

namespace AspNetCore.PluginManager.DemoWebsite.Helpers
{
    /// <summary>
    /// The purpose of this static class is to provide a conduit to integrating with the user session
    /// and obtaining geo ip data, if required
    /// </summary>
    public static class SessionHelper
    {
        public static void InitSessionHelper()
        {
            UserSessionManager.Instance.OnSessionCreated += UserSession_OnSessionCreated;
            UserSessionManager.Instance.OnSavePage += UserSession_OnSavePage;
            UserSessionManager.Instance.OnSessionClosing += UserSession_OnSessionClosing;
            UserSessionManager.Instance.OnSessionRetrieve += UserSession_OnSessionRetrieve;
            UserSessionManager.Instance.OnSessionSave += UserSession_OnSessionSave;
            UserSessionManager.Instance.IPAddressDetails += UserSession_IPAddressDetails;
        }

        private static void UserSession_IPAddressDetails(object sender, Shared.IpAddressArgs e)
        {
            
        }

        private static void UserSession_OnSessionSave(object sender, Shared.UserSessionArgs e)
        {
            
        }

        private static void UserSession_OnSessionRetrieve(object sender, Shared.UserSessionRequiredArgs e)
        {
            
        }

        private static void UserSession_OnSessionClosing(object sender, Shared.UserSessionArgs e)
        {
            
        }

        private static void UserSession_OnSavePage(object sender, Shared.UserPageViewArgs e)
        {
            
        }

        private static void UserSession_OnSessionCreated(object sender, Shared.UserSessionArgs e)
        {
            
        }
    }
}
