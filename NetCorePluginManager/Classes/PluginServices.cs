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
 *  Copyright (c) 2018 Simon Carter.  All Rights Reserved.
 *
 *  Product:  AspNetCore.PluginManager
 *  
 *  File: PluginServices.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  14/10/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

using SharedPluginFeatures;

using Shared.Classes;

namespace AspNetCore.PluginManager
{
    public sealed class PluginServices : IPluginClassesService, IPluginHelperService, IPluginTypesService,
        IPluginNotificationManager
    {
        #region Private Members

        private List<IPluginNotification> _pluginNotifications;
        private object _lockObject;

        #endregion Private Members

        #region Constructors

        public PluginServices()
        {
            _lockObject = new object();
            _pluginNotifications = new List<IPluginNotification>();
        }

        #endregion Constructors

        #region IPluginClassesService Methods

        public List<Type> GetPluginClassTypes<T>()
        {
            return (PluginManagerService._pluginManagerInstance.GetPluginClassTypes<T>());
        }

        public List<T> GetPluginClasses<T>()
        {
            return (PluginManagerService._pluginManagerInstance.GetPluginClasses<T>());
        }

        #endregion IPluginClassesService Methods

        #region IPluginTypesService Methods

        public List<Type> GetPluginTypesWithAttribute<T>()
        {
            return (PluginManagerService._pluginManagerInstance.GetPluginTypesWithAttribute<T>());
        }

        #endregion IPluginTypesService Methods

        #region IPluginHelperService Methods

        public bool PluginLoaded(in string pluginLibraryName, out int version)
        {
            return (PluginManagerService._pluginManagerInstance.PluginLoaded(pluginLibraryName, out version));
        }

        #endregion IPluginHelperService Methods

        #region IPluginNotificationManager Methods

        void IPluginNotificationManager.Add(in IPluginNotification notification)
        {
            if (notification == null)
                throw new ArgumentNullException(nameof(notification));

            using (TimedLock.Lock(_lockObject))
            {
                _pluginNotifications.Add(notification);
            }
        }

        void IPluginNotificationManager.Remove(in IPluginNotification notification)
        {
            if (notification == null)
                throw new ArgumentNullException(nameof(notification));

            using (TimedLock.Lock(_lockObject))
            {
                _pluginNotifications.Remove(notification);
            }
        }

        internal void PluginLoaded(in string plugin, in ushort version)
        {
            if (String.IsNullOrEmpty(plugin))
                throw new ArgumentNullException(nameof(plugin));

            using (TimedLock.Lock(_lockObject))
            {
                foreach (IPluginNotification notification in _pluginNotifications)
                {
                    notification.PluginLoaded(plugin, version);
                }
            }
        }

        internal void PlugnLoadFailed(in string plugin, in string error)
        {
            if (String.IsNullOrEmpty(plugin))
                throw new ArgumentNullException(nameof(plugin));

            if (String.IsNullOrEmpty(error))
                throw new ArgumentNullException(nameof(error));

            using (TimedLock.Lock(_lockObject))
            {
                foreach (IPluginNotification notification in _pluginNotifications)
                {
                    notification.PlugnLoadFailed(plugin, error);
                }
            }
        }

        internal void PluginLoadError(in string plugin, in string message)
        {
            if (String.IsNullOrEmpty(plugin))
                throw new ArgumentNullException(nameof(plugin));

            if (String.IsNullOrEmpty(message))
                throw new ArgumentNullException(nameof(message));

            using (TimedLock.Lock(_lockObject))
            {
                foreach (IPluginNotification notification in _pluginNotifications)
                {
                    notification.PluginLoadError(plugin, message);
                }
            }
        }

        #endregion IPluginNotificationManager Methods
    }
}
