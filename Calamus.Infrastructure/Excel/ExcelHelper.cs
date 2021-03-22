using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Calamus.Infrastructure.Excel
{
    /// <summary>
    /// Excel帮助类
    /// </summary>
    public sealed class ExcelHelper
    {
        /// <summary>
        /// Excel文件流 转 DataTable
        /// </summary>
        /// <param name="excelStream">excel文件流</param>
        /// <param name="extension">后缀名：.xls|.xlsx</param>
        /// <param name="sheetName">工作表 - 名称</param>
        /// <returns>DataTable对象</returns>
        public static DataTable Build2DataTable(Stream excelStream, string extension, string sheetName = "")
        {
            StringNotNull(extension, "extension");

            DataTable dt = new DataTable();

            IWorkbook workbook;
            if (Regex.IsMatch(extension, ".*xls$")) // .xls
            {
                workbook = new HSSFWorkbook(excelStream);
            }
            else    // .xlsx
            {
                workbook = new XSSFWorkbook(excelStream);
            }
            //  工作 表名不为空，根据表名获取 sheet，否则默认获取第一个工作表
            ISheet sheet = !string.IsNullOrWhiteSpace(sheetName) ? workbook.GetSheet(sheetName) : workbook.GetSheetAt(0);

            IRow headerRow = sheet.GetRow(0);   //第一行，通常是表格头 信息
            int cellCount = headerRow.LastCellNum;  // 表格总列数

            // 遍历 列数 并设置 DataTable 列名
            for (int j = 0; j < cellCount; j++)
            {
                ICell cell = headerRow.GetCell(j);

                string columnName = cell.ToString();
                if (string.IsNullOrWhiteSpace(columnName)) throw new Exception("列名不能为空");
                if (dt.Columns.Contains(columnName)) throw new Exception("存在相同的列名");

                dt.Columns.Add(columnName);
            }

            // 遍历 表格 添加到 DataTable
            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                DataRow dataRow = dt.NewRow();

                for (int j = row.FirstCellNum; j < cellCount; j++)
                {
                    if (row.GetCell(j) != null)
                        dataRow[j] = row.GetCell(j).ToString();
                }

                dt.Rows.Add(dataRow);
            }
            return dt;
        }

        /// <summary>
        /// Excel文件流 转 List
        /// </summary>
        /// <typeparam name="T">T 类型</typeparam>
        /// <param name="excelStream">excel文件流</param>
        /// <param name="extension">后缀名：.xls|.xlsx</param>
        /// <param name="sheetName">工作表 - 名称</param>
        /// <returns>T 对象集合</returns>
        public static List<T> Build2List<T>(Stream excelStream, string extension, string sheetName = "") where T : class, new()
        {
            DataTable table = Build2DataTable(excelStream, extension, sheetName);
            List<T> list = table.DataTable2List<T>();
            return list;
        }

        /// <summary>
        /// DataTable生成 IWorkBook
        /// </summary>
        /// <param name="table">DataTable 对象</param>
        /// <param name="sheetName">工作表 - 名称</param>
        /// <param name="extension">文件后缀名：.xls|.xlsx</param>
        /// <returns>IWorkbook 实例</returns>
        static IWorkbook BuildWorkbook(DataTable table, string extension, string sheetName)
        {
            StringNotNull(extension, "extension");

            IWorkbook workbook;
            if (Regex.IsMatch(extension, ".*xls$")) // .xls
            {
                workbook = new HSSFWorkbook();
            }
            else    // .xlsx
            {
                workbook = new XSSFWorkbook();
            }
            ISheet sheet = string.IsNullOrWhiteSpace(sheetName) ? workbook.CreateSheet() : workbook.CreateSheet(sheetName);

            //***************获取每一列 宽度***************
            int[] columnWidths = new int[table.Columns.Count];
            foreach (DataColumn item in table.Columns)
            {
                columnWidths[item.Ordinal] = Encoding.UTF8.GetBytes(item.ColumnName).Length;
            }
            for (int i = 0; i < table.Rows.Count; i++)
            {
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    int intTemp = Encoding.UTF8.GetBytes(table.Rows[i][j].ToString()).Length;
                    if (intTemp > columnWidths[j])
                    {
                        // 循环遍历 设置最大宽度
                        columnWidths[j] = intTemp;
                    }
                }
            }
            //***************表头设置***************
            IRow headerRow = sheet.CreateRow(0);

            // 表头样式
            ICellStyle headStyle = workbook.CreateCellStyle();
            headStyle.Alignment = HorizontalAlignment.Left;
            headStyle.WrapText = true;
            IFont font = workbook.CreateFont();
            font.IsBold = true;
            headStyle.SetFont(font);

            foreach (DataColumn column in table.Columns)
            {
                ICell headCell = headerRow.CreateCell(column.Ordinal);
                headCell.SetCellValue(column.ColumnName);
                headCell.CellStyle = headStyle;
                //设置列宽
                sheet.SetColumnWidth(column.Ordinal, (columnWidths[column.Ordinal] + 1) * 256);
            }
            //***************内容设置***************
            ICellStyle dateStyle = workbook.CreateCellStyle();
            IDataFormat format = workbook.CreateDataFormat();
            dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd");  //  日期格式
            int rowIndex = 1;
            foreach (DataRow row in table.Rows)
            {
                if (rowIndex > 65535) throw new Exception("最大只支持65535记录导出");

                IRow dataRow = sheet.CreateRow(rowIndex);
                foreach (DataColumn column in table.Columns)
                {
                    ICell newCell = dataRow.CreateCell(column.Ordinal);
                    string drValue = row[column].ToString();

                    switch (column.DataType.ToString())
                    {
                        case "System.String"://字符串类型
                            newCell.SetCellValue(drValue);
                            break;
                        case "System.DateTime"://日期类型
                            DateTime dateV;
                            if (DateTime.TryParse(drValue, out dateV))
                            {
                                newCell.SetCellValue(dateV);
                                newCell.CellStyle = dateStyle;//格式化显示
                            }
                            break;
                        case "System.Boolean"://布尔型
                            bool boolV;
                            if (bool.TryParse(drValue, out boolV))
                            {
                                newCell.SetCellValue(boolV);
                            }
                            break;
                        case "System.Int16"://整型
                        case "System.Int32":
                        case "System.Int64":
                        case "System.Byte":
                            int intV;
                            if (int.TryParse(drValue, out intV))
                            {
                                newCell.SetCellValue(intV);
                            }
                            break;
                        case "System.Decimal"://浮点型
                        case "System.Double":
                            double doubV;
                            if (double.TryParse(drValue, out doubV))
                            {
                                newCell.SetCellValue(doubV);
                            }
                            break;
                        case "System.DBNull"://空值处理
                            newCell.SetCellValue(string.Empty);
                            break;
                        default:
                            newCell.SetCellValue(string.Empty);
                            break;
                    }
                }
                rowIndex++;
            }
            return workbook;
        }

        /// <summary>
        /// DataTable 导出 Excel流
        /// </summary>
        /// <param name="table">DataTable对象</param>
        /// <param name="extension">文件后缀名：.xls|.xlsx</param>
        /// <param name="sheetName">表名</param>
        /// <returns>Excel流流</returns>
        public static MemoryStream Build2Stream(DataTable table, string extension = ".xlsx", string sheetName = "")
        {
            IWorkbook workbook = BuildWorkbook(table, extension, sheetName);
            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;
            return ms;
        }

        /// <summary>
        /// DataTable 导出 Excel 字节数组
        /// </summary>
        /// <param name="table">DataTable对象</param>
        /// <param name="extension">文件后缀名：.xls|.xlsx</param>
        /// <param name="sheetName">表名</param>
        /// <returns>Excel 字节数组</returns>
        public static byte[] Build2Byte(DataTable table, string extension = ".xlsx", string sheetName = "")
        {
            MemoryStream stream = Build2Stream(table, extension, sheetName);
            byte[] results = stream.GetBuffer();
            stream.Dispose();
            return results;
        }

        /// <summary>
        /// List 导出 Excel流
        /// </summary>
        /// <typeparam name="T">T 类型</typeparam>
        /// <param name="list">T对象集合</param>
        /// <param name="extension">文件后缀名：.xls|.xlsx</param>
        /// <param name="sheetName">表名</param>
        /// <returns>Excel流</returns>
        public static MemoryStream Build2Stream<T>(IList<T> list, string extension = ".xlsx", string sheetName = "") where T : class, new()
        {
            DataTable table = list.List2DataTable();
            return Build2Stream(table, extension, sheetName);
        }

        /// <summary>
        /// List 导出 Excel 字节数组
        /// </summary>
        /// <typeparam name="T">T 类型</typeparam>
        /// <param name="list">T对象集合</param>
        /// <param name="extension">文件后缀名：.xls|.xlsx</param>
        /// <param name="sheetName">表名</param>
        /// <returns>Excel 字节数组</returns>
        public static byte[] Build2Byte<T>(IList<T> list, string extension = ".xls", string sheetName = "") where T : class, new()
        {
            DataTable table = list.List2DataTable();
            return Build2Byte(table, extension, sheetName);
        }

        #region - 私有方法

        /// <summary>
        /// 检查是否 是否 空或 nulll, 抛异常
        /// </summary>
        /// <param name="target">目标字符串</param>
        /// <param name="paramName">抛异常 - 当前参数名称</param>
        static void StringNotNull(string target, string paramName)
        {
            if (string.IsNullOrWhiteSpace(target)) throw new ArgumentNullException(paramName);
        }

        #endregion
    }
}
