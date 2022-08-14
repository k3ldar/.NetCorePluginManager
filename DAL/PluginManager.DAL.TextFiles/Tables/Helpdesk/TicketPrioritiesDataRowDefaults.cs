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
 *  File: TicketPrioritiesDataRowDefaults.cs
 *
 *  Purpose:  Default values for ticket priorities
 *
 *  Date        Name                Reason
 *  19/07/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using SimpleDB;

namespace PluginManager.DAL.TextFiles.Tables
{
    internal class TicketPrioritiesDataRowDefaults : ITableDefaults<TicketPrioritiesDataRow>
    {
        public long PrimarySequence => 0;

        public long SecondarySequence => 0;

        public ushort Version => 1;

        public List<TicketPrioritiesDataRow> InitialData(ushort version)
        {
            if (version == 1)
            {
                return new List<TicketPrioritiesDataRow>()
                {
                    new TicketPrioritiesDataRow() { Description = "Low" },
                    new TicketPrioritiesDataRow() { Description = "Medium" },
                    new TicketPrioritiesDataRow() { Description = "High" }
                };
            }

            return null;
        }
    }
}
