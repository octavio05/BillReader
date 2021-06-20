using System;

namespace BillReader
{
    public class PdfContent
    {

        public string Comercializadora { get; set; }
        public DateTime FechaEmision { get; set; }
        public DateTime InicioPeriodoFacturacion { get; set; }
        public DateTime FinPeriodoFacturacion { get; set; }
        public DateTime FechaCargo { get; set; }
        public float PagoTotalPotencia { get; set; }
        public float PagoTotalEnergia { get; set; }
        public float TotalDescuentos { get; set; }
        public float PagoTotalOtros { get; set; }
        public float PagoTotalImpuestos { get; set; }
        public float PagoTotalServicios { get; set; }
        public float PagoTotal { get; set; }
        public int ConsumoP1 { get; set; }
        public int ConsumoP2 { get; set; }
        public int ConsumoP3 { get; set; }

    }
}
