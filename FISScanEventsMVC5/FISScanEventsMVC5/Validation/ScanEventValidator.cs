using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentValidation;

using FISScanEventsMVC5.Models;

namespace FISScanEventsMVC5.Validation
{
    public class ScanEventValidator : AbstractValidator<ScanEvent>
    {       
        public ScanEventValidator() {

  
            RuleFor(scanevent => scanevent.ParcelId).GreaterThan(0);
            RuleFor(scanevent => scanevent.CreatedDateTimeUtc).NotNull().WithMessage("Date and Time is not recorded");
            //RuleFor(scanevent => scanevent.StatusCode).NotNull().NotEmpty();
            RuleFor(scanevent => scanevent.Type).NotNull().NotEmpty().WithMessage("Type is required");

            RuleFor(scanevent => scanevent.Device).SetValidator(new DeviceValidator());
            RuleFor(scanevent => scanevent.User).SetValidator(new UserValidator());

            //RuleFor(scanevent => scanevent.Device.DeviceId).NotNull().When(scanevent => scanevent.EventId > 0);
            //RuleFor(scanevent => scanevent.Device.DeviceTransactionId).NotNull().When(scanevent => scanevent.EventId > 0);
            //RuleFor(scanevent => scanevent.User.UserId).NotNull().When(scanevent => scanevent.EventId > 0);
            //RuleFor(scanevent => scanevent.User.CarrierId).NotNull().When(scanevent => scanevent.EventId > 0);
            //RuleFor(scanevent => scanevent.User.RunId).NotNull().When(scanevent => scanevent.EventId > 0);
        }
    }
}
