using BillReader.Interfaces;
using System.Collections.Generic;

namespace BillReader.Entities
{
    /// <summary>
    ///     Contiene información del documento pdf.
    /// </summary>
    public class PdfInfo : IPdfInfo
    {

        /// <summary>
        ///     Texto de las páginas del documento pdf.
        /// </summary>
        public IEnumerable<string> Pages { get; set; }
        /// <summary>
        ///     Nombre del documento.
        /// </summary>
        public string FileName { get; set; }

    }
}
