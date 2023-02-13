using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FISScanEventsMVC5.Models;

namespace FISScanEventsMVC5.Validation
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
