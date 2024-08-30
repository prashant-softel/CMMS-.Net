using CMMSAPIs.Helper;
using CMMSAPIs.Repositories.Utils;
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

