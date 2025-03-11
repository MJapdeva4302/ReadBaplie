using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadEDIFACT.Models
{
    public class BaplieVersion3 : FileDefinition
    {
        public BaplieVersion3() : base("BAPLIE", new Version(3, 1, 1))
        {


            SegmentSeparator = '\'';
            ElementSeparator = '+';
            DataElementSeparator = ':';
            EscapeCharacter = '?';

            Segments = new Segment[]
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
                                    Description = @"The applicable SMDG User Manual version number. For this manual use: ""SMDG31"". This will enable the recipient of the message to translate the message correctly, even if older versions are still in use.",
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

                        new CompositeElement(){
                            Name = "DOCUMENT/MESSAGE NAME",
                            DataElements = new DataElement[]
                        {
                            new DataElement()
                            {
                                Name = "Document type code",
                                Description = "658 = Bayplan/stowage plan, full. 659 = Bayplan/stowage plan, partial",
                                Usage = RuleUsage.Required,
                                DataType = DataType.Alphanumeric,
                                Precision = new[] { 1, 3 }
                            },

                            new DataElement()
                            {
                                Name = string.Empty,
                                Description = string.Empty,
                                Usage = 0,
                                DataType = 0,
                                Precision = new[] { 0, 0 }
                            },

                            new DataElement()
                            {
                                Name = string.Empty,
                                Description = string.Empty,
                                Usage = 0,
                                DataType = 0,
                                Precision = new[] { 0, 0 }
                            },

                            new DataElement()
                            {
                                Name = "Document name",
                                Description = "Name of a document. Dependency: Required in case of partial bayplan (C002.1001 = 659): Codes to be transmitted: SINGLEOP - bayplan contains only stowage location used by selected operator LOADONLY - bayplan contains only stowage locations whose content changed in current port",
                                Usage = RuleUsage.Required,
                                DataType = DataType.Alphanumeric,
                                Precision = new[] { 1, 35 }
                            }

                        }
                        },
                        new EmptyElement(),
                        new EmptyElement(),
                        new EmptyElement(),

                        new DataElement()
                        {
                            Name = "DOCUMENT STATUS CODE ",
                            Description = @"Code specifying the status of a document.",
                            Usage = RuleUsage.Required,
                            DataType = DataType.Alphanumeric,
                            Precision = new[] { 1, 3 }
                        }

                    }
                },// ACA FINALIZA EL SEGMENTO BGM 

                new SegmentGroup()
                {
                    GroupRepeat = 1,
                    Segments = new Segment[]
                    {

                        new SegmentData(){

                            SegmentID = "DTM",
                            Name = "DATE/TIME/PERIOD (grp1)",
                            Notes = "1",
                            Usage = RuleUsage.Mandatory,
                            Position = 99,
                            DataElements = new Element[]
                            {

                                new DataElement()
                                {
                                    Name = "Date or time or period function code qualifier",
                                    Description = @"Code qualifying the function of a date, time or period. 171 Reference date/time",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new[] { 1, 3 }
                                },

                                new CompositeElement()
                                {
                                    Name = "DATE/TIME/PERIOD",
                                    DataElements = new DataElement[]
                                    {

                                        new DataElement()
                                        {
                                            Name = "Date or time or period text",
                                            Description = @"The value of a date, a date and time, a time or of a period in a specified representation.",
                                            Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 35 }
                                        },

                                        new DataElement()
                                        {
                                            Name = "Date/Time/Period Format Qualifier. Allowed qualifiers",
                                            Description = @"Code specifying the representation of a date, time or period. 203 = CCYYMMDDHHMM 303 = CCYYMMDDHHMMZZZ",
                                            Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 3 }
                                        }
                                    }
                                }
                            }
                        }, // ACA FINALIZA EL SEGMENTO DTM (grp1)

                    },

                },// ACA FINALIZA EL GRUPO 1

                new SegmentGroup(){
                    GroupRepeat = 99999,
                    Segments = new Segment[]
                    {

                        new SegmentData(){
                            SegmentID = "GID",
                            Name = "GOODS ITEM DETAILS (grp2)",
                            Notes = "2",
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
                            Notes = "2",
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

                        new SegmentData()
                        {
                            SegmentID = "DIM",
                            Name = "DIMENSIONS (grp2)",
                            Notes = "2",
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
                            Notes = "2",
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
                            Notes = "2",
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
                            Notes = "2",
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

                    }, // ACA FINALIZA LOS SEGMENTOS GRUPO 2
                    
                }, //ACA FINALIZA EL GRUPO 2

                 new SegmentGroup(){
                        GroupRepeat = 9,
                        Segments = new Segment[]
                        {

                            new SegmentData(){
                            SegmentID = "CTA",
                            Name = "Contact Information (grp3)",
                            Notes = "3",
                            DataElements = new Element[]
                            {
                                new DataElement()
                                {
                                    Name = "CONTACT FUNCTION CODE",
                                    Description = @"Code specifying the function of a contact (e.g. department or person). AH = Coordination contact",
                                    Usage = RuleUsage.Required,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 3}
                                },

                                new CompositeElement(){
                                    Name = "CONTACT DETAILS: (grp3)",
                                    DataElements = new Element[]{
                                new DataElement()
                                {
                                    Name = "Contact identifier",
                                    Description = @"To identify a contact, such as a department or employee.",
                                    Usage = RuleUsage.Optional,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 17}
                                },

                                new DataElement()
                                {
                                    Name = @"Contact name",
                                    Description = @"Name of a contact, such as a department or employee.",
                                    Usage = RuleUsage.Optional,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 256 }
                                }
                            }
                            }
                            }
                        }, // ACA FINALIZA EL SEGMENTO CTA DEL GRUPO 3
                        new SegmentData(){
                            SegmentID = "COM",
                            Name = "Communication Contact (grp3)",
                            Notes = "3",
                            DataElements = new Element[]
                            {

                                new CompositeElement(){
                                    Name = "COMMUNICATION CONTACT: (grp3)",
                                    Description = "Communication number of a department or employee in a specified channel.",
                                    DataElements = new Element[]{
                                new DataElement()
                                {
                                    Name = " Communication address identifier",
                                    Description = @"To identify a communication address.",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 512}
                                },

                                new DataElement()
                                {
                                    Name = @"Communication means type code",
                                    Description = @"Code specifying the type of communication address. AL = Cellular phone AV = Inmarsat call number EM = Electronic mail FX = Telefax MA = Mail TE = Telephone",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 3 }
                                }
                            }
                            }
                            }
                        }, // ACA FINALIZA EL SEGMENTO COM DEL GRUPO 3

                            new SegmentData(){
                                SegmentID = "EQA",
                                Name = "EQUIPMENT ATTACHED (grp3)",
                                Notes = "3",
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

                        }
                    }, // ACA FINALIZA EL GRUPO 3

                    new SegmentGroup(){
                        GroupRepeat = 9999,
                        Segments = new Segment[]
                        {

                            new SegmentData()
                        {
                            SegmentID = "TDT",
                            Name = "Transport Information",
                            Notes = "4",
                            Usage = RuleUsage.Mandatory,
                            DataElements = new Element[]
                            {
                                new DataElement()
                                {
                                    Name = "Transport Stage Code Qualifier",
                                    Description = @"Code ""20"" (Main Carriage)",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new[] { 1, 3 }
                                },

                                new DataElement()
                                {
                                    Name = "MEANS OF TRANSPORT JOURNEY IDENTIFIER",
                                    Description = "To identify a journey of a means of transport. Discharge voyage number as assigned by the vessel operator or his agent.",
                                    Usage = RuleUsage.Required,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new[] { 1, 17 }
                                },

                                new EmptyElement(),
                                new EmptyElement(),

                                new CompositeElement()
                                {
                                    Name = "CARRIER",
                                    Description = "Identification of a carrier by code and/or by name. Code preferred.",

                                    DataElements = new DataElement[]
                                    {
                                        new DataElement()
                                        {
                                            Name = "Carrier Identification",
                                            Description = "To identify a carrier.",
                                            Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 17 }
                                        },

                                        new DataElement()
                                        {
                                            Name = "Code list identification code",
                                            Description = @"Code identifying a user or association maintained code list. LINES = SMDG code list for liner codes",
                                            Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 3 }
                                        },

                                        new DataElement()
                                        {
                                            Name = "Code list responsible agency code",
                                            Description = @"Code specifying the agency responsible for a code list. 306 = SMDG (Ship-planning Message Design Group)",
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
                                            Name = "Transport means identification name identifier",
                                            Description = @"Identifies the name of the transport means. Preferably specify IMO-number specified by Lloyd's register of shipping - C222.1131=IMO, C222.3055=11 Alternately specify call sign specified by ITU - C222.131=CALLSIGN, C222.3055=296",
                                            Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 9 }
                                        },

                                        new DataElement()
                                        {
                                            Name = "Code list identification code",
                                            Description = @"Code identifying a user or association maintained code list. CALLSIGN = radio communications call sign IMO = IMO number, unique identifier registered by Lloyd's Register of Shipping",
                                            Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 3 }
                                        },

                                        new DataElement()
                                        {
                                            Name = "Code list responsible agency code",
                                            Description = @"Code specifying the agency responsible for a code list. 11 Lloyd's register of shipping 296 ITU (International Telecommunication Union)",
                                            Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 3 }
                                        },

                                        new DataElement()
                                        {
                                            Name = "Transport means identification name",
                                            Description = "Name identifying a means of transport.",
                                            Usage = RuleUsage.Optional,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 35 }
                                        },

                                    }

                                },
                            }
                        },// ACA FINALIZA EL SEGMENTO TDT DEL GRUPO 4

                        new SegmentData(){
                            SegmentID = "RFF",
                            Name = "REFERENCE (grp4)",
                            Notes = "4",
                            DataElements = new Element[]
                            {
                                new CompositeElement(){
                                    Name = "Reference code qualifier: (grp4)",
                                    DataElements = new Element[]{
                                new DataElement()
                                {
                                    Name = "Reference Qualifier: Allowed qualifiers:",
                                    Description = @"Code qualifying a reference. Loading voyage number VON = Voyage number",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 3}
                                },

                                new DataElement()
                                {
                                    Name = @"Reference identifier",
                                    Description = @"Identifies loading voyage number.. loading voyage number as assigned by vessel operator or his agent",
                                    Usage = RuleUsage.Required,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 70 }
                                }
                            }
                            }
                            }
                        }, // ACA FINALIZA EL SEGMENTO RFF DEL GRUPO 4

                            new SegmentData(){
                                SegmentID = "DGS",
                                Name = "DANGEROUS GOODS (grp4)",
                                Notes = "4",
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
                                Notes = "4",
                                DataElements = new Element[]
                                {
                                   new DataElement()
                                {
                                    Name = "Text Subject Qualifier.",
                                    Description = @"Code qualifying the subject of the text. Refer to D.13B Data Element Dictionary for acceptable code values.",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 3}
                                },

                                new EmptyElement(),
                                new EmptyElement(),


                                new DataElement()
                                {
                                    Name = "Free text",
                                    Description = @"Free form text.",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new [] {1, 512}
                                }

                                }
                            }, // ACA FINALIZA EL SEGMENTO FTX DEL GRUPO 4

                        }
                    }, // ACA FINALIZA EL GRUPO 4

                    new SegmentGroup(){
                        GroupRepeat = 9,
                        Segments = new Segment[]{
                            new SegmentData()
                        {
                            SegmentID = "LOC",
                            Name = "PLACE/LOCATION IDENTIFICATION (grp5)",
                            Notes = "5",
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
                                            Description = "To identify a location. Always use UN/locodes as defined by http://www.unece.org/cefact/locode/service/location.html",
                                            Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 25 }
                                        }
                                    }
                                },

                                new CompositeElement(){
                                    Name = "RELATED LOCATION ONE IDENTIFICATION",
                                    DataElements = new DataElement[]
                                    {
                                        new DataElement()
                                        {
                                            Name = "First related location identifier",
                                            Description = "To identify a first related location.",
                                            Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 35 }
                                        },

                                        new DataElement()
                                        {
                                            Name = "Code list identification code",
                                            Description = @"Code identifying a user or association maintained code list. TERMINALS SMDG code list for terminal facilities",
                                            Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 17 }
                                        },

                                        new DataElement()
                                        {
                                            Name = "Code list responsible agency code",
                                            Description = @"Code specifying the agency responsible for a code list. 306 = SMDG (Ship-planning Message Design Group)",
                                            Usage = RuleUsage.Optional,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 3 }
                                        }
                                    }
                                }
                            }
                        },// ACA FINALIZA EL SEGMENTO LOC DEL GRUPO 5

                        new SegmentData()
                {
                    SegmentID = "DTM",
                    Name = "Date - Time - Period",
                    Usage = RuleUsage.Mandatory,
                    DataElements = new Element[]
                    {
                        new CompositeElement()
                        {
                            Name = "DATE/TIME/PERIOD",
                            Description = "Date and/or time, or period relevant to the specified date/time/period type. It is recommended to transmit date and time as UTC.",
                            DataElements = new DataElement[]
                            {

                                new DataElement()
                                {
                                    Name = "Date or time or period function code qualifier",
                                    Description = @"Code qualifying the function of a date, time or period. 132 = Transport means arrival date time, estimated 133 = Transport means departure date/time, estimated 136 = Transport means departure date time, actual 178 = Transport means arrival date time, actual",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new[] { 1, 3 }
                                },

                                new DataElement()
                                {
                                    Name = "Date or time or period text",
                                    Description = "The value of a date, a date and time, a time or of a period in a specified representation.",
                                    Usage = RuleUsage.Optional,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new[] { 1, 35 }
                                },

                                new DataElement()
                                {
                                    Name = "Date or time or period format code",
                                    Description = @"Code specifying the representation of a date, time or period. Use of codes specifying time with time zone (205, 303) need to be  bilaterally agreed between partners. 102 = CCYYMMDD 203 = CCYYMMDDHHMM 205 = CCYYMMDDHHMMZHHMM 303 = CCYYMMDDHHMMZZZ",
                                    Usage = RuleUsage.Optional,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new[] { 1, 3 }
                                }

                            }
                        }
                    }
                },// ACA FINALIZA EL SEGMENTO DTM DEL GRUPO 5
                        }
                    }, // ACA FINALIZA EL GRUPO 5

                    new SegmentData(){
                            SegmentID = "UNS",
                            Name = "Section Control",
                            Notes = "4",
                            DataElements = new Element[]
                            {
                                new DataElement()
                                {
                                    Name = @"SECTION IDENTIFIER",
                                    Description = @"A character identifying the next section in a message. D = Header/detail section separation",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Numeric,
                                    Precision = 1
                                }

                            }
                        }, // ACA FINALIZA EL SEGMENTO UNS

                        new SegmentGroup(){
                            GroupRepeat = 99999,
                            Segments = new Segment[]{

                                new SegmentData(){
                            SegmentID = "LOC",
                            Name = "PLACE/LOCATION IDENTIFICATION (grp6)",
                            Notes = "6",
                            Position = 1,
                            DataElements = new Element[]
                            {
                                new DataElement()
                                {
                                    Name = "LOCATION FUNCTION CODE QUALIFIER",
                                    Description = @"Code identifying the function of a location. 147 = Transport means stowage location",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new[] { 1, 3 }
                                },

                                new CompositeElement()
                                {
                                    Name = "LOCATION IDENTIFICATION",
                                    Description = "Identification of a location by code. Stowage location: - For container vessel cell-grid positions shall be identified according to ISO 9711. - For RoRo and other vessel identify positions as assigned by carrier",
                                    DataElements = new DataElement[]
                                    {
                                        new DataElement()
                                        {
                                            Name = "Location identifier",
                                            Description = @"To identify a location. Container vessel stowage locations must be transmitted by exactly 7 digits in format BBBRRTT. The SMDG recommendation on tier numbering is to be applied for vessels allowing 10 or more tiers on deck. In case the bay-, row-, tier number does not match the format BBB, RR or  TT leading zero(s) are to be prepended. - C517.1131=9711, C517.3055=5 For RoRo-vessel prepend a 2-digit deck number in form DDBBBRRTT (exactly 9 digits). - C517.1131=STOLOC, C517.3055=87 BBBRRTT bay-row-tier cell grid position as defined by ISO 9711 DDBBBRRTT deck-bay-row-tier position as defined by carrier",
                                            Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 35 }
                                        },

                                        new DataElement()
                                        {
                                            Name = "Code list identification code",
                                            Description = "Code identifying a user or association maintained code list. 9711 - identifies codes according to ISO 9711 9711 stowage location according to ISO 9711 - Information related to freight containers on board vessels STOLOC stowage location identification as assigned by carrier",
                                            Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] {1 , 17 }
                                        },

                                        new DataElement()
                                        {
                                            Name = "Code list responsible agency code",
                                            Description = @"Code specifying the agency responsible for a code list. 5 = ISO (International Organization for Standardization) 87 = Assigned by carrier",
                                            Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 3 }
                                        }
                                    }
                                }
                            }
                        }, // ACA FINALIZA EL SEGMENTO LOC (grp6)

                        new SegmentData(){
                            SegmentID = "FTX",
                            Name = "FREE TEXT (grp6)",
                            Notes = "6",
                            DataElements = new Element[]{
                                new DataElement(){
                                    Name = "TEXT SUBJECT CODE QUALIFIER",
                                    Description = @"Code qualifying the subject of the text. AGW = Location",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new[] { 1, 3 }
                                },

                                new EmptyElement(),

                                new CompositeElement(){
                                    Name = "TEXT REFERENCE",
                                    DataElements = new Element[]{
                                        new DataElement(){
                                            Name = "TEXT REFERENCE",
                                            Description = @"Code specifying information ACCESS = Stowage location blocked in order to allow access to equipment in adjacent stowage location CONTAM = Stowage location is contaminated DAMAGE = Damaged cell guide or stacking cone(s) LOST = Blocked by oversized cargo in adjacent stowage position RESRVD = Stowage location reserved for stowage in subsequent port",
                                            Usage = RuleUsage.Mandatory,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 17 }
                                        },

                                        new DataElement(){
                                            Name = "Code list identification code",
                                            Description = @"Code identifying code list. BLOCKING = SMDG code list for stowage location blocking",
                                            Usage = RuleUsage.Optional,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 17 }
                                        },

                                        new DataElement(){
                                            Name = "Code list responsible agency code",
                                            Description = @"Code specifying the agency responsible for a code list. 306 = SMDG (Ship-planning Message Design Group)",
                                            Usage = RuleUsage.Optional,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 3 }
                                        }
                                    }
                                }
                            }
                        }, // ACA FINALIZA EL SEGMENTO FTX DEL GRUPO 6

                        new SegmentData(){
                            SegmentID = "RFF",
                            Name = "REFERENCE (grp6)",
                            Notes = "6",
                            Position = 00230,
                            DataElements = new Element[]
                            {

                                new CompositeElement()
                                {
                                    Name = "Reference",
                                    DataElements = new DataElement[]
                                    {

                                        new DataElement()
                                        {
                                            Name = "Reference code qualifier",
                                            Description = @"Code qualifying a reference. EQ = Equipment number",
                                            Usage = RuleUsage.Mandatory,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 3 }
                                        },

                                        new DataElement()
                                        {
                                            Name = "Reference identifier",
                                            Description = @"Specify related equipment identification as specified in EQD-segment's C237.8260. Equipment or breakbulk identification as defined in segment EQD data element C237.8260.",
                                            Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 70 }
                                        }
                                    }
                                }
                            }
                        }, // ACA FINALIZA EL SEGMENTO RFF (grp6)

                            }
                        },// ACA FINALIZA EL GRUPO 6

                        new SegmentGroup(){
                            GroupRepeat = 9,
                            Segments = new Segment[]{
                                new SegmentData(){
                                SegmentID = "EQD",
                                Name = "EQUIPMENT DETAILS (grp7)",
                                Notes = "7",
                                DataElements = new Element[]
                                {
                                new DataElement()
                                {
                                    Name = "Equipment Qualifier",
                                    Description = @"Code qualifying a type of equipment. Use BL for any kind of extra equipment used for fixing cargo (breakbulk). BB = Breakbulk BL = Blocks CH = Chassis CN = Container DPL = On-board equipment TE = Trailer",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 3}
                                },

                                new CompositeElement(){
                                    Name = "",
                                    DataElements = new DataElement[]{

                                new DataElement()
                                {
                                    Name = "Equipment identifier",
                                    Description = @"To identify equipment. For ISO-certified containerized equipment specify equipment's standard identification marking which consists of 4-letter prefix, 6-digit registration number and check-digit as defined by ISO 6346. - C237.1131=6346, C237.3055=5 In case equipment's identification is not (yet) known, transmit this field as TBNx - where ""x"" stands for a unique number for each unit of equipment  on board. - C237.1131=TBN, omit C237.3055. In case of containers whose identification does not comply with ISO 6346 (mostly shipper's owned containers) apply SMDG Recommendation #2 (http://www.smdg.org/documents/smdg-recommendations/) - C237.1131=CNID, omit C237.3055 For breakbulk and equipment used for fixing breakbulk specify unique breakbulk identifier consisting of UN-locode of port of loading and a 5-digit reference number. Example: ""DEHAM00001"" - C237.1131=BBID, omit C237.3055",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 17}
                                },

                                new DataElement()
                                {
                                    Name = @"Code list identification code",
                                    Description = @"Code identifying a user or association maintained code list. 6346 = container identification according to ISO 6346 - Freight containers - Coding, identification, marking BBID = breakbulk identification CNID = container identification not compliant with ISO 6346 TBN = identification not known yet",
                                    Usage = RuleUsage.Dependent,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 17 }
                                },

                                 new DataElement()
                                {
                                    Name = @"Code list responsible agency code",
                                    Description = @"Code specifying the agency responsible for a code list. Dependency (semantic): - Required in case of identification according to ISO 6346. - Leave empty in any other case. 5 = ISO (International Organization for Standardization)",
                                    Usage = RuleUsage.Dependent,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 3 }
                                },

                                }
                                },

                                new CompositeElement(){
                                    Name = "EQUIPMENT SIZE AND TYPE",
                                    Description = "Code identifying size and type of equipment.",
                                    DataElements = new DataElement[]{

                                    new DataElement()
                                    {
                                        Name = @"Equipment size and type description code",
                                        Description = @"Code specifying the size and type of equipment. For containerized equipment always use a 4-digt size type code according to ISO 6346. In case actual dimensions are not sufficiently specified by ISO size type code the use of additional DIM segments with according qualifier is required. Do not use data element 8154 for this purpose. At minimum the leading 2 digits specifying equipment's length and height are required. In case the complete size and type code is not known the digits 3 and 4 specifying ""detailed type code"" may be set to ""%"". For example: 22%% - some type of container with length 20 ft and height 8'6 L5%% - some type of container with length 45 ft and height 9'6 Fully specified equipment: - C224.1131=6346, C224.3055=5 otherwise: - C224.1131=6346, omit C224.3055",
                                        Usage = RuleUsage.Required,
                                        DataType = DataType.Alphanumeric,
                                        Precision = new []{ 1, 10 }
                                    },

                                    new DataElement()
                                    {
                                        Name = @"Code list identification code",
                                        Description = @"Code identifying a user or association maintained code list. Dependency: required if C224.3055 is specified 6346 = size and type according to ISO 6346 - Freight containers - Coding, identification and marking",
                                        Usage = RuleUsage.Dependent,
                                        DataType = DataType.Alphanumeric,
                                        Precision = new []{ 1, 17 }
                                    },

                                    new DataElement()
                                    {
                                        Name = @"Code list responsible agency code",
                                        Description = @"Code specifying the agency responsible for a code list. Dependency (semantic): required in case of full size type specification 5 ISO (International Organization for Standardization)",
                                        Usage = RuleUsage.Dependent,
                                        DataType = DataType.Alphanumeric,
                                        Precision = new []{ 1, 3 }
                                    },

                                    }
                                },



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
                            }, // ACA FINALIZA EL SEGMENTO EQD DEL GRUPO 7

                            new SegmentData(){
                            SegmentID = "NAD",
                            Name = "NAME AND ADDRESS (grp7)",
                            Notes = "7",
                            DataElements = new Element[]
                            {
                                new DataElement()
                                {
                                    Name = @"PARTY FUNCTION CODE QUALIFIER",
                                    Description = @"Code giving specific meaning to a party. CF - container operator (booking party), may be different than slot owner (VSA partner) GF - slot owner, partner in vessel sharing agreement (VSA) CF = Container operator/lessee GF = Slot charter party",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 3 }
                                },

                                new CompositeElement(){
                                    Name = "PARTY IDENTIFICATION DETAILS (grp7)",
                                    DataElements = new Element[]{
                                new DataElement()
                                {
                                    Name = "Party identifier",
                                    Description = @"Code specifying the identity of a party",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 35}
                                },

                                new DataElement()
                                {
                                    Name = @" Code list identification code",
                                    Description = @"Code identifying a user or association maintained code list. LINES = SMDG code list for master liner codes",
                                    Usage = RuleUsage.Required,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 17 }
                                },

                                new DataElement()
                                {
                                    Name = @"Code list responsible agency code",
                                    Description = @"Code specifying the agency responsible for a code list. 306 SMDG (Ship-planning Message Design Group)",
                                    Usage = RuleUsage.Required,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 3 }
                                }
                            }
                            }
                            }
                        }, // ACA FINALIZA EL SEGMENTO NAD DEL GRUPO 7

                        new SegmentData(){
                            SegmentID = "MEA",
                            Name = "MEASUREMENTS (grp7)",
                            Notes = "7",
                            DataElements = new Element[]{
                                new DataElement(){
                                    Name = "Measurement Application Qualifier",
                                    Description = @"Code qualifying the purpose of the measurement. AAE = Measurement",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new[] { 1, 3 }
                                },

                               new CompositeElement(){
                                    Name = "MEASUREMENT DETAILS",
                                    Description = "Identification of measurement type.",
                                    DataElements = new Element[]{

                                        new DataElement(){
                                            Name = "Measure Unit Qualifier",
                                            Description = @"Code specifying the attribute measured. note: Code VGM has been introduced in directory D.15B. AAO = Humidity AAS = Air flow AET = Transport equipment gross weight BRJ = Vertical center of gravity BRK = Maximum allowable transport stacking weight BRL = Carbon Dioxide T = Tare weight VGM = Transport equipment verified gross mass (weight) ZO = Oxygen",
                                            Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 3 }
                                        },

                                        new DataElement(){
                                            Name = "Measurement significance code",
                                            Description = @"Code specifying the significance of a measurement. dependency: To be used if C502.6313 = BRK 5 = Greater than or equal to 6 = Greater than 12 = True value",
                                            Usage = RuleUsage.Required,
                                            DataType = DataType.Numeric,
                                            Precision = new []{1, 3}
                                            // Precision = 3 //LONGITUD EXACTA SI O SI TIENE QUE TENER UNA LOGITUD DE 18
                                            
                                        }
                                    }
                               },

                               new CompositeElement(){
                                    Name = "VALUE/RANGE",
                                    Description = "Measurement value.",
                                    DataElements = new Element[]{

                                        new DataElement(){
                                            Name = "Measure Unit Qualifier",
                                            Description = @"Code specifying the unit of measurement. Codes defined by UN/CEFACT recommendation 20. CMT = centimeters KGM = kilogram MQH = cubic meter per hour P1 = percent (%, proportion equal to 0.01)",
                                            Usage = RuleUsage.Mandatory,
                                            DataType = DataType.Alphanumeric,
                                            Precision = new[] { 1, 8 }
                                        },

                                        new DataElement(){
                                            Name = "Measure",
                                            Description = @"To specify the value of a measurement. For VCG specify height above breakbulk's bottom layer.",
                                            Usage = RuleUsage.Required,
                                            DataType = DataType.Numeric,
                                            Precision = new []{1, 18}
                                            // Precision = 18 //LONGITUD EXACTA SI O SI TIENE QUE TENER UNA LOGITUD DE 18
                                            
                                        }
                                    }
                               }
                            }
                        }, // ACA FINALIZA EL SEGMENTO MEA DEL GRUPO 7

                        new SegmentData(){
                            SegmentID = "HAN",
                            Name = "NAME AND ADDRESS (grp7)",
                            Notes = "7",
                            DataElements = new Element[]
                            {
                                new DataElement()
                                {
                                    Name = @"PARTY FUNCTION CODE QUALIFIER",
                                    Description = @"Code giving specific meaning to a party. CF - container operator (booking party), may be different than slot owner (VSA partner) GF - slot owner, partner in vessel sharing agreement (VSA) CF = Container operator/lessee GF = Slot charter party",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 3 }
                                },

                                new CompositeElement(){
                                    Name = "PARTY IDENTIFICATION DETAILS (grp7)",
                                    DataElements = new Element[]{
                                new DataElement()
                                {
                                    Name = "Party identifier",
                                    Description = @"Code specifying the identity of a party",
                                    Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 35}
                                },

                                new DataElement()
                                {
                                    Name = @" Code list identification code",
                                    Description = @"Code identifying a user or association maintained code list. LINES = SMDG code list for master liner codes",
                                    Usage = RuleUsage.Required,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 17 }
                                },

                                new DataElement()
                                {
                                    Name = @"Code list responsible agency code",
                                    Description = @"Code specifying the agency responsible for a code list. 306 SMDG (Ship-planning Message Design Group)",
                                    Usage = RuleUsage.Required,
                                    DataType = DataType.Alphanumeric,
                                    Precision = new []{ 1, 3 }
                                }
                            }
                            }
                            }
                        }, // ACA FINALIZA EL SEGMENTO HAN DEL GRUPO 7

                            }
                        }, // ACA FINALIZA EL GRUPO 7

                    new SegmentData(){
                            SegmentID = "UNT",
                            Name = "MESSAGE TRAILER",
                            Notes = "4",
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
                        }, // ACA FINALIZA EL SEGMENTO UNT

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
                        } // ACA FINALIZA EL SEGMENTO UNZ

            };
        }
    }
}