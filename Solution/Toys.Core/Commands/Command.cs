namespace Toys.Core.Commands
{
    using Ionic.Zip;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.OleDb;
    using System.IO;
    using System.Text;
    using Toys.Data.Contracts;

    public abstract class Command : ICommand
    {
        protected readonly IToysData Data;

        protected Command(IToysData data)
        {
            this.Data = data;
        }

        public abstract bool Execute();

        protected List<string[]> ImportFromZipFile(string filePath, string fileName)
        {
            var path = filePath;
            
            
            using (var reader = new StreamReader(path))
            {
                using (ZipFile zip = Ionic.Zip.ZipFile.Read(path))
                {
                    ZipEntry e = zip[fileName];

                    using (FileStream outputStream = new FileStream(@"../../../Files/" + fileName, FileMode.OpenOrCreate))
                    {
                        e.Extract(outputStream);
                    }
                }
            }

            var dataSource = @"../../../Files/" + fileName;


            var list = ReadExcelFile(dataSource);

            if (File.Exists(dataSource))
            {
                File.Delete(dataSource);
            }

            return list;
        }

        private List<string[]> ReadExcelFile(string dataSource)
        {
            var list = new List<string[]>();
            DataSet dataSet = new DataSet();

            string connectionString = GetConnectionString(dataSource);

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand();
                command.Connection = connection;

                // Get all Sheets in Excel File
                DataTable dataSheet = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                // Loop through all Sheets to get data
                foreach (DataRow dataRow in dataSheet.Rows)
                {
                    string sheetName = dataRow["TABLE_NAME"].ToString();

                    if (!sheetName.EndsWith("$"))
                    {
                        continue;
                    }

                    // Get all rows from the Sheet
                    command.CommandText = "SELECT * FROM [" + sheetName + "]";

                    var rdr = command.ExecuteReader();
                    while (rdr.Read())
                    {
                        string[] fileData = new string[rdr.FieldCount]; 

                        for (int i = 0; i < rdr.FieldCount; i++)
                        {
                            fileData[i] = rdr[i].ToString();
                        }

                            list.Add(fileData);
                    }
                }

                command = null;
                connection.Close();
            }

            return list;
        }

        private string GetConnectionString(string dataSource)
        {
            Dictionary<string, string> props = new Dictionary<string, string>();

            // XLSX - Excel 2007, 2010, 2012, 2013
            //props["Provider"] = "Microsoft.ACE.OLEDB.12.0;";
            //props["Extended Properties"] = "Excel 12.0 XML";
            //props["Data Source"] = dataSource;

            // XLS - Excel 2003 and Older
            props["Provider"] = "Microsoft.Jet.OLEDB.4.0";
            props["Extended Properties"] = "Excel 8.0";
            props["Data Source"] = dataSource;

            StringBuilder sb = new StringBuilder();

            foreach (KeyValuePair<string, string> prop in props)
            {
                sb.Append(prop.Key);
                sb.Append('=');
                sb.Append(prop.Value);
                sb.Append(';');
            }

            return sb.ToString();
        }

        protected bool ZipFile(string filePath)
        {
            var success = false;
            using (ZipFile salesReportsZip = new ZipFile())
            {
                foreach(string file in Directory.GetFiles(filePath))
                {
                    salesReportsZip.AddFile(file, Path.GetFileName(file));
                }
                salesReportsZip.Save(filePath + "\\" + "SalesReports.zip");

                success = true;
            }

            return success;
        }
    }
}