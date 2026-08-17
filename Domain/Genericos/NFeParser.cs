using System.Xml.Linq;
using Domain.Models;

namespace Domain.Genericos
{
    public class NFeParser
    {
        private static readonly XNamespace Ns =
            "http://www.portalfiscal.inf.br/nfe";

        public DocFiscal Parse(string xml)
        {
            var document = XDocument.Parse(xml);

            var infNFe = document
                .Descendants(Ns + "infNFe")
                .FirstOrDefault();

            if (infNFe == null)
                throw new InvalidOperationException(
                    "XML não possui uma NF-e válida.");

            var ide = infNFe.Element(Ns + "ide");
            var emit = infNFe.Element(Ns + "emit");
            var dest = infNFe.Element(Ns + "dest");

            var total = infNFe
                .Element(Ns + "total")?
                .Element(Ns + "ICMSTot");

            return new DocFiscal
            {
                TipoDocumento = "NFe",

                ChaveAcesso = infNFe
                    .Attribute("Id")?
                    .Value
                    .Replace("NFe", ""),

                Numero = int.TryParse(
                    ide?.Element(Ns + "nNF")?.Value,
                    out var numero)
                    ? numero
                    : null,

                Serie = int.TryParse(
                    ide?.Element(Ns + "serie")?.Value,
                    out var serie)
                    ? serie
                    : null,

                DataEmissao = DateTime.TryParse(
                    ide?.Element(Ns + "dhEmi")?.Value,
                    out var data)
                    ? data
                    : null,

                CnpjEmitente =
                    emit?.Element(Ns + "CNPJ")?.Value
                    ?? string.Empty,

                RazaoSocialEmitente =
                    emit?.Element(Ns + "xNome")?.Value,

                CnpjDestinatario =
                    dest?.Element(Ns + "CNPJ")?.Value,

                RazaoSocialDestinatario =
                    dest?.Element(Ns + "xNome")?.Value,

                ValorTotal = decimal.TryParse(
                    total?.Element(Ns + "vNF")?.Value,
                    out var valor)
                    ? valor
                    : null,

                XmlOriginal = xml
            };
        }
    }
}
