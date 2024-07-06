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
 *  Product:  SimpleDB
 *  
 *  File: VersionedReadWriteFactory.cs
 *
 *  Purpose:  Factory class for retrieving read/write instances
 *
 *  Date        Name                Reason
 *  10/12/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using SimpleDB.Abstractions;
using SimpleDB.Interfaces;
using SimpleDB.Readers;
using SimpleDB.Writers;

namespace SimpleDB.Internal
{
	internal class VersionedReadWriteFactory : IVersionedReadWriteFactory
	{
		public IDataReader GetReader(ushort version)
		{
			switch (version)
			{
				case 0:
				case 1:
					return new TableReadVersionOne();

				case 2:
					return new TableReadVersionTwo();

				case 3:
					return new TableReadVersionThree();

				default:
					throw new ArgumentException(null, nameof(version));
			}
		}

		public IDataWriter GetWriter()
		{
			// always return the latest version
			return new TableWriteVersionThree();
		}
	}
}
