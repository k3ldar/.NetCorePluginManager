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
 *  File: TableRowDefinition.cs
 *
 *  Purpose:  Table Row Definition for a table table
 *
 *  Date        Name                Reason
 *  25/05/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginManager.DAL.TextFiles
{
    /// <summary>
    /// Base class for all table row types
    /// </summary>
    public abstract class TableRowDefinition
    {
        private long _id;

        /// <summary>
        /// Unique id of the record
        /// </summary>
        /// <value>long</value>
        [UniqueIndex(IndexType.Ascending)]
        public long Id
        { 
            get => _id;

            set
            {
                if (ImmutableId)
                    throw new InvalidOperationException();

                _id = value;
            }
        }

        /// <summary>
        /// Indicates whether the row has been marked for delete or not
        /// </summary>
        protected internal bool ImmutableId { get; set; } = false;
    }
}
