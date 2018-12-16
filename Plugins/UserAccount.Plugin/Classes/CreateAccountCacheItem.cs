using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserAccount.Plugin
{
    internal class CreateAccountCacheItem
    {
        #region Constructors

        public CreateAccountCacheItem()
        {
            FirstAttempt = DateTime.Now;
            CaptchaText = String.Empty;
        }

        #endregion Constructors

        #region Properties

        public DateTime FirstAttempt { get; private set; }

        public byte CreateAttempts { get; set; }

        public string CaptchaText { get; set; }

        #endregion Properties
    }
}
