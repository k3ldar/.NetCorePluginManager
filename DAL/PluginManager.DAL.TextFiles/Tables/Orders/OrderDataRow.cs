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
 *  File: OrdersDataRow.cs
 *
 *  Purpose:  Table definition for user orders
 *
 *  Date        Name                Reason
 *  18/06/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using PluginManager.SimpleDB;

namespace PluginManager.DAL.TextFiles.Tables
{
    [Table(Constants.TableNameOrders)]
    internal sealed class OrderDataRow : TableRowDefinition
    {
        private long _deliveryAddress;
        private long _userId;
        private decimal _postage;
        private string _culture;
        private int _processStatus;

        [ForeignKey(Constants.TableNameUsers)]
        public long UserId
        {
            get
            {
                return _userId;
            }

            set
            {
                if (_userId == value)
                    return;

                _userId = value;
                Update();
            }
        }

        [ForeignKey(Constants.TableNameAddresses)]
        public long DeliveryAddress
        {
            get
            {
                return _deliveryAddress;
            }

            set
            {
                if (_deliveryAddress == value)
                    return;

                _deliveryAddress = value;
                Update();
            }
        }

        public decimal Postage
        {
            get
            {
                return _postage;
            }

            set
            {
                if (_postage == value)
                    return;

                _postage = value;
                Update();
            }
        }

        public string Culture
        {
            get
            {
                return _culture;
            }

            set
            {
                if (_culture == value)
                    return;

                _culture = value;
                Update();
            }
        }

        public int ProcessStatus
        {
            get
            {
                return _processStatus;
            }

            set
            {
                if (_processStatus == value)
                    return;

                _processStatus = value;
                Update();
            }
        }
    }
}
