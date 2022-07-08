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
 *  File: AddressDataRow.cs
 *
 *  Purpose:  Row definition for Table for delivery addresses
 *
 *  Date        Name                Reason
 *  31/05/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using PluginManager.DAL.TextFiles.Internal;

namespace PluginManager.DAL.TextFiles.Tables
{
    /// <summary>
    /// Delivery address table 
    /// </summary>
    [Table(Constants.TableNameAddresses, CompressionType.Brotli)]
    internal sealed class AddressDataRow : TableRowDefinition
    {
        private long _userId;
        private string _businessName;
        private string _addressLine1;
        private string _addressLine2;
        private string _addressLine3;
        private string _city;
        private string _county;
        private string _postcode;
        private string _country;
        private decimal _postageCost;
        private bool _isDelivery;

        /// <summary>
        /// Id of user owning the address
        /// </summary>
        [ForeignKey(Constants.TableNameUsers)]
        public long UserId 
        { 
            get => _userId;

            set
            {
                _userId = value;
                Update();
            }
        }

        /// <summary>
        /// Business name if applicable.
        /// </summary>
        /// <value>string</value>
        public string BusinessName 
        { 
            get => _businessName;

            set
            {
                _businessName = value;
                Update();
            }
        }

        /// <summary>
        /// Address line 1.
        /// </summary>
        /// <value>string</value>
        public string AddressLine1 
        { 
            get => _addressLine1;

            set
            {
                _addressLine1 = value;
                Update();
            }
        }

        /// <summary>
        /// Address line 2.
        /// </summary>
        /// <value>string</value>
        public string AddressLine2 
        { 
            get => _addressLine2;

            set
            {
                _addressLine2 = value;
                Update();
            }
        }

        /// <summary>
        /// Address line 3.
        /// </summary>
        /// <value>string</value>
        public string AddressLine3 
        { 
            get => _addressLine3;

            set 
            {  
                _addressLine3 = value;
                Update();
            }
        }

        /// <summary>
        /// City name.
        /// </summary>
        /// <value>string</value>
        public string City 
        { 
            get => _city;

            set
            {
                _city = value;
                Update();
            }
}

        /// <summary>
        /// County/state name.
        /// </summary>
        /// <value>string</value>
        public string County 
        { 
            get => _county;

            set
            {
                _county = value;
                Update();
            }
        }

        /// <summary>
        /// Postal or zip code.
        /// </summary>
        /// <value>string</value>
        public string Postcode 
        { 
            get => _postcode;

            set
            {
                _postcode = value;
                Update();
            }
        }

        /// <summary>
        /// Country name.
        /// </summary>
        /// <value>string</value>
        public string Country 
        { 
            get => _country;

            set
            {
                _country = value;
                Update();
            }
        }

        /// <summary>
        /// Postage cost for the address.
        /// </summary>
        /// <value>decimal</value>
        public decimal PostageCost 
        { 
            get => _postageCost;

            set
            {
                _postageCost = value;
                Update();
            }
        }

        /// <summary>
        /// Indicicates whether it's a delivery address or billing address
        /// </summary>
        public bool IsDelivery 
        { 
            get => _isDelivery;

            set
            {
                _isDelivery = value;
                Update();
            }
        }
    }
}
