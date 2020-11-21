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
 *  File: Constants.cs
 *
 *  Purpose:  Global constants for documentation
 *
 *  Date        Name                Reason
 *  12/04/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

namespace DocumentationPlugin.Classes
{
    /// <summary>
    /// Global constants for <see cref="DocumentationPlugin"/>
    /// </summary>
    public sealed class Constants
    {
        #region Constants

        /// <summary>
        /// Count value for &lt;see&gt; references replaced.
        /// </summary>
        public const string CountValueSee = "See";

        /// <summary>
        /// Count value for &lt;see&gt; references not found.
        /// </summary>
        public const string CountValueSeeNotFound = "SeeNotFound";

        /// <summary>
        /// Count value for &lt;seealso&gt; references replaced.
        /// </summary>
        public const string CountValueSeeAlso = "SeeAlso";

        /// <summary>
        /// Count value for &lt;seealso&gt; references not found.
        /// </summary>
        public const string CountValueSeeAlsoNotFound = "SeeAlsoNotFound";

        /// <summary>
        /// Count value for &lt;para&gt; references replaced.
        /// </summary>
        public const string CountValueParaOpen = "ParaOpen";

        /// <summary>
        /// Count value for &lt;/para&gt; references closing tag.
        /// </summary>
        public const string CountValueParaClose = "ParaClose";

        /// <summary>
        /// Count value for &lt;code&gt; references replaced.
        /// </summary>
        public const string CountValueCodeOpen = "CodeOpen";

        /// <summary>
        /// Count value for &lt;/code&gt; references closing tag.
        /// </summary>
        public const string CountValueCodeClose = "CodeClose";

        #endregion Constants
    }
}
