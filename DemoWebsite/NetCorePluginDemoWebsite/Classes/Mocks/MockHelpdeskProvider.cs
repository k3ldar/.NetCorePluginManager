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
 *  Copyright (c) 2019 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Demo Website
 *  
 *  File: MockHelpdeskProvider.cs
 *
 *  Purpose:  Mock IHelpdeskProvider for tesing purpose
 *
 *  Date        Name                Reason
 *  13/04/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Middleware.Helpdesk;

namespace AspNetCore.PluginManager.DemoWebsite.Classes
{
    public class MockHelpdeskProvider : IHelpdeskProvider
    {
        private static List<Feedback> _feedback;

        public List<Feedback> GetFeedback(in bool publiclyVisible)
        {
            if (_feedback == null)
            {
                _feedback = new List<Feedback>()
                {
                    new Feedback(1, "Joe Bloggs", "Asp Net core is awesome", true),
                    new Feedback(2, "Jane Doe", "AspNetCore.PluginManager is extremely flexible", true),
                };
            }

            return _feedback;
        }

        public bool SubmitFeedback(in long userId, in string name, in string feedback)
        {
            List<Feedback> fb = GetFeedback(true);

            fb.Add(new Feedback(fb.Count + 1, name, feedback, true));

            return true;
        }
    }
}
