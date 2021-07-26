using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.BLL.Extension
{
    public class ExcelUnit : Excel
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="path"></param>
        /// <param name="sheet"></param>
        public ExcelUnit(string path, string sheet) : base(path, sheet)
        {
            //标题
            wrTitle();
            //头部文件
            wrHeader();
            //写入
            wrMain();
        }
        //行号
        private int _rowIndex = 5;
        //列号
        private int _colIndex = 0;
        //最大列号
        private int _colIndexMax = 56;
        /// <summary>
        /// 标题
        /// </summary>
        private void wrTitle()
        {
            setCellStyle("方正小标宋_GBK", 24);
            //创建
            CreateExcelTable(1, _colIndexMax);
            //
            wrContent("市本级机关事业单位机构编制职数情况一览表", 0, 0, 0, _colIndexMax - 1);
        }


        private void wrHeader()
        {
            setCellStyle("方正小标宋_GBK", 10);
            //创建
            CreateExcelTable(_rowIndex, 5, _colIndexMax);
            //1 col
            wrContent("序号", 1, 0, 3, 0);
            //2 col
            wrContent("主管单位", 1, 1, 3, 0);
            //3 col
            wrContent("单位名称", 1, 2, 0, 2);
            wrContent("单位名称", 2, 2, 2, 2);
            //4-7 col
            wrContent("机构信息", 1, 5, 0, 4);
            //4,5,6,7 col
            wrContent("机构性质", 2, 5, 2, 0);
            wrContent("机构级别", 2, 6, 2, 0);
            wrContent("经费形式", 2, 7, 2, 0);
            wrContent("机构类别", 2, 8, 2, 0);
            wrContent("参公情况", 2, 9, 2, 0);
            //8-29 col 1 row
            wrContent("编制及人员信息", 1, 10, 0, 25);
            //8-16 col 17- 25 col 2 row
            wrContent("核定编制", 2, 10, 0, 8);
            wrContent("实有人员", 2, 19, 0, 8);
            //25 col 2 row
            wrContent("领导预留", 2, 28, 2, 0);
            //26-27 col  2 row
            wrContent("待上编", 2, 29, 0, 3);
            //30,31 col 2 row
            wrContent("其他预留", 2, 33, 2, 0);
            wrContent("空缺编制", 2, 34, 2, 0);
            //8-9 col 10-13 col 14,15,16 col 3 row
            wrContent("行政", 3, 10, 0, 1);
            wrContent("事业", 3, 12, 0, 3);
            wrContent("工勤", 3, 16, 1, 0);
            wrContent("政府雇员", 3, 17, 1, 0);
            wrContent("员额控制数", 3, 18, 1, 0);
            //17-18 col 19-22 col,23,24,25 col 3 row
            wrContent("行政", 3, 19, 0, 1);
            wrContent("事业", 3, 21, 0, 3);
            wrContent("工勤", 3, 25, 1, 0);
            wrContent("政府雇员", 3, 26, 1, 0);
            wrContent("员额控制数", 3, 27, 1, 0);
            //27,28,29,30 col 3 row
            wrContent("行政编制", 3, 29, 1, 0);
            wrContent("事业编制", 3, 30, 1, 0);
            wrContent("工勤控制数", 3, 31, 1, 0);
            wrContent("政府雇员", 3, 32, 1, 0);
            //8,9 col 4 row
            wrContent("一般行政",4, 10, 0, 0);
            wrContent("政法专项", 4, 11, 0, 0);
            //11,12,13,14 col 4 row
            wrContent("参公事业", 4, 12, 0, 0);
            wrContent("一般事业", 4, 13, 0, 0);
            wrContent("周转事业", 4, 14, 0, 0);
            wrContent("储备事业", 4, 15, 0, 0);
            //17,18 col 4 row
            wrContent("一般行政", 4, 19, 0, 0);
            wrContent("政法专项", 4, 20, 0, 0);
            //19,20,21,22 col 4 row
            wrContent("参公事业", 4, 21, 0, 0);
            wrContent("一般事业", 4, 22, 0, 0);
            wrContent("周转事业", 4, 23, 0, 0);
            wrContent("储备事业", 4, 24, 0, 0);
            //33 col 1 row
            wrContent("职数信息", 1, 35, 0, 15);
            //33 - 40, 41-48 2 row
            wrContent("领导职数", 2, 35, 0, 7);
            wrContent("中层职数", 2, 43, 0, 7);
            //33-35 36-40 col 3 row
            wrContent("职数数量", 3, 35, 0, 2);
            wrContent("核定职数级别", 3, 38, 0, 4);
            //41-44 45-48 col 3 row
            wrContent("职数数量", 3, 43, 0, 3);
            wrContent("核定职数级别", 3, 47, 0, 3);
            //33，34，35 col 4 row
            wrContent("核定", 4, 35, 0, 0);
            wrContent("实配", 4, 36, 0, 0);
            wrContent("空缺", 4, 37, 0, 0);
            //36,37,38,39,40 col 4 row
            wrContent("副厅", 4, 38, 0, 0);
            wrContent("正县", 4, 39, 0, 0);
            wrContent("副县", 4, 40, 0, 0);
            wrContent("正科", 4, 41, 0, 0);
            wrContent("副科", 4, 42, 0, 0);
            //41,42,43,44 col 4 row
            wrContent("正职核定", 4, 43, 0, 0);
            wrContent("正职实配", 4, 44, 0, 0);
            wrContent("副职核定", 4, 45, 0, 0);
            wrContent("副职实配", 4, 46, 0, 0);
            //45,46,47,48 col 4 row
            wrContent("正县", 4, 47, 0, 0);
            wrContent("副县", 4, 48, 0, 0);
            wrContent("正科", 4, 49, 0, 0);
            wrContent("副科", 4, 50, 0, 0);
            //49 
            wrContent("备注", 1, 51, 3, 0);
            //50-53 col 1 row
            wrContent("备注分类别基础数据", 1, 52, 0, 3);
            //50,51,52,53 col 2 row
            wrContent("待上编备注", 2, 52, 2, 0);
            wrContent("领导预留备注", 2, 53, 2, 0);
            wrContent("其他预留备注", 2, 54, 2, 0);
            wrContent("其他备注", 2, 55, 2, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        public void wrMain()
        {
            var dt = CS.BLL.Model.AJTM_UNIT.Instance.GetUnitInfo();
            int mergeR = 0;
            int beginIndex = _rowIndex;
            int num = 0;
            foreach(DataRow dr in dt.Rows)
            {
                num += 1;
                var isBool = (dr["PARENT_ID"].ToString() == dr["ID"].ToString()) ? true : false;
                wrCell(dr["ID"].ToString());
                if (isBool) 
                {
                    wrCell(dr["PANRET_NAME"].ToString(), false);
                    if(mergeR != 0)
                    {
                        //合并单元格
                        Merged(beginIndex, _colIndex - 1, mergeR, 0);
                    }
                    //初始化
                    beginIndex = _rowIndex;
                    mergeR = 0;
                    //单元格移动
                    wrContent(dr["NAME"].ToString(), _rowIndex, _colIndex, 0, 2);
                    _colIndex += 3;
                }
                else
                {
                    mergeR += 1;
                    wrCell("", false);
                    if (num == dt.Rows.Count)
                    {
                        Merged(beginIndex, _colIndex - 1, mergeR, 0);
                        beginIndex = _rowIndex;
                        mergeR = 0;
                    }
                    wrCell(dr["NO1"].ToString());
                    wrCell(dr["NO2"].ToString());
                    wrCell(dr["NAME"].ToString());
                }
                wrCell(dr["SETUP_NATRUE"].ToString());
                wrCell(dr["SETUP_LEVEL"].ToString());
                wrCell(dr["OUTLAY_MODE"].ToString());
                wrCell(dr["SETUP_TYPE"].ToString());
                wrCell(dr["IS_PUBLIC"].ToString());
                wrCell(dr["XZ_YBXZ_NUM"].ToString());
                wrCell(dr["XZ_ZFZX_NUM"].ToString());
                wrCell(dr["SY_YBSY_NUM"].ToString());
                wrCell(dr["SY_CGSY_NUM"].ToString());
                wrCell(dr["SY_CBSY_NUM"].ToString());
                wrCell(dr["SY_ZZSY_NUM"].ToString());
                wrCell(dr["GQ_KZ_NUM"].ToString());
                wrCell(dr["ZF_KY_NUM"].ToString());
                wrCell(dr["P_XZ_YBXZ_NUM"].ToString());
                wrCell(dr["P_XZ_ZFZX_NUM"].ToString());
                wrCell(dr["P_SY_YBSY_NUM"].ToString());
                wrCell(dr["P_SY_CGSY_NUM"].ToString());
                wrCell(dr["P_SY_CBSY_NUM"].ToString());
                wrCell(dr["P_SY_ZZSY_NUM"].ToString());
                wrCell(dr["P_GQ_KZ_NUM"].ToString());
                wrCell(dr["P_ZF_KY_NUM"].ToString());
                wrCell(dr["P_YR_KY_NUM"].ToString());
                wrCell("领导预留");
                wrCell(dr["D_XZ_NUM"].ToString());
                wrCell(dr["D_SY_YBSY_NUM"].ToString());
                wrCell(dr["D_SY_CGSY_NUM"].ToString());
                wrCell(dr["D_GQ_KZ_NUM"].ToString());
                wrCell(dr["D_ZF_KY_NUM"].ToString());
                wrCell("其他预留");
                wrCell("空缺编制");
                wrCell(dr["LEADER_UNIT_NUM"].ToString());
                wrCell(dr["LEADER_SJ_NUM"].ToString());
                wrCell(dr["LEADER_NULL_NUM"].ToString());
                wrCell(dr["OFFICE_MIAN_NUM"].ToString());
                wrCell(dr["OFFICE_VICE_NUM"].ToString());
                wrCell(dr["COUNTY_MIAN_L"].ToString());
                wrCell(dr["COUNTY_VICE_L"].ToString());
                wrCell(dr["VILLAGE_MIAN_L"].ToString());
                wrCell(dr["VILLAGE_VICE_L"].ToString());
                wrCell(dr["WITHIN_MAIN_NUM"].ToString());
                wrCell(dr["WITHIN_MAIN_SP_NUM"].ToString());
                wrCell(dr["WITHIN_VICE_NUM"].ToString());
                wrCell(dr["WITHIN_VICE_SP_NUM"].ToString());
                wrCell(dr["COUNTY_MIAN_MR"].ToString());
                wrCell(dr["COUNTY_VICE_MR"].ToString());
                wrCell(dr["VILLAGE_MIAN_MR"].ToString());
                wrCell(dr["VILLAGE_VICE_MR"].ToString());
                wrCell("备注");
                wrCell("备注");
                wrCell("备注");
                wrCell("备注");
                //
                lineRow(1);
            }
        }

        private void wrCell(string value,bool isMerge = true)
        {
            wrContent(value, _rowIndex, _colIndex, 0, 0, isMerge);
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
        private void lineRow(int rowNum = 1)
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
