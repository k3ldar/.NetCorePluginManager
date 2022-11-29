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
 *  Product:  PluginManager.DAL.TextFiles.Tests
 *  
 *  File: HelpdeskProviderTests.cs
 *
 *  Purpose:  help desk provider tests Tests for text based storage
 *
 *  Date        Name                Reason
 *  18/07/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using AspNetCore.PluginManager.Tests.Shared;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Middleware;
using Middleware.Helpdesk;

using PluginManager.Abstractions;
using PluginManager.DAL.TextFiles.Providers;
using PluginManager.DAL.TextFiles.Tables;
using SimpleDB;
using PluginManager.Tests.Mocks;
using Shared.Classes;

namespace PluginManager.DAL.TextFiles.Tests.Providers
{
	[TestClass]
    public class HelpdeskProviderTests : BaseProviderTests
    {
		[TestInitialize]
		public void Setup()
		{
			ThreadManager.Initialise();
		}

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_UsersNull_Throws_ArgumentNullException()
        {
            new HelpdeskProvider(null, null, null, null, null, null, null, null);
        }

        [TestMethod]
        public void GetTicketDepartments_ReturnsListOfDefaultDepartments()
        {
            string directory = TestHelper.GetTestPath();
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    mockPluginClassesService.Items.Add(new UserDataRowDefaults(provider.GetService<ISettingsProvider>()));

                    IHelpdeskProvider sut = provider.GetService<IHelpdeskProvider>();
                    Assert.IsNotNull(sut);
                    HelpdeskProvider.ClearCache();

                    List<LookupListItem> result = sut.GetTicketDepartments();

                    Assert.IsNotNull(result);
                    Assert.AreEqual(3, result.Count);

                    Assert.AreEqual(1, result[0].Id);
                    Assert.AreEqual("Sales", result[0].Description);

                    Assert.AreEqual(2, result[1].Id);
                    Assert.AreEqual("Support", result[1].Description);

                    Assert.AreEqual(3, result[2].Id);
                    Assert.AreEqual("Returns", result[2].Description);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void GetTicketPriorities_ReturnsListOfDefaultPriorities()
        {
            string directory = TestHelper.GetTestPath();
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    mockPluginClassesService.Items.Add(new UserDataRowDefaults(provider.GetService<ISettingsProvider>()));

                    IHelpdeskProvider sut = provider.GetService<IHelpdeskProvider>();
                    Assert.IsNotNull(sut);
                    HelpdeskProvider.ClearCache();

                    List<LookupListItem> result = sut.GetTicketPriorities();

                    Assert.IsNotNull(result);
                    Assert.AreEqual(3, result.Count);

                    Assert.AreEqual(1, result[0].Id);
                    Assert.AreEqual("Low", result[0].Description);

                    Assert.AreEqual(2, result[1].Id);
                    Assert.AreEqual("Medium", result[1].Description);

                    Assert.AreEqual(3, result[2].Id);
                    Assert.AreEqual("High", result[2].Description);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void GetTicketStatus_ReturnsListOfDefaultStatuses()
        {
            string directory = TestHelper.GetTestPath();
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    mockPluginClassesService.Items.Add(new UserDataRowDefaults(provider.GetService<ISettingsProvider>()));

                    IHelpdeskProvider sut = provider.GetService<IHelpdeskProvider>();
                    Assert.IsNotNull(sut);
                    HelpdeskProvider.ClearCache();

                    List<LookupListItem> result = sut.GetTicketStatus();

                    Assert.IsNotNull(result);
                    Assert.AreEqual(3, result.Count);

                    Assert.AreEqual(1, result[0].Id);
                    Assert.AreEqual("Closed", result[0].Description);

                    Assert.AreEqual(2, result[1].Id);
                    Assert.AreEqual("Open", result[1].Description);

                    Assert.AreEqual(3, result[2].Id);
                    Assert.AreEqual("On Hold", result[2].Description);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void GetFeedback_AllFeedBack_ReturnsEmptyList()
        {
            string directory = TestHelper.GetTestPath();
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    mockPluginClassesService.Items.Add(new UserDataRowDefaults(provider.GetService<ISettingsProvider>()));

                    IHelpdeskProvider sut = provider.GetService<IHelpdeskProvider>();
                    Assert.IsNotNull(sut);
                    HelpdeskProvider.ClearCache();

                    List<Feedback> result = sut.GetFeedback(false);

                    Assert.IsNotNull(result);
                    Assert.AreEqual(0, result.Count);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void GetFeedback_NoFeedbackAllowedOnWebsite_ReturnsEmptyList()
        {
            string directory = TestHelper.GetTestPath();
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    MockApplicationBuilder mockApplicationBuilder = new MockApplicationBuilder(provider);
                    initialisation.AfterConfigure(mockApplicationBuilder);

                    mockPluginClassesService.Items.Add(new UserDataRowDefaults(provider.GetService<ISettingsProvider>()));

                    ISimpleDBOperations<FeedbackDataRow> feedbackData = provider.GetRequiredService<ISimpleDBOperations<FeedbackDataRow>>();
                    Assert.IsNotNull(feedbackData);

                    feedbackData.Insert(new List<FeedbackDataRow> 
                    { 
                        new FeedbackDataRow() { UserName = "a user", ShowOnWebsite = false, Message = "a message" },
                        new FeedbackDataRow() { UserName = "other user", ShowOnWebsite = false, Message = "another message" }
                    });

                    IHelpdeskProvider sut = provider.GetService<IHelpdeskProvider>();
                    Assert.IsNotNull(sut);
                    HelpdeskProvider.ClearCache();

                    List<Feedback> result = sut.GetFeedback(true);

                    Assert.IsNotNull(result);
                    Assert.AreEqual(0, result.Count);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void GetFeedback_OnlyFeedbackAllowedOnWebsite_ReturnsTwoItems()
        {
            string directory = TestHelper.GetTestPath();
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    MockApplicationBuilder mockApplicationBuilder = new MockApplicationBuilder(provider);
                    initialisation.AfterConfigure(mockApplicationBuilder);

                    mockPluginClassesService.Items.Add(new UserDataRowDefaults(provider.GetService<ISettingsProvider>()));

                    ISimpleDBOperations<FeedbackDataRow> feedbackData = provider.GetRequiredService<ISimpleDBOperations<FeedbackDataRow>>();
                    Assert.IsNotNull(feedbackData);

                    feedbackData.Insert(new List<FeedbackDataRow>
                    {
                        new FeedbackDataRow() { UserName = "a user", ShowOnWebsite = true, Message = "a message" },
                        new FeedbackDataRow() { UserName = "other user", ShowOnWebsite = false, Message = "another message" },
                        new FeedbackDataRow() { UserName = "user b", ShowOnWebsite = true, Message = "a message from user b" },
                    });

                    Assert.AreEqual(3, feedbackData.RecordCount);
                    IHelpdeskProvider sut = provider.GetService<IHelpdeskProvider>();
                    Assert.IsNotNull(sut);
                    HelpdeskProvider.ClearCache();

                    List<Feedback> result = sut.GetFeedback(true);

                    Assert.IsNotNull(result);
                    Assert.AreEqual(2, result.Count);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void GetFeedback_GetAllFeedback_ReturnsThreeItems()
        {
            string directory = TestHelper.GetTestPath();
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    MockApplicationBuilder mockApplicationBuilder = new MockApplicationBuilder(provider);
                    initialisation.AfterConfigure(mockApplicationBuilder);

                    mockPluginClassesService.Items.Add(new UserDataRowDefaults(provider.GetService<ISettingsProvider>()));

                    ISimpleDBOperations<FeedbackDataRow> feedbackData = provider.GetRequiredService<ISimpleDBOperations<FeedbackDataRow>>();
                    Assert.IsNotNull(feedbackData);

                    feedbackData.Insert(new List<FeedbackDataRow>
                    {
                        new FeedbackDataRow() { UserName = "a user", ShowOnWebsite = true, Message = "a message" },
                        new FeedbackDataRow() { UserName = "other user", ShowOnWebsite = false, Message = "another message" },
                        new FeedbackDataRow() { UserName = "user b", ShowOnWebsite = true, Message = "a message from user b" },
                    });

                    Assert.AreEqual(3, feedbackData.RecordCount);
                    IHelpdeskProvider sut = provider.GetService<IHelpdeskProvider>();
                    Assert.IsNotNull(sut);
                    HelpdeskProvider.ClearCache();

                    List<Feedback> result = sut.GetFeedback(false);

                    Assert.IsNotNull(result);
                    Assert.AreEqual(3, result.Count);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void SubmitFeedback_FeedbackShowOnWebsiteIsFalse_Success()
        {
            string directory = TestHelper.GetTestPath();
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    MockApplicationBuilder mockApplicationBuilder = new MockApplicationBuilder(provider);
                    initialisation.AfterConfigure(mockApplicationBuilder);

                    mockPluginClassesService.Items.Add(new UserDataRowDefaults(provider.GetService<ISettingsProvider>()));

                    ISimpleDBOperations<FeedbackDataRow> feedbackData = provider.GetRequiredService<ISimpleDBOperations<FeedbackDataRow>>();
                    Assert.IsNotNull(feedbackData);
                    Assert.AreEqual(0, feedbackData.RecordCount);

                    IHelpdeskProvider sut = provider.GetService<IHelpdeskProvider>();
                    Assert.IsNotNull(sut);
                    HelpdeskProvider.ClearCache();

                    bool submitted = sut.SubmitFeedback(0, "Just me", "Hello from here");
                    Assert.IsTrue(submitted);

                    List<Feedback> result = sut.GetFeedback(false);

                    Assert.IsNotNull(result);
                    Assert.AreEqual(1, result.Count);
                    Assert.AreEqual(1, feedbackData.RecordCount);

                    result = sut.GetFeedback(true);
                    Assert.IsNotNull(result);
                    Assert.AreEqual(0, result.Count);
                    Assert.AreEqual(1, feedbackData.RecordCount);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void SubmitTicket_InvalidUserName_Null_ReturnsFalse()
        {
            string directory = TestHelper.GetTestPath();
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    MockApplicationBuilder mockApplicationBuilder = new MockApplicationBuilder(provider);
                    initialisation.AfterConfigure(mockApplicationBuilder);

                    mockPluginClassesService.Items.Add(new UserDataRowDefaults(provider.GetService<ISettingsProvider>()));

                    IHelpdeskProvider sut = provider.GetService<IHelpdeskProvider>();
                    Assert.IsNotNull(sut);
                    HelpdeskProvider.ClearCache();

                    bool submitted = sut.SubmitTicket(0, 1, 1, null, "email@address.com", "subject", "message", out HelpdeskTicket ticket);
                    Assert.IsFalse(submitted);
                    Assert.IsNull(ticket);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void SubmitTicket_InvalidEmail_Null_ReturnsFalse()
        {
            string directory = TestHelper.GetTestPath();
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    MockApplicationBuilder mockApplicationBuilder = new MockApplicationBuilder(provider);
                    initialisation.AfterConfigure(mockApplicationBuilder);

                    mockPluginClassesService.Items.Add(new UserDataRowDefaults(provider.GetService<ISettingsProvider>()));

                    IHelpdeskProvider sut = provider.GetService<IHelpdeskProvider>();
                    Assert.IsNotNull(sut);
                    HelpdeskProvider.ClearCache();

                    bool submitted = sut.SubmitTicket(0, 1, 1, "Jane Doe", null, "subject", "message", out HelpdeskTicket ticket);
                    Assert.IsFalse(submitted);
                    Assert.IsNull(ticket);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void SubmitTicket_InvalidSubject_Null_ReturnsFalse()
        {
            string directory = TestHelper.GetTestPath();
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    MockApplicationBuilder mockApplicationBuilder = new MockApplicationBuilder(provider);
                    initialisation.AfterConfigure(mockApplicationBuilder);

                    mockPluginClassesService.Items.Add(new UserDataRowDefaults(provider.GetService<ISettingsProvider>()));

                    IHelpdeskProvider sut = provider.GetService<IHelpdeskProvider>();
                    Assert.IsNotNull(sut);
                    HelpdeskProvider.ClearCache();

                    bool submitted = sut.SubmitTicket(0, 1, 1, "Jane Doe", "me@here.com", null, "message", out HelpdeskTicket ticket);
                    Assert.IsFalse(submitted);
                    Assert.IsNull(ticket);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void SubmitTicket_InvalidMessage_Null_ReturnsFalse()
        {
            string directory = TestHelper.GetTestPath();
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    MockApplicationBuilder mockApplicationBuilder = new MockApplicationBuilder(provider);
                    initialisation.AfterConfigure(mockApplicationBuilder);

                    mockPluginClassesService.Items.Add(new UserDataRowDefaults(provider.GetService<ISettingsProvider>()));

                    IHelpdeskProvider sut = provider.GetService<IHelpdeskProvider>();
                    Assert.IsNotNull(sut);
                    HelpdeskProvider.ClearCache();

                    bool submitted = sut.SubmitTicket(0, 1, 1, "Jane Doe", "me@here.com", "a query", null, out HelpdeskTicket ticket);
                    Assert.IsFalse(submitted);
                    Assert.IsNull(ticket);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void SubmitTicket_ValidTicketDetails_ReturnsTrue()
        {
            string directory = TestHelper.GetTestPath();
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    ISimpleDBOperations<TicketDataRow> tickeData = provider.GetRequiredService<ISimpleDBOperations<TicketDataRow>>();
                    ISimpleDBOperations<TicketMessageDataRow> ticketMessageData = provider.GetRequiredService<ISimpleDBOperations<TicketMessageDataRow>>();

                    MockApplicationBuilder mockApplicationBuilder = new MockApplicationBuilder(provider);
                    initialisation.AfterConfigure(mockApplicationBuilder);

                    mockPluginClassesService.Items.Add(new UserDataRowDefaults(provider.GetService<ISettingsProvider>()));

                    IHelpdeskProvider sut = provider.GetService<IHelpdeskProvider>();
                    Assert.IsNotNull(sut);
                    HelpdeskProvider.ClearCache();

                    Assert.AreEqual(0, tickeData.RecordCount);
                    Assert.AreEqual(0, ticketMessageData.RecordCount);

                    bool submitted = sut.SubmitTicket(0, 1, 1, "Jane Doe", "me@here.com", "a query", "a message", out HelpdeskTicket ticket);

                    Assert.IsTrue(submitted);
                    Assert.IsNotNull(ticket);
                    Assert.AreEqual("Jane Doe", ticket.LastReplier);
                    Assert.AreEqual("a query", ticket.Subject);
                    Assert.AreEqual("me@here.com", ticket.CreatedByEmail);
                    Assert.AreEqual(1, ticket.Messages.Count);
                    Assert.AreEqual("a message", ticket.Messages[0].Message);
                    Assert.AreEqual("Jane Doe", ticket.Messages[0].UserName);
                    Assert.AreEqual(1, tickeData.RecordCount);
                    Assert.AreEqual(1, ticketMessageData.RecordCount);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void GetTicket_InvalidTicketId_ReturnsNull()
        {
            string directory = TestHelper.GetTestPath();
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    MockApplicationBuilder mockApplicationBuilder = new MockApplicationBuilder(provider);
                    initialisation.AfterConfigure(mockApplicationBuilder);

                    mockPluginClassesService.Items.Add(new UserDataRowDefaults(provider.GetService<ISettingsProvider>()));

                    IHelpdeskProvider sut = provider.GetService<IHelpdeskProvider>();
                    Assert.IsNotNull(sut);
                    HelpdeskProvider.ClearCache();

                    HelpdeskTicket result = sut.GetTicket(-10);
                    Assert.IsNull(result);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void GetTicket_UseIdValidTicketDetails_ReturnsHelpdeskTicket()
        {
            string directory = TestHelper.GetTestPath();
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    ISimpleDBOperations<TicketDataRow> tickeData = provider.GetRequiredService<ISimpleDBOperations<TicketDataRow>>();
                    ISimpleDBOperations<TicketMessageDataRow> ticketMessageData = provider.GetRequiredService<ISimpleDBOperations<TicketMessageDataRow>>();

                    MockApplicationBuilder mockApplicationBuilder = new MockApplicationBuilder(provider);
                    initialisation.AfterConfigure(mockApplicationBuilder);

                    mockPluginClassesService.Items.Add(new UserDataRowDefaults(provider.GetService<ISettingsProvider>()));

                    IHelpdeskProvider sut = provider.GetService<IHelpdeskProvider>();
                    Assert.IsNotNull(sut);
                    HelpdeskProvider.ClearCache();

                    Assert.AreEqual(0, tickeData.RecordCount);
                    Assert.AreEqual(0, ticketMessageData.RecordCount);

                    bool submitted = sut.SubmitTicket(0, 1, 1, "Jane Doe", "me@here.com", "a query", "a message", out HelpdeskTicket ticket);

                    Assert.IsTrue(submitted);

                    HelpdeskTicket result = sut.GetTicket(ticket.Id);
                    Assert.IsNotNull(result);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void GetTicket_UseEmailAndKeyInvalidTicketDetails_ReturnsNull()
        {
            string directory = TestHelper.GetTestPath();
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    ISimpleDBOperations<TicketDataRow> tickeData = provider.GetRequiredService<ISimpleDBOperations<TicketDataRow>>();
                    ISimpleDBOperations<TicketMessageDataRow> ticketMessageData = provider.GetRequiredService<ISimpleDBOperations<TicketMessageDataRow>>();

                    MockApplicationBuilder mockApplicationBuilder = new MockApplicationBuilder(provider);
                    initialisation.AfterConfigure(mockApplicationBuilder);

                    mockPluginClassesService.Items.Add(new UserDataRowDefaults(provider.GetService<ISettingsProvider>()));

                    IHelpdeskProvider sut = provider.GetService<IHelpdeskProvider>();
                    Assert.IsNotNull(sut);
                    HelpdeskProvider.ClearCache();

                    Assert.AreEqual(0, tickeData.RecordCount);
                    Assert.AreEqual(0, ticketMessageData.RecordCount);

                    HelpdeskTicket result = sut.GetTicket("me@here.com", "123");
                    Assert.IsNull(result);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void GetTicket_UseEmailAndKeyValidTicketDetails_ReturnsHelpdeskTicket()
        {
            string directory = TestHelper.GetTestPath();
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    ISimpleDBOperations<TicketDataRow> tickeData = provider.GetRequiredService<ISimpleDBOperations<TicketDataRow>>();
                    ISimpleDBOperations<TicketMessageDataRow> ticketMessageData = provider.GetRequiredService<ISimpleDBOperations<TicketMessageDataRow>>();

                    MockApplicationBuilder mockApplicationBuilder = new MockApplicationBuilder(provider);
                    initialisation.AfterConfigure(mockApplicationBuilder);

                    mockPluginClassesService.Items.Add(new UserDataRowDefaults(provider.GetService<ISettingsProvider>()));

                    IHelpdeskProvider sut = provider.GetService<IHelpdeskProvider>();
                    Assert.IsNotNull(sut);
                    HelpdeskProvider.ClearCache();

                    Assert.AreEqual(0, tickeData.RecordCount);
                    Assert.AreEqual(0, ticketMessageData.RecordCount);

                    bool submitted = sut.SubmitTicket(0, 1, 1, "Jane Doe", "me@here.com", "a query", "a message", out HelpdeskTicket ticket);

                    Assert.IsTrue(submitted);

                    HelpdeskTicket result = sut.GetTicket(ticket.CreatedByEmail, ticket.Key);
                    Assert.IsNotNull(result);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void TicketRespond_ResponseAddedToExistingTicket_ReturnsTrue()
        {
            string directory = TestHelper.GetTestPath();
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    ISimpleDBOperations<TicketDataRow> ticketData = provider.GetRequiredService<ISimpleDBOperations<TicketDataRow>>();
                    ISimpleDBOperations<TicketMessageDataRow> ticketMessageData = provider.GetRequiredService<ISimpleDBOperations<TicketMessageDataRow>>();
                    Assert.AreEqual(0, ticketData.RecordCount);
                    Assert.AreEqual(0, ticketMessageData.RecordCount);

                    MockApplicationBuilder mockApplicationBuilder = new MockApplicationBuilder(provider);
                    initialisation.AfterConfigure(mockApplicationBuilder);

                    mockPluginClassesService.Items.Add(new UserDataRowDefaults(provider.GetService<ISettingsProvider>()));

                    IHelpdeskProvider sut = provider.GetService<IHelpdeskProvider>();
                    Assert.IsNotNull(sut);
                    HelpdeskProvider.ClearCache();

                    Assert.AreEqual(0, ticketData.RecordCount);
                    Assert.AreEqual(0, ticketMessageData.RecordCount);

                    bool submitted = sut.SubmitTicket(0, 1, 1, "Jane Doe", "me@here.com", "a query", "a message", out HelpdeskTicket ticket);

                    Assert.IsTrue(submitted);
                    Assert.AreEqual(1, ticketData.RecordCount);
                    Assert.AreEqual(1, ticketMessageData.RecordCount);
                    Assert.IsNotNull(ticket);
                    Assert.AreEqual("Jane Doe", ticket.LastReplier);

                    bool result = sut.TicketRespond(ticket, "John Doe", "a response");

                    Assert.AreEqual(2, ticket.Messages.Count);
                    Assert.AreEqual("John Doe", ticket.LastReplier);
                    Assert.AreEqual("John Doe", ticket.Messages[1].UserName);
                    Assert.AreEqual("a response", ticket.Messages[1].Message);
                    Assert.AreEqual(1, ticketData.RecordCount);
                    Assert.AreEqual(2, ticketMessageData.RecordCount);

                    TicketDataRow ticketDataRow = ticketData.Select(ticket.Id);
                    Assert.IsNotNull(ticketDataRow);
                    Assert.AreEqual(2, ticketDataRow.Status);
                    Assert.AreEqual("John Doe", ticketDataRow.LastReplier);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void GetKnowledgebaseGroups_NoGroupsAvailable_ReturnsEmptyList()
        {
            string directory = TestHelper.GetTestPath();
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    ISimpleDBOperations<FAQDataRow> faqData = provider.GetRequiredService<ISimpleDBOperations<FAQDataRow>>();
                    Assert.AreEqual(0, faqData.RecordCount);

                    MockApplicationBuilder mockApplicationBuilder = new MockApplicationBuilder(provider);
                    initialisation.AfterConfigure(mockApplicationBuilder);

                    mockPluginClassesService.Items.Add(new UserDataRowDefaults(provider.GetService<ISettingsProvider>()));

                    IHelpdeskProvider sut = provider.GetService<IHelpdeskProvider>();
                    Assert.IsNotNull(sut);
                    HelpdeskProvider.ClearCache();

                    Assert.AreEqual(0, faqData.RecordCount);

                    List<KnowledgeBaseGroup> groups = sut.GetKnowledgebaseGroups(0, null);

                    Assert.IsNotNull(groups);
                    Assert.AreEqual(0, groups.Count);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void GetKnowledgebaseGroups_GroupsAvailable_ReturnsAllGroupsWithAndWithoutParent()
        {
            string directory = TestHelper.GetTestPath();
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    MockApplicationBuilder mockApplicationBuilder = new MockApplicationBuilder(provider);
                    initialisation.AfterConfigure(mockApplicationBuilder);

                    ISimpleDBOperations<FAQDataRow> faqData = provider.GetRequiredService<ISimpleDBOperations<FAQDataRow>>();
                    Assert.AreEqual(0, faqData.RecordCount);

                    ISimpleDBOperations<FAQItemDataRow> faqItemData = provider.GetRequiredService<ISimpleDBOperations<FAQItemDataRow>>();
                    Assert.AreEqual(0, faqItemData.RecordCount);

                    faqData.Insert(new List<FAQDataRow>()
                    {
                        new FAQDataRow() { Name = "group 1", Description = "Group 1 description" },
                        new FAQDataRow() { Name = "group 2", Description = "Group 2 description" },
                        new FAQDataRow() { Name = "group 2a", Description = "Group 2a description", Parent = 2 }
                    });

                    int itemNumber = 0;
                    faqItemData.Insert(new List<FAQItemDataRow>()
                    {
                        new FAQItemDataRow() { ParentId = 1, Description = $"Item {++itemNumber}", Content = $"Content {itemNumber}" },
                        new FAQItemDataRow() { ParentId = 1, Description = $"Item {++itemNumber}", Content = $"Content {itemNumber}" },
                        new FAQItemDataRow() { ParentId = 1, Description = $"Item {++itemNumber}", Content = $"Content {itemNumber}" },
                        new FAQItemDataRow() { ParentId = 2, Description = $"Item {++itemNumber}", Content = $"Content {itemNumber}" },
                        new FAQItemDataRow() { ParentId = 2, Description = $"Item {++itemNumber}", Content = $"Content {itemNumber}" },
                        new FAQItemDataRow() { ParentId = 2, Description = $"Item {++itemNumber}", Content = $"Content {itemNumber}" },
                        new FAQItemDataRow() { ParentId = 2, Description = $"Item {++itemNumber}", Content = $"Content {itemNumber}" },
                        new FAQItemDataRow() { ParentId = 2, Description = $"Item {++itemNumber}", Content = $"Content {itemNumber}" },
                        new FAQItemDataRow() { ParentId = 3, Description = $"Item {++itemNumber}", Content = $"Content {itemNumber}" },
                        new FAQItemDataRow() { ParentId = 3, Description = $"Item {++itemNumber}", Content = $"Content {itemNumber}" },
                    });

                    mockPluginClassesService.Items.Add(new UserDataRowDefaults(provider.GetService<ISettingsProvider>()));

                    IHelpdeskProvider sut = provider.GetService<IHelpdeskProvider>();
                    Assert.IsNotNull(sut);
                    HelpdeskProvider.ClearCache();

                    Assert.AreEqual(3, faqData.RecordCount);

                    List<KnowledgeBaseGroup> groups = sut.GetKnowledgebaseGroups(2, null);

                    Assert.IsNotNull(groups);
                    Assert.AreEqual(2, groups.Count);
                    Assert.AreEqual(3, groups[0].Items.Count);
                    Assert.AreEqual(5, groups[1].Items.Count);

                    List<KnowledgeBaseGroup> subGroups = sut.GetKnowledgebaseGroups(1, groups[1]);

                    Assert.IsNotNull(subGroups);
                    Assert.AreEqual(1, subGroups.Count);
                    Assert.AreEqual(2, subGroups[0].Items.Count);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void GetKnowledgebaseGroup_GroupsAvailable_ReturnsCorrectGroupWithNullParent()
        {
            string directory = TestHelper.GetTestPath();
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    MockApplicationBuilder mockApplicationBuilder = new MockApplicationBuilder(provider);
                    initialisation.AfterConfigure(mockApplicationBuilder);

                    ISimpleDBOperations<FAQDataRow> faqData = provider.GetRequiredService<ISimpleDBOperations<FAQDataRow>>();
                    Assert.AreEqual(0, faqData.RecordCount);

                    ISimpleDBOperations<FAQItemDataRow> faqItemData = provider.GetRequiredService<ISimpleDBOperations<FAQItemDataRow>>();
                    Assert.AreEqual(0, faqItemData.RecordCount);

                    faqData.Insert(new List<FAQDataRow>()
                    {
                        new FAQDataRow() { Name = "group 1", Description = "Group 1 description" },
                        new FAQDataRow() { Name = "group 2", Description = "Group 2 description" },
                        new FAQDataRow() { Name = "group 2a", Description = "Group 2a description", Parent = 2 }
                    });

                    int itemNumber = 0;
                    faqItemData.Insert(new List<FAQItemDataRow>()
                    {
                        new FAQItemDataRow() { ParentId = 1, Description = $"Item {++itemNumber}", Content = $"Content {itemNumber}" },
                        new FAQItemDataRow() { ParentId = 1, Description = $"Item {++itemNumber}", Content = $"Content {itemNumber}" },
                        new FAQItemDataRow() { ParentId = 1, Description = $"Item {++itemNumber}", Content = $"Content {itemNumber}" },
                        new FAQItemDataRow() { ParentId = 2, Description = $"Item {++itemNumber}", Content = $"Content {itemNumber}" },
                        new FAQItemDataRow() { ParentId = 2, Description = $"Item {++itemNumber}", Content = $"Content {itemNumber}" },
                        new FAQItemDataRow() { ParentId = 2, Description = $"Item {++itemNumber}", Content = $"Content {itemNumber}" },
                        new FAQItemDataRow() { ParentId = 2, Description = $"Item {++itemNumber}", Content = $"Content {itemNumber}" },
                        new FAQItemDataRow() { ParentId = 2, Description = $"Item {++itemNumber}", Content = $"Content {itemNumber}" },
                        new FAQItemDataRow() { ParentId = 3, Description = $"Item {++itemNumber}", Content = $"Content {itemNumber}" },
                        new FAQItemDataRow() { ParentId = 3, Description = $"Item {++itemNumber}", Content = $"Content {itemNumber}" },
                    });

                    mockPluginClassesService.Items.Add(new UserDataRowDefaults(provider.GetService<ISettingsProvider>()));

                    IHelpdeskProvider sut = provider.GetService<IHelpdeskProvider>();
                    Assert.IsNotNull(sut);
                    HelpdeskProvider.ClearCache();

                    Assert.AreEqual(3, faqData.RecordCount);

                    KnowledgeBaseGroup group = sut.GetKnowledgebaseGroup(0, 2);

                    Assert.IsNotNull(group);
                    Assert.AreEqual(5, group.Items.Count);
                    Assert.IsNull(group.Parent);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void GetKnowledgebaseGroup_GroupsAvailable_ReturnsCorrectGroupWithParent()
        {
            string directory = TestHelper.GetTestPath();
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    MockApplicationBuilder mockApplicationBuilder = new MockApplicationBuilder(provider);
                    initialisation.AfterConfigure(mockApplicationBuilder);

                    ISimpleDBOperations<FAQDataRow> faqData = provider.GetRequiredService<ISimpleDBOperations<FAQDataRow>>();
                    Assert.AreEqual(0, faqData.RecordCount);

                    ISimpleDBOperations<FAQItemDataRow> faqItemData = provider.GetRequiredService<ISimpleDBOperations<FAQItemDataRow>>();
                    Assert.AreEqual(0, faqItemData.RecordCount);

                    faqData.Insert(new List<FAQDataRow>()
                    {
                        new FAQDataRow() { Name = "group 1", Description = "Group 1 description" },
                        new FAQDataRow() { Name = "group 2", Description = "Group 2 description" },
                        new FAQDataRow() { Name = "group 2a", Description = "Group 2a description", Parent = 2 }
                    });

                    int itemNumber = 0;
                    faqItemData.Insert(new List<FAQItemDataRow>()
                    {
                        new FAQItemDataRow() { ParentId = 1, Description = $"Item {++itemNumber}", Content = $"Content {itemNumber}" },
                        new FAQItemDataRow() { ParentId = 1, Description = $"Item {++itemNumber}", Content = $"Content {itemNumber}" },
                        new FAQItemDataRow() { ParentId = 1, Description = $"Item {++itemNumber}", Content = $"Content {itemNumber}" },
                        new FAQItemDataRow() { ParentId = 2, Description = $"Item {++itemNumber}", Content = $"Content {itemNumber}" },
                        new FAQItemDataRow() { ParentId = 2, Description = $"Item {++itemNumber}", Content = $"Content {itemNumber}" },
                        new FAQItemDataRow() { ParentId = 2, Description = $"Item {++itemNumber}", Content = $"Content {itemNumber}" },
                        new FAQItemDataRow() { ParentId = 2, Description = $"Item {++itemNumber}", Content = $"Content {itemNumber}" },
                        new FAQItemDataRow() { ParentId = 2, Description = $"Item {++itemNumber}", Content = $"Content {itemNumber}" },
                        new FAQItemDataRow() { ParentId = 3, Description = $"Item {++itemNumber}", Content = $"Content {itemNumber}" },
                        new FAQItemDataRow() { ParentId = 3, Description = $"Item {++itemNumber}", Content = $"Content {itemNumber}" },
                    });

                    mockPluginClassesService.Items.Add(new UserDataRowDefaults(provider.GetService<ISettingsProvider>()));

                    IHelpdeskProvider sut = provider.GetService<IHelpdeskProvider>();
                    Assert.IsNotNull(sut);
                    HelpdeskProvider.ClearCache();

                    Assert.AreEqual(3, faqData.RecordCount);

                    KnowledgeBaseGroup group = sut.GetKnowledgebaseGroup(0, 3);

                    Assert.IsNotNull(group);
                    Assert.AreEqual(2, group.Items.Count);
                    Assert.IsNotNull(group.Parent);
                    Assert.IsNull(group.Parent.Parent);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void KnowledgebaseView_ViewCountIncremented()
        {
            string directory = TestHelper.GetTestPath();
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    MockApplicationBuilder mockApplicationBuilder = new MockApplicationBuilder(provider);
                    initialisation.AfterConfigure(mockApplicationBuilder);

                    ISimpleDBOperations<FAQDataRow> faqData = provider.GetRequiredService<ISimpleDBOperations<FAQDataRow>>();
                    Assert.AreEqual(0, faqData.RecordCount);

                    ISimpleDBOperations<FAQItemDataRow> faqItemData = provider.GetRequiredService<ISimpleDBOperations<FAQItemDataRow>>();
                    Assert.AreEqual(0, faqItemData.RecordCount);

                    faqData.Insert(new List<FAQDataRow>()
                    {
                        new FAQDataRow() { Name = "group 1", Description = "Group 1 description" },
                    });

                    int itemNumber = 0;
                    faqItemData.Insert(new List<FAQItemDataRow>()
                    {
                        new FAQItemDataRow() { ParentId = 1, Description = $"Item {++itemNumber}", Content = $"Content {itemNumber}" },
                        new FAQItemDataRow() { ParentId = 1, Description = $"Item {++itemNumber}", Content = $"Content {itemNumber}" },
                        new FAQItemDataRow() { ParentId = 1, Description = $"Item {++itemNumber}", Content = $"Content {itemNumber}" },
                    });

                    mockPluginClassesService.Items.Add(new UserDataRowDefaults(provider.GetService<ISettingsProvider>()));

                    IHelpdeskProvider sut = provider.GetService<IHelpdeskProvider>();
                    Assert.IsNotNull(sut);
                    HelpdeskProvider.ClearCache();

                    Assert.AreEqual(1, faqData.RecordCount);

                    KnowledgeBaseGroup group = sut.GetKnowledgebaseGroup(0, 1);

                    Assert.IsNotNull(group);
                    Assert.AreEqual(3, group.Items.Count);
                    Assert.IsNull(group.Parent);

                    for (int i = 0; i < 100; i++)
                    {
                        sut.KnowledgebaseView(group.Items[0]);
                        Assert.AreEqual(i + 1, group.Items[0].ViewCount);

                        FAQItemDataRow faqItemDataRow = faqItemData.Select(group.Items[0].Id);
                        Assert.AreEqual(i + 1, faqItemDataRow.ViewCount);
                    }
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void GetKnowledgebaseItem_ItemNotFound_ReturnsFalse()
        {
            string directory = TestHelper.GetTestPath();
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    MockApplicationBuilder mockApplicationBuilder = new MockApplicationBuilder(provider);
                    initialisation.AfterConfigure(mockApplicationBuilder);

                    ISimpleDBOperations<FAQDataRow> faqData = provider.GetRequiredService<ISimpleDBOperations<FAQDataRow>>();
                    Assert.AreEqual(0, faqData.RecordCount);

                    ISimpleDBOperations<FAQItemDataRow> faqItemData = provider.GetRequiredService<ISimpleDBOperations<FAQItemDataRow>>();
                    Assert.AreEqual(0, faqItemData.RecordCount);

                    mockPluginClassesService.Items.Add(new UserDataRowDefaults(provider.GetService<ISettingsProvider>()));

                    IHelpdeskProvider sut = provider.GetService<IHelpdeskProvider>();
                    Assert.IsNotNull(sut);
                    HelpdeskProvider.ClearCache();

                    bool result = sut.GetKnowledgebaseItem(0, 1, out KnowledgeBaseItem item, out KnowledgeBaseGroup group);

                    Assert.IsFalse(result);
                    Assert.IsNull(group);
                    Assert.IsNull(item);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void GetKnowledgebaseItem_ItemFound_ReturnsTrue()
        {
            string directory = TestHelper.GetTestPath();
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    MockApplicationBuilder mockApplicationBuilder = new MockApplicationBuilder(provider);
                    initialisation.AfterConfigure(mockApplicationBuilder);

                    ISimpleDBOperations<FAQDataRow> faqData = provider.GetRequiredService<ISimpleDBOperations<FAQDataRow>>();
                    Assert.AreEqual(0, faqData.RecordCount);

                    ISimpleDBOperations<FAQItemDataRow> faqItemData = provider.GetRequiredService<ISimpleDBOperations<FAQItemDataRow>>();
                    Assert.AreEqual(0, faqItemData.RecordCount);

                    faqData.Insert(new List<FAQDataRow>()
                    {
                        new FAQDataRow() { Name = "group 1", Description = "Group 1 description" },
                    });

                    int itemNumber = 0;
                    faqItemData.Insert(new List<FAQItemDataRow>()
                    {
                        new FAQItemDataRow() { ParentId = 1, Description = $"Item {++itemNumber}", Content = $"Content {itemNumber}" },
                        new FAQItemDataRow() { ParentId = 1, Description = $"Item {++itemNumber}", Content = $"Content {itemNumber}" },
                        new FAQItemDataRow() { ParentId = 1, Description = $"Item {++itemNumber}", Content = $"Content {itemNumber}" },
                    });

                    mockPluginClassesService.Items.Add(new UserDataRowDefaults(provider.GetService<ISettingsProvider>()));

                    IHelpdeskProvider sut = provider.GetService<IHelpdeskProvider>();
                    Assert.IsNotNull(sut);
                    HelpdeskProvider.ClearCache();

                    Assert.AreEqual(1, faqData.RecordCount);

                    bool result = sut.GetKnowledgebaseItem(0, 1, out KnowledgeBaseItem item, out KnowledgeBaseGroup group);

                    Assert.IsTrue(result);
                    Assert.IsNotNull(group);
                    Assert.IsNotNull(item);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }
    }
}
