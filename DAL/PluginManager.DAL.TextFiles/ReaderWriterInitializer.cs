using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PluginManager.DAL.TextFiles.Interfaces;

namespace PluginManager.DAL.TextFiles
{
    public sealed class ReaderWriterInitializer : IReaderWriterInitializer
    {
        public const uint DefaultMinimumVersion = 1;

        private uint _minimumVersion = DefaultMinimumVersion;

        public ReaderWriterInitializer(string path)
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
    }
}
