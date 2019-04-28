/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 *  .Net Core Plugin Manager is distributed under the GNU General Public License version 3 and  
 *  is also available under alternative licenses negotiated directly with Simon Carter.  
 *  If you obtained Service Manager under the GPL, then the GPL applies to all loadable 
 *  Service Manager modules used on your system as well. The GPL (version 3) is 
 *  available at https://opensource.org/licenses/GPL-3.0
 *
 *  This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
 *  without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 *  See the GNU General Public License for more details.
 *
 *  The Original Code was created by Simon Carter (s1cart3r@gmail.com)
 *
 *  Copyright (c) 2019 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginMiddleware
 *  
 *  File: Feedback.cs
 *
 *  Purpose:  Download Categories
 *
 *  Date        Name                Reason
 *  13/04/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

namespace Middleware.Helpdesk
{
    public sealed class Feedback
    {
        #region Constructors

        public Feedback (in long id, in string username, in string message, in bool showOnWebsite)
        {
            Id = id;
            Username = username;
            Message = message;
            ShowOnWebsite = showOnWebsite;
        }

        #endregion Constructors

        #region Properties

        public long Id { get; private set; }

        public string Username { get; private set; }

        public string Message { get; private set; }

        public bool ShowOnWebsite { get; private set; }

        #endregion Properties
    }
}
