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
 *  Product:  Documentation Plugin
 *  
 *  File: DocumentViewTypeViewModel.cs
 *
 *  Purpose:  View model for a document/assembly
 *
 *  Date        Name                Reason
 *  27/05/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

using Shared.Docs;

using SharedPluginFeatures;

namespace DocumentationPlugin.Models
{
    public sealed class DocumentViewTypeViewModel : BaseModel
    {
        #region Constructors

        public DocumentViewTypeViewModel(in List<BreadcrumbItem> breadcrumbs, in ShoppingCartSummary cartSummary,
            in string title, in string allReferences)
            : base(breadcrumbs, cartSummary)
        {
            if (String.IsNullOrEmpty(title))
                throw new ArgumentNullException(nameof(title));

            Title = title;
            AllReferences = allReferences ?? String.Empty;
        }

        #endregion Constructors

        #region Properties

        public string Title { get; private set; }

        public string ShortDescription { get; set; }

        public string LongDescription { get; set; }

        public string AllReferences { get; private set; }

        public bool TranslateStrings { get; set; }

        public Dictionary<string, string> SeeAlso { get; set; }

        public string Namespace { get; set; }

        public string Assembly { get; set; }

        public string ClassName { get; set; }

        public string TypeName { get; set; }

        public string ExampleUseage { get; set; }

        public string Value { get; set; }

        public string Returns { get; set; }

        public List<DocumentMethodParameter> Parameters { get; set; }

        public string Summary { get; set; }
        
        #endregion Properties
    }
}
