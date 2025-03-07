using System;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using Newtonsoft.Json;
using ReadEDIFACT;
using ReadEDIFACT.Models; // Add this line if ParserEDI is in the Parsers namespace


// Lee el archivo EDI
string filePathBAPLIE = @"C:\Users\mbermudez\Documents\ReadBaplie\ReadEDIFACT\BAPLIE_Export.EDI";
// string filePathMOVINS = @"C:\Users\mbermudez\Documents\ReadBaplie\ReadEDIFACT\MOVINS DMR V09.edi";
// string filePath = @"C:\Users\mbermudez\Documents\ReadBaplie\ReadEDIFACT\DEPARTURE BAPLIE FINAL - DEL MONTE ROSE - 25360187.edi";
// string filePath = @"C:\Users\mbermudez\OneDrive - JAPDEVA\Proyecto PortLogistics\EDI BAPLEY\Ejemplos\BAPLIE\CRMOB DEP DMG V42.edi";

// string outputFilePathMOVINS = @"C:\Users\mbermudez\Documents\ReadBaplie\ReadEDIFACT\MOVINS DMR V09.json";
// string outputFilePath = @"C:\Users\mbermudez\Documents\ReadBaplie\ReadEDIFACT\DEPARTURE BAPLIE FINAL - DEL MONTE ROSE - 25360187.json";
string outputFilePathBAPLIE = @"C:\Users\mbermudez\Documents\ReadBaplie\ReadEDIFACT\BAPLIE_Export4.json";
// try
// {
//     var parser = new ParserEDI("", definitionBAPLIE);
//     // Leer el contenido del archivo JSON
//             string jsonContent = File.ReadAllText(outputFilePathBAPLIE);
//             Console.WriteLine("Contenido del JSON:");
//             Console.WriteLine(jsonContent);

//             // Generar el archivo EDI a partir del JSON
//             string ediContent = parser.GenerateEDIFromJson(jsonContent);
//             Console.WriteLine($"EDI GENERADO:: {ediContent}");
// }
// catch (System.Exception)
// {
    
//     throw;
// }

// using (var reader = new StreamReader(filePathBAPLIE))
// {
//     try
//     {
//         var parser = new ParserEDI(reader, definitionBAPLIE);

//         // Validar el archivo EDI
//         var validationErrors = parser.ValidateFullEDI(definitionBAPLIE.Name);
//         // Console.WriteLine($"validationErrors: {validationErrors}");
//         if (validationErrors.Any())
//         {
//             Console.WriteLine("Errores de validación:");
//             foreach (var error in validationErrors)
//             {
//                 Console.WriteLine(error);
//             }
//         }
//         else
//         {
//             // Guarda el JSON en un archivo
//             parser.SaveJsonToFile(outputFilePathBAPLIE);
//             Console.WriteLine("Archivo JSON guardado en: " + outputFilePathBAPLIE);
//             // Leer el contenido del archivo JSON
//             string jsonContent = File.ReadAllText(outputFilePathBAPLIE);
//             Console.WriteLine("Contenido del JSON:");
//             Console.WriteLine(jsonContent);

//             // Generar el archivo EDI a partir del JSON
//             string ediContent = parser.GenerateEDIFromJson(jsonContent);
//             Console.WriteLine($"EDI GENERADO:: {ediContent}");
            
//         }
//     }
//     catch (Exception ex)
//     {
//         Console.WriteLine($"Error al procesar el archivo EDI: {ex.Message}");
//     }
// }

// using (var sw = new StreamWriter(string.Format(@"C:\Users\mbermudez\Documents\ReadBaplie\ReadEDIFACT\{0}Output.txt", definitionBAPLIE.Name)))
// {
//     sw.Write(definitionBAPLIE);
// }

// using (var reader = new StreamReader(filePathMOVINS))
// {
//     try
//     {
//         var parser = new ParserEDI(reader, definitionMOVINS);

//         // Validar el archivo EDI
//         var validationErrors = parser.ValidateFullEDI(definitionMOVINS.Name);
//         // Console.WriteLine($"validationErrors: {validationErrors}");
//         if (validationErrors.Any())
//         {
//             Console.WriteLine("Errores de validación:");
//             foreach (var error in validationErrors)
//             {
//                 Console.WriteLine(error);
//             }
//         }
//         else
//         {
//             // Guarda el JSON en un archivo
//             parser.SaveJsonToFile(outputFilePathMOVINS);
//             Console.WriteLine("Archivo JSON guardado en: " + outputFilePathMOVINS);
//             // Leer el contenido del archivo JSON
//             string jsonContent = File.ReadAllText(outputFilePathMOVINS);
//             Console.WriteLine("Contenido del JSON:");
//             Console.WriteLine(jsonContent);

//             // Generar el archivo EDI a partir del JSON
//             string ediContent = parser.GenerateEDIFromJson(jsonContent);
//             Console.WriteLine($"EDI GENERADO:: {ediContent}");
            
//         }
//     }
//     catch (Exception ex)
//     {
//         Console.WriteLine($"Error al procesar el archivo EDI: {ex.Message}");
//     }
// }

// using (var sw = new StreamWriter(string.Format(@"C:\Users\mbermudez\Documents\ReadBaplie\ReadEDIFACT\{0}Output.txt", definitionMOVINS.Name)))
// {
//     sw.Write(definitionMOVINS);
// }

// Console.WriteLine();
// Console.WriteLine("Press any key to exit...");

// Console.ReadKey();
var baplie = new BaplieVersion3();

using (var reader = new StreamReader(filePathBAPLIE))
{
    try
    {
        var parser = new ParserEDI(reader, baplie);

        // Validar el archivo EDI
        var validationErrors = parser.ValidateFullEDI(baplie.Name);
        // Console.WriteLine($"validationErrors: {validationErrors}");
        if (validationErrors.Any())
        {
            Console.WriteLine("Errores de validación:");
            foreach (var error in validationErrors)
            {
                Console.WriteLine(error);
            }
        }
        else
        {
            // Guarda el JSON en un archivo
            parser.SaveJsonToFile(outputFilePathBAPLIE);
            Console.WriteLine("Archivo JSON guardado en: " + outputFilePathBAPLIE);
            // Leer el contenido del archivo JSON
            string jsonContent = File.ReadAllText(outputFilePathBAPLIE);
            Console.WriteLine("Contenido del JSON:");
            Console.WriteLine(jsonContent);

            // Generar el archivo EDI a partir del JSON
            string ediContent = parser.GenerateEDIFromJson(jsonContent);
            Console.WriteLine($"EDI GENERADO:: {ediContent}");
            
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error al procesar el archivo EDI: {ex.Message}");
    }
}
Console.WriteLine($"BAPLIE VERSION 3.1.1 {baplie}");

using (var sw = new StreamWriter(string.Format(@"C:\Users\mbermudez\Documents\ReadBaplie\ReadEDIFACT\{0}Output.txt", baplie.Name)))
{
    sw.Write(baplie);
}
