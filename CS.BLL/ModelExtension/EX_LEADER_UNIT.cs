using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CS.BLL.Model;

namespace CS.BLL.ModelExtension
{

    public class test
    {
        public List<object> data { get; set; }
    }

    public class data
    {
        public List<object> cell { get; set; }
    }

    public class cell 
    { 
        public string value { get; set; }

        public int mergeC { get; set; }
    }

    public class EX_LEADER_UNIT
    {
        public static List<object> GetLeaderUnitReportInfo()
        {
            var unitDt = AJTM_UNIT.Instance.GetTableForExcel();
            var leaderList = AJTM_LEADER.Instance.GetListEntity();
            var leaderTypedic = AJTM_LEADER_TYPE.Instance.GetDictionary("ID", "NAME");
            var leaderUitList = AJTM_LEADER_UNIT.Instance.GetListEntity();

            //写入
            foreach (DataRow dr in unitDt.Rows)
            {
                //单位名，单位id
                var unitId = Convert.ToInt32(dr["ID"]);
                var unitName = dr["NAME"].ToString();
                var unitRange = dr["RANGE_NAME"].ToString();
                var unitLevel = dr["LEVEL_NAME"].ToString();
                //领导正职,领导副职,纪委,机关
                var typeArr = new int[] { 1, 2, 3, 4 };
                //领导正职
                IList<AJTM_LEADER.Entity> leaderNum1 = leaderList.Where(x => x.LEADER_TYPE_ID == 1 && x.UNIT_ID == unitId).ToList();
                var luNum1 = leaderUitList.Where(x => x.LEADER_TYPE_ID == 1 && x.UNIT_ID == unitId).Sum(x => x.NUM);
                var luValue1 = leaderNum1.Select(x => x.LEADER_JOB).ToList();
                var luHS1 = leaderNum1.Select(x => 1.ToString()).ToList();
                //领导副职
                IList<AJTM_LEADER.Entity> leaderNum2 = leaderList.Where(x => x.LEADER_TYPE_ID == 2 && x.UNIT_ID == unitId).ToList();
                var luNum2 = leaderUitList.Where(x => x.LEADER_TYPE_ID == 2 && x.UNIT_ID == unitId).Sum(x => x.NUM);
                var luValue2 = leaderNum2.Select(x => x.LEADER_JOB).ToList();
                var luHS2 = leaderNum2.Select(x => 1.ToString()).ToList();
                //纪委
                IList<AJTM_LEADER.Entity> leaderNum3 = leaderList.Where(x => x.LEADER_TYPE_ID == 4 && x.UNIT_ID == unitId).ToList();
                var luNum3 = leaderUitList.Where(x => x.LEADER_TYPE_ID == 4 && x.UNIT_ID == unitId).Sum(x => x.NUM);
                var luValue3 = leaderNum3.Count;
                //机关
                IList<AJTM_LEADER.Entity> leaderNum4 = leaderList.Where(x => x.LEADER_TYPE_ID == 3 && x.UNIT_ID == unitId).ToList();
                var luNum4 = leaderUitList.Where(x => x.LEADER_TYPE_ID == 3 && x.UNIT_ID == unitId).Sum(x => x.NUM);
                var luValue4 = leaderNum4.Count;
                //其他
                IList<AJTM_LEADER.Entity> leaderNUm5 = leaderList.Where(x => !typeArr.Contains(x.LEADER_TYPE_ID) && x.UNIT_ID == unitId).ToList();
                var luNum5 = leaderUitList.Where(x => !typeArr.Contains(x.LEADER_TYPE_ID) && x.UNIT_ID == unitId).Sum(x => x.NUM);
                var luValue5 = leaderNUm5.Select(x => x.LEADER_JOB).ToList();
                var luHS5 = leaderNUm5.Select(x => 1.ToString()).ToList();
                //计算最大值
                var leaderNumArr = new int[] { leaderNum1.Count(), leaderNum2.Count(), leaderNum3.Count(), leaderNum4.Count(), leaderNUm5.Count() };
                var maxCount = leaderNumArr.Max(); //
                maxCount = maxCount > 1 ? maxCount : 1;
                var mergeRCount = maxCount;    //需要合并的行
                //创建单元格
                //CreateExcelTable(_rowIndex, _rowIndex + maxCount, _colIndexMax);
                //赋值
                
                //wrCell(unitId.ToString(), mergeRCount);
                //wrCell(unitName, mergeRCount);
                //wrCell(unitRange, mergeRCount);
                //wrCell(unitLevel, mergeRCount);
                ////副县级以上
                //wrCell(string.Empty, mergeRCount);
                //wrCell(string.Empty, mergeRCount);
                ////领导正职
                //wrCell(luNum1.ToString(), mergeRCount);
                //wrCell(luValue1, mergeRCount);
                //wrCell(luHS1, mergeRCount);
                ////领导副职
                //wrCell(luNum2.ToString(), mergeRCount);
                //wrCell(luValue2, mergeRCount);
                //wrCell(luHS2, mergeRCount);
                ////纪委
                //wrCell(luNum3.ToString(), mergeRCount);
                //wrCell(luValue3.ToString(), mergeRCount);
                ////机关
                //wrCell(luNum4.ToString(), mergeRCount);
                //wrCell(luValue4.ToString(), mergeRCount);
                ////其他
                //wrCell(luNum5.ToString(), mergeRCount);
                //wrCell(luValue5, mergeRCount);
                //wrCell(luHS5, mergeRCount);
                ////备注
                //wrCell("", mergeRCount);
                ////下一行
                //lineRow(mergeRCount);
            }
            return new List<object>();
        }

    }
}
