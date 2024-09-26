using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinimalApi.Dominio.ModelViews
{
    public struct ErrosValidacao
    {
        public List<string> MensagensErrosList { get; set; }

        public ErrosValidacao() {
            MensagensErrosList = new List<string>();
        }
    }
}