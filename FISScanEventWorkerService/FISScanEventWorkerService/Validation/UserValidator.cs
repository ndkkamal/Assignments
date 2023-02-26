using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

using FISScanEventWorkerService.Entities;

namespace FISScanEventWorkerService.Validation
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator() {
            RuleFor(user => user.UserId).NotEmpty();
            RuleFor(user => user.CarrierId).NotNull();
            RuleFor(user => user.RunId).NotNull();
        }
    }
}
