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
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

using AspNetCore.PluginManager.Tests.Shared;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Middleware;
using Middleware.Accounts;
using Middleware.Accounts.Invoices;
using Middleware.Accounts.Orders;
using Middleware.Helpdesk;

using PluginManager.Abstractions;
using PluginManager.DAL.TextFiles.Internal;
using PluginManager.DAL.TextFiles.Providers;
using PluginManager.DAL.TextFiles.Tables;
using PluginManager.Tests.Mocks;

using SharedPluginFeatures;

namespace PluginManager.DAL.TextFiles.Tests.Providers
{
    [TestClass]
    public class HelpdeskProviderTests : BaseProviderTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_UsersNull_Throws_ArgumentNullException()
        {
            new HelpdeskProvider(null, null, null, null, null, null, null, null);
        }

        [TestMethod]
        public void GetTicketDepartments_ReturnsListOfDefaultDepartments()
        {
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = new ServiceCollection();
                List<object> classServices = new List<object>()
                {
                    new TicketDepartmentsDataRowDefaults(),
                    new TicketStatusDataRowDefaults(),
                    new TicketPrioritiesDataRowDefaults(),
                };

                MockPluginClassesService mockPluginClassesService = new MockPluginClassesService(classServices);

                services.AddSingleton<ISettingsProvider>(new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\"))));
                services.AddSingleton<IPluginClassesService>(mockPluginClassesService);
                
                initialisation.BeforeConfigureServices(services);

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
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = new ServiceCollection();
                List<object> classServices = new List<object>()
                {
                    new TicketDepartmentsDataRowDefaults(),
                    new TicketStatusDataRowDefaults(),
                    new TicketPrioritiesDataRowDefaults(),
                };

                MockPluginClassesService mockPluginClassesService = new MockPluginClassesService(classServices);

                services.AddSingleton<ISettingsProvider>(new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\"))));
                services.AddSingleton<IPluginClassesService>(mockPluginClassesService);

                initialisation.BeforeConfigureServices(services);

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
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = new ServiceCollection();
                List<object> classServices = new List<object>()
                {
                    new TicketDepartmentsDataRowDefaults(),
                    new TicketStatusDataRowDefaults(),
                    new TicketPrioritiesDataRowDefaults(),
                };

                MockPluginClassesService mockPluginClassesService = new MockPluginClassesService(classServices);

                services.AddSingleton<ISettingsProvider>(new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\"))));
                services.AddSingleton<IPluginClassesService>(mockPluginClassesService);

                initialisation.BeforeConfigureServices(services);

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
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = new ServiceCollection();
                List<object> classServices = new List<object>()
                {
                    new TicketDepartmentsDataRowDefaults(),
                    new TicketStatusDataRowDefaults(),
                    new TicketPrioritiesDataRowDefaults(),
                };

                MockPluginClassesService mockPluginClassesService = new MockPluginClassesService(classServices);

                services.AddSingleton<ISettingsProvider>(new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\"))));
                services.AddSingleton<IPluginClassesService>(mockPluginClassesService);

                initialisation.BeforeConfigureServices(services);

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
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = new ServiceCollection();
                List<object> classServices = new List<object>()
                {
                    new TicketDepartmentsDataRowDefaults(),
                    new TicketStatusDataRowDefaults(),
                    new TicketPrioritiesDataRowDefaults(),
                };

                MockPluginClassesService mockPluginClassesService = new MockPluginClassesService(classServices);

                services.AddSingleton<ISettingsProvider>(new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\"))));
                services.AddSingleton<IPluginClassesService>(mockPluginClassesService);

                initialisation.BeforeConfigureServices(services);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    MockApplicationBuilder mockApplicationBuilder = new MockApplicationBuilder(provider);
                    initialisation.AfterConfigure(mockApplicationBuilder);

                    mockPluginClassesService.Items.Add(new UserDataRowDefaults(provider.GetService<ISettingsProvider>()));

                    ITextTableOperations<FeedbackDataRow> feedbackData = provider.GetRequiredService<ITextTableOperations<FeedbackDataRow>>();
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
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = new ServiceCollection();
                List<object> classServices = new List<object>()
                {
                    new TicketDepartmentsDataRowDefaults(),
                    new TicketStatusDataRowDefaults(),
                    new TicketPrioritiesDataRowDefaults(),
                };

                MockPluginClassesService mockPluginClassesService = new MockPluginClassesService(classServices);

                services.AddSingleton<ISettingsProvider>(new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\"))));
                services.AddSingleton<IPluginClassesService>(mockPluginClassesService);

                initialisation.BeforeConfigureServices(services);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    MockApplicationBuilder mockApplicationBuilder = new MockApplicationBuilder(provider);
                    initialisation.AfterConfigure(mockApplicationBuilder);

                    mockPluginClassesService.Items.Add(new UserDataRowDefaults(provider.GetService<ISettingsProvider>()));

                    ITextTableOperations<FeedbackDataRow> feedbackData = provider.GetRequiredService<ITextTableOperations<FeedbackDataRow>>();
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
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = new ServiceCollection();
                List<object> classServices = new List<object>()
                {
                    new TicketDepartmentsDataRowDefaults(),
                    new TicketStatusDataRowDefaults(),
                    new TicketPrioritiesDataRowDefaults(),
                };

                MockPluginClassesService mockPluginClassesService = new MockPluginClassesService(classServices);

                services.AddSingleton<ISettingsProvider>(new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\"))));
                services.AddSingleton<IPluginClassesService>(mockPluginClassesService);

                initialisation.BeforeConfigureServices(services);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    MockApplicationBuilder mockApplicationBuilder = new MockApplicationBuilder(provider);
                    initialisation.AfterConfigure(mockApplicationBuilder);

                    mockPluginClassesService.Items.Add(new UserDataRowDefaults(provider.GetService<ISettingsProvider>()));

                    ITextTableOperations<FeedbackDataRow> feedbackData = provider.GetRequiredService<ITextTableOperations<FeedbackDataRow>>();
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
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = new ServiceCollection();
                List<object> classServices = new List<object>()
                {
                    new TicketDepartmentsDataRowDefaults(),
                    new TicketStatusDataRowDefaults(),
                    new TicketPrioritiesDataRowDefaults(),
                };

                MockPluginClassesService mockPluginClassesService = new MockPluginClassesService(classServices);

                services.AddSingleton<ISettingsProvider>(new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\"))));
                services.AddSingleton<IPluginClassesService>(mockPluginClassesService);

                initialisation.BeforeConfigureServices(services);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    MockApplicationBuilder mockApplicationBuilder = new MockApplicationBuilder(provider);
                    initialisation.AfterConfigure(mockApplicationBuilder);

                    mockPluginClassesService.Items.Add(new UserDataRowDefaults(provider.GetService<ISettingsProvider>()));

                    ITextTableOperations<FeedbackDataRow> feedbackData = provider.GetRequiredService<ITextTableOperations<FeedbackDataRow>>();
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
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = new ServiceCollection();
                List<object> classServices = new List<object>()
                {
                    new TicketDepartmentsDataRowDefaults(),
                    new TicketStatusDataRowDefaults(),
                    new TicketPrioritiesDataRowDefaults(),
                };

                MockPluginClassesService mockPluginClassesService = new MockPluginClassesService(classServices);

                services.AddSingleton<ISettingsProvider>(new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\"))));
                services.AddSingleton<IPluginClassesService>(mockPluginClassesService);

                initialisation.BeforeConfigureServices(services);

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
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = new ServiceCollection();
                List<object> classServices = new List<object>()
                {
                    new TicketDepartmentsDataRowDefaults(),
                    new TicketStatusDataRowDefaults(),
                    new TicketPrioritiesDataRowDefaults(),
                };

                MockPluginClassesService mockPluginClassesService = new MockPluginClassesService(classServices);

                services.AddSingleton<ISettingsProvider>(new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\"))));
                services.AddSingleton<IPluginClassesService>(mockPluginClassesService);

                initialisation.BeforeConfigureServices(services);

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
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = new ServiceCollection();
                List<object> classServices = new List<object>()
                {
                    new TicketDepartmentsDataRowDefaults(),
                    new TicketStatusDataRowDefaults(),
                    new TicketPrioritiesDataRowDefaults(),
                };

                MockPluginClassesService mockPluginClassesService = new MockPluginClassesService(classServices);

                services.AddSingleton<ISettingsProvider>(new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\"))));
                services.AddSingleton<IPluginClassesService>(mockPluginClassesService);

                initialisation.BeforeConfigureServices(services);

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
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = new ServiceCollection();
                List<object> classServices = new List<object>()
                {
                    new TicketDepartmentsDataRowDefaults(),
                    new TicketStatusDataRowDefaults(),
                    new TicketPrioritiesDataRowDefaults(),
                };

                MockPluginClassesService mockPluginClassesService = new MockPluginClassesService(classServices);

                services.AddSingleton<ISettingsProvider>(new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\"))));
                services.AddSingleton<IPluginClassesService>(mockPluginClassesService);

                initialisation.BeforeConfigureServices(services);

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
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = new ServiceCollection();
                List<object> classServices = new List<object>()
                {
                    new TicketDepartmentsDataRowDefaults(),
                    new TicketStatusDataRowDefaults(),
                    new TicketPrioritiesDataRowDefaults(),
                };

                MockPluginClassesService mockPluginClassesService = new MockPluginClassesService(classServices);

                services.AddSingleton<ISettingsProvider>(new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\"))));
                services.AddSingleton<IPluginClassesService>(mockPluginClassesService);

                initialisation.BeforeConfigureServices(services);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    ITextTableOperations<TicketDataRow> tickeData = provider.GetRequiredService<ITextTableOperations<TicketDataRow>>();
                    ITextTableOperations<TicketMessageDataRow> ticketMessageData = provider.GetRequiredService<ITextTableOperations<TicketMessageDataRow>>();

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
    }
}
