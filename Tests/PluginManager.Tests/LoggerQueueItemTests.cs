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
 *  File: LoggerQueueItemTests.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  21/03/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Diagnostics.CodeAnalysis;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PluginManager.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class LoggerQueueItemTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_Message_Null_Throws_ArgumentNullException()
        {
            new LoggerQueueItem(LogLevel.Error, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_Message_EmptyString_Throws_ArgumentNullException()
        {
            new LoggerQueueItem(LogLevel.Error, "");
        }

        [TestMethod]
        public void Construct_ValidLoggerQueueItem_Success()
        {
            DateTime createDate = DateTime.Now;
            LoggerQueueItem sut = new LoggerQueueItem(LogLevel.Error, "some data");

            Assert.IsTrue(sut.Date.Ticks >= createDate.Ticks);
            Assert.AreEqual(LogLevel.Error, sut.Level);
            Assert.AreEqual("some data", sut.Message);
        }
    }
}
