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