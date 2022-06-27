using ClosedXML.Excel;
using ConvertData.Domain.Interfaces;
using ConvertData.Domain.Response;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;

namespace ConvertData.Domain
{
    public class GenerateService : IGenerateFile
    {
        public async Task<StateResponse> FileSplitWriter(IFormFile _file)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = new MemoryStream())
            {
                _file.CopyTo(stream);
                stream.Position = 0;

                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    int rowCount = 0;
                    int totalOfRows = 2500;
                    int columnCount = 0;
                    string[,] array = new string[reader.RowCount , reader.FieldCount];
                    string[,] headers = new string[2, reader.FieldCount];

                    while (reader.Read()) //Each row of the file
                    {

                        while (columnCount < reader.FieldCount)
                        {
                            if (rowCount < 1)
                            {
                                headers[rowCount, columnCount] = reader.GetValue(columnCount) == null ? "" : reader.GetValue(columnCount).ToString();
                            }

                            array[rowCount, columnCount] = reader.GetValue(columnCount) == null ? "" : reader.GetValue(columnCount).ToString();
                            columnCount++;
                        }

                        rowCount++;
                        columnCount = 0;

                    }

                    double totalOfFiles = Math.Ceiling(((float)reader.RowCount - 1) / totalOfRows);
                    var fileNumber = 1;

                    var start = 0;
                    var end = 1;

                    while (fileNumber <= totalOfFiles)
                    {
                        var file = @"C:\myOutput" + fileNumber + ".csv";

                        start = end;
                        if (fileNumber == totalOfFiles)
                        {
                            var rowsLastFile = reader.RowCount - end;
                            end += rowsLastFile;
                        }
                        else
                        {
                            
                            end = end + totalOfRows;
                        }

                        //generate xlxs files
                        var workbook = new XLWorkbook();
                        workbook.AddWorksheet("sheetName");
                        var ws = workbook.Worksheet("sheetName");



                        for (var x = 1; x < 2; x++)
                        {
                            for (var y = 1; y < reader.FieldCount; y++)
                            {
                                ws.Cell(x, y).Value = headers[x - 1, y - 1].ToString();
                            }

                        }


                        var row = 2;

                        for (var x = start; x <= end; x++)
                        {
                            for (var y = 1; y < reader.FieldCount; y++)
                            {
                                ws.Cell(row, y).Value = array[x - 1, y - 1].ToString();
                            }
                            row++;
                        }

                        workbook.SaveAs("yourExcel" + "-" + fileNumber + ".csv");
                        fileNumber++;

                    }

                    return new StateResponse(System.Net.HttpStatusCode.OK, "Generated Files");
                }
            }


        }
    }
}

