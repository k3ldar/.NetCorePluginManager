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
 *  Product:  SystemAdmin.Plugin
 *  
 *  File: GCSnapshot.cs
 *
 *  Purpose:  Garbage collection snapshot
 *
 *  Date        Name                Reason
 *  07/11/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

namespace SystemAdmin.Plugin.Classes
{
    internal sealed class GCSnapshot
    {
        internal GCSnapshot(double timeTaken, long memorySaved)
        {
            TimeStarted = DateTime.UtcNow;
            TimeTaken = timeTaken;

            if (memorySaved < 0)
                memorySaved = memorySaved * -1;

            MemorySaved = memorySaved;
        }

        internal DateTime TimeStarted { get; private set; }

        internal double TimeTaken { get; private set; }

        internal long MemorySaved { get; private set; }
    }
}
