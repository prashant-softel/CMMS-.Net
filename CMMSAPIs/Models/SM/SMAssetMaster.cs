﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMMSAPIs.Models.SM
{
    public class SMAssetMaster
    {
        public string assetsCode { get; set; }
        public string assetName { get; set; }
        public int assetType { get; set; }
        public int assetCat { get; set; }
        public string description { get; set; }
        public int unitMeasurement { get; set; }
        public int approvalRequired { get; set; }
        public int id { get; set; }


    }
}

