using BillReader.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;

namespace BillReader
{

    /// <summary>
    ///     Parsea un fichero pdf y lo convierte en un objeto PdfContent.
    /// </summary>
    public class PdfParser
    {

        /// <summary>
        ///     Nombre de la comercializadora.
        /// </summary>
        public enum MarketerName { Undefined, Endesa };

        /// <summary>
        ///     Unifica todas las páginas del pdf en una sola cadena de texto quitando los saltos de línea.
        /// </summary>
        /// <param name="pages">Páginas del pdf.</param>
        /// <returns>string con el texto unificado y sin saltos de línea.</returns>
        /// <exception cref="ArgumentNullException">pages can't be null.</exception>
        private string UnifyText(IEnumerable<string> pages)
        {

            if (pages == null)
                throw new ArgumentNullException("pages", "can't be null.");

            string unifiedText = string.Empty;

            foreach(var page in pages)
                unifiedText += page.Replace("\n", "");

            return unifiedText;
        
        }

        /// <summary>
        ///     Comprueba a que comercializadora pertenece la factura.
        /// </summary>
        /// <param name="unifiedText">Texto del documento pdf.</param>
        /// <returns>Nombre de la comercializadora o Undefined si no se ha podido determinar.</returns>
        private MarketerName GetMarketer(string unifiedText)
        {

            if (EndesaParser.IsValid(unifiedText))
                return MarketerName.Endesa;
            else
                return MarketerName.Undefined;

        }

        /// <summary>
        ///     Parsea una colección de fichero pdf y lo convierte en una colección PdfContent.
        /// </summary>
        /// <param name="pdfs">Objeto que implemente la interfaz IPdfInfo.</param>
        /// <returns>Colección de PdfContent.</returns>
        /// <exception cref="ArgumentNullException">pdfs can't be null.</exception>
        public IEnumerable<PdfContent> Parse(IEnumerable<IPdfInfo> pdfInfo)
        {

            if (pdfInfo == null)
                throw new ArgumentNullException("pdfs", "can't be null.");

            ArrayList contentList = new ArrayList();

            foreach (var info in pdfInfo)
            {

                try
                {

                    contentList.Add(Parse(info));

                }
                catch (Exception)
                {

                    contentList.Add(new PdfContent
                    {

                        Comercializadora = MarketerName.Undefined,
                        FileName = info.FileName

                    });
                
                }

            }

            return (PdfContent[])contentList.ToArray(typeof(PdfContent));

        }

        /// <summary>
        ///     Parsea un fichero pdf y lo convierte ne un objeto PdfContent.
        /// </summary>
        /// <param name="pdfInfo">Objeto que implemente la interfaz IpdfInfo.</param>
        /// <returns>Objeto PdfContent.</returns>
        /// <exception cref="ArgumentNullException">pdf can't be null.</exception>
        /// <exception cref="InvalidOperationException">Comercializadora no definida.</exception>
        /// <exception cref="FormatException">Formato de factura no válido.</exception>
        public PdfContent Parse(IPdfInfo pdfInfo)
        {

            if (pdfInfo == null)
                throw new ArgumentNullException("pdf", "can't be null.");

            var unifiedText = UnifyText(pdfInfo.Pages);
            PdfContent content;

            switch (GetMarketer(unifiedText))
            {

                case MarketerName.Endesa:
                    content = EndesaParser.Parse(unifiedText);
                    break;

                case MarketerName.Undefined:
                default:
                    content = new PdfContent
                    {

                        Comercializadora = MarketerName.Undefined,

                    };
                    break;

            }

            content.FileName = pdfInfo.FileName;

            return content;

        }

    }

}
