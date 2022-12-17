using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMMSAPIs.Models.SM
{
    public class SMAssetMaster
    {
        public int id { get; set; }
        public string assetsCode { get; set; }
        public string assetName { get; set; }
        public string assetType { get; set; }
        public string assetCat { get; set; }
        public string description { get; set; }

        public string unitMeasurement { get; set; }
        public string approvalRequired { get; set; }

    }
}

