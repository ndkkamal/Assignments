﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FISScanEventWorkerService.Entities
{
    public record Device
    {
        public int DeviceTransactionId { get; set; }
        public int DeviceId { get; set; }
    }
}
