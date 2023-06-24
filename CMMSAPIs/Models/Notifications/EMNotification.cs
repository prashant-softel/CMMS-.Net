using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMMSAPIs.Helper;
using CMMSAPIs.Models.EscalationMatrix;
using CMMSAPIs.Models.JC;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Utils;

namespace CMMSAPIs.Models.Notifications
{
    public class EMNotification : CMMSNotification
    {
        int m_Id;
        CMEscalationMatrixModel m_emObj;
        public EMNotification(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, CMEscalationMatrixModel emObj) : base(moduleID, notificationID)
        {
            m_emObj = emObj;
            m_Id = m_emObj.Status_ID;
        }

        override protected string getSubject(params object[] args)
        {
            string retValue = "It is Escalation Matrix subject";
            m_Id = m_emObj.Status_ID;

            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.EM_CREATED:
                    retValue = String.Format("Escalation Matrix Opened for <{0}>", m_emObj.Status_ID);
                    break;
                case CMMS.CMMS_Status.EM_ASSIGNED:    //updated name 
                    retValue = String.Format("Escalation Matrix <{0}> updated Escalation Matrix Updated By ", m_emObj.Status_ID, m_emObj.updatedBy);
                    break;
                case CMMS.CMMS_Status.EM_IN_PROGRESS:
                    retValue = String.Format("Escalation Matrix <{0}> is in progress ", m_emObj.Status_ID);
                    break;
                case CMMS.CMMS_Status.EM_LINKED:
                    retValue = String.Format("Escalation Matrix <{0}> linked ", m_emObj.Status_ID);
                    break;
                case CMMS.CMMS_Status.EM_CLOSED:   //approved name   permit issuer = jc  approver
                    retValue = String.Format("Escalation Matrix <{0}> Closed , Escalation Matrix Closed By Name <{1}>", m_emObj.Status_ID, m_emObj.updatedBy);
                    break;
                case CMMS.CMMS_Status.EM_CANCELLED:
                    retValue = String.Format("Escalation Matrix <{0}> Cancelled , Escalation Matrix Cancelled By Name <{1}>", m_emObj.Status_ID, m_emObj.updatedBy);
                    break;
                case CMMS.CMMS_Status.EM_DELETED:
                    retValue = String.Format("Escalation Matrix <{0}> Deleted , Escalation Matrix Deleted By Name <{1}>", m_emObj.Status_ID, m_emObj.updatedBy);
                    break;
            }
            return retValue;

        }


        override protected string getHTMLBody(params object[] args)
        {
            string retValue = "";
            int Status_ID = m_emObj.Status_ID;
            string Module = m_emObj.Module;
            long DayDifference = m_emObj.DayDifference;



            var template = getHTMLBodyTemplate(args);
            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.EM_CREATED:
                    retValue = String.Format(template, Status_ID);
                    break;
                case CMMS.CMMS_Status.EM_ASSIGNED:
                    retValue = String.Format(template, Status_ID);
                    break;
                case CMMS.CMMS_Status.EM_IN_PROGRESS:
                    retValue = String.Format(template, Status_ID);
                    break;
                case CMMS.CMMS_Status.EM_LINKED:
                    retValue = String.Format(template, Status_ID);
                    break;
                case CMMS.CMMS_Status.EM_CLOSED:
                    retValue = String.Format(template, Status_ID);
                    break;
                case CMMS.CMMS_Status.EM_CANCELLED:
                    retValue = String.Format(template, Status_ID);
                    break;
                case CMMS.CMMS_Status.EM_DELETED:
                    retValue = String.Format(template, Status_ID);
                    break;
                default:
                    break;
            }

            return retValue;
        }

        internal string getHTMLBodyTemplate(params object[] args)
        {
            string template = String.Format("<h1>This is Escalation Matrix Module {0}</h1>", m_emObj.Module);
            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.EM_CREATED:
                    template += String.Format("<p><b>Escalation Matrix status is :</b> CREATED</p> Escalation Matrix No {0}", m_emObj.Status_ID);
                    break;
                case CMMS.CMMS_Status.EM_ASSIGNED:
                    template += String.Format("<p><b>Escalation Matrix status is : ASSIGNED</p>");
                    template += String.Format("<p><b>Escalation Matrix No:</b> {0}</p><p> Escalation Matrix Updated By {1}</p>", m_emObj.Status_ID, m_emObj.updatedBy);
                    break;
                case CMMS.CMMS_Status.EM_IN_PROGRESS:
                    template += String.Format("<p><b>Escalation Matrix status is : Escalation Matrix IN PROGRESS</p>");
                    template += String.Format("<p><b>Escalation Matrix No :</b> {0}</p><p>Escalation Matrix Closed By{1}</p>", m_emObj.Status_ID, m_emObj.updatedBy);
                    break;
                case CMMS.CMMS_Status.EM_LINKED:
                    template += String.Format("<p><b>Escalation Matrix  status is : Linked </p>");
                    template += String.Format("<p>Escalation Matrix No:</b> {0}</p>", m_emObj.Status_ID);
                    break;
                case CMMS.CMMS_Status.EM_CLOSED:
                    template += String.Format("<p><b>Escalation Matrix status is : Escalation Matrix CLOSED </p>");
                    template += String.Format("<p>Escalation Matrix No {0}</p><p>Escalation Matrix CLOSED </b> {1}</p>", m_emObj.Status_ID, m_emObj.updatedBy);
                    break;
                case CMMS.CMMS_Status.EM_CANCELLED:
                    template += String.Format("<p><b>Escalation Matrix status is : Escalation Matrix CANCELLED </p>");
                    template += String.Format("<p>Escalation Matrix No {0} Escalation Matrix CANCELLED By :</b> {1}</p>", m_emObj.Status_ID, m_emObj.updatedBy);
                    break;
                case CMMS.CMMS_Status.EM_DELETED:
                    template += String.Format("<p><b>Escalation Matrix status is : Escalation Matrix DELETED </p>");
                    template += String.Format("<p>Escalation Matrix No {0} Escalation Matrix DELETED By :</b> {1}</p>", m_emObj.Status_ID, m_emObj.updatedBy);
                    break;
                default:
                    break;
            }
            return template;
        }
    }
}
