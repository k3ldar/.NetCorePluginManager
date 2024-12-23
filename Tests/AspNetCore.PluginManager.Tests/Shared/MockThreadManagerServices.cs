﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
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
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: MockThreadManagerServices.cs
 *
 *  Purpose:  Mock IThreadManagerServices class
 *
 *  Date        Name                Reason
 *  26/07/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using PluginManager.Abstractions;

using Shared.Classes;

namespace AspNetCore.PluginManager.Tests.Shared
{
    [ExcludeFromCodeCoverage]
    public class MockThreadManagerServices : IThreadManagerServices
    {
        private readonly Dictionary<string, Type> _registeredThreads = new Dictionary<string, Type>();

        public void RegisterStartupThread(string threadName, Type type)
        {
            if (String.IsNullOrEmpty(threadName))
                throw new ArgumentNullException(nameof(threadName));

            if (_registeredThreads.ContainsKey(threadName))
                throw new InvalidOperationException("Thread name is already registered");

            if (!type.IsSubclassOf(typeof(ThreadManager)))
                throw new ArgumentException("Type must descend from ThreadManager class");

            _registeredThreads.Add(threadName, type);
        }

        public bool ContainsRegisteredStartupThread(string threadName, Type type)
        {
            return _registeredThreads.ContainsKey(threadName) &&
                _registeredThreads[threadName].IsEquivalentTo(type);
        }
    }
}
