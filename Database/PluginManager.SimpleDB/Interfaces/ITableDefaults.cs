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
 *  Product:  SimpleDB
 *  
 *  File: ITableDefaults.cs
 *
 *  Purpose:  Interface for default table values
 *
 *  Date        Name                Reason
 *  09/06/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
namespace SimpleDB
{
	public interface ITableDefaults<T>
        where T : TableRowDefinition
    {
        /// <summary>
        /// Initial primary sequence provided for the table
        /// </summary>
        long PrimarySequence { get; }

        /// <summary>
        /// Secondary sequence
        /// </summary>
        long SecondarySequence { get; }

        /// <summary>
        /// Latest version of data row
        /// </summary>
        ushort Version { get; }

        /// <summary>
        /// Initial data that will be added when the table is first created and for each upgrade
        /// </summary>
        List<T> InitialData(ushort version);
    }
}
