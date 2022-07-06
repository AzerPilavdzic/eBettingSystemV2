using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RezultatiImporter.Services
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
