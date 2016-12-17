﻿using Maturidade_Online.Dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Maturidade_Online.Models
{
    public class PilarModel
    {

        public int? Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Titulo { get; set; }
    
    }
}