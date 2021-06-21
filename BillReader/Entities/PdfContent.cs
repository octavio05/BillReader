using System;
using static BillReader.PdfParser;

namespace BillReader
{

    /// <summary>
    ///     Contenido del pdf.
    /// </summary>
    public class PdfContent
    {

        /// <summary>
        ///     Nombre del fichero.
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        ///     Comercializadora.
        /// </summary>
        public MarketerName Comercializadora { get; set; }
        /// <summary>
        ///     Fecha de emisión de la factura.
        /// </summary>
        public DateTime FechaEmision { get; set; }
        /// <summary>
        ///     Fecha del inicio de período de facturación.
        /// </summary>
        public DateTime InicioPeriodoFacturacion { get; set; }
        /// <summary>
        ///     Fecha del fin de período de facturación.
        /// </summary>
        public DateTime FinPeriodoFacturacion { get; set; }
        /// <summary>
        ///     Fecha de cargo en cuenta.
        /// </summary>
        public DateTime FechaCargo { get; set; }
        /// <summary>
        ///     Coste total de la potencia.
        /// </summary>
        public float CosteTotalPotencia { get; set; }
        /// <summary>
        ///     Coste total de la energía.
        /// </summary>
        public float CosteTotalEnergia { get; set; }
        /// <summary>
        ///     Descuentos totales.
        /// </summary>
        public float TotalDescuentos { get; set; }
        /// <summary>
        ///     Coste total otros.
        /// </summary>
        public float CosteTotalOtros { get; set; }
        /// <summary>
        ///      Coste total impuestos.
        /// </summary>
        public float CosteTotalImpuestos { get; set; }
        /// <summary>
        ///     Coste total servicios.
        /// </summary>
        public float CosteTotalServicios { get; set; }
        /// <summary>
        ///     Coste total
        /// </summary>
        public float CosteTotal { get; set; }
        /// <summary>
        ///     Consumo total P1.
        /// </summary>
        public int ConsumoP1 { get; set; }
        /// <summary>
        ///     Consumo total P2.
        /// </summary>
        public int ConsumoP2 { get; set; }
        /// <summary>
        ///     Consumo total P3.
        /// </summary>
        public int ConsumoP3 { get; set; }

    }
}
