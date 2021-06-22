using System.Collections.Generic;

namespace BillReader.Interfaces
{
    public interface IPdfParser
    {

        enum MarketerName { };

        IEnumerable<PdfContent> Parse(IEnumerable<IPdfInfo> pdfInfo);

    }
}
