using CarConsole.Models;
using FluentValidation;

namespace CarConsole.Validation;

public class CarValidator : AbstractValidator<Car>
{
  public CarValidator()
  {
    RuleFor(x => x.Name).NotEmpty().MinimumLength(2);
  }
    
}
