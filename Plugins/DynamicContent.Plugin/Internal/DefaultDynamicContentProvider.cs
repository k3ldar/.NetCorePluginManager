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
 *  Product:  DynamicContent.Plugin
 *  
 *  File: DefaultDynamicContentServices.cs
 *
 *  Purpose:  Default dynamic content services
 *
 *  Date        Name                Reason
 *  13/08/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

using Middleware;
using Middleware.DynamicContent;

using PluginManager.Abstractions;

using SharedPluginFeatures.DynamicContent;

namespace DynamicContent.Plugin.Internal
{
    public class DefaultDynamicContentProvider : IDynamicContentProvider
    {
        #region Private Members

        private readonly IPluginClassesService _pluginClassesService;

        #endregion Private Members

        #region Costructors

        public DefaultDynamicContentProvider(IPluginClassesService pluginClassesService)
        {
            _pluginClassesService = pluginClassesService ?? throw new ArgumentNullException(nameof(pluginClassesService));
        }

        #endregion Constructors

        #region IDynamicContentProvider Methods

        public String RenderDynamicPage(DynamicContentTemplate contentTemplate)
        {
            throw new NotImplementedException();
        }

        public List<LookupListItem> GetCustomPageList()
        {
            throw new NotImplementedException();
        }

        public IDynamicContentPage GetCustomPage(int id)
        {
            throw new NotImplementedException();
        }

        public List<DynamicContentTemplate> Templates()
        {
            throw new NotImplementedException();
        }

        public List<IDynamicContentPage> GetCustomPages()
        {
            throw new NotImplementedException();
        }

        public bool PageNameExists(int id, string pageName)
        {
            throw new NotImplementedException();
        }

        public bool RouteNameExists(int id, string routeName)
        {
            throw new NotImplementedException();
        }

        public bool Save(IDynamicContentPage dynamicContentPage)
        {
            throw new NotImplementedException();
        }


        #endregion IDynamicContentProvider Methods

        #region Private Methods

        //private string ProcessCreateWebContent(IDynamicContentProvider dynamicContentProvider, WebControl webControl,
        //    out ushort errors)
        //{
        //errors = 0;
        //StringBuilder Result = new StringBuilder(2048);
        //Result.AppendFormat("<div class=\"col-sm-{0} col-md-{1} col-lg-{2}\">",
        //    webControl.WidthSmall, webControl.WidthMedium, webControl.WidthLarge);
        //Result.Append(webControl.Content);

        //List<WebContentProperty> properties = ProcessGetProperties(webControl);

        //foreach (WebContentProperty property in properties)
        //{
        //    WebContent webContent = dynamicContentProvider.GetWebContent(property.ContentName);

        //    if (webContent == null)
        //    {
        //        errors++;
        //        continue;
        //    }

        //    switch (webContent.ContentType)
        //    {
        //        case DynamicContentType.Image:
        //        case DynamicContentType.Text:
        //        case DynamicContentType.Url:
        //            Result = Result.Replace($"[*{property.Name}={property.ContentName}*]", webContent.Data);
        //            break;

        //        case DynamicContentType.Class:
        //            Result = Result.Replace($"[*{property.Name}={property.ContentName}*]",
        //                GetClassDynamicContent(webContent.Data, ref errors));
        //            break;

        //        default:
        //            throw new NotImplementedException();
        //    }
        //}

        //Result.Append("</div>");

        //return Result.ToString();
        //}

        //private string GetClassDynamicContent(string className, ref ushort errors)
        //{
        //    List<Type> customContentProviders = _pluginClassesService.GetPluginClassTypes<ICustomDynamicContentProvider>();

        //    Type customProviderType = customContentProviders.Where(t => t.Name.Equals(className)).FirstOrDefault();

        //    if (customProviderType == null)
        //    {
        //        errors++;
        //        return String.Empty;
        //    }

        //    object[] paramInstances = _pluginClassesService.GetPluginClassParameters(customProviderType);

        //    ICustomDynamicContentProvider customProvider = Activator.CreateInstance(customProviderType, paramInstances) as ICustomDynamicContentProvider;

        //    if (customProvider == null)
        //    {
        //        errors++;
        //        return String.Empty;
        //    }

        //    return customProvider.GenerateCustomData();
        //}

        //private List<WebContentProperty> ProcessGetProperties(in WebControl webControl)
        //{
        //    List<WebContentProperty> Result = new List<WebContentProperty>();

        //    string data = webControl.Content;
        //    StringBuilder propertyData = new StringBuilder(100);
        //    bool inProperty = false;
        //    char lastChar = '\0';
        //    int startPos = -1;

        //    for (int i = 0; i < data.Length; i++)
        //    {
        //        char currentChar = data[i];
        //        bool canPeek = i < data.Length;

        //        if (currentChar == '*' && !inProperty && lastChar == '[')
        //        {
        //            inProperty = true;
        //            lastChar = currentChar;
        //            propertyData.Clear();
        //            startPos = i - 1;
        //            continue;
        //        }

        //        if (inProperty && currentChar == ']' && lastChar == '*')
        //        {
        //            inProperty = false;
        //            lastChar = currentChar;
        //            string[] parts = propertyData.ToString().Split('=');
        //            Result.Add(new WebContentProperty(parts[0], parts[1], startPos, i));
        //            continue;
        //        }

        //        if (inProperty)
        //        {
        //            if (currentChar == '*' && canPeek && data[i + 1] == ']')
        //            {
        //                lastChar = currentChar;
        //                continue;
        //            }

        //            propertyData.Append(currentChar);
        //        }


        //        lastChar = currentChar;
        //    }


        //    return Result;
        //}

        //private bool ProcessValidateWebControl(in WebControl webControl, ref List<string> errors, ref int propertCount)
        //{
        //    string data = webControl.Content;
        //    StringBuilder propertyData = new StringBuilder(100);
        //    Dictionary<string, string> duplicatePropertyNames = new Dictionary<string, string>();
        //    bool inProperty = false;
        //    bool isOpen = false;
        //    char lastChar = '\0';

        //    for (int i = 0; i < data.Length; i++)
        //    {
        //        char currentChar = data[i];
        //        bool canPeek = i < data.Length;

        //        if (currentChar == '*' && !inProperty && lastChar == '[')
        //        {
        //            if (isOpen)
        //                errors.Add(LanguageStrings.WebControlOpenProperty);

        //            inProperty = true;
        //            isOpen = true;
        //            lastChar = currentChar;
        //            propertyData.Clear();
        //            continue;
        //        }

        //        if (currentChar == '*' && inProperty && lastChar == '[')
        //        {
        //            if (isOpen)
        //                errors.Add(LanguageStrings.WebControlOpenProperty);

        //            inProperty = true;
        //            isOpen = true;
        //            lastChar = currentChar;
        //            propertyData.Clear();
        //            continue;
        //        }

        //        if (inProperty && currentChar == ']' && lastChar == '*')
        //        {
        //            propertCount++;
        //            isOpen = false;
        //            inProperty = false;
        //            lastChar = currentChar;

        //            if (!propertyData.ToString().Contains('='))
        //            {
        //                errors.Add(LanguageStrings.WebControlNoSeperator);
        //                continue;
        //            }

        //            string[] parts = propertyData.ToString().Split('=');

        //            if (parts.Length > 2)
        //            {
        //                errors.Add(LanguageStrings.WebControlTooManySeparators);
        //                continue;
        //            }

        //            if (parts[0].Trim().Length == 0)
        //            {
        //                errors.Add(LanguageStrings.WebControlPropertyEmpty);
        //                continue;
        //            }

        //            if (parts[0].Trim().Length > 30)
        //                errors.Add(LanguageStrings.WebControlPropertyNameTooLong);

        //            if (parts[0].Length != parts[0].Trim().Length)
        //                errors.Add(LanguageStrings.WebControlRequiresTrim);

        //            if (InvalidCharacters(parts[0], false))
        //                errors.Add(LanguageStrings.WebControlPropertyInvalidCharacters);

        //            if (duplicatePropertyNames.ContainsKey(parts[0].Trim()))
        //                errors.Add(LanguageStrings.WebControlPropertyNameDuplicates);
        //            else
        //                duplicatePropertyNames.Add(parts[0], String.Empty);

        //            if (parts[1].Length != parts[1].Trim().Length)
        //                errors.Add(LanguageStrings.WebControlPropertyValueHasSpaces);

        //            if (parts[1].Trim().Length == 0)
        //                errors.Add(LanguageStrings.WebControlPropertyValueNotFound);

        //            continue;
        //        }

        //        if (inProperty)
        //        {
        //            if (currentChar == '*' && canPeek && data[i + 1] == ']')
        //            {
        //                lastChar = currentChar;
        //                continue;
        //            }

        //            propertyData.Append(currentChar);
        //        }


        //        lastChar = currentChar;
        //    }

        //    if (isOpen)
        //        errors.Add(LanguageStrings.WebControlOpenProperty);

        //    return errors.Count > 0;
        //}

        //private bool ProcessValidateWebControlTemplate(in WebControlTemplate webControlTemplate, ref List<string> errors, ref int propertCount)
        //{
        //    string data = webControlTemplate.Content;
        //    StringBuilder propertyData = new StringBuilder(100);
        //    Dictionary<string, string> duplicatePropertyNames = new Dictionary<string, string>();
        //    bool inProperty = false;
        //    bool isOpen = false;
        //    char lastChar = '\0';

        //    for (int i = 0; i < data.Length; i++)
        //    {
        //        char currentChar = data[i];
        //        bool canPeek = i < data.Length;

        //        if (currentChar == '*' && !inProperty && lastChar == '[')
        //        {
        //            if (isOpen)
        //                errors.Add(LanguageStrings.WebControlOpenProperty);

        //            inProperty = true;
        //            isOpen = true;
        //            lastChar = currentChar;
        //            propertyData.Clear();
        //            continue;
        //        }

        //        if (currentChar == '*' && inProperty && lastChar == '[')
        //        {
        //            if (isOpen)
        //                errors.Add(LanguageStrings.WebControlOpenProperty);

        //            inProperty = true;
        //            isOpen = true;
        //            lastChar = currentChar;
        //            propertyData.Clear();
        //            continue;
        //        }

        //        if (inProperty && currentChar == ']' && lastChar == '*')
        //        {
        //            propertCount++;
        //            isOpen = false;
        //            inProperty = false;
        //            lastChar = currentChar;

        //            if (!propertyData.ToString().Contains('='))
        //            {
        //                errors.Add(LanguageStrings.WebControlNoSeperator);
        //                continue;
        //            }

        //            string[] parts = propertyData.ToString().Split('=');

        //            if (parts.Length > 2)
        //            {
        //                errors.Add(LanguageStrings.WebControlTooManySeparators);
        //                continue;
        //            }

        //            if (parts[0].Trim().Length == 0)
        //            {
        //                errors.Add(LanguageStrings.WebControlPropertyEmpty);
        //                continue;
        //            }

        //            if (parts[0].Trim().Length > 30)
        //                errors.Add(LanguageStrings.WebControlPropertyNameTooLong);

        //            if (parts[0].Length != parts[0].Trim().Length)
        //                errors.Add(LanguageStrings.WebControlRequiresTrim);

        //            if (InvalidCharacters(parts[0], false))
        //                errors.Add(LanguageStrings.WebControlPropertyInvalidCharacters);

        //            if (duplicatePropertyNames.ContainsKey(parts[0].Trim()))
        //                errors.Add(LanguageStrings.WebControlPropertyNameDuplicates);
        //            else
        //                duplicatePropertyNames.Add(parts[0], String.Empty);

        //            if (parts[1].Length != parts[1].Trim().Length)
        //                errors.Add(LanguageStrings.WebControlPropertyValueHasSpaces);

        //            if (parts[1].Trim().Length != 0)
        //                errors.Add(LanguageStrings.WebControlTemplatePropertyValueHasValue);

        //            continue;
        //        }

        //        if (inProperty)
        //        {
        //            if (currentChar == '*' && canPeek && data[i + 1] == ']')
        //            {
        //                lastChar = currentChar;
        //                continue;
        //            }

        //            propertyData.Append(currentChar);
        //        }


        //        lastChar = currentChar;
        //    }

        //    if (isOpen)
        //        errors.Add(LanguageStrings.WebControlOpenProperty);

        //    return errors.Count > 0;
        //}

        //private bool ProcessValidateWebContent(in WebContent webContent, ref List<string> errors)
        //{
        //    if (InvalidCharacters(webContent.Name, true))
        //        errors.Add(LanguageStrings.WebContentInvalidName);

        //    if (webContent.Name.Length != webContent.Name.Trim().Length)
        //        errors.Add(LanguageStrings.WebContentInvalidLeadingTrailingSpaces);

        //    return errors.Count > 0;
        //}

        //private bool InvalidCharacters(in string value, in bool allowSpaces)
        //{
        //    foreach (char c in value)
        //    {
        //        bool isInvalidChar = !((c >= 65 && c <= 90) ||
        //            (c >= 61 && c <= 122) ||
        //            (c >= 48 && c <= 57) ||
        //            (c == 45) ||
        //            (allowSpaces && c == 32));

        //        if (isInvalidChar)
        //            return true;
        //    }

        //    return false;
        //}

        #endregion Private Methods
    }
}
