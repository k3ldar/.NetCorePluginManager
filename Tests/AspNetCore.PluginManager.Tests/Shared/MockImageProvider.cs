/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
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
using System.IO;
using System.Linq;

using Middleware.Images;
using Middleware.Interfaces;

namespace AspNetCore.PluginManager.Tests.Shared
{
    [ExcludeFromCodeCoverage]
    internal class MockImageProvider : IImageProvider
    {
        private readonly Dictionary<string, List<string>> _groups;
        private readonly Dictionary<string, byte[]> _filesAdded;
        private readonly List<ImageFile> _imageFiles;
        private readonly List<string> _deletedImageNames;
        private readonly List<string> _temporaryFileNames;

        public MockImageProvider()
        {
            _groups = new Dictionary<string, List<string>>();
            _filesAdded = new Dictionary<string, byte[]>();
            _imageFiles = new List<ImageFile>();
            _deletedImageNames = new List<string>();
            _temporaryFileNames = new List<string>();
            CanDeleteImages = true;
        }

        public MockImageProvider(Dictionary<string, List<string>> groups, List<ImageFile> images)
        {
            _groups = groups ?? throw new ArgumentNullException(nameof(groups));
            _filesAdded = new Dictionary<string, byte[]>();
            _imageFiles = images ?? throw new ArgumentNullException(nameof(groups));
            _deletedImageNames = new List<string>();
            _temporaryFileNames = new List<string>();
            CanDeleteImages = true;
        }

        public bool AddSubgroup(string groupName, string subGroupName)
        {
            if (!_groups.ContainsKey(groupName))
                throw new InvalidOperationException("groupName not found");

            if (_groups[groupName].Contains(subGroupName))
                throw new InvalidOperationException("Sub group already exists");

            _groups[groupName].Add(subGroupName);
            return true;
        }

        public bool CreateGroup(string groupName)
        {
            if (_groups.ContainsKey(groupName))
                return false;

            _groups.Add(groupName, new List<string>());
            return true;
        }

        public bool DeleteGroup(string groupName)
        {
            if (!_groups.ContainsKey(groupName))
                return false;

            _groups.Remove(groupName);
            return true;
        }

        public bool DeleteSubgroup(string groupName, string subGroupName)
        {
            throw new NotImplementedException();
        }

        public bool GroupExists(string groupName)
        {
            return _groups.ContainsKey(groupName);
        }

        public Dictionary<string, List<string>> Groups()
        {
            return _groups;
        }

        public bool ImageDelete(string groupName, string imageName)
        {
            if (CanDeleteImages)
                _deletedImageNames.Add(imageName);

            return CanDeleteImages;
        }

        public bool ImageDelete(string groupName, string subgroupName, string imageName)
        {
            if (CanDeleteImages)
                _deletedImageNames.Add(imageName);

            return CanDeleteImages;
        }

        public bool ImageExists(string groupName, string imageName)
        {
            return _imageFiles.Where(i => i.Name.Equals(imageName)).Any();
        }

        public bool ImageExists(string groupName, string subgroupName, string imageName)
        {
            return _imageFiles.Where(i => i.Name.Equals(imageName)).Any();
        }

        public List<ImageFile> Images(string groupName)
        {
            return _imageFiles;
        }

        public List<ImageFile> Images(string groupName, string subgroupName)
        {
            return _imageFiles;
        }

        public bool SubgroupExists(string groupName, string subGroupName)
        {
            return _groups[groupName].Contains(subGroupName);
        }

        public string TemporaryImageFile(string fileExtension)
        {
            string tempFile = Path.GetTempFileName();
            _temporaryFileNames.Add(Path.ChangeExtension(tempFile, fileExtension));

            return tempFile;
        }

        public void AddFile(string groupName, string subgroupName, string fileName, byte[] fileContents)
        {
            if (ThrowExceptionWhenAddingFile)
            {
                throw new InvalidOperationException("Forced to throw exception");
            }

            if (String.IsNullOrEmpty(subgroupName))
                _filesAdded.Add($"{groupName}.{fileName}", fileContents);
            else
                _filesAdded.Add($"{groupName}.{subgroupName}.{fileName}", fileContents);
        }

        public List<string> DeletedImageList => _deletedImageNames;

        public List<string> TemporaryFiles => _temporaryFileNames;

        public Dictionary<string, byte[]> FilesAdded => _filesAdded;

        public bool CanDeleteImages { get; set; }

        public bool ThrowExceptionWhenAddingFile { get; set; }

        public static MockImageProvider CreateDefaultMockImageProvider()
        {
            Dictionary<string, List<string>> groups = new Dictionary<string, List<string>>()
            {
                { "Group 1", new List<string>() },
                { "Group 2", new List<string>() }
            };

            groups["Group 1"].Add("Group 1 Subgroup 1");
            groups["Group 1"].Add("Group 1 Subgroup 2");

            List<ImageFile> images = new List<ImageFile>()
            {
                { new ImageFile(new Uri("/", UriKind.RelativeOrAbsolute), "myfile.gif", ".gif", 23, DateTime.Now, DateTime.Now) }
            };

            return new MockImageProvider(groups, images);
        }

        public static MockImageProvider CreateDefaultMockImageProviderWithSubgroupAndImage()
        {
            Dictionary<string, List<string>> groups = new Dictionary<string, List<string>>()
            {
                { "Products", new List<string>() },
                { "General", new List<string>() }
            };

            groups["Products"].Add("C230");
            groups["Products"].Add("C330");

            List<ImageFile> images = new List<ImageFile>()
            {
                { new ImageFile(new Uri("/", UriKind.RelativeOrAbsolute), "myfile.gif", ".gif", 23, DateTime.Now, DateTime.Now) }
            };

            return new MockImageProvider(groups, images);
        }

        public static MockImageProvider CreateDefaultMockImageProviderForProductC()
        {
            Dictionary<string, List<string>> groups = new Dictionary<string, List<string>>()
            {
                { "Products", new List<string>() }
            };

            groups["Products"].Add("ProdC");

            List<ImageFile> images = new List<ImageFile>()
            {
                { new ImageFile(new Uri("/", UriKind.RelativeOrAbsolute), "myfile1.gif", ".gif", 23, DateTime.Now, DateTime.Now) },
                { new ImageFile(new Uri("/", UriKind.RelativeOrAbsolute), "myfile2_orig.gif", ".gif", 23, DateTime.Now, DateTime.Now) }
            };

            return new MockImageProvider(groups, images);
        }
    }
}
