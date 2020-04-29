using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;

namespace WebApplication1.AppData
{
    public class CustomDataService:BaseDataService
    {
        public string customSourceField;
        public CustomDataService(IConfiguration config) : base(config) { }

        public int customFieldIndex =-1;
        public override Boolean ValidateDataIntoStaging(string headers)
        {

            this.stagingTable = GetStagingTable(headers);

            var inputData = getSampleData();

            if (!string.IsNullOrEmpty(customSourceField))
            {
                var te = headers.Split(",").ToArray();
              
                customFieldIndex = Array.IndexOf(te,customSourceField);
            }
            DataRow dr;
            try
            {
                for (int i = 0; i < inputData.Count; i++)
                {
                    if (inputData[i].Count() > stagingTable.Columns.Count)
                    {
                        return false;
                    }
                    else
                    {
                        dr = stagingTable.NewRow();
                        for (int j = 0; j < stagingTable.Columns.Count; j++)
                        {
                            if (customFieldIndex != -1 && j == customFieldIndex)
                            {
                                dr[j] = inputData[i][j].ToString().Remove(0, 3);
                            }
                            else
                            {
                                dr[j] = inputData[i][j];
                            }
                        }
                        stagingTable.Rows.Add(dr);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }

             return false;
        }

         protected override List<string[]> getSampleData()
        {

            Sampledata = new List<string[]>();
            for (int i = 0; i <= 10; i++)
            {
                //AccountCode,Name,Type,OpenDate,Currency
                Sampledata.Add(new string[] { "123ABCD" + i.ToString(), "Test" + i.ToString(), "2","01-01-2018", "CD",  });
            }
            return Sampledata;
        }
    }
}
