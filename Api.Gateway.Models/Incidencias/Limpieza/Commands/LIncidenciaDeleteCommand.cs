﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Incidencias.Limpieza.Commands
{
    public class LIncidenciaDeleteCommand
    {

        public int Id { get; set; }
        public int CedulaEvaluacionId { get; set; }
        public int Pregunta { get; set; }
    }
}
