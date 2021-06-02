using CS.Common;
using CS.BLL;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CS.BLL.FW;
using CS.Base.Log;

namespace CS.Web.Rdlc
{
    public partial class rdlc : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        /// <summary>
        /// 以内存流形式返回rdlc报表配置信息
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        public MemoryStream GenerateRdlc(string inStr)
        {
            byte[] b = Encoding.UTF8.GetBytes(inStr);
            MemoryStream ms = new MemoryStream(b);
            return ms;
        }
        /// <summary>
        /// 触发的rdlc报表查询展示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SearchBtn_Click(object sender, EventArgs e)
        {
            try
            {
                string rdlcId = this.rdlcId.Value;
                string queryParams = this.queryParams.Value;
                if (!string.IsNullOrWhiteSpace(rdlcId))
                {
                    var rdlc = BF_RDLC_REPORT.Instance.GetEntityByKey<BF_RDLC_REPORT.Entity>(rdlcId);
                    DataTable dt = BF_TB_REPORT.CheckSqlReturnDt(rdlc.DB_ID, rdlc.SQL_CODE, queryParams);

                    reportViewer1.LocalReport.DataSources.Clear();//清理原rdlc数据
                    reportViewer1.LocalReport.DisplayName = rdlc.NAME;
                    reportViewer1.LocalReport.LoadReportDefinition(GenerateRdlc(rdlc.RDLC_CODE));
                    ReportDataSource reportDataSource = new ReportDataSource("DataSet1", dt);
                    reportViewer1.LocalReport.DataSources.Add(reportDataSource);//赋值新数据
                    reportViewer1.LocalReport.Refresh();
                }
            }
            catch(Exception ex)
            {
                BLog.Write(BLog.LogLevel.WARN, string.Format(@"查询rdlc报表出错：{0}.",ex.Message));
            }
        }

    }
}