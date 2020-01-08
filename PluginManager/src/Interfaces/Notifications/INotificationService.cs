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
 *  Copyright (c) 2012 - 2018 Simon Carter.  All Rights Reserved.
 *
 *  Product:  SharedPluginFeatues
 *  
 *  File: INotificationListner.cs
 *
 *  Purpose:  Event listener interface
 *
 *  Date        Name                Reason
 *  12/05/2019  Simon Carter        Initially Created
 *  28/12/2019  Simon Carter        Converted to generic plugin that can be used by any 
 *                                  application type.  Originally part of .Net 
 *                                  Core Plugin Manager
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

namespace PluginManager.Abstractions
{
    /// <summary>
    /// This interface is designed to allow callers to register and unregister INotificationListener objects and raise events throughout the system.
    /// </summary>
    public interface INotificationService
    {
        /// <summary>
        /// Raises an event that will be broadcast to all listeners.  This method will send the message within the same thread, this 
        /// could incur slight delays whilst the message is being responded to and should be used only when a response is 
        /// required for processing purposes.
        /// </summary>
        /// <param name="eventId">Name of the event being raised.</param>
        /// <param name="param1">User defined parameter value, pertinent to the event.</param>
        /// <param name="param2">User defined parameter value, pertinent to the event.</param>
        /// <param name="result">User defined result obtained by a listener processing the event.</param>
        /// <returns></returns>
        bool RaiseEvent(in string eventId, in object param1, in object param2, ref object result);

        /// <summary>
        /// Raises an event that will be broadcast to all listeners.  This method will send the message within a separate 
        /// thread, this ensures there are no delays to the current thread whilst the message is being processed and should 
        /// be used when no response is required by the class raising the event.
        /// </summary>
        /// <param name="eventId">Name of the event being raised.</param>
        /// <param name="param1">User defined parameter value, pertinent to the event.</param>
        /// <param name="param2">User defined parameter value, pertinent to the event.</param>
        void RaiseEvent(in string eventId, in object param1, in object param2);

        /// <summary>
        /// Raises an event that will be broadcast to all listeners.  This method will send the message within a separate 
        /// thread, this ensures there are no delays to the current thread whilst the message is being processed and should 
        /// be used when no response is required by the class raising the event.
        /// </summary>
        /// <param name="eventId">Name of the event being raised.</param>
        /// <param name="param1">User defined parameter value, pertinent to the event.</param>
        void RaiseEvent(in string eventId, in object param1);

        /// <summary>
        /// Raises an event that will be broadcast to all listeners.  This method will send the message within a separate 
        /// thread, this ensures there are no delays to the current thread whilst the message is being processed and should 
        /// be used when no response is required by the class raising the event.
        /// </summary>
        /// <param name="eventId">Name of the event being raised.</param>
        void RaiseEvent(in string eventId);

        /// <summary>
        /// Registers an INotificationListener for receiving event notifications.
        /// </summary>
        /// <param name="listener">INotificationListener instance that is being registered.</param>
        /// <returns>bool</returns>
        bool RegisterListener(in INotificationListener listener);

        /// <summary>
        /// Unregisters an INotificationListener class from receiving event notifications.
        /// </summary>
        /// <param name="listener">INotificationListener instance being unregistered.</param>
        /// <returns>bool</returns>
        bool UnregisterListener(in INotificationListener listener);
    }
}
