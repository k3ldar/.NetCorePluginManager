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
 *  File: HelpdeskTicketMessage.cs
 *
 *  Purpose:  Helpdesk support ticket Message
 *
 *  Date        Name                Reason
 *  21/04/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

namespace Middleware.Helpdesk
{
    public sealed class HelpdeskTicketMessage
    {
        #region Constructors

        public HelpdeskTicketMessage(in DateTime dateCreated, in string userName, in string message)
        {
            if (String.IsNullOrEmpty(userName))
                throw new ArgumentNullException(nameof(userName));

            if (String.IsNullOrEmpty(message))
                throw new ArgumentNullException(nameof(message));

            DateCreated = dateCreated;
            UserName = userName;
            Message = message;
        }

        #endregion Constructors

        #region Properties

        public DateTime DateCreated { get; private set; }

        public string UserName { get; private set; }

        public string Message { get; private set; }

        #endregion Properties
    }
}
