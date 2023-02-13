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
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: MockPluginClassesService.cs
 *
 *  Purpose:  Test IPluginClassesService implementation
 *
 *  Date        Name                Reason
 *  29/03/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

using PluginManager.Abstractions;

namespace AspNetCore.PluginManager.Tests.Shared
{
    [ExcludeFromCodeCoverage]
    public class MockPluginClassesService : IPluginClassesService
    {
        private readonly List<object> _items;

        public MockPluginClassesService()
        {
            _items = new List<object>();
        }

        public MockPluginClassesService(List<object> items)
        {
            _items = items ?? new List<object>();
        }

        public List<T> GetPluginClasses<T>()
        {
            List<T> Result = new List<T>();

            if (_items != null)
            {
                foreach (object item in _items)
                {
                    if (item.GetType().IsAssignableTo(typeof(T)))
                        Result.Add((T)item);
                }
            }

            return Result;
        }

        public List<Type> GetPluginClassTypes<T>()
        {
            throw new NotImplementedException();
        }

		public object[] GetParameterInstances(Type type)
		{
			{
				if (type == null)
					throw new ArgumentNullException(nameof(type));

				List<object> Result = new List<object>();

				//grab a list of all constructors in the class, start with the one with most parameters
				List<ConstructorInfo> constructors = type.GetConstructors()
					.Where(c => c.IsPublic && !c.IsStatic && c.GetParameters().Length > 0)
					.OrderByDescending(c => c.GetParameters().Length)
					.ToList();

				foreach (ConstructorInfo constructor in constructors)
				{
					foreach (ParameterInfo param in constructor.GetParameters())
					{
						object paramClass = _items.FirstOrDefault(p => p.GetType().Equals(param.ParameterType));

						// if we didn't find a specific param type for this constructor, try the next constructor
						if (paramClass == null)
						{
							Result.Clear();
							break;
						}

						Result.Add(paramClass);
					}

					if (Result.Count > 0)
						return Result.ToArray();
				}

				return Result.ToArray();
			}
		}

		public List<object> Items => _items;
    }
}
