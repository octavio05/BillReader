using BillReader.Interfaces;
using System;

namespace BillReader
{

    /// <summary>
    ///     Parsea un fichero pdf y lo convierte en un objeto PdfContent.
    /// </summary>
    public class PdfParser
    {

        private enum MarketerName { Undefined, Endesa };

        private readonly IPdf _pdf;
        private readonly MarketerName? _marketer;
        private string _unifiedText;

        /// <summary>
        ///     Constructor de la clase.
        /// </summary>
        /// <param name="pdf">Objeto que implemente la interfaz IPdf.</param>
        /// <exception cref="ArgumentNullException">pdf o pdf.PagedText no pueden ser nulos.</exception>
        public PdfParser(IPdf pdf)
        {

            if (pdf == null)
                throw new ArgumentNullException("pdf", "can't be null");

            if (pdf.PagedText == null)
                throw new ArgumentNullException("pdf.PagedText", "can't be null");

            _pdf = pdf;

            UnifyText();

            _marketer = GetMarketer();

        }

        /// <summary>
        ///     Unifica todas las páginas del pdf en una sola cadena de texto quitando 
        ///     los saltos de línea y los almacena en la variable privada _unifiedText.
        /// </summary>
        private void UnifyText()
        {

            _unifiedText = string.Empty;

            for (int i = 0, len = _pdf.PagedText.Length; i < len; i++)
                _unifiedText += _pdf.PagedText[i].Replace("\n", "");
        
        }

        /// <summary>
        ///     Comprueba a que comercializadora pertenece la factura.
        /// </summary>
        /// <returns>Nombre de la comercializadora o Undefined si no se ha podido determinar.</returns>
        private MarketerName GetMarketer()
        {

            if (EndesaParser.IsValid(_unifiedText))
                return MarketerName.Endesa;
            else
                return MarketerName.Undefined;

        }

        /// <summary>
        ///     Parsea el fichero pdf y lo convierte en un objeto PdfContent.
        /// </summary>
        /// <returns>Objeto PdfContent.</returns>
        /// <exception cref="InvalidOperationException">Comercializadora no definida.</exception>
        /// <exception cref="FormatException">Formato de factura no válido.</exception>
        public PdfContent Parse()
        {

            switch (_marketer)
            {

                case MarketerName.Endesa:
                    return EndesaParser.Parse(_unifiedText);

                case MarketerName.Undefined:
                default:
                    throw new InvalidOperationException("Marketer not defined.");
            
            }

        }

    }

}
