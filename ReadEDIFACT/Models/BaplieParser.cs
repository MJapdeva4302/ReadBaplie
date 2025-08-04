using System;
using System.Collections.Generic;

namespace ReadEDIFACT.Models
{
    public class BaplieParser : BaseEDIParser
    {
        private readonly BaplieVersion2? _baplieData;

        public BaplieParser(BaplieVersion2 baplieData)
        {
            _baplieData = baplieData;
        }

        public static BaplieVersion2? LoadBaplieJson(string filePath)
        {
            return LoadJson<BaplieVersion2>(filePath);
        }

        public string GenerateEDI()
        {
            if (_baplieData == null)
            {
                throw new InvalidOperationException("BAPLIE data is not initialized.");
            }

            var InterchangeControl = GenerateInterchangeRef();
            var MessageRefNumber = GenerateMessageRefNumber();

            var segments = new List<SegmentData>
            {
                // UNB - Interchange Header
                new SegmentData
                {
                    SegmentID = "UNB",
                    Usage = RuleUsage.Mandatory,
                    DataElements = new Element[]
                    {
                        new CompositeElement
                        {
                            DataElements = new DataElement[]
                            {
                                new DataElement { Name = "Syntax Identifier", Usage = RuleUsage.Mandatory,
                                        DataType = DataType.Alphabetic, Value = "UNOA" },
                                new DataElement { Name = "Syntax Version", Usage = RuleUsage.Mandatory,
                                        DataType = DataType.Numeric, Value = "2" }
                            }
                        },
                        new DataElement { Name = "Sender Identification", Usage = RuleUsage.Mandatory,
                                DataType = DataType.Alphanumeric, Value = _baplieData.SenderIdentification },
                        new DataElement { Name = "Receiver Identification", Usage = RuleUsage.Mandatory,
                                DataType = DataType.Alphanumeric, Value = _baplieData.ReceiverIdentification },
                        new CompositeElement
                        {
                            DataElements = new DataElement[]
                            {
                                new DataElement { Name = "Date", Usage = RuleUsage.Mandatory,
                                        DataType = DataType.Numeric, Value = DateTime.UtcNow.ToString("yyMMdd") },
                                new DataElement { Name = "Time", Usage = RuleUsage.Mandatory,
                                        DataType = DataType.Numeric, Value = DateTime.UtcNow.ToString("HHmm") }
                            }
                        },
                        new DataElement { Name = "Interchange Control Reference", Usage = RuleUsage.Mandatory,
                                DataType = DataType.Alphanumeric, Value = InterchangeControl }
                    }
                },

                // UNH - Message Header
                new SegmentData
                {
                    SegmentID = "UNH",
                    Usage = RuleUsage.Mandatory,
                    DataElements = new Element[]
                    {
                        new DataElement { Name = "Message Reference Number", Usage = RuleUsage.Mandatory,
                                DataType = DataType.Alphanumeric, Value = MessageRefNumber },
                        new CompositeElement
                        {
                            DataElements = new DataElement[]
                            {
                                new DataElement { Name = "Message Type", Usage = RuleUsage.Mandatory,
                                        DataType = DataType.Alphanumeric, Value = "BAPLIE" },
                                new DataElement { Name = "Version", Usage = RuleUsage.Mandatory,
                                        DataType = DataType.Alphanumeric, Value = "D" },
                                new DataElement { Name = "Release", Usage = RuleUsage.Mandatory,
                                        DataType = DataType.Alphanumeric, Value = "95B" },
                                new DataElement { Name = "Agency", Usage = RuleUsage.Mandatory,
                                        DataType = DataType.Alphanumeric, Value = "UN" },
                                new DataElement { Name = "Association", Usage = RuleUsage.Required,
                                        DataType = DataType.Alphanumeric, Value = "SMDG20" }
                            }
                        }
                    }
                }
            };

            // Add vessel information
            if (_baplieData.VesselInformation != null)
            {
                segments.Add(new SegmentData
                {
                    SegmentID = "TDT",
                    Usage = RuleUsage.Mandatory,
                    DataElements = new Element[]
                    {
                        new DataElement { Name = "Transport Stage Qualifier", Usage = RuleUsage.Mandatory,
                                DataType = DataType.Alphanumeric, Value = "20" },
                        new DataElement { Name = "Vessel Voyage", Usage = RuleUsage.Required,
                                DataType = DataType.Alphanumeric, Value = _baplieData.VesselInformation.VoyageNumber },
                        new EmptyElement(),
                        new EmptyElement(),
                        new CompositeElement
                        {
                            DataElements = new DataElement[]
                            {
                                new DataElement { Name = "Vessel Name", Usage = RuleUsage.Required,
                                        DataType = DataType.Alphanumeric, Value = _baplieData.VesselInformation.VesselName },
                                new DataElement { Name = "Vessel Operator", Usage = RuleUsage.Required,
                                        DataType = DataType.Alphanumeric, Value = _baplieData.VesselInformation.VesselOperator }
                            }
                        }
                    }
                });
            }

            // Add container information
            if (_baplieData.Containers != null)
            {
                foreach (var container in _baplieData.Containers)
                {
                    segments.Add(new SegmentData
                    {
                        SegmentID = "LOC",
                        Usage = RuleUsage.Mandatory,
                        DataElements = new Element[]
                        {
                            new DataElement { Name = "Location Function", Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric, Value = "147" },
                            new CompositeElement
                            {
                                DataElements = new DataElement[]
                                {
                                    new DataElement { Name = "Location", Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric, Value = container.Location },
                                    new DataElement { Name = "Location Type", Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric, Value = "139" }
                                }
                            }
                        }
                    });
                }
            }

            // Add UNT - Message Trailer
            segments.Add(new SegmentData
            {
                SegmentID = "UNT",
                Usage = RuleUsage.Mandatory,
                DataElements = new Element[]
                {
                    new DataElement { Name = "Number of Segments", Usage = RuleUsage.Mandatory,
                            DataType = DataType.Numeric, Value = (segments.Count + 1).ToString() },
                    new DataElement { Name = "Message Reference Number", Usage = RuleUsage.Mandatory,
                            DataType = DataType.Alphanumeric, Value = MessageRefNumber }
                }
            });

            // Add UNZ - Interchange Trailer
            segments.Add(new SegmentData
            {
                SegmentID = "UNZ",
                Usage = RuleUsage.Mandatory,
                DataElements = new Element[]
                {
                    new DataElement { Name = "Interchange Control Count", Usage = RuleUsage.Mandatory,
                            DataType = DataType.Numeric, Value = "1" },
                    new DataElement { Name = "Interchange Control Reference", Usage = RuleUsage.Mandatory,
                            DataType = DataType.Alphanumeric, Value = InterchangeControl }
                }
            });

            return string.Join("'", segments.Select(s => s.ToString()));
        }
    }
}
