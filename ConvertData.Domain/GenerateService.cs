using ConvertData.Domain.Interfaces;
using ConvertData.Domain.Response;
using Microsoft.AspNetCore.Http;

namespace ConvertData.Domain
{
    public class GenerateService : IGenerateFile
    {
        public async Task<StateResponse> FileSplitWriter(IFormFile _file)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            var stream = new MemoryStream();

            _file.CopyTo(stream);


            stream.Position = 0;

            var ws = new StreamReader(stream);
            string header = "";

            int rows = 0;
            string currentLine;
            while ((currentLine = ws.ReadLine()) != null)
            {
                if (rows == 0)
                    header = currentLine;

                rows++;
            }

            stream.Position = 0;

            int columns = ws.ReadLine().Split(';').Count();
            int rowCount = 0;
            int totalOfRows = 2500;
            int columnCount = 0;

            string[,] array = new string[rows, columns];
            string[] headers = header.Split(';');

            //ws.Close();


            stream.Position = 0;



            while (!ws.EndOfStream) //Each row of the file
            {
                var line = ws.ReadLine().ToString();
                var values = line.Split(';');


                foreach (var item in values)
                {
                    if (rowCount == 0)
                    {
                        for (var headItem = 0; headItem < headers.Length; headItem++)
                        {
                            array[rowCount, headItem] = headers[headItem].ToString() == null ? "" : headers[headItem].ToString();
                        }
                    } else
                    {
                        array[rowCount, columnCount] = item.ToString() == null ? "" : item.ToString();
                    }
                    
                    columnCount++;

                }


                rowCount++;
                columnCount = 0;

            }






            //double totalOfFiles = Math.Ceiling(((float)reader.RowCount - 1) / totalOfRows);
            //var fileNumber = 1;

            //var start = 0;
            //var end = 1;

            //while (fileNumber <= totalOfFiles)
            //{
            //    var file = @"C:\myOutput" + fileNumber + ".csv";

            //    start = end;
            //    if (fileNumber == totalOfFiles)
            //    {
            //        var rowsLastFile = reader.RowCount - end;
            //        end += rowsLastFile;
            //    }
            //    else
            //    {

            //        end = end + totalOfRows;
            //    }

            //    //generate xlxs files
            //    var workbook = new XLWorkbook();
            //    workbook.AddWorksheet("sheetName");
            //    var ws = workbook.Worksheet("sheetName");



            //    for (var x = 1; x < 2; x++)
            //    {
            //        for (var y = 1; y < reader.FieldCount; y++)
            //        {
            //            ws.Cell(x, y).Value = headers[x - 1, y - 1].ToString();
            //        }

            //    }


            //    var row = 2;

            //    for (var x = start; x <= end; x++)
            //    {
            //        for (var y = 1; y < reader.FieldCount; y++)
            //        {
            //            ws.Cell(row, y).Value = array[x - 1, y - 1].ToString();
            //        }
            //        row++;
            //    }

            //    workbook.SaveAs("yourExcel" + "-" + fileNumber + ".csv");
            //    fileNumber++;

            //}

            return new StateResponse(System.Net.HttpStatusCode.OK, "Generated Files");




        }


        private int CountRowsFile(IFormFile file)
        {
            using (var str = new MemoryStream())
            {
                int rows = 1;
                file.CopyTo(str);

                using (var rdr = new StreamReader(str))
                {
                    while (rdr.ReadLine() != null)
                    {
                        rows++;
                    }


                    str.Position = 0;  //Site tip
                    rdr.Dispose();
                    return rows;
                }
            }
        }
    }
}

