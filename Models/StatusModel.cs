using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KJCFRubberRoller.Models
{
    //Account Status
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

    //User Role
    public class UserRole
    {
        public static int EXECUTIVE = 1;
        public static int MANAGER = 2;
        public static int QA_STAFF = 3;
        public static int ROLLER_PIC = 4;

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

    //Roller Category Critical Status
    public class RollerCat
    {
        public static int YES = 1;
        public static int NO = 2;
        public static int CONSIDERED_TO_BE_REMOVED_FRM_DB = 3;

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

    //Roller Status
    public class RollerStatus
    {
        public static int IN_USE = 1;
        public static int IN_STORE_ROOM = 2;
        public static int WAITING_FOR_QA_CHECK = 3;
        public static int WAITING_FOR_VERIFICATION = 4;
        public static int SENT_TO_MAINTENANCE = 5;

        public static string getStatus(int stats)
        {
            string status = "";
            switch (stats)
            {
                case 1:
                    status = "In use";
                    break;
                case 2:
                    status = "In store room";
                    break;
                case 3:
                    status = "Waiting for QA check";
                    break;
                case 4:
                    status = "Waiting for verification";
                    break;
                case 5:
                    status = "Sent to maintenance";
                    break;
                default:
                    status = "";
                    break;
            }
            return status;
        }
    }

    //Roller Maintenance Status
    public class RollerMaintenance
    {
        public static int PENDING_APPROVAL = 1;
        public static int APPROVED = 2;
        public static int REJECTED = 3;
        
        public static string getStatus(int stats)
        {
            string status = "";
            switch (stats)
            {
                case 1:
                    status = "Pending Approval";
                    break;
                case 2:
                    status = "Approved";
                    break;
                case 3:
                    status = "Rejected";
                    break;
                default:
                    status = "";
                    break;
            }
            return status;
        }
    }

    //Schedule Status
    public class ScheduleStatus
    {
        public static int ACTIVE = 1;
        public static int COMPLETED = 2;

        public static string getStatusString(int stats)
        {
            string status = "";
            switch (stats)
            {
                case 1:
                    status = "Active";
                    break;
                case 2:
                    status = "Completed";
                    break;
                default:
                    status = "";
                    break;
            }
            return status;
        }
    }
}