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
 *  Copyright (c) 2018 - 2021 Simon Carter.  All Rights Reserved.
 *
 *  Product:  SharedPluginFeatures
 *  
 *  File: IUserSessionService.cs
 *
 *  Purpose:  Provides interface for Managing user sessions
 *
 *  Date        Name                Reason
 *  04/10/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using Shared.Classes;

namespace SharedPluginFeatures
{
    /// <summary>
    /// Provides an interface for saving UserSession data.
    /// 
    /// This interface needs to be implemented by the host application which can control how the 
    /// session data is stored for the application.  The implementation must be registered as a service available
    /// usind IoC through the default DI container.
    /// 
    /// Session data is managed by it's own thread and all implementation of this method must be thread safe.
    /// 
    /// By using a seperate thread the current pipeline requests are not delayed waiting for data to be saved.
    /// 
    /// When implementing these methods, the hosting class must update the SaveStatus property for the 
    /// session and page data.
    /// </summary>
    public interface IUserSessionService
    {
        /// <summary>
        /// Indicates the current session requires saving.
        /// </summary>
        /// <param name="userSession">UserSession that needs to be saved.</param>
        void Save(in UserSession userSession);

        /// <summary>
        /// Requests that a previously saved session is loaded.  Mostly used if implemented within a web farm and
        /// the users request is passed to a different server for processing.
        /// </summary>
        /// <param name="userSessionId">Id of session required.</param>
        /// <param name="userSession">UserSession instance that should be populated with user session data.</param>
        void Retrieve(in string userSessionId, ref UserSession userSession);

        /// <summary>
        /// Indicates the current session has expired and will be removed from the list of monitored sessions 
        /// and may require saving.
        /// </summary>
        /// <param name="userSession">UserSession being expired.</param>
        void Closing(in UserSession userSession);

        /// <summary>
        /// Indicates tha page views requires saving.
        /// 
        /// When implementing this methods, the hosting class must update the SaveStatus property for the 
        /// session and page data.
        /// </summary>
        /// <param name="pageView"></param>
        void SavePage(in UserSession pageView);

        /// <summary>
        /// Indicates a session has been created and needs to be saved.
        /// 
        /// When implementing this methods, the hosting class must update the SaveStatus property for the 
        /// session and page data.
        /// </summary>
        /// <param name="userSession">UserSession that has been created.</param>
        void Created(in UserSession userSession);
    }
}
