using System;
using System.Collections.Generic;
using System.Data;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CS.BLL.Model;

namespace CS.BLL.ModelExtension
{


    public class EX_LEADER_UNIT
    {

        public class UNIT_SINGLE
        {
            /// <summary>
            /// 多少行
            /// </summary>
            public object row { get; set; }
        }

        public class UNIT_ROW
        {
            /// <summary>
            /// 多少列
            /// </summary>
            public List<UNIT_CELL> cell { get; set; }
        }

        public class UNIT_CELL
        {
            /// <summary>
            /// 值
            /// </summary>
            public object value { get; set; }
            /// <summary>
            /// 合并行
            /// </summary>
            public int rowspan { get; set; }
        }



        public List<UNIT_ROW> GetLeaderUnitReportInfo()
        {
            var unitDt = AJTM_UNIT.Instance.GetTableForExcel();
            var leaderList = AJTM_LEADER.Instance.GetListEntity();
            var leaderTypedic = AJTM_LEADER_TYPE.Instance.GetDictionary("ID", "NAME");
            var leaderUitList = AJTM_LEADER_UNIT.Instance.GetListEntity();
            //
            List<UNIT_ROW> arr = new List<UNIT_ROW>();
            //写入
            foreach (DataRow dr in unitDt.Rows)
            {
                //单位名，单位id
                var unitId = Convert.ToInt32(dr["ID"]);
                var unitName = dr["NAME"].ToString();
                var unitRange = dr["RANGE_NAME"].ToString();
                var unitLevel = dr["LEVEL_NAME"].ToString();
                var unitNum = leaderUitList.Where(x => x.UNIT_ID == unitId).Sum(x => x.NUM);
                var unitHS = leaderList.Where(x => x.IS_USE == 1 && x.UNIT_ID == unitId).Count();
                //领导正职,领导副职,纪委,机关
                var typeArr = new int[] { 1, 2, 3, 4 };
                //领导正职
                IList<AJTM_LEADER.Entity> leaderNum1 = leaderList.Where(x => x.LEADER_TYPE_ID == 1 && x.UNIT_ID == unitId).ToList();
                var luNum1 = leaderUitList.Where(x => x.LEADER_TYPE_ID == 1 && x.UNIT_ID == unitId).Sum(x => x.NUM);
                var luValue1 = leaderNum1.Select(x => x.LEADER_JOB).ToList();
                var luHS1 = leaderNum1.Select(x => x.LEADER_NAME).ToList();
                //领导副职
                IList<AJTM_LEADER.Entity> leaderNum2 = leaderList.Where(x => x.LEADER_TYPE_ID == 2 && x.UNIT_ID == unitId).ToList();
                var luNum2 = leaderUitList.Where(x => x.LEADER_TYPE_ID == 2 && x.UNIT_ID == unitId).Sum(x => x.NUM);
                var luValue2 = leaderNum2.Select(x => x.LEADER_JOB).ToList();
                var luHS2 = leaderNum2.Select(x => x.LEADER_NAME).ToList();
                //纪委
                IList<AJTM_LEADER.Entity> leaderNum3 = leaderList.Where(x => x.LEADER_TYPE_ID == 4 && x.UNIT_ID == unitId).ToList();
                var luNum3 = leaderUitList.Where(x => x.LEADER_TYPE_ID == 4 && x.UNIT_ID == unitId).Sum(x => x.NUM);
                //var luValue3 = leaderNum3.Count;
                var luHS3 = leaderNum3.Select(x => x.LEADER_NAME).ToList();
                //机关
                IList<AJTM_LEADER.Entity> leaderNum4 = leaderList.Where(x => x.LEADER_TYPE_ID == 3 && x.UNIT_ID == unitId).ToList();
                var luNum4 = leaderUitList.Where(x => x.LEADER_TYPE_ID == 3 && x.UNIT_ID == unitId).Sum(x => x.NUM);
                //var luValue4 = leaderNum4.Count;
                var luHS4 = leaderNum3.Select(x => x.LEADER_NAME).ToList();
                //其他
                IList<AJTM_LEADER.Entity> leaderNUm5 = leaderList.Where(x => !typeArr.Contains(x.LEADER_TYPE_ID) && x.UNIT_ID == unitId).ToList();
                var luNum5 = leaderUitList.Where(x => !typeArr.Contains(x.LEADER_TYPE_ID) && x.UNIT_ID == unitId).Sum(x => x.NUM);
                var luValue5 = leaderNUm5.Select(x => x.LEADER_JOB).ToList();
                var luHS5 = leaderNUm5.Select(x => x.LEADER_NAME).ToList();
                //计算最大值
                var leaderNumArr = new int[] { leaderNum1.Count(), leaderNum2.Count(), leaderNum3.Count(), leaderNum4.Count(), leaderNUm5.Count() };
                var maxCount = leaderNumArr.Max(); //

                if (unitId == 75)
                {
                    int s = 2;
                }

                if (maxCount > 0)
                {
                    //初始加载
                    for (int i = 0; i < maxCount; i++)
                    {
                        UNIT_ROW row = new UNIT_ROW();
                        row.cell = new List<UNIT_CELL>();
                        if (i == 0)
                        {
                            //初始信息
                            row.cell.Add(setCell(unitId.ToString(), maxCount));
                            row.cell.Add(setCell(unitName, maxCount));
                            row.cell.Add(setCell(unitRange, maxCount));
                            row.cell.Add(setCell(unitLevel, maxCount));
                            //副县级以上
                            row.cell.Add(setCell(unitNum.ToString(), maxCount));
                            row.cell.Add(setCell(unitHS.ToString(), maxCount));
                            //领导正职
                            row.cell.Add(setCell(luNum1.ToString(), maxCount));
                            setCell(luValue1, i, maxCount, row);
                            setCell(luHS1, i, maxCount, row);
                            //领导副职
                            row.cell.Add(setCell(luNum2.ToString(), maxCount));
                            setCell(luValue2, i, maxCount, row);
                            setCell(luHS2, i, maxCount, row);
                            //纪委
                            row.cell.Add(setCell(luNum3.ToString(), maxCount));
                            //row.cell.Add(setCell(luValue3.ToString(), maxCount));
                            setCell(luHS3, i, maxCount, row);
                            //机关
                            row.cell.Add(setCell(luNum4.ToString(), maxCount));
                            //row.cell.Add(setCell(luValue4.ToString(), maxCount));
                            setCell(luHS4, i, maxCount, row);
                            //其他
                            row.cell.Add(setCell(luNum5.ToString(), maxCount));
                            setCell(luValue5, i, maxCount, row);
                            setCell(luHS5, i, maxCount, row);
                            //备注
                            row.cell.Add(setCell("", maxCount));
                        }
                        else
                        {
                            //领导正职
                            setCell(luValue1, i, maxCount, row);
                            setCell(luHS1, i, maxCount, row);
                            //领导副职
                            setCell(luValue2, i, maxCount, row);
                            setCell(luHS2, i, maxCount, row);
                            //纪委
                            setCell(luHS3, i, maxCount, row);
                            //机关
                            setCell(luHS4, i, maxCount, row);
                            //其他
                            setCell(luValue5, i, maxCount, row);
                            setCell(luHS5, i, maxCount, row);
                        }
                        arr.Add(row);
                    }
                }
                else
                {
                    UNIT_ROW row = new UNIT_ROW();
                    row.cell = new List<UNIT_CELL>();
                    //初始信息
                    row.cell.Add(setCell(unitId.ToString(), maxCount));
                    row.cell.Add(setCell(unitName, maxCount));
                    row.cell.Add(setCell(unitRange, maxCount));
                    row.cell.Add(setCell(unitLevel, maxCount));
                    //副县级以上
                    row.cell.Add(setCell(string.Empty, maxCount));
                    row.cell.Add(setCell(string.Empty, maxCount));
                    //领导正职
                    row.cell.Add(setCell(luNum1.ToString(), maxCount));
                    setCell(luValue1, 0, maxCount, row);
                    setCell(luHS1, 0, maxCount, row);
                    //领导副职
                    row.cell.Add(setCell(luNum2.ToString(), maxCount));
                    setCell(luValue2, 0, maxCount, row);
                    setCell(luHS2, 0, maxCount, row);
                    //纪委
                    row.cell.Add(setCell(luNum3.ToString(), maxCount));
                    //row.cell.Add(setCell(luValue3.ToString(), maxCount));
                    setCell(luHS3, 0, maxCount, row);
                    //机关
                    row.cell.Add(setCell(luNum4.ToString(), maxCount));
                    //row.cell.Add(setCell(luValue4.ToString(), maxCount));
                    setCell(luHS4, 0, maxCount, row);
                    //其他
                    row.cell.Add(setCell(luNum5.ToString(), maxCount));
                    setCell(luValue5, 0, maxCount, row);
                    setCell(luHS5, 0, maxCount, row);
                    //备注
                    row.cell.Add(setCell("", maxCount));
                    arr.Add(row);
                }

            

            }
            return arr;
        }

        public UNIT_CELL setCell(string value, int rowspan)
        {
            value = value == "0" ? "" : value;
            return new UNIT_CELL()
            {
                value = value,
                rowspan = rowspan
            };
        }

        public void setCell(List<string> valuelist, int index, int maxCount, UNIT_ROW row)
        {

            if (valuelist.Count > 0)
            {
                bool isSet = true;
                int rowspan = 0;
                var value = analysisValueList(valuelist, index, maxCount, ref rowspan, ref isSet);
                if (isSet)
                    row.cell.Add(setCell(value, rowspan));
            }
            else
            {
                if (index == 0)
                {
                    row.cell.Add(setCell("", maxCount));
                }
            }
        }

        public string analysisValueList(List<string> valuelist, int index, int maxCount, ref int rowspan, ref bool isSet)
        {
            int valueCount = valuelist.Count;
            int pagesize = Convert.ToInt32(maxCount / valueCount);
            int remainder = index % pagesize;
            int valueIndex = Convert.ToInt32(index / pagesize);
            int valueEndIndex = valuelist.Count - 1;
            //当余数为0时
            if (remainder == 0 && valueIndex <= valueEndIndex)
            {
                isSet = true;
                int endindex = (valueCount - 1) * pagesize;
                if (index >= endindex)
                {
                    rowspan = maxCount - endindex;
                    return valuelist[valueCount - 1];
                }
                else
                {
                    rowspan = pagesize;
                    //int valueIndex = Convert.ToInt32(index / pagesize);
                    return valuelist[valueIndex];
                }
            }
            else
            {
                isSet = false;
                return "";
            }
        }
    }
}
