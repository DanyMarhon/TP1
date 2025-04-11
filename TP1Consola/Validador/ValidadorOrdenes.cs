using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP1Entities;
using FluentValidation;


namespace TP1Consola.Validador
{
    public class ValidadorOrdenes : AbstractValidator<Orden>
    {
        public ValidadorOrdenes()
        {
            RuleFor(o => o.NumeroOrden).NotEmpty().WithMessage("El {PropertyName} es requerido");

            RuleFor(o => o.ClienteId).NotEmpty().WithMessage("El {PropertyName} es requerido")
                .GreaterThan(0).WithMessage("El {PropertyName} must be greather than {ComparisonValue}");

            RuleFor(o => o.FechaOrden).NotEmpty().WithMessage("El {PropertyName} es requerido")
                .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today.Date)).WithMessage("El {PropertyName} debe ser menor a {ComparisonValue}");

        }
    }
}
