
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FISScanEventWorkerService.Entities
{
    public record User
    {
        public string UserId { get; set; }
        public string CarrierId { get; set; }
        public string RunId { get; set; }
    }
}
