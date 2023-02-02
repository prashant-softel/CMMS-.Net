using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using CommonUtilities;
using CMMSAPIs.Models.Notifications;

namespace CMMSAPIs.Helper
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
        protected virtual string getSubject(params object[] args)
        {
            return String.Format("subject : My module {0} Event {1} and arg[0] : {3} ", m_moduleID, m_notificationID, args[0]);
        }
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
            int userID = (int)args[0];
            //get user details from userID
            string Name = "Dummy Name";
            string designation = "Manager";
            string email = "Manager@company.com";
            string phone = "+91 12345 67890";
            string retValue = "This is footer href twitter, fb, etc";
            return retValue;
        }


        protected CMMS.RETRUNCODE sendEmail(string subject, string HTMLBody, string HTMLHeader, string HTMLFooter, string HTMLSignature, params object[] args)
        {
            CMMS.RETRUNCODE retValue = CMMS.RETRUNCODE.SUCCESS;

            //CommonUtilities.Mail.MailManager mailObj = new CommonUtilities.Mail.MailManager();

            return retValue;
        }
        public CMMS.RETRUNCODE sendEmailNotification(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, params object[] args)
        {
            //get user details from userID

            //            string strFile = LogHelper.__FILE__;
            //int line = LogHelper.__LINE__;

            //MessageBox.Show("Line " + __LINE__() + " in " + __FILE__());

            CMMS.RETRUNCODE retValue = CMMS.RETRUNCODE.FAILURE;

            string subject = getSubject(moduleID, notificationID, "stirn1", 10, 234);
            string HTMLBody = getHTMLBody(moduleID, notificationID, args);
            string HTMLHeader = getHTMLHeader(moduleID, notificationID, args);
            string HTMLFooter = getHTMLFooter(moduleID, notificationID, args);
            string HTMLSignature = getHTMLSignature(moduleID, notificationID, args);

            retValue = sendEmail(subject, HTMLBody, HTMLHeader, HTMLFooter, HTMLSignature);
            return retValue;

        }

        public static CMMS.RETRUNCODE sendNotification(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, int refID, params object[] args)
        {
            //string sqlConnection = "";
            //LogHelper lh = new LogHelper(sqlConnection);
            //lh.addErrorLog("My log string");
            //get value
            //int day = (int)CMMS.Day.Tuesday;
#if DEBUG
            //Console.WriteLine(Days.TuesDay);
            //WriteLine("Test");
#endif


            CMMS.RETRUNCODE retValue = CMMS.RETRUNCODE.FAILURE;
            CMMSNotification notificationObj;

            if (moduleID == CMMS.CMMS_Modules.JOB)     //JOB
            {
                notificationObj = new JobNotification(moduleID, notificationID);
                retValue = notificationObj.sendEmailNotification(moduleID, notificationID, refID, args);
            }
            else if (moduleID == CMMS.CMMS_Modules.PTW)    //PTW
            {
                notificationObj = new PTWNotification(moduleID, notificationID);
                retValue = notificationObj.sendEmailNotification(moduleID, notificationID, refID, args);
            }
            else if (moduleID == CMMS.CMMS_Modules.JOBCARD)    //Job card
            {
                //notificationObj = new JCNotification(moduleID, notificationID);
            }
            return retValue;
        }

    }
}
