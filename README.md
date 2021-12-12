<h1 align="center">
  BillReader
</h1>

<div align="center">
 
 [![BillReader Linux Test](https://github.com/octavio05/BillReader/actions/workflows/testLinux.yml/badge.svg)](https://github.com/octavio05/BillReader/actions/workflows/testLinux.yml)
[![BillReader Windows Test](https://github.com/octavio05/BillReader/actions/workflows/testWindows.yml/badge.svg)](https://github.com/octavio05/BillReader/actions/workflows/testWindows.yml)
[![BillReader MacOS Test](https://github.com/octavio05/BillReader/actions/workflows/testMacOS.yml/badge.svg)](https://github.com/octavio05/BillReader/actions/workflows/testMacOS.yml)
 
 </div>
            
<i>BillReader</i> es una librería de clases desarrollada en C# .NET Core 3.1 que es capaz de leer una factura eléctrica y obtener información de interés de ella para su posterior manejo.

### Licencia
[![License](https://img.shields.io/badge/License-Apache%202.0-blue.svg)](https://opensource.org/licenses/Apache-2.0)
<br />
<i>BillReader</i> está bajo la licencia [Apache License, Version 2.0](https://opensource.org/licenses/Apache-2.0) 

### Comercializadoras disponibles
Actualmente las comercializadoras disponibles son:

- Endesa

### Guía de uso
El uso de esta librería es bastante sencillo. Primero obtendremos un objeto de tipo Pdf que recuperará la información del pdf que queremos parsear. Posteriormente, este pdf se enviará por parámetro a la clase PdfParser, que será la que obtenga la información de la factura.

```c#
// Crea el objeto IPdf.
string path = @"pdf/file/path.pdf";
IPdf pdf = new Pdf();

// Lee el pdf
pdf.Read(path); // Se puede leer a partir del path.
// pdf.Read(new FileStream(path, FileMode.Open)); // O a partir de un objeto FileStream.

// Obtiene información del pdf y la almacena en PdfContent.
PdfParser parser = new PdfParser(pdf);
PdfContent content = parser.Parse();
``` 
