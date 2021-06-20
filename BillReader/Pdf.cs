using BillReader.Interfaces;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using System;

namespace BillReader
{

    public class Pdf : IPdf
    {

        /// <summary>
        ///     Texto del fichero pdf donde cada índice del array corresponde a una página del fichero.
        /// </summary>
        public string[] PagedText { get; private set; }

        /// <summary>
        ///     Lee el fichero pdf y obtiene el texto para almacenarlo en la propiedad PagedText.
        /// </summary>
        /// <param name="path">Ruta del fichero pdf.</param>
        /// <exception cref="ArgumentNullException">path no puede ser nulo.</exception>
        /// <exception cref="IOException">Error en la lectura del fichero pdf (ruta incorrecta o vacía).</exception>
        public void Read(string path)
        {

            if (path == null)
                throw new ArgumentNullException("path", "can't be null.");

            PdfReader pdfReader = new PdfReader(path);
            PdfDocument pdfDoc = new PdfDocument(pdfReader);

            try
            {

                int numberOfPages = pdfDoc.GetNumberOfPages();
                PagedText = new string[numberOfPages];

                for (int page = 1; page <= numberOfPages; page++)
                {

                    ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                    PagedText[page - 1] = PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(page), strategy);

                }

            }
            finally
            {

                pdfDoc.Close();
                pdfReader.Close();

            }

        }

    }

}
