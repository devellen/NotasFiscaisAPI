namespace Domain.Genericos
{
    public class ResultadoPaginado<T>
    {
        public IEnumerable<T> Itens { get; set; }
        public int ContagemTotal { get; set; }
    }
}
