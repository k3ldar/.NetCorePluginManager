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
 *  File: InvoiceDataRow.cs
 *
 *  Purpose:  Table definition for user invoices
 *
 *  Date        Name                Reason
 *  16/07/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using SimpleDB;

namespace PluginManager.DAL.TextFiles.Tables
{
    [Table(Constants.TableNameInvoices)]
    internal class InvoiceDataRow : TableRowDefinition
    {
        private long _deliveryAddress;
        private long _userId;
        private long _orderId;
        private decimal _postage;
        private string _culture;
        private int _paymentStatus;
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

        [ForeignKey(Constants.TableNameOrders)]
        public long OrderId
        {
            get
            {
                return _orderId;
            }

            set
            {
                if (_orderId == value)
                    return;

                _orderId = value;
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

        public int PaymentStatus
        {
            get
            {
                return _paymentStatus;
            }

            set
            {
                if (_paymentStatus == value)
                    return;

                _paymentStatus = value;
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
