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
 *  File: UserClaimsDataRow.cs
 *
 *  Purpose:  Table definition for user claims
 *
 *  Date        Name                Reason
 *  06/06/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using PluginManager.SimpleDB;

namespace PluginManager.DAL.TextFiles.Tables
{
    [Table(Constants.TableNameUserApi, CompressionType.None, CachingStrategy.None)]
    internal class UserApiDataRow : TableRowDefinition
    {
        #region Private Members

        long _userId;
        string _merchantId;
        string _apiKey;
        string _secret;

        #endregion Private Members

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

        public string MerchantId
        {
            get
            {
                return _merchantId;
            }

            set
            {
                if (_merchantId == value)
                    return;

                _merchantId = value;
                Update();
            }
        }

        public string ApiKey
        {
            get
            {
                return _apiKey;
            }

            set
            {
                if (_apiKey == value)
                    return;

                _apiKey = value;
                Update();
            }
        }

        public string Secret
        {
            get
            {
                return _secret;
            }

            set
            {
                if (_secret == value)
                    return;

                _secret = value;
                Update();
            }
        }
    }
}
