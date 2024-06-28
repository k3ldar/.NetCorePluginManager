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
 *  File: MockLoggerFactory.cs
 *
 *  Purpose:  Mock ILoggerFactory for unit tests
 *
 *  Date        Name                Reason
 *  01/04/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.Logging;

namespace PluginManager.Tests.Mocks
{
	[ExcludeFromCodeCoverage]
	public sealed class MockLoggerFactory : ILoggerFactory
	{
		public void AddProvider(ILoggerProvider provider)
		{

		}

		public ILogger CreateLogger(string categoryName)
		{
			return new MockMicrosoftLogger();
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
	}

	[ExcludeFromCodeCoverage]
	public class MockMicrosoftLogger : ILogger
	{
		public IDisposable BeginScope<TState>(TState state)
		{
			throw new NotImplementedException();
		}

		public bool IsEnabled(Microsoft.Extensions.Logging.LogLevel logLevel)
		{
			return true;
		}

		public void Log<TState>(Microsoft.Extensions.Logging.LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
		{

		}
	}
}
