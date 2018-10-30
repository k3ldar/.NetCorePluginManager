using System;
using System.Collections.Generic;
using System.Text;

using SharedPluginFeatures;

using Shared.Classes;

namespace AspNetCore.PluginManager.Classes.SystemAdmin
{
    public class ThreadMenu : SystemAdminSubMenu
    {
        public override string Action()
        {
            return (String.Empty);
        }

        public override string Area()
        {
            return (String.Empty);
        }

        public override string Controller()
        {
            return (String.Empty);
        }

        public override Enums.SystemAdminMenuType MenuType()
        {
            return (Enums.SystemAdminMenuType.Grid);
        }

        public override string Data()
        {
            StringBuilder Result = new StringBuilder("Name|Process Usage|System Usage|Thread IdD|Cancelled|Unresponsive|Marked For Removal\r");

            for (int i = 0; i < ThreadManager.ThreadCount; i++)
            {
                ThreadManager thread = ThreadManager.Get(i);

                string threadData = String.Format("{0}\r\n", thread.ToString());
                string[] parts = threadData.Split(';');

                Result.Append(SplitText(parts[1], ':') + "|");
                string cpu = SplitText(parts[0], ':');
                Result.Append(cpu.Substring(0, cpu.IndexOf("/")) + "|");
                Result.Append(cpu.Substring(cpu.IndexOf("/") + 1) + "|");
                Result.Append(SplitText(parts[2], ':') + "|");
                Result.Append(SplitText(parts[3], ':') + "|");
                Result.Append(SplitText(parts[4], ':') + "|");
                Result.Append(SplitText(parts[5], ':') + "\r");
            }

            return (Result.ToString().Trim());
        }

        public override string Name()
        {
            return ("Threads");
        }

        public override string ParentMenuName()
        {
            return ("System");
        }

        public override int SortOrder()
        {
            return (0);
        }

        public override string Image()
        {
            return (String.Empty);
        }

        private string SplitText(string text, char splitText)
        {
            if (text.Contains(splitText.ToString()))
            {
                string result = text.Substring(text.IndexOf(splitText) + 1);
                return (result.Trim());
            }
            else
                return (text);
        }
    }
}
