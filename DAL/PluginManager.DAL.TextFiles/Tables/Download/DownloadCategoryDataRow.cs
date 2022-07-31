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
 *  File: DownloadCategoryDataRow.cs
 *
 *  Purpose:  Row definition for Table for download categories
 *
 *  Date        Name                Reason
 *  24/07/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using SimpleDB;

namespace PluginManager.DAL.TextFiles.Tables
{
    [Table(Constants.DomainDownloads, Constants.TableNameDownloadCategories)]
    internal class DownloadCategoryDataRow : TableRowDefinition
    {
        private string _name;
        private long _userId;

        public string Name
        {
            get => _name;

            set
            {
                if (_name == value)
                    return;

                _name = value;
                Update();
            }
        }

        [ForeignKey(Constants.TableNameUsers, true)]
        public long UserId
        {
            get => _userId;

            set
            {
                if (value == _userId)
                    return;

                _userId = value;
                Update();
            }
        }
    }
}
