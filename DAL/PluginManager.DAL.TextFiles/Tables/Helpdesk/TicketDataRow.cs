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
 *  File: FeedbackDataRow.cs
 *
 *  Purpose:  Table definition for ticket departments
 *
 *  Date        Name                Reason
 *  18/07/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using SimpleDB;

namespace PluginManager.DAL.TextFiles.Tables
{
	[Table(Constants.DomainHelpdesk, Constants.TableNameTicket, CompressionType.Brotli)]
	internal class TicketDataRow : TableRowDefinition
	{
		public const string IndexUserKey = "UserKey";

		private long _priority;
		private long _department;
		private long _status;
		private string _key;
		private string _subject;
		private string _createdBy;
		private string _createdByEmail;
		private string _lastReplier;
		private ObservableList<TicketMessageDataRow> _messages;

		public TicketDataRow()
		{
			_messages = new ObservableList<TicketMessageDataRow>();
			_messages.Changed += ObservableDataChanged;
		}

		[ForeignKey(Constants.TableNameTicketPriorities)]
		public long Priority
		{
			get => _priority;

			set
			{
				if (_priority == value)
					return;

				_priority = value;
				Update();
			}
		}

		[ForeignKey(Constants.TableNameTicketDepartments)]
		public long Department
		{
			get => _department;

			set
			{
				if (_department == value)
					return;

				_department = value;
				Update();
			}
		}

		[ForeignKey(Constants.TableNameTicketStatus)]
		public long Status
		{
			get => _status;

			set
			{
				if (_status == value)
					return;

				_status = value;
				Update();
			}
		}

		[UniqueIndex(IndexUserKey)]
		public string Key
		{
			get => _key;

			set
			{
				if (_key == value)
					return;

				_key = value;
				Update();
			}
		}

		public string Subject
		{
			get => _subject;

			set
			{
				if (_subject == value)
					return;

				_subject = value;
				Update();
			}
		}

		public string CreatedBy
		{
			get => _createdBy;

			set
			{
				if (_createdBy == value)
					return;

				_createdBy = value;
				Update();
			}
		}

		[UniqueIndex(IndexUserKey)]
		public string CreatedByEmail
		{
			get => _createdByEmail;

			set
			{
				if (_createdByEmail == value)
					return;

				_createdByEmail = value;
				Update();
			}
		}

		public string LastReplier
		{
			get => _lastReplier;

			set
			{
				if (_lastReplier == value)
					return;

				_lastReplier = value;
				Update();
			}
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0016:Use 'throw' expression", Justification = "Validation required before removing changed event")]
		public ObservableList<TicketMessageDataRow> Messages
		{
			get => _messages;

			set
			{
				if (value == null)
					throw new InvalidOperationException();

				if (_messages != null)
					_messages.Changed -= ObservableDataChanged;

				_messages = value;
				_messages.Changed += ObservableDataChanged;
				Update();
			}
		}
	}
}
