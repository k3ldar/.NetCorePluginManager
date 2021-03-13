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
 *  Copyright (c) 2012 - 2021 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginMiddleware
 *  
 *  File: ISmokeTestProvider.cs
 *
 *  Purpose:  Smoke test provider
 *
 *  Date        Name                Reason
 *  19/06/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using Shared.Classes;

namespace Middleware
{
    /// <summary>
    /// Provides an opportunity for the website to provide custom data which can be used as part
    /// of the smoke test.
    /// </summary>
    public interface ISmokeTestProvider
    {
        /// <summary>
        /// Indicates the test is about to start.
        /// 
        /// The result of this method can contain custom name value pairs, this can be used by the Smoke Test engine
        /// when preparing tests to be run.
        /// 
        /// For example the following name value pair could be used to indicate a logged on user name and password:
        /// 
        /// LoggedOnUser=admin
        /// LoggedOnUserPassword=qwerty123
        /// 
        /// When a test is being prepared, it will search for {LoggedOnUser} within the body, or form parameters and
        /// the value will be replaced with admin and {LoggedOnUserPassword} will be searched for in the form parameters
        /// or body and replaced with qwerty123
        /// 
        /// Ideally the test data should be created when this function is called and cleaned up using <seealso cref="SmokeTestEnd"/>.
        /// 
        /// Data can be created dynamically within a database or other storage mechanism to help test functionality of a website.
        /// </summary>
        /// <returns>NVPCodec instance containing name value pairs that will be returned to the Smoke Test Engine or null if not required.</returns>
        NVPCodec SmokeTestStart();

        /// <summary>
        /// Indicates that the Smoke Test has finished and that all previously created data can be cleaned up and removed.
        /// </summary>
        void SmokeTestEnd();
    }
}
