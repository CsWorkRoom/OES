using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.HSSF.UserModel;
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
        /// 工作表最大记录条数
        /// </summary>
        private int _pageSize = 1000000;
        /// <summary>
        /// 对象实例
        /// </summary>
        private XSSFWorkbook _workbook;
        /// <summary>
        /// 
        /// </summary>
        private ISheet _sheet;
        /// <summary>
        /// 行号
        /// </summary>
        private int RowNum = 0;
        /// <summary>
        /// 
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
        /// 写表头
        /// </summary>
        /// <returns></returns>
        public void wrHeader(int num,object value)
        {
            IRow row = _sheet.CreateRow(0);
            ICell cell = row.CreateCell(0);
            cell.SetCellValue(value.ToString());
            //
            cell.CellStyle = GetStyle(Header_fontName, Header_fontSize);
            _sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(RowNum, RowNum, 0, num));
        }
        
        private string Header_fontName = "方正小标宋_GBK";
        private int Header_fontSize = 24;
        private bool Header_isCenter = true;
        public void setHeaderStyle(string fontName = "方正小标宋_GBK", int fontSize = 24, bool isCenter = true)
        {
            this.Header_fontName = fontName;
            this.Header_fontSize = fontSize;
            this.Header_isCenter = isCenter;
        }

        private int colIndex = 0;
        public void wrTitleAndLineCol(int mergeCol =0,params string[] titlename)
        {
            wrTitle(mergeCol, titlename);
            //换列
            wrTitleLineCol();
        }
        public void wrTitle(int mergeCol = 0,params string[] titlename)
        {
            for(int i = 0; i < titlename.Length; i++)
            {
                int rn = RowNum + i;
                IRow row = GetRow(rn);
                ICell cell = GetCell(row, colIndex);
                cell.SetCellValue(titlename[i]);
                cell.CellStyle = GetStyle(Title_fontName, Title_fontSize);
            }
            if (mergeCol > 0)
                _sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(RowNum, RowNum + mergeCol, colIndex, colIndex));
        }

        public int wrTitleLineCol()
        {
            colIndex = colIndex + 1;
            return colIndex;
        }

        private string Title_fontName = "方正小标宋_GBK";
        private int Title_fontSize = 10;
        private bool Title_isCenter = true;
        public void setTitleStyle(string fontName = "方正小标宋_GBK", int fontSize = 10, bool isCenter = true)
        {
            this.Title_fontName = fontName;
            this.Title_fontSize = fontSize;
            this.Title_isCenter = isCenter;
        }

        private IRow GetRow(int i)
        {
            if (i > _sheet.LastRowNum)
            {
                int c = i - _sheet.LastRowNum;
                for(int z = 1; z <= c; z++)
                {
                    _sheet.CreateRow(_sheet.LastRowNum + z);
                }
            }
            return _sheet.GetRow(i);
        }

        private ICell GetCell(IRow row, int i)
        {
            while(i >= row.LastCellNum)
            {
                int rn = 0;
                if (row.LastCellNum != -1) rn = row.LastCellNum;
                row.CreateCell(rn);
            }
            return row.GetCell(i);
        }
        /// <summary>
        /// 换行
        /// </summary>
        public void wrLineFeed()
        {
            RowNum += 1;
        }


        public ICellStyle GetStyle(string fontName = "方正小标宋_GBK", int fontSize = 24, bool isCenter = true)
        {
            //样式
            ICellStyle style = _workbook.CreateCellStyle();
            if (isCenter)
                style.Alignment = HorizontalAlignment.CenterSelection;//垂直居中
            //创建字体
            XSSFFont font = (XSSFFont)_workbook.CreateFont();
            font.FontName = fontName;
            font.FontHeight = fontSize;
            return style;
        }

        public string Save()
        {
            string fileName = _rootPath + "/" + DateTime.Now.Ticks.ToString() + ".xlsx";
            using (FileStream fs = File.OpenWrite(fileName))
            {
                _workbook.Write(fs);
            }
            return fileName;
        }
    }
}
