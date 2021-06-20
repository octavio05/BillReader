using Xunit;
using BillReader;
using FluentAssertions;

namespace BillReaderTest.Unitary
{
    public class EndesaParserTest
    {

        [Fact]
        public void IsValid_StringCorrecta_RetornaTrue_Correctamente()
        {

            // Arrange
            var text = "Endesa Energía, S.A.";

            // Act
            var result = EndesaParser.IsValid(text);

            // Assert
            result.Should().BeTrue();
        
        }

        [Theory]
        [InlineData("String incorrecta")]
        [InlineData("")]
        [InlineData(null)]
        public void IsValid_StringIncorrecta_RetornaFalse_Correctamente(string text)
        {

            // Arrange

            // Act
            var result = EndesaParser.IsValid(text);

            // Assert
            result.Should().BeFalse();
        
        }

    }
}
