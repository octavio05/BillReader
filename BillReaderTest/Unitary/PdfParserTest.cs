using BillReader;
using BillReader.Interfaces;
using BillReaderTest.Attributes;
using FluentAssertions;
using Moq;
using System;
using Xunit;

namespace BillReaderTest.Unitary
{
    public class PdfParserTest
    {

        Mock<IPdf> pdfMock = new Mock<IPdf>(MockBehavior.Strict);

        [Fact]
        public void CreaInstancia_Correctamente()
        {

            // Arrange
            pdfMock.SetupGet(x => x.PagedText).Returns(new string[] { "Page 1", "Page 2" });

            // Act
            var pdfParser = new PdfParser(pdfMock.Object);

            // Assert
            pdfParser.Should().NotBeNull().And.BeOfType<PdfParser>();
        
        }

        [Fact]
        public void CreaInstancia_RecibePdfNulo_ArgumentNullException()
        {

            // Arrange

            // Act
            Action pdfParser = () => new PdfParser(null);

            // Assert
            pdfParser.Should().Throw<ArgumentNullException>();
        
        }

        [Fact]
        public void CreaInstancia_RecibePagedTextNulo_ArgumentNullException()
        {

            // Arrange
            pdfMock.SetupGet(x => x.PagedText).Returns((string[])null);

            // Act
            Action result = () => new PdfParser(pdfMock.Object);

            // Assert
            result.Should().Throw<ArgumentNullException>();
        
        }

        [Fact]
        public void Parser_ComercializadoraNoDefinida_InvalidOperationException()
        {

            // Arrange
            pdfMock.SetupGet(x => x.PagedText).Returns(new string[] { "Contenido de comercializadora no definida." });
            var pdfParser = new PdfParser(pdfMock.Object);

            // Act
            Action result = () => pdfParser.Parse();

            // Assert
            result.Should().Throw<InvalidOperationException>();
        
        }

        [Fact]
        public void Parser_FormatoPdfNoValido_FormatException()
        {

            // Arrange
            var txt = @"
                Endesa Energía, S.A. Unipersonal. Inscrita en el Registro Mercantil de Madrid. Tomo 12.797, Libro 0, Folio 208,
                 Sección 8ª, Hoja M-205.381, CIF A81948077. Domicilio Social: C/Ribera del Loira, nº60 28042 - Madrid.
                Endesa Energía, S.A. Unipersonal.
                CIF A81948077.
                C/Albareda nº 38 35008 - Las Palmas de Gran Canaria
                DATOS DE LA FACTURA
                Nº factura: NMP108N0006054
                Referencia: 130008765511/0047
                Fecha emisión factu: 08/06/2021";
            pdfMock.SetupGet(x => x.PagedText).Returns(new string[] { txt });
            var pdfParser = new PdfParser(pdfMock.Object);

            // Act
            Action result = () => pdfParser.Parse();

            // Assert
            result.Should().Throw<FormatException>();
        
        }

        [Theory]
        [PdfData(@"../../../Files")]
        public void Parser_RetornaPdfContent_Correctamente(string[] pageContent, PdfContent expected)
        {

            // Arrange
            pdfMock.SetupGet(x => x.PagedText).Returns(pageContent);
            pdfMock.SetupGet(x => x.FileName).Returns("file.pdf");
            var pdfParser = new PdfParser(pdfMock.Object);

            // Act
            var result = pdfParser.Parse();

            // Assert
            result.Should().BeEquivalentTo(expected);
        
        }

    }
}
