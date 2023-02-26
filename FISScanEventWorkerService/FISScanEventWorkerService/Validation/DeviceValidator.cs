using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FISScanEventWorkerService.Entities;

namespace FISScanEventWorkerService.Validation
{
    public class DeviceValidator : AbstractValidator<Device>
    {
        public DeviceValidator()
        {
            RuleFor(device => device.DeviceId).NotEmpty();
            RuleFor(device => device.DeviceTransactionId).NotEmpty();
        }
    }
}
