using eBettingSystemV2.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBettingSystemV2.Services.Extensions
{
    public class Rasponse<T1>
        where T1:class
    {
        public static bool IsSuccess { get; set; }
        public static int Returning_Id { get; set; }
        public static string ErrorMessage { get; set; }
        public static int ReturningId { get; set; }
        public string TableName { get; set;}
        public List<GenericRasponseClass<T1>> ListGeneric { get; set; } = new List<GenericRasponseClass<T1>>();







    }
}
