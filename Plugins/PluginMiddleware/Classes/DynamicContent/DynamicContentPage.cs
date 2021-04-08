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
 *  Product:  SharedPluginFeatures
 *  
 *  File: DynamicContentPage.cs
 *
 *  Purpose:  Dynamic content page
 *
 *  Date        Name                Reason
 *  05/12/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Linq;

using SharedPluginFeatures.DynamicContent;

namespace Middleware.DynamicContent
{
    /// <summary>
    /// Contains the contents for a dynamic page
    /// </summary>
    public sealed class DynamicContentPage : IDynamicContentPage
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public DynamicContentPage()
        {
            Content = new List<DynamicContentTemplate>();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Unique page id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of dynamic page
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Dynamic content that will be displayed within the page
        /// </summary>
        public List<DynamicContentTemplate> Content { get; private set; }

        #endregion Properties

        #region Public Methods

        /// <summary>
        /// Adds a new template items to the dynamic content page
        /// </summary>
        /// <param name="dynamicContentTemplate">New dynamic content template to be added.</param>
        /// <param name="beforeControlId">Unique id of the control which the new template will be placed next to (before).</param>
        public void AddContentTemplate(DynamicContentTemplate dynamicContentTemplate, string beforeControlId)
        {
            if (dynamicContentTemplate == null)
                throw new ArgumentNullException(nameof(dynamicContentTemplate));

            int index = Content.Count;

            if (Content.Count > 0 && !String.IsNullOrEmpty(beforeControlId))
            {
                DynamicContentTemplate nextPage = Content.Where(c => c.UniqueId.Equals(beforeControlId)).FirstOrDefault();

                if (nextPage != null)
                {
                    index = Content.IndexOf(nextPage);
                }
            }

            DynamicContentTemplate clone = dynamicContentTemplate.Clone(GetNextControlName());
            Content.Insert(index, clone);
            UpdateSortOrders();
        }

        #endregion Public Methods

        #region Private Methods

        private void UpdateSortOrders()
        {
            for (int i = 0; i < Content.Count; i++)
            {
                Content[i].SortOrder = i;
            }
        }

        private string GetNextControlName()
        {
            const string ControlName = "Control-{0}";

            int nextIndex = Content.Count;
            string Result = String.Empty;

            do
            {
                nextIndex++;
                Result = String.Format(ControlName, nextIndex);
            }
            while (Content.Where(c => c.UniqueId.Equals(Result)).Any());

            return Result;
        }

        #endregion Private Methods
    }
}
