using BillReader.Entities;
using BillReader.Interfaces;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using System;
using System.Collections;
using System.IO;

namespace BillReader
{

    /// <summary>
    ///     Lee los ficheros pdf y genera una clase PdfInfo.
    /// </summary>
    public class Pdf : IPdf
    {

        /// <summary>
        ///     Lee el fichero pdf y obtiene el texto para almacenarlo en la propiedad PagedText.
        /// </summary>
        /// <param name="path">Ruta del fichero pdf.</param>
        /// <returns>Objeto de tipo PdfInfo.</returns>
        /// <exception cref="ArgumentNullException">path no puede ser nulo.</exception>
        /// <exception cref="IOException">Error en la lectura del fichero pdf (ruta incorrecta o vacía).</exception>
        public IPdfInfo Read(string path)
        {

            if (path == null)
                throw new ArgumentNullException("path", "can't be null.");

            PdfReader pdfReader = new PdfReader(path);
            PdfDocument pdfDoc = new PdfDocument(pdfReader);
            ArrayList pages = new ArrayList();

            try
            {

                for (int page = 1, len = pdfDoc.GetNumberOfPages(); page <= len; page++)
                {

                    ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                    pages.Add(PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(page), strategy));

                }

            }
            finally
            {

                pdfDoc.Close();
                pdfReader.Close();

            }

            return new PdfInfo
            { 
                
                FileName = getFileName(path),
                Pages = (string[])pages.ToArray(typeof(string))

            };

        }

        /// <summary>
        ///     Obtiene el nombre del fichero a partir de la ruta.
        /// </summary>
        /// <param name="path">Ruta del fichero.</param>
        /// <returns>Nombre del fichero.</returns>
        private string getFileName(string path)
        {

            var pathSplit = path.Split('/');
            return pathSplit[pathSplit.Length - 1];

        }

    }

}
