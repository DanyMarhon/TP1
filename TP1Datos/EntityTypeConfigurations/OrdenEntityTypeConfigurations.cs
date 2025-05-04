using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP1Entities;

namespace TP1Datos.EntityTypeConfigurations
{
    class OrdenEntityTypeConfigurations : IEntityTypeConfiguration<Orden>
    {
        public void Configure(EntityTypeBuilder<Orden> builder)
        {
            throw new NotImplementedException();
        }
    }
}
