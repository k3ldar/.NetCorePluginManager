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
 *  Product:  PluginManager.Tests
 *  
 *  File: MockServiceCollection.cs
 *
 *  Purpose:  Mock IServiceCollection for unit tests
 *
 *  Date        Name                Reason
 *  17/03/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using PluginManager.Internal;

namespace PluginManager.Tests.Mocks
{
	[ExcludeFromCodeCoverage]
	public class MockServiceCollection : IServiceCollection
	{
		private readonly List<ServiceDescriptor> _serviceDescriptors;
		private readonly NotificationService _notificationService;

		public MockServiceCollection(ServiceDescriptor[] serviceDescriptors = null)
		{
			_serviceDescriptors = new List<ServiceDescriptor>();

			if (serviceDescriptors != null)
			{
				foreach (ServiceDescriptor serviceDescriptor in serviceDescriptors)
				{
					if (serviceDescriptor.ImplementationInstance != null &&
						serviceDescriptor.ImplementationInstance.GetType().Equals(typeof(NotificationService)))
					{
						_notificationService = serviceDescriptor.ImplementationInstance as NotificationService;
					}

					Add(serviceDescriptor);
				}
			}

			ServicesRegistered = 0;
		}

		public int ServicesRegistered { get; set; }

		public T GetServiceInstance<T>(ServiceLifetime serviceLifetime)
		{
			ServiceDescriptor descriptor = _serviceDescriptors.Find(sd => sd.Lifetime.Equals(serviceLifetime) && sd.ServiceType.Equals(typeof(T)));

			if (descriptor == null)
				return default;

			return (T)descriptor.ImplementationInstance;
		}

		public bool HasServiceRegistered<T>(ServiceLifetime serviceLifetime)
		{
			bool Result = _serviceDescriptors.Exists(sd => sd.Lifetime.Equals(serviceLifetime) && sd.ServiceType != null && sd.ServiceType.Equals(typeof(T)));

			if (!Result)
			{
				Result = _serviceDescriptors.Exists(sd => sd.Lifetime.Equals(serviceLifetime) && sd.ServiceType != null && GetNameWithoutGenericArity(sd.ServiceType).Equals(GetNameWithoutGenericArity(typeof(T))));
			}

			return Result;
		}

		public bool HasServiceRegistered(ServiceLifetime serviceLifetime, Type serviceType)
		{
			bool Result = _serviceDescriptors.Exists(sd => sd.Lifetime.Equals(serviceLifetime) && sd.ServiceType != null && sd.ServiceType.Equals(serviceType));

			if (!Result)
			{
				Result = _serviceDescriptors.Exists(sd => sd.Lifetime.Equals(serviceLifetime) && sd.ServiceType != null && GetNameWithoutGenericArity(sd.ServiceType).Equals(GetNameWithoutGenericArity(serviceType)));
			}

			return Result;
		}

		public bool HasListenerRegistered<T>()
		{
			if (_notificationService == null)
				return false;

			return _notificationService.ListenerRegistered<T>();
		}

		public bool HasListenerRegisteredEvent<T>(string eventName)
		{
			if (_notificationService == null)
				return false;

			return _notificationService.ListenerRegisteredEvent<T>(eventName);
		}

		public bool HasPolicyConfigured(string policyName, string[] requiredClaimNames)
		{
			List<ServiceDescriptor> configureOptions = _serviceDescriptors.Where(sd => sd.ServiceType != null && sd.ServiceType.Equals(typeof(IConfigureOptions<AuthorizationOptions>))).ToList();

			if (configureOptions == null)
				return false;

			foreach (ServiceDescriptor configureOption in configureOptions)
			{
				ConfigureNamedOptions<AuthorizationOptions> configureNamedOptions = (ConfigureNamedOptions<AuthorizationOptions>)configureOption.ImplementationInstance;

				AuthorizationOptions authorizationOptions = new();
				configureNamedOptions.Action.Invoke(authorizationOptions);

				AuthorizationPolicy authorizationPolicy = authorizationOptions.GetPolicy(policyName);

				if (authorizationPolicy == null)
					continue;

				Assert.AreEqual(requiredClaimNames.Length, authorizationPolicy.Requirements.Count, "Policy claim count does not match expected claim count");

				foreach (ClaimsAuthorizationRequirement requirement in authorizationPolicy.Requirements)
				{
					if (!requiredClaimNames.Contains(requirement.ClaimType))
						continue;
				}

				return true;
			}

			return false;
		}

		public bool HasMvcEndpointRouting()
		{
			List<ServiceDescriptor> configureOptions = _serviceDescriptors
				.Where(sd => sd.ServiceType != null && sd.ImplementationInstance != null && sd.Lifetime == ServiceLifetime.Singleton && sd.ServiceType.Equals(typeof(IConfigureOptions<MvcOptions>)))
				.ToList();

			Assert.IsNotNull(configureOptions, "Could not find ServiceDescriptor for MvcOptions");

			foreach (ServiceDescriptor configureOption in configureOptions)
			{
				ConfigureNamedOptions<MvcOptions> configureNamedOptions = (ConfigureNamedOptions<MvcOptions>)configureOption.ImplementationInstance;

				MvcOptions authorizationOptions = new();
				configureNamedOptions.Action.Invoke(authorizationOptions);

				return authorizationOptions.EnableEndpointRouting;
			}

			throw new InvalidOperationException("Could not find ServiceDescriptor for MvcOptions");
		}

		public bool HasMvcConfigured()
		{
			List<ServiceDescriptor> configureOptions = _serviceDescriptors
				.Where(sd => sd.ServiceType != null && sd.Lifetime == ServiceLifetime.Singleton && sd.ServiceType.Equals(typeof(IActionInvokerFactory)))
				.ToList();

			return configureOptions != null;
		}

		public bool HasSessionStateTempDataProvider()
		{
			List<ServiceDescriptor> configureOptions = _serviceDescriptors
				.Where(sd => sd.ServiceType != null && sd.Lifetime == ServiceLifetime.Singleton && sd.ServiceType.Equals(typeof(IMvcBuilder)))
				.ToList();

			return configureOptions != null;
		}


		public bool HasConfigurationOptions(Type options, ServiceLifetime serviceLifetime)
		{
			List<ServiceDescriptor> configureOptions = _serviceDescriptors
				.Where(sd => sd.ServiceType != null && sd.Lifetime == serviceLifetime && sd.ServiceType.Equals(options))
				.ToList();

			return configureOptions != null;
		}

		#region IServiceCollection 

		public ServiceDescriptor this[int index] { get => _serviceDescriptors[index]; set => throw new NotImplementedException(); }

		public int Count => _serviceDescriptors.Count;

		public bool IsReadOnly => false;

		public void Add(ServiceDescriptor item)
		{
			_serviceDescriptors.Add(item);
			ServicesRegistered++;
		}

		public void Clear()
		{
			throw new NotImplementedException();
		}

		public bool Contains(ServiceDescriptor item)
		{
			throw new NotImplementedException();
		}

		public void CopyTo(ServiceDescriptor[] array, int arrayIndex)
		{
			_serviceDescriptors.CopyTo(array, arrayIndex);
		}

		public IEnumerator<ServiceDescriptor> GetEnumerator()
		{
			return _serviceDescriptors.GetEnumerator();
		}

		public int IndexOf(ServiceDescriptor item)
		{
			throw new NotImplementedException();
		}

		public void Insert(int index, ServiceDescriptor item)
		{
			throw new NotImplementedException();
		}

		public bool Remove(ServiceDescriptor item)
		{
			return _serviceDescriptors.Remove(item);
		}

		public void RemoveAt(int index)
		{
			_serviceDescriptors.RemoveAt(index);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			throw new NotImplementedException();
		}

		#endregion IServiceCollection

		#region Private Methods

		private string GetNameWithoutGenericArity(Type t)
		{
			string name = t.Name;
			int index = name.IndexOf('`');
			return index == -1 ? name : name.Substring(0, index);
		}

		#endregion Private Methods
	}
}
