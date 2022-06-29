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
                _blogId = value;
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
                _parentComment = value;
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
                _username = value;
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
                _userId = value;
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
                _approved = value;
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
                _comment = value;
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
                _comments = value;
            }
        }

        #endregion Properties
    }
}
