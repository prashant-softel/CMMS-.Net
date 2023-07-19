using CMMSAPIs.Helper;
using CMMSAPIs.Models.MC;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using CMMSAPIs.BS.Facility;
using CMMSAPIs.Models.SM;
using static CMMSAPIs.Helper.CMMS;

namespace CMMSAPIs.Repositories.CleaningRepository
{

    public class MCRepository : CleaningRepository
    {
        private UtilsRepository _utilsRepo;
        public MCRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {
            _utilsRepo = new UtilsRepository(sqlDBHelper);
            moduleType = (int)cleaningType.ModuleCleaning;
        }  


    }
}
