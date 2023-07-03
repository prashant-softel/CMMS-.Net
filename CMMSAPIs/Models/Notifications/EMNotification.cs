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

            return retValue;

        }


        override protected string getHTMLBody(params object[] args)
        {
            string retValue = "";
            int Status_ID = m_emObj.Status_ID;
            string Module = m_emObj.Module;
            long DayDifference = m_emObj.DayDifference;



            var template = getHTMLBodyTemplate(args);
            
            return retValue;
        }

        internal string getHTMLBodyTemplate(params object[] args)
        {
            string template = String.Format("<h1>This is Escalation Matrix Module {0}</h1>", m_emObj.Module);
      
            return template;
        }
    }
}
