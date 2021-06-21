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
        private readonly bool _returnCollection;

        public PdfDataAttribute(string path, bool returnCollection)
        {

            _path = path;
            _returnCollection = returnCollection;
        
        }

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {

            if (_returnCollection)
            {

                ArrayList listFiles = new ArrayList();
                ArrayList listExpected = new ArrayList();

                foreach (var file in GetFilesPath())
                {

                    listFiles.Add(new string[] { File.ReadAllText(file) });
                    listExpected.Add(GetExpectedResult(file));

                }

                yield return new object[] 
                {
                    
                    listFiles, 
                    (PdfContent[])listExpected.ToArray(typeof(PdfContent)) 

                };

            }
            else
            {

                foreach (var file in GetFilesPath())
                    yield return new object[] { new string[] { File.ReadAllText(file) }, GetExpectedResult(file) };

            }

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

                    FileName = "file.pdf",
                    Comercializadora = MarketerName.Endesa,
                    FechaEmision = new DateTime(2021, 6, 8),
                    InicioPeriodoFacturacion = new DateTime(2021, 5, 11),
                    FinPeriodoFacturacion = new DateTime(2021, 5, 31),
                    FechaCargo = new DateTime(2021, 6, 15),
                    CosteTotalPotencia = 9.04f,
                    CosteTotalEnergia = 16.19f,
                    TotalDescuentos = -2.03f,
                    CosteTotalOtros = 0.57f,
                    CosteTotalImpuestos = 1.23f,
                    CosteTotalServicios = 3.60f,
                    CosteTotal = 28.60f,
                    ConsumoP1 = 47,
                    ConsumoP2 = 62,
                    ConsumoP3 = 15

                };

            }
            else if (path.EndsWith("20210513.txt"))
            {

                return new PdfContent
                {

                    FileName = "file.pdf",
                    Comercializadora = MarketerName.Endesa,
                    FechaEmision = new DateTime(2021, 5, 13),
                    InicioPeriodoFacturacion = new DateTime(2021, 4, 13),
                    FinPeriodoFacturacion = new DateTime(2021, 5, 11),
                    FechaCargo = new DateTime(2021, 5, 20),
                    CosteTotalPotencia = 12.65f,
                    CosteTotalEnergia = 24.32f,
                    TotalDescuentos = -2.97f,
                    CosteTotalOtros = 0.80f,
                    CosteTotalImpuestos = 1.80f,
                    CosteTotalServicios = 3.72f,
                    CosteTotal = 40.32f,
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
