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
 *  File: SimpleDBHelper.cs
 *
 *  Purpose:  SimpleDB Helper Methods
 *
 *  Date        Name                Reason
 *  14/08/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using Microsoft.Extensions.DependencyInjection;

using SharedPluginFeatures;

using SimpleDB.Internal;

namespace SimpleDB
{
	/// <summary>
	/// Helper class for registering Simple Db with IoC container
	/// </summary>
	public static class SimpleDBHelper
	{
		/// <summary>
		/// Add required services
		/// </summary>
		/// <param name="services"></param>
		/// <returns></returns>
		public static IServiceCollection AddSimpleDB(this IServiceCollection services)
		{
			services.AddSingleton<IForeignKeyManager, ForeignKeyManager>();
			services.AddSingleton<ISimpleDBManager, SimpleDBManager>();
			services.AddSingleton(typeof(ISimpleDBOperations<>), typeof(SimpleDBOperations<>));
			services.AddSingleton<IDatabaseTimings, DatabaseTimings>();

			return services;
		}

		/// <summary>
		/// Add required services with specific path and encryption key
		/// </summary>
		/// <param name="services"></param>
		/// <param name="path"></param>
		/// <param name="encryptionKey"></param>
		/// <returns></returns>
		public static IServiceCollection AddSimpleDB(this IServiceCollection services, string path, string encryptionKey)
		{
			services.AddSingleton<IForeignKeyManager, ForeignKeyManager>();
			services.AddSingleton<ISimpleDBManager>(new SimpleDBManager(path, encryptionKey));
			services.AddSingleton(typeof(ISimpleDBOperations<>), typeof(SimpleDBOperations<>));
			services.AddSingleton<IDatabaseTimings, DatabaseTimings>();

			return services;
		}
	}
}
