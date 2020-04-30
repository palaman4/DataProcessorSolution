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
            if (base.ValidateDataIntoStaging(headers))
            {
                try
                {
                    stagingTable.Select(string.Format("[{0}] = '1' or 1=1", customSourceField)).ToList<DataRow>().ForEach(r => r[customSourceField] = ExtractionMethod(r[customSourceField].ToString()));
                    return true;
                }

                catch (Exception ex)
                {
                    throw;
                }
            }
             return false;
        }

        public Func<string, string> ExtractionMethod;
        
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
