using System;
using Xunit;
using FluentAssertions;
using BillReader;
using System.IO;

namespace BillReaderTest.Unitary
{
    public class PdfTest
    {

        private Pdf _pdf = new Pdf();

        [Fact]
        public void CreaInstancia_Correcto()
        {

            // Arrange

            // Act
            var result = new Pdf();

            // Assert
            result.Should().NotBeNull().And.BeOfType<Pdf>();

        }

        [Fact]
        public void Read_RecibePathValido_Correcto()
        {

            // Arrange
            var path = @"../../../Files/testPdf.pdf";

            // Act
            var result = _pdf.Read(path);

            // Assert
            result.FileName.Should().Be("testPdf.pdf");
            result.Pages.Should().NotBeNull();

        }

        [Theory]
        [InlineData(@"../../../Files/testPdfNotExists.pdf")]
        [InlineData("")]
        [InlineData("NoEsPath")]
        public void Read_RecibePathNoValido_IOException(string path)
        {

            // Arrange

            // Act
            Action action = () => _pdf.Read(path);

            // Assert        
            action.Should().Throw<IOException>();

        }

        [Fact]
        public void Read_RecibePathNulo_ArgumentNullException()
        {

            // Arrange
            string path = null;

            // Act
            Action action = () => _pdf.Read(path);

            // Assert
            action.Should().Throw<ArgumentNullException>();

        }

        [Fact]
        public void Read_RecibeFileStream_Correcto()
        {

            // Arrange
            var path = @"../../../Files/testPdf.pdf";
            var fileStream = new FileStream(path, FileMode.Open);

            // Act
            var result = _pdf.Read(fileStream);

            // Assert
            result.FileName.Should().Be("testPdf.pdf");
            result.Pages.Should().NotBeNull();

        }

        [Fact]
        public void Read_RecibeFileStreamNulo_ArgumentNullException()
        {

            // Arrange
            FileStream stream = null;

            // Act
            Action action = () => _pdf.Read(stream);

            // Assert
            action.Should().Throw<ArgumentNullException>();

        }

    }
}
