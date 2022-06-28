using Microsoft.Data.SqlClient;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBettingSystemV2.Services.Extensions
{
    public static class Extensions
    {
    
        public static T GetRecord<T>(this IDataRecord record, string fieldName)
        {
            if (record.IsDBNull(record.GetOrdinal(fieldName))) return default(T);
            return (T)record[fieldName];
        }







    }
}
