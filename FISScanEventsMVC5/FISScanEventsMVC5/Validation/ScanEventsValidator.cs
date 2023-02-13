using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FISScanEventsMVC5.Models;

namespace FISScanEventsMVC5.Validation
{
    public class ScanEventsValidator : AbstractValidator<ScanEvents>
    {
        public ScanEventsValidator() { 
            RuleForEach(x => x.scanevents).SetValidator(new ScanEventValidator());
        }
    }
}
