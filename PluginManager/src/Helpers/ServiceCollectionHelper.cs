/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 *  .Net Core Plugin Manager is distributed under the GNU General Public License version 3 and  
 *  is also available under alternative licenses negotiated directly with Simon Carter.  
 *  If you obtained Service Manager under the GPL, then the GPL applies to all loadable 
 *  Service Manager modules used on your system as well. The GPL (version 3) is 
 *  available at https://opensource.org/licenses/GPL-3.0
 *
 *  This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY,
 *  without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 *  See the GNU General Public License for more details.
 *
 *  The Original Code was created by Simon Carter (s1cart3r@gmail.com)
 *
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginManager
 *  
 *  File: ServiceCollectionHelper.cs
 *
 *  Purpose:  Allows the retrieval of class instances from an IServiceCollection
 *
 *  Date        Name                Reason
 *  27/09/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

using Microsoft.Extensions.DependencyInjection;

using Shared.Classes;

namespace PluginManager
{
	/// <summary>
	/// IServiceCollection helper method which allows the retrieval of a class instance prior to the 
	/// Service Provider being built
	/// </summary>
	public static class ServiceCollectionHelper
	{
		private static readonly object _lockObject = new();

		/// <summary>
		/// Retrieves an instance of a class from within an IServiceCollection
		/// </summary>
		/// <typeparam name="T">Type of class instance being sought</typeparam>
		/// <param name="serviceCollection"></param>
		/// <returns>Instance of type T if found within the service collection, otherwise null</returns>
		public static T GetServiceInstance<T>(this IServiceCollection serviceCollection) where T : class
		{
			if (serviceCollection == null)
				return null;

			using (TimedLock tl = TimedLock.Lock(_lockObject))
			{
				return GetClassImplementation<T>(serviceCollection, typeof(T));
			}
		}

		#region Private Methods

		private static T GetClassImplementation<T>(IServiceCollection serviceCollection, Type classType) where T : class
		{
			ServiceDescriptor sd = serviceCollection
				.FirstOrDefault(sd => GetNameWithoutGenericArity(sd.ServiceType).Equals(GetNameWithoutGenericArity(classType)));

			if (sd == null)
				return null;

			T Result = null;

			if (sd.ImplementationInstance != null)
			{
				Result = (T)sd.ImplementationInstance;
			}
			else if (sd.ImplementationType != null)
			{
				Result = (T)Activator.CreateInstance(sd.ImplementationType, GetInstancesConstructorParameters(serviceCollection, sd.ImplementationType));

				if (sd.Lifetime == ServiceLifetime.Singleton)
				{
					ServiceDescriptor replacementServiceDescriptor = new(sd.ServiceType, Result);

					serviceCollection.Remove(sd);
					serviceCollection.Add(replacementServiceDescriptor);
				}
			}
			else if (sd.ImplementationFactory != null)
			{
				Result = sd.ImplementationFactory.Invoke(null) as T;
			}

			return Result;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0057:Use range operator", Justification = "Not available for net standard")]
		private static string GetNameWithoutGenericArity(Type t)
		{
			string name = t.FullName;
			int index = name.IndexOf('`');

			return index == -1 ? name : name.Substring(0, index);
		}

		internal static object[] GetInstancesConstructorParameters(IServiceCollection serviceCollection, Type type)
		{
			List<object> Result = [];

			//grab a list of all constructors in the class, start with the one with most parameters
			List<ConstructorInfo> constructors = [.. type.GetConstructors()
				.Where(c => c.IsPublic && !c.IsStatic && c.GetParameters().Length > 0)
				.OrderByDescending(c => c.GetParameters().Length)];

			foreach (ConstructorInfo constructor in constructors)
			{
				foreach (ParameterInfo param in constructor.GetParameters())
				{
					object paramClass = GetClassImplementation<object>(serviceCollection, param.ParameterType);

					// if we didn't find a specific param type for this constructor, try the next constructor
					if (paramClass == null)
					{
						Result.Clear();
						break;
					}

					Result.Add(paramClass);
				}

				if (Result.Count > 0)
					return [.. Result];
			}

			return [.. Result];
		}


		#endregion Private Methods

	}
}
