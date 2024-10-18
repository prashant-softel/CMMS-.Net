using CMMSAPIs.Helper;
using CMMSAPIs.Models.Masters;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Utils;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CMMSAPIs.Repositories.Masters
{
    public class TrainingRepository : GenericRepository

    {


        private UtilsRepository _utilsRepo;
        public static Microsoft.AspNetCore.Hosting.IWebHostEnvironment _environment;
        private MYSQLDBHelper getDB;
        private ErrorLog m_errorLog;

        public const string MA_Actual = "MA_Actual";
        public const string MA_Contractual = "MA_Contractual";
        public const string Internal_Grid = "Internal_Grid";
        public const string External_Grid = "External_Grid";
        private Dictionary<CMMS.CMMS_Modules, int> module_dict = new Dictionary<CMMS.CMMS_Modules, int>()
        {
            { CMMS.CMMS_Modules.DASHBOARD, 1 },
            { CMMS.CMMS_Modules.JOB, 2 },
            { CMMS.CMMS_Modules.PTW, 3 },
            { CMMS.CMMS_Modules.JOBCARD, 4 },
            { CMMS.CMMS_Modules.CHECKLIST_NUMBER, 5 },
            { CMMS.CMMS_Modules.CHECKPOINTS, 6 },
            { CMMS.CMMS_Modules.CHECKLIST_MAPPING, 7 },
            { CMMS.CMMS_Modules.PM_SCHEDULE, 8 },
            { CMMS.CMMS_Modules.PM_SCEHDULE_VIEW, 9 },
            { CMMS.CMMS_Modules.PM_EXECUTION, 10 },
            { CMMS.CMMS_Modules.PM_SCHEDULE_REPORT, 11 },
            { CMMS.CMMS_Modules.PM_SUMMARY, 12 },
            { CMMS.CMMS_Modules.SM_MASTER, 13 },
            { CMMS.CMMS_Modules.SM_GO, 14 },
            { CMMS.CMMS_Modules.SM_MRS, 15 },
            { CMMS.CMMS_Modules.SM_MRS_RETURN, 16 },
            { CMMS.CMMS_Modules.SM_S2S, 17 },
            { CMMS.CMMS_Modules.AUDIT_PLAN, 18 },
            { CMMS.CMMS_Modules.AUDIT_SCHEDULE, 19 },
            { CMMS.CMMS_Modules.AUDIT_SCEHDULE_VIEW, 20 },
            { CMMS.CMMS_Modules.AUDIT_EXECUTION, 21 },
            { CMMS.CMMS_Modules.AUDIT_SUMMARY, 22 },
            { CMMS.CMMS_Modules.HOTO_PLAN, 23 },
            { CMMS.CMMS_Modules.HOTO_SCHEDULE, 24 },
            { CMMS.CMMS_Modules.HOTO_SCEHDULE_VIEW, 25 },
            { CMMS.CMMS_Modules.HOTO_EXECUTION, 26 },
            { CMMS.CMMS_Modules.HOTO_SUMMARY, 27 },
            { CMMS.CMMS_Modules.INVENTORY, 28 },
            { CMMS.CMMS_Modules.WARRANTY_CLAIM, 30 },
            { CMMS.CMMS_Modules.CALIBRATION, 31 },

            { CMMS.CMMS_Modules.VEGETATION_PLAN, 33 }
        };


        public TrainingRepository(MYSQLDBHelper sqlDBHelper, IWebHostEnvironment _webHost = null) : base(sqlDBHelper)
        {
            _utilsRepo = new UtilsRepository(sqlDBHelper);
            m_errorLog = new ErrorLog(_webHost);
            getDB = sqlDBHelper;
        }
        internal static string getshortstatus(int statusID)
        {
            string retValue = "";
            switch (statusID)
            {
                case (int)CMMS.CMMS_Status.COURSE_CREATED:
                    retValue = "Waiting for Approval";
                    break;
                case (int)CMMS.CMMS_Status.COURSE_UPDATED:
                    retValue = "Waiting for Approval";
                    break;
                case (int)CMMS.CMMS_Status.COURSE_DELETED:
                    retValue = "Course Delted";
                    break;
                case (int)CMMS.CMMS_Status.COURSE_SCHEDULE:
                    retValue = "Scheduled";
                    break;
                case (int)CMMS.CMMS_Status.SCHEDULE_APPROVED:
                    retValue = "Schedule-Approved";
                    break;
                case (int)CMMS.CMMS_Status.SCHEDULE_REJECTED:
                    retValue = "Schedule-Reject";
                    break;
                case (int)CMMS.CMMS_Status.COURSE_ENDED:
                    retValue = "Schedule-End";
                    break;
            }
            return retValue;
        }
        internal async Task<CMDefaultResponse> CreateTrainingCourse(CMTrainingCourse request, int userID)
        {
            string ctq = $"INSERT INTO mis_training_course (Topic,facility_id, Description, Traning_category_id, No_Of_Days, Max_capacity, Targated_group_id, Duration_in_Minutes,Status,status_code,CreatedAt,CreatedBy) Values" +
                       $"('{request.name}',{request.facility_id},'{request.description}',{request.category_id},{request.number_of_days},{request.max_cap},{request.group_id},{request.duration},1,{(int)CMMS.CMMS_Status.COURSE_CREATED},'{UtilsRepository.GetUTCTime()}',{userID}) ; " +
                       $"SELECT LAST_INSERT_ID();";
            DataTable dt = await Context.FetchData(ctq).ConfigureAwait(false);
            int id = Convert.ToInt32(dt.Rows[0][0]);

            if (request.uploadfile_ids != null && request.uploadfile_ids.Count > 0)
            {
                foreach (int data in request.uploadfile_ids)
                {

                    string qryuploadFiles = $"UPDATE uploadedfiles SET facility_id = {request.facility_id},description='{request.description}', module_type={(int)CMMS.CMMS_Modules.TRAINING_COURSE},module_ref_id={id} where id = {data}";
                    await Context.ExecuteNonQry<int>(qryuploadFiles).ConfigureAwait(false);
                }
            }

            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Course Added Successfully.");
        }

        internal async Task<List<CMTrainingCourse>> GetCourseList(int facility_id, DateTime from_date, DateTime to_date)
        {
            string From = Convert.ToDateTime(from_date).ToString("yyyy-MM-dd");
            string to = Convert.ToDateTime(to_date).ToString("yyyy-MM-dd");
            string gcl = $" SELECT c.id as id, c.facility_id as facility_id,c.Topic as name, c.Description as description ,c.Traning_category_id as category_id,cs.name as Traning_category , c.No_Of_Days as number_of_days, c.Max_capacity as max_cap ," +
                $" c.Targated_group_id as group_id ,tg.name as Targated_group, " +
                $" c.Duration_in_Minutes as duration ,c.status_code as status,c.CreatedAt, c.CreatedBy  , c.UpdatedAt  , c.UpdatedBy from mis_training_course as c " +
                $" Left join mis_course_category as cs on cs.id=c.Traning_category_id " +
                $" Left join mis_targeted_group  as tg on tg.id=c.Targated_group_id " +
                $" where c.facility_id={facility_id} and c.Status=1 or CreatedAt='{From}' or CreatedAt='{to}'  ";
            List<CMTrainingCourse> result = await Context.GetData<CMTrainingCourse>(gcl).ConfigureAwait(false);

            foreach (var item in result)
            {
                string shortstatus = getshortstatus(item.status);
                item.short_status = shortstatus;
            }
            return result;
        }
        internal async Task<List<CMTrainingCourse>> GetCourseDetailById(int ids)
        {
            string gcl = $" SELECT c.id as Id, c.facility_id as facility_id,c.Topic as name, c.Description as description ," +
                         $" c.Traning_category_id as category_id ,cs.name as Traning_category , c.No_Of_Days as number_of_days, c.Max_capacity as max_cap, " +
                         $" c.Targated_group_id as group_id,tg.name as Targated_group, c.Duration_in_Minutes as duration ,c.status_code as status, c.CreatedAt  , c.CreatedBy  , c.UpdatedAt  , c.UpdatedBy from mis_training_course as c " +
                         $" Left join mis_course_category as cs on cs.id=c.Traning_category_id " +
                         $" Left join mis_targeted_group  as tg on tg.id=c.Targated_group_id " +
                         $" where c.id={ids} and c.Status=1 ";
            List<CMTrainingCourse> result = await Context.GetData<CMTrainingCourse>(gcl).ConfigureAwait(false);

            string myQuery17 = "SELECT cor.id as id, file_path as fileName,U.description FROM uploadedfiles AS U " +
                             "Left JOIN mis_training_course as cor on cor.id= U.module_ref_id" +
                             " where module_ref_id =" + ids + " and U.module_type = " + (int)CMMS.CMMS_Modules.TRAINING_COURSE + ";";
            List<CMTRAININGFILE> _fileUpload = await Context.GetData<CMTRAININGFILE>(myQuery17).ConfigureAwait(false);
            result[0].ImageDetails = _fileUpload;
            foreach (var item in result)
            {
                string shortstatus = getshortstatus(item.status);
                item.short_status = shortstatus;
            }
            return result;
        }
        internal async Task<CMDefaultResponse> UpdateCourseList(CMTrainingCourse request, int userID)
        {
            string ctq = $"UPDATE mis_training_course SET " +
             $"Topic = '{request.name}', " +
             $"facility_id = {request.facility_id}, " +
             $"Description = '{request.description}', " +
             $"Traning_category_id= {request.category_id}, " +
             $"No_Of_Days = {request.number_of_days}, " +
             $"Max_capacity = {request.max_cap}, " +
             $"Targated_group_id = {request.group_id}, " +
             $"Duration_in_Minutes = {request.duration}, " +
             $"Status = 1, " +
             $"status_code={(int)CMMS.CMMS_Status.COURSE_UPDATED}, " +
             $"UpdatedAt = '{UtilsRepository.GetUTCTime()}', " +
             $"UpdatedBy = {userID} " +
             $"WHERE  id={request.Id};";
            await Context.ExecuteNonQry<int>(ctq).ConfigureAwait(false);
            if (request.uploadfile_ids != null && request.uploadfile_ids.Count > 0)
            {
                foreach (int data in request.uploadfile_ids)
                {

                    string qryuploadFiles = $"UPDATE uploadedfiles SET facility_id = {request.facility_id}, module_type={(int)CMMS.CMMS_Modules.TRAINING_COURSE},module_ref_id={request.Id} where id = {data}";
                    await Context.ExecuteNonQry<int>(qryuploadFiles).ConfigureAwait(false);
                }
            }

            return new CMDefaultResponse(request.Id, CMMS.RETRUNSTATUS.SUCCESS, "Course Updated Successfully.");
        }
        internal async Task<CMDefaultResponse> DeleteCourseList(int id, int userID)
        {
            string deleteQry = $"UPDATE mis_training_course " +
               $" SET Status = 0 , UpdatedBy = {userID} , UpdatedAt = '{UtilsRepository.GetUTCTime()}',status_code={(int)CMMS.CMMS_Status.COURSE_DELETED} WHERE id = {id};";
            await Context.ExecuteNonQry<int>(deleteQry).ConfigureAwait(false);
            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Course Deleted Successfully.");
        }

        internal async Task<CMDefaultResponse> CreateScheduleCourse(TrainingSchedule request, int userID)
        {
            int Exterid = 0;
            string crtqry = $"INSERT INTO mis_training_schedule ( courseId,facility_id, course_name, ScheduleDate, TraingCompany, Trainer,Mode, Comment, Venue, hfeEmployeeId,status_code, CreatedAt, CreatedBy) values " +
                            $"({request.CourseId},{request.facility_id},'{request.CourseName}','{request.Date_of_training}',{request.TrainingAgencyId},'{request.TrainerName}','{request.Mode}','{request.Comment}','{request.Venue}',{request.HfeEmployeeId},{(int)CMMS.CMMS_Status.COURSE_SCHEDULE},'{UtilsRepository.GetUTCTime()}',{userID}) ; " +
                            $"SELECT LAST_INSERT_ID();";
            DataTable dt = await Context.FetchData(crtqry).ConfigureAwait(false);
            int schid = Convert.ToInt32(dt.Rows[0][0]);

            foreach (ExternalEmployee items in request.externalEmployees)
            {
                string externalsc = $"INSERT INTO mis_visitor_details (Schid,Name, Email, Mobile, Company,CreatedAt,CreatedBy) values " +
                                    $"({schid},'{items.employeeName}','{items.employeeEmail}','{items.employeeNumber}','{items.companyName}','{UtilsRepository.GetUTCTime()}',{userID}) ; " +
                                    $"SELECT LAST_INSERT_ID();";
                DataTable dt1 = await Context.FetchData(externalsc).ConfigureAwait(false);
                Exterid = Convert.ToInt32(dt1.Rows[0][0]);
            }

            foreach (InternalEmployee item in request.internalEmployees)
            {
                string inviteschdule = $"INSERT INTO mis_schedule_invites ( Schid, employee_id, Visitor_id, Rsvp, designation, Attendend, CreatedAt, CreatedBy) values " +
                                       $"('{schid}', {item.EmpId}, {Exterid}, 0, '{item.empDesignation}', '{item.Attendence}', '{UtilsRepository.GetUTCTime()}', {userID}) ; " +
                                       $"SELECT LAST_INSERT_ID();";
                DataTable dt2 = await Context.FetchData(inviteschdule).ConfigureAwait(false);
                int schdinviid = Convert.ToInt32(dt2.Rows[0][0]);
            }

            if (request.uploadfile_ids != null && request.uploadfile_ids.Count > 0)
            {
                foreach (int data in request.uploadfile_ids)
                {
                    string qryuploadFiles = $"UPDATE uploadedfiles SET facility_id = {request.facility_id}, module_type = {(int)CMMS.CMMS_Modules.TRAINNING_SCHEDULE}, module_ref_id = {schid} where id = {data}";
                    await Context.ExecuteNonQry<int>(qryuploadFiles).ConfigureAwait(false);
                }
            }

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.TRAINNING_SCHEDULE, schid, 0, 0, request.Comment, CMMS.CMMS_Status.COURSE_SCHEDULE);
            return new CMDefaultResponse(schid, CMMS.RETRUNSTATUS.SUCCESS, "Course Schedule Successfully Created");
        }
        /*
        protected async Task<CMDefaultResponse> sendEmail(string subject, string body, string emailTo)
        {
            CMMailSettings _settings = new CMMailSettings();
            var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            _settings.Mail = MyConfig.GetValue<string>("MailSettings:Mail");
            _settings.DisplayName = MyConfig.GetValue<string>("MailSettings:DisplayName");
            _settings.Password = MyConfig.GetValue<string>("MailSettings:Password");
            _settings.Host = MyConfig.GetValue<string>("MailSettings:Host");
            _settings.Port = MyConfig.GetValue<int>("MailSettings:Port");

            List<string> AddCc = new List<string>();

            DateTime today = DateTime.Now;

            string emailBody = "<div style='width:100%; padding:0.5rem; text-align:center;'><span><img src='https://i.ibb.co/FD60YSY/hfe.png' alt='hfe' border='0'></span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b style='color:#31576D; padding:0.5rem;'><i>" + today.ToString("dddd") + " , " + today.ToString("dd-MMM-yyyy") + "</i></b></div><hr><br><div style='text-align:center;'>";

            emailBody += body;
            emailBody += "</div><br><div><p style='text-align:center;'>visit:<a href='https://i.ibb.co/FD60YSY/hfe.png'> http://cmms_726897com</a></p></div><br><p style='padding:0.5rem; '><b>Disclaimer:</b> The information contained in this electronic message and any attachments to this message are intended for the exclusive use of the addressee(s) and may contain proprietary, confidential or privileged information. If you are not the intended recipient, you should not disseminate, distribute, print or copy this e-mail. Please notify the sender immediately and destroy all copies of this message and any attachments. Although the company has taken reasonable precautions to ensure no viruses are present in this email, the company cannot accept responsibility for any loss or damage arising from the use of this email or attachments.</p>";


            CMMailRequest request = new CMMailRequest
            {
                emailtraingn = emailTo,
                CcEmail = AddCc,
                Subject = subject,
                Body = emailBody,
            };

            try
            {
                var res = await MailService.SendEmailAsync(request, _settings);
            }
            catch (Exception e)
            {
                string msg = e.Message;
            }

            return new CMDefaultResponse(1, CMMS.RETRUNSTATUS.SUCCESS, "mail sent");
        }
        private string GenerateInvitationLink(int schid, int empId)
        {
            // Assuming you have a method to generate a unique URL for the invitation
            return $"https://mail.google.com/rsvp?schid={schid}&empId={empId}";
        }

        // Endpoint to handle RSVP responses (This would be part of your API controller)

        public async Task<string> HandleRsvpResponse(int schid, int empId, int response)
        {
            // Update RSVP status in the database
            string updateRsvpQry = $"UPDATE mis_schedule_invites SET Rsvp = {response} WHERE Schid = {schid} AND employee_id = {empId}";
            await Context.ExecuteNonQry<int>(updateRsvpQry).ConfigureAwait(false);
            return "RSVP response recorded successfully";
        }
        */
        internal async Task<List<GETSCHEDULE>> GetScheduleCourseList(int facility_id, DateTime from_date, DateTime to_date)
        {
            string getsch = $"SELECT  Schid as ScheduleID ,courseId as  courseID ,mis_training_schedule.status_code as status_code, course_name, " +
                            $"ScheduleDate, case When  TraingCompany=1 then 'Hero Future Energies' else 'Softel Technologies' End  as TrainingCompany , " +
                            $"Trainer, Mode as mode,  Venue , " +
                            $" c.Topic,c.Traning_category_id, c.No_Of_Days, c.Targated_group_id, " +
                            $"c.Duration_in_Minutes ,cc.name as course_Category ,tg.name as targeted_group from  mis_training_schedule " +
                            $"  LEFT JOIN mis_training_course as c on c.id = mis_training_schedule.courseId " +
                            $"  LEFT JOIN mis_course_category cc on cc.id = c.Traning_category_id " +
                            $" LEFT JOIN mis_targeted_group as tg on tg.id = c.Targated_group_id " +
                            $" where mis_training_schedule.facility_id={facility_id} ";
            List<GETSCHEDULE> Schedules = await Context.GetData<GETSCHEDULE>(getsch).ConfigureAwait(false);
            foreach (var item in Schedules)
            {
                string shortstatus = getshortstatus(item.status_code);
                item.short_status = shortstatus;
            }
            return Schedules;
        }
        internal async Task<List<GETSCHEDULEDETAIL>> GetScheduleCourseDetail(int schedule_id)
        {

            string getsch = $"SELECT  Schid as ScheduleID,mis_training_schedule.status_code,mis_training_schedule.facility_id ,courseId as courseID ,mis_training_schedule.hfeEmployeeId, course_name as training_course ,DATE_FORMAT(ScheduleDate,'%Y-%m-%d') as Date_of_Trainig,case When  TraingCompany=1 then 'Hero Future Energies' else 'Softel Technologies' End as Training_Agency , Trainer,concat(u.firstName,u.lastName) as HFE_Epmloyee, Mode as mode,  Venue,  mis_training_schedule.status_code as status  " +
                $" from  mis_training_schedule " +
                $"  LEFT JOIN mis_training_course as c on c.id = mis_training_schedule.courseId " +
                $"  LEFT JOIN mis_course_category cc on cc.id = c.Traning_category_id " +
                 $" LEFT JOIN users as u on u.id =mis_training_schedule.hfeEmployeeId  " +
                $" LEFT JOIN mis_targeted_group as tg on tg.id = c.Targated_group_id " +
                $" where mis_training_schedule.Schid={schedule_id} ";
            List<GETSCHEDULEDETAIL> Schedules = await Context.GetData<GETSCHEDULEDETAIL>(getsch).ConfigureAwait(false);


            string Exteremployee = $"SELECT id , Name as employeeName,  Email as employeeEmail, Mobile as employeeNumber,Company as  companyName,Attendend,notes, Rsvp from mis_visitor_details where Schid={schedule_id}";
            List<ExternalEmployee> externalemployee = await Context.GetData<ExternalEmployee>(Exteremployee).ConfigureAwait(false);

            string interemployee = $" SELECT Schid as schid, employee_id," +
                $"Visitor_id, Rsvp, notes,u.mobileNumber as mobile, " +
                $"concat(u.firstName, ' ', u.lastname) as name,u.loginId as email, designation from " +
                $"mis_schedule_invites " +
                $"LEFT JOIN users u on u.id = mis_schedule_invites.employee_id " +
                $"where Schid={schedule_id}";
            List<INTERNALEMPLOYEES> internalemployee = await Context.GetData<INTERNALEMPLOYEES>(interemployee).ConfigureAwait(false);
            string myQuery17 = "SELECT U.id as id, file_path as fileName,U.description FROM uploadedfiles AS U " +
                            "Left JOIN mis_training_schedule as cor on cor.Schid= U.module_ref_id" +
                            " where module_ref_id =" + schedule_id + " and U.module_type = " + (int)CMMS.CMMS_Modules.TRAINNING_SCHEDULE + ";";
            List<CMTRAININGFILE> _fileUpload = await Context.GetData<CMTRAININGFILE>(myQuery17).ConfigureAwait(false);
            if (externalemployee.Count > 0)
            {
                Schedules[0].external_employee = externalemployee;
            }
            if (internalemployee.Count > 0)
            {
                Schedules[0].internal_employee = internalemployee;
            }
            if (_fileUpload.Count > 0)
            {
                Schedules[0].uploadfile_ids = _fileUpload; ;
            }
            foreach (var item in Schedules)
            {
                string shortstatus = getshortstatus(item.status);
                item.short_status = shortstatus;
            }
            return Schedules;
        }

        internal async Task<CMDefaultResponse> ExecuteScheduleCourse(GETSCHEDULEDETAIL request, int userid)
        {

            string updateQry = $"UPDATE mis_training_schedule SET status_code = {(int)CMMS.CMMS_Status.COURSE_ENDED},Course_name='{request.course_name}' ,  " +
                    $" Mode='{request.Mode.Replace("+", "-")}',Trainer='{request.Trainer}', hfeEmployeeId = {request.hfeEmployeeId},Venue = '{request.Venue}', " +
                    $"facility_id={request.facility_id},Course_name='{request.Training_course}', UpdateBy = {userid},UpdatedAt = '{UtilsRepository.GetUTCTime()}' " +
                    $"WHERE Schid = {request.ScheduleID};";


            await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);



            foreach (INTERNALEMPLOYEES item in request.internal_employee)
            {


                string execute = $"update mis_schedule_invites set Attendend={item.attendend},notes='{item.notes}',  " +
                                    $"Rsvp='{item.rsvp}' where employee_id={item.employee_id}";
                await Context.ExecuteNonQry<int>(execute).ConfigureAwait(false);
            }
            foreach (ExternalEmployee item1 in request.external_employee)
            {
                //id, Schid, Name, Email, Mobile, Company, Address, Rsvp, notes, Attendend, CreatedBy, CreatedAt
                string execute2 = $"update mis_visitor_details set Attendend={item1.attendend},notes='{item1.notes}' ,  " +
                                  $" Rsvp='{item1.rsvps}', Email='{item1.email}' , " +
                                   $"Mobile ={item1.mobile},Name='{item1.name}' " +
                                  $"where id={item1.employee_id}";
                await Context.ExecuteNonQry<int>(execute2).ConfigureAwait(false);
            }
            return new CMDefaultResponse(request.ScheduleID, CMMS.RETRUNSTATUS.SUCCESS, "Schedule Coures Executed");
        }
        //Master OF Trainig Category
        internal async Task<List<CMTRAININGCATE>> GetTrainingCategorty()
        {
            string myQuery = "SELECT id, name, description as description,status as status FROM mis_course_category where status=1 ";
            List<CMTRAININGCATE> Data = await Context.GetData<CMTRAININGCATE>(myQuery).ConfigureAwait(false);
            return Data;
        }
        internal async Task<CMDefaultResponse> CreateTrainingCategorty(CMTRAININGCATE request)
        {

            String myqry = $"INSERT INTO mis_course_category(name, description, status) VALUES " +
                                $"('{request.name}','{request.description}',1); " +
                                 $"SELECT LAST_INSERT_ID(); ";
            DataTable dt = await Context.FetchData(myqry).ConfigureAwait(false);
            int id = Convert.ToInt32(dt.Rows[0][0]);
            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Training Category Added");
        }
        internal async Task<CMDefaultResponse> UpdateTrainingCategorty(CMTRAININGCATE request)
        {
            string updateQry = "UPDATE mis_course_category  SET ";
            updateQry += $"name = '{request.name}', " +
                 $"description = '{request.description}', " +
                 $"status = 1 " +
                $"WHERE id = {request.id};";
            await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
            return new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Training Category Updated");

        }
        internal async Task<CMDefaultResponse> DeleteTrainingCategorty(int id)
        {
            string delqry = $"UPDATE mis_course_category  SET status=0  where id={id};";
            await Context.ExecuteNonQry<int>(delqry).ConfigureAwait(false);
            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Training Category deleted");
        }

        internal async Task<List<CMTRAININGCATE>> GetTargetedGroupList()
        {
            string myQuery1 = "SELECT id, name, description as  description, status FROM mis_targeted_group where status=1 ";
            List<CMTRAININGCATE> Data = await Context.GetData<CMTRAININGCATE>(myQuery1).ConfigureAwait(false);
            return Data;
        }

        internal async Task<CMDefaultResponse> CreateTargetedGroup(CMTRAININGCATE request)
        {
            String myqry1 = $"INSERT INTO mis_targeted_group(name, description, status) VALUES " +
                               $"('{request.name}','{request.description}',1); " +
                                $"SELECT LAST_INSERT_ID(); ";
            DataTable dt = await Context.FetchData(myqry1).ConfigureAwait(false);
            int id = Convert.ToInt32(dt.Rows[0][0]);
            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Target Group  Add Successfully");
        }

        internal async Task<CMDefaultResponse> UpdateTargetedGroup(CMTRAININGCATE request)
        {
            string updateQry1 = "UPDATE mis_targeted_group  SET ";
            updateQry1 += $"name = '{request.name}', " +
                 $"description = '{request.description}', " +
                 $"status = 1 " +
                $"WHERE id = {request.id};";
            await Context.ExecuteNonQry<int>(updateQry1).ConfigureAwait(false);
            return new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Target Group Update Successfully");
        }

        internal async Task<CMDefaultResponse> DeleteTargetedGroup(int id)
        {
            string delqry1 = $"UPDATE mis_targeted_group  SET status=0  where id={id};";
            await Context.ExecuteNonQry<int>(delqry1).ConfigureAwait(false);
            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Target Group delete Successfully");
        }


        internal async Task<List<CMTrainingSummary>> GetTrainingReportByCategory(int facility_id, DateTime fromDate, DateTime toDate)
        {


            string getsch = $" SELECT ts.Schid AS ScheduleID, ts.courseId, ts.ScheduleDate, MONTHNAME(ts.ScheduleDate) AS month_name, " +
            $" c.Traning_category_id, c.No_Of_Days, c.Duration_in_Minutes, cc.name AS mis_course_category, ts.status_code AS status_code " +
            $" FROM mis_training_schedule ts " +
            $" LEFT JOIN mis_training_course c ON c.id = ts.courseId " +
            $" LEFT JOIN mis_course_category cc ON cc.id = c.Traning_category_id  " +
            $" LEFT JOIN mis_targeted_group tg ON tg.id = c.Targated_group_id " +
            $" LEFT JOIN mis_training_course status ON status.id = ts.CourseId " +
            $" WHERE ts.facility_id = {facility_id} " +
            $" AND status.Status = 1 ";

            if (fromDate != DateTime.MinValue && toDate != DateTime.MinValue)
            {
                getsch += $" AND ts.ScheduleDate BETWEEN '{fromDate:yyyy-MM-dd}' AND '{toDate:yyyy-MM-dd}'";
            }




            List<GETSCHEDULE> trainings = await Context.GetData<GETSCHEDULE>(getsch).ConfigureAwait(false);

            Dictionary<int, CMTrainingSummary> monthlyTrainingSummary = new Dictionary<int, CMTrainingSummary>();
            //Dictonary <category_id int,Count int>
            //Loop here of above list

            decimal duration_in_minutes = 0;
            if (trainings != null)
            {
                foreach (var item in trainings)
                {
                    bool isScheduled = false;
                    bool isEnded = false;

                    bool mockDrillManHours = false;
                    bool inductionManHours = false;

                    DateTime d_o_t = (DateTime)item.ScheduleDate;
                    string date_of_training = d_o_t.ToString("yyyy-MM-dd");
                    string strMonth = date_of_training.Substring(5, 2);
                    int month = int.Parse(strMonth);
                    string mn = item.month_name;

                    string strYear = date_of_training.Substring(0, 4);
                    int year = int.Parse(strYear);
                    DateTime date_of_observation_Date = DateTime.ParseExact(date_of_training, "yyyy-MM-dd", null);

                    CMTrainingSummary forMonth;


                    if (!monthlyTrainingSummary.TryGetValue(month, out forMonth))
                    {

                        forMonth = new CMTrainingSummary(month, mn, year);
                        monthlyTrainingSummary.Add(month, forMonth);
                    }

                    forMonth.created++;
                    int num = item.status_code;
                    if (num == (int)CMMS.CMMS_Status.COURSE_SCHEDULE)
                    {
                        forMonth.scheduled++;
                        isScheduled = true;
                    }
                    else if (num == (int)CMMS.CMMS_Status.COURSE_ENDED)
                    {
                        forMonth.ended++;
                        isEnded = true;
                    }

                    switch (item.Traning_category_id)
                    {
                        case 1:
                            forMonth.hfe_training++;
                            break;

                        case 2:
                            forMonth.hfe_mockDrill++;
                            mockDrillManHours = true;
                            break;

                        case 3:
                            forMonth.induction++;
                            inductionManHours = true;
                            break;

                        case 4:
                            forMonth.special_training++;
                            break;

                        case 5:
                            forMonth.special_mockDrill++;
                            break;
                    }

                    duration_in_minutes = item.Duration_in_Minutes;

                    decimal hours = duration_in_minutes / 60;

                    decimal decimal_hours = Decimal.Round(hours, 2);

                    forMonth.manHours += decimal_hours;

                    if (mockDrillManHours == false && inductionManHours == false)
                    {
                        forMonth.total_man_hours_excluding_mock_and_induction += decimal_hours;
                    }

                    if (mockDrillManHours == false && inductionManHours == false)
                    {
                        forMonth.total_training_hours_excluding_mock_and_induction++;
                    }

                }

            }

            return monthlyTrainingSummary.Values.ToList();
        }


        internal async Task<CMDefaultResponse> RejectScheduleCourse(CMApproval request, int userid)
        {
            string approve = $"update  mis_training_schedule set RejectedBy={userid} ," +
               $" RejectedAt='{UtilsRepository.GetUTCTime()}', status_code={(int)CMMS.CMMS_Status.SCHEDULE_REJECTED} " +
               $" where Schid={request.id}";
            int id = await Context.CheckGetData(approve).ConfigureAwait(false);
            return new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Schedule Rejected");

        }

        internal async Task<CMDefaultResponse> ApproveScheduleCourse(CMApproval request, int userID)
        {

            string approve = $"update  mis_training_schedule set approvedby={userID} ," +
                $" approvedat='{UtilsRepository.GetUTCTime()}', status_code={(int)CMMS.CMMS_Status.SCHEDULE_APPROVED} " +
                $" where Schid={request.id}";
            int id = await Context.CheckGetData(approve).ConfigureAwait(false);
            return new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Schedule Approved");
        }
    }

}


