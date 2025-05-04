using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP1Entities;

namespace TP1Consola.Validador
{
    public class ValidadorClientes : AbstractValidator<Cliente>
    {
        public ValidadorClientes()
        {
            RuleFor(o => o.Nombre).NotEmpty().WithMessage("El {PropertyName} es requerido");

            RuleFor(o => o.Apellido).NotEmpty().WithMessage("El {PropertyName} es requerido");

            RuleFor(o => o.Dni).NotEmpty().WithMessage("El {PropertyName} es requerido");

        }
    }
}
