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
 *  File: MockImageProvider.cs
 *
 *  Purpose:  Mock class for testing ImageProvider
 *
 *  Date        Name                Reason
 *  16/04/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Middleware.Images;
using Middleware.Interfaces;

namespace AspNetCore.PluginManager.Tests.Plugins.ImageManagerTests.Mocks
{
    [ExcludeFromCodeCoverage]
    internal class MockImageProvider : IImageProvider
    {
        private readonly List<string> _groups;
        private readonly List<ImageFile> _imageFiles;

        public MockImageProvider()
        {
            _groups = new List<string>();
            _imageFiles = new List<ImageFile>();
        }

        public MockImageProvider(List<string> groups, List<ImageFile> images)
        {
            _groups = groups ?? throw new ArgumentNullException(nameof(groups));
            _imageFiles = images ?? throw new ArgumentNullException(nameof(groups));
        }

        public bool CreateGroup(string groupName)
        {
            if (_groups.Contains(groupName))
                return false;

            _groups.Add(groupName);
            return true;
        }

        public bool DeleteGroup(string groupName)
        {
            if (!_groups.Contains(groupName))
                return false;

            _groups.Remove(groupName);
            return true;
        }

        public List<string> Groups()
        {
            return _groups;
        }

        public List<ImageFile> Images(string groupName)
        {
            return _imageFiles;
        }
    }
}
