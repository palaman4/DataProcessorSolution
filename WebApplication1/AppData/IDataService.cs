using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace WebApplication1.AppData
{
    public interface IDataService
    {
        Boolean ValidateDataIntoStaging(string headers);
        void InsertintoOutputTable();

        List<IField> Fields { get; set; }
    }
}
