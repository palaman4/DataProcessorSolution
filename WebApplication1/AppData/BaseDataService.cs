using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace WebApplication1.AppData
{
    public class BaseDataService : IDataService
    {
        public BaseDataService(IConfiguration config)
        {
            this._config = config;
        }

        protected DataColumn[] headers;

        protected IConfiguration _config;

        protected DataTable stagingTable;

        protected DataTable outputTable;

        protected DataTable inputTable;
        public List<IField> Fields { get; set; }
        public virtual Boolean ValidateDataIntoStaging(string headers)
        {

            stagingTable = GetStagingTable(headers);

            var inputData = getSampleData();

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
                            dr[j] = inputData[i][j];

                        }
                        stagingTable.Rows.Add(dr);
                    }
                }

    
                return true;
            }
            catch(Exception ex)
            {
                //
            }

           
            return false;
        }


        public virtual bool ApplyMappingsToStaging(string dataMappings)
        {
            List<DataMapping> mappingsList = JsonConvert.DeserializeObject<List<DataMapping>>(dataMappings);
           bool tes= stagingTable.Columns.Contains("Currency");
           
            foreach (DataMapping dm in mappingsList) {
               
             stagingTable.Select(string.Format("[{0}] = '{1}'", dm.ColumnName,dm.ReplaceValue)).ToList<DataRow>().ForEach(r =>r[dm.ColumnName] = dm.NewValue);
            }
            return true;
        }
        public virtual void InsertintoOutputTable()
        {
            DataTable outputTable = GetOutputTable();

            outputTable.Merge(inputTable);

            outputTable.FeedTableToCsv(_config.GetSection("CustomSetttings").GetSection("outputFileLocation").Value);
        }


        protected DataColumn[] GetMetadata()
        {
            return _config.GetSection("CustomSetttings").GetSection("DataFields").GetSection("FieldType").GetChildren().Select(i =>
                new DataColumn(i.Key, System.Type.GetType(i.Value))).ToArray();


        }

        protected DataTable GetStagingTable(string headers)
        {
            if (inputTable is null)
            {
                var inputData = headers.Split(",").Select((i => new { name = i })).ToList();
                var metaData = GetMetadata();
                var dataMaxLength = _config.GetSection("CustomSetttings").GetSection("DataFields").GetSection("FieldLength").GetChildren().Select(i =>
                new { name = i.Key, maxLength = i.Value }).ToArray();

                var inputColumns = metaData.Join(inputData,
                    met => met.ColumnName,
                    inp => inp.name,
                    (metdat, input) => new { metdat.ColumnName, metdat.DataType }).Join(dataMaxLength,
                     x => x.ColumnName,
                     y => y.name,
                     (x, y) => new DataColumn(x.ColumnName, x.DataType) { MaxLength = Convert.ToInt16(y.maxLength) }).ToArray();

                inputTable = new DataTable();
                AddColumnsToInputTable(inputColumns, headers);
            }
            return inputTable;

        }

        protected DataTable GetOutputTable()
        {
            if (outputTable is null)
            {
                var metaData = GetMetadata();
                var dataMaxLength = _config.GetSection("CustomSetttings").GetSection("DataFields").GetSection("FieldLength").GetChildren().Select(i =>
                new { name = i.Key, maxLength = i.Value }).ToArray();

                var outputColumns = metaData.Join(dataMaxLength,
                     x => x.ColumnName,
                     y => y.name,
                     (x, y) => new DataColumn(x.ColumnName, x.DataType) { MaxLength = Convert.ToInt16(y.maxLength) }).ToArray();
                outputTable = new DataTable();
                AddColumnsToOutputTable(outputColumns);
                
            }
            return outputTable;

        }

        protected void AddColumnsToOutputTable(DataColumn[] columns)
        {
            var columnOrder = _config.GetSection("CustomSetttings").GetSection("DataFields").GetSection("FieldOrder").GetChildren().Select(i =>
                new { name = i.Key, order = i.Value }).ToList().OrderBy(x=>x.order);
            foreach (var column in columnOrder) 
            {
                outputTable.Columns.Add(columns.First(x => x.ColumnName == column.name));
            } 
        }

        protected void AddColumnsToInputTable(DataColumn[] columns,string inputColumns)
        {
            var inputColumnsOrder = inputColumns.Split(",").ToList();

            foreach (var column in inputColumnsOrder)
            {
                inputTable.Columns.Add(columns.First(x=>x.ColumnName==column.ToString()));
            }
        }
        protected List<string[]> Sampledata
        {
            get; set;
        }

        protected virtual List<string[]> getSampleData()
        {


            Sampledata = new List<string[]>();
            for (int i = 0; i <= 10; i++)
            {
                                
                //Name,Type,Currency,AccountCode
                Sampledata.Add(new string[] { "Test" + i.ToString(), "RESP", "U", "ABCD" + i.ToString() });
                //Sampledata.Add(new string[] {"USD", "200" + i.ToString(), "abc" + i.ToString() });

            }
            return Sampledata;
        }



    }

    public class DataMapping
    {
        [JsonProperty("ColumnName")]
        public string ColumnName { get; set; }

        [JsonProperty("ReplaceValue")]
        public string ReplaceValue { get; set; }

        [JsonProperty("NewValue")]
        public string NewValue { get; set; }
    }


}
