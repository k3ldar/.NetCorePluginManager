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
 *  Copyright (c) 2018 - 2019 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Error Manager Plugin
 *  
 *  File: ErrorThreadManager.cs
 *
 *  Purpose:  Notifies host app of error's raised
 *
 *  Date        Name                Reason
 *  17/11/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

using Shared.Classes;

using SharedPluginFeatures;

namespace ErrorManager.Plugin
{
    public sealed class ErrorThreadManager : ThreadManager
    {
        #region Static Members

        private readonly IErrorManager _errorManager;
        private static readonly object _lockObject = new object();
        private static readonly List<ErrorInformation> _errorList = new List<ErrorInformation>();
        private static readonly List<ErrorInformation> _processList = new List<ErrorInformation>();

        #endregion Static Members

        #region Constructors

        public ErrorThreadManager(IErrorManager errorManager)
            : base (null, new TimeSpan(0, 0, 10))
        {
            _errorManager = errorManager ?? throw new ArgumentNullException(nameof(errorManager));
        }

        #endregion Constructors

        #region Overridden Methods

        protected override bool Run(object parameters)
        {
            using (TimedLock lck = TimedLock.Lock(_lockObject))
            {
                foreach (ErrorInformation error in _errorList)
                {
                    _processList.Add(error);
                }

                _errorList.Clear();
            }
            try
            {
                // process the list of errors
                for (int i = _processList.Count -1; i >= 0; i--)
                {
                    ErrorInformation errorInformation = _processList[i];

                    _errorManager.ErrorRaised(errorInformation);

                    _processList.RemoveAt(i);
                }
            }
            finally
            {
                // if there are any items left in the list, add them back to the list
                // for processing next time
                using (TimedLock lck = TimedLock.Lock(_lockObject))
                {
                    foreach (ErrorInformation error in _processList)
                        _errorList.Add(error);
                }

                _processList.Clear();
            }

            return !HasCancelled();
        }

        #endregion Overridden Methods

        #region Internal Methods

        internal void AddError(ErrorInformation errorInformation)
        {
            using (TimedLock lck = TimedLock.Lock(_lockObject))
            {
                _errorList.Add(errorInformation);
            }
        }

        #endregion Internal Methods
    }
}
