using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI
{
    public class FuncionariosTag
    {
        public FuncionariosTag()
        {
                
        }
        public FuncionariosTag(string funcNome, string funcTag)
        {
            Nome = funcNome;
            Tag = funcTag;
        }
        public string Nome { get; set; }
        public string Tag { get; set; }
    }
}
