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
 *  Product:  Blog Plugin
 *  
 *  File: Comment.cs
 *
 *  Purpose:  Blog post comment view model
 *
 *  Date        Name                Reason
 *  21/06/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

namespace Blog.Plugin.Models
{
	/// <summary>
	/// Comment made by a user for a blog entry
	/// </summary>
	public sealed class BlogCommentViewModel
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="id">Unique id of the comment.</param>
		/// <param name="dateTime">Date and time comment made.</param>
		/// <param name="userName">Name of person making the comment.</param>
		/// <param name="comment">Comment made by a user.</param>
		public BlogCommentViewModel(in int id, in DateTime dateTime, in string userName,
			in string comment)
		{
			if (String.IsNullOrEmpty(userName))
				throw new ArgumentNullException(nameof(userName));

			if (String.IsNullOrEmpty(comment))
				throw new ArgumentNullException(nameof(comment));

			Id = id;
			DateTime = dateTime;
			Username = userName;
			Comment = comment;
		}

		#endregion Constructors

		#region Properties

		/// <summary>
		/// Unique id for the comment.
		/// </summary>
		/// <value>int</value>
		public int Id { get; private set; }

		/// <summary>
		/// Date/Time the comment was made.
		/// </summary>
		/// <value>DateTime</value>
		public DateTime DateTime { get; private set; }

		/// <summary>
		/// Name of user making the comment.
		/// </summary>
		/// <value>string</value>
		public string Username { get; set; }

		/// <summary>
		/// Comment made by a user
		/// </summary>
		/// <value>string</value>
		public string Comment { get; set; }

		#endregion Properties
	}
}
