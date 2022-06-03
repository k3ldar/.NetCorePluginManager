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
 *  Product:  PluginManager.DAL.TextFiles
 *  
 *  File: ReaderWriterInitializer.cs
 *
 *  Purpose:  ReaderWriterInitializer for text based storage
 *
 *  Date        Name                Reason
 *  23/05/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using PluginManager.Abstractions;
using PluginManager.DAL.TextFiles.Internal;

using Shared.Classes;

namespace PluginManager.DAL.TextFiles
{
    public sealed class ReaderWriterInitializer : IReaderWriterInitializer
    {
        public const uint DefaultMinimumVersion = 1;

        private uint _minimumVersion = DefaultMinimumVersion;
        private readonly Dictionary<string, ITextTable> _tables = new Dictionary<string, ITextTable>();
        private readonly object _lock = new object();

        private ReaderWriterInitializer(IForeignKeyManager foreignKeyManager)
        {
            ForeignKeyManager = foreignKeyManager ?? throw new ArgumentNullException(nameof(foreignKeyManager));
        }

        public ReaderWriterInitializer(ISettingsProvider settingsProvider, IForeignKeyManager foreignKeyManager)
            : this (foreignKeyManager)
        {
            if (settingsProvider == null)
                throw new ArgumentNullException(nameof(settingsProvider));

            TextFileSettings settings = settingsProvider.GetSettings<TextFileSettings>(nameof(TextFileSettings));

            if (settings == null)
                throw new InvalidOperationException();

            if (String.IsNullOrEmpty(settings.Path))
                throw new ArgumentNullException(nameof(settings.Path));

            if (!Directory.Exists(settings.Path))
                throw new ArgumentException($"Path does not exist: {settings.Path}", nameof(settings.Path));

            Path = settings.Path;
        }

        public ReaderWriterInitializer(string path, IForeignKeyManager foreignKeyManager)
            : this(foreignKeyManager)
        {
            if (String.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));

            if (!Directory.Exists(path))
                throw new ArgumentException($"Path does not exist: {path}", nameof(path));

            Path = path;
        }


        public string Path { get; private set; }

        public uint MinimumVersion
        {
            get
            {
                return _minimumVersion;
            }

            set
            {
                if (value < DefaultMinimumVersion)
                    value = DefaultMinimumVersion;

                _minimumVersion = value;
            }
        }

        public IForeignKeyManager ForeignKeyManager { get; }

        public void RegisterTable(ITextTable textTable)
        {
            if (textTable == null)
                throw new ArgumentNullException(nameof(textTable));

            using (TimedLock timedLock = TimedLock.Lock(_lock))
            {
                if (_tables.ContainsKey(textTable.TableName))
                    throw new ArgumentException($"Table {textTable.TableName} already exists");

                _tables.Add(textTable.TableName, textTable);
            }
        }

        public void UnregisterTable(ITextTable textTable)
        {
            if (textTable == null)
                throw new ArgumentNullException(nameof(textTable));

            using (TimedLock timedLock = TimedLock.Lock(_lock))
            {
                if (!_tables.ContainsKey(textTable.TableName))
                    throw new ArgumentException($"Table {textTable.TableName} already exists");

                _tables.Remove(textTable.TableName);
            }
        }

        public IReadOnlyDictionary<string, ITextTable> Tables
        {
            get
            {
                return new Dictionary<string, ITextTable>(_tables);
            }
        }
    }
}
