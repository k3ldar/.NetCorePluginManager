/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 *  Plugin Manager is distributed under the GNU General Public License version 3 and  
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
 *  Product:  PluginManager
 *  
 *  File: Constants.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  28/12/2019  Simon Carter        Converted to generic plugin that can be used by any 
 *                                  application type.  Originally part of .Net 
 *                                  Core Plugin Manager (AspNetCore.PluginManager)
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

namespace PluginManager.Internal
{
	internal static class Constants
	{
		/// <summary>
		/// Exception message used when registering a INotificationListener with the INotificationService message notifications, when the listener does not provide any event names.
		/// </summary>
		public const string InvalidListener = "Listener must provide at least one event";

		/// <summary>
		/// Exception message used when registering a INotificationListener with the INotificationService message notifications, when the event name has not been recognised.
		/// </summary>
		public const string InvalidEventName = "Invalid event name in listener";

		/// <summary>
		/// Name of thread used for INotificationService message notifications
		/// </summary>
		public const string ThreadNotificationService = "Notification Service";
	}
}
