using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SharedPluginFeatures;

namespace SystemAdmin.Plugin.Models
{
    public sealed class GridViewModel
    {
        #region Constructors

        public GridViewModel(SystemAdminSubMenu subMenu)
        {
            if (subMenu == null)
                throw new ArgumentNullException(nameof(subMenu));

            Title = subMenu.Name();

            // data has to have the header on first row, each column seperated by a pipe |
            // the data is on all following lines and is also seperated by pipe |
            // each line is seperated with \r
            string[] allLines = subMenu.Data().Split('\r');

            // must have a header at the very least!
            if (allLines.Length == 0)
                throw new ArgumentNullException(nameof(subMenu.Data));

            Headers = allLines[0].Split('|');

            int columnCount = Headers.Length;

            Items = new List<string[]>();

            for (int i = 1; i < allLines.Length; i++)
            {
                string[] line = allLines[i].Split('|');

                if (line.Length != Headers.Length)
                    throw new InvalidOperationException("column count much match header column count");

                Items.Add(line);
            }

            BreadCrumb = $"<ul><li><a href=\"/SystemAdmin/\">System Admin</a></li><li><a href=\"/SystemAdmin/Index/" +
                $"{subMenu.ParentMenu.UniqueId}\">{subMenu.ParentMenu.Name()}</a></li><li>{Title}</li></ul>";
        }

        #endregion Constructors

        #region Public Properties

        public string[] Headers { get; set; }

        public List<string[]> Items { get; set; }

        public string Title { get; set; }

        public string BreadCrumb { get; set; }

        #endregion Public Properties
    }
}
