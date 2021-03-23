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
 *  Copyright (c) 2018 - 2021 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginManager.Tests
 *  
 *  File: ThreadManagerInitialisationTests.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  21/03/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using PluginManager.Internal;
using PluginManager.Tests.Mocks;

using Shared.Classes;

namespace PluginManager.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ThreadManagerInitialisationTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Initialise_InvalidParam_Null_Throws_ArgumentNullException()
        {
            ThreadManagerInitialisation.Initialise(null);
        }

        [TestMethod]
        public void ThreadStarts_RaisesThreadStartMessage_Success()
        {
            TestLogger testLogger = new TestLogger();

            ThreadManagerInitialisation.Initialise(testLogger);

            ThreadManager.ThreadStart(new TestThreadThatStops(), "Test that stops", System.Threading.ThreadPriority.BelowNormal);

            ThreadManager.CancelAll(3);

            Assert.IsTrue(testLogger.ContainsMessage("ThreadManager Thread cancel all"));
            Assert.IsTrue(testLogger.Logs[1].Data.StartsWith("Thread started: Test that stops, "));
        }

        [TestMethod]
        public void ThreadStarts_ThreadThrowsExceptionAddedToLog_Success()
        {
            TestLogger testLogger = new TestLogger();

            ThreadManagerInitialisation.Initialise(testLogger);

            ThreadManager.ThreadStart(new TestThreadThatThrowsException(), "Test that stops", System.Threading.ThreadPriority.BelowNormal);

            Thread.Sleep(1000);
            ThreadManager.CancelAll(3);

            Assert.IsTrue(testLogger.ContainsMessage("ThreadManager Thread cancel all"));
            Assert.IsTrue(testLogger.Errors[0].Data.Contains("Thread exception raised: Test that stops, "));
        }
    }

    [ExcludeFromCodeCoverage]
    internal class TestThreadThatStops : ThreadManager
    {
        internal TestThreadThatStops()
            : base(null, new TimeSpan())
        {

        }

        protected override bool Run(object parameters)
        {
            return false;
        }
    }

    [ExcludeFromCodeCoverage]
    internal class TestThreadThatThrowsException : ThreadManager
    {
        internal TestThreadThatThrowsException()
            : base(null, new TimeSpan())
        {

        }

        protected override bool Run(object parameters)
        {
            throw new InvalidOperationException();
        }
    }
}
