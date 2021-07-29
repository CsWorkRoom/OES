using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using com.force.json;
using System.Drawing;
using System.IO;

namespace CS.BLL.Extension
{

    public class LuckSheetByExcel
    {
        private string _rooPath;

        private JSONArray _data;

        private XSSFWorkbook excel;
        /// <summary>
        /// 构造函数
        /// </summary>
        public LuckSheetByExcel(string rooPath, string Json)
        {
            _data = new JSONObject(Json).GetJSONArray("data");
            excel = new XSSFWorkbook();
            _rooPath = rooPath;
            exportLuckySheetXlsxByNPOI();
        }

        /// <summary>
        /// 
        /// </summary>
        public void exportLuckySheetXlsxByNPOI()
        {
            //创建操作Excel的XSSFWorkbook对象
            for (int sheetIndex = 0; sheetIndex < _data.Length(); sheetIndex++)
            {
                //获取sheet
                JSONObject jsonObject = (JSONObject)_data.Get(sheetIndex);
                JSONArray celldataObjectList = jsonObject.GetJSONArray("celldata");//获取所有单元格（坐标，内容，字体类型，字体大小...）
                JSONArray rowObjectList = jsonObject.GetJSONArray("visibledatarow");
                JSONArray colObjectList = jsonObject.GetJSONArray("visibledatacolumn");
                JSONArray dataObjectList = jsonObject.GetJSONArray("data");//获取所有单元格,与celldata类似（坐标，内容，字体类型，字体大小...）
                JSONObject mergeObject = jsonObject.GetJSONObject("config").GetJSONObject("merge");//合并单元格
                JSONObject columnlenObject = jsonObject.GetJSONObject("config").GetJSONObject("columnlen");//表格列宽
                JSONObject rowlenObject = jsonObject.GetJSONObject("config").GetJSONObject("rowlen");//表格行高
                JSONArray borderInfoObjectList = jsonObject.GetJSONObject("config").GetJSONArray("borderInfo");//边框样式
                //参考：https://blog.csdn.net/jdtugfcg/article/details/84100315
                ICellStyle cellStyle = excel.CreateCellStyle();
                //创建XSSFSheet对象
                XSSFSheet sheet = (XSSFSheet)excel.CreateSheet(jsonObject.GetString("name"));
                //我们都知道excel是表格，即由一行一行组成的，那么这一行在java类中就是一个XSSFRow对象，我们通过XSSFSheet对象就可以创建XSSFRow对象
                //如：创建表格中的第一行（我们常用来做标题的行)  XSSFRow firstRow = sheet.createRow(0); 注意下标从0开始
                //根据luckysheet创建行列
                //创建行和列
                if (rowObjectList != null && rowObjectList.Length() > 0)
                {
                    for (int i = 0; i < rowObjectList.Length(); i++)
                    {
                        IRow row = sheet.CreateRow(i);//创建行
                        try
                        {
                            row.HeightInPoints = float.Parse(rowlenObject.Get(i.ToString()).ToString());
                        }
                        catch (Exception e)
                        {
                            row.HeightInPoints = 20f;
                        }

                        if (colObjectList != null && colObjectList.Length() > 0)
                        {
                            for (int j = 0; j < colObjectList.Length(); j++)
                            {
                                if (columnlenObject != null && columnlenObject.GetInt(j.ToString()) != 0)
                                {
                                    sheet.SetColumnWidth(j, columnlenObject.GetInt(j.ToString()) * 42);//列宽px值
                                }
                                row.CreateCell(j);//创建列
                            }
                        }
                    }
                }

                //设置值,样式
                setCellValue(celldataObjectList, borderInfoObjectList, sheet, excel);

            }
        }
        /// <summary>
        /// 設置值和样式
        /// </summary>
        /// <param name="jsonObjectList"></param>
        /// <param name="borderInfoObjectList"></param>
        /// <param name="sheet"></param>
        /// <param name="workbook"></param>
        public void setCellValue(JSONArray jsonObjectList, JSONArray borderInfoObjectList, XSSFSheet sheet, XSSFWorkbook workbook)
        {
            Dictionary<int, string> fontMap = new Dictionary<int, string>();
            fontMap.Add(-1, "Arial");
            fontMap.Add(0, "Times New Roman");
            fontMap.Add(1, "Arial");
            fontMap.Add(2, "Tahoma");
            fontMap.Add(3, "Verdana");
            fontMap.Add(4, "微软雅黑");
            fontMap.Add(5, "宋体");
            fontMap.Add(6, "黑体");
            fontMap.Add(7, "楷体");
            fontMap.Add(8, "仿宋");
            fontMap.Add(9, "新宋体");
            fontMap.Add(10, "华文新魏");
            fontMap.Add(11, "华文行楷");
            fontMap.Add(12, "华文隶书");

            //遍歷每一個單元格（先遍歷行，再遍歷列）
            for (int index = 0; index < jsonObjectList.Length(); index++)
            {

                XSSFCellStyle style = (XSSFCellStyle)workbook.CreateCellStyle();//样式
                IFont font = workbook.CreateFont();//字体样式
                                                   //獲取單元格
                JSONObject _object = jsonObjectList.GetJSONObject(index);
                if (_object.Opt("c") == null || _object.Opt("v") == null|| _object.Opt("r") ==null) continue;
                //str_ = 行坐標+列坐標=內容
                string str_ = (int)_object.Get("r") + "_" + _object.Get("c") + "=";
                if (((JSONObject)_object.Get("v")).Opt("v") == null)
                {

                }
                else {
                    str_ = str_ + ((JSONObject)_object.Get("v")).Get("v") + "\n";
                }
            
                JSONObject jsonObjectValue = ((JSONObject)_object.Get("v"));//獲取單元格樣式
                var keyList = jsonObjectValue.GetKeys();
                //單元格內容
                String value = "";
                if (jsonObjectValue != null && jsonObjectValue.Opt("v") != null)
                {
                   
                    value = jsonObjectValue.GetString("v");
                }
                else if(keyList.Contains("ct")&& jsonObjectValue.GetJSONObject("ct").Opt("s") != null)
                {
                    JSONArray Arr = jsonObjectValue.GetJSONObject("ct").GetJSONArray("s");
                    for(var gg = 0; gg < Arr.Length(); gg++)
                    {
                        value += ((JSONObject)Arr.Get(gg)).GetString("v");
                    }
                }
                else
                {
                    
                }
                if (sheet.GetRow((int)_object.Get("r")) != null && sheet.GetRow((int)_object.Get("r")).GetCell((int)_object.Get("c")) != null)
                {
                    XSSFCell cell = (XSSFCell)sheet.GetRow((int)_object.Get("r")).GetCell((int)_object.Get("c"));
                    //設置公式 注意：luckysheet与Java的公式可能存在不匹配问题，例如js的Int(data)
                    if (jsonObjectValue != null && keyList.Contains("f"))
                    {//如果有公式，设置公式
                        value = jsonObjectValue.GetString("f");
                        cell.SetCellFormula(value.Substring(1, value.Length));//不需要=符号,例：INT(12.3)
                    }
                    //合并单元格与填充单元格颜色
                    setMergeAndColorByObject(jsonObjectValue, sheet, style);
                    //填充值
                    cell.SetCellValue(value);
                    IRow row = sheet.GetRow((int)_object.Get("r"));

                    if (jsonObjectValue.Opt("vt") != null && jsonObjectValue.Opt("ht") != null)
                    {
                        //设置垂直水平对齐方式
                        int vt = jsonObjectValue.GetInt("vt") == 0 ? 1 : (int)jsonObjectValue.Get("vt");//垂直对齐	 0 中间、1 上、2下
                        int ht = jsonObjectValue.GetInt("ht") == 0 ? 1 : (int)jsonObjectValue.Get("ht");//0 居中、1 左、2右
                        switch (vt)
                        {
                            case 0:
                                style.VerticalAlignment = VerticalAlignment.Center;
                                break;
                            case 1:
                                style.VerticalAlignment = VerticalAlignment.Top;
                                break;
                            case 2:
                                style.VerticalAlignment = VerticalAlignment.Bottom;
                                break;
                        }
                        switch (ht)
                        {
                            case 0:
                                style.Alignment = HorizontalAlignment.Center;
                                break;
                            case 1:
                                style.Alignment = HorizontalAlignment.Left;
                                break;
                            case 2:
                                style.Alignment = HorizontalAlignment.Right;
                                break;
                        }
                    }
                    //0 Times New Roman、 1 Arial、2 Tahoma 、3 Verdana、4 微软雅黑、5 宋体（Song）、6 黑体（ST Heiti）、7 楷体（ST Kaiti）、 8 仿宋（ST FangSong）、9 新宋体（ST Song）、10 华文新魏、11 华文行楷、12 华文隶书
                    int fs = jsonObjectValue.Opt("fs") == null ? 14 : (int)jsonObjectValue.GetInt("fs");//字体大小
                    int bl = jsonObjectValue.Opt("bl") == null ? 0 : (int)jsonObjectValue.GetInt("bl");//粗体	0 常规 、 1加粗
                    int it = jsonObjectValue.Opt("it") == null ? 0 : (int)jsonObjectValue.GetInt("it");//斜体	0 常规 、 1 斜体
                    string fc = jsonObjectValue.Opt("fc") == null ? "" : jsonObjectValue.GetString("fc");//字体颜色
                    //字體顏色
                    if (fc.Length > 0)
                    {
                        font.Color = Convert.ToInt16(fc.Replace("#", ""));
                    }
                    //设置合并单元格的样式有问题
                    if (jsonObjectValue.Opt("ff") != null)
                    {
                        string ff = jsonObjectValue.GetString("ff");
                        font.FontName = ff;//字体名字
                    }
                    font.FontHeightInPoints = (short)fs;  //字体大小
                    if (bl == 1)
                    {
                        font.IsBold = true; //粗体显示
                    }
                    //是否斜體
                    font.IsItalic = (it == 1 ? true : false); //斜体
                    style.SetFont(font);
                    //style.WrapText = true;     //设置自动换行
                    cell.CellStyle = style;

                }
                else
                {
                    throw new Exception("错误的=" + index + ">>>" + str_);
                }
                //设置边框
                setBorder(borderInfoObjectList, workbook, sheet);

            }
        }

        /// <summary>
        /// 合并单元格与填充单元格颜色
        /// </summary>
        /// <param name="jsonObjectValue"></param>
        /// <param name="sheet"></param>
        /// <param name="style"></param>
        public void setMergeAndColorByObject(JSONObject jsonObjectValue, XSSFSheet sheet, XSSFCellStyle style)
        {
            if (jsonObjectValue.Opt("mc") == null) return;
            JSONObject mergeObject = (JSONObject)jsonObjectValue.Get("mc");
            var keyList = mergeObject.GetKeys();
            //合併單元格
            if (mergeObject != null)
            {
                int r = (int)(mergeObject.Get("r"));
                int c = (int)(mergeObject.Get("c"));
                if ((mergeObject.Opt("rs") != null && (mergeObject.Opt("cs") != null)))
                {
                    int rs = (int)(mergeObject.Get("rs"));
                    int cs = (int)(mergeObject.Get("cs"));
                    sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(r, r + rs - 1, (short)(c), (short)(c + cs - 1)));
                }
            }
            //填充顏色
            if (keyList.Contains("bg"))
            {
                short bg = Convert.ToInt16(jsonObjectValue.GetString("bg").Replace("#", ""));
                style.FillPattern = FillPattern.SolidForeground;                  //设置填充方案
                style.SetFillForegroundColor(new XSSFColor(Color.FromArgb(bg)));  //设置填充颜色
            }
        }
        /// <summary>
        /// 设置边框
        /// </summary>
        /// <param name="borderInfoObjectList"></param>
        /// <param name="workbook"></param>
        /// <param name="sheet"></param>
        public void setBorder(JSONArray borderInfoObjectList, XSSFWorkbook workbook, XSSFSheet sheet)
        {
            //设置边框样式map
            Dictionary<int, BorderStyle> bordMap = new Dictionary<int, BorderStyle>();
            bordMap.Add(1, BorderStyle.Thin);
            bordMap.Add(2, BorderStyle.Hair);
            bordMap.Add(3, BorderStyle.Dotted);
            bordMap.Add(4, BorderStyle.Dashed);
            bordMap.Add(5, BorderStyle.DashDot);
            bordMap.Add(6, BorderStyle.DashDotDot);
            bordMap.Add(7, BorderStyle.Double);
            bordMap.Add(8, BorderStyle.Medium);
            bordMap.Add(9, BorderStyle.MediumDashed);
            bordMap.Add(10, BorderStyle.MediumDashDot);
            bordMap.Add(11, BorderStyle.MediumDashDotDot);
            bordMap.Add(12, BorderStyle.SlantedDashDot);
            bordMap.Add(13, BorderStyle.Thick);
            // 一定要通过 cell.getCellStyle()  不然的话之前设置的样式会丢失
            //设置边框
            if (borderInfoObjectList != null && borderInfoObjectList.Length() > 0)
            {
                for (int i = 0; i < borderInfoObjectList.Length(); i++)
                {
                    JSONObject borderInfoObject = (JSONObject)borderInfoObjectList.Get(i);
                    if (borderInfoObject.Get("rangeType").Equals("cell"))
                    {//单个单元格
                        JSONObject borderValueObject = borderInfoObject.GetJSONObject("value");


                        int row = borderValueObject.GetInt("row_index");
                        int col = borderValueObject.GetInt("col_index");

                        XSSFCell cell = (XSSFCell)sheet.GetRow(row).GetCell(col);

                        if (borderInfoObject.Opt("l") == null)
                        {
                            JSONObject l = borderValueObject.GetJSONObject("l");
                            if (l != null)
                            {
                                cell.CellStyle.BorderLeft = bordMap[(int)l.Get("style")];     //左边框
                                short bg = Convert.ToInt16(l.GetString("color").Replace("#", ""));
                                cell.CellStyle.LeftBorderColor = bg;//左边框颜色
                            }
                        }
                        if (borderInfoObject.Opt("r") != null)
                        {
                            JSONObject r = borderValueObject.GetJSONObject("r");

                            if (r != null)
                            {
                                cell.CellStyle.BorderRight = bordMap[(int)r.Get("style")];     //右边框
                                short bg = Convert.ToInt16(r.GetString("color").Replace("#", ""));
                                cell.CellStyle.RightBorderColor = bg;//右边框颜色
                            }
                        }
                        if (borderInfoObject.Opt("t") != null)
                        {
                            JSONObject t = borderValueObject.GetJSONObject("t");
                            if (t != null)
                            {
                                cell.CellStyle.BorderTop = bordMap[(int)t.Get("style")];     //顶部边框
                                short bg = Convert.ToInt16(t.GetString("color").Replace("#", ""));
                                cell.CellStyle.TopBorderColor = bg;//顶部边框颜色
                            }
                        }
                        if (borderInfoObject.Opt("b") != null)
                        {
                            JSONObject b = borderValueObject.GetJSONObject("b");
                            if (b != null)
                            {
                                cell.CellStyle.BorderBottom = bordMap[(int)b.Get("style")];     //顶部边框
                                short bg = Convert.ToInt16(b.GetString("color").Replace("#", ""));
                                cell.CellStyle.BottomBorderColor = bg;//顶部边框颜色
                            }
                        }
                    }
                    else if (borderInfoObject.Get("rangeType").Equals("range"))
                    {//选区
                        //short bg_ = Convert.ToInt16(borderInfoObject.GetString("color").Replace("#", ""));
                        //int style_ = borderInfoObject.GetInt("style");

                        //JSONObject rangObject = (JSONObject)((JSONArray)(borderInfoObject.Get("range"))).Get(0);

                        //JSONArray rowList = rangObject.GetJSONArray("row");
                        //JSONArray columnList = rangObject.GetJSONArray("column");


                        //for (int row_ = rowList.getInteger(0); row_ < rowList.getInteger(rowList.size() - 1) + 1; row_++)
                        //{
                        //    for (int col_ = columnList.getInteger(0); col_ < columnList.getInteger(columnList.size() - 1) + 1; col_++)
                        //    {
                        //        XSSFCell cell = sheet.getRow(row_).getCell(col_);

                        //        cell.getCellStyle().setBorderLeft(bordMap.Get(style_)); //左边框
                        //        cell.getCellStyle().setLeftBorderColor(new XSSFColor(new Color(bg_)));//左边框颜色
                        //        cell.getCellStyle().setBorderRight(bordMap.Get(style_)); //右边框
                        //        cell.getCellStyle().setRightBorderColor(new XSSFColor(new Color(bg_)));//右边框颜色
                        //        cell.getCellStyle().setBorderTop(bordMap.Get(style_)); //顶部边框
                        //        cell.getCellStyle().setTopBorderColor(new XSSFColor(new Color(bg_)));//顶部边框颜色
                        //        cell.getCellStyle().setBorderBottom(bordMap.Get(style_)); //底部边框
                        //        cell.getCellStyle().setBottomBorderColor(new XSSFColor(new Color(bg_)));//底部边框颜色 }
                        //    }
                        //}


                    }
                }
            }
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        public string Save()
        {
            string fileName = _rooPath + "/" + DateTime.Now.Ticks.ToString() + ".xlsx";
            using (FileStream fs = File.OpenWrite(fileName))
            {
                excel.Write(fs);
            }
            return fileName;
        }
    }
}