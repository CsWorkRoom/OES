using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.XWPF.UserModel;
using NPOI.OpenXmlFormats.Wordprocessing;
using NPOI.OpenXmlFormats.Dml.WordProcessing;
using System.Data;
using System.IO;

namespace CS.BLL.Extension.Export
{
    /// <summary>
    /// Word文件
    /// </summary>
    public class WordFile
    {
        /// <summary>
        /// 文档对象
        /// </summary>
        private XWPFDocument doc = new XWPFDocument();
        /// <summary>
        /// 数字编号定义
        /// </summary>
        private string numberID = "1";
        /// <summary>
        /// 是否已经设置过多级编号
        /// </summary>
        private bool _isSetedNumberGallery = false;
        /// <summary>
        /// 是否已经设置标题样式
        /// </summary>
        private bool _isSetedChapterStyle = false;
        /// <summary>
        /// 标题字体大小
        /// </summary>
        private int[] _fontSizes = new int[] { 36, 28, 24, 20, 16, 12, 12, 12, 12, 12 };

        /// <summary>
        /// 默认字体
        /// </summary>
        private string _fontFamily = "宋体";
        /// <summary>
        /// 正文文字大小
        /// </summary>
        private int _fontSize = 16;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fontFamily">正文字体</param>
        /// <param name="fontSize">正文字体大小</param>
        public WordFile(string fontFamily = "宋体", int fontSize = 16)
        {
            _fontFamily = fontFamily;
            _fontSize = Math.Min(400, Math.Max(8, fontSize));
        }

        public WordFile(string path, string filename)
        {
            Stream stream = File.OpenRead(path + filename);
            doc = new XWPFDocument(stream);
            _fontFamily = "宋体";
            _fontSize = Math.Min(400, Math.Max(8, 16));
        }

        /// <summary>
        /// 获取文档对象
        /// </summary>
        /// <returns></returns>
        public XWPFDocument GetDocument()
        {
            return doc;
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string Save(string fileName)
        {
            try
            {
                using (FileStream sw = File.Create(fileName))
                {
                    doc.Write(sw);
                }
                return fileName;
            }
            catch (Exception ex)
            {
                throw new Exception("保存Word文档出错：" + ex.Message);
            }
        }

        /// <summary>
        /// 添加段落
        /// </summary>
        /// <param name="content">内容</param>
        /// <returns></returns>
        public XWPFParagraph AddParagraph(string content)
        {
            return AddParagraph(content, ParagraphAlignment.LEFT, _fontSize, _fontFamily, true, false);
        }

        /// <summary>
        /// 添加段落
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="alignment">对齐方式</param>
        /// <param name="fontSize">字号</param>
        /// <param name="fontFamily">字体</param>
        /// <param name="isIndentation">是否首行缩进</param>
        /// <param name="isBold">是否加粗</param>
        /// <returns></returns>
        public XWPFParagraph AddParagraph(string content, ParagraphAlignment alignment, int fontSize = 16, string fontFamily = "宋体", bool isIndentation = true, bool isBold = false)
        {
            XWPFParagraph paragraph = doc.CreateParagraph();
            paragraph.Alignment = alignment;
            if (isIndentation == true)
            {
                //1英寸 = 1440缇     1厘米 = 567缇     1磅 = 20缇     1像素 = 15缇
                //常用页面尺寸：（单位Twip）
                //A4（纵向）：W = 11906     H = 16838
                paragraph.IndentationFirstLine = fontSize * 20 * 2;
            }
            XWPFRun run = paragraph.CreateRun();
            if (fontSize > 0)
            {
                run.FontSize = fontSize;
            }
            run.FontFamily = fontFamily;
            run.IsBold = isBold;
            run.SetText(content);

            return paragraph;
        }

        /// <summary>
        /// 添加顶级标题
        /// </summary>
        /// <param name="title"></param>
        /// <param name="fontSize"></param>
        public void AddTitle(string title, int fontSize = 36, string fontFamily = "黑体", bool isIndentation = false, bool isBold = true)
        {
            AddParagraph(title, ParagraphAlignment.CENTER, fontSize, fontFamily, isIndentation, isBold);
        }

        /// <summary>
        /// 添加章节标题
        /// </summary>
        /// <param name="title"></param>
        /// <param name="fontSize"></param>
        /// <param name="fontFamily"></param>
        /// <param name="isIndentation"></param>
        /// <param name="isBold"></param>
        public void AddChapter(string title, int fontSize, string fontFamily, bool isIndentation = true, bool isBold = true)
        {
            AddParagraph(title, ParagraphAlignment.LEFT, fontSize, fontFamily, isIndentation, isBold);
        }

        /// <summary>
        /// 添加章节
        /// </summary>
        /// <param name="title">章节标题</param>
        /// <param name="level">层级（从1开始）</param>
        /// <param name="isAddSpaceParagraph">是否在下方添加空白段落</param>
        public void AddChapter(string title, int level, bool isAddSpaceParagraph = false)
        {
            //设置样式
            SetStyles();
            //设置编号
            SetNumberGallery();

            XWPFParagraph paragraph = AddParagraph(title, ParagraphAlignment.LEFT, _fontSizes[level], _fontFamily, false, true);
            paragraph.Style = "标题" + level;
            paragraph.SetNumID(numberID, (level - 1).ToString());
            //添加空白段落
            if (isAddSpaceParagraph == true)
            {
                AddParagraph("", ParagraphAlignment.LEFT, _fontSize);
            }
        }

        #region 添加线条

        /// <summary>
        /// 添加线条
        /// </summary>
        /// <param name="top"></param>
        public XWPFParagraph AddLine(int top)
        {
            string el = string.Empty;
            el = "<v:rect id=\"_x0000_i1026\" style=\"width:0;height:1.5pt\" o:hralign=\"center\" o:hrstd=\"t\" o:hr=\"t\" fillcolor=\"#a0a0a0\" stroked=\"f\"/>";
            XWPFParagraph paragraph = doc.CreateParagraph();


            //paragraph.Alignment = ParagraphAlignment.LEFT;

            XWPFRun run = paragraph.CreateRun();
            //CT_InLine inline = run.GetCTR().AddNewDrawing().AddNewInline();

            //CT_Drawing drawing = run.GetCTR().AddNewDrawing();
            //CT_Inline inline = drawing.AddNewInline();
            //inline.graphic = new NPOI.OpenXmlFormats.Dml.CT_GraphicalObject();
            //inline.graphic.graphicData = new NPOI.OpenXmlFormats.Dml.CT_GraphicalObjectData();
            //inline.graphic.graphicData.uri = "";
            //inline.graphic.graphicData.AddPicElement(el);
            //CT_Picture pic= run.GetCTR().AddNewPict();
            //CT_R r = run.GetCTR();

            //pic.Items = new List<System.Xml.XmlNode>();
            //pic.Items.Add(null);
            //pic.Items[0].InnerXml = el;
            CT_CustomXmlRun xr = new CT_CustomXmlRun();
            xr.element = el;


            return paragraph;
        }

        #endregion

        #region 标题样式

        private void SetStyles()
        {
            if (_isSetedChapterStyle == true)
            {
                return;
            }

            for (int i = 1; i <= 9; i++)
            {
                string styleID = "标题" + i;
                SetStyle(i, "000000");
            }

            _isSetedChapterStyle = true;
        }


        /// <summary>
        /// 添加标题样式
        /// </summary>
        /// <param name="level"></param>
        /// <param name="hexColor"></param>
        private void SetStyle(int level, string hexColor = "000000")
        {
            string styleID = "标题" + level;
            int fontSize = _fontSizes[level];

            CT_Style ctStyle = new CT_Style();
            ctStyle.styleId = styleID;
            ctStyle.name = new CT_String();
            ctStyle.name.val = styleID;
            ctStyle.uiPriority = new CT_DecimalNumber();
            ctStyle.uiPriority.val = level.ToString();
            ctStyle.unhideWhenUsed = new CT_OnOff();
            ctStyle.qFormat = new CT_OnOff();
            ctStyle.pPr = new CT_PPr();
            ctStyle.pPr.outlineLvl = ctStyle.uiPriority;

            ctStyle.rPr = new CT_RPr();
            ctStyle.rPr.rFonts = new CT_Fonts();
            ctStyle.rPr.rFonts.ascii = _fontFamily;
            ctStyle.rPr.rFonts.eastAsia = _fontFamily;
            ctStyle.rPr.sz = new CT_HpsMeasure();
            ctStyle.rPr.sz.val = Convert.ToUInt64(fontSize);
            ctStyle.rPr.szCs = new CT_HpsMeasure();
            ctStyle.rPr.szCs.val = Convert.ToUInt64(fontSize);

            ctStyle.rPr.color = new CT_Color();
            ctStyle.rPr.color.val = hexColor;

            XWPFStyle style = new XWPFStyle(ctStyle);
            style.StyleType = ST_StyleType.paragraph;

            XWPFStyles styles = doc.CreateStyles();
            styles.AddStyle(style);
        }

        #endregion

        #region 多级编号

        /// <summary>
        /// 设置多级编号
        /// </summary>
        private void SetNumberGallery()
        {
            if (_isSetedNumberGallery == true)
            {
                return;
            }

            CT_AbstractNum ctAbstractNum = new CT_AbstractNum();
            ctAbstractNum.abstractNumId = numberID;
            ctAbstractNum.multiLevelType = new CT_MultiLevelType();
            ctAbstractNum.multiLevelType.val = ST_MultiLevelType.multilevel;
            ctAbstractNum.lvl = new List<CT_Lvl>();
            string format = string.Empty;

            //默认定义1到9级
            for (int i = 0; i < 9; i++)
            {
                CT_Lvl lvl = new CT_Lvl();
                lvl.ilvl = i.ToString();
                lvl.start = new CT_DecimalNumber();
                lvl.start.val = "1";
                lvl.numFmt = new CT_NumFmt();
                lvl.numFmt.val = ST_NumberFormat.@decimal;
                lvl.lvlText = new CT_LevelText();
                lvl.lvlText.val = format + "%" + (i + 1) + ".";
                lvl.lvlJc.val = ST_Jc.left;
                lvl.pPr = new CT_PPr();
                lvl.pPr.ind = new CT_Ind();
                lvl.pPr.ind.left = "0";
                lvl.pPr.ind.hanging = 0;
                lvl.pPr.rPr = new CT_ParaRPr();
                lvl.pPr.rPr.sz = new CT_HpsMeasure();
                lvl.pPr.rPr.sz.val = Convert.ToUInt64(_fontSizes[i]);
                lvl.pPr.rPr.szCs = new CT_HpsMeasure();
                lvl.pPr.rPr.szCs.val = Convert.ToUInt64(_fontSizes[i]);

                //lvl.pStyle = new CT_String();
                //lvl.pStyle.val = "标题" + (i + 1);
                ctAbstractNum.lvl.Add(lvl);

                format += "%" + (i + 1) + ".";
            }

            //创建编号
            XWPFNumbering numbering = doc.CreateNumbering();

            XWPFAbstractNum abstractNum = new XWPFAbstractNum(ctAbstractNum, numbering);
            numbering.AddAbstractNum(abstractNum);

            CT_Num ctNum = new CT_Num();
            ctNum.numId = numberID;
            ctNum.abstractNumId = new CT_DecimalNumber();
            ctNum.abstractNumId.val = numberID;

            XWPFNum num = new XWPFNum(ctNum, numbering);
            numbering.AddNum(num);

            _isSetedNumberGallery = true;
        }

        #endregion

        #region 添加表格

        /// <summary>
        /// 添加一个表
        /// </summary>
        /// <param name="table">表格</param>
        /// <param name="fontFamily">字体</param>
        /// <param name="fontSize">字体大小</param>
        /// <param name="widthList">列宽度（如果为空则自动平分）</param>
        /// <param name="isFull">是否全宽</param>
        /// <param name="titleIsBlod">首行标题是否加粗</param>
        public void AddTable(DataTable table, string fontFamily = "宋体", int fontSize = 10, List<int> widthList = null, bool isFull = true, bool titleIsBlod = false)
        {
            if (table == null || table.Columns.Count < 1)
            {
                return;
            }
            int fullWidth = 8520;
            int[] wl = new int[table.Columns.Count];
            if (widthList == null)
            {
                for (int c = 0; c < wl.Length; c++)
                {
                    wl[c] = fullWidth / table.Columns.Count;
                }
            }
            else
            {
                int totalWidth = Math.Max(1, widthList.Sum());

                if (widthList.Count >= table.Columns.Count)
                {
                    if (isFull == true)
                    {
                        for (int c = 0; c < wl.Length; c++)
                        {
                            wl[c] = Convert.ToInt32(1.0 * widthList[c] / totalWidth * fullWidth);
                        }
                    }
                    else
                    {
                        for (int c = 0; c < wl.Length; c++)
                        {
                            wl[c] = widthList[c];
                        }
                    }
                }
                else
                {
                    for (int c = widthList.Count; c < wl.Length; c++)
                    {
                        wl[c] = Math.Abs((fullWidth - totalWidth) / (wl.Length - widthList.Count));
                    }
                }
            }

            XWPFTable tb = doc.CreateTable(table.Rows.Count + 1, table.Columns.Count);

            //标题
            for (int c = 0; c < table.Columns.Count; c++)
            {
                DataColumn col = table.Columns[c];

                CT_TcPr m_Pr = tb.GetRow(0).GetCell(c).GetCTTc().AddNewTcPr();
                m_Pr.tcW = new CT_TblWidth();
                if (isFull == true)
                {
                    m_Pr.tcW.w = "";
                }
                //m_Pr.tcW.w = (500 + c * 100).ToString();
                m_Pr.tcW.w = wl[c].ToString();
                m_Pr.tcW.type = ST_TblWidth.dxa;

                XWPFParagraph par = tb.GetRow(0).GetCell(c).Paragraphs[0];
                XWPFRun run = par.CreateRun();
                run.FontFamily = string.IsNullOrWhiteSpace(fontFamily) ? _fontFamily : fontFamily;
                run.FontSize = fontSize <= 0 ? _fontSize : fontSize;
                run.IsBold = titleIsBlod;
                run.SetText(col.Caption);
            }

            int i = 0;
            foreach (DataRow row in table.Rows)
            {
                i++;
                for (int c = 0; c < table.Columns.Count; c++)
                {
                    //tb.GetRow(i).GetCell(c).SetText(Convert.ToString(row[c]));
                    XWPFRun run = tb.GetRow(i).GetCell(c).Paragraphs[0].CreateRun();
                    run.FontFamily = string.IsNullOrWhiteSpace(fontFamily) ? _fontFamily : fontFamily;
                    run.FontSize = fontSize <= 0 ? _fontSize : fontSize;
                    run.SetText(Convert.ToString(row[c]));
                }
            }
        }

        #endregion


        /// <summary>
        /// 替换关键字
        /// </summary>
        public void ReplaceKeyword(Dictionary<string, string> DicWord)
        {
            //遍历段落                  
            foreach (var para in doc.Paragraphs)
            {
                string oldText = para.ParagraphText;
                if (oldText != "" && oldText != string.Empty && oldText != null)
                {
                    string tempText = para.ParagraphText;

                    foreach (KeyValuePair<string, string> kvp in DicWord)
                    {
                        if (tempText.Contains(kvp.Key))
                        {
                            tempText = tempText.Replace(kvp.Key, kvp.Value);

                            para.ReplaceText(oldText, tempText);
                        }
                    }

                }
            }

            //遍历表格      
            var tables = doc.Tables;
            foreach (var table in tables)
            {
                foreach (var row in table.Rows)
                {
                    foreach (var cell in row.GetTableCells())
                    {
                        foreach (var para in cell.Paragraphs)
                        {
                            string oldText = para.ParagraphText;
                            if (oldText != "" && oldText != string.Empty && oldText != null)
                            {
                                //记录段落文本
                                string tempText = para.ParagraphText;
                                foreach (KeyValuePair<string, string> kvp in DicWord)
                                {
                                    if (tempText.Contains(kvp.Key))
                                    {
                                        tempText = tempText.Replace(kvp.Key, kvp.Value);

                                        //替换内容
                                        para.ReplaceText(oldText, tempText);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}