using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KJCFRubberRoller.Models
{
    public class AccountStatus
    {
        public static int ACTIVE = 1;
        public static int INACTIVE = 0;

        public static string getStatus(int status)
        {
            string stat = "";
            switch (status)
            {
                case 0:
                    stat = "Inactive";
                    break;
                case 1:
                    stat = "Active";
                    break;
                default:
                    stat = "";
                    break;
            }
            return stat;
        }
    }

    public class UserRole
    {
        public static string getRole(int position)
        {
            string role = "";
            switch (position)
            {
                case 1:
                    role = "Executive";
                    break;
                case 2:
                    role = "Manager";
                    break;
                case 3:
                    role = "QA Staff";
                    break;
                case 4:
                    role = "Roller PIC";
                    break;
                default:
                    role = "";
                    break;
            }
            return role;
        }
    }

    public class RollerCat
    {
        public static string getCriticalStatus(int critStats)
        {
            string status = "";
            switch (critStats)
            {
                case 1:
                    status = "Yes";
                    break;
                case 2:
                    status = "No";
                    break;
                case 3:
                    status = "To considered to be removed from database";
                    break;
                default:
                    status = "";
                    break;
            }
            return status;
        }
    }
}