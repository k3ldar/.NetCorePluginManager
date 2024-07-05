
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
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
 *
 *  Product:  SystemAdmin.Plugin
 *  
 *  File: SystemAdminController.Settings.cs
 *
 *  Purpose:  Manage settings for plugins
 *
 *  Date        Name                Reason
 *  18/12/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json;

using AppSettings;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

using SharedPluginFeatures;

using SystemAdmin.Plugin.Classes.MenuItems;
using SystemAdmin.Plugin.Models;



#pragma warning disable CS1591, IDE0008

namespace SystemAdmin.Plugin.Controllers
{
	public partial class SystemAdminController
	{
		#region Controller Action Methods

		[HttpGet]
		[Authorize(Policy = Constants.PolicyNameManageSystemSettings)]
		public IActionResult Settings(int id)
		{
			SystemAdminSubMenu subMenu = _systemAdminHelperService.GetSubMenuItem(id);

			if (subMenu == null)
				return Redirect(DefaultSysAdminRoute);

			SettingsMenuItem baseType;

			if ((baseType = subMenu as SettingsMenuItem) == null)
				return Redirect(DefaultSysAdminRoute);

			return View(CreateSettingsViewModel(baseType));
		}

		[HttpPost]
		[AjaxOnly]
		[Authorize(Policy = Constants.PolicyNameManageSystemSettings)]
		public IActionResult Settings([FromBody] SettingsViewModel model)
		{
			if (model == null)
				return RedirectToAction(nameof(Index));

			SystemAdminSubMenu subMenu = _systemAdminHelperService.GetSubMenuItem(model.SettingId);

			if (subMenu == null)
				return RedirectToAction(nameof(Index));

			SettingsMenuItem baseType;

			if ((baseType = subMenu as SettingsMenuItem) == null)
				return RedirectToAction(nameof(Index));

			List<string> errors = [];

			ValidateIncomingProperties(model, baseType, errors);

			if (errors.Count == 0)
			{
				SaveUpdatedSettingsToAppSettings(baseType);

				return new JsonResult(new JsonResponseModel(Languages.LanguageStrings.SettingsUpdated));
			}
			else
			{
				return new JsonResult(new JsonResponseModel(false, JsonSerializer.Serialize(errors)));
			}
		}

		#endregion Controller Action Methods

		#region Private Methods

		private void SaveUpdatedSettingsToAppSettings(SettingsMenuItem baseType)
		{
			string json = System.IO.File.ReadAllText(_pluginManagerConfiguration.ConfigurationFile);

			var jsonData = JsonSerializer.Deserialize<Dictionary<string, object>>(json);
			JsonSerializerOptions options = new()
			{
				WriteIndented = true,
				IgnoreReadOnlyProperties = true,
				Encoder = JavaScriptEncoder.Create(new TextEncoderSettings(System.Text.Unicode.UnicodeRanges.All))
			};

			object newSettings = baseType.PluginSettings;
			jsonData[baseType.PluginSettings.SettingsName] = newSettings;

			System.IO.File.WriteAllText(_pluginManagerConfiguration.ConfigurationFile, JsonSerializer.Serialize(jsonData, options));
		}

		private static void ValidateIncomingProperties(SettingsViewModel model, SettingsMenuItem baseType, List<string> errors)
		{
			PropertyInfo[] properties = baseType.PluginSettings.GetType().GetProperties();

			// validate property names match
			foreach (ApplicationSettingViewModel modelProperty in model.Settings)
			{
				PropertyInfo property = properties.FirstOrDefault(p => p.Name.Equals(modelProperty.Name) && p.CanWrite && p.CanRead);

				if (property == null)
				{
					errors.Add($"Property {modelProperty.Name} was not found");
				}
				else
				{
					ValidateAndSetPropertyValues(baseType, errors, modelProperty, property);
				}
			}

			try
			{
				ValidateSettings<SettingsViewModel>.ValidateAllSettings(baseType.PluginSettings.GetType(),
					baseType.PluginSettings, null, null, null);
			}
			catch (SettingException ex)
			{
				errors.Add(ex.Message);
			}
		}

		private static void ValidateAndSetPropertyValues(SettingsMenuItem baseType, List<string> errors,
			ApplicationSettingViewModel modelProperty, PropertyInfo property)
		{
			switch (modelProperty.DataType)
			{
				case "String[]":
					StringSplitOptions splitOptions = StringSplitOptions.RemoveEmptyEntries;

#if NET5_0_OR_GREATER
					splitOptions |= StringSplitOptions.TrimEntries;
#endif

					string[] values = modelProperty.Value.Split("\n", splitOptions);
					property.SetValue(baseType.PluginSettings, values, null);
					break;

				case "String":

					property.SetValue(baseType.PluginSettings, modelProperty.Value, null);

					break;

				case "Boolean":

					if (Boolean.TryParse(modelProperty.Value, out bool boolValue))
						property.SetValue(baseType.PluginSettings, boolValue, null);
					else
						errors.Add($"Property {modelProperty.Name} does not contain a valid Boolean value");

					break;

				case "Int32":

					if (Int32.TryParse(modelProperty.Value, out int intValue))
						property.SetValue(baseType.PluginSettings, intValue, null);
					else
						errors.Add($"Property {modelProperty.Name} does not contain a valid Int32 value");

					break;

				case "Int64":

					if (Int64.TryParse(modelProperty.Value, out long longValue))
						property.SetValue(baseType.PluginSettings, longValue, null);
					else
						errors.Add($"Property {modelProperty.Name} does not contain a valid Int64 value");

					break;

				case "UInt32":

					if (UInt32.TryParse(modelProperty.Value, out uint uintValue))
						property.SetValue(baseType.PluginSettings, uintValue, null);
					else
						errors.Add($"Property {modelProperty.Name} does not contain a valid UInt32 value");

					break;

				case "UInt64":

					if (UInt64.TryParse(modelProperty.Value, out ulong ulongValue))
						property.SetValue(baseType.PluginSettings, ulongValue, null);
					else
						errors.Add($"Property {modelProperty.Name} does not contain a valid UInt64 value");

					break;

				case "Decimal":

					if (Decimal.TryParse(modelProperty.Value, out decimal decimalValue))
						property.SetValue(baseType.PluginSettings, decimalValue, null);
					else
						errors.Add($"Property {modelProperty.Name} does not contain a valid Decimal value");

					break;

				default:

					var propValue = JsonSerializer.Deserialize(modelProperty.Value, property.PropertyType);
					property.SetValue(baseType.PluginSettings, propValue, null);

					break;
			}
		}

		private SettingsViewModel CreateSettingsViewModel(SettingsMenuItem baseType)
		{
			SettingsViewModel Result = new(GetModelData(), baseType.UniqueId, baseType.Name());

			string jsonFile = _pluginManagerConfiguration.ConfigurationFile;

			ConfigurationBuilder builder = new();
			IConfigurationBuilder configBuilder = builder.SetBasePath(Path.GetDirectoryName(jsonFile));
			configBuilder.AddJsonFile(jsonFile);
			IConfigurationRoot config = builder.Build();

			Type settingsType = baseType.PluginSettings.GetType();
			var test = Activator.CreateInstance(settingsType, _pluginClassesService.GetParameterInstances(settingsType));

			config.GetSection(baseType.Name()).Bind(test);

			ProcessIndividualProperties(baseType, Result, test);

			SystemAdminMainMenu selectedMenu = _systemAdminHelperService.GetSystemAdminMainMenu()
				.First(sam => sam.Name.Equals(Languages.LanguageStrings.Settings, StringComparison.InvariantCultureIgnoreCase));

			Result.Breadcrumbs.Add(new BreadcrumbItem(Languages.LanguageStrings.SystemAdmin, "/SystemAdmin/Index", false));
			Result.Breadcrumbs.Add(new BreadcrumbItem(Languages.LanguageStrings.Settings, $"/SystemAdmin/Index/{selectedMenu.UniqueId}/", false));
			Result.Breadcrumbs.Add(new BreadcrumbItem(Result.SettingsName, $"/SystemAdmin/Setting/{Result.SettingId}/", false));

			return Result;
		}

		private static void ProcessIndividualProperties(SettingsMenuItem baseType, SettingsViewModel Result, object test)
		{
			PropertyInfo[] allProperties = baseType.PluginSettings.GetType().GetProperties().Where(p => p.CanRead && p.CanWrite).ToArray();

			foreach (PropertyInfo property in allProperties)
			{
				ApplicationSettingViewModel appSetting = null;

				switch (property.PropertyType.Name)
				{
					case "Decimal":
					case "String":
					case "Int32":
					case "Int64":
					case "UInt32":
					case "UInt64":
					case "Boolean":

						string value = property.GetValue(test, null)?.ToString() ?? String.Empty;

						CustomAttributeData defaultAttr = property.CustomAttributes
							.FirstOrDefault(ca => ca.AttributeType.FullName.Equals(typeof(SettingDefaultAttribute).FullName));

						if (defaultAttr != null)
						{
							bool isDefault = (property.PropertyType.FullName == "System.String" &&
								String.IsNullOrEmpty((string)property.GetValue(baseType.PluginSettings))) ||
								property.GetValue(baseType.PluginSettings) == null ||
								property.GetValue(baseType.PluginSettings).Equals(GetDefault(property.PropertyType));

							if (isDefault)
							{
								value = defaultAttr.ConstructorArguments[0].Value.ToString();
							}
						}

						appSetting = CreatePropertySetting(value, property);

						break;

					case "String[]":

						string[] valueArray = (string[])(property.GetValue(test, null));
						appSetting = CreatePropertySetting(String.Join('\t', valueArray), property);

						break;

					default:

						string json = JsonSerializer.Serialize(property.GetValue(test, null));
						appSetting = CreatePropertySetting(json, property);
						appSetting.DataType = "Json";

						break;
				}

				if (appSetting != null)
					Result.Settings.Add(appSetting);
			}
		}

		private static object GetDefault(in Type type)
		{
			if (type == null || !type.IsValueType)
				return null;

			return Activator.CreateInstance(type);
		}

		private static ApplicationSettingViewModel CreatePropertySetting(string value, PropertyInfo property)
		{
			ApplicationSettingViewModel setting = new();
			setting.Name = property.Name;
			setting.Value = value;
			setting.DataType = property.PropertyType.Name;
			return setting;
		}

		#endregion Private Methods
	}
}

#pragma warning restore CS1591, IDE0008
