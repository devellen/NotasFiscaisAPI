namespace Domain.DTOs
{
    public class DocFiscalDto
    {
        public int Id { get; set; }
        public string TipoDocumento { get; set; } = string.Empty;
        public string? ChaveAcesso { get; set; }
        public int? Numero { get; set; }
        public int? Serie { get; set; }
        public DateTime? DataEmissao { get; set; }
        public string CnpjEmitente { get; set; }
        public string? RazaoSocialEmitente { get; set; }
        public string? CnpjDestinatario { get; set; }
        public string? RazaoSocialDestinatario { get; set; }
        public string? Uf { get; set; }
        public decimal? ValorTotal { get; set; }
        public string XmlOriginal { get; set; } = string.Empty;
        public DateTime DataImportacao { get; set; }
    }
}
