using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.BLL.Extension
{
    public class ExcelUnit : Excel
    {
        //行号
        private int _rowIndex = 3;
        //列号
        private int _colIndex = 0;
        //最大列号
        private int _colIndexMax = 29;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="path"></param>
        /// <param name="sheet"></param>
        public ExcelUnit(string path, string sheet) : base(path, sheet)
        {
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
    }
}
