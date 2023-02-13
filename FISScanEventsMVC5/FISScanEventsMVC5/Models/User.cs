using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FISScanEventsMVC5.Models
{
    public record User
    {
        public string UserId { get; set; }
        public string CarrierId { get; set; }
        public string RunId { get; set;}
    }
}
