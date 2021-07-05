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
 *  Copyright (c) 2018 - 2020 Simon Carter.  All Rights Reserved.
 *
 *  Product:  DynamicContent.Plugin
 *  
 *  File: EditTemplateModel.cs
 *
 *  Purpose:  generic template editor model
 *
 *  Date        Name                Reason
 *  19/12/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

using SharedPluginFeatures;

namespace DynamicContent.Plugin.Model
{
    public sealed class EditTemplateModel
    {
        #region Properties

        public string CacheId { get; set; }

        public string UniqueId { get; set; }

        public string EditorInstructions { get; set; }

        public string TemplateEditor { get; set; }

        public string Name { get; set; }

        public int SortOrder { get; set; }

        public DynamicContentWidthType WidthType { get; set; }

        public int Width { get; set; }

        public DynamicContentHeightType HeightType { get; set; }

        public int Height { get; set; }

        public string Data { get; set; }

        public DateTime ActiveFrom { get; set; }

        #endregion Properties
    }
}
