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
 *  File: ContentItemDataRow.cs
 *
 *  Purpose:  Row definition for Table for dynamic content page items
 *
 *  Date        Name                Reason
 *  25/07/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using PluginManager.SimpleDB;

namespace PluginManager.DAL.TextFiles.Tables
{
    [Table(Constants.DomainDynamicContent, Constants.TableNameContentItem, CompressionType.None, CachingStrategy.Memory, WriteStrategy.Forced)]
    internal class ContentPageItemDataRow : TableRowDefinition
    {
        private long _pageId;
        private string _uniqueId;
        private string _cssClassName;
        private string _cssStyle;
        private int _sortOrder;
        private byte _widthType;
        private int _width;
        private byte _heightType;
        private int _height;
        private string _data;
        private long _activeFromTicks;
        private long _activeToTicks;
        private string _assemblyQualifiedName;

        public string AssemblyQualifiedName
        {
            get => _assemblyQualifiedName;

            set
            {
                if (value == _assemblyQualifiedName)
                    return;

                _assemblyQualifiedName = value;
                Update();
            }
        }

        [ForeignKey(Constants.TableNameContentPage)]
        public long PageId
        {
            get => _pageId;

            set
            {
                if (_pageId == value)
                    return;

                _pageId = value;
                Update();
            }
        }

        [UniqueIndex]
        public string UniqueId
        {
            get => _uniqueId;

            set
            {
                if (_uniqueId == value)
                    return;

                _uniqueId = value;
                Update();
            }
        }

        public string ClassName
        {
            get => _cssClassName;

            set
            {
                if (_cssClassName == value)
                    return;

                _cssClassName = value;
                Update();
            }
        }

        public string CssStyle
        {
            get => _cssStyle;

            set
            {
                if (_cssStyle == value)
                    return;

                _cssStyle = value;
                Update();
            }
        }

        public int SortOrder
        {
            get => _sortOrder;

            set
            {
                if (_sortOrder == value)
                    return;

                _sortOrder = value;
                Update();
            }
        }

        public byte WidthType
        {
            get => _widthType;

            set
            {
                if (_widthType == value)
                    return;

                _widthType = value;
                Update();
            }
        }

        public int Width
        {
            get => _width;

            set
            {
                if (_width == value)
                    return;

                _width = value;
                Update();
            }
        }

        public byte HeightType
        {
            get => _heightType;

            set
            {
                if (_heightType == value)
                    return;

                _heightType = value;
                Update();
            }
        }

        public int Height
        {
            get => _height;

            set
            {
                if (_height == value)
                    return;

                _height = value;
                Update();
            }
        }

        public string Data
        {
            get => _data;

            set
            {
                if (_data == value)
                    return;

                _data = value;
                Update();
            }
        }

        public long ActiveFromTicks
        {
            get => _activeFromTicks;

            set
            {
                if (_activeFromTicks == value)
                    return;

                _activeFromTicks = value;
                Update();
            }
        }

        public long ActiveToTicks
        {
            get => _activeToTicks;

            set
            {
                if (_activeToTicks == value)
                    return;

                _activeToTicks = value;
                Update();
            }
        }
    }
}
