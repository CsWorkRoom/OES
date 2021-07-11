using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using CS.BLL.Model;
using System.Data;

namespace CS.BLL.Extension
{
    public class UnitLeaderExcel : Excel
    {

        public UnitLeaderExcel(string path, string sheet) : base(path, sheet)
        {
            //写头部文件
            wrHeader();
            //写头部样式
            wrTitle();
            //
            wrDataTable();
        }

        private void wrHeader()
        {
            setCellStyle("方正小标宋_GBK", 24);
            wrContent("市本级县处级及以上单位核定领导职数情况表", 0, 0, 0, 18);
        }

        private void wrTitle()
        {
            setCellStyle("方正小标宋_GBK", 10);
            wrContent("序号", 1, 0, 1, 0);
            SetColumnWidth(0, 4);
            wrContent("机构名称", 1, 1, 1, 0);
            SetColumnWidth(1, 20);
            wrContent("行政", 1, 2, 1, 0);
            SetColumnWidth(2, 4);
            wrContent("机构规格", 1, 3, 1, 0);
            SetColumnWidth(3, 4);
            wrContent("副县级以上领导职数总计", 1, 4, 0, 1);
            wrContent("核定", 2, 4, 0, 0);
            wrContent("实有", 2, 5, 0, 0);
            SetColumnWidth(4, 10);
            SetColumnWidth(5, 10);
            wrContent("领导正职", 1, 6, 0, 2);
            wrContent("核定", 2, 6, 0, 0);
            wrContent("名称", 2, 7, 0, 0);
            wrContent("实有", 2, 8, 0, 0);
            wrContent("领导副职", 1, 9, 0, 2);
            wrContent("核定", 2, 9, 0, 0);
            wrContent("名称", 2, 10, 0, 0);
            wrContent("实有", 2, 11, 0, 0);
            wrContent("纪委书记(纪工委书记、纪检组长、纪检员)", 1, 12, 0, 1);
            wrContent("核定", 2, 12, 0, 0);
            wrContent("实有", 2, 13, 0, 0);
            wrContent("机关党委书记", 1, 14, 0, 1);
            wrContent("核定", 2, 14, 0, 0);
            wrContent("实有", 2, 15, 0, 0);
            wrContent("其他领导", 1, 16, 0, 2);
            wrContent("核定", 2, 16, 0, 0);
            wrContent("名称", 2, 17, 0, 0);
            wrContent("实有", 2, 18, 0, 0);
            wrContent("备注", 1, 19, 1, 0);
        }
        private int rowNum = 3;
        private int col = 0;
        private void wrContent(string value, int mergeR = 0, int mergeC = 0)
        {
            wrContent(value, rowNum, col, mergeR, mergeC);
            col += 1;
        }
        private void wrContent(int col, string value, int mergeR = 0, int mergeC = 0)
        {
            wrContent(value, rowNum, col, mergeR, mergeC);
        }
        private void wrRowEnd(int row)
        {
            col = 0;
            rowNum = rowNum + row;
        }

        public void wrDataTable()
        {
            var unitDt = CS.BLL.Model.AJTM_UNIT.Instance.GetTable();
            IList<AJTM_LEADER.Entity> leaderList = AJTM_LEADER.Instance.GetListEntity();
            Dictionary<int, string> leaderTypedic = AJTM_LEADER_TYPE.Instance.GetDictionary("ID", "NAME");
            IList<AJTM_LEADER_UNIT.Entity> leaderUitList = AJTM_LEADER_UNIT.Instance.GetListEntity();

            foreach (DataRow dr in unitDt.Rows)
            {
                var unitName = dr["NAME"].ToString();
                var unitId = Convert.ToInt32(dr["ID"]);
                //领导正职
                IList<AJTM_LEADER.Entity> leaderNum1 = leaderList.Where(x => x.LEADER_TYPE_ID == 1 && x.UNIT_ID == unitId).ToList();
                //领导副职
                IList<AJTM_LEADER.Entity> leaderNum2 = leaderList.Where(x => x.LEADER_TYPE_ID == 2 && x.UNIT_ID == unitId).ToList();
                //纪委
                IList<AJTM_LEADER.Entity> leaderNum3 = leaderList.Where(x => x.LEADER_TYPE_ID == 4 && x.UNIT_ID == unitId).ToList();
                //机关
                IList<AJTM_LEADER.Entity> leaderNum4 = leaderList.Where(x => x.LEADER_TYPE_ID == 3 && x.UNIT_ID == unitId).ToList();
                //其他
                var typeArr = new int[] { 1, 2, 3, 4 };
                IList<AJTM_LEADER.Entity> leaderNUm5 = leaderList.Where(x => !typeArr.Contains(x.LEADER_TYPE_ID) && x.UNIT_ID == unitId).ToList();
                //计算最大值
                var leaderNumArr = new int[] { leaderNum1.Count(), leaderNum2.Count(), leaderNum3.Count(), leaderNum4.Count(), leaderNUm5.Count() };
                var maxCount = leaderNumArr.Max();
                maxCount = maxCount > 1 ? maxCount : 1;
                var maxRowCount = maxCount - 1;
                //
                wrContent(unitId.ToString(), maxRowCount, 0);
                wrContent(unitName, maxRowCount, 0);
                wrContent(string.Empty, maxRowCount, 0);
                wrContent(string.Empty, maxRowCount, 0);
                wrContent(string.Empty, maxRowCount, 0);
                wrContent(string.Empty, maxRowCount, 0);
                //正职
                var leaderUnit1Num = leaderUitList.Where(x => x.LEADER_TYPE_ID == 1 && x.UNIT_ID == unitId).Sum(x => x.NUM);
                wrValue(unitId,  maxRowCount, maxCount, leaderUnit1Num, leaderNum1);
                var leaderUnit2Num = leaderUitList.Where(x => x.LEADER_TYPE_ID == 2 && x.UNIT_ID == unitId).Sum(x => x.NUM); 
                wrValue(unitId,  maxRowCount, maxCount, leaderUnit2Num, leaderNum2);
                //
                var leaderUnit3Num = leaderUitList.Where(x => x.LEADER_TYPE_ID == 4 && x.UNIT_ID == unitId).Sum(x => x.NUM);
                wrValueBy2(unitId, maxRowCount, maxCount, leaderUnit2Num, leaderNum3);
                //
                var leaderUnit4Num = leaderUitList.Where(x => x.LEADER_TYPE_ID == 3 && x.UNIT_ID == unitId).Sum(x => x.NUM);
                wrValueBy2(unitId, maxRowCount, maxCount, leaderUnit4Num, leaderNum4);
                //
                var leaderUnit5Num = leaderUitList.Where(x => !typeArr.Contains(x.LEADER_TYPE_ID) && x.UNIT_ID == unitId).Sum(x => x.NUM);
                wrValue(unitId, maxRowCount, maxCount, leaderUnit5Num, leaderNUm5);
                wrRowEnd(maxCount);
            }
        }

        private void wrValue(int unitId, int maxRowCount, int maxCount,  int leaderUnitNum, IList<AJTM_LEADER.Entity> leaderNum)
        {
            //汇总数据
            if (leaderUnitNum <= 0)
            {
                wrContent("", maxRowCount, 0);
                wrContent("", maxRowCount, 0);
                wrContent("", maxRowCount, 0);
                return;
            }
            wrContent(leaderUnitNum.ToString(), maxRowCount, 0);
            //多少页
            var page = leaderNum.Count;
            //名称与实有
            if (page > 0)
            {
                var remainder = maxCount; //余数
                var pagesize = Convert.ToInt32(Math.Floor(Convert.ToDecimal(maxCount / page))); //分页
                for (int i = 0; i < page; i++)
                {
                    int currRowNum = (i * pagesize) + rowNum;//当前行
                    int currMergeR = pagesize - 1;//合并行数
                    //最后一行时，取剩余数
                    if (i == (page - 1))
                    {
                        currRowNum = (maxCount - remainder) + rowNum;
                        currMergeR = remainder - 1;
                    }
                    //职务
                    string leaderJob = leaderNum[i].LEADER_JOB;
                    //写入
                    base.wrContent(leaderJob, currRowNum, col, currMergeR, 0);
                    //下一列
                    int lastCol = col + 1;
                    //写入
                    base.wrContent("1", currRowNum, lastCol, currMergeR, 0);
                    //减去当前页的数量
                    remainder = remainder - pagesize;
                }
                //跨行
                col += 2;
            }
            else
            {
                wrContent("", maxRowCount, 0);
                wrContent("", maxRowCount, 0);
            }
        }
        
        private void wrValueBy2(int unitId,int maxRowCount,int maxCount,int leaderUnitNum, IList<AJTM_LEADER.Entity> leaderNum)
        {
            //汇总数据
            if (leaderUnitNum <= 0)
            {
                wrContent("", maxRowCount, 0);
                return;
            }
            wrContent(leaderUnitNum.ToString(), maxRowCount, 0);
            wrContent(leaderNum.Count().ToString(), maxRowCount, 0);
        }
    }
}
