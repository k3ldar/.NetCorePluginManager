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
 *  File: LicenseDataRow.cs
 *
 *  Purpose:  Table definition for licenses
 *
 *  Date        Name                Reason
 *  16/07/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using SimpleDB;

namespace PluginManager.DAL.TextFiles.Tables
{
    [Table(Constants.TableNameLicenses)]
    internal class LicenseDataRow : TableRowDefinition
    {
        private long _userId;
        private long _licenseType;
        private long _startDateTicks;
        private long _expireDateTicks;
        private bool _isValid;
        private bool _isTrial;
        private byte _updateCount;
        private long _invoiceId;
        private string _domainName;
        private string _encryptedLicense;

        [ForeignKey(Constants.TableNameUsers)]
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

        [ForeignKey(Constants.TableNameLicenseTypes)]
        public long LicenseType
        {
            get => _licenseType;

            set
            {
                if (_licenseType == value)
                    return;

                _licenseType = value;
                Update();
            }
        }

        public long StartDateTicks
        {
            get => _startDateTicks;

            set
            {
                if (_startDateTicks == value)
                    return;

                _startDateTicks = value;
                Update();
            }
        }

        public long ExpireDateTicks
        {
            get => _expireDateTicks;

            set
            {
                if (_expireDateTicks == value)
                    return;

                _expireDateTicks = value;
                Update();
            }
        }

        public bool IsValid
        {
            get => _isValid;

            set
            {
                if (_isValid == value)
                    return;

                _isValid = value;
                Update();
            }
        }

        public bool IsTrial
        {
            get => _isTrial;

            set
            {
                if (_isTrial == value)
                    return;

                _isTrial = value;
                Update();
            }
        }

        public byte UpdateCount
        {
            get => _updateCount;

            set
            {
                if (_updateCount == value)
                    return;

                _updateCount = value;
                Update();
            }
        }

        [ForeignKey(Constants.TableNameInvoices, true)]
        public long InvoiceId
        {
            get => _invoiceId;

            set
            {
                if (_invoiceId == value)
                    return;

                _invoiceId = value;
                Update();
            }
        }

        public string DomainName
        {
            get => _domainName;

            set
            {
                if (_domainName == value)
                    return;

                _domainName = value;
                Update();
            }
        }

        public string EncryptedLicense
        {
            get => _encryptedLicense;

            set
            {
                if (_encryptedLicense == value)
                    return;

                _encryptedLicense = value;
                Update();
            }
        }

        public DateTime StartDate => new DateTime(_startDateTicks, DateTimeKind.Utc);

        public DateTime ExpireDate => new DateTime(_expireDateTicks, DateTimeKind.Utc);
    }
}
