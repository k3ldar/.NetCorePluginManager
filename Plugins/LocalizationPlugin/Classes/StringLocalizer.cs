using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Resources;

using Microsoft.Extensions.Localization;

using Languages;

namespace Localization.Plugin
{
    public class StringLocalizer : IStringLocalizer
    {
        #region Private Members

        private static readonly ResourceManager _resourceManager = new ResourceManager("Languages.LanguageStrings", 
            typeof(LanguageStrings).Assembly);

        private readonly CultureInfo _resourceCulture;

        #endregion Private Members

        #region Constructors

        public StringLocalizer(string culture)
            : this (new CultureInfo(culture))
        {

        }

        public StringLocalizer(CultureInfo culture)
        {
            _resourceCulture = culture ?? throw new ArgumentNullException(nameof(culture));
        }

        #endregion Constructors

        #region IStringLocalizer Methods

        public LocalizedString this[string name]
        {
            get
            {
                try
                {
                    return new LocalizedString(name, _resourceManager.GetString(name, _resourceCulture));
                }
                catch
                {
                    return new LocalizedString(name, name);
                }
            }
        }

        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                try
                {
                    string resourceString = _resourceManager.GetString(name, _resourceCulture);
                    return new LocalizedString(name, String.Format(resourceString, arguments));
                }
                catch
                {
                    return new LocalizedString(name, name);
                }
            }
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            throw new NotImplementedException();
        }

        public IStringLocalizer WithCulture(CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion IStringLocalizer Methods
    }
}
