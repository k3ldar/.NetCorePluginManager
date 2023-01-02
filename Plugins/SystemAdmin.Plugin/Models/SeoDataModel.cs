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
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
 *
 *  Product:  SystemAdmin.Plugin
 *  
 *  File: SeoDataModel.cs
 *
 *  Purpose:  Model for viewing/updating seo data
 *
 *  Date        Name                Reason
 *  04/08/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.ComponentModel.DataAnnotations;

#pragma warning disable CS1591

namespace SystemAdmin.Plugin.Models
{
    public sealed class SeoDataModel
    {
        #region Constructors

        public SeoDataModel()
        {
			SeoTitle = string.Empty;
			SeoUrl = string.Empty;
			SeoTags = string.Empty;
			SeoAuthor = string.Empty;
        }

        public SeoDataModel(string url)
			:this ()
        {
            if (String.IsNullOrEmpty(url))
                throw new ArgumentNullException(nameof(url));

            SeoUrl = url;
        }

        #endregion Constructors

        #region Properties

        [Display(Name = nameof(Languages.LanguageStrings.Title))]
        [StringLength(20, MinimumLength = 5)]
        [Required(ErrorMessage = nameof(Languages.LanguageStrings.TitleRequired))]
        public string SeoTitle { get; set; }

        [Display(Name = nameof(Languages.LanguageStrings.MetaDescription))]
        [StringLength(20, MinimumLength = 5)]
        [Required(ErrorMessage = nameof(Languages.LanguageStrings.MetaDescriptionRequired))]
        public string SeoMetaDescription { get; set; }

        [Display(Name = nameof(Languages.LanguageStrings.Tags))]
        [StringLength(80, MinimumLength = 5)]
        public string SeoTags { get; set; }

        [Display(Name = nameof(Languages.LanguageStrings.Author))]
        [StringLength(20, MinimumLength = 0)]
        public string SeoAuthor { get; set; }

        public string SeoUrl { get; set; }

        #endregion Properties
    }
}

#pragma warning restore CS1591