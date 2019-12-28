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
}