using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;


namespace CS.BLL.Extension
{
    public class Excel
    {
        /// <summary>
        /// 根目录
        /// </summary>
        private string _rootPath = string.Empty;
        /// <summary>
        /// 对象实例
        /// </summary>
        private XSSFWorkbook _workbook;
        /// <summary>
        /// 
        /// </summary>
        protected ISheet _sheet;

        #region 公开
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="path"></param>
        /// <param name="sheet">表头</param>
        public Excel(string path, string sheet)
        {
            if (Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }
            _rootPath = new DirectoryInfo(path).FullName;
            _workbook = new XSSFWorkbook();
            _sheet = _workbook.CreateSheet(sheet);
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        public string Save()
        {
            string fileName = _rootPath + "/" + DateTime.Now.Ticks.ToString() + ".xlsx";
            using (FileStream fs = File.OpenWrite(fileName))
            {
                _workbook.Write(fs);
            }
            return fileName;
        }
        #endregion

        #region 继承
        /// <summary>
        /// 创建表格-新增范围
        /// </summary>
        /// <param name="rowNum">行数</param>
        /// <param name="colNum">列数</param>
        protected void CreateExcelTable(int rowNum = 0, int colNum = 0)
        {
            CreateExcelTable(0, rowNum, 0, colNum);
        }
        /// <summary>
        /// 创建表格-新增行
        /// </summary>
        /// <param name="rowStartIndex"></param>
        /// <param name="rowEndIndex"></param>
        /// <param name="colNum"></param>
        protected void CreateExcelTable(int rowStartIndex, int rowEndIndex, int colNum)
        {
            CreateExcelTable(rowStartIndex, rowEndIndex, 0, colNum);
        }
        /// <summary>
        /// 创建表格-新增表格范围
        /// </summary>
        /// <param name="rowStartIndex">起始行号</param>
        /// <param name="rowEndIndex">结束行号</param>
        /// <param name="colStartIndex">起始列号</param>
        /// <param name="colEndIndex">结束列号</param>
        protected void CreateExcelTable(int rowStartIndex, int rowEndIndex, int colStartIndex, int colEndIndex)
        {
            var rowIndex = rowEndIndex - rowStartIndex;
            var colIndex = colEndIndex - colStartIndex;
            for (int i = rowStartIndex; i <= rowIndex; i++)
            {
                var row = GetRow(i);
                for (int j = colStartIndex; j <= colIndex; j++)
                {
                    var cell = GetCell(row, j);
                }
            }
        }
        /// <summary>
        /// 写内容
        /// </summary>
        /// <param name="rown"></param>
        /// <param name="coln"></param>
        /// <param name="value"></param>
        /// <param name="mergeR"></param>
        /// <param name="mergeC"></param>
        protected void wrContent(string value, int rowIndex, int colIndex, int mergeR = 0, int mergeC = 0)
        {
            mergeR = mergeR < 0 ? 0 : mergeR;
            mergeC = mergeC < 0 ? 0 : mergeC;
            //设置值
            setCell(value, rowIndex, colIndex);
            //用于value为null时，避免自动合并单元格
            _sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex + mergeR, colIndex, colIndex + mergeC));
        }
        /// <summary>
        /// 批量写入单元格
        /// </summary>
        /// <param name="rowIndex">行号</param>
        /// <param name="colIndex">列号</param>
        /// <param name="valuelist">值的集合</param>
        /// <param name="mergeRowNum">写入总行数</param>
        protected void wrCellMerge(int rowIndex, int colIndex, IList<string> valuelist, int mergeRowNum)
        {
            if (valuelist.Count == 0) return;
            if (valuelist.Count > mergeRowNum) throw new Exception("value list more then mergeRowNum");
            //计算
            var page = valuelist.Count; //多少页
            var remainder = mergeRowNum;  //余数
            var pagesize = Convert.ToInt32(Math.Floor(Convert.ToDecimal(mergeRowNum / page))); //分页
                                                                                               //循环设置值
            for (int i = 0; i < page; i++)
            {
                int currRowIndex = (i * pagesize) + rowIndex; //当前行
                int currMergeR = pagesize - 1;  //合并行数
                //最后一行时，取剩余数
                if (i == (page - 1))
                {
                    currRowIndex = (mergeRowNum - remainder) + rowIndex;
                    currMergeR = remainder - 1;
                }
                var value = valuelist[i];
                //设置值
                wrContent(value, currRowIndex, colIndex, currMergeR, 0);
                //减去当前页的数量
                remainder = remainder - pagesize;
            }
        }
        /// <summary>
        /// 设置样式
        /// </summary>
        protected void setCellStyle(string fontName = "方正小标宋_GBK", int fontSize = 24, bool isCenter = true)
        {
            _fontName = fontName;
            _fontSize = fontSize;
            _isCenter = isCenter;
        }
        /// <summary>
        /// 设置列宽度
        /// </summary>
        /// <param name="col"></param>
        /// <param name="w"></param>
        protected void SetColumnWidth(int col, int w)
        {
            _sheet.SetColumnWidth(col, w * 256);
        }

        #endregion

        #region 私有
        private string _fontName = "方正小标宋_GBK";
        private int _fontSize = 24;
        private bool _isCenter = true;
        private ICellStyle getStyle()
        {
            //样式
            ICellStyle style = _workbook.CreateCellStyle();
            if (_isCenter)
            {
                style.VerticalAlignment = VerticalAlignment.Center;
                style.Alignment = HorizontalAlignment.CenterSelection;//垂直居中
            }
            //自动换行
            style.WrapText = true;
            //创建字体
            XSSFFont font = (XSSFFont)_workbook.CreateFont();
            font.FontName = _fontName;
            font.FontHeight = _fontSize;
            style.SetFont(font);
            //设置边框
            //style.BorderBottom = BorderStyle.Thin;
            //style.BorderLeft = BorderStyle.Thin;
            //style.BorderRight = BorderStyle.Thin;
            //style.BorderTop = BorderStyle.Thin;
            //设置单元格为文本格式
            style.DataFormat = HSSFDataFormat.GetBuiltinFormat("@");
            return style;
        }
        /// <summary>
        /// 获取行
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private IRow GetRow(int i)
        {
            if (_sheet.LastRowNum == i && i == 0) return _sheet.CreateRow(0);
            if (i > _sheet.LastRowNum)
            {
                int c = i - _sheet.LastRowNum;
                for (int z = 1; z <= c; z++)
                {
                    _sheet.CreateRow(_sheet.LastRowNum + 1);
                }
            }
            return _sheet.GetRow(i);
        }
        /// <summary>
        /// 获取单元格
        /// </summary>
        /// <param name="row"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        private ICell GetCell(IRow row, int i)
        {
            while (i >= row.LastCellNum)
            {
                int rn = 0;
                if (row.LastCellNum != -1) rn = row.LastCellNum;
                ICell cell = row.CreateCell(rn);
                cell.CellStyle = getStyle();
            }
            return row.GetCell(i);
        }
        /// <summary>
        /// 设置单元格值
        /// </summary>
        /// <param name="value"></param>
        /// <param name="rowIndex"></param>
        /// <param name="colIndex"></param>
        private void setCell(object value, int rowIndex, int colIndex)
        {
            //获取行
            var row = GetRow(rowIndex);
            //获取列
            var cell = GetCell(row, colIndex);
            //设置值
            cell.SetCellValue(value.ToString());
            //设置样式
            cell.CellStyle = getStyle();
        }
        #endregion
    }
}
