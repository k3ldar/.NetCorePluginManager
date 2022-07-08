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
 *  Copyright (c) 2018 - 2022 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginManager.DAL.TextFiles
 *  
 *  File: BlogCommentData.cs
 *
 *  Purpose:  Definition for blog comments
 *
 *  Date        Name                Reason
 *  27/06/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using PluginManager.DAL.TextFiles.Internal;

namespace PluginManager.DAL.TextFiles.Tables
{
    [Table(Constants.TableNameBlogComments)]
    internal sealed class BlogCommentDataRow : TableRowDefinition
    {
        private long _blogId;
        private long? _parentComment;
        private string _username;
        private long _userId;
        private bool _approved;
        private string _comment;
        private ObservableList<BlogCommentDataRow> _comments;

        #region Properties

        [ForeignKey(Constants.TableNameBlogs)]
        public long BlogId
        {
            get
            {
                return _blogId;
            }

            set
            {
                if (_blogId == value)
                    return;

                _blogId = value;
                Update();
            }
        }

        [ForeignKey(Constants.TableNameBlogComments)]
        public long? ParentComment
        {
            get
            {
                return _parentComment;
            }

            set
            {
                if (_parentComment == value)
                    return;

                _parentComment = value;
                Update();
            }
        }

        public string Username
        {
            get
            {
                return _username;
            }

            set
            {
                if (_username == value)
                    return;

                _username = value;
                Update();
            }
        }

        [ForeignKey(Constants.TableNameUsers)]
        public long UserId
        {
            get
            {
                return _userId;
            }

            set
            {
                if (_userId == value)
                    return;

                _userId = value;
                Update();
            }
        }

        public bool Approved
        {
            get
            {
                return _approved;
            }

            set
            {
                if (_approved == value)
                    return;

                _approved = value;
                Update();
            }
        }

        public string Comment
        {
            get
            {
                return _comment;
            }

            set
            {
                if (_comment == value)
                    return;

                _comment = value;
                Update();
            }
        }

        public ObservableList<BlogCommentDataRow> Comments
        {
            get
            {
                return _comments;
            }

            set
            {
                if (value != null)
                    value.Changed -= ObservableDataChanged;

                _comments = value;
                _comments.Changed += ObservableDataChanged;
                Update();
            }
        }

        #endregion Properties
    }
}
