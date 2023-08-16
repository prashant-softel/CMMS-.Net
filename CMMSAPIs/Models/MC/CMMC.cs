﻿using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq.Expressions;

namespace CMMSAPIs.Models.MC
{
    public class CMMC
    {
        public int id { get; set; }
        public string scheduler_number { get; set; }
        public int block_id { get; set; }
        public string block_name { get; set; }
        public int asset_category_id { get; set; }
        public string asset_category_name { get; set; }
        public int total_module { get; set; }
        public int operational_year { get; set; }
        public int annual_cycle_no { get; set; }
        public int duration_day { get; set; }
        public string status { get; set; }
        public int created_by_id { get; set; }
        public string created_by_name { get; set; }
        public DateTime created_at { get; set; }

    }

    //public class CMMCPlanList: CMMCPlan
    //{
    //    public int planId { get; set; } 
    //    public int noOfDays { get; set; } 
    //    public string status { get; set; } 
    //}

    //MCStatus
    //

    public class CMMCPlan
    {
        public UInt64 id { get; set; }
        public int planId { get; set; }
        public int facilityId { get; set; }
        //public int blockId { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public int frequencyId { get; set; }
        public string frequency { get; set; }
        public int noOfCleaningDays { get; set; }
        public int createdById { get; set; }
        public string createdBy { get; set; }
        public DateTime createdAt { get; set; }
        public int approvedById { get; set; }
        public string approvedBy { get; set; }
        public DateTime approvedAt { get; set; }
        public string deletedBy { get; set; }
        public int status { get; set; }
        public string status_short { get; set; }
        public string status_long { get; set; }

        public bool scheduleAdded = false;
        public List<CMMCSchedule> schedules { get; set; }

    }
    public class CMMCPlanSummary
    {
        public int save { get; set; }
        public int planId { get; set; }
        public List<CMMCPlanScheduleSummary> schedules { get; set; }

    }
    public class CMMCPlanScheduleSummary
    {

        // public UInt64 id { get; set; }
        public int scheduleId { get; set; }
        public long cleaningDay { get; set; }  //First, second etc day
        public string cleaningType { get; set; }  //First, second etc day
        // public string cleaningTypeName { get; set; }  //First, second etc day
        // public Int64 totalBlocks { get; set; }
        public Int64 totalInvs { get; set; }
        public Int64 totalSmbs { get; set; }
        public decimal totalModules { get; set; }
        //public decimal ScheduledArea { get; set; }
        // public DateTime plannedDate { get; set; }
        // public string status_short { get; set; }
        public List<CMMCEquipment> equipments { get; set; }

    }
    public class CMMCEquipment
    {
        public int id { get; set; }
        public string equipName { get; set; }
        public int moduleQuantity { get; set; }
        public int cleaningDay { get; set; }

    }
    public class CMMCEquipmentDetails
    {
        public int id { get; set; }
        public string equipmentName { get; set; }
        public int parentId { get; set; }
        public int moduleQuantity { get; set; }
        public int noOfPlanDay { get; set; }
        public DateTime scheduledCleaningDate { get; set; }
        public DateTime actualCleaningDate { get; set; }
        public string short_status { get; set; } //Cleaned or Abandoned

        public bool isCleaned = false;//Cleaned or Abandoned
        public bool isAbandoned = false; //Cleaned or Abandoned
        public int updatedById { get; set; }
        public int grassCuttingArea { get; set; }
    }

    class CMMCSetEquipment
    {
        public int id { get; set; }
        public int day { get; set; }
    }

    public class CMMCSchedule
    {

        public UInt64 id { get; set; }
        public int scheduleId { get; set; }
        public int executionId { get; set; }
        public int cleaningDay { get; set; }  //First, second etc day
        public int cleaningType { get; set; }  //First, second etc day
        public string cleaningTypeName { get; set; }  //First, second etc day
        public Int64 blocks { get; set; }
        public Int64 Invs { get; set; }
        public Int64 smbs { get; set; }
        public decimal ScheduledModules { get; set; }
        public decimal ScheduledArea { get; set; }
        public DateTime plannedDate { get; set; }
        public int status { get; set; }
        public string status_short { get; set; }
        public List<CMMCEquipmentDetails> equipments { get; set; }

    }


    public class CMMCExecutionSchedule 
    {
        public UInt64 id { get; set; }
        public int scheduleId { get; set; }
        public int executionId { get; set; }
        public int cleaningDay { get; set; }  //First, second etc day
        //public int cleaningType { get; set; }  //First, second etc day
        public string cleaningTypeName { get; set; }
        public decimal ScheduledModules { get; set; }
        public decimal cleanedModules { get; set; }
        public decimal abandonedModules { get; set; }
        public decimal pendingModules { get; set; }
        public int waterUsed { get; set; }
        public DateTime execution_date { get; set; }
        public string remark { get; set; }
        public string status_short { get; set; }
        public List<CMMCExecutionEquipment> equipments { get; set; }


    }

    public class CMMCExecutionEquipment
    {
        public int id { get; set; }
        public string equipmentName { get; set; }
        public int parentId { get; set; }
        public int cleaningDay { get; set; }
        public int moduleQuantity { get; set; }
        public string short_status { get; set; } //Cleaned or Abandoned

        //public bool isCleaned = false;//Cleaned or Abandoned
        //public bool isAbandoned = false; //Cleaned or Abandoned
        //public int updatedById { get; set; }
        //0public int grassCuttingArea { get; set; }
    }

    public class CMMCExecution
    {
        public UInt64 id { get; set; }
        public int executionId { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string frequency { get; set; }
        //public DateTime startDate { get; set; }
        public int noOfDays { get; set; }
        public string plannedBy { get; set; }
        public DateTime plannedAt { get; set; }
        public string startedBy { get; set; }
        public DateTime startedAt { get; set; }
        public string abandonedBy { get; set; }
        public DateTime abandonedAt { get; set; }
        public int status { get; set; }     //Completed
        public string status_short { get; set; }
        public string status_long { get; set; }

        // public string siteName { get; set; }
        //public List<CMMCEquipmentDetails> smbIds;
        public List<CMMCExecutionSchedule> schedules { get; set; }
    }

    public class CMMCPlanFilter
    {
        public string facility_id { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public bool Draft { get; set; }
        public bool Submitted { get; set; }
        public bool Rejected { get; set; }
        public bool Approved { get; set; }
        public bool Deleted { get; set; }

    }
    public class CMMCExecutionFilter
    {
        public int facility_id { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public bool isCompleted { get; set; }
        public bool isOpen { get; set; }
        public bool isAbandoned { get; set; }
        public bool isRejected { get; set; }
        public bool isInProgress { get; set; }
    }

    //Delete

    //public class CMMCPlan : CMMC
    //{
    //    public List<CMMCPlanList> plan_list { get; set; }
    //}
    public class CMMCTaskList
    {
        public int id { get; set; }
        public int planId { get; set; }
        public string responsibility { get; set; }
        public string frequency { get; set; }
        public int noOfDays { get; set; }
        public DateTime startDate { get; set; }
        public DateTime doneDate { get; set; }
        public string status_short { get; set; }
        public int water_used { get; set; }
    }
    public class CMMCExecutionList
    {
        public int id { get; set; }
        public int schedule_id { get; set; }
        public int completed_quantity { get; set; }
        public int water_used { get; set; }
    }

    public class CMMCEquipmentList
    {
        public int invId { get; set; }
        public string invName { get; set; }
        public int moduleQuantity { get; set; }
        public List<CMSMB> smbs { get; set; } = new List<CMSMB>();


        // public int area { get; set; }
    }
    public class CMSMB
    {
        public int parentId { get; set; }
        public int smbId { get; set; }
        public string smbName { get; set; }
        public int moduleQuantity { get; set; }

    }
    public class CMVegEquipmentList
    {
        public int blockId { get; set; }
        public string blockName { get; set; }
        public int area { get; set; }
        public List<CMInv> invs { get; set; } = new List<CMInv>();

    }
    public class CMInv
    {
        public int blockId { get; set; }
        public int invId { get; set; }
        public string invName { get; set; }
        public int area { get; set; }

    }

}
