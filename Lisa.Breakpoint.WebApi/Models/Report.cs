using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lisa.Breakpoint.WebApi.Models
{
    public class Report
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public DateTime? Date { get; set; }
    }
}
