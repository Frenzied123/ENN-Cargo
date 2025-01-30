using System.Collections.Generic;

namespace ENN_Cargo.Models
{
    public class CompanyStockViewModel
    {
        public IEnumerable<CompanyStock> CompanyStocks { get; set; }
        public string SortOrder { get; set; }
        public string CountryFilter { get; set; }
        public string TownFilter { get; set; }
    }
}