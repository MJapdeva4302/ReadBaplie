using System;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using Newtonsoft.Json;
using ReadEDIFACT;
using ReadEDIFACT.Models;
using ReadEDIFACT.Models.Coarri; // Add this line if ParserEDI is in the Parsers namespace


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
// var baplie = new BaplieVersion3();

// using (var reader = new StreamReader(filePathBAPLIE))
// {
//     try
//     {
//         var parser = new ParserEDI(reader, baplie);

//         // Validar el archivo EDI
//         var validationErrors = parser.ValidateFullEDI(baplie.Name);
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
// Console.WriteLine($"BAPLIE VERSION 3.1.1 {baplie}");

// using (var sw = new StreamWriter(string.Format(@"C:\Users\mbermudez\Documents\ReadBaplie\ReadEDIFACT\{0}Output.txt", baplie.Name)))
// {
//     sw.Write(baplie);
// }


// // Leer el archivo JSON
//             string json = File.ReadAllText("data.json");
//             var root = JsonConvert.DeserializeObject<Root>(json);

//             // Datos del JSON
//             var arrivalData = root.ArrivalData;
//             var equipments = root.Equipments;

//             // Construir el mensaje COARRI
//             var builder = new COARRIMessageBuilder(
//                 equipments,
//                 arrivalData.ShippingLineIdentification, // Sender
//                 "CHIQ2BBX", // Receiver (este valor no está en el JSON, lo dejo como ejemplo)
//                 arrivalData.ArrivalNumber.ToString(), // InterchangeRef
//                 arrivalData.ShipName, // VesselName
//                 arrivalData.ArrivalNumber.ToString(), // VoyageNumber
//                 arrivalData.ShippingLineIdentification, // Location
//                 arrivalData.ShippingLineIdentification // TransportID
//             );

//             string coarriMessage = builder.BuildMessage();

//             // Imprimir el mensaje generado
//             Console.WriteLine(coarriMessage);

var unb = new UNB();
var unh = new UNH();
var bgm = new BGM(){DocumentName = "122"};
var tdt = new TDT();
var dtmETA = new DTM();
var dtmETD = new DTM();
var rff = new RFF();
var loc1 = new LOC(){BGM = bgm};
var loc2 = new LOC(){LocationCode = "CRCLIO", LocationQualifier = "94", BGM = bgm};
var loc3 = new LOC(){LocationCode = "", LocationQualifier = "94", BGM = bgm};
var loc4 = new LOC(){LocationCode = "", LocationQualifier = "", BGM = bgm};
var dtm1 = new DTM(){BGM = bgm};
var nad = new NAD(){PartyQualifier = "CA", PartyIdentifier = "CHIQUITA BRANDS", CodeListIdentification = "160", CodeListResponsibleAgency = "166", PartyName = "CHIQUITA BRANDS"};
var coarriMessage = unb.ToEDIString();
Console.WriteLine(coarriMessage);
var coarriMessageunh = unh.ToEDIString();
Console.WriteLine(coarriMessageunh);
// Console.WriteLine("");
var coarriMessagebgm = bgm.ToEDIString();
Console.WriteLine(coarriMessagebgm);
var coarriMessagetdt = tdt.ToEDIString();
Console.WriteLine(coarriMessagetdt);
var coarriMessagedtmETA = dtmETA.ReturnFormat(null, "2021-09-01");
Console.WriteLine(coarriMessagedtmETA);
var coarriMessagedtmETD = dtmETD.ReturnFormat(null, "2021-09-01");
Console.WriteLine(coarriMessagedtmETD);
var coarriMessagerff = rff.ToEDIString();
Console.WriteLine(coarriMessagerff);
var coarriMessageloc = loc1.ToEDIString();
Console.WriteLine(coarriMessageloc);
var coarriMessageloc2 = loc2.ToCustomEDI();
Console.WriteLine(coarriMessageloc2);
var coarriMessageloc3 = loc3.ToCustomEDI();
Console.WriteLine(coarriMessageloc3);
var coarriMessageloc4 = loc4.ToCustomEDI();
Console.WriteLine(coarriMessageloc4);
var coarriMessagedtm1 = dtm1.ToCustomEDI();
Console.WriteLine(coarriMessagedtm1);
var coarriMessagenad = nad.ToEDIString();
Console.WriteLine(coarriMessagenad);