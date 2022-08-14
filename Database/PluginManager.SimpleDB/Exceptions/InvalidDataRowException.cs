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
 *  File: InvalidDataRowException.cs
 *
 *  Purpose:  InvalidDataRowException for validation issues when inserting/updating data rows
 *
 *  Date        Name                Reason
 *  28/06/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

namespace SimpleDB
{
    public sealed class InvalidDataRowException : Exception
    {
        public InvalidDataRowException(string dataRow, string property, string message)
            : base($"{message}; Table: {dataRow}; Property {property}")
        {
            if (String.IsNullOrEmpty(dataRow))
                throw new ArgumentNullException(nameof(dataRow));

            if (String.IsNullOrEmpty(property))
                throw new ArgumentNullException(nameof(property));

            if (String.IsNullOrEmpty(message))
                throw new ArgumentNullException(nameof(message));

            DataRow = dataRow;
            Property = property;
            OriginalMessage = message;
        }

        public string DataRow { get; }

        public string Property { get; }

        public string OriginalMessage { get; }
    }
}
