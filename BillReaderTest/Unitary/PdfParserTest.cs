using BillReader;
using BillReader.Interfaces;
using BillReaderTest.Attributes;
using FluentAssertions;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;
using static BillReader.PdfParser;

namespace BillReaderTest.Unitary
{
    public class PdfParserTest
    {

        Mock<IPdfInfo> pdfMock = new Mock<IPdfInfo>(MockBehavior.Strict);

        [Fact]
        public void CreaInstancia_Correctamente()
        {

            // Arrange

            // Act
            var pdfParser = new PdfParser();

            // Assert
            pdfParser.Should().NotBeNull().And.BeOfType<PdfParser>();

        }

        [Fact]
        public void Parse_ParametroObjetoPdf_ComercializadoraNoDefinida_RetornaPdfContent_Correctamente()
        {

            // Arrange
            pdfMock.SetupGet(x => x.Pages).Returns(new string[] { "Contenido de comercializadora no definida." });
            pdfMock.SetupGet(x => x.FileName).Returns("notDefined.pdf");
            var pdfParser = new PdfParser();

            // Act
            var result = pdfParser.Parse(pdfMock.Object);

            // Assert
            result.Should().BeOfType<PdfContent>();
            result.Comercializadora.Should().Be(MarketerName.Undefined);
            result.FileName.Should().Be("notDefined.pdf");

        }

        [Fact]
        public void Parse_ParametroObjetoPdf_Nulo_ArgumentNullException()
        {

            // Arrange
            var pdfParser = new PdfParser();
            IPdfInfo pdf = null;

            // Act
            Action result = () => pdfParser.Parse(pdf);

            // Assert
            result.Should().Throw<ArgumentNullException>();

        }

        [Fact]
        public void Parse_ParametroObjetoPdf_PagedTextNulo_ArgumentNullException()
        {

            // Arrange
            var pdfParser = new PdfParser();
            pdfMock.SetupGet(x => x.Pages).Returns((string[])null);

            // Act
            Action result = () => pdfParser.Parse(pdfMock.Object);

            // Result
            result.Should().Throw<ArgumentNullException>();

        }

        [Fact]
        public void Parse_ParametroObjetoPdf_FormatoPdfNoValido_FormatException()
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
            pdfMock.SetupGet(x => x.Pages).Returns(new string[] { txt });
            var pdfParser = new PdfParser();

            // Act
            Action result = () => pdfParser.Parse(pdfMock.Object);

            // Assert
            result.Should().Throw<FormatException>();

        }

        [Theory]
        [PdfData(@"../../../Files", false)]
        public void Parse_ParametroObjetoPdf_RetornaPdfContent_Correctamente(string[] pageContent, PdfContent expected)
        {

            // Arrange
            pdfMock.SetupGet(x => x.Pages).Returns(pageContent);
            pdfMock.SetupGet(x => x.FileName).Returns("file.pdf");
            var pdfParser = new PdfParser();

            // Act
            var result = pdfParser.Parse(pdfMock.Object);

            // Assert
            result.Should().BeEquivalentTo(expected);

        }

        [Fact]
        public void Parse_ParametroColeccionPdf_ComercializadoraNoDefinida_RetornaColeccionPdfContent_Correctamente()
        {

            // Arrange
            Mock<IPdfInfo>
                pdfMock1 = new Mock<IPdfInfo>(MockBehavior.Strict),
                pdfMock2 = new Mock<IPdfInfo>(MockBehavior.Strict),
                pdfMock3 = new Mock<IPdfInfo>(MockBehavior.Strict);

            pdfMock1.SetupGet(x => x.Pages).Returns(new string[] { "Contenido de comercializadora no definida." });
            pdfMock1.SetupGet(x => x.FileName).Returns("notDefined1.pdf");
            pdfMock2.SetupGet(x => x.Pages).Returns(new string[] { "Contenido de comercializadora no definida." });
            pdfMock2.SetupGet(x => x.FileName).Returns("notDefined2.pdf");
            pdfMock3.SetupGet(x => x.Pages).Returns(new string[] { "Contenido de comercializadora no definida." });
            pdfMock3.SetupGet(x => x.FileName).Returns("notDefined3.pdf");

            var expected = new PdfContent[]
                {
                    new PdfContent
                    {
                        Comercializadora = MarketerName.Undefined,
                        FileName = "notDefined1.pdf"
                    },
                    new PdfContent
                    {
                        Comercializadora = MarketerName.Undefined,
                        FileName = "notDefined2.pdf"
                    },
                    new PdfContent
                    {
                        Comercializadora = MarketerName.Undefined,
                        FileName = "notDefined3.pdf"
                    }
                };

            var pdfMockColl = new IPdfInfo[] { pdfMock1.Object, pdfMock2.Object, pdfMock3.Object };

            PdfParser pdfParser = new PdfParser();

            // Act
            var result = pdfParser.Parse(pdfMockColl);

            // Result
            result.Should().BeEquivalentTo(expected);

        }

        [Fact]
        public void Parse_ParametroColeccionPdf_Nulo_ArgumentNullException()
        {

            // Arrange
            var pdfParser = new PdfParser();
            IEnumerable<IPdfInfo> pdfs = null;

            // Act
            Action result = () => pdfParser.Parse(pdfs);

            // Assert
            result.Should().Throw<ArgumentNullException>();
        
        }

        [Fact]
        public void Parse_ParametroColeccionPdf_PagedTextNulo_RetornaColeccionPdfContent_Correctamente()
        {

            // Arrange
            Mock<IPdfInfo>
                pdfMock1 = new Mock<IPdfInfo>(MockBehavior.Strict),
                pdfMock2 = new Mock<IPdfInfo>(MockBehavior.Strict),
                pdfMock3 = new Mock<IPdfInfo>(MockBehavior.Strict);

            pdfMock1.SetupGet(x => x.Pages).Returns((string[])null);
            pdfMock1.SetupGet(x => x.FileName).Returns("notDefined1.pdf");
            pdfMock2.SetupGet(x => x.Pages).Returns((string[])null);
            pdfMock2.SetupGet(x => x.FileName).Returns("notDefined2.pdf");
            pdfMock3.SetupGet(x => x.Pages).Returns((string[])null);
            pdfMock3.SetupGet(x => x.FileName).Returns("notDefined3.pdf");

            var expected = new PdfContent[]
                {
                    new PdfContent
                    {
                        Comercializadora = MarketerName.Undefined,
                        FileName = "notDefined1.pdf"
                    },
                    new PdfContent
                    {
                        Comercializadora = MarketerName.Undefined,
                        FileName = "notDefined2.pdf"
                    },
                    new PdfContent
                    {
                        Comercializadora = MarketerName.Undefined,
                        FileName = "notDefined3.pdf"
                    }
                };

            var pdfMockColl = new IPdfInfo[] { pdfMock1.Object, pdfMock2.Object, pdfMock3.Object };

            PdfParser pdfParser = new PdfParser();

            // Act
            var result = pdfParser.Parse(pdfMockColl);

            // Assert
            result.Should().BeEquivalentTo(expected);

        }

        [Fact]
        public void Parse_ParametroColeccionPdf_FormatoPdfNoValido_RetornaColeccionPdfContent_Correctamente()
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

            Mock<IPdfInfo>
                pdfMock1 = new Mock<IPdfInfo>(MockBehavior.Strict),
                pdfMock2 = new Mock<IPdfInfo>(MockBehavior.Strict),
                pdfMock3 = new Mock<IPdfInfo>(MockBehavior.Strict);

            pdfMock1.SetupGet(x => x.Pages).Returns(new string[] { txt });
            pdfMock1.SetupGet(x => x.FileName).Returns("notDefined1.pdf");
            pdfMock2.SetupGet(x => x.Pages).Returns(new string[] { txt });
            pdfMock2.SetupGet(x => x.FileName).Returns("notDefined2.pdf");
            pdfMock3.SetupGet(x => x.Pages).Returns(new string[] { txt });
            pdfMock3.SetupGet(x => x.FileName).Returns("notDefined3.pdf");

            var expected = new PdfContent[]
                {
                    new PdfContent
                    {
                        Comercializadora = MarketerName.Undefined,
                        FileName = "notDefined1.pdf"
                    },
                    new PdfContent
                    {
                        Comercializadora = MarketerName.Undefined,
                        FileName = "notDefined2.pdf"
                    },
                    new PdfContent
                    {
                        Comercializadora = MarketerName.Undefined,
                        FileName = "notDefined3.pdf"
                    }
                };

            var pdfMockColl = new IPdfInfo[] { pdfMock1.Object, pdfMock2.Object, pdfMock3.Object };

            PdfParser pdfParser = new PdfParser();

            // Act
            var result = pdfParser.Parse(pdfMockColl);

            // Assert
            result.Should().BeEquivalentTo(expected);

        }

        [Theory]
        [PdfData(@"../../../Files", true)]
        public void Parse_ParametroColeccionPdf_RetornaColeccionPdfContent_Correctamente(ArrayList pageContent, PdfContent[] expected)
        {

            // Arrange
            Mock<IPdfInfo>
                pdfMock1 = new Mock<IPdfInfo>(MockBehavior.Strict),
                pdfMock2 = new Mock<IPdfInfo>(MockBehavior.Strict);

            pdfMock1.SetupGet(x => x.Pages).Returns((string[])pageContent[0]);
            pdfMock1.SetupGet(x => x.FileName).Returns("file.pdf");
            pdfMock2.SetupGet(x => x.Pages).Returns((string[])pageContent[1]);
            pdfMock2.SetupGet(x => x.FileName).Returns("file.pdf");

            var pdfParser = new PdfParser();

            var pdfMockColl = new IPdfInfo[] { pdfMock1.Object, pdfMock2.Object };

            // Act
            var result = pdfParser.Parse(pdfMockColl);

            // Assert
            result.Should().BeEquivalentTo(expected);

        }

    }
}
