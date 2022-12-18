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
 *  Copyright (c) 2021 Simon Carter.  All Rights Reserved.
 *
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: ImportEmailIntoHelpdeskTests.cs
 *
 *  Purpose:  Unit tests for email importer
 *
 *  Date        Name                Reason
 *  15/12/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AspNetCore.PluginManager.Tests.Shared;

using HelpdeskPlugin.Classes;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using PluginManager.Internal;

using PluginManager.Tests.Mocks;

namespace AspNetCore.PluginManager.Tests.Plugins.HelpdeskTests
{
	[TestClass]
	[ExcludeFromCodeCoverage]
	public class ImportEmailIntoHelpdeskTests
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Construct_InvalidParameter_HelpdeskProviderNull_Throws_ArgumentNullException()
		{
			new ImportEmailIntoHelpdeskThread(null, new MockPop3ClientFactory(), new MockUserSearch(), new MockLogger());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Construct_InvalidParameter_Pop3ClientFactoryNull_Throws_ArgumentNullException()
		{
			new ImportEmailIntoHelpdeskThread(new MockHelpdeskProvider(), null, new MockUserSearch(), new MockLogger());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Construct_InvalidParameter_UserSearchNull_Throws_ArgumentNullException()
		{
			new ImportEmailIntoHelpdeskThread(new MockHelpdeskProvider(), new MockPop3ClientFactory(), null, new MockLogger());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Construct_InvalidParameter_LoggerNull_Throws_ArgumentNullException()
		{
			new ImportEmailIntoHelpdeskThread(new MockHelpdeskProvider(), new MockPop3ClientFactory(), new MockUserSearch(), null);
		}

		[TestMethod]
		public void ProcessIncomingEmails_NoEmailsToAdd_CompletesWithoutError()
		{
			MockHelpdeskProvider helpdeskProvider = new();
			ImportEmailIntoHelpdeskThread sut = new(helpdeskProvider, new MockPop3ClientFactory(), new MockUserSearch(), new MockLogger());
			sut.ProcessIncomingEmails();
			Assert.AreEqual(0, helpdeskProvider.TicketsSubmitted.Count);
		}

		[TestMethod]
		public void ProcessIncomingEmails_InvalidEmail_LogsAndDoesNotAddTicket()
		{
			MockLogger mockLogger = new();
			MockHelpdeskProvider helpdeskProvider = new();
			MockPop3Client mockPop3Client = new MockPop3Client();

			mockPop3Client.Messages.Add("not a real email");
			MockPop3ClientFactory mockPop3ClientFactory = new(mockPop3Client);

			ImportEmailIntoHelpdeskThread sut = new(helpdeskProvider, mockPop3ClientFactory, new MockUserSearch(), mockLogger);
			sut.ProcessIncomingEmails();
			Assert.AreEqual(0, helpdeskProvider.TicketsSubmitted.Count);
			Assert.AreEqual("Invalid email received from Pop3 Server", mockLogger.Logs[0].Data);
		}

		[TestMethod]
		public void ProcessIncomingEmails_ThreeNewEmailsToAdd_CompletesProcessingCorrectly()
		{
			MockPop3Client mockPop3Client = new MockPop3Client();

			mockPop3Client.Messages.Add(Properties.Resources.TestEmail1);
			mockPop3Client.Messages.Add(Properties.Resources.TestEmail2);
			mockPop3Client.Messages.Add(Properties.Resources.TestEmail3);

			MockHelpdeskProvider helpdeskProvider = new();
			MockPop3ClientFactory mockPop3ClientFactory = new(mockPop3Client);

			ImportEmailIntoHelpdeskThread sut = new(helpdeskProvider, mockPop3ClientFactory, new MockUserSearch(), new MockLogger());

			sut.ProcessIncomingEmails();

			Assert.IsTrue(mockPop3Client.DisposeCalled);
			Assert.AreEqual("removed", mockPop3Client.Messages[0]);
			Assert.AreEqual("removed", mockPop3Client.Messages[1]);
			Assert.AreEqual("removed", mockPop3Client.Messages[2]);

			Assert.AreEqual(3, helpdeskProvider.TicketsSubmitted.Count);
			Assert.AreEqual(2, helpdeskProvider.TicketsSubmitted[0].Department.Id);
			Assert.AreEqual(2, helpdeskProvider.TicketsSubmitted[0].Status.Id);
			Assert.AreEqual(2, helpdeskProvider.TicketsSubmitted[0].Priority.Id);
			Assert.AreEqual(2, helpdeskProvider.TicketsSubmitted[0].Messages.Count);
		}

		[TestMethod]
		public void ProcessIncomingEmails_AddToExistingEmailWithSameId_CompletesProcessingCorrectly()
		{
			MockPop3Client mockPop3Client = new MockPop3Client();

			mockPop3Client.Messages.Add(Properties.Resources.TestEmail1);
			mockPop3Client.Messages.Add(Properties.Resources.TestEmail2);
			mockPop3Client.Messages.Add(Properties.Resources.TestEmail3);
			mockPop3Client.Messages.Add(Properties.Resources.TestEmail1); 

			MockHelpdeskProvider helpdeskProvider = new();
			MockPop3ClientFactory mockPop3ClientFactory = new(mockPop3Client);

			ImportEmailIntoHelpdeskThread sut = new(helpdeskProvider, mockPop3ClientFactory, new MockUserSearch(), new MockLogger());

			sut.ProcessIncomingEmails();

			Assert.IsTrue(mockPop3Client.DisposeCalled);
			Assert.AreEqual("removed", mockPop3Client.Messages[0]);
			Assert.AreEqual("removed", mockPop3Client.Messages[1]);
			Assert.AreEqual("removed", mockPop3Client.Messages[2]);

			Assert.AreEqual(3, helpdeskProvider.TicketsSubmitted.Count);
			Assert.AreEqual(2, helpdeskProvider.TicketsSubmitted[0].Department.Id);
			Assert.AreEqual(2, helpdeskProvider.TicketsSubmitted[0].Status.Id);
			Assert.AreEqual(2, helpdeskProvider.TicketsSubmitted[0].Priority.Id);

			Assert.AreEqual(2, helpdeskProvider.TicketsSubmitted[0].Messages.Count);
		}
	}
}
