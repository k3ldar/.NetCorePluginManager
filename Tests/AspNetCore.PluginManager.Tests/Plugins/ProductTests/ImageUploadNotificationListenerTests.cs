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
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: ImageUploadNotificationListenerTests.cs
 *
 *  Purpose:  Tests for product plugin notifications
 *
 *  Date        Name                Reason
 *  30/05/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;

using AspNetCore.PluginManager.Tests.Shared;

using ImageManager.Plugin.Classes;
using ImageManager.Plugin.Models;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Middleware.Interfaces;

using Newtonsoft.Json;

using PluginManager.Abstractions;

using ProductPlugin;
using ProductPlugin.Classes;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Tests.Plugins.ProductTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ImageUploadNotificationListenerTests : GenericBaseClass
    {
        private const string TestCategoryName = "Product Manager Tests";

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_ValidInstanceSuccess()
        {
            ImageUploadNotificationListener sut = new ImageUploadNotificationListener(new MockImageProvider(), new MockSettingsProvider());
            Assert.IsNotNull(sut);
            Assert.IsInstanceOfType(sut, typeof(INotificationListener));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParamImageProvider_Null_Throws_ArgumentNullException()
        {
            ImageUploadNotificationListener sut = new ImageUploadNotificationListener(null, new MockSettingsProvider());
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParamSettingsProvider_Null_Throws_ArgumentNullException()
        {
            ImageUploadNotificationListener sut = new ImageUploadNotificationListener(new MockImageProvider(), null);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void GetEvents_Success()
        {
            ImageUploadNotificationListener sut = CreateListener();
            Assert.IsNotNull(sut);
            List<string> events = sut.GetEvents();

            Assert.AreEqual(2, events.Count);
            Assert.AreEqual("ImageUploadedEvent", events[0]);
            Assert.AreEqual("ImageUploadOptions", events[1]);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void EventRaised_DoesNotThrowException_Success()
        {
            ImageUploadNotificationListener sut = CreateListener();
            Assert.IsNotNull(sut);
            sut.EventRaised("", null, null);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void EventRaised_NullEventName_NotRecognised_ReturnsFalse()
        {
            ImageUploadNotificationListener sut = CreateListener();
            Assert.IsNotNull(sut);
            object response = null;
            bool result = sut.EventRaised(null, null, null, ref response);
            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void EventRaised_EmptyStringEventName_NotRecognised_ReturnsFalse()
        {
            ImageUploadNotificationListener sut = CreateListener();
            Assert.IsNotNull(sut);
            object response = null;
            bool result = sut.EventRaised("", null, null, ref response);
            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void EventRaised_ExceptionRaised_LoggedAndFails_ReturnsFalse()
        {
            string imagePath = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());

            try
            {
                ProductPluginSettings settings = new ProductPluginSettings()
                {
                    ResizeImages = true,
                    ResizeWidths = "178xred;-1x114;200x-1;89x64;288x268x34",
                    ResizeBackfillColor = "999z999#"
                };

                MockImageProvider mockImageProvider = MockImageProvider.CreateDefaultMockImageProviderWithSubgroupAndImage();
                mockImageProvider.ThrowExceptionWhenAddingFile = true;
                MockSettingsProvider testSettingsProvider = new MockSettingsProvider("{\"Products\":" + JsonConvert.SerializeObject(settings) + "}");

                ExtractImageResources(imagePath);
                ImageUploadNotificationListener sut = CreateListener(mockImageProvider, testSettingsProvider);
                Assert.IsNotNull(sut);

                CachedImageUpload cachedImageUpload = new CachedImageUpload("Products");
                cachedImageUpload.Files.Add(Path.Combine(imagePath, "life.jpg"));
                cachedImageUpload.Files.Add(Path.Combine(imagePath, "racism is stupid.jpg"));

                object response = null;
                bool result = sut.EventRaised("ImageUploadedEvent", cachedImageUpload, "C256", ref response);
                Assert.IsTrue(result);
                Assert.IsNotNull(response);
                Assert.IsInstanceOfType(response, typeof(List<string>));

                // validate new files exist
                Assert.IsFalse(ValidateImage(Path.Combine(imagePath, "Products", "C256", "C256_1_orig.jpg"), 480, 360));
                Assert.IsFalse(ValidateImage(Path.Combine(imagePath, "Products", "C256", "C256_1_89.png"), 89, 64));
                Assert.IsFalse(ValidateImage(Path.Combine(imagePath, "Products", "C256", "C256_2_orig.jpg"), 907, 960));
                Assert.IsFalse(ValidateImage(Path.Combine(imagePath, "Products", "C256", "C256_2_89.png"), 89, 64));
            }
            finally
            {
                if (Directory.Exists(imagePath))
                    Directory.Delete(imagePath, true);
            }
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void EventRaised_InvalidParam_Param1_Null_ReturnsFalse()
        {
            ImageUploadNotificationListener sut = CreateListener();
            Assert.IsNotNull(sut);
            object response = null;
            bool result = sut.EventRaised("asdfasdf", null, null, ref response);
            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void EventRaised_ImageUploadedEvent_InvalidOptions_Null_ReturnsFalse()
        {
            ImageUploadNotificationListener sut = CreateListener();
            Assert.IsNotNull(sut);
            object response = null;
            bool result = sut.EventRaised("ImageUploadedEvent", null, null, ref response);
            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void EventRaised_ImageUploadedEvent_InvalidOptions_InvalidIImageProcessOptions_ReturnsFalse()
        {
            ImageUploadNotificationListener sut = CreateListener();
            Assert.IsNotNull(sut);
            object response = null;
            bool result = sut.EventRaised("ImageUploadedEvent", new List<string>(), null, ref response);
            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void EventRaised_ImageUploadedOptionsEvent_InvalidOptions_Null_ReturnsFalse()
        {
            ImageUploadNotificationListener sut = CreateListener();
            Assert.IsNotNull(sut);
            object response = null;
            bool result = sut.EventRaised("ImageUploadOptions", null, null, ref response);
            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void EventRaised_ImageUploadedOptionsEvent_InvalidOptions_InvalidIImageProcessOptions_ReturnsFalse()
        {
            ImageUploadNotificationListener sut = CreateListener();
            Assert.IsNotNull(sut);
            object response = null;
            bool result = sut.EventRaised("ImageUploadOptions", new List<string>(), null, ref response);
            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void EventRaised_ImageUploadedOptionsEvent_InvalidOptions_NotProductFolder_ReturnsFalse()
        {
            ImageUploadNotificationListener sut = CreateListener();
            Assert.IsNotNull(sut);

            ImageProcessOptionsViewModel model = new ImageProcessOptionsViewModel()
            {
                GroupName = "Not products"
            };

            object response = null;
            bool result = sut.EventRaised("ImageUploadOptions", model, null, ref response);
            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void EventRaised_ImageUploadedOptionsEvent_ProductFolderMixedCase_ReturnsTrue()
        {
            ImageUploadNotificationListener sut = CreateListener();
            Assert.IsNotNull(sut);

            ImageProcessOptionsViewModel model = new ImageProcessOptionsViewModel()
            {
                GroupName = "pRodUCts"
            };

            object response = null;
            bool result = sut.EventRaised("ImageUploadOptions", model, null, ref response);
            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void EventRaised_InvalidParamAdditionalData_Null_ReturnsFalse()
        {
            ImageUploadNotificationListener sut = CreateListener();
            Assert.IsNotNull(sut);

            CachedImageUpload cachedImageUpload = new CachedImageUpload("Products");
            object response = null;
            bool result = sut.EventRaised("ImageUploadedEvent", cachedImageUpload, null, ref response);
            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void EventRaised_InvalidParamAdditionalData_EmptyString_ReturnsFalse()
        {
            ImageUploadNotificationListener sut = CreateListener();
            Assert.IsNotNull(sut);

            CachedImageUpload cachedImageUpload = new CachedImageUpload("Products");
            object response = null;
            bool result = sut.EventRaised("ImageUploadedEvent", cachedImageUpload, "", ref response);
            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void EventRaised_InvalidParamGroupName_DoesNotEqualProducts_ReturnsFalse()
        {
            ImageUploadNotificationListener sut = CreateListener();
            Assert.IsNotNull(sut);

            CachedImageUpload cachedImageUpload = new CachedImageUpload("Non products");
            object response = null;
            bool result = sut.EventRaised("ImageUploadedEvent", cachedImageUpload, "some data", ref response);
            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void EventRaised_EventNameImageUploadedEvent_NoFiles_ReturnsTrue()
        {
            ImageUploadNotificationListener sut = CreateListener();
            Assert.IsNotNull(sut);

            CachedImageUpload cachedImageUpload = new CachedImageUpload("Products");
            object response = null;
            bool result = sut.EventRaised("ImageUploadedEvent", cachedImageUpload, "C256", ref response);
            Assert.IsTrue(result);
            Assert.IsNotNull(response);
            Assert.IsInstanceOfType(response, typeof(List<string>));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void EventRaised_EventNameImageUploadOptions_ReturnsTrue()
        {
            ImageUploadNotificationListener sut = CreateListener();
            Assert.IsNotNull(sut);

            ImageProcessOptionsViewModel model = new ImageProcessOptionsViewModel()
            {
                GroupName = "Products"
            };

            object response = null;
            bool result = sut.EventRaised("ImageUploadOptions", model, null, ref response);
            Assert.IsTrue(result);
            Assert.IsTrue(model.AdditionalDataMandatory);
            Assert.IsFalse(model.ShowSubgroup);
            Assert.AreEqual("Product SKU", model.AdditionalDataName);
            Assert.AreEqual(model, response);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void EventRaised_EventNameImageUploadedEvent_SingleFileMakes5CopiesInDifferentSizes_ReturnsTrue()
        {
            string imagePath = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());

            try
            {
                ProductPluginSettings settings = new ProductPluginSettings()
                {
                    ResizeImages = true,
                    ResizeWidths = "178x128;148x114;200x145;89x64;288x268"
                };

                IImageProvider mockImageProvider = CreateDefaultImageProvider(imagePath);
                MockSettingsProvider testSettingsProvider = new MockSettingsProvider("{\"Products\":" + JsonConvert.SerializeObject(settings) + "}");

                ExtractImageResources(imagePath);
                ImageUploadNotificationListener sut = CreateListener(mockImageProvider, testSettingsProvider);
                Assert.IsNotNull(sut);

                CachedImageUpload cachedImageUpload = new CachedImageUpload("Products");
                cachedImageUpload.Files.Add(Path.Combine(imagePath, "life.jpg"));

                object response = null;
                bool result = sut.EventRaised("ImageUploadedEvent", cachedImageUpload, "C256", ref response);
                Assert.IsTrue(result);
                Assert.IsNotNull(response);
                Assert.IsInstanceOfType(response, typeof(List<string>));

                // validate new files exist
                Assert.IsTrue(ValidateImage(Path.Combine(imagePath, "Products", "C256", "C256_1_orig.jpg"), 480, 360));
                Assert.IsTrue(ValidateImage(Path.Combine(imagePath, "Products", "C256", "C256_1_89.png"), 89, 64));
                Assert.IsTrue(ValidateImage(Path.Combine(imagePath, "Products", "C256", "C256_1_148.png"), 148, 114));
                Assert.IsTrue(ValidateImage(Path.Combine(imagePath, "Products", "C256", "C256_1_178.png"), 178, 128));
                Assert.IsTrue(ValidateImage(Path.Combine(imagePath, "Products", "C256", "C256_1_200.png"), 200, 145));
                Assert.IsTrue(ValidateImage(Path.Combine(imagePath, "Products", "C256", "C256_1_288.png"), 288, 268));
            }
            finally
            {
                if (Directory.Exists(imagePath))
                    Directory.Delete(imagePath, true);
            }
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void EventRaised_EventNameImageUploadedEvent_SingleFileMakes1CopiesInDifferentSize_ReturnsTrue()
        {
            string imagePath = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());

            try
            {
                ProductPluginSettings settings = new ProductPluginSettings()
                {
                    ResizeImages = true,
                    ResizeWidths = "178xred;-1x114;200x-1;89x64;288x268x34",
                    ResizeBackfillColor = "999z999#"
                };

                IImageProvider mockImageProvider = CreateDefaultImageProvider(imagePath);
                MockSettingsProvider testSettingsProvider = new MockSettingsProvider("{\"Products\":" + JsonConvert.SerializeObject(settings) + "}");

                ExtractImageResources(imagePath);
                ImageUploadNotificationListener sut = CreateListener(mockImageProvider, testSettingsProvider);
                Assert.IsNotNull(sut);

                CachedImageUpload cachedImageUpload = new CachedImageUpload("Products");
                cachedImageUpload.Files.Add(Path.Combine(imagePath, "life.jpg"));

                object response = null;
                bool result = sut.EventRaised("ImageUploadedEvent", cachedImageUpload, "C256", ref response);
                Assert.IsTrue(result);
                Assert.IsNotNull(response);
                Assert.IsInstanceOfType(response, typeof(List<string>));

                // validate new files exist
                Assert.IsTrue(ValidateImage(Path.Combine(imagePath, "Products", "C256", "C256_1_orig.jpg"), 480, 360));
                Assert.IsTrue(ValidateImage(Path.Combine(imagePath, "Products", "C256", "C256_1_89.png"), 89, 64));
            }
            finally
            {
                if (Directory.Exists(imagePath))
                    Directory.Delete(imagePath, true);
            }
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void EventRaised_EventNameImageUploadedEvent_TwoFilesMakes1CopyOfEachInDifferentSizes_ReturnsTrue()
        {
            string imagePath = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());

            try
            {
                ProductPluginSettings settings = new ProductPluginSettings()
                {
                    ResizeImages = true,
                    ResizeWidths = "178xred;-1x114;200x-1;89x64;288x268x34",
                    ResizeBackfillColor = "999z999#"
                };

                IImageProvider mockImageProvider = CreateDefaultImageProvider(imagePath);
                MockSettingsProvider testSettingsProvider = new MockSettingsProvider("{\"Products\":" + JsonConvert.SerializeObject(settings) + "}");

                ExtractImageResources(imagePath);
                ImageUploadNotificationListener sut = CreateListener(mockImageProvider, testSettingsProvider);
                Assert.IsNotNull(sut);

                CachedImageUpload cachedImageUpload = new CachedImageUpload("Products");
                cachedImageUpload.Files.Add(Path.Combine(imagePath, "life.jpg"));
                cachedImageUpload.Files.Add(Path.Combine(imagePath, "racism is stupid.jpg"));

                object response = null;
                bool result = sut.EventRaised("ImageUploadedEvent", cachedImageUpload, "C256", ref response);
                Assert.IsTrue(result);
                Assert.IsNotNull(response);
                Assert.IsInstanceOfType(response, typeof(List<string>));

                // validate new files exist
                Assert.IsTrue(ValidateImage(Path.Combine(imagePath, "Products", "C256", "C256_1_orig.jpg"), 480, 360));
                Assert.IsTrue(ValidateImage(Path.Combine(imagePath, "Products", "C256", "C256_1_89.png"), 89, 64));
                Assert.IsTrue(ValidateImage(Path.Combine(imagePath, "Products", "C256", "C256_2_orig.jpg"), 907, 960));
                Assert.IsTrue(ValidateImage(Path.Combine(imagePath, "Products", "C256", "C256_2_89.png"), 89, 64));
            }
            finally
            {
                if (Directory.Exists(imagePath))
                    Directory.Delete(imagePath, true);
            }
        }

        private bool ValidateImage(string fileName, int width, int height)
        {
            if (!File.Exists(fileName))
                return false;

            using (Image image = Image.FromFile(fileName))
            {
                if (image.Width != width)
                    return false;

                if (image.Height != height)
                    return false;
            }

            return true;
        }

        private ImageUploadNotificationListener CreateListener(IImageProvider mockImageProvider = null, MockSettingsProvider testSettingsProvider = null)
        {
            return new ImageUploadNotificationListener(
                mockImageProvider ?? new MockImageProvider(),
                testSettingsProvider ?? new MockSettingsProvider());
        }

        private DefaultImageProvider CreateDefaultImageProvider(string imagePath = "")
        {
            if (imagePath == null)
                imagePath = String.Empty;

            if (!String.IsNullOrEmpty(imagePath))
                imagePath = imagePath.Replace("\\", "\\\\");

            MockSettingsProvider testSettingsProvider = new MockSettingsProvider("{\"ImageManager\": {\"ImagePath\": \"" + imagePath + "\"}}");

            return new DefaultImageProvider(new MockHostEnvironment(), testSettingsProvider);
        }

    }
}
