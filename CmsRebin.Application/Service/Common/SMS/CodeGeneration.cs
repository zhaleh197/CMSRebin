using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmsRebin.Application.Service.Common.SMS
{
    public class CodeGeneration
    {
        // tolid code random baraye faramoshie ramze oboor
        public static string Activecode()
        {
            Random r = new Random();
            return r.Next(100000, 999999).ToString();


        }
        // tolid adad random baraye sefaresh haye ma

        public static string Factorcode()
        {
            Random r = new Random();
            r.Next(10000000, 99999999);
            return r.ToString();
        }
        // tolid adad random baraye nomgozari khod file ha dar server
        //tarkibi az horof va adada.
        public static string filecode()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
    }
}
