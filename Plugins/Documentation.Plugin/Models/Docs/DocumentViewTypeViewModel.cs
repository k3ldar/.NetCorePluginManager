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
	/// <summary>
	/// Summary document for a documents type, method, field, constructor, property etc.
	/// </summary>
	public sealed class DocumentViewTypeViewModel : BaseModel
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="modelData">Base model data.</param>
		/// <param name="title">Title of document</param>
		/// <param name="allReferences">All references associated with the document.</param>
		public DocumentViewTypeViewModel(in BaseModelData modelData,
			in string title, in string allReferences)
			: base(modelData)
		{
			if (String.IsNullOrEmpty(title))
				throw new ArgumentNullException(nameof(title));

			Title = title;
			AllReferences = allReferences ?? String.Empty;
		}

		#endregion Constructors

		#region Properties

		/// <summary>
		/// Title for type
		/// </summary>
		/// <value>string</value>
		public string Title { get; private set; }

		/// <summary>
		/// Short description for the type.
		/// </summary>
		/// <value>string</value>
		public string ShortDescription { get; set; }

		/// <summary>
		/// Long description of the type.
		/// </summary>
		/// <value>string</value>
		public string LongDescription { get; set; }

		/// <summary>
		/// All references associated with type.
		/// </summary>
		/// <value>string</value>
		public string AllReferences { get; private set; }

		/// <summary>
		/// Determines whether strings are translated.
		/// </summary>
		/// <value>bool</value>
		public bool TranslateStrings { get; set; }

		/// <summary>
		/// List of alternative references linked to this type.
		/// </summary>
		/// <value>Dictionary&lt;string, string&gt;</value>
		public Dictionary<string, string> SeeAlso { get; set; }

		/// <summary>
		/// Namespace for the type.
		/// </summary>
		/// <value>string</value>
		public string Namespace { get; set; }

		/// <summary>
		/// Name of assembly where the type belongs.
		/// </summary>
		/// <value>string</value>
		public string Assembly { get; set; }

		/// <summary>
		/// Class name that the type belongs to.
		/// </summary>
		/// <value>string</value>
		public string ClassName { get; set; }

		/// <summary>
		/// Name of type.
		/// </summary>
		/// <value>string</value>
		public string TypeName { get; set; }

		/// <summary>
		/// Example useage.
		/// </summary>
		/// <value>string</value>
		public string ExampleUseage { get; set; }

		/// <summary>
		/// Value of type.
		/// </summary>
		/// <value>string</value>
		public string Value { get; set; }

		/// <summary>
		/// Value that is returned by the type.
		/// </summary>
		/// <value>string</value>
		public string Returns { get; set; }

		/// <summary>
		/// Any parameters that exist for the type.
		/// </summary>
		/// <value>List&lt;DocumentMethodParameter&gt;</value>
		public List<DocumentMethodParameter> Parameters { get; set; }

		/// <summary>
		/// Document summary
		/// </summary>
		/// <value>string</value>
		public string Summary { get; set; }

		/// <summary>
		/// Any supplementary information regarding the class, type, method, constructor, property etc
		/// </summary>
		/// <value>string</value>
		public string Remarks { get; set; }

		/// <summary>
		/// Preformatted exceptions for the relevant constructor, method or property
		/// </summary>
		public string Exceptions { get; set; }

		#endregion Properties
	}
}
