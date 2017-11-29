using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp2._0.Services
{
    public interface IDataService
    {
        string GetName();
    }

    public class DataService: IDataService
    {
        public string GetName()
        {
            return "Chris Vitko";
        }
    }
}
