using CMMSAPIs.BS.Mails;
using CMMSAPIs.Helper;
using CMMSAPIs.Models.Calibration;
using CMMSAPIs.Models.Grievance;
using CMMSAPIs.Models.Incident_Reports;
using CMMSAPIs.Models.Inventory;
using CMMSAPIs.Models.JC;
using CMMSAPIs.Models.Jobs;
using CMMSAPIs.Models.Mails;
using CMMSAPIs.Models.MC;
using CMMSAPIs.Models.Permits;
using CMMSAPIs.Models.Users;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Models.WC;
using CMMSAPIs.Repositories.Users;
using CMMSAPIs.Repositories.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

//using CommonUtilities;
//using CMMSAPIs.Models.Notifications;

namespace CMMSAPIs.Models.Notifications
{

    abstract public class CMMSNotification
    {
        public static bool print = false;
        public static string printBody = "";

        private CMMS.CMMS_Modules m_moduleID;
        protected CMMS.CMMS_Status m_notificationID;
        protected int m_notificationType = 1;
        protected int m_delayDays = 0;
        protected int module_ref_id = 0;
        protected int m_role = 0;


        public string template = "<tr><td style=' text-align: left; padding:0.5rem; background-color:#31576D;color:#ffffff;width:35%' ><b>&nbsp;&nbsp;{0}</b></td><td style='text-align: left; padding:0.5rem;'>&nbsp;&nbsp;{1}</td></tr>";

        public string templateEnd = "<tr><td style=' text-align: left; padding:0.5rem; background-color:#31576D;color:#ffffff'><b>&nbsp;&nbsp;{0}</b></td><td style='text-align: left; padding:0.5rem;'>&nbsp;&nbsp;{1}</td></tr></table>";

        private static IConfigurationRoot MyConfig;
        private static string _conString;
        public static MYSQLDBHelper _conn;


        private UserAccessRepository _userAccessRepository;
        public CMMSNotification(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID)
        {
            m_moduleID = moduleID;
            m_notificationID = notificationID;

            MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            _conString = MyConfig.GetValue<string>("ConnectionStrings:Con");
            _conn = new MYSQLDBHelper(_conString);

            IWebHostEnvironment webHostEnvironment = null;
            IConfiguration configuration = null;
            _userAccessRepository = new UserAccessRepository(_conn, webHostEnvironment, configuration);
        }


        protected virtual int getId(params object[] args)
        {
            return 0;
        }

        protected virtual string getModuleName(params object[] args)
        {
            return Convert.ToString(m_moduleID);
        }
        protected virtual string getNotificationName(params object[] args)
        {
            //Console.WriteLine(Days.TuesDay);

            return Convert.ToString(m_notificationID);
        }
        protected virtual string getEMSubject(params object[] args)
        {
            //default implementation of subject
            return String.Format("subject : My module {0} Event {1}", m_moduleID, m_notificationID);
        }

        //This method needs to be implemented for each 
        protected string getEMHTMLBody(params object[] args)
        {
            return String.Format("Implement EM HTML Body for module {}", m_moduleID);
        }

        protected virtual string getSubject(params object[] args)
        {
            //default implementation of subject
            return String.Format("subject : My module {0} Event {1}", m_moduleID, m_notificationID);
        }

        //This method needs to be implemented for each 
        protected abstract string getHTMLBody(params object[] args);

        protected virtual string getHTMLHeader(params object[] args)
        {
            string retValue = "This is header. Display company name etc or topic name";
            return retValue;
        }

        protected virtual string getHTMLFooter(params object[] args)
        {
            string retValue = "This is footer href twitter, fb, etc";
            return retValue;
        }

        protected virtual string getHTMLSignature(params object[] args)
        {
            //int userID = (int)args[0];
            //get user details from userID
            string Name = "Dummy Name";
            string designation = "Manager";
            string email = "Manager@company.com";
            string phone = "+91 12345 67890";
            string retValue = "This is footer href twitter, fb, etc";
            return retValue;
        }


        protected async Task<CMDefaultResponse> sendEmail(string subject, string HTMLBody, string HTMLHeader, string HTMLFooter, string HTMLSignature, List<string> emailTo, params object[] args)
        {


            //CommonUtilities.Mail.MailManager mailObj = new CommonUtilities.Mail.MailManager();

            //MAILING FUNCTIONALITY
            CMMailSettings _settings = new CMMailSettings();
            var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            _settings.Mail = MyConfig.GetValue<string>("MailSettings:Mail");
            _settings.DisplayName = MyConfig.GetValue<string>("MailSettings:DisplayName");
            _settings.Password = MyConfig.GetValue<string>("MailSettings:Password");
            _settings.Host = MyConfig.GetValue<string>("MailSettings:Host");
            _settings.Port = MyConfig.GetValue<int>("MailSettings:Port");

            //List<CMUser> emailList = GetUserByNotificationId(23);
            List<string> AddTo = emailTo;
            List<string> AddCc = new List<string>();

            DateTime today = DateTime.Now;

            string Body = "<div style='width:100%; padding:0.5rem; text-align:center;'><span><img src='https://i.ibb.co/FD60YSY/hfe.png' alt='hfe' border='0'></span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b style='color:#31576D; padding:0.5rem;'><i>" + today.ToString("dddd") + " , " + today.ToString("dd-MMM-yyyy") + "</i></b></div><hr><br><div style='text-align:center;'>";

            Body += HTMLBody;

            Body += "</div><br><div><p style='text-align:center;'>visit:<a href='https://i.ibb.co/FD60YSY/hfe.png'> http://cmms_726897com</a></p></div><br><p style='padding:0.5rem; '><b>Disclaimer:</b> The information contained in this electronic message and any attachments to this message are intended for the exclusive use of the addressee(s) and may contain proprietary, confidential or privileged information. If you are not the intended recipient, you should not disseminate, distribute, print or copy this e-mail. Please notify the sender immediately and destroy all copies of this message and any attachments. Although them company has taken reasonable precautions to ensure no viruses are present in this email, the company cannot accept responsibility for any loss or damage arising from the use of this email or attachments.</p>";

            CMMailRequest request = new CMMailRequest();

            //AddTo.Add("tanvi@softeltech.in");
            request.ToEmail = AddTo;
            request.CcEmail = AddCc;
            request.Subject = subject;
            request.Body = Body;
            request.Headers = HTMLHeader;

            try
            {
                var res = MailService.SendEmailAsync(request, _settings);
            }
            catch (Exception e)
            {
                string msg = e.Message;
            }
            CMDefaultResponse retValue = new CMDefaultResponse(1, CMMS.RETRUNSTATUS.SUCCESS, "mail send");

            return retValue;
        }


        public async Task<CMDefaultResponse> sendEmailNotification(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, int[] userID, int facilityId, params object[] args)
        {
            //m_delayDays = delayDays;
            CMDefaultResponse response = new CMDefaultResponse();

            List<CMUser> emailList = new List<CMUser>();
            // emailList = GetUserByNotificationId(notificationID);

            string subject;
            string printBody;

            if (m_notificationType == 2)
            {
                subject = getEMSubject(args);

            }
            else
            {
                subject = getSubject(args);
            }

            //string HTMLBody = getHTMLBody(args);
            string HTMLHeader = getHTMLHeader(args);
            string HTMLFooter = getHTMLFooter(args);
            string HTMLSignature = getHTMLSignature(args);
            int module_ref_id = getId(args);

            printBody = getHTMLBody(args);



            CMUserByNotificationId notification = new CMUserByNotificationId();
            notification.facility_id = facilityId;
            notification.module_id = moduleID;
            notification.role_id = m_role;
            notification.user_ids = userID;

            List<CMUser> users = new List<CMUser>();
            string notificationQry = "";
            try
            {
                //CMMSNotification objc = new CMMSNotification(_conn);
                // UserAccessRepository obj = new UserAccessRepository(_conn);
                if (m_notificationType == 2)

                {
                    users = await _userAccessRepository.GetEMUsers(facilityId, m_role, (int)moduleID);
                    notificationQry = $"INSERT INTO escalationlog (moduleId, moduleRefId, moduleStatus, notifSentToId, notifSentAt) VALUES " +
                                    $"({(int)moduleID}, {module_ref_id}, {(int)notificationID}, {m_role}, '{UtilsRepository.GetUTCTime()}'); " +
                                    $"SELECT LAST_INSERT_ID(); ";
                }
                else
                {

                    users = await _userAccessRepository.GetUserByNotificationId(notification);
                    notificationQry = $"INSERT INTO escalationlog (moduleId, moduleRefId, moduleStatus, notifSentToId, notifSentAt) VALUES " +
                                    $"({(int)moduleID}, {module_ref_id}, {(int)notificationID}, {m_role}, '{UtilsRepository.GetUTCTime()}'); " +
                                    $"SELECT LAST_INSERT_ID(); ";


                }
            }
            catch (Exception e)
            {

                if (users == null || users.Count == 0)
                {
                    return response = new CMDefaultResponse(2, CMMS.RETRUNSTATUS.INVALID_ARG, $"Email List for notification {notificationID} is empty "); ;
                }
            }
            List<string> EmailTo = new List<string>();
            // List<CMUser> EmailTo = users;

            System.Data.DataTable dt1 = await _conn.FetchData(notificationQry).ConfigureAwait(false);
            int escalationlogID = Convert.ToInt32(dt1.Rows[0][0]);

            string notificationRecordsQry = "INSERT INTO escalationsentto (escalationLogId, notifSentTo) VALUES ";
            string delimiter = "";
            int emailCount = 0;
            foreach (var email in users)
            {
                if (email != null)
                {
                    //EmailTo.Add(email.user_name);     //Temp . Remove when testing done
                    notificationRecordsQry += $"({escalationlogID}, '{email.id}'),";
                    emailCount++;
                }
            }
            /*
            if (users.Count > 0)
            {
                EmailTo.Add("cmms@softeltech.in");
                notificationRecordsQry = notificationRecordsQry.Substring(0, notificationRecordsQry.Length - 1);
            }
            System.Data.DataTable dt2 = await _conn.FetchData(notificationRecordsQry).ConfigureAwait(false);*/
            EmailTo.Add("notifications@softeltech.in");
            if (emailCount > 0)
            {
                EmailTo.Add("cmms@softeltech.in");

                notificationRecordsQry = notificationRecordsQry.TrimEnd(',');
                System.Data.DataTable dt2 = await _conn.FetchData(notificationRecordsQry).ConfigureAwait(false);
            }
            if (print)
            {
                response = new CMDefaultResponse(1, CMMS.RETRUNSTATUS.SUCCESS, "");
            }
            else
            {
                response = await sendEmail(subject, printBody, HTMLHeader, HTMLFooter, HTMLSignature, EmailTo);

            }
            return response;
        }


        public static async Task<CMDefaultResponse> sendEMNotification(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, int[] userID, int module_ref_id, int role, int delayDays, params object[] args)
        {
            CMDefaultResponse retValue;

            int notificationType = 2;
            /*
                        if (moduleID == CMMS.CMMS_Modules.JOB)     //JOB
                        {
                            CMJobView _jobView = (CMJobView)args[0];
                            notificationObj = new JobNotification(moduleID, notificationID, _jobView, notificationType);
                            facilityId = _jobView.facility_id;
                        }
            */

            //create else if block for your module and add Notification class for  your module to implement yous notification
            retValue = await sendBaseNotification(moduleID, notificationID, userID, module_ref_id, role, delayDays, notificationType, args);
            return retValue;
        }

        public static async Task<CMDefaultResponse> sendNotification(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, int[] userID, params object[] args)
        {
            CMDefaultResponse retValue;
            int notificationType = 1;
            int module_ref_id = 0;
            int role = 0;
            int delayDays = 0;

            //retValue = await sendBaseNotification(moduleID, notificationID, userID, args);
            retValue = await sendBaseNotification(moduleID, notificationID, userID, module_ref_id, role, delayDays, notificationType, args);
            return retValue;
        }


        //create else if block for your module and add Notification class for  your module to implement yous notification
        /*    public static CMMS.RETRUNSTATUS sendNotification(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, params object[] args)*/
        public static async Task<CMDefaultResponse> sendBaseNotification(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, int[] userID, int module_ref_id, int role, int delayDays, int notificationType, params object[] args)
        {
            CMDefaultResponse retValue = new CMDefaultResponse();

            int facilityId = 0;
            CMMSNotification notificationObj = null;

            if (moduleID == CMMS.CMMS_Modules.JOB)     //JOB
            {
                CMJobView _jobView = (CMJobView)args[0];
                notificationObj = new JobNotification(moduleID, notificationID, _jobView);
                notificationObj.module_ref_id = module_ref_id;
                notificationObj.m_delayDays = delayDays;
                notificationObj.m_notificationType = notificationType;
                notificationObj.m_role = role;
                facilityId = _jobView.facility_id;
                //                module_ref_id = _jobView.id;
            }
            else if (moduleID == CMMS.CMMS_Modules.PTW)    //PTW
            {
                CMPermitDetail _Permit = (CMPermitDetail)args[0];
                notificationObj = new PTWNotification(moduleID, notificationID, _Permit);
                facilityId = _Permit.facility_id;
            }
            else if (moduleID == CMMS.CMMS_Modules.JOBCARD)    //Job card
            {
                CMJCDetail _JobCard = (CMJCDetail)args[0];
                notificationObj = new JCNotification(moduleID, notificationID, _JobCard);
                // facilityId = _JobCard.facility_id;
            }
            else if (moduleID == CMMS.CMMS_Modules.INCIDENT_REPORT)    //Incident Report
            {
                CMViewIncidentReport _IncidentReport = (CMViewIncidentReport)args[0];
                notificationObj = new IncidentReportNotification(moduleID, notificationID, _IncidentReport);
                //facilityId = _IncidentReport.facility_id;
            }
            else if (moduleID == CMMS.CMMS_Modules.WARRANTY_CLAIM)    //Incident Report
            {
                CMWCDetail _WC = (CMWCDetail)args[0];
                notificationObj = new WCNotification(moduleID, notificationID, _WC);
                facilityId = _WC.facility_id;
            }
            else if (moduleID == CMMS.CMMS_Modules.CALIBRATION)    //Incident Report
            {
                CMCalibrationDetails _Calibration = (CMCalibrationDetails)args[0];
                notificationObj = new CalibrationNotification(moduleID, notificationID, _Calibration);
                //facilityId = _Calibration.facility_id;
            }
            else if (moduleID == CMMS.CMMS_Modules.INVENTORY)    //Incident Report
            {
                CMViewInventory _Inventory = (CMViewInventory)args[0];
                notificationObj = new InventoryNotification(moduleID, notificationID, _Inventory);
                //facilityId = _Inventory.facility_id;
            }

            else if (moduleID == CMMS.CMMS_Modules.GRIEVANCE)
            {
                CMGrievance _Grievance = (CMGrievance)args[0];
                notificationObj = new GrievanceNotification(moduleID, notificationID, _Grievance);
                //facilityId = _Inventory.facility_id;
            }

            else if (moduleID == CMMS.CMMS_Modules.MC_PLAN)
            {
                CMMCPlan _Plan = (CMMCPlan)args[0];
                notificationObj = new MCNotification(moduleID, notificationID, _Plan, null);
                //facilityId = _Inventory.facility_id;
            }
            else if (moduleID == CMMS.CMMS_Modules.MC_TASK)
            {
                CMMCExecution _Task = (CMMCExecution)args[0];
                notificationObj = new MCNotification(moduleID, notificationID, null, _Task);
                //facilityId = _Inventory.facility_id;
            }
            else if (moduleID == CMMS.CMMS_Modules.VEGETATION_PLAN)
            {
                CMMCPlan _Plan = (CMMCPlan)args[0];
                notificationObj = new VegetationNotification(moduleID, notificationID, _Plan, null);
                //facilityId = _Inventory.facility_id;
            }
            else if (moduleID == CMMS.CMMS_Modules.VEGETATION_TASK)
            {
                CMMCExecution _Task = (CMMCExecution)args[0];
                notificationObj = new VegetationNotification(moduleID, notificationID, null, _Task);
                //facilityId = _Inventory.facility_id;

            }
            //create else if block for your module and add Notification class for  your module to implement yous notification
            retValue = await notificationObj.sendEmailNotification(moduleID, notificationID, userID, facilityId, module_ref_id, 0, 0, notificationType, args);
            return retValue;
        }



    }
}
