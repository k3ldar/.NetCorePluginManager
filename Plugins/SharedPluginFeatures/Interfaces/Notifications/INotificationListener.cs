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
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Collections.Generic;

namespace SharedPluginFeatures
{
    public interface INotificationListener
    {
        /// <summary>
        /// This method is used to notify registered listeners that an event has been raised and is generally used in order 
        /// to obtain a response.  If called then the active thread is blocked whilst processing is completed and can be 
        /// used by callers to ask a question which needs a response.
        /// </summary>
        /// <param name="eventId">Name of the event being raised.</param>
        /// <param name="param1">User defined parameter value, pertinent to the event.</param>
        /// <param name="param2">User defined parameter value, pertinent to the event.</param>
        /// <param name="result">User defined result to be passed back to the class that raised the event.</param>
        /// <returns>bool.  If a listener retures true, then the answer is deemed to be received and no other listeners will 
        /// be called and the result will be passed straight back to the class where the event was raised.  If false, the 
        /// next listener in the chain will be called.</returns>
        bool EventRaised(in string eventId, in object param1, in object param2, ref object result);

        /// <summary>
        /// This method is used to notify registered listeners that an event has been raised and is generally used when 
        /// no response is required.  If called then the active thread is not blocked whilst processing is completed.
        /// </summary>
        /// <param name="eventId">Name of the event being raised.</param>
        /// <param name="param1">User defined parameter value, pertinent to the event.</param>
        /// <param name="param2">User defined parameter value, pertinent to the event.</param>
        void EventRaised(in string eventId, in object param1, in object param2);

        /// <summary>
        /// This method is called after a call to RegisterListener, if the function returns null, an empty list or a list 
        /// which contains an empty or null string then an Invalid Operation exception will be raised.  
        /// 
        /// An instance of this interface can register multiple events.
        /// </summary>
        /// <returns>List&lt;string&gt;.  List of events which the listener is interested in receiving.</returns>
        List<string> GetEvents();
    }
}
