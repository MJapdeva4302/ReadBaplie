using System;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using Newtonsoft.Json;
using ReadEDIFACT;
using ReadEDIFACT.Models; // Add this line if ParserEDI is in the Parsers namespace

FileDefinition definitionBAPLIE = new FileDefinition("BAPLIE", new Version(2, 2, 1));

definitionBAPLIE.SegmentSeparator = '\'';
definitionBAPLIE.ElementSeparator = '+';
definitionBAPLIE.DataElementSeparator = ':';
definitionBAPLIE.EscapeCharacter = '?';

definitionBAPLIE.Segments = new Segment[]
{
                //SON LOS SEGMENTOS QUE COMPONEN EL ARCHIVO COMO POR EJEMPLO: UNB+UNOA:1+SEACOS+PUBLIC+241013:1441+31039624+++++UNKNOWN'
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
                                    Precision = 4
                                },

                                new DataElement()
                                {
                                    Name = "Sintax Version Number",
                                    Notes = @"Always ""2"".",
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
                                    Precision = 6
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

                        // ESTOS EmptyElement SON PARA LOS "+" QUE SE ENCUENTRAN EN EL SEGMENTO en este caso son 4 y son vacios
                        new EmptyElement(),
                        new EmptyElement(),
                        new EmptyElement(),
                        new EmptyElement(),

                        new DataElement()
                        {
                            Name = "Communications Agreement Id",
                            Description = "A code identifying the shipping line of the vessel (BIC, SCAC or mutually agreed).",
                            Usage = RuleUsage.Recommended,
                            DataType = DataType.Alphanumeric,
                            Precision = new[] { 1, 35}
                        }
                    }
                }, // ACA FINALIZA EL SEGMENTO UNB

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
                                    Description = @"The name of the UNSM or standard EDIFACT message. In this case always ""BAPLIE""",
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
                                    Description = @"The release number of the message. See EDIFACT documentation. At this moment the release number is ""95B""",
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
                                    Description = @"The applicable SMDG User Manual version number. For this manual use: ""SMDG22"". This will enable the recipient of the message to translate the message correctly, even if older versions are still in use.",
                                    Usage = RuleUsage.Required,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new[] { 1, 6 }
                                },

                            }
                        },


                    }
                },// ACA FINALIZA EL SEGMENTO UNH

                new SegmentData()
                {
                    SegmentID = "BGM",
                    Name = "Beginning of Message",
                    Usage = RuleUsage.Mandatory,
                    DataElements = new Element[]
                    {
                        new EmptyElement(),

                        new DataElement()
                        {
                            Name = "Message Name",
                            Description = "Reference allocated by the sender individually, taken from the application.",
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

                new SegmentData()
                {
                    SegmentID = "DTM",
                    Name = "Date - Time - Period",
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
                                    Description = @"Code ""137"" (Document/Message Date/Time)",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new[] { 1, 3 }
                                },

                                new DataElement()
                                {
                                    Name = "Date/Time/Period",
                                    Description = "Date or date/time of compiling the message",
                                    Usage = RuleUsage.Required,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new[] { 1, 35 }
                                },

                                new DataElement()
                                {
                                    Name = "Date/Time/Period Format Qualifier",
                                    Description = @"Allowed qualifiers: ""101"" = YYMMDD ""201"" = YYMMDDHHMM""301"" = YYMMDDHHMMZZZ(""ZZZ"" = Time zone, e.g. ""GMT"" or other)",
                                    Usage = RuleUsage.Required,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new[] { 1, 3 }
                                }

                            }
                        }
                    }
                },// ACA FINALIZA EL SEGMENTO DTM

                new SegmentGroup()
                {
                    GroupRepeat = 1,
                    Segments = new Segment[]
                    {

                        new SegmentData()
                        {
                            SegmentID = "TDT",
                            Name = "DETAILS OF TRANSPORT",
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

                                new EmptyElement(),
                                new EmptyElement(),

                                new CompositeElement()
                                {
                                    Name = "Code List",
                                    Description = "Carrier name, coded. Codes to be agreed or standard carrier alpha code (SCAC).",

                                    DataElements = new DataElement[]
                                    {
                                        new DataElement()
                                        {
                                            Name = "Carrier Identification",
                                            Description = "Carrier name, coded. Codes to be agreed or standard carrier alpha code (SCAC).",
                                            Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 17 }
                                        },

                                        new DataElement()
                                        {
                                            Name = "Code List Qualifier",
                                            Description = @"Code ""172"" (Carrier Code)",
                                            Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 3 }
                                        },

                                        new DataElement()
                                        {
                                            Name = "Code list responsible agency, coded. Allowed codes",
                                            Description = @"""20"" = BIC (Bureau International des Containeurs) ""166"" = US National Motor Freight Classification Association (SCAC) ""ZZZ"" = Mutually defined.",
                                            Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 3 }
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
                                        },

                                        new DataElement()
                                        {
                                            Name = "Nationality of Means of Transport",
                                            Description = "Coded according to UN-country code (ISO 3166).",
                                            Usage = RuleUsage.Optional,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 3 }
                                        }
                                    }

                                },
                            }
                        },// ACA FINALIZA EL SEGMENTO TDT

                        new SegmentData()
                        {
                            SegmentID = "LOC",
                            Name = "PLACE/LOCATION IDENTIFICATION (grp1)",
                            Usage = RuleUsage.Mandatory,
                            DataElements = new Element[]
                            {
                                new DataElement()
                                {
                                    Name = "Place/Location Qualifier",
                                    Description = @"""5"" = Place of Departure ""61"" = Next port of call",
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
                                            Name = "Location Identification",
                                            Description = "place of departure (normally the sender of the message). If possible, UN-Locodes of 5 characters according to UN recommendation no.16. must be used.",
                                            Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 25 }
                                        },

                                        new DataElement()
                                        {
                                            Name = "Code list qualifier. Allowed qualifiers:",
                                            Description = @"""139"" = Port.",
                                            Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 3 }
                                        },

                                        new DataElement()
                                        {
                                            Name = "Code list responsible agency, coded. Allowed codes",
                                            Description = @"""112"" = US, US Census Bureau, Schedule D for U S locations, Schedule K for foreign port locations.""6"" = UN/ECE - United Nations - Economic Commission for Europe. (UN-Locodes)..",
                                            Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 3 }
                                        }
                                    }
                                },

                                new CompositeElement(){
                                    Name = "location one identification",
                                    DataElements = new DataElement[]
                                    {
                                        new DataElement()
                                        {
                                            Name = "Related place/location one identification",
                                            Description = "The ISO country code",
                                            Usage = RuleUsage.Optional,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 25 }
                                        },

                                        new DataElement()
                                        {
                                            Name = "Code list qualifier. Allowed qualifiers:",
                                            Description = @"""162"" = Country",
                                            Usage = RuleUsage.Optional,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 3 }
                                        },

                                        new DataElement()
                                        {
                                            Name = "Code list responsible agency, coded. Allowed codes",
                                            Description = @"""5"" = ISO",
                                            Usage = RuleUsage.Optional,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 3 }
                                        }
                                    }
                                },

                                new CompositeElement(){
                                    Name = "location two identification.",
                                    DataElements = new DataElement[]
                                    {
                                        new DataElement()
                                        {
                                            Name = "Related place/location two identification",
                                            Description = "The state or province code, postal abbreviations.",
                                            Usage = RuleUsage.Optional,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 25 }
                                        },

                                        new DataElement()
                                        {
                                            Name = "Code list qualifier. Allowed qualifiers:",
                                            Description = @"""163"" = Country sub-entity; state or province.",
                                            Usage = RuleUsage.Optional,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 3 }
                                        },

                                    }
                                }
                            }
                        },// ACA FINALIZA EL SEGMENTO LOC

                        new SegmentData(){

                            SegmentID = "DTM",
                            Name = "DATE/TIME/PERIOD (grp1)",
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
                                            Description = @"""178"" = actual date/time of arrival at senders port ""132"" = estimated date or date/time of arrival at the next port of call ""133"" = estimated date or date/time of departure at senders port ""136"" = actual date/time of departure at senders port",
                                            Usage = RuleUsage.Mandatory,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 3 }
                                        },

                                        new DataElement()
                                        {
                                            Name = "Date/Time/Period",
                                            Description = @"Date or date/time in local time when Means of Transport has arrived/departed or is expected to depart at the senders port or is expected to arrive at the next port of call.",
                                            Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 35 }
                                        },

                                        new DataElement()
                                        {
                                            Name = "Date/Time/Period Format Qualifier. Allowed qualifiers",
                                            Description = @"""101"" = YYMMDD ""201"" = YYMMDDHHMM ""301"" = YYMMDDHHMMZZZ (""ZZZ"" = Time zone, e.g. ""GMT"" or other)",
                                            Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 3 }
                                        }
                                    }
                                }
                            }
                        }, // ACA FINALIZA EL SEGMENTO DTM (grp1)

                        new SegmentData(){
                            SegmentID = "RFF",
                            Name = "REFERENCE (grp1)",
                            Position = 1,
                            DataElements = new Element[]
                            {

                                new CompositeElement()
                                {
                                    Name = "Reference",
                                    DataElements = new DataElement[]
                                    {

                                        new DataElement()
                                        {
                                            Name = "Reference Qualifier",
                                            Description = @"Code ""VON"" (Loading Voyage number, if different from the voyage number in the TDT-segment, assigned by the Operating Carrier or his agent to the voyage of the vessel).",
                                            Usage = RuleUsage.Mandatory,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 3 }
                                        },

                                        new DataElement()
                                        {
                                            Name = "Reference Number",
                                            Description = @"The Loading voyage number",
                                            Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 35 }
                                        }
                                    }
                                }
                            }
                        }, // ACA FINALIZA EL SEGMENTO RFF (grp1)
                    },

                },// ACA FINALIZA EL GRUPO 1

                new SegmentGroup(){
                    GroupRepeat = 99999,
                    Segments = new Segment[]
                    {
                        new SegmentData(){
                            SegmentID = "LOC",
                            Name = "PLACE/LOCATION IDENTIFICATION (grp2)",
                            Position = 1,
                            DataElements = new Element[]
                            {
                                new DataElement()
                                {
                                    Name = "Place/Location Qualifier",
                                    Description = @"Code ""147"" (Stowage Cell)",
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
                                            Name = "Location Identification",
                                            Description = @"The actual location of the equipment or cargo on the vessel. The following formats are allowed:1. ISO-format 2. Ro/Ro-format3. Other non-ISO-format (to be agreed between partners)1. ISO-format: Bay/Row/Tier (BBBRRTT). If Bay number is less than 3 characters it must be filled with leading zeroes, e.g. ""0340210"".2. Ro/Ro-format:Deck/Bay/Row/Tier (DDBBBRRTT).",
                                            Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 25 }
                                        },

                                        new DataElement()
                                        {
                                            Name = string.Empty,
                                            Description = string.Empty,
                                            Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] {0 , 0 }
                                        },

                                        new DataElement()
                                        {
                                            Name = "Code list responsible agency, coded. Allowed codes",
                                            Description = @"To indicate which format is used. Valid codes are:""5"" (ISO-format)""87"" (Ro/Ro-format, assigned by the Carrier""ZZZ"" (non-ISO-format, mutually defined).",
                                            Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 3 }
                                        }
                                    }
                                }
                            }
                        }, // ACA FINALIZA EL SEGMENTO LOC (grp2)

                        new SegmentData(){
                            SegmentID = "GID",
                            Name = "GOODS ITEM DETAILS (grp2)",
                            Position = 2,
                            DataElements = new Element[]
                            {
                                new EmptyElement(),

                                new CompositeElement()
                                {
                                    Name = "The number of packages",
                                    DataElements = new DataElement[]{
                                        new DataElement()
                                        {
                                            Name = "Number of packages",
                                            Description = @"Number of packages. The number of packages of non containerized cargo. If the cargo is Ro/Ro then the number ""1"" is used.",
                                            Usage = RuleUsage.Optional,
                                            DataType = DataType.Numeric,
                                            Precision = 8
                                        },

                                        new DataElement()
                                        {
                                            Name = "Type of packages identification",
                                            Description = @"Type of packages identification. Package type for non containerized cargo.",
                                            Usage = RuleUsage.Optional,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 17 }
                                        }

                                    }
                                }
                            }
                        }, // ACA FINALIZA EL SEGMENTO GID (grp2)

                        new SegmentData(){
                            SegmentID = "GDS",
                            Name = "NATURE OF CARGO (grp2)",
                            DataElements = new Element[]{
                                new DataElement(){
                                    Name = "Nature of cargo",
                                    Description = @"Nature of cargo, coded. Codes to be agreed between partners",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new[] { 1, 3 }
                                }
                            }
                        }, // ACA FINALIZA EL SEGMENTO GDS DEL GRUPO 2

                        new SegmentData(){
                            SegmentID = "FTX",
                            Name = "FREE TEXT (grp2)",

                            DataElements = new Element[]{
                                new DataElement(){
                                    Name = "Text Subject Qualifier",
                                    Description = @"Allowed qualifiers: ""AAA"" = Description of Goods""HAN"" = Handling Instructions""CLR"" = Container Loading Remarks""SIN"" = Special instructions""AAI"" = General information""AAY"" = Certification statements""ZZZ"" = Mutually defined use",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new[] { 1, 3 }
                                },

                                new EmptyElement(),

                                new DataElement(){
                                    Name = "Text reference",
                                    Description = @"In case of e4451 = AAY used for specification of data transmitted in c108. Use codes defined in SMDG codelist VGM.",
                                    Usage = RuleUsage.Optional,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new[] { 1, 3 }
                                },

                                new CompositeElement(){
                                    Name = "Description/Instructions/Remarks",
                                    DataElements = new Element[]{
                                        new DataElement(){
                                            Name = "Free Text",
                                            Description = @"Description/Instructions/Remarks in plain language or coded, for specific cargo/equipment. Codes, etc. to be agreed between partners.",
                                            Usage = RuleUsage.Mandatory,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 70 }
                                        },

                                        new DataElement(){
                                            Name = "Free Text 1",
                                            Description = @"Information about VGM according to code specified in c107.4441. (For details see page 42.)",
                                            Usage = RuleUsage.Optional,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 70 }
                                        },

                                        new DataElement(){
                                            Name = "Free Text 2",
                                            Description = @"Information about VGM according to code specified in c107.4441. (For details see page 42.)",
                                            Usage = RuleUsage.Optional,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 70 }
                                        },

                                        new DataElement(){
                                            Name = "Free Text 3",
                                            Description = @"Information about VGM according to code specified in c107.4441. (For details see page 42.)",
                                            Usage = RuleUsage.Optional,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 70 }
                                        },

                                        new DataElement(){
                                            Name = "Free Text 4",
                                            Description = @"Information about VGM according to code specified in c107.4441. (For details see page 42.)",
                                            Usage = RuleUsage.Optional,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 70 }
                                        }
                                    }
                                }
                            }
                        }, // ACA FINALIZA EL SEGMENTO FTX DEL GRUPO 2

                        new SegmentData(){
                            SegmentID = "MEA",
                            Name = "MEASUREMENTS (grp2)",
                            DataElements = new Element[]{
                                new DataElement(){
                                    Name = "Measurement Application Qualifier",
                                    Description = @"Allowed qualifiers: ""WT"" (gross weight / gross mass) – not confirmed as verified ""VGM"" (verified gross mass) – specified weight is verified[code VGM has been introduced in D.15B for data element 6313]",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new[] { 1, 3 }
                                },

                                new EmptyElement(),

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
                                            Description = @"The gross mass of the transport equipment including cargo in kilograms or pounds, as qualified (no decimals)",
                                            Usage = RuleUsage.Required,
                                            DataType = DataType.Numeric,
                                            Precision = new []{1, 18}
                                            // Precision = 18 //LONGITUD EXACTA SI O SI TIENE QUE TENER UNA LOGITUD DE 18
                                            
                                        }
                                    }
                               }
                            }
                        },

                        new SegmentData()
                        {
                            SegmentID = "DIM",
                            Name = "DIMENSIONS (grp2)",
                            DataElements = new Element[]
                            {
                                new DataElement()
                                {
                                    Name = "Dimension Qualifier",
                                    Description = @"Code ""1"" = Gross dimensions (break-bulk) Code ""5"" = Off-standard dims. (over-length front)Code ""6"" = Off-standard dims. (over-length back)Code ""7"" = Off-standard dims. (over-width right)Code ""8"" = Off-standard dims. (over-width left)Code ""9"" = Off-standard dims. (over-height)Code ""10"" = external equipment dimensions (Non-ISO equipment)Basically allowed qualifier ""1"" for break-bulk cargo and from ""5"" to ""9"" for odd-sized-cargo. However allowed from ""5"" to ""9"" for break-bulk cargo as additional information, if required.",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new[] { 1, 3 }
                                },

                                new CompositeElement(){
                                    Name = "Measure Unit",
                                    DataElements = new Element[]{
                                new DataElement()
                                {
                                    Name = "Measure Unit Qualifier",
                                    Description = @"Allowed qualifiers: ""CMT"" = Centimeters = preferred ""INH"" = Inches",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new[]{ 1, 3 }
                                },

                                new DataElement()
                                {
                                    Name = "Length Dimension",
                                    Description = @"Break-bulk length or over-length for containers, as qualified.",
                                    Usage = RuleUsage.Dependent,
                                    DataType = DataType.Numeric,
                                    Precision = 15
                                },

                                new DataElement()
                                {
                                    Name = "Width Dimension",
                                    Description = @"Break-bulk width or over-width for containers, as qualified.",
                                    Usage = RuleUsage.Dependent,
                                    DataType = DataType.Numeric,
                                    Precision = 15
                                },

                                new DataElement()
                                {
                                    Name = "Height Dimension",
                                    Description = @" Break-bulk height or over-height for containers, as qualified",
                                    Usage = RuleUsage.Dependent,
                                    DataType = DataType.Numeric,
                                    Precision = 15
                                }
                                }
                            }
                        }
                        }, // ACA FINALIZA EL SEGMENTO DEL GRUPO 2 

                        new SegmentData()
                        {
                            SegmentID = "TMP",
                            Name = "TEMPERATURE (grp2)",
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
                                    Name = "Temperature Setting",
                                    Description = "Actual temperature according to Reefer List (no deviation allowed) at which the cargo is to be transported. For field format see remarks below.",
                                    Usage = RuleUsage.Required,
                                    DataType = DataType.Decimal,
                                    Precision = 3
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
                        }, // ACA FINALIZA EL SEGMENTO TMP DEL GRUPO 2

                        new SegmentData()
                        {
                            SegmentID = "RNG",
                            Name = "RANGE DETAILS (grp2)",
                            DataElements = new Element[]
                            {
                                new DataElement()
                                {
                                    Name = "Range Type Qualifier",
                                    Description = @" Allowed qualifier: ""4"" = Quantity range.",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new[] { 1, 3 }
                                },

                                new CompositeElement(){
                                    Name = "Unit Qualifier",
                                    DataElements = new Element[]{
                                new DataElement()
                                {
                                    Name = "Measure Unit Qualifier",
                                    Description = @"""CEL"" = degrees Celsius ""FAH"" = degrees Fahrenheit",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = 3
                                },

                                new DataElement()
                                {
                                    Name = "Range Minimum",
                                    Description = @"Minimum temperature according to Reefer List at which the cargo is to be transported.",
                                    Usage = RuleUsage.Required,
                                    DataType = DataType.Numeric,
                                    Precision = 18
                                },

                                new DataElement()
                                {
                                    Name = "Range Maximum",
                                    Description = @"Maximum temperature according to Reefer List at which the cargo is to be transported.",
                                    Usage = RuleUsage.Required,
                                    DataType = DataType.Numeric,
                                    Precision = 18
                                }
                            }
                            }
                            }
                        }, // ACA FINALIZA EL SEGMENTO RGN DEL GRUPO 2

                        new SegmentData(){
                            SegmentID = "LOC",
                            Name = "PLACE/LOCATION IDENTIFICATION (grp2)",
                            DataElements = new Element[]
                            {
                                new DataElement()
                                {
                                    Name = "Place/Location Qualifier",
                                    Description = @"""9"" = Place/Port of Loading""11"" = Place/Port of discharge""13"" = Transshipment port/Place of transshipment""64"" = 1st optional port of discharge""68"" = 2nd optional port of discharge""70"" = 3rd optional port of discharge""76"" = Original port of loading""83"" = Place of delivery (to be used as final destination or double stack train destination).""97"" = Optional place/port of discharge. To be used if actual port of discharge is undefined, i.e. ""XXOPT"".""152"" = Next port of discharge",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new[] { 1, 3 }
                                },

                                new CompositeElement(){
                                    Name = "Namecode of the place/port, as qualified",
                                    DataElements = new Element[]{
                                new DataElement()
                                {
                                    Name = "Place/Location Identification",
                                    Description = @"Allowed code lists: UN-Locode or US-Census codes.""Sample codes: JPTYO = TokyoUSLAX = Los AngelesUSOAK = OaklandUSSEA = SeattleUSCHI = ChicagoFor optional port of discharge: ""XXOPT"" (Qualifier e3227: ""97"").",
                                    Usage = RuleUsage.Required,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 25}
                                },

                                new DataElement()
                                {
                                    Name = "Code list qualifier. Allowed qualifiers",
                                    Description = @"""139"" = Port",
                                    Usage = RuleUsage.Optional,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 3 }
                                },

                                new DataElement()
                                {
                                    Name = "Code list responsible agency, coded. Allowed codes",
                                    Description = @"""112"" = US, US Census Bureau, Schedule D for U S locations,Schedule K for foreign port locations.""6"" = UN/ECE - United Nations - Economic Commission forEurope. (UN-Locodes).""ZZZ"" = Optional ports.",
                                    Usage = RuleUsage.Optional,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 3 }
                                }
                            }
                            },

                            new CompositeElement(){
                                    Name = "Related place/location one identification (grp2)",
                                    DataElements = new Element[]{
                                new DataElement()
                                {
                                    Name = "Related place/location one identification",
                                    Description = @"The name code of the Container Terminal in the port ofdischarge or the port of loading. Terminal codes to be used as defined in SMDG’s Master Terminal Facilities code list.",
                                    Usage = RuleUsage.Required,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 25}
                                },

                                new DataElement()
                                {
                                    Name = "Code list qualifier. Allowed qualifier",
                                    Description = @"""TER"" = TERMINALS (leading 3 characters due to limited size)",
                                    Usage = RuleUsage.Optional,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 3 }
                                },

                                new DataElement()
                                {
                                    Name = "Code list responsible agency, coded. Allowed codes",
                                    Description = @" ""306"" = SMDG (code 306 is defined in D.02B and later)",
                                    Usage = RuleUsage.Optional,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 3 }
                                }
                            }
                            }

                            }
                        }, // ACA FINALIZA EL SEGMENTO LOC DEL GRUPO 2

                        new SegmentData(){
                            SegmentID = "RFF",
                            Name = "REFERENCE (grp2)",
                            DataElements = new Element[]
                            {
                                new CompositeElement(){
                                    Name = "Reference Qualifier: Allowed qualifiers: (grp2)",
                                    DataElements = new Element[]{
                                new DataElement()
                                {
                                    Name = "Reference Qualifier: Allowed qualifiers:",
                                    Description = @"""BM"" = B/L-number. ""BN"" = Booking reference number.""ET"" = Excess Transportation Number to be used for leading Stowage position, in case of Break-bulk or odd-sized-cargo. ""ZZZ"" = Mutually defined.",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 3}
                                },

                                new DataElement()
                                {
                                    Name = @"Reference Number: For Qualifiers ""BM"", ""BN"" or ""ZZZ"":",
                                    Description = @"Dummy value ""1"" or the actual Bill of Lading number resp. Booking Reference number, as agreed. For Qualifier ""ET"": leading stowage location, containing relevant data for this consignment.",
                                    Usage = RuleUsage.Required,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 35 }
                                }
                            }
                            }
                            }
                        } // ACA FINALIZA EL SEGMENTO RFF DEL GRUPO 2

                    }, // ACA FINALIZA LOS SEGMENTOS GRUPO 2
                    
                }, //ACA FINALIZA EL GRUPO 2

                 new SegmentGroup(){
                        GroupRepeat = 9,
                        Segments = new Segment[]
                        {
                            new SegmentData(){
                                SegmentID = "EQD",
                                Name = "EQUIPMENT DETAILS (grp3)",
                                DataElements = new Element[]
                                {
                                   new DataElement()
                                {
                                    Name = "Equipment Qualifier",
                                    Description = @" ""CN"" = Container ""BB"" = Break-bulk ""TE"" = Trailer ""ZZZ"" = Ro/Ro or otherwise",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 3}
                                },

                                new DataElement()
                                {
                                    Name = @"Equipment Identification Number",
                                    Description = @"1. The container number: Format: One continuous string with the identification, prefixand number. Examples: SCXU 2387653 must be transmitted as ""SCXU2387653"", EU 876 must be transmitted as ""EU876"". The number will be treated as a character string. E.g. alphanumeric check-digits can be transmitted here. If this segment is used the unique equipment identification number must always be transmitted, although this element is not mandatory! 2. Break-bulk: The break-bulk reference number. The assigned break-bulk reference numbers must be agreed between partners. 3. Otherwise (Ro/Ro): The equipment identification number.",
                                    Usage = RuleUsage.Required,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 17 }
                                },

                                new DataElement()
                                {
                                    Name = @"Equipment Size and Type Identification",
                                    Description = @"ISO size-type code of 4 digits (ISO 6346). Leave blank in case of break-bulk. For unknown ISO size/type codes the following codes can beagreed between partners ""9999"" = No information at all. ""4999"" = Length = 40ft, rest unknown""2999"" = Length = 20ft, rest unknown""4299"" = 40ft ""8'6"", rest unknown""2299"" = 20ft ""8'6"", rest unknown""4099"" = 40ft ""8'0"", rest unknown ""2099"" = 20ft ""8'0"", rest unknownOther codes to be agreed between partners.",
                                    Usage = RuleUsage.Dependent,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 4 }
                                },

                               // new EmptyElement(),

                                //  new DataElement()
                                // {
                                //     Name = @"Equipment status, coded",
                                //     Description = @"1: Continental 11: Direct delivery 2: Export 12: Bond transport 3: Import 13: Transship to other vesse 4: Remain on board 14: Transship to other pier 5: Shifter 15: Rail road transport 6: Transshipment 16: Road transport 7: Hot delivery 17: Barge transport 8: MLB 18: Temporary stowage 9: MCB (Micro Land Bridge) 19: Urgent unpacking 10: Canada Bound transport 20: Sea & Air",
                                //     Usage = RuleUsage.Optional,
                                //     DataType = TypeCode.String,
                                //     Precision = new []{ 1, 3 }
                                // },

                                new EmptyElement(),
                                new EmptyElement(),

                                new DataElement()
                                {
                                    Name = @"Full/Empty Indicator, coded. Allowed codes",
                                    Description = @"""5"" = Full ""4"" = Empty. Leave blank in case of break-bulk.",
                                    Usage = RuleUsage.Dependent,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 3 }
                                }
                                }
                            }, // ACA FINALIZA EL SEGMENTO EQD DEL GRUPO 3

                            new SegmentData(){
                                SegmentID = "EQA",
                                Name = "EQUIPMENT ATTACHED (grp3)",
                                DataElements = new Element[]
                                {
                                   new DataElement()
                                {
                                    Name = "Equipment Qualifier: Allowed qualifiers",
                                    Description = @" ""RG"" = Reefer Generator ""CN"" = Container ""CH"" = Chassis",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 3}
                                },

                                new DataElement()
                                {
                                    Name = @"Equipment Identification Number",
                                    Description = @"The unit number",
                                    Usage = RuleUsage.Required,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 17 }
                                }

                                }
                            }, // ACA FINALIZA EL SEGMENTO EQA DEL GRUPO 3

                            new SegmentData(){
                            SegmentID = "NAD",
                            Name = "NAME AND ADDRESS (grp3)",
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
                                    Name = "Party Id Identification (grp3)",
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
                            }
                            }
                        } // ACA FINALIZA EL SEGMENTO NAD DEL GRUPO 3

                        }
                    }, // ACA FINALIZA EL GRUPO 3

                    new SegmentGroup(){
                        GroupRepeat = 9999,
                        Segments = new Segment[]
                        {
                            new SegmentData(){
                                SegmentID = "DGS",
                                Name = "DANGEROUS GOODS (grp4)",
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
                                    Name = "Hazard Code Identification (grp4)",
                                    DataElements = new Element[]{
                                new DataElement()
                                {
                                    Name = "Hazard Code Identification",
                                    Description = @"IMDG Code, e.g. ""1.2"" or ""8""",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 7}
                                },

                                new DataElement()
                                {
                                    Name = @"Hazard Substance/item/page number",
                                    Description = @"The IMDG code page number (English version).",
                                    Usage = RuleUsage.Optional,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 7 }
                                }
                            }
                            },

                            new DataElement()
                            {
                                Name = @"UNDG Number",
                                Description = @"UN number of respective dangerous cargo transported (4 digits)",
                                Usage = RuleUsage.Optional,
                                DataType = DataType.Numeric,
                                Precision = 4
                            },

                            new CompositeElement(){
                                    Name = "Shipment Flashpoint (grp4)",
                                    DataElements = new Element[]{
                                new DataElement()
                                {
                                    Name = "Shipment Flashpoint",
                                    Description = @"the actual flashpoint in degrees Celsius or Fahrenheit. For inserting temperatures below zero or tenth degrees please refer to remarks under TMP-segment respectively to ISO 9735. If different dangerous goods with different flashpoints within one load to be transported, only the lowest flashpoint should be inserted",
                                    Usage = RuleUsage.Optional,
                                    DataType = DataType.Numeric,
                                    Precision = 3
                                },

                                new DataElement()
                                {
                                    Name = @"Measure Unit Qualifier",
                                    Description = @"Allowed qualifiers: ""CEL"" (degrees Celsius) = Preferred ""FAH"" (degrees Fahrenheit)",
                                    Usage = RuleUsage.Optional,
                                    DataType = DataType.Decimal,
                                    Precision = 3
                                }
                            }
                            },

                            new DataElement()
                            {
                                Name = @"Packing group, coded",
                                Description = @"The packing group code of the hazardous goods.",
                                Usage = RuleUsage.Optional,
                                DataType = DataType.Alphanumeric,
                                Precision = 3
                            },

                            new DataElement()
                            {
                                Name = @"EMS number",
                                Description = @"Emergency schedule number.",
                                Usage = RuleUsage.Optional,
                                DataType = DataType.Alphanumeric,
                                Precision = 6
                            },

                            new DataElement()
                            {
                                Name = @"MFAG",
                                Description = @"Medical First Aid Guide number",
                                Usage = RuleUsage.Optional,
                                DataType = DataType.Alphanumeric,
                                Precision = 4
                            },

                            new EmptyElement(),

                            new CompositeElement(){
                                    Name = "Hazard Identification number, upper part (grp4)",
                                    DataElements = new Element[]{
                                new DataElement()
                                {
                                    Name = "Hazard Identification number",
                                    Description = @"Hazard Identification number, upper part",
                                    Usage = RuleUsage.Optional,
                                    DataType = DataType.Alphanumeric,
                                    Precision = 4
                                },

                                new DataElement()
                                {
                                    Name = @"Substance Identification number",
                                    Description = @"Substance Identification number, lower part",
                                    Usage = RuleUsage.Optional,
                                    DataType = DataType.Alphanumeric,
                                    Precision = 4
                                }
                            }
                            },

                            new CompositeElement(){
                                    Name = "Dangerous Goods Label Marking (grp4)",
                                    DataElements = new Element[]{
                                new DataElement()
                                {
                                    Name = "Dangerous Goods Label Marking (1)",
                                    Description = @"See below for possible use of this data element",
                                    Usage = RuleUsage.Optional,
                                    DataType = DataType.Alphanumeric,
                                    Precision = 4
                                },

                                new DataElement()
                                {
                                    Name = @"Dangerous Goods Label Marking (2)",
                                    Description = @"Dangerous Goods Label Marking (2)",
                                    Usage = RuleUsage.Optional,
                                    DataType = DataType.Alphanumeric,
                                    Precision = 4
                                },

                                new DataElement()
                                {
                                    Name = @"Dangerous Goods Label Marking (3)",
                                    Description = @"Dangerous Goods Label Marking (3)",
                                    Usage = RuleUsage.Optional,
                                    DataType = DataType.Alphanumeric,
                                    Precision = 4
                                }
                            }
                            }

                            }
                        }, // ACA FINALIZA EL SEGMENTO DGS DEL GRUPO 4

                            new SegmentData(){
                                SegmentID = "FTX",
                                Name = "FREE TEXT (grp4)",
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
                                    Name = "Free text (grp4)",
                                    DataElements = new Element[]{
                                new DataElement()
                                {
                                    Name = "Free text",
                                    Description = @"Description of hazard material in plain language. One element of maximum 70 characters to be given only for the description. Transmit the text ""NIL"", if no description is available and one or both of the following data elements must be transmitted.",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new [] {1, 70}
                                },

                                new DataElement()
                                {
                                    Name = @"Free text 2: (grp4)",
                                    Description = @"The net weight in kilos of the hazardous material to be transmitted here.",
                                    Usage = RuleUsage.Optional,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{1, 70}
                                },

                                new DataElement()
                                {
                                    Name = @"Free text 3: (grp4)",
                                    Description = @"The DG-reference number as allocated by the central planner, if known.",
                                    Usage = RuleUsage.Optional,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{1, 70}
                                }
                            }
                            }

                                }
                            }, // ACA FINALIZA EL SEGMENTO FTX DEL GRUPO 4

                            new SegmentData(){
                            SegmentID = "UNT",
                            Name = "MESSAGE TRAILER",
                            DataElements = new Element[]
                            {
                                new DataElement()
                                {
                                    Name = @"Number of segments in the message",
                                    Description = @"Number of segments in the message, including UNH and UNT segments, but excluding UNA, UNB and UNZ segments.",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Numeric,
                                    Precision = new []{1, 6}
                                },

                                new DataElement()
                                {
                                    Name = "Message reference number",
                                    Description = @"This reference must be identical to the reference in the UNH-segment (e0062).",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 14}
                                }

                            }
                        }, // ACA FINALIZA EL SEGMENTO UNT DEL GRUPO 4

                        new SegmentData(){
                            SegmentID = "UNZ",
                            Name = "INTERCHANGE TRAILER",
                            DataElements = new Element[]
                            {
                                new DataElement()
                                {
                                    Name = @"Interchange Control Count",
                                    Description = @"The number of messages in the interchange.",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Numeric,
                                    Precision = new []{1, 6}
                                },

                                new DataElement()
                                {
                                    Name = "Interchange Control Reference",
                                    Description = @"This reference must be identical to the reference in the UNB-segment (e0020).",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 14}
                                }

                            }
                        } // ACA FINALIZA EL SEGMENTO UNZ DEL GRUPO 4

                        }
                    } // ACA FINALIZA EL GRUPO 4

};
FileDefinition definitionCOARRI = new FileDefinition("COARRI", new Version(2, 2, 1));

definitionCOARRI.SegmentSeparator = '\'';
definitionCOARRI.ElementSeparator = '+';
definitionCOARRI.DataElementSeparator = ':';
definitionCOARRI.EscapeCharacter = '?';

definitionCOARRI.Segments = new Segment[]
{

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
                                    Name = "Message type Release Number",
                                    Description = @"The release number of the message. See EDIFACT documentation. At this moment the release number is ""00B""",
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
                                    Description = @"The applicable SMDG User Manual version number. For this manual use: ""SMDG21"". This will enable the recipient of the message to translate the message correctly, even if older versions are still in use.",
                                    Usage = RuleUsage.Required,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new[] { 1, 6 }
                                },

                            }
                        },


                    }
                },// ACA FINALIZA EL SEGMENTO UNH

                new SegmentData()
                {
                    SegmentID = "BGM",
                    Name = "Beginning of Message",
                    Usage = RuleUsage.Mandatory,
                    DataElements = new Element[]
                    {

                        new DataElement()
                        {
                            Name = "Document name code",
                            Description = "24 Restow Only used to report position to which equipment is restowed (new stowage position). Position from which equipment is restowed is not to be reported. 119 Transport discharge report. 122 Transport loading report",
                            Usage = RuleUsage.Required,
                            DataType = DataType.Alphanumeric,
                            Precision = new[] { 1, 3 }
                        },

                        new DataElement()
                        {
                            Name = "Document identifier",
                            Description = @"Sender's unique reference number",
                            Usage = RuleUsage.Required,
                            DataType = DataType.Alphanumeric,
                            Precision = new[] { 1, 35 }
                        },

                        new DataElement()
                        {
                            Name = "MESSAGE FUNCTION CODE",
                            Description = @"If code 1 or 5 is used, reference number of the message which is being cancelled/replaced must appear in the header level RFF using the code ACW in e1153. 1 = Cancellation 5 = Replace 9 = Original",
                            Usage = RuleUsage.Required,
                            DataType = DataType.Alphanumeric,
                            Precision = new[] { 1, 3 }
                        },

                        new DataElement()
                        {
                            Name = "RESPONSE TYPE CODE",
                            Description = @"Message acknowledgement",
                            Usage = RuleUsage.Optional,
                            DataType = DataType.Alphanumeric,
                            Precision = new[] { 1, 3 }
                        }

                    }
                },// ACA FINALIZA EL SEGMENTO BGM 

                new SegmentData()
                {
                    SegmentID = "DTM",
                    Name = "Date - Time - Period",
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
                                    Description = @"Code ""137"" (Document/Message Date/Time)",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new[] { 1, 3 }
                                },

                                new DataElement()
                                {
                                    Name = "Date/Time/Period",
                                    Description = "137 Document/message date/time",
                                    Usage = RuleUsage.Required,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new[] { 1, 35 }
                                },

                                new DataElement()
                                {
                                    Name = "Date/Time/Period Format Qualifier",
                                    Description = @"203 CCYYMMDDHHMM",
                                    Usage = RuleUsage.Required,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new[] { 1, 3 }
                                }

                            }
                        }
                    }
                },// ACA FINALIZA EL SEGMENTO DTM

                new SegmentGroup()
                {
                    GroupRepeat = 1,
                    Segments = new Segment[]
                    {
                        new SegmentData(){
                            SegmentID = "RFF",
                            Name = "REFERENCE (grp1)",
                            Position = 1,
                            DataElements = new Element[]
                            {

                                new CompositeElement()
                                {
                                    Name = "Reference",
                                    DataElements = new DataElement[]
                                    {

                                        new DataElement()
                                        {
                                            Name = " Reference code qualifier",
                                            Description = @"AAY = Carrier's agent reference number. ACA = Cargo acceptance order reference number. ACW = Reference number to previous message. AKA = Container disposition order reference number. ARJ = Personal identity card number. ATX = Transport equipment acceptance order reference. ATY = Transport equipment release order reference. BM = Bill of lading numberBN Booking reference number. CN = Carrier's reference numberRE Release number",
                                            Usage = RuleUsage.Mandatory,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 3 }
                                        },

                                        new DataElement()
                                        {
                                            Name = " Reference identifier",
                                            Description = @"Reference number",
                                            Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 70 }
                                        }
                                    }
                                }
                            }
                        }, // ACA FINALIZA EL SEGMENTO RFF (grp1)

                        new SegmentData()
                        {
                            SegmentID = "LOC",
                            Name = "PLACE/LOCATION IDENTIFICATION (grp1)",
                            Usage = RuleUsage.Mandatory,
                            DataElements = new Element[]
                            {
                                new DataElement()
                                {
                                    Name = "Place/Location Qualifier",
                                    Description = @"""5"" = Place of Departure ""61"" = Next port of call",
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
                                            Name = "Location Identification",
                                            Description = "place of departure (normally the sender of the message). If possible, UN-Locodes of 5 characters according to UN recommendation no.16. must be used.",
                                            Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 25 }
                                        },

                                        new DataElement()
                                        {
                                            Name = "Code list qualifier. Allowed qualifiers:",
                                            Description = @"""139"" = Port.",
                                            Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 3 }
                                        },

                                        new DataElement()
                                        {
                                            Name = "Code list responsible agency, coded. Allowed codes",
                                            Description = @"""112"" = US, US Census Bureau, Schedule D for U S locations, Schedule K for foreign port locations.""6"" = UN/ECE - United Nations - Economic Commission for Europe. (UN-Locodes)..",
                                            Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 3 }
                                        }
                                    }
                                },

                                new CompositeElement(){
                                    Name = "location one identification",
                                    DataElements = new DataElement[]
                                    {
                                        new DataElement()
                                        {
                                            Name = "Related place/location one identification",
                                            Description = "The ISO country code",
                                            Usage = RuleUsage.Optional,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 25 }
                                        },

                                        new DataElement()
                                        {
                                            Name = "Code list qualifier. Allowed qualifiers:",
                                            Description = @"""162"" = Country",
                                            Usage = RuleUsage.Optional,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 3 }
                                        },

                                        new DataElement()
                                        {
                                            Name = "Code list responsible agency, coded. Allowed codes",
                                            Description = @"""5"" = ISO",
                                            Usage = RuleUsage.Optional,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 3 }
                                        }
                                    }
                                },

                                new CompositeElement(){
                                    Name = "location two identification.",
                                    DataElements = new DataElement[]
                                    {
                                        new DataElement()
                                        {
                                            Name = "Related place/location two identification",
                                            Description = "The state or province code, postal abbreviations.",
                                            Usage = RuleUsage.Optional,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 25 }
                                        },

                                        new DataElement()
                                        {
                                            Name = "Code list qualifier. Allowed qualifiers:",
                                            Description = @"""163"" = Country sub-entity; state or province.",
                                            Usage = RuleUsage.Optional,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 3 }
                                        },

                                    }
                                }
                            }
                        },// ACA FINALIZA EL SEGMENTO LOC

                        new SegmentData(){

                            SegmentID = "DTM",
                            Name = "DATE/TIME/PERIOD (grp1)",
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
                                            Description = @"""178"" = actual date/time of arrival at senders port ""132"" = estimated date or date/time of arrival at the next port of call ""133"" = estimated date or date/time of departure at senders port ""136"" = actual date/time of departure at senders port",
                                            Usage = RuleUsage.Mandatory,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 3 }
                                        },

                                        new DataElement()
                                        {
                                            Name = "Date/Time/Period",
                                            Description = @"Date or date/time in local time when Means of Transport has arrived/departed or is expected to depart at the senders port or is expected to arrive at the next port of call.",
                                            Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 35 }
                                        },

                                        new DataElement()
                                        {
                                            Name = "Date/Time/Period Format Qualifier. Allowed qualifiers",
                                            Description = @"""101"" = YYMMDD ""201"" = YYMMDDHHMM ""301"" = YYMMDDHHMMZZZ (""ZZZ"" = Time zone, e.g. ""GMT"" or other)",
                                            Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 3 }
                                        }
                                    }
                                }
                            }
                        }, // ACA FINALIZA EL SEGMENTO DTM (grp1)

                        new SegmentData(){
                            SegmentID = "RFF",
                            Name = "REFERENCE (grp1)",
                            Position = 1,
                            DataElements = new Element[]
                            {

                                new CompositeElement()
                                {
                                    Name = "Reference",
                                    DataElements = new DataElement[]
                                    {

                                        new DataElement()
                                        {
                                            Name = "Reference Qualifier",
                                            Description = @"Code ""VON"" (Loading Voyage number, if different from the voyage number in the TDT-segment, assigned by the Operating Carrier or his agent to the voyage of the vessel).",
                                            Usage = RuleUsage.Mandatory,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 3 }
                                        },

                                        new DataElement()
                                        {
                                            Name = "Reference Number",
                                            Description = @"The Loading voyage number",
                                            Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 35 }
                                        }
                                    }
                                }
                            }
                        }, // ACA FINALIZA EL SEGMENTO RFF (grp1)
                    },

                },// ACA FINALIZA EL GRUPO 1

                new SegmentGroup(){
                    GroupRepeat = 99999,
                    Segments = new Segment[]
                    {
                        new SegmentData()
                        {
                            SegmentID = "TDT",
                            Name = "DETAILS OF TRANSPORT",
                            Usage = RuleUsage.Mandatory,
                            DataElements = new Element[]
                            {
                                new DataElement()
                                {
                                    Name = "Transport Stage Qualifier",
                                    Description = @"Code ""20"" (Main-carriage transport)",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new[] { 1, 3 }
                                },

                                new DataElement()
                                {
                                    Name = "MEANS OF TRANSPORT JOURNEY IDENTIFIER",
                                    Description = "Vessel operator's voyage number. Either this element or the Group 2 RFF must be used to specify a voyage number",
                                    Usage = RuleUsage.Dependent,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new[] { 1, 17 }
                                },

                                new EmptyElement(),
                                new EmptyElement(),

                                new CompositeElement()
                                {
                                    Name = "Code List",
                                    Description = "Carrier name, coded. Codes to be agreed or standard carrier alpha code (SCAC).",

                                    DataElements = new DataElement[]
                                    {
                                        new DataElement()
                                        {
                                            Name = "Carrier Identification",
                                            Description = "Carrier name, coded. Codes to be agreed or standard carrier alpha code (SCAC).",
                                            Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 17 }
                                        },

                                        new DataElement()
                                        {
                                            Name = "Code List Qualifier",
                                            Description = @"Code ""172"" (Carrier Code)",
                                            Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 3 }
                                        },

                                        new DataElement()
                                        {
                                            Name = "Code list responsible agency, coded. Allowed codes",
                                            Description = @"""20"" = BIC (Bureau International des Containeurs) ""166"" = US National Motor Freight Classification Association (SCAC) ""ZZZ"" = Mutually defined.",
                                            Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 3 }
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
                                        },

                                        new DataElement()
                                        {
                                            Name = "Nationality of Means of Transport",
                                            Description = "Coded according to UN-country code (ISO 3166).",
                                            Usage = RuleUsage.Optional,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 3 }
                                        }
                                    }

                                },
                            }
                        },// ACA FINALIZA EL SEGMENTO TDT

                        new SegmentData(){
                            SegmentID = "LOC",
                            Name = "PLACE/LOCATION IDENTIFICATION (grp2)",
                            Position = 1,
                            DataElements = new Element[]
                            {
                                new DataElement()
                                {
                                    Name = "Place/Location Qualifier",
                                    Description = @"Code ""147"" (Stowage Cell)",
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
                                            Name = "Location Identification",
                                            Description = @"The actual location of the equipment or cargo on the vessel. The following formats are allowed:1. ISO-format 2. Ro/Ro-format3. Other non-ISO-format (to be agreed between partners)1. ISO-format: Bay/Row/Tier (BBBRRTT). If Bay number is less than 3 characters it must be filled with leading zeroes, e.g. ""0340210"".2. Ro/Ro-format:Deck/Bay/Row/Tier (DDBBBRRTT).",
                                            Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 25 }
                                        },

                                        new DataElement()
                                        {
                                            Name = string.Empty,
                                            Description = string.Empty,
                                            Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] {0 , 0 }
                                        },

                                        new DataElement()
                                        {
                                            Name = "Code list responsible agency, coded. Allowed codes",
                                            Description = @"To indicate which format is used. Valid codes are:""5"" (ISO-format)""87"" (Ro/Ro-format, assigned by the Carrier""ZZZ"" (non-ISO-format, mutually defined).",
                                            Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 3 }
                                        }
                                    }
                                }
                            }
                        }, // ACA FINALIZA EL SEGMENTO LOC (grp2)

                        new SegmentData(){
                            SegmentID = "GID",
                            Name = "GOODS ITEM DETAILS (grp2)",
                            Position = 2,
                            DataElements = new Element[]
                            {
                                new EmptyElement(),

                                new CompositeElement()
                                {
                                    Name = "The number of packages",
                                    DataElements = new DataElement[]{
                                        new DataElement()
                                        {
                                            Name = "Number of packages",
                                            Description = @"Number of packages. The number of packages of non containerized cargo. If the cargo is Ro/Ro then the number ""1"" is used.",
                                            Usage = RuleUsage.Optional,
                                            DataType = DataType.Numeric,
                                            Precision = 8
                                        },

                                        new DataElement()
                                        {
                                            Name = "Type of packages identification",
                                            Description = @"Type of packages identification. Package type for non containerized cargo.",
                                            Usage = RuleUsage.Optional,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 17 }
                                        }

                                    }
                                }
                            }
                        }, // ACA FINALIZA EL SEGMENTO GID (grp2)

                        new SegmentData(){
                            SegmentID = "GDS",
                            Name = "NATURE OF CARGO (grp2)",
                            DataElements = new Element[]{
                                new DataElement(){
                                    Name = "Nature of cargo",
                                    Description = @"Nature of cargo, coded. Codes to be agreed between partners",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new[] { 1, 3 }
                                }
                            }
                        }, // ACA FINALIZA EL SEGMENTO GDS DEL GRUPO 2

                        new SegmentData(){
                            SegmentID = "FTX",
                            Name = "FREE TEXT (grp2)",

                            DataElements = new Element[]{
                                new DataElement(){
                                    Name = "Text Subject Qualifier",
                                    Description = @"Allowed qualifiers: ""AAA"" = Description of Goods""HAN"" = Handling Instructions""CLR"" = Container Loading Remarks""SIN"" = Special instructions""AAI"" = General information""AAY"" = Certification statements""ZZZ"" = Mutually defined use",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new[] { 1, 3 }
                                },

                                new EmptyElement(),

                                new DataElement(){
                                    Name = "Text reference",
                                    Description = @"In case of e4451 = AAY used for specification of data transmitted in c108. Use codes defined in SMDG codelist VGM.",
                                    Usage = RuleUsage.Optional,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new[] { 1, 3 }
                                },

                                new CompositeElement(){
                                    Name = "Description/Instructions/Remarks",
                                    DataElements = new Element[]{
                                        new DataElement(){
                                            Name = "Free Text",
                                            Description = @"Description/Instructions/Remarks in plain language or coded, for specific cargo/equipment. Codes, etc. to be agreed between partners.",
                                            Usage = RuleUsage.Mandatory,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 70 }
                                        },

                                        new DataElement(){
                                            Name = "Free Text 1",
                                            Description = @"Information about VGM according to code specified in c107.4441. (For details see page 42.)",
                                            Usage = RuleUsage.Optional,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 70 }
                                        },

                                        new DataElement(){
                                            Name = "Free Text 2",
                                            Description = @"Information about VGM according to code specified in c107.4441. (For details see page 42.)",
                                            Usage = RuleUsage.Optional,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 70 }
                                        },

                                        new DataElement(){
                                            Name = "Free Text 3",
                                            Description = @"Information about VGM according to code specified in c107.4441. (For details see page 42.)",
                                            Usage = RuleUsage.Optional,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 70 }
                                        },

                                        new DataElement(){
                                            Name = "Free Text 4",
                                            Description = @"Information about VGM according to code specified in c107.4441. (For details see page 42.)",
                                            Usage = RuleUsage.Optional,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 70 }
                                        }
                                    }
                                }
                            }
                        }, // ACA FINALIZA EL SEGMENTO FTX DEL GRUPO 2

                        new SegmentData(){
                            SegmentID = "MEA",
                            Name = "MEASUREMENTS (grp2)",
                            DataElements = new Element[]{
                                new DataElement(){
                                    Name = "Measurement Application Qualifier",
                                    Description = @"Allowed qualifiers: ""WT"" (gross weight / gross mass) – not confirmed as verified ""VGM"" (verified gross mass) – specified weight is verified[code VGM has been introduced in D.15B for data element 6313]",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new[] { 1, 3 }
                                },

                                new EmptyElement(),

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
                                            Description = @"The gross mass of the transport equipment including cargo in kilograms or pounds, as qualified (no decimals)",
                                            Usage = RuleUsage.Required,
                                            DataType = DataType.Numeric,
                                            Precision = new []{1, 18}
                                            // Precision = 18 //LONGITUD EXACTA SI O SI TIENE QUE TENER UNA LOGITUD DE 18
                                            
                                        }
                                    }
                               }
                            }
                        },

                        new SegmentData()
                        {
                            SegmentID = "DIM",
                            Name = "DIMENSIONS (grp2)",
                            DataElements = new Element[]
                            {
                                new DataElement()
                                {
                                    Name = "Dimension Qualifier",
                                    Description = @"Code ""1"" = Gross dimensions (break-bulk) Code ""5"" = Off-standard dims. (over-length front)Code ""6"" = Off-standard dims. (over-length back)Code ""7"" = Off-standard dims. (over-width right)Code ""8"" = Off-standard dims. (over-width left)Code ""9"" = Off-standard dims. (over-height)Code ""10"" = external equipment dimensions (Non-ISO equipment)Basically allowed qualifier ""1"" for break-bulk cargo and from ""5"" to ""9"" for odd-sized-cargo. However allowed from ""5"" to ""9"" for break-bulk cargo as additional information, if required.",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new[] { 1, 3 }
                                },

                                new CompositeElement(){
                                    Name = "Measure Unit",
                                    DataElements = new Element[]{
                                new DataElement()
                                {
                                    Name = "Measure Unit Qualifier",
                                    Description = @"Allowed qualifiers: ""CMT"" = Centimeters = preferred ""INH"" = Inches",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new[]{ 1, 3 }
                                },

                                new DataElement()
                                {
                                    Name = "Length Dimension",
                                    Description = @"Break-bulk length or over-length for containers, as qualified.",
                                    Usage = RuleUsage.Dependent,
                                    DataType = DataType.Numeric,
                                    Precision = 15
                                },

                                new DataElement()
                                {
                                    Name = "Width Dimension",
                                    Description = @"Break-bulk width or over-width for containers, as qualified.",
                                    Usage = RuleUsage.Dependent,
                                    DataType = DataType.Numeric,
                                    Precision = 15
                                },

                                new DataElement()
                                {
                                    Name = "Height Dimension",
                                    Description = @" Break-bulk height or over-height for containers, as qualified",
                                    Usage = RuleUsage.Dependent,
                                    DataType = DataType.Numeric,
                                    Precision = 15
                                }
                                }
                            }
                        }
                        }, // ACA FINALIZA EL SEGMENTO DEL GRUPO 2 

                        new SegmentData()
                        {
                            SegmentID = "TMP",
                            Name = "TEMPERATURE (grp2)",
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
                                    Name = "Temperature Setting",
                                    Description = "Actual temperature according to Reefer List (no deviation allowed) at which the cargo is to be transported. For field format see remarks below.",
                                    Usage = RuleUsage.Required,
                                    DataType = DataType.Decimal,
                                    Precision = 3
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
                        }, // ACA FINALIZA EL SEGMENTO TMP DEL GRUPO 2

                        new SegmentData()
                        {
                            SegmentID = "RNG",
                            Name = "RANGE DETAILS (grp2)",
                            DataElements = new Element[]
                            {
                                new DataElement()
                                {
                                    Name = "Range Type Qualifier",
                                    Description = @" Allowed qualifier: ""4"" = Quantity range.",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new[] { 1, 3 }
                                },

                                new CompositeElement(){
                                    Name = "Unit Qualifier",
                                    DataElements = new Element[]{
                                new DataElement()
                                {
                                    Name = "Measure Unit Qualifier",
                                    Description = @"""CEL"" = degrees Celsius ""FAH"" = degrees Fahrenheit",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = 3
                                },

                                new DataElement()
                                {
                                    Name = "Range Minimum",
                                    Description = @"Minimum temperature according to Reefer List at which the cargo is to be transported.",
                                    Usage = RuleUsage.Required,
                                    DataType = DataType.Numeric,
                                    Precision = 18
                                },

                                new DataElement()
                                {
                                    Name = "Range Maximum",
                                    Description = @"Maximum temperature according to Reefer List at which the cargo is to be transported.",
                                    Usage = RuleUsage.Required,
                                    DataType = DataType.Numeric,
                                    Precision = 18
                                }
                            }
                            }
                            }
                        }, // ACA FINALIZA EL SEGMENTO RGN DEL GRUPO 2

                        new SegmentData(){
                            SegmentID = "LOC",
                            Name = "PLACE/LOCATION IDENTIFICATION (grp2)",
                            DataElements = new Element[]
                            {
                                new DataElement()
                                {
                                    Name = "Place/Location Qualifier",
                                    Description = @"""9"" = Place/Port of Loading""11"" = Place/Port of discharge""13"" = Transshipment port/Place of transshipment""64"" = 1st optional port of discharge""68"" = 2nd optional port of discharge""70"" = 3rd optional port of discharge""76"" = Original port of loading""83"" = Place of delivery (to be used as final destination or double stack train destination).""97"" = Optional place/port of discharge. To be used if actual port of discharge is undefined, i.e. ""XXOPT"".""152"" = Next port of discharge",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new[] { 1, 3 }
                                },

                                new CompositeElement(){
                                    Name = "Namecode of the place/port, as qualified",
                                    DataElements = new Element[]{
                                new DataElement()
                                {
                                    Name = "Place/Location Identification",
                                    Description = @"Allowed code lists: UN-Locode or US-Census codes.""Sample codes: JPTYO = TokyoUSLAX = Los AngelesUSOAK = OaklandUSSEA = SeattleUSCHI = ChicagoFor optional port of discharge: ""XXOPT"" (Qualifier e3227: ""97"").",
                                    Usage = RuleUsage.Required,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 25}
                                },

                                new DataElement()
                                {
                                    Name = "Code list qualifier. Allowed qualifiers",
                                    Description = @"""139"" = Port",
                                    Usage = RuleUsage.Optional,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 3 }
                                },

                                new DataElement()
                                {
                                    Name = "Code list responsible agency, coded. Allowed codes",
                                    Description = @"""112"" = US, US Census Bureau, Schedule D for U S locations,Schedule K for foreign port locations.""6"" = UN/ECE - United Nations - Economic Commission forEurope. (UN-Locodes).""ZZZ"" = Optional ports.",
                                    Usage = RuleUsage.Optional,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 3 }
                                }
                            }
                            },

                            new CompositeElement(){
                                    Name = "Related place/location one identification (grp2)",
                                    DataElements = new Element[]{
                                new DataElement()
                                {
                                    Name = "Related place/location one identification",
                                    Description = @"The name code of the Container Terminal in the port ofdischarge or the port of loading. Terminal codes to be used as defined in SMDG’s Master Terminal Facilities code list.",
                                    Usage = RuleUsage.Required,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 25}
                                },

                                new DataElement()
                                {
                                    Name = "Code list qualifier. Allowed qualifier",
                                    Description = @"""TER"" = TERMINALS (leading 3 characters due to limited size)",
                                    Usage = RuleUsage.Optional,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 3 }
                                },

                                new DataElement()
                                {
                                    Name = "Code list responsible agency, coded. Allowed codes",
                                    Description = @" ""306"" = SMDG (code 306 is defined in D.02B and later)",
                                    Usage = RuleUsage.Optional,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 3 }
                                }
                            }
                            }

                            }
                        }, // ACA FINALIZA EL SEGMENTO LOC DEL GRUPO 2

                        new SegmentData(){
                            SegmentID = "RFF",
                            Name = "REFERENCE (grp2)",
                            DataElements = new Element[]
                            {
                                new CompositeElement(){
                                    Name = "Reference Qualifier: Allowed qualifiers: (grp2)",
                                    DataElements = new Element[]{
                                new DataElement()
                                {
                                    Name = "Reference Qualifier: Allowed qualifiers:",
                                    Description = @"""BM"" = B/L-number. ""BN"" = Booking reference number.""ET"" = Excess Transportation Number to be used for leading Stowage position, in case of Break-bulk or odd-sized-cargo. ""ZZZ"" = Mutually defined.",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 3}
                                },

                                new DataElement()
                                {
                                    Name = @"Reference Number: For Qualifiers ""BM"", ""BN"" or ""ZZZ"":",
                                    Description = @"Dummy value ""1"" or the actual Bill of Lading number resp. Booking Reference number, as agreed. For Qualifier ""ET"": leading stowage location, containing relevant data for this consignment.",
                                    Usage = RuleUsage.Required,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 35 }
                                }
                            }
                            }
                            }
                        } // ACA FINALIZA EL SEGMENTO RFF DEL GRUPO 2

                    }, // ACA FINALIZA LOS SEGMENTOS GRUPO 2
                    
                }, //ACA FINALIZA EL GRUPO 2

                 new SegmentGroup(){
                        GroupRepeat = 9,
                        Segments = new Segment[]
                        {
                            new SegmentData(){
                                SegmentID = "EQD",
                                Name = "EQUIPMENT DETAILS (grp3)",
                                DataElements = new Element[]
                                {
                                   new DataElement()
                                {
                                    Name = "Equipment Qualifier",
                                    Description = @" ""CN"" = Container ""BB"" = Break-bulk ""TE"" = Trailer ""ZZZ"" = Ro/Ro or otherwise",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 3}
                                },

                                new DataElement()
                                {
                                    Name = @"Equipment Identification Number",
                                    Description = @"1. The container number: Format: One continuous string with the identification, prefixand number. Examples: SCXU 2387653 must be transmitted as ""SCXU2387653"", EU 876 must be transmitted as ""EU876"". The number will be treated as a character string. E.g. alphanumeric check-digits can be transmitted here. If this segment is used the unique equipment identification number must always be transmitted, although this element is not mandatory! 2. Break-bulk: The break-bulk reference number. The assigned break-bulk reference numbers must be agreed between partners. 3. Otherwise (Ro/Ro): The equipment identification number.",
                                    Usage = RuleUsage.Required,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 17 }
                                },

                                new DataElement()
                                {
                                    Name = @"Equipment Size and Type Identification",
                                    Description = @"ISO size-type code of 4 digits (ISO 6346). Leave blank in case of break-bulk. For unknown ISO size/type codes the following codes can beagreed between partners ""9999"" = No information at all. ""4999"" = Length = 40ft, rest unknown""2999"" = Length = 20ft, rest unknown""4299"" = 40ft ""8'6"", rest unknown""2299"" = 20ft ""8'6"", rest unknown""4099"" = 40ft ""8'0"", rest unknown ""2099"" = 20ft ""8'0"", rest unknownOther codes to be agreed between partners.",
                                    Usage = RuleUsage.Dependent,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 4 }
                                },

                               // new EmptyElement(),

                                //  new DataElement()
                                // {
                                //     Name = @"Equipment status, coded",
                                //     Description = @"1: Continental 11: Direct delivery 2: Export 12: Bond transport 3: Import 13: Transship to other vesse 4: Remain on board 14: Transship to other pier 5: Shifter 15: Rail road transport 6: Transshipment 16: Road transport 7: Hot delivery 17: Barge transport 8: MLB 18: Temporary stowage 9: MCB (Micro Land Bridge) 19: Urgent unpacking 10: Canada Bound transport 20: Sea & Air",
                                //     Usage = RuleUsage.Optional,
                                //     DataType = TypeCode.String,
                                //     Precision = new []{ 1, 3 }
                                // },

                                new EmptyElement(),
                                new EmptyElement(),

                                new DataElement()
                                {
                                    Name = @"Full/Empty Indicator, coded. Allowed codes",
                                    Description = @"""5"" = Full ""4"" = Empty. Leave blank in case of break-bulk.",
                                    Usage = RuleUsage.Dependent,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 3 }
                                }
                                }
                            }, // ACA FINALIZA EL SEGMENTO EQD DEL GRUPO 3

                            new SegmentData(){
                                SegmentID = "EQA",
                                Name = "EQUIPMENT ATTACHED (grp3)",
                                DataElements = new Element[]
                                {
                                   new DataElement()
                                {
                                    Name = "Equipment Qualifier: Allowed qualifiers",
                                    Description = @" ""RG"" = Reefer Generator ""CN"" = Container ""CH"" = Chassis",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 3}
                                },

                                new DataElement()
                                {
                                    Name = @"Equipment Identification Number",
                                    Description = @"The unit number",
                                    Usage = RuleUsage.Required,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 17 }
                                }

                                }
                            }, // ACA FINALIZA EL SEGMENTO EQA DEL GRUPO 3

                            new SegmentData(){
                            SegmentID = "NAD",
                            Name = "NAME AND ADDRESS (grp3)",
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
                                    Name = "Party Id Identification (grp3)",
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
                            }
                            }
                        } // ACA FINALIZA EL SEGMENTO NAD DEL GRUPO 3

                        }
                    }, // ACA FINALIZA EL GRUPO 3

                    new SegmentGroup(){
                        GroupRepeat = 9999,
                        Segments = new Segment[]
                        {
                            new SegmentData(){
                                SegmentID = "DGS",
                                Name = "DANGEROUS GOODS (grp4)",
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
                                    Name = "Hazard Code Identification (grp4)",
                                    DataElements = new Element[]{
                                new DataElement()
                                {
                                    Name = "Hazard Code Identification",
                                    Description = @"IMDG Code, e.g. ""1.2"" or ""8""",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 7}
                                },

                                new DataElement()
                                {
                                    Name = @"Hazard Substance/item/page number",
                                    Description = @"The IMDG code page number (English version).",
                                    Usage = RuleUsage.Optional,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 7 }
                                }
                            }
                            },

                            new DataElement()
                            {
                                Name = @"UNDG Number",
                                Description = @"UN number of respective dangerous cargo transported (4 digits)",
                                Usage = RuleUsage.Optional,
                                DataType = DataType.Numeric,
                                Precision = 4
                            },

                            new CompositeElement(){
                                    Name = "Shipment Flashpoint (grp4)",
                                    DataElements = new Element[]{
                                new DataElement()
                                {
                                    Name = "Shipment Flashpoint",
                                    Description = @"the actual flashpoint in degrees Celsius or Fahrenheit. For inserting temperatures below zero or tenth degrees please refer to remarks under TMP-segment respectively to ISO 9735. If different dangerous goods with different flashpoints within one load to be transported, only the lowest flashpoint should be inserted",
                                    Usage = RuleUsage.Optional,
                                    DataType = DataType.Numeric,
                                    Precision = 3
                                },

                                new DataElement()
                                {
                                    Name = @"Measure Unit Qualifier",
                                    Description = @"Allowed qualifiers: ""CEL"" (degrees Celsius) = Preferred ""FAH"" (degrees Fahrenheit)",
                                    Usage = RuleUsage.Optional,
                                    DataType = DataType.Decimal,
                                    Precision = 3
                                }
                            }
                            },

                            new DataElement()
                            {
                                Name = @"Packing group, coded",
                                Description = @"The packing group code of the hazardous goods.",
                                Usage = RuleUsage.Optional,
                                DataType = DataType.Alphanumeric,
                                Precision = 3
                            },

                            new DataElement()
                            {
                                Name = @"EMS number",
                                Description = @"Emergency schedule number.",
                                Usage = RuleUsage.Optional,
                                DataType = DataType.Alphanumeric,
                                Precision = 6
                            },

                            new DataElement()
                            {
                                Name = @"MFAG",
                                Description = @"Medical First Aid Guide number",
                                Usage = RuleUsage.Optional,
                                DataType = DataType.Alphanumeric,
                                Precision = 4
                            },

                            new EmptyElement(),

                            new CompositeElement(){
                                    Name = "Hazard Identification number, upper part (grp4)",
                                    DataElements = new Element[]{
                                new DataElement()
                                {
                                    Name = "Hazard Identification number",
                                    Description = @"Hazard Identification number, upper part",
                                    Usage = RuleUsage.Optional,
                                    DataType = DataType.Alphanumeric,
                                    Precision = 4
                                },

                                new DataElement()
                                {
                                    Name = @"Substance Identification number",
                                    Description = @"Substance Identification number, lower part",
                                    Usage = RuleUsage.Optional,
                                    DataType = DataType.Alphanumeric,
                                    Precision = 4
                                }
                            }
                            },

                            new CompositeElement(){
                                    Name = "Dangerous Goods Label Marking (grp4)",
                                    DataElements = new Element[]{
                                new DataElement()
                                {
                                    Name = "Dangerous Goods Label Marking (1)",
                                    Description = @"See below for possible use of this data element",
                                    Usage = RuleUsage.Optional,
                                    DataType = DataType.Alphanumeric,
                                    Precision = 4
                                },

                                new DataElement()
                                {
                                    Name = @"Dangerous Goods Label Marking (2)",
                                    Description = @"Dangerous Goods Label Marking (2)",
                                    Usage = RuleUsage.Optional,
                                    DataType = DataType.Alphanumeric,
                                    Precision = 4
                                },

                                new DataElement()
                                {
                                    Name = @"Dangerous Goods Label Marking (3)",
                                    Description = @"Dangerous Goods Label Marking (3)",
                                    Usage = RuleUsage.Optional,
                                    DataType = DataType.Alphanumeric,
                                    Precision = 4
                                }
                            }
                            }

                            }
                        }, // ACA FINALIZA EL SEGMENTO DGS DEL GRUPO 4

                            new SegmentData(){
                                SegmentID = "FTX",
                                Name = "FREE TEXT (grp4)",
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
                                    Name = "Free text (grp4)",
                                    DataElements = new Element[]{
                                new DataElement()
                                {
                                    Name = "Free text",
                                    Description = @"Description of hazard material in plain language. One element of maximum 70 characters to be given only for the description. Transmit the text ""NIL"", if no description is available and one or both of the following data elements must be transmitted.",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new [] {1, 70}
                                },

                                new DataElement()
                                {
                                    Name = @"Free text 2: (grp4)",
                                    Description = @"The net weight in kilos of the hazardous material to be transmitted here.",
                                    Usage = RuleUsage.Optional,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{1, 70}
                                },

                                new DataElement()
                                {
                                    Name = @"Free text 3: (grp4)",
                                    Description = @"The DG-reference number as allocated by the central planner, if known.",
                                    Usage = RuleUsage.Optional,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{1, 70}
                                }
                            }
                            }

                                }
                            }, // ACA FINALIZA EL SEGMENTO FTX DEL GRUPO 4

                            new SegmentData(){
                            SegmentID = "UNT",
                            Name = "MESSAGE TRAILER",
                            DataElements = new Element[]
                            {
                                new DataElement()
                                {
                                    Name = @"Number of segments in the message",
                                    Description = @"Number of segments in the message, including UNH and UNT segments, but excluding UNA, UNB and UNZ segments.",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Numeric,
                                    Precision = new []{1, 6}
                                },

                                new DataElement()
                                {
                                    Name = "Message reference number",
                                    Description = @"This reference must be identical to the reference in the UNH-segment (e0062).",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 14}
                                }

                            }
                        }, // ACA FINALIZA EL SEGMENTO UNT DEL GRUPO 4

                        new SegmentData(){
                            SegmentID = "UNZ",
                            Name = "INTERCHANGE TRAILER",
                            DataElements = new Element[]
                            {
                                new DataElement()
                                {
                                    Name = @"Interchange Control Count",
                                    Description = @"The number of messages in the interchange.",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Numeric,
                                    Precision = new []{1, 6}
                                },

                                new DataElement()
                                {
                                    Name = "Interchange Control Reference",
                                    Description = @"This reference must be identical to the reference in the UNB-segment (e0020).",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 14}
                                }

                            }
                        } // ACA FINALIZA EL SEGMENTO UNZ DEL GRUPO 4

                        }
                    } // ACA FINALIZA EL GRUPO 4

};

// Console.WriteLine(definition);

// Lee el archivo EDI
// string filePath = @"C:\Users\mbermudez\Documents\ReadBaplie\ReadEDIFACT\BAPLIE_Export.EDI";
string filePath = @"C:\Users\mbermudez\Documents\ReadBaplie\ReadEDIFACT\MOVINS DMR V09.edi";
// string filePath = @"C:\Users\mbermudez\Documents\ReadBaplie\ReadEDIFACT\DEPARTURE BAPLIE FINAL - DEL MONTE ROSE - 25360187.edi";
// string filePath = @"C:\Users\mbermudez\OneDrive - JAPDEVA\Proyecto PortLogistics\EDI BAPLEY\Ejemplos\BAPLIE\CRMOB DEP DMG V42.edi";

string outputFilePath = @"C:\Users\mbermudez\Documents\ReadBaplie\ReadEDIFACT\MOVINS DMR V09.json";
// string outputFilePath = @"C:\Users\mbermudez\Documents\ReadBaplie\ReadEDIFACT\DEPARTURE BAPLIE FINAL - DEL MONTE ROSE - 25360187.json";
// string outputFilePath = @"C:\Users\mbermudez\Documents\ReadBaplie\ReadEDIFACT\BAPLIE_Export.json";

using (var reader = new StreamReader(filePath))
{
    try
    {
        var parser = new ParserEDI(reader, definition);

        // Validar el archivo EDI
        var validationErrors = parser.ValidateFullEDI();
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
            parser.SaveJsonToFile(outputFilePath);
            Console.WriteLine("Archivo JSON guardado en: " + outputFilePath);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error al procesar el archivo EDI: {ex.Message}");
    }
}

using (var sw = new StreamWriter(string.Format(@"C:\Users\mbermudez\Documents\ReadBaplie\ReadEDIFACT\{0}Output.txt", definition.Name)))
{
    sw.Write(definition);
}

// Console.WriteLine();
// Console.WriteLine("Press any key to exit...");

// Console.ReadKey();