using CMMSAPIs.BS.Mails;
using CMMSAPIs.Helper;
using CMMSAPIs.Models.Calibration;
using CMMSAPIs.Models.EM;
using CMMSAPIs.Models.Grievance;
using CMMSAPIs.Models.Incident_Reports;
using CMMSAPIs.Models.Inventory;
using CMMSAPIs.Models.JC;
using CMMSAPIs.Models.Jobs;
using CMMSAPIs.Models.Mails;
using CMMSAPIs.Models.MC;
using CMMSAPIs.Models.Permits;
using CMMSAPIs.Models.PM;
using CMMSAPIs.Models.SM;
using CMMSAPIs.Models.Users;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Models.WC;
using CMMSAPIs.Repositories;
using CMMSAPIs.Repositories.Audits;
using CMMSAPIs.Repositories.Calibration;
using CMMSAPIs.Repositories.Incident_Reports;
using CMMSAPIs.Repositories.Inventory;
using CMMSAPIs.Repositories.JC;
using CMMSAPIs.Repositories.Jobs;
using CMMSAPIs.Repositories.Permits;
using CMMSAPIs.Repositories.SM;
using CMMSAPIs.Repositories.Users;
using CMMSAPIs.Repositories.Utils;
using CMMSAPIs.Repositories.CleaningRepository;
using CMMSAPIs.Repositories.WC;
using CMMSAPIs.Repositories.PM;
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
        public static string HTMLBody = "";

        protected CMMS.CMMS_Modules m_moduleID;
        protected CMMS.CMMS_Status m_notificationID;
        protected int m_notificationType = 1;
        protected int m_delayDays = 0;
        protected int m_module_ref_id = 0;
        protected int m_role = 0;
        protected string m_baseURL = "http://172.20.43.9:82/#/cmms-screen/";


        public string template = "<tr><td style=' text-align: left; padding:0.5rem; background-color:#31576D;color:#ffffff;width:35%' ><b>&nbsp;&nbsp;{0}</b></td><td style='text-align: left; padding:0.5rem;'>&nbsp;&nbsp;{1}</td></tr>";

        public string templateEnd = "<tr><td style=' text-align: left; padding:0.5rem; background-color:#31576D;color:#ffffff'><b>&nbsp;&nbsp;{0}</b></td><td style='text-align: left; padding:0.5rem;'>&nbsp;&nbsp;{1}</td></tr></table>";

        private static IConfigurationRoot MyConfig;
        private static string _conString;
        public static MYSQLDBHelper getDB;
        //public static MYSQLDBHelper _conn;


        private UserAccessRepository _userAccessRepository;
        public static Microsoft.AspNetCore.Hosting.IWebHostEnvironment _environment;

        public CMMSNotification(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID)
        {
            m_moduleID = moduleID;
            m_notificationID = notificationID;

            MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            _conString = MyConfig.GetValue<string>("ConnectionStrings:Con");
            if (getDB == null)
            {
                getDB = new MYSQLDBHelper(_conString);
            }

            IWebHostEnvironment webHostEnvironment = null;
            IConfiguration configuration = null;
            _userAccessRepository = new UserAccessRepository(getDB, webHostEnvironment, configuration);
        }


        protected virtual int getId(params object[] args)
        {
            return m_module_ref_id;
        }
        protected virtual string getURL(params object[] args)
        {
            return $"{m_baseURL}/{(int)m_moduleID}/{m_module_ref_id}";
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

            string Body = "";// "<div style='width:100%; padding:0.5rem; text-align:center;'><span><img src='https://i.ibb.co/FD60YSY/hfe.png' alt='hfe' border='0'></span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b style='color:#31576D; padding:0.5rem;'><i>" + today.ToString("dddd") + " , " + today.ToString("dd-MMM-yyyy") + "</i></b></div><hr><br><div style='text-align:center;'>";

            Body += HTMLBody;
            string url = getURL(args);
            //string disclaimer = "Information contained in this email is strictly confidential, proprietary of Hero Future Energies and intended solely for the use of the addressee. If you are not the intended recipient, please notify the sender, delete this mail from your system immediately and do not disseminate, distribute, or copy this e-mail/its contents.";
            //Body += "</div><br><div><p style='text-align:center;'>visit:<a href=" + url + "> click here </a></p></div><br><p style='padding:0.5rem; '><b>Disclaimer:</b> " + disclaimer + " </p>";

            CMMailRequest request = new CMMailRequest();

            //AddTo.Add("tanvi@softeltech.in");
            request.ToEmail = AddTo;
            request.CcEmail = AddCc;
            request.Subject = subject;
            request.Body = Body;
            request.Headers = HTMLHeader;

            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;
            string strMessage = "mail with subject <" + subject + "> sent";
            try
            {
                var res = MailService.SendEmailAsync(request, _settings);

                if (res != null && res.Exception != null)
                {
                    if (res.Exception.InnerExceptions.Count > 0)
                    {
                        strMessage = res.Exception.Message;
                    }
                    else
                    {
                        strMessage = "Some error";
                    }
                }
                else
                {
                    retCode = CMMS.RETRUNSTATUS.SUCCESS;
                }
            }
            catch (Exception e)
            {
                strMessage = e.Message;
            }
            CMDefaultResponse retValue = new CMDefaultResponse(1, retCode, strMessage);

            return retValue;
        }

        public async Task<CMDefaultResponse> sendEmailNotification(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, string userIDs, int facilityId, params object[] args)
        {
            //m_delayDays = delayDays;
            CMDefaultResponse response = new CMDefaultResponse();

            List<CMUser> emailList = new List<CMUser>();

            //int notificationID_int = (int)notificationID;
            // emailList = GetUserByNotificationId(notificationID);

            string subject;

            if (m_notificationType == 2)
            {
                subject = getEMSubject(args);

            }
            else
            {
                subject = getSubject(args);
            }

            HTMLBody = getHTMLBody(args);
            string HTMLHeader = getHTMLHeader(args);
            string HTMLFooter = getHTMLFooter(args);
            string HTMLSignature = getHTMLSignature(args);
            int module_ref_id = getId(args);
            string url = getURL(args);


            CMUserByNotificationId notification = new CMUserByNotificationId();
            notification.facility_id = facilityId;
            notification.module_id = moduleID;
            notification.notification_id = (int)notificationID;
            notification.additional_user_ids = userIDs;
            notification.role_id = m_role;
            //notification.user_ids = userID;

            List<CMUser> users = new List<CMUser>();
            string notificationQry = "";
            try
            {
                //CMMSNotification objc = new CMMSNotification(_conn);
                // UserAccessRepository obj = new UserAccessRepository(_conn);
                if (m_notificationType == 2)
                {
                    users = await _userAccessRepository.GetEMUsers(notification);
                    notificationQry = $"INSERT INTO escalationlog (moduleId, moduleRefId, moduleStatus, notifSentToId, notifSentAt, emailUserCount, notificationType) VALUES " +
                                    $"({(int)moduleID}, {module_ref_id}, {(int)notificationID}, {m_role}, '{UtilsRepository.GetUTCTime()}', {users.Count}, {m_notificationType}); " +
                                    $"SELECT LAST_INSERT_ID(); ";
                }
                else
                {
                    users = await _userAccessRepository.GetUserByNotificationId(notification);
                    notificationQry = $"INSERT INTO escalationlog (moduleId, moduleRefId, moduleStatus, notifSentToId, notifSentAt, emailUserCount, notificationType) VALUES " +
                                    $"({(int)moduleID}, {module_ref_id}, {(int)notificationID}, {m_role}, '{UtilsRepository.GetUTCTime()}', {users.Count}, {m_notificationType}); " +
                                    $"SELECT LAST_INSERT_ID(); ";
                }
                List<string> EmailTo = new List<string>();
           if (getDB == null)
                {
                    getDB = new MYSQLDBHelper(_conString);
                }
                System.Data.DataTable dt1 = await getDB.FetchData(notificationQry).ConfigureAwait(false);
                int escalationlogID = Convert.ToInt32(dt1.Rows[0][0]);

                string notificationRecordsQry = "INSERT INTO escalationsentto (escalationLogId, notifSentTo,notifSentAt) VALUES ";
                string delimiter = "";
                int emailCount = 0;
                foreach (var email in users)
                {
                    if (email != null)
                    {
                        EmailTo.Add(email.user_name);     //Temp . Remove when testing done
                        notificationRecordsQry += $"({escalationlogID}, '{email.id}', '{UtilsRepository.GetUTCTime()}'),";
                        emailCount++;
                    }
                }

                EmailTo.Add("notifications@softeltech.in");
                if (users.Count > 0)
                 {
                     notificationRecordsQry = notificationRecordsQry.TrimEnd(',');
                     //notificationRecordsQry = notificationRecordsQry.Substring(0, notificationRecordsQry.Length - 1);
                     if (getDB == null)
                     {
                         getDB = new MYSQLDBHelper(_conString);
                     }
                     System.Data.DataTable dt2 = await getDB.FetchData(notificationRecordsQry).ConfigureAwait(false);
                 }
                 

                if (print)
                {
                    response = new CMDefaultResponse(1, CMMS.RETRUNSTATUS.SUCCESS, "");
                }
                else
                {
                    response = await sendEmail(subject, HTMLBody, HTMLHeader, HTMLFooter, HTMLSignature, EmailTo);

                }
            }
            catch (Exception e)
            {

                if (users == null || users.Count == 0)
                {
                    return response = new CMDefaultResponse(2, CMMS.RETRUNSTATUS.INVALID_ARG, $"Email List for notification {notificationID} is empty "); ;
                }
                else
                {
                    response = new CMDefaultResponse(1, CMMS.RETRUNSTATUS.SUCCESS, e.Message);
                }
            }            
            return response;
        }

        public static async Task<CMDefaultResponse> sendNotification2(int notificationType, CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, int module_ref_id, string additionalUserIds, int userId, string facilitytimeZone, int role=0, int delayDays=0)
        {
            CMDefaultResponse retValue;
            //string facilitytimeZone = "";
            if (getDB == null)
            {
                MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
                _conString = MyConfig.GetValue<string>("ConnectionStrings:Con");

                getDB = new MYSQLDBHelper(_conString);
            }

            switch (moduleID)
            {
                case CMMS.CMMS_Modules.JOB:
                    JobRepository obj0 = new JobRepository(getDB);
                    CMJobView _jobView = await obj0.GetJobDetails(module_ref_id, facilitytimeZone);
                    //notificationID = (CMMS.CMMS_Status)(_jobView.status);
                    retValue = await sendBaseNotification(moduleID, notificationID, module_ref_id, additionalUserIds, role, delayDays, notificationType, _jobView);
                    break;
                case CMMS.CMMS_Modules.PTW:
                    PermitRepository obj1 = new PermitRepository(getDB);
                    CMPermitDetail _Permit = await obj1.GetPermitDetails(module_ref_id, facilitytimeZone);
                    //notificationID = (CMMS.CMMS_Status)(_Permit.ptwStatus);
                    retValue = await sendBaseNotification(moduleID, notificationID, module_ref_id, additionalUserIds, role, delayDays, notificationType, _Permit);
                    break;
                case CMMS.CMMS_Modules.JOBCARD:
                    JCRepository obj2 = new JCRepository(getDB);
                    List<CMJCDetail> _JobCard = await obj2.GetJCDetail(module_ref_id, facilitytimeZone);
                    //notificationID = (CMMS.CMMS_Status)(_JobCard[0].status);
                    retValue = await sendBaseNotification(moduleID, notificationID, module_ref_id, additionalUserIds, role, delayDays, notificationType, _JobCard[0]);
                    break;
                case CMMS.CMMS_Modules.INCIDENT_REPORT:
                    IncidentReportRepository obj3 = new IncidentReportRepository(getDB);
                    CMViewIncidentReport _IncidentReport = await obj3.GetIncidentDetailsReport(module_ref_id, facilitytimeZone);
                    //notificationID = (CMMS.CMMS_Status)(_IncidentReport.status);
                    retValue = await sendBaseNotification(moduleID, notificationID, module_ref_id, additionalUserIds, role, delayDays, notificationType, _IncidentReport);
                    break;
                case CMMS.CMMS_Modules.WARRANTY_CLAIM:
                    WCRepository obj4 = new WCRepository(getDB);
                    CMWCDetail _WC = await obj4.GetWCDetails(module_ref_id, facilitytimeZone);
                    //notificationID = (CMMS.CMMS_Status)(_WC.status);
                    retValue = await sendBaseNotification(moduleID, notificationID, module_ref_id, additionalUserIds, role, delayDays, notificationType, _WC);
                    break;
                case CMMS.CMMS_Modules.CALIBRATION:
                    CalibrationRepository obj5 = new CalibrationRepository(getDB);
                    CMCalibrationDetails _Calibration = await obj5.GetCalibrationDetails(module_ref_id, facilitytimeZone);
                    //notificationID = (CMMS.CMMS_Status)(_Calibration.statusID + 100);
                    retValue = await sendBaseNotification(moduleID, notificationID, module_ref_id, additionalUserIds, role, delayDays, notificationType, _Calibration);
                    break;
                case CMMS.CMMS_Modules.INVENTORY:
                    InventoryRepository obj6 = new InventoryRepository(getDB, _environment);
                    CMViewInventory _Inventory = await obj6.GetInventoryDetails(module_ref_id, facilitytimeZone);
                    //notificationID = (CMMS.CMMS_Status)(_Inventory[0].status + 100);
                    retValue = await sendBaseNotification(moduleID, notificationID, module_ref_id, additionalUserIds, role, delayDays, notificationType, _Inventory);
                    break;
                case CMMS.CMMS_Modules.MC_PLAN:
                    CleaningRepository obj15 = new CleaningRepository(getDB);
                    CMMCPlan _ViewPlan = await obj15.GetPlanDetails(module_ref_id, facilitytimeZone);
                    retValue = await sendBaseNotification(moduleID, notificationID, module_ref_id, additionalUserIds, role, delayDays, notificationType, _ViewPlan);
                    break;
                case CMMS.CMMS_Modules.MC_TASK:
                    CleaningRepository obj16 = new CleaningRepository(getDB);
                    CMMCExecution _ViewTask = await obj16.GetExecutionDetails(module_ref_id, facilitytimeZone);
                    retValue = await sendBaseNotification(moduleID, notificationID, module_ref_id, additionalUserIds, role, delayDays, notificationType, _ViewTask);
                    break;
                case CMMS.CMMS_Modules.MC_EXECUTION:
                    CleaningRepository obj17 = new CleaningRepository(getDB);
                    CMMCExecutionSchedule _ViewSchedule = await obj17.GetScheduleDetails(module_ref_id, facilitytimeZone);
                    retValue = await sendBaseNotification(moduleID, notificationID, module_ref_id, additionalUserIds, role, delayDays, notificationType, _ViewSchedule);
                    break;

                case CMMS.CMMS_Modules.VEGETATION_PLAN:
                    VegetationRepository obj18 = new VegetationRepository(getDB);
                    CMMCPlan _ViewPlanList = await obj18.GetPlanDetails(module_ref_id, facilitytimeZone);
                    retValue = await sendBaseNotification(moduleID, notificationID, module_ref_id, additionalUserIds, role, delayDays, notificationType, _ViewPlanList);
                    break;
                case CMMS.CMMS_Modules.VEGETATION_TASK:
                    VegetationRepository obj19 = new VegetationRepository(getDB);
                    CMMCExecution _ViewTaskList = await obj19.GetExecutionDetails(module_ref_id, facilitytimeZone);
                    retValue = await sendBaseNotification(moduleID, notificationID, module_ref_id, additionalUserIds, role, delayDays, notificationType, _ViewTaskList);
                    break;
                case CMMS.CMMS_Modules.VEGETATION_EXECUTION:
                    VegetationRepository obj20 = new VegetationRepository(getDB);
                    CMMCExecutionSchedule _ViewVegSchedule = await obj20.GetScheduleDetails(module_ref_id, facilitytimeZone);
                    retValue = await sendBaseNotification(moduleID, notificationID, module_ref_id, additionalUserIds, role, delayDays, notificationType, _ViewVegSchedule);
                    break;

                case CMMS.CMMS_Modules.PM_PLAN:
                    PMRepository obj21 = new PMRepository(getDB, _environment);
                    CMPMPlanDetail _ViewPMPlan = await obj21.GetPMPlanDetail(module_ref_id, facilitytimeZone);
                    retValue = await sendBaseNotification(moduleID, notificationID, module_ref_id, additionalUserIds, role, delayDays, notificationType, _ViewPMPlan);
                    break;
                case CMMS.CMMS_Modules.PM_TASK:
                    PMScheduleViewRepository obj22 = new PMScheduleViewRepository(getDB);
                    CMPMTaskView _PMTaskList = await obj22.GetPMTaskDetail(module_ref_id, facilitytimeZone);
                    retValue = await sendBaseNotification(moduleID, notificationID, module_ref_id, additionalUserIds, role, delayDays, notificationType, _PMTaskList);
                    break;
                case CMMS.CMMS_Modules.PM_SCHEDULE:
                    PMScheduleViewRepository obj23 = new PMScheduleViewRepository(getDB);
                    CMPMScheduleExecutionDetail PMTaskSchedule = await obj23.GetPMTaskScheduleDetail(0, module_ref_id, facilitytimeZone);
                    retValue = await sendBaseNotification(moduleID, notificationID, module_ref_id, additionalUserIds, role, delayDays, notificationType, PMTaskSchedule);
                    break;
                case CMMS.CMMS_Modules.SM_MRS:
                    MRSRepository obj7 = new MRSRepository(getDB);
                    CMMRSList _MRS = await obj7.getMRSDetails(module_ref_id, facilitytimeZone);
                    //notificationID = (CMMS.CMMS_Status)(_MRS[0].status + 100);
                    retValue = await sendBaseNotification(moduleID, notificationID, module_ref_id, additionalUserIds, role, delayDays, notificationType, _MRS);
                    break;
                case CMMS.CMMS_Modules.SM_MRS_RETURN:
                    MRSRepository obj8 = new MRSRepository(getDB);
                    CMMRSReturnList _RMRS = await obj8.getReturnDataByID(module_ref_id, facilitytimeZone);
                    //notificationID = (CMMS.CMMS_Status)(_RMRS[0].status + 100);
                    retValue = await sendBaseNotification(moduleID, notificationID, module_ref_id, additionalUserIds, role, delayDays, notificationType, _RMRS);
                    break;
                case CMMS.CMMS_Modules.SM_GO:
                    GORepository obj9 = new GORepository(getDB);
                    CMGOMaster _GO = await obj9.GetGODetailsByID(module_ref_id, facilitytimeZone);
                    //notificationID = (CMMS.CMMS_Status)(_GO[0].status + 100);
                    retValue = await sendBaseNotification(moduleID, notificationID, module_ref_id, additionalUserIds, role, delayDays, notificationType, _GO);
                    break;
                case CMMS.CMMS_Modules.SM_RO:
                    RequestOrderRepository obj10 = new RequestOrderRepository(getDB);
                    List<CMCreateRequestOrderGET> _SMRO = await obj10.GetRODetailsByID(module_ref_id.ToString(), facilitytimeZone);
                    //notificationID = (CMMS.CMMS_Status)(_SMRO[0].status + 100);
                    retValue = await sendBaseNotification(moduleID, notificationID, module_ref_id, additionalUserIds, role, delayDays, notificationType, _SMRO);
                    break;
                case CMMS.CMMS_Modules.AUDIT_TASK:
                    AuditPlanRepository obj11 = new AuditPlanRepository(getDB);
                    CMPMTaskView _Audit = await obj11.GetTaskDetail(module_ref_id, facilitytimeZone);
                    //notificationID = (CMMS.CMMS_Status)(_Audit[0].status + 100);
                    retValue = await sendBaseNotification(moduleID, notificationID, module_ref_id, additionalUserIds, role, delayDays, notificationType, _Audit);
                    break;
                case CMMS.CMMS_Modules.AUDIT_PLAN:
                    AuditPlanRepository obj12 = new AuditPlanRepository(getDB);
                    CMPMPlanDetail _AuditPlan = await obj12.GetAuditPlanDetail(module_ref_id, facilitytimeZone);
                    //notificationID = (CMMS.CMMS_Status)(_AuditPlan[0].status + 100);
                    retValue = await sendBaseNotification(moduleID, notificationID, module_ref_id, additionalUserIds, role, delayDays, notificationType, _AuditPlan);
                    break;
                default:
                    string sReturn = $"Escalation performed for {moduleID} {module_ref_id} for role {role} for {delayDays} days period.";
                    //throw  System.SystemException(sReturn);
                    retValue = new CMDefaultResponse(module_ref_id, CMMS.RETRUNSTATUS.INVALID_ARG, sReturn);
                    break;
            }
            //await CMMSNotification.sendEMNotification(moduleID, notificationID, module_ref_id, additionalUserIds, role, delayDays, _jobView);

            //            retValue = await sendBaseNotification(moduleID, notificationID, userID, module_ref_id, role, delayDays, notificationType, obj);
            return retValue;

        }

        public static async Task<CMDefaultResponse> sendNotification(CMMS.CMMS_Modules moduleId, CMMS.CMMS_Status statusId, int moduleRefId, int userID, string facilitytimeZone)
        {
            int notificationType = 1;
            string additionalUserIds = "";
            return await sendNotification2(notificationType, moduleId, statusId, moduleRefId, additionalUserIds, userID, facilitytimeZone);
        }
        public static async Task<CMDefaultResponse> sendNotification(CMNotification request, int userID, string facilitytimeZone)
        {
            int notificationType = 1;
            return await sendNotification2(notificationType, request.moduleId, request.statusId, request.moduleRefId, request.additionalUserIds, userID, facilitytimeZone);
        }
        public static async Task<CMDefaultResponse> sendEMNotification(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, int module_ref_id, int userID, string facilitytimeZone, string additionalUserIds, int role, int delayDays)
        {
            int notificationType = 2;

            return await sendNotification2(notificationType, moduleID, notificationID, module_ref_id, additionalUserIds, userID, facilitytimeZone, role, delayDays);
        }

        public static async Task<CMDefaultResponse> sendNotification(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, int[] userID, params object[] args)
        {
            CMDefaultResponse retValue;
            int notificationType = 1;
            int module_ref_id = 0;
            string additionalUserIds = "";
            int role = 0;
            int delayDays = 0;

            //retValue = await sendBaseNotification(moduleID, notificationID, userID, args);
            retValue = await sendBaseNotification(moduleID, notificationID, module_ref_id, additionalUserIds, role, delayDays, notificationType, args);
            return retValue;
        }


        public static async Task<CMDefaultResponse> sendBaseNotification(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, int module_ref_id, string userIDs, int role, int delayDays, int notificationType, params object[] args)
        {
            CMDefaultResponse retValue = new CMDefaultResponse();

            int facilityId = 0;
            CMMSNotification notificationObj = null;

            if (moduleID == CMMS.CMMS_Modules.JOB)     //JOB
            {
                CMJobView _Job = (CMJobView)args[0];
                notificationObj = new JobNotification(moduleID, notificationID, _Job);
                facilityId = _Job.facility_id;
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
                facilityId = _JobCard.facility_id;
            }
            else if (moduleID == CMMS.CMMS_Modules.INCIDENT_REPORT)    //Incident Report
            {
                CMViewIncidentReport _IncidentReport = (CMViewIncidentReport)args[0];
                notificationObj = new IncidentReportNotification(moduleID, notificationID, _IncidentReport);
                facilityId = _IncidentReport.facility_id;
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
                facilityId = _Calibration.facilityId;
            }
            else if (moduleID == CMMS.CMMS_Modules.INVENTORY)    //Incident Report
            {
                CMViewInventory _Inventory = (CMViewInventory)args[0];
                notificationObj = new InventoryNotification(moduleID, notificationID, _Inventory);
                facilityId = _Inventory.facilityId;
            }

            else if (moduleID == CMMS.CMMS_Modules.GRIEVANCE)
            {
                CMGrievance _Grievance = (CMGrievance)args[0];
                notificationObj = new GrievanceNotification(moduleID, notificationID, _Grievance);
                facilityId = _Grievance.facilityId;
            }


            else if (moduleID == CMMS.CMMS_Modules.MC_PLAN)
            {
                CMMCPlan _Plan = (CMMCPlan)args[0];
                notificationObj = new MCNotification(moduleID, notificationID, _Plan);
                facilityId = _Plan.facilityId;
            }
            else if (moduleID == CMMS.CMMS_Modules.MC_TASK)
            {
                CMMCExecution _Task = (CMMCExecution)args[0];
                notificationObj = new MCNotificationTask(moduleID, notificationID, _Task);
                facilityId = _Task.facility_id;
            }
            else if (moduleID == CMMS.CMMS_Modules.MC_EXECUTION)
            {
                CMMCExecutionSchedule _TaskSchedule = (CMMCExecutionSchedule)args[0];
                notificationObj = new MCScheduleNotification(moduleID, notificationID, _TaskSchedule);
                facilityId = _TaskSchedule.facility_id;
            }
            else if (moduleID == CMMS.CMMS_Modules.VEGETATION_PLAN)
            {
                CMMCPlan _Plan = (CMMCPlan)args[0];
                notificationObj = new VegetationNotification(moduleID, notificationID, _Plan);
                facilityId = _Plan.facilityId;
            }
            else if (moduleID == CMMS.CMMS_Modules.VEGETATION_TASK)
            {
                CMMCExecution _Task = (CMMCExecution)args[0];
                notificationObj = new VEGNotificationTask(moduleID, notificationID, _Task);
                facilityId = _Task.facility_id;
            }
            else if (moduleID == CMMS.CMMS_Modules.VEGETATION_EXECUTION)
            {
                CMMCExecutionSchedule _Schedule = (CMMCExecutionSchedule)args[0];
                notificationObj = new VEGScheduleNotification(moduleID, notificationID, _Schedule);
                facilityId = _Schedule.facility_id;
            }
            else if (moduleID == CMMS.CMMS_Modules.SM_MRS)     //MRS Report
            {
                CMMRSList _MRS = (CMMRSList)args[0];
                notificationObj = new MRSNotification(moduleID, notificationID, _MRS);
                facilityId = _MRS.facilityId;   
            }
            else if (moduleID == CMMS.CMMS_Modules.SM_MRS_RETURN)     //Return MRS Report
            {
                CMMRSReturnList _RMRS = (CMMRSReturnList)args[0];
                notificationObj = new ReturnMRSNotification(moduleID, notificationID, _RMRS);
                facilityId = _RMRS.facilityId;   
            }
            else if (moduleID == CMMS.CMMS_Modules.PM_PLAN)
            {
                CMPMPlanDetail _Plan = (CMPMPlanDetail)args[0];
                notificationObj = new PMNotification(moduleID, notificationID, _Plan);
                facilityId = _Plan.facility_id;
            }
            else if (moduleID == CMMS.CMMS_Modules.PM_TASK)
            {
                CMPMTaskView _Task = (CMPMTaskView)args[0];
                notificationObj = new PMNotification(moduleID, notificationID, _Task);
                facilityId = _Task.facility_id;
            }
            else if (moduleID == CMMS.CMMS_Modules.PM_SCHEDULE)
            {
                CMPMScheduleExecutionDetail _Schedule = (CMPMScheduleExecutionDetail)args[0];
                notificationObj = new PMNotification(moduleID, notificationID, _Schedule);
                //facilityId = _Schedule.facility_id;
            }
            else if (moduleID == CMMS.CMMS_Modules.SM_GO)      //GO Report
            {
                CMGOMaster _GO = (CMGOMaster)args[0];
                notificationObj = new GONotification(moduleID, notificationID, _GO);
                facilityId = _GO.facility_id;
            }
            else if (moduleID == CMMS.CMMS_Modules.SM_RO)      //RO Report
            {
                CMCreateRequestOrderGET _SMRO = (CMCreateRequestOrderGET)args[0];
                notificationObj = new SMNotification(moduleID, notificationID, _SMRO);
                facilityId = _SMRO.facilityID;
            }
            else if (moduleID == CMMS.CMMS_Modules.AUDIT_TASK)      //Audit Task
            {
                CMPMTaskView _Audit = (CMPMTaskView)args[0];
                notificationObj = new AuditTasknotification(moduleID, notificationID, _Audit);
                facilityId = _Audit.facility_id;
            }
            else if (moduleID == CMMS.CMMS_Modules.AUDIT_PLAN)      //Audit Plan
            {
                CMPMPlanDetail _AuditPlan = (CMPMPlanDetail)args[0];
                notificationObj = new AuditPlannotification(moduleID, notificationID, _AuditPlan);
                facilityId = _AuditPlan.facility_id;
            }
            else
            {
                throw new Exception("Notification code is  implemented for module <" + moduleID + ">");
            }
            notificationObj.m_notificationType = notificationType;
            notificationObj.m_delayDays = delayDays;
            notificationObj.m_role = role;
            //create else if block for your module and add Notification class for  your module to implement yous notification
            retValue = await notificationObj.sendEmailNotification(moduleID, notificationID, userIDs, facilityId, args);
            return retValue;
        }
    }
}
