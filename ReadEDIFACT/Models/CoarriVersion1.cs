using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadEDIFACT.Models
{
    public class CoarriVersion1 : FileDefinition
    {
        public CoarriVersion1() : base("COARRI", new Version(1, 0, 0))
        {
            SegmentSeparator = '\'';
            ElementSeparator = '+';
            DataElementSeparator = ':';
            EscapeCharacter = '?';

            Segments = new Segment[]
            {
                //SON LOS SEGMENTOS QUE COMPONEN EL ARCHIVO COMO POR EJEMPLO: UNB+UNOA:1+SEACOS+PUBLIC+241013:1441+31039624+++++UNKNOWN' LISTO
                new SegmentData()
                {
                    SegmentID = "UNB",
                    Name = "Interchange Header",
                    Usage = RuleUsage.Mandatory,

                    DataElements = new Element[]
                    {
                        //DATOS COMPUESTOS EJEMPLO: UNOA:1
                        new CompositeElement()
                        {
                            Name = "Sintax",
                            DataElements = new DataElement[]
                            {
                                new DataElement()
                                {
                                    Name = "Sintax Identifier",
                                    Notes = @"Always ""UNOA"", indicating the user of level ""A"" character set.",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphabetic,
                                    Precision = 4,
                                    JsonProperty = "ArrivalData.ArrivalNumber"
                                },

                                new DataElement()
                                {
                                    Name = "Sintax Version Number",
                                    Notes = @"Always ""1"".",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Numeric,
                                    Precision = 1
                                }
                            }
                        },

                        // DATO DE UN SEGMENTO EJEMPLO: SEACOS NOTA: NO ES UN ELEMENTO COMPUESTO YA QUE LOS ELEMENTOS COMPUESTO SON LOS QUE ESTAN SEPARADOS POR ":"
                        new DataElement()
                        {
                            Name = "Sender Identification",
                            Description = @"Name code of the recipient of the interchange (message). To be agreed between partners.",
                            Usage = RuleUsage.Mandatory,
                            DataType = DataType.Alphanumeric,
                            Precision = new[] { 1, 35 }
                        },

                        new DataElement()
                        {
                            Name = "Recipient Identification",
                            Description = @"Name code of the recipient of the interchange (message). To be agreed between partners.",
                            Usage = RuleUsage.Mandatory,
                            DataType = DataType.Alphanumeric,
                            Precision = new[] { 1, 35 }
                        },

                        // FECHA COMPUESTA EJEMPLO: 241013:1441
                        new CompositeElement()
                        {
                            Name = "Datetime Preparation",
                            DataElements = new DataElement[]
                            {
                                new DataElement()
                                {
                                    Name = "Date of preparation",
                                    Description = "Preparation date of the interchange (message)",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Numeric,
                                    Precision = 6,
                                    JsonProperty = "FileDefinition.Date"
                                },
                                new DataElement()
                                {
                                    Name = "Time of preparation",
                                    Description = "Preparation date of the interchange (message)",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Numeric,
                                    Precision = 4
                                }
                            }
                        },

                        new DataElement()
                        {
                            Name = "Interchange control reference",
                            Description = "A reference allocated by the sender, uniquely identifying an interchange.",
                            Notes = "This reference must also be transmitted in the interchange Trailer segment UNZ.",
                            Usage = RuleUsage.Mandatory,
                            DataType = DataType.Alphanumeric,
                            Precision = new[] { 1, 14 }
                        },

                    }
                }, // ACA FINALIZA EL SEGMENTO UNB

                // UNH LISTO
                new SegmentData()
                {
                    SegmentID = "UNH",
                    Name = "Message Header",
                    Usage = RuleUsage.Mandatory,
                    DataElements = new Element[]
                    {
                        new DataElement()
                        {
                            Name = "Message Reference Number",
                            Description = "A reference allocated by the sender, uniquely identifying a message. This reference must also be transmitted in the Message Trailer segment UNT.",
                            Usage = RuleUsage.Mandatory,
                            DataType = DataType.Alphanumeric,
                            Precision = new[] { 1, 14 }
                        },

                        new CompositeElement()
                        {
                            Name = "Message Identifier",
                            DataElements = new DataElement[]
                            {
                                new DataElement()
                                {
                                    Name = "Message Type Identifier",
                                    Description = @"The name of the UNSM or standard EDIFACT message. In this case always ""COARRI""",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new[] { 1, 6 }
                                },
                                new DataElement()
                                {
                                    Name = "Message Type Version Number",
                                    Description = @"The version number of the message. See EDIFACT documentation. At this moment the version is ""D""",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new[] { 1, 3 }
                                },

                                new DataElement()
                                {
                                    Name = "Message Release Number",
                                    Description = @"The release number of the message. See EDIFACT documentation. At this moment the release number is ""23A""",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new[] { 1, 3 }
                                },

                                new DataElement()
                                {
                                    Name = "Controlling Agency",
                                    Description = "A unique reference allocated by the sender to identify the message.",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new[] { 1, 2 }
                                },

                                new DataElement()
                                {
                                    Name = "Association Assigned Code",
                                    Description = @"The applicable SMDG User Manual version number. For this manual use: ""ITG10"". This will enable the recipient of the message to translate the message correctly, even if older versions are still in use.",
                                    Usage = RuleUsage.Required,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new[] { 1, 6 }
                                },

                            }
                        },


                    }
                },// ACA FINALIZA EL SEGMENTO UNH

                // BGM LISTO
                new SegmentData()
                {
                    SegmentID = "BGM",
                    Name = "Beginning of Message",
                    Usage = RuleUsage.Mandatory,
                    DataElements = new Element[]
                    {

                        new DataElement()
                        {
                            Name = "Message Name",
                            Description = "Reference allocated by the sender individually, taken from the application.",
                            Usage = RuleUsage.Required,
                            DataType = DataType.Alphanumeric,
                            Precision = new[] { 1, 3 }
                        },
                        new DataElement()
                        {
                            Name = "Document identifier",
                            Description = "Unique reference to identify a message, e.g. returned error messages refer to this value/reference.",
                            Usage = RuleUsage.Required,
                            DataType = DataType.Alphanumeric,
                            Precision = new[] { 1, 35 }
                        },

                        new DataElement()
                        {
                            Name = "Message Function",
                            Description = @"Code indicating the function of the message. Acceptable codes are: ""2"" = Add. Add to previous message. ""3"" = Delete. Delete from previous message. ""4"" = Change. Message with changes on previous message. ""5"" = Replace. Message replacing a previous one. ""9"" = Original. First or basic message. ""22"" = Final. The final message in a series of BAPLIE messages.",
                            Usage = RuleUsage.Required,
                            DataType = DataType.Alphanumeric,
                            Precision = new[] { 1, 3 }
                        }

                    }
                },// ACA FINALIZA EL SEGMENTO BGM 

                // PENDIENTE YA QUE EN LA DOCUMENTACION NO ESTA ESTE SEGMENTO
                // new SegmentData()
                // {
                //     SegmentID = "DTM",
                //     Name = "Date - Time - Period",
                //     Usage = RuleUsage.Mandatory,
                //     DataElements = new Element[]
                //     {
                //         new CompositeElement()
                //         {
                //             Name = "Date Time",
                //             DataElements = new DataElement[]
                //             {

                //                 new DataElement()
                //                 {
                //                     Name = "Date/Time/Period Qualifier",
                //                     Description = @"Code ""137"" (Document/Message Date/Time)",
                //                     Usage = RuleUsage.Mandatory,
                //                     DataType = DataType.Alphanumeric,
                //                     Precision = new[] { 1, 3 }
                //                 },

                //                 new DataElement()
                //                 {
                //                     Name = "Date/Time/Period",
                //                     Description = "Date or date/time of compiling the message",
                //                     Usage = RuleUsage.Required,
                //                     DataType = DataType.Alphanumeric,
                //                     Precision = new[] { 1, 35 }
                //                 },

                //                 new DataElement()
                //                 {
                //                     Name = "Date/Time/Period Format Qualifier",
                //                     Description = @"Allowed qualifiers: ""101"" = YYMMDD ""201"" = YYMMDDHHMM""301"" = YYMMDDHHMMZZZ(""ZZZ"" = Time zone, e.g. ""GMT"" or other)",
                //                     Usage = RuleUsage.Required,
                //                     DataType = DataType.Alphanumeric,
                //                     Precision = new[] { 1, 3 }
                //                 }

                //             }
                //         }
                //     }
                // },// ACA FINALIZA EL SEGMENTO DTM

                // GRUPO 2
                new SegmentGroup(){
                    GroupRepeat = 9,
                    Segments = new Segment[]
                    {
                        // TDT LISTO
                        new SegmentData()
                        {
                            SegmentID = "TDT",
                            Name = "DETAILS OF TRANSPORT",
                            Notes = "1",
                            Usage = RuleUsage.Mandatory,
                            DataElements = new Element[]
                            {
                                new DataElement()
                                {
                                    Name = "Transport Stage Qualifier",
                                    Description = @"Code ""20"" (Main Carriage)",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new[] { 1, 3 }
                                },

                                new DataElement()
                                {
                                    Name = "Conveyance Reference Number",
                                    Description = "Discharge voyage number as assigned by the Operating Carrier or his agent. The trade route could be included in this voyage number, if required.",
                                    Usage = RuleUsage.Required,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new[] { 1, 17 }
                                },

                                new DataElement()
                                {
                                    Name = "Transport mode name code",
                                    Description = "1 Maritime transport",
                                    Usage = RuleUsage.Conditional,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new[] { 1, 3 }
                                },

                                new EmptyElement(),

                                new CompositeElement()
                                {
                                    Name = "Code List",
                                    Description = "Carrier name, coded. Codes to be agreed or standard carrier alpha code (SCAC).",

                                    DataElements = new DataElement[]
                                    {
                                        // CHQ
                                        new DataElement()
                                        {
                                            Name = "Carrier identifier",
                                            Description = "Carrier name, coded. Codes to be agreed or standard carrier alpha code (SCAC).",
                                            Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 17 }
                                        },

                                        // ZZZ
                                        new DataElement()
                                        {
                                            Name = "Code list responsible agency, coded. Allowed codes",
                                            Description = @"""20"" = BIC (Bureau International des Containeurs) ""166"" = US National Motor Freight Classification Association (SCAC) ""ZZZ"" = Mutually defined.",
                                            Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 3 }
                                        },

                                        // 172
                                        new DataElement()
                                        {
                                            Name = "Code List Qualifier",
                                            Description = @"Code ""172"" (Carrier Code)",
                                            Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 3 }
                                        },

                                        // 166
                                        new DataElement()
                                        {
                                            Name = "Code list responsible agency, coded. Allowed codes",
                                            Description = @"""20"" = BIC (Bureau International des Containeurs) ""166"" = US National Motor Freight Classification Association (SCAC) ""ZZZ"" = Mutually defined.",
                                            Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 3 }
                                        },

                                        // CHIQUITA
                                        new DataElement()
                                        {
                                            Name = "Carrier Name",
                                            Description = "Name of a carrier.",
                                            Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 17 }
                                        },
                                    }

                                },

                                new EmptyElement(),
                                new EmptyElement(),

                                new CompositeElement()
                                {
                                    Name = "Id of Means of Transport Identification. Vessel code",
                                    Usage = RuleUsage.Required,
                                    DataElements = new DataElement[]
                                    {
                                        new DataElement()
                                        {
                                            Name = "Id of Means of Transport Identification. Vessel code:",
                                            Description = @"1. Lloyd’s Code (IMO number) 2. Call Sign 3. Mutually agreed vessel code.",
                                            Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 9 }
                                        },

                                        new DataElement()
                                        {
                                            Name = "Code List Qualifier: Allowed qualifiers:",
                                            Description = @"""103"" = Call Sign Directory ""146"" = Means of Transport Identification(Lloyd's Code or IMO number) ""ZZZ"" = Mutually defined or IMO number",
                                            Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 3 }
                                        },

                                        new DataElement()
                                        {
                                            Name = "Code list responsible agency, coded. Allowed code:",
                                            Description = @"""11"" = Lloyd's register of shipping. Only to be used whenLloyd's Code is used for vessel/barge identification (Code ""146"" in c222.e1131). ""ZZZ"" = Mutually defined. To be used in all other cases.",
                                            Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 3 }
                                        },

                                        new DataElement()
                                        {
                                            Name = "Name of Means of Transport Identification. Vessel name",
                                            Description = "Name of the vessel.",
                                            Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 35 }
                                        }
                                    }

                                },
                            }
                        },// ACA FINALIZA EL SEGMENTO TDT DEL GRUPO 2

                        // RFF LISTO
                        new SegmentData(){

                            SegmentID = "RFF",
                            Name = "REFERENCE (grp2)",
                            Notes = "2",
                            DataElements = new Element[]
                            {
                                new CompositeElement(){
                                    Name = "Reference Qualifier: Allowed qualifiers: (grp2)",
                                    DataElements = new Element[]{
                                // VM
                                new DataElement()
                                {
                                    Name = "Reference Qualifier: Allowed qualifiers:",
                                    Description = @"""BM"" = B/L-number. ""BN"" = Booking reference number.""ET"" = Excess Transportation Number to be used for leading Stowage position, in case of Break-bulk or odd-sized-cargo. ""ZZZ"" = Mutually defined.",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 3}
                                },

                                // IDENTIFICATIVO DE LLAMADA (CALL SIGN)
                                new DataElement()
                                {
                                    Name = @"Reference Number: For Qualifiers ""BM"", ""BN"" or ""ZZZ"":",
                                    Description = @"Dummy value ""1"" or the actual Bill of Lading number resp. Booking Reference number, as agreed. For Qualifier ""ET"": leading stowage location, containing relevant data for this consignment.",
                                    Usage = RuleUsage.Required,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 70 }
                                }
                            }
                            }
                            }
                        }, // ACA FINALIZA EL SEGMENTO RFF DEL GRUPO 2

                    }, // ACA FINALIZA LOS SEGMENTOS GRUPO 2
                    
                }, //ACA FINALIZA EL GRUPO 2

                // GRUPO 3
                 new SegmentGroup(){
                        GroupRepeat = 99,
                        Segments = new Segment[]
                        {
                            // PRIMER LOC ME DICE SI ES CARGA O DESCARGA
                            /*
                                Cuando el mensaje COARRI informa de la salida de un buque (valor 122 en el segmento BGM), el valor debe ser 42 a 
                                puerto/localización de Costa Rica. Si se informa de una operación de descarga (valor 119 en el segmento BGM), el 
                                valor debe ser 41.
                                Ejemplo: 
                                BGM+119+APM2351463+9' 
                                LOC+41+0002' (aduanas de entrada en un reporte de descarga)
                                BGM+122+APM2351463+9' 
                                LOC+42+0002' (aduanas de salida en un reporte de carga)
                            */
                            // LISTO
                            new SegmentData(){
                            SegmentID = "LOC",
                            Name = "PLACE/LOCATION IDENTIFICATION (grp3)",
                            Notes = "3",
                            Position = 1,
                            DataElements = new Element[]
                            {
                                new DataElement()
                                {
                                    Name = "Place/Location Qualifier",
                                    Description = @"Code identifying the function of a location.",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new[] { 1, 3 }
                                },

                                new DataElement()
                                {
                                    Name = "Location name code",
                                    Description = @"The actual location of the equipment or cargo on the vessel. The following formats are allowed:1. ISO-format 2. Ro/Ro-format3. Other non-ISO-format (to be agreed between partners)1. ISO-format: Bay/Row/Tier (BBBRRTT). If Bay number is less than 3 characters it must be filled with leading zeroes, e.g. ""0340210"".2. Ro/Ro-format:Deck/Bay/Row/Tier (DDBBBRRTT).",
                                    Usage = RuleUsage.Required,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new[] { 1, 25 }
                                }
                            }
                        }, // ACA FINALIZA EL SEGMENTO LOC (grp3)

                        // SEGUNDO LOC
                        /*
                            Cuando el mensaje COARRI informa de la salida de un buque (valor 122 en el segmento BGM), el valor debe ser un 
                            LOCODE para un puerto/localización de Costa Rica. El identificador de ubicación (campo 3223 en el elemento C519) 
                            debe incluir el código OMI-GISIS de cuatro dígitos para un puerto costarricense. 
                            Ejemplo: 
                            LOC+9+ECGYE:ZZZ:98:GUAYAQUIL' (discharge report)
                            LOC+9+CRMOB:ZZZ:98: MOIN TERMINAL DE CONTENEDORES+0002' (Loading report)
                        */
                        // LISTO
                        new SegmentData(){
                            SegmentID = "LOC",
                            Name = "PLACE/LOCATION IDENTIFICATION (grp3)",
                            Notes = "3",
                            Position = 1,
                            DataElements = new Element[]
                            {
                                // 9
                                new DataElement()
                                {
                                    Name = "Place/Location Qualifier",
                                    Description = @"Code identifying the function of a location.",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new[] { 1, 3 }
                                },

                                new CompositeElement()
                                {
                                    Name = "Place/Location Identification:",
                                    DataElements = new DataElement[]
                                    {
                                        new DataElement()
                                        {
                                            Name = "Location name code",
                                            Description = @"Código LOCODE del Puerto de salida",
                                            Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 25 }
                                        },

                                        new DataElement()
                                        {
                                            Name = "Code list Identification",
                                            Description = @"ZZZ = Definido por mutuo acuerdo",
                                            Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 25 }
                                        },

                                        new DataElement()
                                        {
                                            Name = "Code list responsible agency, coded. Allowed codes",
                                            Description = @"98 = Aduanas de Costa Rica",
                                            Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 3 }
                                        },

                                        new DataElement()
                                        {
                                            Name = "Location name",
                                            Description = @"Nombre del Puerto de salida",
                                            Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 3 }
                                        }
                                    }
                                },

                                // crmob= 0002 CRCLIO = 0001
                                new DataElement()
                                        {
                                            Name = " First related location name code",
                                            Description = @"Si el Puerto es costarricense, se debe incluir el Código de 4 dígitos IMO-GISIS. ",
                                            Usage = RuleUsage.Dependent,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 4 }
                                        }
                            }
                        }, // ACA FINALIZA EL SEGMENTO LOC (grp3)

                        // TERCER LOC
                        /*Ejemplos: 
                          LOC+94+KRKAN:ZZZ:98:GWAYNGYANG'
                        */
                        // LISTO
                        new SegmentData(){
                            SegmentID = "LOC",
                            Name = "PLACE/LOCATION IDENTIFICATION (grp3)",
                            Notes = "3",
                            Position = 1,
                            DataElements = new Element[]
                            {
                                new DataElement()
                                {
                                    Name = "Place/Location Qualifier",
                                    Description = @"Code identifying the function of a location.",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new[] { 1, 3 }
                                },

                                new CompositeElement()
                                {
                                    Name = "Place/Location Identification:",
                                    DataElements = new DataElement[]
                                    {
                                        new DataElement()
                                        {
                                            Name = "Location name code",
                                            Description = @"The actual location of the equipment or cargo on the vessel. The following formats are allowed:1. ISO-format 2. Ro/Ro-format3. Other non-ISO-format (to be agreed between partners)1. ISO-format: Bay/Row/Tier (BBBRRTT). If Bay number is less than 3 characters it must be filled with leading zeroes, e.g. ""0340210"".2. Ro/Ro-format:Deck/Bay/Row/Tier (DDBBBRRTT).",
                                            Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 25 }
                                        },
                                        // ZZZ
                                        new DataElement()
                                        {
                                            Name = "Code list Identification",
                                            Description = @"The actual location of the equipment or cargo on the vessel. The following formats are allowed:1. ISO-format 2. Ro/Ro-format3. Other non-ISO-format (to be agreed between partners)1. ISO-format: Bay/Row/Tier (BBBRRTT). If Bay number is less than 3 characters it must be filled with leading zeroes, e.g. ""0340210"".2. Ro/Ro-format:Deck/Bay/Row/Tier (DDBBBRRTT).",
                                            Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 25 }
                                        },

                                        // 98
                                        new DataElement()
                                        {
                                            Name = "Code list responsible agency, coded. Allowed codes",
                                            Description = @"To indicate which format is used. Valid codes are:""5"" (ISO-format)""87"" (Ro/Ro-format, assigned by the Carrier""ZZZ"" (non-ISO-format, mutually defined).",
                                            Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 3 }
                                        },

                                        new DataElement()
                                        {
                                            Name = "Location name",
                                            Description = @"Nombre del puerto",
                                            Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 3 }
                                        }
                                    }
                                }
                            }
                        }, // ACA FINALIZA EL SEGMENTO LOC (grp3)

                        // CUARTO LOC ESTE LOC SE INCLUYE SIEMPRE Y CUANDO EL SEGMENTO BGM EN 119
                        /*
                        Los valores esperados en el campo 3225 deben coincidir siempre con Puertos de Costa Rica cuando 
                        el propósito del mensaje sea informar de una operación de descarga (valor 119 en el segmento 
                        BGM). El campo 3223 en el elemento C519 es obligatorio y debe ser el código IMO-GISIS de 
                        cuatro dígitos de la lista utilizada por la Autoridad Aduanera.
                        */
                        // Ejemplo: BGM+119++9' LOC+12+CRMOB:ZZZ:98:MOIN TERMINAL DE CONTENEDORES+0002'
                        // LISTO
                        new SegmentData(){
                            SegmentID = "LOC",
                            Name = "PLACE/LOCATION IDENTIFICATION (grp3)",
                            Notes = "3",
                            Position = 1,
                            DataElements = new Element[]
                            {
                                new DataElement()
                                {
                                    Name = "Location Qualifier",
                                    Description = @"11 Puerto de descarga",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new[] { 1, 3 }
                                },

                                new CompositeElement()
                                {
                                    Name = "Place/Location Identification:",
                                    DataElements = new DataElement[]
                                    {
                                        new DataElement()
                                        {
                                            Name = "Location name code",
                                            Description = @"Código LOCODE del Puerto",
                                            Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 25 }
                                        },

                                        new DataElement()
                                        {
                                            Name = "Code list Identification",
                                            Description = @"ZZZ = Por acuerdo mutuo",
                                            Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 17 }
                                        },

                                        new DataElement()
                                        {
                                            Name = "Code list responsible agency",
                                            Description = @"98 = Aduanas de Costa Rica",
                                            Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 3 }
                                        },

                                        new DataElement()
                                        {
                                            Name = "Location name",
                                            Description = @"Nombre del puerto",
                                            Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 256 }
                                        }
                                    }
                                },
                                new DataElement()
                                {
                                    Name = " First related location name code",
                                    Description = @"Si el Puerto es de Costa Rica, se deberá incluir el identificador del estándar OMI GISISthe IMO-GISIS standard.",
                                    Usage = RuleUsage.Required,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new[] { 1, 25 }
                                }
                            }
                        }, // ACA FINALIZA EL SEGMENTO LOC (grp3)

                        /*
                        QUINTO LOC
                            El código identificador en el campo 3223 en el elemento C519 sólo se espera cuando el valor en el 
                            campo 3225 es un LOCODE para un puerto de Costa Rica. El Valor esperado en el campo 3225 no 
                            puede ser igual al valor en Puerto de descarga (LOC+11).
                            En las operaciones, este campo debe contener información sobre el siguiente puerto de escala dentro 
                            del itinerario de un buque. Por ejemplo, si un buque atraca en Caldera y su siguiente escala es 
                            Cartagena, se esperan datos de Cartagena (LOCODE y nombre). 
                            Ejemplos: 
                            LOC+61+CRCAL:ZZZ:98:CALDERA PORT+0001'
                            LOC+61+COCTG:ZZZ:98:CARTAGENA'
                        */
                        // LISTO
                        new SegmentData(){
                            SegmentID = "LOC",
                            Name = "PLACE/LOCATION IDENTIFICATION (grp3)",
                            Notes = "3",
                            Position = 1,
                            DataElements = new Element[]
                            {
                                new DataElement()
                                {
                                    Name = "Location Qualifier",
                                    Description = @"61 Próximo Puerto de llamada",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new[] { 1, 3 }
                                },

                                new CompositeElement()
                                {
                                    Name = "Place/Location Identification:",
                                    DataElements = new DataElement[]
                                    {
                                        new DataElement()
                                        {
                                            Name = "Location name code",
                                            Description = @"Código LOCODE del Puerto",
                                            Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 25 }
                                        },

                                        new DataElement()
                                        {
                                            Name = "Code list Identification",
                                            Description = @"ZZZ = Por acuerdo mutuo",
                                            Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 17 }
                                        },

                                        new DataElement()
                                        {
                                            Name = "Code list responsible agency",
                                            Description = @"98 = Aduanas de Costa Rica",
                                            Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 3 }
                                        },

                                        new DataElement()
                                        {
                                            Name = "Location name",
                                            Description = @"Nombre del puerto",
                                            Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 256 }
                                        }
                                    }
                                },
                                new DataElement()
                                {
                                    Name = " First related location name code",
                                    Description = @"Si es un Puerto de Costa Rica, se deberá incluir el identificador OMI-GISIS de 4 dígitos",
                                    Usage = RuleUsage.Required,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new[] { 1, 25 }
                                }
                            }
                        }, // ACA FINALIZA EL SEGMENTO LOC (grp3)

                        /*
                            El valor 178 (para ATA) en el campo 2005 sólo se debe utilizar para reports de descarga mientras 
                            que el valor 186 sólo aplica para reports de carga. 
                            Ejemplo
                            BGM+119++9'
                            DTM+178:20140125:102'
                        */
                        // LISTO
                        new SegmentData(){

                            SegmentID = "DTM",
                            Name = "DATE/TIME/PERIOD (grp3)",
                            Notes = "3",
                            Usage = RuleUsage.Mandatory,
                            Position = 99,
                            DataElements = new Element[]
                            {

                                new CompositeElement()
                                {
                                    Name = "Place/Location Identification:",
                                    DataElements = new DataElement[]
                                    {
                                        new DataElement()
                                        {
                                            Name = "Date/Time/Period Qualifier: Allowed qualifiers",
                                            Description = @"178 Fecha efectiva de arribo (ATA) 186 Fecha efectiva de salida (ATD)",
                                            Usage = RuleUsage.Mandatory,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 3 }
                                        },

                                        new DataElement()
                                        {
                                            Name = "Date/Time/Period",
                                            Description = @"ATA o ATD",
                                            Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 35 }
                                        },

                                        new DataElement()
                                        {
                                            Name = "Date/Time/Period Format Qualifier. Allowed qualifiers",
                                            Description = @"102 CCYYMMDD",
                                            Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 3 }
                                        }
                                    }
                                }
                            }
                        }, // ACA FINALIZA EL SEGMENTO DTM (grp3)

                        }
                    }, // ACA FINALIZA EL GRUPO 3

                    // GRUPO 4
                    new SegmentGroup(){
                        GroupRepeat = 9999,
                        Segments = new Segment[]
                        {
                            // LISTO
                            new SegmentData(){
                            SegmentID = "NAD",
                            Name = "NAME AND ADDRESS (grp4)",
                            Notes = "4",
                            DataElements = new Element[]
                            {
                                new DataElement()
                                {
                                    Name = @"Party Qualifier",
                                    Description = @"Allowed code: ""CA"" (Carrier of the cargo).",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 3 }
                                },

                                new CompositeElement(){
                                    Name = "Party Id Identification (grp4)",
                                    DataElements = new Element[]{
                                new DataElement()
                                {
                                    Name = "Party Id Identification",
                                    Description = @"Name code of party responsible for the carriage of the goods and/or equipment.",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 35}
                                },

                                new DataElement()
                                {
                                    Name = @"Code List Qualifier",
                                    Description = @"Qualifier ""172"" (Carrier Code)",
                                    Usage = RuleUsage.Required,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 3 }
                                },

                                new DataElement()
                                {
                                    Name = @"Code List Responsible Agency, coded. Allowed codes",
                                    Description = @"""20"" = BIC (Bureau International des Containeurs) ""166"" = US National Motor Freight Classification Association (SCAC) ""ZZZ"" = Mutually agreed",
                                    Usage = RuleUsage.Required,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 3 }
                                }
                            }
                            },
                            new EmptyElement(),
                            new DataElement()
                                {
                                    Name = @"Party name",
                                    Description = @"Nombre",
                                    Usage = RuleUsage.Required,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 35 }
                                }
                            }
                        }, // ACA FINALIZA EL SEGMENTO NAD DEL GRUPO 4

                        }
                    }, // ACA FINALIZA EL GRUPO 4

                    // GRUPO 6
                    new SegmentGroup(){
                        GroupRepeat = 99,
                        Segments = new Segment[]
                        {
                            // LISTO
                            new SegmentData(){
                                SegmentID = "EQD",
                                Name = "EQUIPMENT DETAILS (grp6)",
                                Notes = "3",
                                DataElements = new Element[]
                                {
                                    // CN
                                   new DataElement()
                                {
                                    Name = "Equipment Qualifier",
                                    Description = @" ""CN"" = Container ""BB"" = Break-bulk ""TE"" = Trailer ""ZZZ"" = Ro/Ro or otherwise",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 3}
                                },

                                // BEAU5199464
                                new DataElement()
                                {
                                    Name = @"Equipment Identification Number",
                                    Description = @"1. The container number: Format: One continuous string with the identification, prefixand number. Examples: SCXU 2387653 must be transmitted as ""SCXU2387653"", EU 876 must be transmitted as ""EU876"". The number will be treated as a character string. E.g. alphanumeric check-digits can be transmitted here. If this segment is used the unique equipment identification number must always be transmitted, although this element is not mandatory! 2. Break-bulk: The break-bulk reference number. The assigned break-bulk reference numbers must be agreed between partners. 3. Otherwise (Ro/Ro): The equipment identification number.",
                                    Usage = RuleUsage.Required,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 17 }
                                },


                                new CompositeElement(){
                                    Name = "Equipment size and type (grp6)",
                                    DataElements = new Element[]{
                                    // 45R1
                                new DataElement()
                                {
                                    Name = "Equipment size and type",
                                    Description = @" Tamaño de contenedor (Código ISO). Obligatorio siempre y cuando el valor en el campo 8053 de este segmento sea CN",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new [] {1, 10}
                                },

                                // 102
                                new DataElement()
                                {
                                    Name = "Code list identification code",
                                    Description = @"Description of hazard material in plain language. One element of maximum 70 characters to be given only for the description. Transmit the text ""NIL"", if no description is available and one or both of the following data elements must be transmitted.",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new [] {1, 17}
                                },
                                
                                // 5
                                 new DataElement()
                                {
                                    Name = @"Code list responsible agency code",
                                    Description = @"ISO size-type code of 4 digits (ISO 6346). Leave blank in case of break-bulk. For unknown ISO size/type codes the following codes can beagreed between partners ""9999"" = No information at all. ""4999"" = Length = 40ft, rest unknown""2999"" = Length = 20ft, rest unknown""4299"" = 40ft ""8'6"", rest unknown""2299"" = 20ft ""8'6"", rest unknown""4099"" = 40ft ""8'0"", rest unknown ""2099"" = 20ft ""8'0"", rest unknownOther codes to be agreed between partners.",
                                    Usage = RuleUsage.Conditional,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 4 }
                                },

                                },
                                },

                                new EmptyElement(),
                                // 2
                                new DataElement()
                                {
                                    Name = @"Equipment status code",
                                    Description = @"Equipment status code: 2 = Export, 3 = Import",
                                    Usage = RuleUsage.Conditional,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 3 }
                                },

                                new DataElement()
                                {
                                    Name = @"Full/Empty Indicator, coded. Allowed codes",
                                    Description = @"""5"" = Full ""4"" = Empty. Leave blank in case of break-bulk.",
                                    Usage = RuleUsage.Dependent,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 3 }
                                },

                                }
                            }, // ACA FINALIZA EL SEGMENTO EQD DEL GRUPO 6

                            // PRIMERA REFERENCIA LISTO
                            new SegmentData(){
                            SegmentID = "RFF",
                            Name = "REFERENCE (grp6)",
                            Notes = "6",
                            DataElements = new Element[]
                            {
                                new CompositeElement(){
                                    Name = "Reference Qualifier: Allowed qualifiers: (grp6)",
                                    DataElements = new Element[]{
                                // BM
                                new DataElement()
                                {
                                    Name = "Reference Qualifier",
                                    Description = @"BM Guía de transporte",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 3}
                                },

                                // HJSC1234740
                                new DataElement()
                                {
                                    Name = @"Reference Number",
                                    Description = @"Número de Guía de transporte",
                                    Usage = RuleUsage.Required,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 70 }
                                }
                            }
                            }
                            }
                        }, // ACA FINALIZA EL SEGMENTO RFF DEL GRUPO 6

                        // SEGUNDA REFERENCIA LISTO
                        new SegmentData(){
                            SegmentID = "RFF",
                            Name = "REFERENCE (grp6)",
                            Notes = "6",
                            DataElements = new Element[]
                            {
                                new CompositeElement(){
                                    Name = "Reference Qualifier: Allowed qualifiers: (grp6)",
                                    DataElements = new Element[]{
                                // BM
                                new DataElement()
                                {
                                    Name = "Reference Qualifier",
                                    Description = @"BN Número de reserva asignado por la línea naviera.",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 3}
                                },

                                // HJSC1234740
                                new DataElement()
                                {
                                    Name = @"Reference Number",
                                    Description = @"Número de reserva.",
                                    Usage = RuleUsage.Required,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 70 }
                                }
                            }
                            }
                            }
                        }, // ACA FINALIZA EL SEGMENTO RFF DEL GRUPO 6
                        /*
                            Sólo requerido en contenedores llenos de exportación desde Costa Rica.
                            Ejemplo: 
                            RFF+ABT:DE245678369'
                        */
                        // TERCERA REFERENCIA LISTO
                        new SegmentData(){
                            SegmentID = "RFF",
                            Name = "REFERENCE (grp6)",
                            Notes = "6",
                            DataElements = new Element[]
                            {
                                new CompositeElement(){
                                    Name = "Reference Qualifier: Allowed qualifiers: (grp6)",
                                    DataElements = new Element[]{
                                // BM
                                new DataElement()
                                {
                                    Name = "Reference Qualifier",
                                    Description = @"ABT: Número de declaración de exportación emitido por las aduanas de Costa Rica..",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 3}
                                },

                                // HJSC1234740
                                new DataElement()
                                {
                                    Name = @"Reference identifier",
                                    Description = @"Número de Declaración",
                                    Usage = RuleUsage.Required,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 70 }
                                }
                            }
                            }
                            }
                        }, // ACA FINALIZA EL SEGMENTO RFF DEL GRUPO 6

                        /*
                            El valor esperado es la fecha y hora de carga o descarga del contenedor. 
                            Ejemplo
                            DTM+203:201401251450:203
                        */ 
                        // LISTO
                        new SegmentData()
                        {
                            SegmentID = "DTM",
                            Name = "Date - Time - Period GRP(6)",
                            Usage = RuleUsage.Mandatory,
                            DataElements = new Element[]
                            {
                        new CompositeElement()
                        {
                            Name = "Date Time",
                            DataElements = new DataElement[]
                            {

                                new DataElement()
                                {
                                    Name = "Date/Time/Period Qualifier",
                                    Description = @"203 Fecha y hora de la operación.",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new[] { 1, 3 }
                                },

                                new DataElement()
                                {
                                    Name = "Date/Time/Period",
                                    Description = "Fecha y hora de operación",
                                    Usage = RuleUsage.Required,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new[] { 1, 35 }
                                },

                                new DataElement()
                                {
                                    Name = "Date/Time/Period Format Qualifier",
                                    Description = @"203 YYYYMMDDHHMM",
                                    Usage = RuleUsage.Required,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new[] { 1, 3 }
                                }

                            }
                        }
                    }
                },// ACA FINALIZA EL SEGMENTO DTM DEL GRUPO 6

                    // LISTO
                    new SegmentData(){
                            SegmentID = "MEA",
                            Name = "MEASUREMENTS (grp6)",
                            Notes = "6",
                            DataElements = new Element[]{

                                new DataElement(){
                                    Name = "Measurement Application Qualifier",
                                    Description = @"AAE Medida",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new[] { 1, 3 }
                                },

                                new DataElement(){
                                    Name = "Measurement Application Qualifier",
                                    Description = @"VGM = Peso verificado y certificado del contenedor.",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new[] { 1, 3 }
                                },

                               new CompositeElement(){
                                    Name = "Measure Unit",
                                    DataElements = new Element[]{
                                        new DataElement(){
                                            Name = "Measure Unit Qualifier",
                                            Description = @"Allowed qualifiers: ""KGM"" = kilogram = preferred ""LBR"" = pounds",
                                            Usage = RuleUsage.Mandatory,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 3 }
                                        },

                                        new DataElement(){
                                            Name = "Measurement Value",
                                            Description = @"Peso bruto del contenedor, embalaje y mercancía. Dos decimales",
                                            Usage = RuleUsage.Required,
                                            DataType = DataType.Numeric,
                                            Precision = new []{1, 18}
                                            // Precision = 18 //LONGITUD EXACTA SI O SI TIENE QUE TENER UNA LOGITUD DE 18  
                                        }
                                    }
                               }
                            }
                        },// ACA FINALIZA EL SEGMENTO MEA DEL GRUPO 6

                        // LISTO
                        new SegmentData()
                        {
                            SegmentID = "TMP",
                            Name = "TEMPERATURE (grp6)",
                            Notes = "6",
                            DataElements = new Element[]
                            {
                                new DataElement()
                                {
                                    Name = "Temperature Qualifier",
                                    Description = @" ""2"" = Transport Temperature",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new[] { 1, 3 }
                                },

                                new CompositeElement(){
                                    Name = "Temperature Setting",
                                    DataElements = new Element[]{
                                new DataElement()
                                {
                                    Name = "Temperature value",
                                    Description = "Actual temperature according to Reefer List (no deviation allowed) at which the cargo is to be transported. For field format see remarks below.",
                                    Usage = RuleUsage.Required,
                                    DataType = DataType.Decimal,
                                    Precision = new []{1, 15}
                                },
                                new DataElement()
                                {
                                    Name = "Measure Unit Qualifier",
                                    Description = @"Allowed qualifiers: ""CEL"" = Celsius, ""FAH"" = Fahrenheit",
                                    Usage = RuleUsage.Required,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new[] { 1, 3 }
                                }
                            }
                            }
                            }
                        }, // ACA FINALIZA EL SEGMENTO TMP DEL GRUPO 6

                        // LISTO
                        new SegmentData()
                        {
                            SegmentID = "SEL",
                            Name = "SEAL NUMBER (grp6)",
                            Notes = "6",
                            DataElements = new Element[]
                            {
                                new DataElement()
                                {
                                    Name = "Seal identifier",
                                    Description = @"número de Sello",
                                    Usage = RuleUsage.Required,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new[] { 1, 35 }
                                },

                                new CompositeElement(){
                                    Name = "Seal issuer",
                                    DataElements = new Element[]{
                                new DataElement()
                                {
                                    Name = "Sealing party name code",
                                    Description = "AA consolidador AB desconocido  AC Agencia cuarentenaria CA Naviera  CU Aduanas SH Exportador TO Operador portuario",
                                    Usage = RuleUsage.Required,
                                    DataType = DataType.Decimal,
                                    Precision = new []{1, 3}
                                },
                                new DataElement()
                                {
                                    Name = "SEAL TYPE CODE",
                                    Description = @"1 = MECHANICAL, 2 = ELECTRONIC, 3 = AIR FRESH VENT",
                                    Usage = RuleUsage.Required,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new[] { 1, 3 }
                                }
                            }
                            }
                            }
                        }, // ACA FINALIZA EL SEGMENTO TMP DEL GRUPO 6

                        // LISTO
                        new SegmentData(){
                            SegmentID = "NAD",
                            Name = "NAME AND ADDRESS (grp6)",
                            Notes = "6",
                            DataElements = new Element[]
                            {
                                new DataElement()
                                {
                                    Name = @"Party Qualifier",
                                    Description = @"CF Naviera o agencia Naviera responsible del contenedor",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 3 }
                                },

                                new CompositeElement(){
                                    Name = "PARTY IDENTIFICATION DETAILS (grp6)",
                                    DataElements = new Element[]{
                                new DataElement()
                                {
                                    Name = "Party Id Identification",
                                    Description = @"Code specifying the identity of a party.",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 35}
                                },

                                new DataElement()
                                {
                                    Name = @"Code List Qualifier",
                                    Description = @"Code identifying a user or association maintained code list. 160",
                                    Usage = RuleUsage.Required,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 3 }
                                },

                                new DataElement()
                                {
                                    Name = @"Code List Responsible Agency, coded. Allowed codes",
                                    Description = @"""20"" = BIC (Bureau International des Containeurs) ""166"" = US National Motor Freight Classification Association (SCAC) ""ZZZ"" = Mutually agreed",
                                    Usage = RuleUsage.Required,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 3 }
                                }
                            }
                            }
                            }
                        }, // ACA FINALIZA EL SEGMENTO NAD DEL GRUPO 6

                        }
                    }, // ACA FINALIZA EL GRUPO 6

                    // GRUPO 7
                    new SegmentGroup(){
                        GroupRepeat = 9999,
                        Segments = new Segment[]
                        {
                            // PRIMER LOC QUE LE PERTENECE A CADA CONTENEDOR
                            /*
                                Ejemplos: 
                                LOC+147+0060382:139:5' 
                            */
                            // LISTO
                            new SegmentData(){
                            SegmentID = "LOC",
                            Name = "PLACE/LOCATION IDENTIFICATION (grp7)",
                            Notes = "7",
                            Position = 1,
                            DataElements = new Element[]
                            {
                                // 147 Localización de estiba.
                                new DataElement()
                                {
                                    Name = "Place/Location Qualifier",
                                    Description = @"147 Localización de estiba.",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new[] { 1, 3 }
                                },

                                new CompositeElement()
                                {
                                    Name = "Place/Location Identification:",
                                    DataElements = new DataElement[]
                                    {
                                        new DataElement()
                                        {
                                            Name = "Location name code",
                                            Description = @"Código LOCODE del Puerto de salida",
                                            Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 25 }
                                        },

                                        new DataElement()
                                        {
                                            Name = "Code list Identification",
                                            Description = @"ZZZ = Definido por mutuo acuerdo",
                                            Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 25 }
                                        },

                                        new DataElement()
                                        {
                                            Name = "Code list responsible agency, coded. Allowed codes",
                                            Description = @"98 = Aduanas de Costa Rica",
                                            Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 3 }
                                        },
                                    }
                                },

                            }
                        }, // ACA FINALIZA EL SEGMENTO LOC (grp7)

                        // SEGUNDO LOC QUE LE PERTENECE A CADA CONTENEDOR
                        /*
                            Corresponde al último Puerto de carga previo a su llegada a Costa Rica. 
                            LOC+9+COCTG'
                        */
                        // LISTO
                            new SegmentData(){
                            SegmentID = "LOC",
                            Name = "PLACE/LOCATION IDENTIFICATION (grp7)",
                            Notes = "7",
                            Position = 1,
                            DataElements = new Element[]
                            {
                                // 9
                                new DataElement()
                                {
                                    Name = "Place/Location Qualifier",
                                    Description = @"9 Lugar de procedencia / carga",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new[] { 1, 3 }
                                },

                                new DataElement()
                                {
                                    Name = "Location name code",
                                    Description = @"Código LOCODE",
                                    Usage = RuleUsage.Required,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new[] { 1, 25 }
                                },

                            }
                        }, // ACA FINALIZA EL SEGMENTO LOC (grp7)

                        // TERCER LOC QUE LE PERTENECE A CADA CONTENEDOR
                        /*
                            Siguiente Puerto de descarga del contenedor. Opcional para vacíos en reportes de descarga.
                            Ejemplos: 
                            LOC+9+COCTG'
                        */
                        // LISTO
                            new SegmentData(){
                            SegmentID = "LOC",
                            Name = "PLACE/LOCATION IDENTIFICATION (grp7)",
                            Notes = "7",
                            Position = 1,
                            DataElements = new Element[]
                            {
                                // 9
                                new DataElement()
                                {
                                    Name = "Place/Location Qualifier",
                                    Description = @"11 Lugar de descarga.",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new[] { 1, 3 }
                                },

                                new DataElement()
                                {
                                    Name = "Location name code",
                                    Description = @"Código LOCODE",
                                    Usage = RuleUsage.Required,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new[] { 1, 25 }
                                },

                            }
                        }, // ACA FINALIZA EL SEGMENTO LOC (grp7)

                        }
                        }, // ACA FINALIZA EL GRUPO 7

                        // GRUPO 9 ESTO VA SIEMPRE Y CUANDO UN CONTENEDOR TRAIGA CARGA PELIGROSA
                        new SegmentGroup(){
                            GroupRepeat = 9999,
                            Segments = new Segment[]{
                                // LISTO
                                new SegmentData(){
                                SegmentID = "DGS",
                                Name = "DANGEROUS GOODS (grp9)",
                                Notes = "9",
                                DataElements = new Element[]
                                {
                                   new DataElement()
                                {
                                    Name = "Dangerous Goods Regulations",
                                    Description = @"Code ""IMD"" (IMO IMDG Code)",
                                    Usage = RuleUsage.Required,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 3}
                                },

                                new CompositeElement(){
                                    Name = "Hazard Code Identification (grp9)",
                                    DataElements = new Element[]{
                                new DataElement()
                                {
                                    Name = "Hazard Code Identification",
                                    Description = @"IMDG Code, e.g. ""1.2"" or ""8""",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 7}
                                },
                            }
                            },

                            new CompositeElement(){
                                    Name = "United Nations Dangerous Goods (UNDG) identifier",
                                    DataElements = new Element[]{
                                new DataElement()
                                {
                                    Name = "United Nations Dangerous Goods",
                                    Description = @"Información UNDG: Número ONU.",
                                    Usage = RuleUsage.Optional,
                                    DataType = DataType.Numeric,
                                    Precision = 3
                                },
                               
                            }
                            },

                            }
                        }, // ACA FINALIZA EL SEGMENTO DGS DEL GRUPO 9

                        // LISTO
                        new SegmentData(){
                                SegmentID = "FTX",
                                Name = "FREE TEXT (grp9)",
                                Notes = "9",
                                DataElements = new Element[]
                                {
                                   new DataElement()
                                {
                                    Name = "Text Subject Qualifier.",
                                    Description = @" ""AAC"" = Dangerous goods additional information ""AAD"" = Dangerous goods, technical name, proper shipping name.",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 3}
                                },

                                new EmptyElement(),
                                new EmptyElement(),

                                new CompositeElement(){
                                    Name = "Free text (grp9)",
                                    DataElements = new Element[]{
                                new DataElement()
                                {
                                    Name = "Free text",
                                    Description = @"Descripción o nombre técnico de la carga peligrosa (en español o inglés.)",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new [] {1, 512}
                                }
                            }
                            }

                                }
                            }, // ACA FINALIZA EL SEGMENTO FTX DEL GRUPO 9
                            }
                        }, // ACA FINALIZA EL GRUPO 9

                        // SEGMENTO CNT LISTO
                        new SegmentData(){
                                SegmentID = "CNT",
                                Name = "CONTROL TOTAL",
                                Notes = "",
                                DataElements = new Element[]
                                {
                                    new CompositeElement(){
                                        Name = "CONTROL",
                                    DataElements = new Element[]{
                                    new DataElement()
                                    {
                                        Name = "Control total type code qualifier",
                                        Description = @"16 Número total de contenedores",
                                        Usage = RuleUsage.Mandatory,
                                        DataType = DataType.Alphanumeric,
                                        Precision = new []{ 1, 3}
                                    },

                                    new DataElement()
                                    {
                                        Name = @"Control total value",
                                        Description = @"Número total de contenedores",
                                        Usage = RuleUsage.Mandatory,
                                        DataType = DataType.Alphanumeric,
                                        Precision = new []{ 1, 18 }
                                    }
                                    }
                                    }
                                   
                                }
                            }, // ACA FINALIZA EL SEGMENTO CNT

                    // LISTO
                    new SegmentData(){
                            SegmentID = "UNT",
                            Name = "MESSAGE TRAILER",
                            Notes = "4",
                            DataElements = new Element[]
                            {
                                new DataElement()
                                {
                                    Name = @"Number of segments in the message",
                                    Description = @"Número de segmentos dentro del mensaje, excluyendo los segmentos UnA, UNB y UNZ e incluyendo UNH y UNT",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Numeric,
                                    Precision = new []{1, 6}
                                },

                                new DataElement()
                                {
                                    Name = "Message reference number",
                                    Description = @"Número de referencia del mensaje. Debe ser idéntico al número en el capo 0062 del segment UNH.",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 14}
                                }

                            }
                        }, // ACA FINALIZA EL SEGMENTO UNT

                        // LISTO
                        new SegmentData(){
                            SegmentID = "UNZ",
                            Name = "INTERCHANGE TRAILER",
                            DataElements = new Element[]
                            {
                                new DataElement()
                                {
                                    Name = @"Interchange Control Count",
                                    Description = @"Conteo de control de intercambio: Número de mensajes en el intercambio.",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Numeric,
                                    Precision = new []{1, 6}
                                },

                                new DataElement()
                                {
                                    Name = "Interchange Control Reference",
                                    Description = @"Referencia de control de intercambio: esta referencia debe ser idéntica a la que se encuentra en UNB campo 0020.",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 14}
                                }

                            }
                        } // ACA FINALIZA EL SEGMENTO UNZ

            };
        }
    }
}