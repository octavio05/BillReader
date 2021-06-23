using System;
using System.Collections.Generic;

namespace BillReader.Interfaces
{

    /// <summary>
    ///     Parsea un fichero pdf y lo convierte en un objeto PdfContent.
    /// </summary>
    public interface IPdfParser
    {

        /// <summary>
        ///     Nombre de la comercializadora.
        /// </summary>
        enum MarketerName
        {
            /// <summary>
            ///     Comercializadora no definida.
            /// </summary>
            Undefined,
            /// <summary>
            ///     Comercializadora Endesa.
            /// </summary>
            Endesa
        };

        /// <summary>
        ///     Parsea un fichero pdf y lo convierte ne un objeto PdfContent.
        /// </summary>
        /// <param name="pdfInfo">Objeto que implemente la interfaz IpdfInfo.</param>
        /// <returns>Objeto PdfContent.</returns>
        /// <exception cref="ArgumentNullException">pdf can't be null.</exception>
        /// <exception cref="InvalidOperationException">Comercializadora no definida.</exception>
        /// <exception cref="FormatException">Formato de factura no válido.</exception>
        IEnumerable<PdfContent> Parse(IEnumerable<IPdfInfo> pdfInfo);

    }

}
