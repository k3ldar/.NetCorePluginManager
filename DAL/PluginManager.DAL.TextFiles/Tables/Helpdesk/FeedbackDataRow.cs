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
 *  Copyright (c) 2018 - 2022 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginManager.DAL.TextFiles
 *  
 *  File: FeedbackDataRow.cs
 *
 *  Purpose:  Table definition for feed back
 *
 *  Date        Name                Reason
 *  18/07/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using PluginManager.DAL.TextFiles.Internal;

namespace PluginManager.DAL.TextFiles.Tables
{
    [Table(Constants.DomainHelpdesk, Constants.TableNameFeedback)]
    internal class FeedbackDataRow : TableRowDefinition
    {
        private long _userId;
        private string _message;
        private bool _showOnWebsite;

        [ForeignKey(Constants.TableNameUsers, true)]
        public long UserId
        {
            get => _userId;

            set
            {
                if (_userId == value)
                    return;

                _userId = value;
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

        public bool ShowOnWebsite
        {
            get => _showOnWebsite;

            set
            {
                if (_showOnWebsite == value)
                    return;

                _showOnWebsite = value;
                Update();
            }
        }
    }
}
