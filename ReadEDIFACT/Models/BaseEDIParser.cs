using System;
using System.Text.Json;
using System.Linq;

namespace ReadEDIFACT.Models
{
    public abstract class BaseEDIParser
    {
        protected static string GenerateInterchangeRef()
        {
            string timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            if (timestamp.Length > 10)
            {
                timestamp = timestamp.Substring(timestamp.Length - 10);
            }
            Random random = new Random();
            string randomNumber = random.Next(1000, 9999).ToString();
            string interchangeRef = $"{timestamp}{randomNumber}";
            if (interchangeRef.Length > 14)
            {
                interchangeRef = interchangeRef.Substring(0, 14);
            }
            return interchangeRef;
        }

        protected static string GenerateMessageRefNumber()
        {
            string timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            if (timestamp.Length > 5)
            {
                timestamp = timestamp.Substring(timestamp.Length - 5);
            }
            Random random = new Random();
            string randomNumber = random.Next(100, 999).ToString();
            string messageRef = $"{timestamp}{randomNumber}";
            if (messageRef.Length > 8)
            {
                messageRef = messageRef.Substring(0, 8);
            }
            return messageRef;
        }

        protected static T? LoadJson<T>(string filePath) where T : class
        {
            try
            {
                string json = File.ReadAllText(filePath);
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var result = JsonSerializer.Deserialize<T>(json, options);
                if (result == null)
                {
                    throw new InvalidOperationException($"Failed to deserialize JSON to {typeof(T).Name}.");
                }
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading JSON: {ex.Message}");
                return null;
            }
        }

        protected SegmentData CreateUnbSegment(string senderIdentification, string receiverIdentification, string interchangeControl)
        {
            return new SegmentData
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
                            DataType = DataType.Alphanumeric, Value = senderIdentification },
                    new DataElement { Name = "Receiver Identification", Usage = RuleUsage.Mandatory,
                            DataType = DataType.Alphanumeric, Value = receiverIdentification },
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
                            DataType = DataType.Alphanumeric, Value = interchangeControl }
                }
            };
        }

        protected SegmentData CreateUnhSegment(string messageType, string version, string release, string messageRefNumber)
        {
            return new SegmentData
            {
                SegmentID = "UNH",
                Usage = RuleUsage.Mandatory,
                DataElements = new Element[]
                {
                    new DataElement { Name = "Message Reference Number", Usage = RuleUsage.Mandatory,
                            DataType = DataType.Alphanumeric, Value = messageRefNumber },
                    new CompositeElement
                    {
                        DataElements = new DataElement[]
                        {
                            new DataElement { Name = "Message Type", Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric, Value = messageType },
                            new DataElement { Name = "Version", Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric, Value = version },
                            new DataElement { Name = "Release", Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric, Value = release },
                            new DataElement { Name = "Agency", Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric, Value = "UN" }
                        }
                    }
                }
            };
        }

        protected SegmentData CreateUntSegment(int segmentCount, string messageRefNumber)
        {
            return new SegmentData
            {
                SegmentID = "UNT",
                Usage = RuleUsage.Mandatory,
                DataElements = new Element[]
                {
                    new DataElement { Name = "Number of Segments", Usage = RuleUsage.Mandatory,
                            DataType = DataType.Numeric, Value = segmentCount.ToString() },
                    new DataElement { Name = "Message Reference Number", Usage = RuleUsage.Mandatory,
                            DataType = DataType.Alphanumeric, Value = messageRefNumber }
                }
            };
        }

        protected SegmentData CreateUnzSegment(string interchangeControl)
        {
            return new SegmentData
            {
                SegmentID = "UNZ",
                Usage = RuleUsage.Mandatory,
                DataElements = new Element[]
                {
                    new DataElement { Name = "Interchange Control Count", Usage = RuleUsage.Mandatory,
                            DataType = DataType.Numeric, Value = "1" },
                    new DataElement { Name = "Interchange Control Reference", Usage = RuleUsage.Mandatory,
                            DataType = DataType.Alphanumeric, Value = interchangeControl }
                }
            };
        }
    }
}
