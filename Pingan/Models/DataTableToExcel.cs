using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Aspose.Cells;
 

namespace Pingan
{ 
    public class DataTableToExcel
    {
        public static void WorkbookToExcel(Workbook wb, string FileName)
        {
            HttpContext curContext = HttpContext.Current;

            HttpBrowserCapabilities b = curContext.Request.Browser;
            if (b.Browser == "IE")
                FileName = HttpUtility.UrlEncode(FileName, System.Text.Encoding.UTF8);
            else
                FileName = HttpUtility.UrlDecode(FileName, System.Text.Encoding.UTF8);

            var options = new Aspose.Cells.XlsSaveOptions(Aspose.Cells.SaveFormat.Excel97To2003);
            wb.Save(curContext.Response, FileName, Aspose.Cells.ContentDisposition.Inline, options);
            //wb.Save(FileName, Aspose.Cells.FileFormatType.Excel2003XML, Aspose.Cells.SaveType.OpenInExcel, curContext.Response, Encoding.UTF8);
        }
        /// <summary> 
        /// 将DataTable数据导出到EXCEL，调用该方法后自动返回可下载的文件流 
        /// </summary> 
        /// <param name="dt">要导出的数据源</param> 
        /// <param name="FileName">文件名</param> 
        public static void DataTable1Excel(DataTable dt, string FileName)
        {
            GridView gvExport = null;
            // 当前对话 
            HttpContext curContext = HttpContext.Current;
            // IO用于导出并返回excel文件 
            StringWriter strWriter = null;
            HtmlTextWriter htmlWriter = null;

            if (dt != null)
            {
                // 设置编码和附件格式 
                curContext.Response.AddHeader("content-disposition", "attachment;filename=" + HttpUtility.UrlEncode(FileName, System.Text.Encoding.UTF8) + ".xls");
                curContext.Response.ContentType = "application/vnd.ms-excel";
                curContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
                curContext.Response.Charset = "utf-8";

                // 导出excel文件 
                strWriter = new StringWriter();
                htmlWriter = new HtmlTextWriter(strWriter);
                // 为了解决gvData中可能进行了分页的情况，需要重新定义一个无分页的GridView 
                gvExport = new GridView();
                gvExport.DataSource = dt.DefaultView;
                gvExport.AllowPaging = false;
                gvExport.DataBind();

                for (int i = 0; i < gvExport.Rows.Count; i++)
                {
                    //  gvExport.Rows[i].BackColor = System.Drawing.Color.Red;
                    for (int j = 0; j < gvExport.Rows[i].Cells.Count; j++)
                    {
                        gvExport.Rows[i].Cells[j].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
                    }
                }

                // 返回客户端 
                gvExport.RenderControl(htmlWriter);
                curContext.Response.Write("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=gb2312\" />" + strWriter.ToString());
                curContext.Response.End();
            }
        }




        /// <summary>
        /// 直接输出Excel
        /// </summary>
        /// <param name="dtData"></param>
        public static void DataTable2Excel(DataTable dtData)
        {
            DataGrid dgExport = null;
            // 当前对话
            HttpContext curContext = HttpContext.Current;
            // IO用于导出并返回excel文件
            StringWriter strWriter = null;
            HtmlTextWriter htmlWriter = null;

            if (dtData != null)
            {
                // 设置编码和附件格式
                curContext.Response.ContentType = "application/vnd.ms-excel";
                curContext.Response.ContentEncoding = System.Text.Encoding.UTF8;
                curContext.Response.Charset = "";

                // 导出excel文件
                strWriter = new StringWriter();
                htmlWriter = new HtmlTextWriter(strWriter);

                // 为了解决dgData中可能进行了分页的情况，需要重新定义一个无分页的DataGrid
                dgExport = new DataGrid();
                dgExport.DataSource = dtData.DefaultView;
                dgExport.AllowPaging = false;
                dgExport.DataBind();

                // 返回客户端
                dgExport.RenderControl(htmlWriter);
                curContext.Response.Write(strWriter.ToString());
                curContext.Response.End();
            }
        }



        /// <summary>
        /// dtData是要导出为Excel的DataTable,FileName是要导出的Excel文件名(不加.xls)
        /// </summary>
        /// <param name="dtData"></param>
        /// <param name="FileName"></param>
        public static void DataTable3Excel(DataTable dtData, String FileName)
        {
            GridView dgExport = null;
            //当前对话 
            HttpContext curContext = HttpContext.Current;
            //IO用于导出并返回excel文件 
            StringWriter strWriter = null;
            HtmlTextWriter htmlWriter = null;

            if (dtData != null)
            {
                //设置编码和附件格式 
                //HttpUtility.UrlEncode(FileName, System.Text.Encoding.UTF8)作用是方式中文文件名乱码
                curContext.Response.AddHeader("content-disposition", "attachment;filename=" + HttpUtility.UrlEncode(FileName, System.Text.Encoding.UTF8) + ".xls");
                curContext.Response.ContentType = "application nd.ms-excel";
                curContext.Response.ContentEncoding = System.Text.Encoding.UTF8;
                curContext.Response.Charset = "GB2312";

                //导出Excel文件 
                strWriter = new StringWriter();
                htmlWriter = new HtmlTextWriter(strWriter);

                //为了解决dgData中可能进行了分页的情况,需要重新定义一个无分页的GridView 
                dgExport = new GridView();
                dgExport.DataSource = dtData.DefaultView;
                dgExport.AllowPaging = false;
                dgExport.DataBind();

                //下载到客户端 
                dgExport.RenderControl(htmlWriter);
                curContext.Response.Write(strWriter.ToString());
                curContext.Response.End();
            }
        }

        /// <summary>
        /// Datatable to Excel带自定文件名的导出
        /// </summary>
        /// <param name="dt">数据表</param>
        /// <param name="FileName">文件名</param>
        public static void DataTable4Excel(DataTable dt, string FileName, int[] aColTxtFormat)
        {
            StringWriter stringWriter = new StringWriter();
            HtmlTextWriter htmlWriter = new HtmlTextWriter(stringWriter);
            DataGrid excel = new DataGrid();
            //TableItemStyle AlternatingStyle = new TableItemStyle();
            TableItemStyle headerStyle = new TableItemStyle();
            //TableItemStyle itemStyle = new TableItemStyle();
            //AlternatingStyle.BackColor = System.Drawing.Color.LightGray;
            headerStyle.BackColor = System.Drawing.Color.LightGray;
            headerStyle.Font.Bold = true;
            headerStyle.HorizontalAlign = HorizontalAlign.Center;
            //itemStyle.HorizontalAlign = HorizontalAlign.Center; ;

            //excel.AlternatingItemStyle.MergeWith(AlternatingStyle);
            excel.HeaderStyle.MergeWith(headerStyle);
            //excel.ItemStyle.MergeWith(itemStyle);
            excel.GridLines = GridLines.Both;
            excel.HeaderStyle.Font.Bold = true;
            excel.DataSource = dt.DefaultView;   //输出DataTable的内容
            excel.DataBind();

            for (int i = 0; i < excel.Items.Count; i++)
            {
                excel.Columns[3].ItemStyle.CssClass = "xlsText";
                excel.Items[i].Cells[3].Style.Add("mso-number-format", "");
                for (int j = 0; j < aColTxtFormat.Length; j++)
                {
                    int nCol = aColTxtFormat[j];
                    excel.Items[i].Cells[nCol].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
                }
            }


        }

        /// <summary>
        /// 将DataTable导出到Excel，调用如:m_ExportExcel(dt,new int[]{3});
        /// </summary>
        /// <param name="dt">要导出的DataTable</param>
        /// <param name="aColTxtFormat">要设置为字符串格式的列，比如银行帐号等</param>
        private void m_ExportExcel(DataTable dt, int[] aColTxtFormat)
        {
            System.IO.StringWriter stringWriter = new System.IO.StringWriter();
            HtmlTextWriter htmlWriter = new HtmlTextWriter(stringWriter);
            DataGrid excel = new DataGrid();
            //System.Web.UI.WebControls.TableItemStyle AlternatingStyle = new TableItemStyle();
            System.Web.UI.WebControls.TableItemStyle headerStyle = new TableItemStyle();
            //System.Web.UI.WebControls.TableItemStyle itemStyle = new TableItemStyle();
            //AlternatingStyle.BackColor = System.Drawing.Color.LightGray;
            headerStyle.BackColor = System.Drawing.Color.LightGray;
            headerStyle.Font.Bold = true;
            headerStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            //itemStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center; ;

            //excel.AlternatingItemStyle.MergeWith(AlternatingStyle);
            excel.HeaderStyle.MergeWith(headerStyle);
            //excel.ItemStyle.MergeWith(itemStyle);
            excel.GridLines = GridLines.Both;
            excel.HeaderStyle.Font.Bold = true;
            excel.DataSource = dt.DefaultView;   //输出DataTable的内容
            excel.DataBind();
            for (int i = 0; i < excel.Items.Count; i++)
            {
                //excel.Columns[3].ItemStyle.CssClass = "xlsText";
                //excel.Items[i].Cells[3].Style.Add("mso-number-format", "\"@\"");
                for (int j = 0; j < aColTxtFormat.Length; j++)
                {
                    int nCol = aColTxtFormat[j];
                    excel.Items[i].Cells[nCol].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
                }
            }
            excel.RenderControl(htmlWriter);

            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=Excel.xls");
            HttpContext.Current.Response.Charset = "UTF-8";
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            //设置导出的格式
            //HttpContext.Current.Response.ContentType=".xls/.txt/.doc";image/JPEG;text/HTML;image/GIF;vnd.ms-excel/msword 
            HttpContext.Current.Response.ContentType = ".xls";
            HttpContext.Current.Response.Write(stringWriter.ToString());
            HttpContext.Current.Response.End();
            //string filestr = "d:\\data\\" + filePath; //filePath是文件的路径
            //int pos = filestr.LastIndexOf("\\");
            //string file = filestr.Substring(0, pos);
            //if (!Directory.Exists(file)) {
            //    Directory.CreateDirectory(file);
            //}
            //System.IO.StreamWriter sw = new StreamWriter(filestr);
            //sw.Write(stringWriter.ToString());
            //sw.Close();

        }

        //public static void DocumentToDoc(Document doc, string FileName)
        //{
        //    HttpContext curContext = HttpContext.Current;
        //    HttpBrowserCapabilities b = curContext.Request.Browser;
        //    curContext.Response.ContentEncoding = System.Text.Encoding.UTF8;
        //    curContext.Response.Charset = "GB2312";
        //    if (b.Browser == "IE")
        //        FileName = HttpUtility.UrlEncode(FileName, System.Text.Encoding.UTF8);
        //    else
        //        FileName = HttpUtility.UrlDecode(FileName, System.Text.Encoding.UTF8);

        //    byte[] buffer1 = Encoding.Default.GetBytes(FileName);
        //    byte[] buffer2 = Encoding.Convert(Encoding.UTF8, Encoding.Default, buffer1, 0, buffer1.Length);
        //    FileName = Encoding.UTF8.GetString(buffer2);

        //    doc.Save(curContext.Response, FileName, Aspose.Words.ContentDisposition.Attachment, new Aspose.Words.Saving.DocSaveOptions());
        //}

        //public static void DocumentToDoc2(string content, string FileName)
        //{
        //    HttpContext curContext = HttpContext.Current;
        //    //IO用于导出并返回excel文件 
        //    StringWriter strWriter = null;
        //    HtmlTextWriter htmlWriter = null;
        //    curContext.Response.AddHeader("content-disposition", "attachment;filename=" + HttpUtility.UrlEncode(FileName, System.Text.Encoding.UTF8) + ".doc");
        //    curContext.Response.ContentType = "application nd.ms-word";
        //    curContext.Response.ContentEncoding = System.Text.Encoding.UTF8;
        //    curContext.Response.Charset = "UTF8";

        //    //导出Excel文件 
        //    StringBuilder sb = new StringBuilder(content);
        //    strWriter = new StringWriter(sb);
        //    htmlWriter = new HtmlTextWriter(strWriter);
        //    curContext.Response.Write("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF8\" />" + strWriter.ToString());
        //    curContext.Response.End();
        //}
    }
}