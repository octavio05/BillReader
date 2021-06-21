using BillReader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Xunit.Sdk;
using static BillReader.PdfParser;

namespace BillReaderTest.Attributes
{

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class PdfDataAttribute : DataAttribute
    {

        private readonly string _path;

        public PdfDataAttribute(string path)
        {

            _path = path;
        
        }

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {

            foreach (var file in GetFilesPath())
                yield return new object[] { new string[] { File.ReadAllText(file) }, GetExpectedResult(file) };

        }

        private IEnumerable<string> GetFilesPath()
        {

            ArrayList filesPath = new ArrayList();
            DirectoryInfo d = new DirectoryInfo(_path);

            foreach (var file in d.GetFiles("*.txt"))
                filesPath.Add(string.Concat(_path, "/", file.Name));

            return (string[])filesPath.ToArray(typeof(string));
        
        }

        private PdfContent GetExpectedResult(string path)
        {

            if (path.EndsWith("20210608.txt"))
            {

                return new PdfContent
                {

                    Comercializadora = MarketerName.Endesa,
                    FechaEmision = new DateTime(2021, 6, 8),
                    InicioPeriodoFacturacion = new DateTime(2021, 5, 11),
                    FinPeriodoFacturacion = new DateTime(2021, 5, 31),
                    FechaCargo = new DateTime(2021, 6, 15),
                    PagoTotalPotencia = 9.04f,
                    PagoTotalEnergia = 16.19f,
                    TotalDescuentos = -2.03f,
                    PagoTotalOtros = 0.57f,
                    PagoTotalImpuestos = 1.23f,
                    PagoTotalServicios = 3.60f,
                    PagoTotal = 28.60f,
                    ConsumoP1 = 47,
                    ConsumoP2 = 62,
                    ConsumoP3 = 15

                };

            }
            else if (path.EndsWith("20210513.txt"))
            {

                return new PdfContent
                {

                    Comercializadora = MarketerName.Endesa,
                    FechaEmision = new DateTime(2021, 5, 13),
                    InicioPeriodoFacturacion = new DateTime(2021, 4, 13),
                    FinPeriodoFacturacion = new DateTime(2021, 5, 11),
                    FechaCargo = new DateTime(2021, 5, 20),
                    PagoTotalPotencia = 12.65f,
                    PagoTotalEnergia = 24.32f,
                    TotalDescuentos = -2.97f,
                    PagoTotalOtros = 0.80f,
                    PagoTotalImpuestos = 1.80f,
                    PagoTotalServicios = 3.72f,
                    PagoTotal = 40.32f,
                    ConsumoP1 = 77,
                    ConsumoP2 = 83,
                    ConsumoP3 = 22

                };
            
            }
            else
                throw new InvalidOperationException("No se han creado resultados para esta prueba.");

        
        }

    }
}
