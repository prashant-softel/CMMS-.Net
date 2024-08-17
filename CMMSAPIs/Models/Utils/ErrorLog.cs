using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

//using System.Linq;
//using System.Threading.Tasks;

namespace CMMSAPIs.Models.Utils
{
    public class ErrorLog
    {
        private IWebHostEnvironment env;
        public ErrorLog(IWebHostEnvironment obj)
        {
            env = obj;
        }
        ~ErrorLog()
        {
        }
        //Collection of message
        public ArrayList messageArray = new ArrayList();
        int errorCount;

        class cMessage
        {
            //Int TypeOfMsg, //1 = nifo, 2= warning, 3 = Error
            public enum messageType
            {
                Information,
                Warning,
                Error,
                ImportInformation
            }
            messageType m_messageType;
            string m_sMessage;
            public messageType Get_MessageType()
            {
                return m_messageType;
            }

            public string Get_Message()
            {
                return m_sMessage;
            }


            public string Get_FormatedMessage(string sMessage)
            {
                switch (m_messageType)
                {
                    case messageType.Information:
                        sMessage = "Information : " + sMessage;
                        break;
                    case messageType.Warning:
                        sMessage = "Warning : " + sMessage;
                        break;
                    case messageType.Error:
                        sMessage = "Error : " + sMessage;
                        break;
                    case messageType.ImportInformation:
                        sMessage = ""+ sMessage;
                        break;
                }
                return sMessage;
            }


            public cMessage(cMessage.messageType mtype, string sMessage)
            {
                m_messageType = mtype;
                m_sMessage = sMessage;
            }

        }

        public void Clear()
        {
            errorCount = 0;
            messageArray.Clear();
        }

        public void SetError(string sMsg)
        {
            //Pushed or inserted at end of the collection
            //Create a message
            //Add the message to the collection
            if (!(string.IsNullOrEmpty(sMsg)))
            {
                cMessage objMessage = new cMessage(cMessage.messageType.Error, sMsg);
                messageArray.Add(objMessage);
                errorCount++;
            }
        }


        public void SetInformation(string sMsg)
        {
            //Pushed or inserted at end of the collection
            //m_Messages.Add(new CMessage(1, sMsg));
            if (!(string.IsNullOrEmpty(sMsg)))
            {
                cMessage objMessage = new cMessage(cMessage.messageType.Information, sMsg);
                messageArray.Add(objMessage);
            }
        }
        //ImportInformation
        public void SetImportInformation(string sMsg)
        {
            //Pushed or inserted at end of the collection
            //m_Messages.Add(new CMessage(1, sMsg));
            if (!(string.IsNullOrEmpty(sMsg)))
            {
                cMessage objMessage = new cMessage(cMessage.messageType.ImportInformation, sMsg);
                messageArray.Add(objMessage);
            }
        }
        public void SetWarning(string sMsg)
        {
            //Pushed or inserted at end of the collection
            //m_Messages.Add(new CMessage(2, sMsg));
            if (!(string.IsNullOrEmpty(sMsg)))
            {
                cMessage objMessage = new cMessage(cMessage.messageType.Warning, sMsg);
                messageArray.Add(objMessage);
            }
        }

        public int GetErrorCount()
        {
            return errorCount;
        }

        void ShowResults()
        {

        }

        public string SaveAsText(string csvPath)
        {
            string sMessage = "";
            StringBuilder content = new StringBuilder();
            foreach (cMessage msg in messageArray)
            {
                string indexMsg = msg.Get_Message();
                sMessage = msg.Get_FormatedMessage(indexMsg);
                content.AppendLine(sMessage);
            }
            sMessage = "Total errors <" + errorCount + ">";
            content.AppendLine(sMessage);
            string csvDir = env.WebRootPath + @"\LogFile\";
            if (!Directory.Exists(csvDir))
            {
                Directory.CreateDirectory(csvDir);
            }
            csvPath = csvDir + csvPath + ".txt";
            if (!Directory.Exists(csvPath.Substring(0, csvPath.LastIndexOf('\\')+1)))
            {
                Directory.CreateDirectory(csvPath.Substring(0, csvPath.LastIndexOf('\\') + 1));
            }
            //csvPath = @"C:\LogFile\" + csvPath; 
            File.AppendAllText(csvPath, Convert.ToString(content));
            csvPath = csvPath.Replace(@"\", @"\\");
            return csvPath;
        }

        public List<string> errorLog()
        {
            string sMessage = "";
            List<string> messageList = new List<string>();
            foreach (cMessage msg in messageArray)
            {
                string indexMsg = msg.Get_Message();
                sMessage = msg.Get_FormatedMessage(indexMsg);
                messageList.Add(sMessage);
            }
            return messageList;
        }
    }

}
