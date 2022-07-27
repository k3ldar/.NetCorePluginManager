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
 *  File: ExternalUsersDataRow.cs
 *
 *  Purpose:  Table definition for external users
 *
 *  Date        Name                Reason
 *  11/06/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using PluginManager.DAL.TextFiles.Internal;


namespace PluginManager.DAL.TextFiles.Tables
{
    [Table(Constants.TableNameExternalUsers, CompressionType.Brotli)]
    internal sealed class ExternalUsersDataRow : TableRowDefinition
    {
        private string _userId;
        private string _email;
        private string _userName;
        private string _provider;
        private string _picture;
        private string _token;

        public string UserId
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

        public string Email
        {
            get
            {
                return _email;
            }

            set
            {
                if (_email == value)
                    return;

                _email = value;
                Update();
            }
        }

        public string UserName
        {
            get
            {
                return _userName;
            }

            set
            {
                if (_userName == value)
                    return;

                _userName = value;
                Update();
            }
        }

        public string Provider
        {
            get
            {
                return _provider;
            }

            set
            {
                if (_provider == value)
                    return;

                _provider = value;
                Update();
            }
        }

        public string Picture
        {
            get
            {
                return _picture;
            }

            set
            {
                if (_picture == value)
                    return;

                _picture = value;
                Update();
            }
        }

        public string Token
        {
            get
            {
                return _token;
            }

            set
            {
                if (_token == value)
                    return;

                _token = value;
                Update();
            }
        }
    }
}
