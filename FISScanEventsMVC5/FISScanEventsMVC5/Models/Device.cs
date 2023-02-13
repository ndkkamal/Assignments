using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FISScanEventsMVC5.Models
{
    public record Device
    {      
        public int DeviceTransactionId { get; set; }  
        public int DeviceId { get; set; }
    }
}
