using System;
using System.Collections.Generic;
using System.Text;

namespace ExercicioAplicacaoISI
{
    public class PrevisaoIPMA
    {
        public string owner { get; set; }
        public string country { get; set; }
        public PrevisaoDia[] data { get; set; }
        public int globalIdLocal { get; set; }
        public DateTime dataUpdate { get; set; }
        // ---- 
        public string local { get; set; }
    }
}
