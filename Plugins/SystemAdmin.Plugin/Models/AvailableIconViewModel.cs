using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SharedPluginFeatures;

namespace SystemAdmin.Plugin.Models
{
    public class AvailableIconViewModel
    {
        #region Constructors

        public AvailableIconViewModel()
        {
            BreadCrumb = String.Empty;
        }

        public AvailableIconViewModel(in List<SystemAdminMainMenu> homeMenuItems)
            : this ()
        {
            HomeIcons = homeMenuItems ?? throw new ArgumentNullException(nameof(homeMenuItems));
        }

        public AvailableIconViewModel(in SystemAdminMainMenu mainMenu)
        {
            if (mainMenu == null)
                throw new ArgumentNullException(nameof(mainMenu));

            Title = mainMenu.Name();
            MenuItems = mainMenu.ChildMenuItems ?? throw new ArgumentNullException(nameof(mainMenu.ChildMenuItems));

            BreadCrumb = $"<ul><li><a href=\"/SystemAdmin/\">System Admin</a></li>&nbsp;&gt;&nbsp;<li>{Title}</li></ul>";
        }

        #endregion Constructors

        #region Public Methods

        public AvailableIconViewModel ClearBreadCrumb()
        {
            BreadCrumb = String.Empty;
            return (this);
        }

        public string ProcessImage(in string imageName)
        {
            if (String.IsNullOrEmpty(imageName))
                return ("/images/setting-icon.png");

            return (imageName);
        }

        public string GetMenuLink(in SystemAdminSubMenu menu)
        {
            if (menu == null)
                throw new ArgumentNullException(nameof(menu));

            switch (menu.MenuType())
            {
                case Enums.SystemAdminMenuType.Grid:
                    return ($"/SystemAdmin/Grid/{menu.UniqueId}");
                case Enums.SystemAdminMenuType.Text:
                    return ($"/SystemAdmin/Text/{menu.UniqueId}");
                case Enums.SystemAdminMenuType.PartialView:
                    return ($"/SystemAdmin/View/{menu.UniqueId}");
            }

            throw new InvalidOperationException();
        }

        #endregion Public Methods

        #region Properties

        public string Title { get; set; }

        public List<SystemAdminSubMenu> MenuItems { get; set; }

        public List<SystemAdminMainMenu> HomeIcons { get; set; }

        public string BreadCrumb { get; private set; }

        #endregion Properties
    }
}
