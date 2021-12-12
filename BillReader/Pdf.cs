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

            return CreatePdfInfo(path);

        }

        /// <summary>
        ///     Lee el fichero pdf y obtiene el texto para almacenarlo en la propiedad PagedText.
        /// </summary>
        /// <param name="fileStream">Fichero</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">fileStream no puede ser nulo.</exception>
        public IPdfInfo Read(FileStream fileStream)
        {

            if (fileStream == null)
                throw new ArgumentNullException("can't be null.", "fileStream");

            return CreatePdfInfo(fileStream);

        }

        /// <summary>
        ///     Crea un objeto que implemente la interfaz IPdfInfo.
        /// </summary>
        /// <param name="fileObject">Objeto que representa el fichero. Puede ser un path o un FileStream.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">fileObject can't be null.</exception>
        /// <exception cref="InvalidOperationException">Tipo fileObject no válido.</exception>
        /// <exception cref="IOException">Error en la lectura del fichero pdf (ruta incorrecta o vacía).</exception>
        private IPdfInfo CreatePdfInfo(object fileObject)
        {

            if (fileObject == null)
                throw new ArgumentNullException("can't be null.", "fileObject");

            // Crea el objeto pdfReader.
            PdfReader pdfReader;
            string path;

            if (fileObject is FileStream)
            {

                FileStream fileStream = fileObject as FileStream;
                path = fileStream.Name;

                pdfReader = new PdfReader(fileStream);

            }
            else if (fileObject is string)
            {

                path = fileObject as string;

                pdfReader = new PdfReader(path);

            }
            else
                throw new InvalidOperationException("fileObject type not valid.");

            // Intenta extraer la información del documento.
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

                FileName = Path.GetFileName(path),
                Pages = (string[])pages.ToArray(typeof(string))

            };

        }

    }

}
