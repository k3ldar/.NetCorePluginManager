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
 *  File: UserDataRowDefaults.cs
 *
 *  Purpose:  Default values for user data row
 *
 *  Date        Name                Reason
 *  14/07/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using PluginManager.Abstractions;

using SimpleDB;

namespace PluginManager.DAL.TextFiles.Tables
{
    internal class UserDataRowDefaults : ITableDefaults<UserDataRow>
    {
        private readonly string _encryptionKey;

        public UserDataRowDefaults(ISettingsProvider settingsProvider)
        {
            SimpleDBSettings settings = settingsProvider.GetSettings<SimpleDBSettings>(nameof(SimpleDBSettings));

            if (settings == null)
                throw new InvalidOperationException();

            _encryptionKey = settings.EnycryptionKey;
        }

        public long PrimarySequence => 0;

        public long SecondarySequence => 0;

        public ushort Version => 1;

        public List<UserDataRow> InitialData(ushort version)
        {
            if (version == 1)
            {
                return new List<UserDataRow>()
                {
                    new()
                    {
                        Email = "admin",
                        FirstName = "admin",
                        Surname = "user",
                        Password = Shared.Utilities.Encrypt("M4sterK3y", _encryptionKey),
                    }
                };
            }

            return null;
        }
    }
}
