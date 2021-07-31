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

        private enum TipoConsumo { Normal, Supervalle };

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

            var tConsumo = GetTipoConsumo(billText);

            try
            {

                return new PdfContent
                {

                    Comercializadora = MarketerName.Endesa,
                    FechaEmision = billText.GetDateTimeBetween("Fecha emisión factura: ", "Periodo"),
                    InicioPeriodoFacturacion = billText.GetDateTimeBetween("Periodo de facturación: del ", " a"),
                    FinPeriodoFacturacion = GetFinPeriodoFacturacion(billText),
                    FechaCargo = GetFechaCargo(billText),
                    CosteTotalPotencia = billText.GetFloatBetween("Potencia ", " €"),
                    CosteTotalEnergia = billText.GetFloatBetween("Energía ", " €"),
                    TotalDescuentos = billText.GetFloatBetween("Descuentos ", " €"),
                    CosteTotalOtros = billText.GetFloatBetween("Otros ", " €"),
                    CosteTotalImpuestos = billText.GetFloatBetween("Impuestos ", " €"),
                    CosteTotalServicios = billText.GetFloatBetween("Importe Servicios Endesa X ", " €"),
                    CosteTotal = GetCosteTotal(billText),
                    ConsumoP1 = billText.GetStringBetween<int>("Consumo punta ", " kWh"),
                    ConsumoP2 = GetConsumoP2(billText, tConsumo),
                    ConsumoP3 = GetConsumoP3(billText, tConsumo)

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

            var periodText = billText.GetStringBetween<string>("Periodo de facturación: del ", ")");

            return periodText.GetDateTimeBetween("a ", " (");
        
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

            var dateTimeText = billText.GetStringBetween<string>("Fecha de cargo: ", "....").Split(' ');

            int
                day = Convert.ToInt32(dateTimeText[0]),
                month = DateTime.ParseExact(dateTimeText[2], "MMMM", new CultureInfo("es-ES")).Month,
                year = Convert.ToInt32(dateTimeText[4]);

            return new DateTime(year, month, day);

        }

        /// <summary>
        ///     Obtiene el coste total a partir de la factura.
        /// </summary>
        /// <param name="billText">Texto de la factura.</param>
        /// <returns>Coste total.</returns>
        private static float GetCosteTotal(string billText)
        {

            var costeTotal = billText.GetFloatBetween("Total importe a pagar", " €");

            if (costeTotal.Equals(default))
                costeTotal = billText.GetFloatBetween("Total", " €");

            return costeTotal;

        }

        /// <summary>
        ///     Obtiene el consumo p2 a partir de la factura.
        /// </summary>
        /// <param name="billText">Texto de la factura.</param>
        /// <param name="tConsumo">Tipo de consumo. (Normal, supervalle...)</param>
        /// <returns>Consumo P2.</returns>
        /// <exception cref="FormatException">Formato de tipo de consumo p2 no válido.</exception>
        private static int GetConsumoP2(string billText, TipoConsumo tConsumo)
        {

            switch (tConsumo)
            {

                case TipoConsumo.Normal:
                    return billText.GetStringBetween<int>("Consumo llano ", " kWh");

                case TipoConsumo.Supervalle:
                    return billText.GetStringBetween<int>("Consumo valle ", " kWh");

                default:
                    throw new FormatException("Formato de tipo de consumo p2 no válido.");

            }
        
        }

        /// <summary>
        ///     Obtiene el consumo p3 a partir de la factura.
        /// </summary>
        /// <param name="billText">Texto de la factura.</param>
        /// <param name="tConsumo">Tipo de consumo. (Normal, supervalle...)</param>
        /// <returns>Consumo P3.</returns>
        /// <exception cref="FormatException">Formato de tipo de consumo p3 no válido.</exception>
        private static int GetConsumoP3(string billText, TipoConsumo tConsumo)
        {

            switch (tConsumo)
            {

                case TipoConsumo.Normal:
                    return billText.GetStringBetween<int>("Consumo valle ", " kWh");

                case TipoConsumo.Supervalle:
                    return billText.GetStringBetween<int>("Consumo supervalle ", " kWh");

                default:
                    throw new FormatException("Formato de tipo de consumo p3 no válido.");

            }
        
        }

        /// <summary>
        ///     Obtiene el tpo de consumo (Normal, supervalle...).
        /// </summary>
        /// <param name="billText">Texto de la factura.</param>
        /// <returns>Tipo de consumo.</returns>
        private static TipoConsumo GetTipoConsumo(string billText)
        {

            if (billText.IndexOf("Consumo supervalle ") != -1 &&
                int.TryParse(billText.GetStringBetween<string>("Consumo supervalle ", " kWh"), out _))
            {

                return TipoConsumo.Supervalle;

            }

            return TipoConsumo.Normal;
        
        }

    }

}
