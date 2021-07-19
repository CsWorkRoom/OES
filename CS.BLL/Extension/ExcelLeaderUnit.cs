using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CS.BLL.Model;

namespace CS.BLL.Extension
{
    public class ExcelLeaderUnit : Excel
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="path"></param>
        /// <param name="sheet"></param>
        public ExcelLeaderUnit(string path, string sheet) : base(path, sheet)
        {
            //标题
            wrTitle();
            //头部文件
            wrHeader();
            //写入
            wrMain();
        }
        //行号
        private int _rowIndex = 3;
        //列号
        private int _colIndex = 0;
        //最大列号
        private int _colIndexMax = 20;
        /// <summary>
        /// 标题
        /// </summary>
        private void wrTitle()
        {
            setCellStyle("方正小标宋_GBK", 24);
            //创建
            CreateExcelTable(1, _colIndexMax);
            //
            wrContent("市本级县处级及以上单位核定领导职数情况表", 0, 0, 0, _colIndexMax - 1);
        }
        /// <summary>
        /// 头部
        /// </summary>
        private void wrHeader()
        {
            setCellStyle("方正小标宋_GBK", 10);
            //创建
            CreateExcelTable(_rowIndex, 3, _colIndexMax);
            //
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
            wrContent("领导正职", 1, 6, 0, 2);
            wrContent("核定", 2, 6, 0, 0);
            wrContent("名称", 2, 7, 0, 0);
            SetColumnWidth(7, 20);
            wrContent("实有", 2, 8, 0, 0);
            wrContent("领导副职", 1, 9, 0, 2);
            wrContent("核定", 2, 9, 0, 0);
            wrContent("名称", 2, 10, 0, 0);
            SetColumnWidth(10, 20);
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
            SetColumnWidth(17, 20);
            wrContent("实有", 2, 18, 0, 0);
            wrContent("备注", 1, 19, 1, 0);
            SetColumnWidth(19, 30);
            ////第一列
            //var header1 = new List<string>();
            //header1.Add("序号");
            //header1.Add("机构名称");
            //header1.Add("行政");
            //header1.Add("机构规格");
            //header1.Add("副县级以上领导职数总计");
            //header1.Add(string.Empty);
            //header1.Add("领导正职");
            //header1.Add(string.Empty);
            //header1.Add(string.Empty);
            //header1.Add("领导副职");
            //header1.Add(string.Empty);
            //header1.Add(string.Empty);
            //header1.Add("纪委书记(纪工委书记、纪检组长、纪检员)");
            //header1.Add(string.Empty);
            //header1.Add("机关党委书记");
            //header1.Add(string.Empty);
            //header1.Add("其他领导");
            //header1.Add(string.Empty);
            //header1.Add(string.Empty);
            //header1.Add("备注");
            ////第二列
            //var header2 = new List<string>();
            //header2.Add(string.Empty);
            //header2.Add(string.Empty);
            //header2.Add(string.Empty);
            //header2.Add(string.Empty);
            //header2.Add("核定");
            //header2.Add("实有");
            //header2.Add("核定");
            //header2.Add("名称");
            //header2.Add("实有");
            //header2.Add("核定");
            //header2.Add("名称");
            //header2.Add("实有");
            //header2.Add("核定");
            //header2.Add("实有");
            //header2.Add("核定");
            //header2.Add("实有");
            //header2.Add("核定");
            //header2.Add("名称");
            //header2.Add("实有");
            //header2.Add(string.Empty);
            ////
            //wrRow(header1);
            //wrRow(header2);
        }
        /// <summary>
        /// 主体
        /// </summary>
        private void wrMain()
        {
            var unitDt = CS.BLL.Model.AJTM_UNIT.Instance.GetTableForExcel();
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
                var unitNum = leaderUitList.Where(x => x.UNIT_ID == unitId).Sum(x => x.NUM);
                var unitHS = leaderList.Where(x => x.IS_USE == 1 && x.UNIT_ID == unitId).Count();
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
                wrCell(unitId.ToString(), mergeRCount);
                wrCell(unitName, mergeRCount);
                wrCell(unitRange, mergeRCount);
                wrCell(unitLevel, mergeRCount);
                //副县级以上
                wrCell(unitNum.ToString(), mergeRCount);
                wrCell(unitHS.ToString(), mergeRCount);
                //领导正职
                wrCell(luNum1.ToString(), mergeRCount);
                wrCell(luValue1, mergeRCount);
                wrCell(luHS1, mergeRCount);
                //领导副职
                wrCell(luNum2.ToString(), mergeRCount);
                wrCell(luValue2, mergeRCount);
                wrCell(luHS2, mergeRCount);
                //纪委
                wrCell(luNum3.ToString(), mergeRCount);
                wrCell(luValue3.ToString(), mergeRCount);
                //机关
                wrCell(luNum4.ToString(), mergeRCount);
                wrCell(luValue4.ToString(), mergeRCount);
                //其他
                wrCell(luNum5.ToString(), mergeRCount);
                wrCell(luValue5, mergeRCount);
                wrCell(luHS5, mergeRCount);
                //备注
                wrCell("", mergeRCount);
                //下一行
                lineRow(mergeRCount);
            }
        }



        private void wrCell(string value,int MergeRowNum)
        {
            wrCellMerge(_rowIndex, _colIndex, new List<string>() { value }, MergeRowNum);
            //下一列
            lineCol();
        }

        private void wrCell(IList<string> value,int MergeRowNum)
        {
            if (value.Count == 0) value.Add(string.Empty);
            wrCellMerge(_rowIndex, _colIndex, value, MergeRowNum);
            //下一列
            lineCol();
        }
        /// <summary>
        /// 复原列号
        /// </summary>
        private void ResetCol()
        {
            _colIndex = 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rowNum"></param>
        private void lineRow(int rowNum)
        {
            _rowIndex = _rowIndex + rowNum;
            _colIndex = 0;
        }
        //换列
        private void lineCol()
        {
            _colIndex = _colIndex + 1;
        }
        /// <summary>
        /// 换行
        /// </summary>
        private void lineRow()
        {
            _rowIndex = _rowIndex + 1;
        }
    }
}
