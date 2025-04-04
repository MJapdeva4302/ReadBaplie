using System;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using Newtonsoft.Json;
using ReadEDIFACT;
using ReadEDIFACT.Models;
using ReadEDIFACT.Models.Coarri;


// string filePath = @"C:\Users\mbermudez\OneDrive - JAPDEVA\Proyecto PortLogistics\EDI BAPLEY\Ejemplos\BAPLIE\CRMOB DEP DMG V42.edi";

// string filePath = @"C:\Users\mbermudez\Documents\ReadBaplie\ReadEDIFACT\DEPARTURE BAPLIE FINAL - DEL MONTE ROSE - 25360187.edi";
// string outputFilePath = @"C:\Users\mbermudez\Documents\ReadBaplie\ReadEDIFACT\DEPARTURE BAPLIE FINAL - DEL MONTE ROSE - 25360187.json";

// Prueba de lectura de archivo BAPLIE para export y generación de archivo JSON y despues apartir del json volver a generar el archivo EDI
// string filePathBAPLIE = @"C:\Users\mbermudez\Documents\ReadBaplie\ReadEDIFACT\BAPLIE_Export.EDI";
// string outputFilePathBAPLIE = @"C:\Users\mbermudez\Documents\ReadBaplie\ReadEDIFACT\BAPLIE_VERSION2_Export_NEW.json";

// var baplieVersion2 = new BaplieVersion2();

// using (var reader = new StreamReader(filePathBAPLIE))
// {
//     try
//     {
//         var parser = new ParserEDI(reader, baplieVersion2);

//         // Validar el archivo EDI
//         var validationErrors = parser.ValidateFullEDI(baplieVersion2.Name);
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
//             // Console.WriteLine(jsonContent);

//             // Generar el archivo EDI a partir del JSON
//             string ediContent = parser.GenerateEDIFromJson(jsonContent);
//             File.WriteAllText(@"C:\Users\mbermudez\Documents\ReadBaplie\ReadEDIFACT/BAPLIE_VERSION2_EXPORT_NEW.edi", ediContent);
//             // Console.WriteLine($"EDI GENERADO:: {ediContent}");

//         }
//     }
//     catch (Exception ex)
//     {
//         Console.WriteLine($"Error al procesar el archivo EDI: {ex.Message}");
//     }
// }

// using (var sw = new StreamWriter(string.Format(@"C:\Users\mbermudez\Documents\ReadBaplie\ReadEDIFACT\{0}Output.txt", baplieVersion2.Name)))
// {
//     sw.Write(baplieVersion2);
// }


// Prueba de lectura de archivo BAPLIE para export y generación de archivo JSON y despues apartir del json volver a generar el archivo EDI
string filePathMOVINS = @"C:\Users\mbermudez\Documents\ReadBaplie\ReadEDIFACT\MOVINS DMR V09.edi";
string outputFilePathMOVINS = @"C:\Users\mbermudez\Documents\ReadBaplie\ReadEDIFACT\MOVINS DMR V09_NEW.json";

var movinsVersion2 = new MovinsVersion2();

using (var reader = new StreamReader(filePathMOVINS))
{
    try
    {
        var parser = new ParserEDI(reader, movinsVersion2);

        // Validar el archivo EDI
        var validationErrors = parser.ValidateFullEDI(movinsVersion2.Name);
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
            parser.SaveJsonToFile(outputFilePathMOVINS);
            Console.WriteLine("Archivo JSON guardado en: " + outputFilePathMOVINS);
            // Leer el contenido del archivo JSON
            string jsonContent = File.ReadAllText(outputFilePathMOVINS);
            Console.WriteLine("Contenido del JSON:");
            // Console.WriteLine(jsonContent);

            // Generar el archivo EDI a partir del JSON
            string ediContent = parser.GenerateEDIFromJson(jsonContent);
            File.WriteAllText(@"C:\Users\mbermudez\Documents\ReadBaplie\ReadEDIFACT/MOVINS_VERSION2_NEW.edi", ediContent);
            // Console.WriteLine($"EDI GENERADO:: {ediContent}");

        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error al procesar el archivo EDI: {ex.Message}");
    }
}

using (var sw = new StreamWriter(string.Format(@"C:\Users\mbermudez\Documents\ReadBaplie\ReadEDIFACT\{0}Output.txt", movinsVersion2.Name)))
{
    sw.Write(movinsVersion2);
}

// Probando los segmentos de COARRI
// var unb = new UNB();
// var unh = new UNH();
// var bgm = new BGM(){DocumentName = "122"};
// var tdt = new TDT();
// var dtmETA = new DTM();
// var dtmETD = new DTM();
// var rff = new RFF();
// var loc1 = new LOC(){BGM = bgm};
// var loc2 = new LOC(){LocationCode = "CRCLIO", LocationQualifier = "94", BGM = bgm};
// var loc3 = new LOC(){LocationCode = "", LocationQualifier = "94", BGM = bgm};
// var loc4 = new LOC(){LocationCode = "", LocationQualifier = "", BGM = bgm};
// var dtm1 = new DTM(){BGM = bgm};
// var nad = new NAD(){PartyQualifier = "CA", PartyIdentifier = "CHIQUITA BRANDS", CodeListIdentification = "160", CodeListResponsibleAgency = "166", PartyName = "CHIQUITA BRANDS"};

// var eqd = new EQD(){EquipmentQualifier = "CN", ContainerNumber = "BEAU5199464", EquipmentSizeAndType = "45G1", CodeListIdentification = "102", CodeListResponsibleAgency = "5", EquipmentStatusCode = "2", FullEmptyIndicator = "4"};
// var rff2 = new RFF(){ReferenceQualifier = "BN", ReferenceIdentifier = "HJSC1234740", EQD = eqd};
// var dtm203 = new DTM(){DateOrTimeQualifier = "203", DateOrTime = "20210901", DateOrTimeFormatQualifier = "203"};
// var loc147 = new LOC(){LocationQualifier = "147", LocationCode = ""};
// var loc9 = new LOC(){LocationQualifier = "9", LocationCode = ""};
// var loc11 = new LOC(){LocationQualifier = "11", LocationCode = ""};
// var mea = new MEA(){MeasurementQualifier = "AAE", MeasurementAttribute = "VGM", WeightUnitCode = "KGM", MeasurementValue = 10000.00};
// var tmp = new TMP(){TemperatureQualifier = "2", TemperatureValue = 10.0, TemperatureUnit = "CEL"};
// var sel = new SEL(){ SealNumber = "123456", SealType = "AA"};
// var dgs = new DGS(){DangerousGoodsCode = "IMD", HazardIdentificationCode = "3.1", DangerousGoodsClassificationCode = "9999"};
// var ftx = new FTX(){TextSubjectCode = "", TextValue = "", DGS = dgs};
// var nads = new NAD(){PartyQualifier = "CF", PartyIdentifier = "MAEU", CodeListIdentification = "160", CodeListResponsibleAgency = "166"};

// var cnt = new CNT(){ControlTotalQualifier = "16", ControlTotalValue = "1"};
// var unt = new UNT(){SegmentCount = 16.ToString("D6"), MessageRef = unh.MessageRefNumber};
// var unz = new UNZ(){InterchangeControlCount = "1", MessageRef = unb.InterchangeRef};

// var coarriMessage = unb.ToEDIString();
// Console.WriteLine(coarriMessage);
// var coarriMessageunh = unh.ToEDIString();
// Console.WriteLine(coarriMessageunh);
// // Console.WriteLine("");
// var coarriMessagebgm = bgm.ToEDIString();
// Console.WriteLine(coarriMessagebgm);
// var coarriMessagetdt = tdt.ToEDIString();
// Console.WriteLine(coarriMessagetdt);
// var coarriMessagedtmETA = dtmETA.ReturnFormat("132", "20210901");
// Console.WriteLine(coarriMessagedtmETA);
// var coarriMessagedtmETD = dtmETD.ReturnFormat("133", "20210901");
// Console.WriteLine(coarriMessagedtmETD);
// var coarriMessagerff = rff.ToEDIString();
// Console.WriteLine(coarriMessagerff);
// var coarriMessageloc = loc1.ToEDIString();
// Console.WriteLine(coarriMessageloc);
// var coarriMessageloc2 = loc2.ToCustomEDI();
// Console.WriteLine(coarriMessageloc2);
// var coarriMessageloc3 = loc3.ToCustomEDI();
// Console.WriteLine(coarriMessageloc3);
// var coarriMessageloc4 = loc4.ToCustomEDI();
// Console.WriteLine(coarriMessageloc4);
// var coarriMessagedtm1 = dtm1.ToCustomEDI();
// Console.WriteLine(coarriMessagedtm1);
// var coarriMessagenad = nad.ToEDIString();
// Console.WriteLine(coarriMessagenad);
// Console.WriteLine("");

// var coarriMessageeqd = eqd.ToEDIString();
// Console.WriteLine(coarriMessageeqd);
// var coarriMessagerff2 = rff2.ToCustomEDI();
// Console.WriteLine(coarriMessagerff2);
// var coarriMessagedtm203 = dtm203.DateOperation("20210901");
// Console.WriteLine(coarriMessagedtm203);
// var coarriMessageloc147 = loc147.Location("147", "0060382", "139", "5");
// Console.WriteLine(coarriMessageloc147);
// var coarriMessageloc9 = loc9.Location("9", "COCTG", "", "");
// Console.WriteLine(coarriMessageloc9);
// var coarriMessageloc11 = loc11.Location("11", "COCTG", "", "");
// Console.WriteLine(coarriMessageloc11);
// var coarriMessagemea = mea.ToEDIString();
// Console.WriteLine(coarriMessagemea);
// var coarriMessagetmp = tmp.ToEDIString();
// Console.WriteLine(coarriMessagetmp);
// var coarriMessagesel = sel.ToEDIString();
// Console.WriteLine(coarriMessagesel);
// var coarriMessagedgs = dgs.ToCustomEDI();
// Console.WriteLine(coarriMessagedgs);
// var coarriMessageftx = ftx.ToEDIString();
// Console.WriteLine(coarriMessageftx);
// var coarriMessagenads = nads.ToCustomEDI();
// Console.WriteLine(coarriMessagenads);
// var coarriMessagecnt = cnt.ToEDIString();
// Console.WriteLine(coarriMessagecnt);
// var coarriMessageunt = unt.ToEDIString();
// Console.WriteLine(coarriMessageunt);
// var coarriMessageunz = unz.ToEDIString();
// Console.WriteLine(coarriMessageunz);


// Leer un json y convertirlo a EDI COARRI de carga y descarga
// string filePath = @"C:\Users\mbermudez\Documents\ReadBaplie\ReadEDIFACT/Json COARRI Export Ejemplo.JSON";
string filePath = @"C:\Users\mbermudez\Documents\ReadBaplie\ReadEDIFACT/Json COARRI Import Ejemplo.json";

try
{
    // 2. Cargar y parsear el JSON
    var rootData = ParserCOARRI.LoadJson(filePath);

    // 3. Mapear los datos a las estructuras EDI
    if (rootData == null)
    {
        throw new ArgumentNullException(nameof(rootData), "RootData cannot be null.");
    }
    // 2. Validar CAMPOS DEL JSON (no segmentos EDI)
    ParserCOARRI.ValidateJsonBeforeMapping(rootData);

    var arrivalData = ParserCOARRI.MapArrivalDataFromJson(rootData.ArrivalData);

    var equipments = ParserCOARRI.MapEquipmentFromJson(rootData.Equipments);

    var builder = new ParserCOARRI(arrivalData, equipments);
    var (isValid, validationErrors) = builder.ValidateMandatoryFields();

    if (!isValid)
    {
        Console.WriteLine("=== Errores de Validación ===");
        validationErrors.ForEach(e => Console.WriteLine($"- {e}"));
        return;
    }
    string ediMessage = builder.BuildMessage();
    ediMessage = ediMessage.Replace("\r\n", "").Replace("\n", "").Replace("\r", "");

    // 5. Mostrar resultados
    Console.WriteLine("=== Mensaje EDI Generado ===");
    // Console.WriteLine(ediMessage);

    File.WriteAllText(@"C:\Users\mbermudez\Documents\ReadBaplie\ReadEDIFACT/COARRI_NEW.edi", ediMessage);
    // File.WriteAllText(@"C:\Users\mbermudez\Documents\ReadBaplie\ReadEDIFACT/outputExportSinDGS.edi", ediMessage);
    Console.WriteLine("\nMensaje guardado");

    // var dataToSave = new {
    //     ediMessage,
    // };
    // Serializar a JSON
    string jsonOutput = JsonConvert.SerializeObject(ediMessage, Formatting.None);
    jsonOutput = jsonOutput.Replace("\\r\\n", "");
    // Console.WriteLine(jsonOutput);

    string jsonFilePath = @"C:\Users\mbermudez\Documents\ReadBaplie\ReadEDIFACT\COARRI_NEW.json";
    // string jsonFilePath = @"C:\Users\mbermudez\Documents\ReadBaplie\ReadEDIFACT\outputExportSinDGS.json";
    File.WriteAllText(jsonFilePath, jsonOutput);
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
    if (ex.InnerException != null)
    {
        Console.WriteLine($"Detalles: {ex.InnerException.Message}");
    }
}
