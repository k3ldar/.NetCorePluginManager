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
 *  Product:  PluginManager.Tests
 *  
 *  File: DefaultLoggerTests.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  21/03/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using PluginManager.Internal;

namespace PluginManager.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class DefaultLoggerTests
    {
        private TestTraceListener _traceListner;

        [TestInitialize]
        public void InitializeTests()
        {
            _traceListner = new TestTraceListener();

            Trace.Listeners.Add(_traceListner);
        }

        [TestCleanup]
        public void FinalizeTests()
        {
            Trace.Listeners.Remove(_traceListner);
        }

        [TestMethod]
        public void AddToLog_SingleData_Success()
        {
            DefaultLogger sut = new DefaultLogger();

            sut.AddToLog(LogLevel.Information, "single line of information");

#if TRACE
            _traceListner.ListnerLines.Contains("Information  single line of information");
#endif
        }

        [TestMethod]
        public void AddToLog_SingleDataWithModuleName_Success()
        {
            DefaultLogger sut = new DefaultLogger();

            sut.AddToLog(LogLevel.Information, "mymodule", "next line of information");

#if TRACE
            _traceListner.ListnerLines.Contains("Information mymodule next line of information");
#endif
        }

        [TestMethod]
        public void AddToLog_Exception_Success()
        {
            DefaultLogger sut = new DefaultLogger();

            sut.AddToLog(LogLevel.Warning, new Exception("Exception with no module name"));

#if TRACE
            _traceListner.ListnerLines.Contains("Warning  Exception with no module name");
#endif
        }

        [TestMethod]
        public void AddToLog_ExceptionWithData_Success()
        {
            DefaultLogger sut = new DefaultLogger();

            sut.AddToLog(LogLevel.Warning, new Exception("Exception with no data"), "a bit of data");

#if TRACE
            _traceListner.ListnerLines.Contains("Warning\t\tException with no data\ta bit of data");
#endif
        }

        [TestMethod]
        public void AddToLog_ExceptionDataWithModuleName_Success()
        {
            DefaultLogger sut = new DefaultLogger();

            sut.AddToLog(LogLevel.Error, "modulename", new Exception("Exception with module name"), "some extra data");

#if TRACE
            _traceListner.ListnerLines.Contains("Error\tmodulename\tException with module name\tsome extra data");
#endif
        }

    }

    [ExcludeFromCodeCoverage]
    internal sealed class TestTraceListener : TraceListener
    {
        internal TestTraceListener()
        {
            ListnerLines = new List<string>();
        }

        public override void Write(string message)
        {
            throw new NotImplementedException();
        }

        public override void WriteLine(string message)
        {
            ListnerLines.Add(message);
        }

        public List<string> ListnerLines { get; }
    }
}
