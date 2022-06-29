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
 *  File: SeoDataRow.cs
 *
 *  Purpose:  Table definition for seo data
 *
 *  Date        Name                Reason
 *  19/06/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using PluginManager.DAL.TextFiles.Internal;

namespace PluginManager.DAL.TextFiles.Tables
{
    [Table(Constants.TableNameSeo, CompressionType.None, CachingStrategy.None)]
    internal class SeoDataRow : TableRowDefinition
    {
        string _route;
        string _title;
        string _author;
        string _description;
        private ObservableList<string> _keywords;

        public SeoDataRow()
        {
            Keywords = new ObservableList<string>();
            Keywords.Changed += ListUpdated;
        }

        private void ListUpdated(object sender, EventArgs e)
        {
            Update();
        }

        public string Route
        {
            get
            {
                return _route;
            }
        
            set
            {
                _route = value;
                Update();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0016:Use 'throw' expression", Justification = "Validation is required before removing the Changed event")]
        public ObservableList<string> Keywords 
        { 
            get
            {
                return _keywords;
            }

            set
            {
                if (value == null)
                    throw new InvalidOperationException();

                if (_keywords != null)
                    _keywords.Changed -= ListUpdated;

                _keywords = value;
                _keywords.Changed += ListUpdated;
            }
        }

        public string Title
        {
            get
            {
                return _title;
            }
        
            set
            {
                _title = value;
                Update();
            }
        }

        public string Description
        {
            get
            {
                return _description;
            }
        
            set
            {
                _description = value;
                Update();
            }
        }

        public string Author
        {
            get
            {
                return _author;
            }
        
            set
            {
                _author = value;
                Update();
            }
        }
    }
}
