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
 *  File: DocumentViewModel.cs
 *
 *  Purpose:  View model for a document/assembly
 *
 *  Date        Name                Reason
 *  20/05/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

using Shared.Docs;

using SharedPluginFeatures;

namespace DocumentationPlugin.Models
{
    public sealed class DocumentViewModel : BaseModel
    {
        #region Constructors

        public DocumentViewModel(in List<BreadcrumbItem> breadcrumbs, in ShoppingCartSummary cartSummary,
            in string title, in string shortDescription, in string longDescription, in string allReferences)
            : base (breadcrumbs, cartSummary)
        {
            if (String.IsNullOrEmpty(title))
                throw new ArgumentNullException(nameof(title));

            Title = title;
            ShortDescription = shortDescription ?? throw new ArgumentNullException(nameof(shortDescription));
            LongDescription = longDescription ?? throw new ArgumentNullException(nameof(longDescription));
            AllReferences = allReferences ?? String.Empty;
        }

        #endregion Constructors

        #region Properties

        public string Title { get; private set; }

        public string ShortDescription { get; private set; }

        public string LongDescription { get; private set; }

        public string AllReferences { get; private set; }

        public bool TranslateStrings { get; set; }

        public Dictionary<string, string> Contains { get; set; }

        public Dictionary<string, string> SeeAlso { get; set; }

        public string Namespace { get; set; }

        public string Assembly { get; set; }

        public List<DocumentField> Fields { get; set; }

        public List<DocumentMethod> Methods { get; set; }

        public List<DocumentMethodException> Exceptions { get; set; }

        public List<DocumentProperty> Properties { get; set; }

        public List<DocumentMethod> Constructors { get; set; }

        public string PreviousDocument { get; set; }

        public string NextDocument { get; set; }

        #endregion Properties
    }
}
