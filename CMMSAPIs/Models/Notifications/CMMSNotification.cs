using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMMSAPIs.Helper;
using CMMSAPIs.Models.Jobs;
using CMMSAPIs.Models.Permits;
using CMMSAPIs.Models.JC;
using CMMSAPIs.Models.Incident_Reports;
using CMMSAPIs.Models.SM;
using CMMSAPIs.Models.EscalationMatrix;

//using CommonUtilities;
//using CMMSAPIs.Models.Notifications;

namespace CMMSAPIs.Models.Notifications
{

    abstract public class CMMSNotification
    {
        private CMMS.CMMS_Modules m_moduleID;
        protected CMMS.CMMS_Status m_notificationID;

        protected CMMSNotification(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID)
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


        protected CMMS.RETRUNSTATUS sendEmail(string subject, string HTMLBody, string HTMLHeader, string HTMLFooter, string HTMLSignature, params object[] args)
        {
            CMMS.RETRUNSTATUS retValue = CMMS.RETRUNSTATUS.SUCCESS;

            //CommonUtilities.Mail.MailManager mailObj = new CommonUtilities.Mail.MailManager();

            return retValue;
        }
        public CMMS.RETRUNSTATUS sendEmailNotification(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, params object[] args)
        {
            CMMS.RETRUNSTATUS retValue = CMMS.RETRUNSTATUS.FAILURE;
          
            string subject = getSubject(args);
            string HTMLBody = getHTMLBody(args);
            string HTMLHeader = getHTMLHeader(args);
            string HTMLFooter = getHTMLFooter(args);
            string HTMLSignature = getHTMLSignature(args);

            retValue = sendEmail(subject, HTMLBody, HTMLHeader, HTMLFooter, HTMLSignature);
            return retValue;

        }

        //create else if block for your module and add Notification class for  your module to implement yous notification
    /*    public static CMMS.RETRUNSTATUS sendNotification(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, params object[] args)*/
        public static CMMS.RETRUNSTATUS sendNotification(CMMS.CMMS_Modules moduleID , CMMS.CMMS_Status notificationID, params object[] args)
        {
            CMMS.RETRUNSTATUS retValue = CMMS.RETRUNSTATUS.FAILURE;
            CMMSNotification notificationObj = null;

            if (moduleID == CMMS.CMMS_Modules.JOB)     //JOB
            {
                CMJobView _jobView = (CMJobView)args[0];
                notificationObj = new JobNotification(moduleID, notificationID, _jobView);
            }
            else if (moduleID == CMMS.CMMS_Modules.PTW)    //PTW
            {
                CMPermitDetail _Permit = (CMPermitDetail)args[0];
                notificationObj = new PTWNotification(moduleID, notificationID, _Permit);
            }
            else if (moduleID == CMMS.CMMS_Modules.JOBCARD)    //Job card
            {
                CMJCDetail _JobCard = (CMJCDetail)args[0];
                notificationObj = new JCNotification(moduleID, notificationID, _JobCard);
            }
            else if (moduleID == CMMS.CMMS_Modules.INCIDENT_REPORT)    //Incident Report
            {
                CMViewIncidentReport _IncidentReport = (CMViewIncidentReport)args[0];
                notificationObj = new IncidentReportNotification(moduleID, notificationID, _IncidentReport);
            }else if (moduleID == CMMS.CMMS_Modules.SM_MASTER)
            {
                CMSMMaster cMSMMaster = (CMSMMaster)args[0];
                //notificationObj = new SMMasterNotification(moduleID, notificationID, cMSMMaster);
            }else if(moduleID == CMMS.CMMS_Modules.SM_PO) {
                CMGO _CmGO = (CMGO)args[0];
                //notificationObj = new IncidentReportNotification(moduleID, notificationID, _CmGO);
            }else if (moduleID == CMMS.CMMS_Modules.Escalation_Matrix)
            {
                CMEscalationMatrixModel _CMEM = (CMEscalationMatrixModel)args[0];
                notificationObj = new EMNotification(moduleID, notificationID, _CMEM);
            }
            //create else if block for your module and add Notification class for  your module to implement yous notification
            retValue = notificationObj.sendEmailNotification(moduleID, notificationID, args);
            return retValue;
        }
    }
}
