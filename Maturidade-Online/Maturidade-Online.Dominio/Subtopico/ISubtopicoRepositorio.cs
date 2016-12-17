﻿using Maturidade_Online.Dominio.Abstrato;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maturidade_Online.Dominio
{
    public interface ISubtopicoRepositorio : IRepositorio<Subtopico>
    {
        ICollection<Subtopico> Listar(ICollection<Subtopico> subtopico);
        ICollection<Subtopico> Listar(ICollection<Caracteristica> caracteristica);

    }
}
