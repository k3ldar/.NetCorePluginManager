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
 *  Copyright (c) 2018 - 2019 Simon Carter.  All Rights Reserved.
 *
 *  Product:  DynamicContent.Plugin
 *  
 *  File: YouTubeTemplateEditorModel.cs
 *
 *  Purpose: YouTube template editor model
 *
 *  Date        Name                Reason
 *  21/06/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

using SharedPluginFeatures;

namespace DynamicContent.Plugin.Model
{
    public sealed class YouTubeTemplateEditorModel
    {
        public YouTubeTemplateEditorModel(string data)
        {
            if (String.IsNullOrEmpty(data))
                data = Constants.PipeString;

            string[] parts = data.Split(Constants.PipeChar, StringSplitOptions.None);

            if (parts.Length > 0)
                VideoId = parts[0];

            if (parts.Length > 1)
                AutoPlay = parts[1].Equals(Boolean.TrueString, StringComparison.InvariantCultureIgnoreCase);

            Data = data;
        }

        public string VideoId { get; }

        public bool AutoPlay { get; }

        public string Data { get; set; }
    }
}
