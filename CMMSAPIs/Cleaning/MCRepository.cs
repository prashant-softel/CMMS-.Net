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
        /*
        internal async new Task<List<CMMCEquipmentList>> GetEquipmentList(int facilityId)
        {
            string filter = "";

            if (facilityId > 0)
            {
                filter += $" and facilityId={facilityId} ";
            }

            string invQuery = $"select id as invId, name as invName from assets where categoryId = 2 {filter}";

            List<CMMCEquipmentList> invs = await Context.GetData<CMMCEquipmentList>(invQuery).ConfigureAwait(false);


            string smbQuery = $"select id as smbId, name as smbName , parentId, moduleQuantity from assets where categoryId = 4 {filter}";

            List<CMPlanSMB> smbs = await Context.GetData<CMPlanSMB>(smbQuery).ConfigureAwait(false);

            //List<CMSMB> invSmbs = new List<CMSMB>;

            foreach (CMMCEquipmentList inv in invs)
            {
                foreach (CMPlanSMB smb in smbs)
                {
                    if (inv.invId == smb.parentId)
                    {
                        inv?.smbs.Add(smb);
                    }
                }

            }
            return invs;
        }
        */
    }
}
