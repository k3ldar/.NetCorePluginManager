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
    /// <summary>
    /// Model used when viewing a document
    /// </summary>
    public sealed class DocumentViewModel : BaseModel
    {
        #region Constructors

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="modelData">Base model data.</param>
        /// <param name="title">Title of document.</param>
        /// <param name="shortDescription">Short description for the document</param>
        /// <param name="longDescription">Long description for the document.</param>
        /// <param name="allReferences">Any references found in other documents.</param>
        public DocumentViewModel(in BaseModelData modelData,
            in string title, in string shortDescription, in string longDescription, in string allReferences)
            : base(modelData)
        {
            if (String.IsNullOrEmpty(title))
                throw new ArgumentNullException(nameof(title));

            Title = title;
            ShortDescription = shortDescription ?? throw new ArgumentNullException(nameof(shortDescription));
            LongDescription = longDescription ?? throw new ArgumentNullException(nameof(longDescription));
            AllReferences = allReferences ?? String.Empty;
            SeeAlso = new Dictionary<string, string>();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Document title.
        /// </summary>
        /// <value>string</value>
        public string Title { get; private set; }

        /// <summary>
        /// Short description for the document.
        /// </summary>
        /// <value>string</value>
        public string ShortDescription { get; private set; }

        /// <summary>
        /// Long description for the document.
        /// </summary>
        /// <value>string</value>
        public string LongDescription { get; private set; }

        /// <summary>
        /// All references found for the document.
        /// </summary>
        /// <value>string</value>
        public string AllReferences { get; private set; }

        /// <summary>
        /// Determines whether strings should be translated.
        /// </summary>
        /// <value>bool</value>
        public bool TranslateStrings { get; set; }

        /// <summary>
        /// List of sub sections within the document.
        /// </summary>
        /// <value>Dictionary&lt;string, string&gt;</value>
        public Dictionary<string, string> Contains { get; set; }

        /// <summary>
        /// List of other Documents linked to this document.
        /// </summary>
        /// <value>Dictionary&lt;string, string&gt;</value>
        public Dictionary<string, string> SeeAlso { get; set; }

        /// <summary>
        /// Namespace the document belongs to.
        /// </summary>
        /// <value>string</value>
        public string Namespace { get; set; }

        /// <summary>
        /// Assembly the document belongs to.
        /// </summary>
        /// <value>string</value>
        public string Assembly { get; set; }

        /// <summary>
        /// List of any fields contained within the document.
        /// </summary>
        /// <value>List&lt;DocumentField&gt;</value>
        public List<DocumentField> Fields { get; set; }

        /// <summary>
        /// List of any methods contained within the document.
        /// </summary>
        /// <value>List&lt;DocumentMethod&gt;</value>
        public List<DocumentMethod> Methods { get; set; }

        /// <summary>
        /// Exceptions contained within the document.
        /// </summary>
        /// <value>string</value>
        public string Exceptions { get; set; }

        /// <summary>
        /// List of any properties contained within the document
        /// </summary>
        /// <value>List&lt;DocumentProperty&gt;</value>
        public List<DocumentProperty> Properties { get; set; }

        /// <summary>
        /// List of constructors if any exist.
        /// </summary>
        /// <value>List&lt;DocumentMethod&gt;</value>
        public List<DocumentMethod> Constructors { get; set; }

        /// <summary>
        /// Previous root document in the list.
        /// </summary>
        /// <value>string</value>
        public string PreviousDocument { get; set; }

        /// <summary>
        /// Next root document in the list.
        /// </summary>
        /// <value>string</value>
        public string NextDocument { get; set; }

        /// <summary>
        /// Example text, this should be formatted html
        /// </summary>
        /// <value>string</value>
        public string Example { get; set; }

        /// <summary>
        /// Indicates whether the short description is shown or not.
        /// </summary>
        /// <value>bool.  If true and there is a short description, it will be shown.</value>
        public bool ShowShortDescription { get; set; }

        /// <summary>
        /// Any supplementary information regarding the class, type, method, constructor, property etc
        /// </summary>
        /// <value>string</value>
        public string Remarks { get; set; }

        #endregion Properties
    }
}
