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