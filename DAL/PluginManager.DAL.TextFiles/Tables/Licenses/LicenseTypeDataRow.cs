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
 *  File: LicenseTypeDataRow.cs
 *
 *  Purpose:  Table definition for license types
 *
 *  Date        Name                Reason
 *  16/07/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using PluginManager.SimpleDB;

namespace PluginManager.DAL.TextFiles.Tables
{
    [Table(Constants.TableNameLicenseTypes)]
    internal class LicenseTypeDataRow : TableRowDefinition
    {
        private string _description;

        [UniqueIndex]
        public string Description
        {
            get
            {
                return _description;
            }

            set
            {
                if (_description == value)
                    return;

                _description = value;
                Update();
            }
        }
    }
}
