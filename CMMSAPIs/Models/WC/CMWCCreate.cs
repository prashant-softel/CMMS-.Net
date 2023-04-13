using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace CMMSAPIs.Models.WC
{
    public class CMWCCreate
    {

        public int id{ get; set; }
        public int facilityId { get; set; }
        public int equipmentCategoryId { get; set; }
        public int equipmentId { get; set; }
        public int manufacturerId { get; set; }        
        public List<int> additionalEmailEmployees { get; set; }
        public List<CMWCExternalEmail> externalEmails { get; set; }
        public string contactReferenceNumber { get; set; }
        public int costOfReplacement { get; set; }
        public int currencyId { get; set; }
        public DateTime warrantyStartAt { get; set; }
        public DateTime warrantyEndAt { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public List<IFormFile> equipmentImages { get; set; }
        public string actionByBuyer { get; set; }
        public string requestToSupplier { get; set; }
        public List<CMWCSupplierActions> supplierActions { get; set; }
        public List<IFormFile> attachments { get; set; }
        public int approverId { get; set; }
        public string equipmentSrNo { get; set; }
        public int goodsOrderId { get; set; }  
        public string affectedPart { get; set; }
        public string orderReference { get; set; }
        public string affectedSrNo { get; set; }
        public string warrantyClaimTitle { get; set; }
        public string warrantyDescription { get; set; }
        public string correctiveActionByBuyer { get; set; }
        public DateTime issuedOn { get; set; }
        public int status{ get;  set; }
        public string lastModifiedDate { get; set; }
        public string approvedBy { get; set; }
        public string approvedOn { get; internal set; }
        public string wcFacCode { get; internal set; }
        public DateTime failureTime { get; internal set; }
        public int estimatedLoss { get; internal set; }
        public int estimatedLossCurrencyId { get; internal set; } 
        public int quantity { get; set; }
    }

    public class CMWCExternalEmail 
    {
        public string name { get; set; }
        public string email { get; set; }
        public string comment { get; set; }
    }

    public class CMWCSupplierActions 
    {
        public string name { get; set; }
        public bool is_required { get; set; }
        public DateTime required_by_date { get; set; }

    }
}
