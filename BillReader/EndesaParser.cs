using BillReader.Extensions;
using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using static BillReader.PdfParser;

[assembly: InternalsVisibleTo("BillReaderTest")]
namespace BillReader
{

    /// <summary>
    ///     Clase que parsea las facturas de Endesa.
    /// </summary>
    internal static class EndesaParser
    {

        /// <summary>
        ///     Comprueba, a través de la factura, si corresponde a la comercializadora Endesa.
        /// </summary>
        /// <param name="billText">Texto de la factura.</param>
        /// <returns>True si corresponde a Endesa, False si no.</returns>
        public static bool IsValid(string billText)
            => !string.IsNullOrEmpty(billText) && billText.Contains("Endesa Energía, S.A.");

        /// <summary>
        ///     Parsea el texto recibido para convertirlo en un objeto de tipo PdfContent.
        /// </summary>
        /// <param name="billText">Texto de la factura.</param>
        /// <returns>Objeto PdfContent.</returns>
        /// <exception cref="FormatException">Formato de factura Endesa no válido.</exception>
        public static PdfContent Parse(string billText)
        {

            try
            {

                return new PdfContent
                {

                    Comercializadora = MarketerName.Endesa,
                    FechaEmision = DateTime.Parse(billText.GetStringBetween("Fecha emisión factura: ", "Periodo"), new CultureInfo("es-ES")),
                    InicioPeriodoFacturacion = DateTime.Parse(billText.GetStringBetween("Periodo de facturación: del ", " a"), new CultureInfo("es-ES")),
                    FinPeriodoFacturacion = GetFinPeriodoFacturacion(billText),
                    FechaCargo = GetFechaCargo(billText),
                    CosteTotalPotencia = float.Parse(billText.GetStringBetween("Potencia ", " €").Replace(',', '.'), CultureInfo.InvariantCulture.NumberFormat),
                    CosteTotalEnergia = float.Parse(billText.GetStringBetween("Energía ", " €").Replace(',', '.'), CultureInfo.InvariantCulture.NumberFormat),
                    TotalDescuentos = float.Parse(billText.GetStringBetween("Descuentos ", " €").Replace(',', '.'), CultureInfo.InvariantCulture.NumberFormat),
                    CosteTotalOtros = float.Parse(billText.GetStringBetween("Otros ", " €").Replace(',', '.'), CultureInfo.InvariantCulture.NumberFormat),
                    CosteTotalImpuestos = float.Parse(billText.GetStringBetween("Impuestos ", " €").Replace(',', '.'), CultureInfo.InvariantCulture.NumberFormat),
                    CosteTotalServicios = float.Parse(billText.GetStringBetween("Importe Servicios Endesa X ", " €").Replace(',', '.'), CultureInfo.InvariantCulture.NumberFormat),
                    CosteTotal = float.Parse(billText.GetStringBetween("Total importe a pagar", " €").Replace(',', '.'), CultureInfo.InvariantCulture.NumberFormat),
                    ConsumoP1 = int.Parse(billText.GetStringBetween("Consumo punta ", " kWh")),
                    ConsumoP2 = int.Parse(billText.GetStringBetween("Consumo valle ", " kWh")),
                    ConsumoP3 = int.Parse(billText.GetStringBetween("Consumo supervalle ",  " kWh"))

                };

            }
            catch (Exception ex)
            {

                throw new FormatException("Formato de factura Endesa no válido.", ex);
            
            }
        
        }

        /// <summary>
        ///     Obtiene la fecha de fin del período de facturación a partir de la factura.
        /// </summary>
        /// <param name="billText">Texto de la factura.</param>
        /// <returns>Fecha de fin del período de facturación.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FormatExcepton"></exception>
        private static DateTime GetFinPeriodoFacturacion(string billText)
        {

            var periodText = billText.GetStringBetween("Periodo de facturación: del ", ")");

            return DateTime.Parse(periodText.GetStringBetween("a ", " ("), new CultureInfo("es-ES"));
        
        }

        /// <summary>
        ///     Obtiene la fecha del cargo a partir de la factura.
        /// </summary>
        /// <param name="billText">Texto de la factura.</param>
        /// <returns>Fecha del cargo.</returns>
        /// <exception cref="FormatException"></exception>
        /// <exception cref="OverflowException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private static DateTime GetFechaCargo(string billText)
        {

            var dateTimeText = billText.GetStringBetween("Fecha de cargo: ", "....").Split(' ');

            int
                day = Convert.ToInt32(dateTimeText[0]),
                month = DateTime.ParseExact(dateTimeText[2], "MMMM", new CultureInfo("es-ES")).Month,
                year = Convert.ToInt32(dateTimeText[4]);

            return new DateTime(year, month, day);

        }

    }
}
