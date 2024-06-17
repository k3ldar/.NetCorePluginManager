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
 *  Product:  PluginManager.DAL.TextFiles
 *  
 *  File: BlogsDataRow.cs
 *
 *  Purpose:  Table definition for blogs
 *
 *  Date        Name                Reason
 *  06/06/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using SimpleDB;

namespace PluginManager.DAL.TextFiles.Tables
{
	[Table(Constants.TableNameBlogs, CompressionType.Brotli)]
	internal sealed class BlogDataRow : TableRowDefinition
	{
		#region Private Members

		private ObservableList<string> _tags;
		private ObservableList<BlogCommentDataRow> _comments;
		private DateTime _publishDateTime;
		private bool _published;
		private string _username;
		private string _blogText;
		private string _excerpt;
		private string _title;
		private long _userId;

		#endregion Private Members

		public BlogDataRow()
		{
			_comments = new ObservableList<BlogCommentDataRow>();
			_comments.Changed += ObservableDataChanged;
			_tags = new ObservableList<string>();
			_tags.Changed += ObservableDataChanged;
		}

		/// <summary>
		/// Unique id of user creating the blog entry.
		/// </summary>
		[ForeignKey(Constants.TableNameUsers)]
		public long UserId
		{
			get => _userId;

			set
			{
				if (_userId == value)
					return;

				_userId = value;
				Update();
			}
		}

		/// <summary>
		/// Title of blog entry.
		/// </summary>
		/// <value>string</value>
		public string Title
		{
			get => _title;

			set
			{
				if (_title == value)
					return;

				_title = value;
				Update();
			}
		}

		/// <summary>
		/// Brief description describing the blog entry.
		/// </summary>
		/// <value>string</value>
		public string Excerpt
		{
			get => _excerpt;

			set
			{
				if (_excerpt == value)
					return;

				_excerpt = value;
				Update();
			}
		}

		/// <summary>
		/// The main blog text.
		/// </summary>
		/// <value>string</value>
		public string BlogText
		{
			get => _blogText;

			set
			{
				if (_blogText == value)
					return;

				_blogText = value;
				Update();
			}
		}

		/// <summary>
		/// Name of user creating the blog entry.
		/// </summary>
		/// <string>string</string>
		/// <value>string</value>
		public string Username
		{
			get => _username;

			set
			{
				if (_username == value)
					return;

				_username = value;
				Update();
			}
		}

		/// <summary>
		/// Indicates whether the blog entry has been published or not.
		/// </summary>
		/// <value>bool</value>
		public bool Published
		{
			get => _published;

			set
			{
				if (_published == value)
					return;

				_published = value;
				Update();
			}
		}

		/// <summary>
		/// The date/time the blog entry will appear live on the website.
		/// </summary>
		/// <value>DateTime</value>
		public DateTime PublishDateTime
		{
			get => _publishDateTime;

			set
			{
				if (_publishDateTime == value)
					return;

				_publishDateTime = value;
				Update();
			}
		}


		/// <summary>
		/// Descriptive tags for the blog.
		/// </summary>
		/// <value>List&lt;string&gt;</value>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0016:Use 'throw' expression", Justification = "Validation required before removing changed event")]
		public ObservableList<string> Tags
		{
			get => _tags;

			set
			{
				if (value == null)
					throw new InvalidOperationException();

				if (_tags != null)
					_tags.Changed -= ObservableDataChanged;

				_tags = value;
				_tags.Changed += ObservableDataChanged;
				Update();
			}
		}

		/// <summary>
		/// List of comments for the blog entry.
		/// </summary>
		/// <value>List&lt;BlogComment&gt;</value>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0016:Use 'throw' expression", Justification = "Validation required before removing changed event")]
		public ObservableList<BlogCommentDataRow> Comments
		{
			get => _comments;

			set
			{
				if (value == null)
					throw new InvalidOperationException();

				if (_comments != null)
					_comments.Changed -= ObservableDataChanged;

				_comments = value;
				_comments.Changed += ObservableDataChanged;
				Update();
			}
		}
	}
}
