using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMMSAPIs.Helper;
using CMMSAPIs.Models.Jobs;
using CMMSAPIs.Models.Permits;
using CMMSAPIs.Models.JC;
using CMMSAPIs.Models.Incident_Reports;
using CMMSAPIs.Models.WC;
using CMMSAPIs.Models.Calibration;
using CMMSAPIs.Models.Inventory;
using CMMSAPIs.Models.Mails;
using Microsoft.Extensions.Configuration;
using CMMSAPIs.BS;
using CMMSAPIs.BS.Mails;
using CMMSAPIs.Repositories.Users;
using CMMSAPIs.Models.Users;
using System.IO;
using CMMSAPIs.BS.Facility;
using CMMSAPIs.BS.Users;
using System.Data;
using CMMSAPIs.Models.Utils;
using System.Drawing;
using Microsoft.AspNetCore.Hosting;
using System.Configuration;

//using CommonUtilities;
//using CMMSAPIs.Models.Notifications;

namespace CMMSAPIs.Models.Notifications
{

    abstract public class CMMSNotification : UserAccessRepository
    {
        public static bool print = false;
        public static string printBody = "";

        private CMMS.CMMS_Modules m_moduleID;
        protected CMMS.CMMS_Status m_notificationID;

        public string template = "<tr><td style=' text-align: left; padding:0.5rem; background-color:#31576D;color:#000000;width:35%' ><b>&nbsp;&nbsp;{0}</b></td><td style='text-align: left; padding:0.5rem;'>&nbsp;&nbsp;{1}</td></tr>";

        public string templateEnd = "<tr><td style=' text-align: left; padding:0.5rem; background-color:#31576D;color:#000000'><b>&nbsp;&nbsp;{0}</b></td><td style='text-align: left; padding:0.5rem;'>&nbsp;&nbsp;{1}</td></tr></table>";

        private static MYSQLDBHelper sqlDBHelper;

        public CMMSNotification(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID): base(sqlDBHelper)
        {
            m_moduleID = moduleID;
            m_notificationID = notificationID;
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

            AddTo.Add("tanvi@softeltech.in");

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
        public async Task<CMDefaultResponse> sendEmailNotification(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, int facilityId, params object[] args)
        {
            CMDefaultResponse response = new CMDefaultResponse();

            List<CMUser> emailList = new List<CMUser>();
            // emailList = GetUserByNotificationId(notificationID);

            string subject = getSubject(args);
            string HTMLBody = getHTMLBody(args);
            string HTMLHeader = getHTMLHeader(args);
            string HTMLFooter = getHTMLFooter(args);
            string HTMLSignature = getHTMLSignature(args);
            printBody = getHTMLBody(args);

            CMUserByNotificationId notification = new CMUserByNotificationId();
            notification.facility_id = facilityId;
            notification.notification_id = notificationID;

            List<CMUser> users = new List<CMUser>();
            try
            {
                users = await GetUserByNotificationId(notification);
            }
            catch (Exception e) { 

               if(users == null || users.Count==0)
                {
                    return response = new CMDefaultResponse(2, CMMS.RETRUNSTATUS.INVALID_ARG, "Email List is empty "); ;
                }
            } 
            List<string> EmailTo = new List<string>();
            // List<CMUser> EmailTo = users;
            foreach (var email in users)
            {
                EmailTo.Add(email.user_name);
            }




            if (print)
            {
                response = new CMDefaultResponse(1, CMMS.RETRUNSTATUS.SUCCESS, ""); 
            }
            else
            {
                response = await sendEmail(subject, HTMLBody, HTMLHeader, HTMLFooter, HTMLSignature, EmailTo);

            }
            return response;

        }

        //create else if block for your module and add Notification class for  your module to implement yous notification
        /*    public static CMMS.RETRUNSTATUS sendNotification(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, params object[] args)*/
        public static async Task<CMDefaultResponse> sendNotification(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, params object[] args)
        {
            CMDefaultResponse retValue = new CMDefaultResponse();
            CMMSNotification notificationObj = null;
            int facilityId = 0;

            if (moduleID == CMMS.CMMS_Modules.JOB)     //JOB
            {
                CMJobView _jobView = (CMJobView)args[0];
                notificationObj = new JobNotification(moduleID, notificationID, _jobView);
                facilityId = _jobView.facility_id;
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
            //create else if block for your module and add Notification class for  your module to implement yous notification
            retValue = await notificationObj.sendEmailNotification(moduleID, notificationID, facilityId, args);
            return retValue;
        }



    }
}
