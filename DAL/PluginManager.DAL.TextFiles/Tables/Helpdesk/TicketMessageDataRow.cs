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
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginManager.DAL.TextFiles
 *  
 *  File: FeedbackDataRow.cs
 *
 *  Purpose:  Table definition for ticket departments
 *
 *  Date        Name                Reason
 *  18/07/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using SimpleDB;

namespace PluginManager.DAL.TextFiles.Tables
{
    [Table(Constants.DomainHelpdesk, Constants.TableNameTicketMessages, CompressionType.Brotli)]
    internal class TicketMessageDataRow : TableRowDefinition
    {
        private long _ticketId;
        private string _userName;
        private string _message;

        [ForeignKey(Constants.TableNameTicket)]
        public long TicketId
        {
            get => _ticketId;

            set
            {
                if (_ticketId == value)
                    return;

                _ticketId = value;
                Update();
            }
        }

        public string UserName
        {
            get => _userName;

            set
            {
                if (_userName == value)
                    return;

                _userName = value;
                Update();
            }
        }

        public string Message
        {
            get => _message;

            set
            {
                if (_message == value)
                    return;

                _message = value;
                Update();
            }
        }
    }
}
