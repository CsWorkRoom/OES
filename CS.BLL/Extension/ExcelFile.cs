/* =====================================================================================
 * 简要描述：Excel文件操作类
 * 主要功能：EXCEL文件
 * 编写日期：2018-04-27
 * 修改日期：
 
 * 
 * =====================================================================================
 */

using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace CS.BLL.Extension.Export
{
    /// <summary>
    /// EXCEL文件
    /// </summary>
    public class ExcelFile
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
        /// 构造函数
        /// </summary>
        /// <param name="path">存放文件的根目录</param>
        public ExcelFile(string path)
        {
            if (Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }
            _rootPath = new DirectoryInfo(path).FullName;
        }

        /// <summary>
        /// 导出文件到EXCEL
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public string ToExcel(DataTable dt)
        {
            if (dt == null)
            {
                return string.Empty;
            }

            string fileName = _rootPath + "/" + DateTime.Now.Ticks.ToString() + ".xlsx";
            Delete(fileName);

            XSSFWorkbook wk = new XSSFWorkbook();
            ISheet sheet = null;
            IRow row = null;
            IDataFormat format = wk.CreateDataFormat();
            //日期
            ICellStyle dateStyle = wk.CreateCellStyle();
            dateStyle.DataFormat = format.GetFormat("yyyy-MM-dd HH:mm");
            //浮点数
            ICellStyle floatStyle = wk.CreateCellStyle();
            floatStyle.DataFormat = format.GetFormat("0.00");

            int i = 0;
            int sheetCount = 0;
            if (dt.Rows.Count < 1)
            {
                sheetCount++;
                sheet = wk.CreateSheet("Sheet" + sheetCount);
                row = sheet.CreateRow(0);
                for (int c = 0; c < dt.Columns.Count; c++)
                {
                    ICell cell = row.CreateCell(c);
                    cell.SetCellValue(dt.Columns[c].Caption);
                    //设置日期格式
                    if (dt.Columns[c].DataType == typeof(DateTime))
                    {
                        sheet.SetColumnWidth(c, 20 * 256);
                    }
                }
            }
            foreach (DataRow dr in dt.Rows)
            {
                if (i >= _pageSize)
                {
                    i = 0;
                }

                if (i == 0)
                {
                    sheetCount++;
                    sheet = wk.CreateSheet("Sheet" + sheetCount);
                    row = sheet.CreateRow(0);
                    for (int c = 0; c < dt.Columns.Count; c++)
                    {
                        ICell cell = row.CreateCell(c);
                        cell.SetCellValue(dt.Columns[c].Caption);
                        //设置日期格式
                        if (dt.Columns[c].DataType == typeof(DateTime))
                        {
                            sheet.SetColumnWidth(c, 20 * 256);
                        }

                        //Base.Log.BLog.Write(Base.Log.BLog.LogLevel.DEBUG, c + "\t" + dt.Columns[c].ColumnName + "\t" + dt.Columns[c].Caption + "\t" + dt.Columns[c].DataType.ToString());
                    }
                }

                i++;
                row = sheet.CreateRow(i);
                for (int c = 0; c < dt.Columns.Count; c++)
                {
                    ICell cell = row.CreateCell(c);
                    if (dt.Columns[c].DataType == typeof(DateTime))
                    {
                        DateTime vd = new DateTime();
                        if (DateTime.TryParse(dr[c].ToString(), out vd))
                        {
                            cell.SetCellValue(vd);
                            cell.CellStyle = dateStyle;
                        }
                    }
                    else if (dt.Columns[c].DataType != typeof(string))
                    {
                        float vf = 0;
                        if (float.TryParse(dr[c].ToString(), out vf))
                        {
                            if (vf > int.MaxValue)
                            {
                                //cell.SetCellValue(vf.ToString("0"));
                                cell.SetCellValue(dr[c].ToString());
                            }
                            else
                            {
                                int vi = Convert.ToInt32(vf);
                                if (vi == vf)
                                {
                                    cell.SetCellValue(vi);
                                }
                                else
                                {
                                    cell.SetCellValue(Math.Round(vf, 2));
                                }
                                //cell.SetCellValue(vf);
                            }
                        }
                        else
                        {
                            cell.SetCellValue(dr[c].ToString());
                        }
                    }
                    else
                    {
                        cell.SetCellValue(dr[c].ToString());
                    }
                }
            }

            //打开一个xls文件，如果没有则自行创建，如果存在myxls.xls文件则在创建时不要打开该文件  
            using (FileStream fs = File.OpenWrite(fileName))
            {
                wk.Write(fs);
            }

            return fileName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="datatable"></param>
        /// <param name="sheetIndex"></param>
        /// <returns></returns>
        public string ToRepalceExcel(string filename, DataTable datatable, Dictionary<string, string> dic,int sheetIndex = 0)
        {
            int startWriteIndex = 0;
            
            //读取
            IWorkbook workBook = FindxlsForKeyInfo(filename, dic, ref startWriteIndex);
            return IWorkbookToExcel(workBook, startWriteIndex, datatable, sheetIndex);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="sheetIndex"></param>
        /// <returns></returns>
        private IWorkbook FindxlsForKeyInfo(string filename, Dictionary<string,string> dic, ref int startWriteIndex, int sheetIndex = 0)
        {
            string name = _rootPath + "/" + filename;
            if (File.Exists(name) == false)
            {
                throw new Exception("文件" + name + "不存在");
            }

            IWorkbook workBook = null;
            using (FileStream fs = File.OpenRead(name))
            {
                if (name.ToLower().EndsWith(".xls"))
                {
                    workBook = new HSSFWorkbook(fs);
                }
                else if (name.ToLower().EndsWith(".xlsx"))
                {
                    workBook = new XSSFWorkbook(fs);
                }
            }
            ISheet sheet = workBook.GetSheetAt(sheetIndex);
            //读取工作表
            for (int r = 1; r <= sheet.LastRowNum; r++)
            {
                IRow row = sheet.GetRow(r);
                if (row == null)
                {
                    continue;
                }
                for (int i = 0; i <= row.LastCellNum; i++)
                {
                    ICell cell = row.GetCell(i);
                    if (cell == null) continue;
                    if (cell.ToString() == "<START_WRITE_DATA>")
                    {
                        startWriteIndex = r;
                    }
                    try
                    {
                        var value1 = dic[cell.ToString()];
                        if (value1 != null)
                            cell.SetCellValue(value1);
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
            return workBook;
        }

        /// <summary>
        /// 导出文件到EXCEL
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private string IWorkbookToExcel(IWorkbook wk, int startWriteIndex, DataTable dt,int sheetIndex=0)
        {
            if (startWriteIndex == 0) return string.Empty;
            if (dt == null)
            {
                return string.Empty;
            }
            string filen = DateTime.Now.Ticks.ToString() + ".xlsx"; 
            string fileName = _rootPath + "/" + filen;
            Delete(fileName);
            ISheet sheet = wk.GetSheetAt(sheetIndex);
            IRow row = null;
            IDataFormat format = wk.CreateDataFormat();
            //日期
            ICellStyle dateStyle = wk.CreateCellStyle();
            dateStyle.DataFormat = format.GetFormat("yyyy-MM-dd HH:mm");
            //浮点数
            ICellStyle floatStyle = wk.CreateCellStyle();
            floatStyle.DataFormat = format.GetFormat("0.00");

            int i = startWriteIndex;
            //IRow rs = sheet.GetRow(startWriteIndex);
            //sheet.RemoveRow(rs);
            //sheet.ShiftRows(i, sheet.LastRowNum , dt.Rows.Count, true, false);
            foreach (DataRow dr in dt.Rows)
            {
                //row = sheet.CreateRow(i);
                if (startWriteIndex != i)
                    row = sheet.CopyRow(startWriteIndex, i);
                else
                    row = sheet.GetRow(i);

                for (int c = 0; c < dt.Columns.Count; c++)
                {
                    //ICell cell = row.CreateCell(c);
                    ICell cell = row.GetCell(c);
                    if (dt.Columns[c].DataType == typeof(DateTime))
                    {
                        DateTime vd = new DateTime();
                        if (DateTime.TryParse(dr[c].ToString(), out vd))
                        {
                            cell.SetCellValue(vd);
                            cell.CellStyle = dateStyle;
                        }
                    }
                    else if (dt.Columns[c].DataType != typeof(string))
                    {
                        float vf = 0;
                        if (float.TryParse(dr[c].ToString(), out vf))
                        {
                            if (vf > int.MaxValue)
                            {
                                cell.SetCellValue(dr[c].ToString());
                            }
                            else
                            {
                                int vi = Convert.ToInt32(vf);
                                if (vi == vf)
                                {
                                    cell.SetCellValue(vi);
                                }
                                else
                                {
                                    cell.SetCellValue(Math.Round(vf, 2));
                                }
                            }
                        }
                        else
                        {
                            cell.SetCellValue(dr[c].ToString());
                        }
                    }
                    else
                    {
                        cell.SetCellValue(dr[c].ToString());
                    }
                }
                i++;
            }
            //打开一个xls文件，如果没有则自行创建，如果存在myxls.xls文件则在创建时不要打开该文件  
            using (FileStream fs = File.OpenWrite(fileName))
            {
                wk.Write(fs);
            }

            return filen;
        }
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fileName"></param>
        public void Delete(string fileName)
        {
            if (File.Exists(fileName) == true)
            {
                File.Delete(fileName);
            }
        }

        /// <summary>
        /// 加载工作表数据到DataTable里面
        /// </summary>
        /// <param name="filename">文件名（不包含目录路径）</param>
        /// <param name="datatable">DataTable</param>
        /// <param name="sheetIndex">工作表序号</param>
        /// <returns></returns>
        public void ToDataTable(string filename, ref DataTable datatable, int sheetIndex = 0)
        {
            string name = _rootPath + "/" + filename;
            if (File.Exists(name) == false)
            {
                throw new Exception("文件" + name + "不存在");
            }

            IWorkbook workBook = null;
            using (FileStream fs = File.OpenRead(name))
            {
                if (name.ToLower().EndsWith(".xls"))
                {
                    workBook = new HSSFWorkbook(fs);
                }
                else if (name.ToLower().EndsWith(".xlsx"))
                {
                    workBook = new XSSFWorkbook(fs);
                }
            }
            ISheet sheet = workBook.GetSheetAt(sheetIndex);

            //读取工作表
            for (int r = 1; r <= sheet.LastRowNum; r++)
            {
                IRow row = sheet.GetRow(r);
                if (row == null)
                {
                    continue;
                }
                DataRow dr = datatable.NewRow();
                int c = 0;
                foreach (DataColumn col in datatable.Columns)
                {
                    ICell cell = row.GetCell(c);
                    if (cell != null)
                    {
                        //日期类型
                        if (col.DataType == typeof(DateTime))
                        {
                            dr[col] = cell.DateCellValue;
                        }
                        else if (col.DataType == typeof(string))
                        {
                            dr[col] = cell.ToString();
                        }
                        else
                        {
                            dr[col] = cell.NumericCellValue;
                        }
                    }
                    c++;
                }
                datatable.Rows.Add(dr);
            }
        }
    }
}

