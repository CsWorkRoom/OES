using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.BLL.Extension
{
    public class ExcelTest : Excel
    {
        //行号
        private int _rowIndex = 5;
        //列号
        private int _colIndex = 0;
        //最大列号
        private int _colIndexMax = 56;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="path"></param>
        /// <param name="sheet"></param>
        public ExcelTest(string path, string sheet) : base(path, sheet)
        {
            //标题
            wrTitle();
            //
            wrHeader();
        }
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

        private Dictionary<int, List<int>> _dic = new Dictionary<int, List<int>>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="MR"></param>
        /// <param name="MC"></param>
        private void wrHC(string value, int MR = 0, int MC = 0)
        {
            int rindex = _rowIndex;
            int cindex = _colIndex;
            List<int> dicCol = new List<int>();
            //先检查是否存在该行
            if (_dic.ContainsKey(rindex))
            {
                dicCol = _dic[rindex];
                //检查该列是否被占用
                if (dicCol.Contains(cindex))
                {
                    int min = 0;
                    do
                    {
                        cindex += 1;
                        var dicList = dicCol.Where(x => x >= cindex);
                        if (dicList.Count() == 0) min = cindex + 1;
                        else min = dicList.Min();
                    } while (min <= cindex);
                }
            }
            else
            {
                _dic.Add(rindex, dicCol);
            }
            wrContent(value, rindex, cindex, MR, MC);
            //下一个单元格
            int nextCol = MC + 1;
            //下一个单元格
            _colIndex = cindex + nextCol;
            //部署单元格占有
            for (int i = 0; i <= MR; i++)
            {
                var r = _rowIndex + i;
                var c = new List<int>();
                if (!_dic.ContainsKey(r))
                {
                    _dic.Add(r, c);
                }
                else
                {
                    c = _dic[r];
                }
                for(int j = 0; j <= MC; j++)
                {
                    var cI = cindex + j;
                    c.Add(cI);
                }
            }
        }
        public void line()
        {
            _rowIndex += 1;
            _colIndex = 0;
        }
        private void wrHeader()
        {
            setCellStyle("方正小标宋_GBK", 10);
            //创建
            CreateExcelTable(_rowIndex, 5, _colIndexMax);
            //1 col
            wrHC("序号", 3, 0);
            //2 col
            wrHC("主管单位", 3, 0);
            //3 col
            wrHC("单位名称", 0, 2);
            //4-7 col
            wrHC("机构信息", 0, 4);
            //8-29 col 1 row
            wrHC("编制及人员信息", 0, 24);
            //33 col 1 row
            wrHC("职数信息", 0, 15);
            //49 
            wrHC("备注", 3, 0);
            //50-53 col 1 row
            wrHC("备注分类别基础数据", 0, 3);
            line();

            wrHC("单位名称", 2, 2);
            //4,5,6,7 col
            wrHC("机构性质", 2, 0);
            wrHC("机构级别", 2, 0);
            wrHC("经费形式", 2, 0);
            wrHC("机构类别", 2, 0);
            wrHC("参公情况", 2, 0);
            //8-16 col 17- 25 col 2 row
            wrHC("核定编制", 0, 8);
            wrHC("实有人员", 0, 8);
            //25 col 2 row
            wrHC("领导预留", 2, 0);
            //26-27 col  2 row
            wrHC("待上编", 0, 3);
            //30,31 col 2 row
            wrHC("其他预留", 2, 0);
            wrHC("空缺编制", 2, 0);
            //33 - 40, 41-48 2 row
            wrHC("领导职数", 0, 7);
            wrHC("中层职数", 0, 7);
            //50,51,52,53 col 2 row
            wrHC("待上编备注", 2, 0);
            wrHC("领导预留备注", 2, 0);
            wrHC("其他预留备注", 2, 0);
            wrHC("其他备注", 2, 0);
            line();
            //8-9 col 10-13 col 14,15,16 col 3 row
            wrHC("行政", 0, 1);
            wrHC("事业", 0, 3);
            wrHC("工勤", 1, 0);
            wrHC("政府雇员", 1, 0);
            wrHC("员额控制数", 1, 0);
            //17-18 col 19-22 col,23,24,25 col 3 row
            wrHC("行政", 0, 1);
            wrHC("事业", 0, 3);
            wrHC("工勤", 1, 0);
            wrHC("政府雇员", 1, 0);
            wrHC("员额控制数", 1, 0);
            //27,28,29,30 col 3 row
            wrHC("行政编制", 1, 0);
            wrHC("事业编制", 1, 0);
            wrHC("工勤控制数", 1, 0);
            wrHC("政府雇员", 1, 0);
            //33-35 36-40 col 3 row
            wrHC("职数数量", 0, 2);
            wrHC("核定职数级别", 0, 4);
            //41-44 45-48 col 3 row
            wrHC("职数数量", 0, 3);
            wrHC("核定职数级别", 0, 3);
            line();
            ////8,9 col 4 row
            wrHC("一般行政");
            wrHC("政法专项");
            ////11,12,13,14 col 4 row
            wrHC("参公事业");
            wrHC("一般事业");
            wrHC("周转事业");
            wrHC("储备事业");
            //17,18 col 4 row
            wrHC("一般行政");
            wrHC("政法专项");
            //19,20,21,22 col 4 row
            wrHC("参公事业");
            wrHC("一般事业");
            wrHC("周转事业");
            wrHC("储备事业");
            //33，34，35 col 4 row
            wrHC("核定");
            wrHC("实配");
            wrHC("空缺");
            //36,37,38,39,40 col 4 row
            wrHC("副厅");
            wrHC("正县");
            wrHC("副县");
            wrHC("正科");
            wrHC("副科");
            //41,42,43,44 col 4 row
            wrHC("正职核定");
            wrHC("正职实配");
            wrHC("副职核定");
            wrHC("副职实配");
            //45,46,47,48 col 4 row
            wrHC("正县");
            wrHC("副县");
            wrHC("正科");
            wrHC("副科");
            line();
            SetColumnWidth(1, 30);
            SetColumnWidth(4, 30);
            SetColumnWidth(51, 30);
            SetColumnWidth(52, 40);
            SetColumnWidth(53, 40);
            SetColumnWidth(54, 40);
            SetColumnWidth(55, 40);
        }
    }
}
