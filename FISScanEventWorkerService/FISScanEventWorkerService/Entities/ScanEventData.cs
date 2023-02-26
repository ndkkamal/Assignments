using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FISScanEventWorkerService.Entities
{
    public class ScanEventData
    {
        public int LastEventId { get; set; }
        public Dictionary<int, ScanEvent> Events { get; set; } = new Dictionary<int, ScanEvent>();
        public HashSet<int> PickupEvents { get; set; } = new HashSet<int>();
        public HashSet<int> DeliveryEvents { get; set; } = new HashSet<int>();
    }
}
