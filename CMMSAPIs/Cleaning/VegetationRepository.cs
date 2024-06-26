﻿using CMMSAPIs.Helper;
using CMMSAPIs.Models.MC;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using CMMSAPIs.BS.Facility;
using CMMSAPIs.Models.SM;
using OfficeOpenXml.VBA;
using static CMMSAPIs.Helper.CMMS;

namespace CMMSAPIs.Repositories.CleaningRepository
{

    public class VegetationRepository : CleaningRepository
    {
        private UtilsRepository _utilsRepo;
        public VegetationRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {
            _utilsRepo = new UtilsRepository(sqlDBHelper);
            moduleType = (int)cleaningType.Vegetation;
            measure = "area";
        }

        internal async Task<List<CMVegEquipmentList>> GetEquipmentList(int facilityId)
        {
            string filter = "";
            string Query = $"select id as blockId,  name as blockName from facilities ";

            if (facilityId > 0)
            {
                Query += $" where facilityId={facilityId} ";
                filter += $" and facilityId={facilityId} ";
            }

            List<CMVegEquipmentList> blocks = await Context.GetData<CMVegEquipmentList>(Query).ConfigureAwait(false);

            //string blockId = "";
            //foreach (var block in blocks)
            //{
            //    blockId += $"{block.blockId},";
            //}

            //blockId = blockId.Substring(0, blockId.Length - 1);

            string InvQuery = $"select id as invId, name as invName , blockId ,area from assets where categoryId = 2 {filter} ";
            List<CMInv> Invs = await Context.GetData<CMInv>(Query).ConfigureAwait(false);


            foreach (CMVegEquipmentList block in blocks)
            {
                foreach (CMInv inv in Invs)
                {
                    if (block.blockId == inv.blockId)
                    {
                        block.invs.Add(inv);
                    }

                }
            }
            return blocks;
        }


    }
}
