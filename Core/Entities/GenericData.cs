using System.Xml.Serialization;

namespace Core.Entities
{
    [XmlRoot(ElementName = "GenericData", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message")]
    public class GenericData
    {
        [XmlElement(ElementName = "DataSet", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message")]
        public required DataSet DataSet { get; set; }
    }

    public class DataSet
    {
        [XmlElement(ElementName = "Series", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/data/generic")]
        public required List<Series> Series { get; set; }
    }

    public class Series
    {
        [XmlElement(ElementName = "SeriesKey", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/data/generic")]
        public required SeriesKey SeriesKey { get; set; }

        [XmlElement(ElementName = "Obs", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/data/generic")]
        public required List<Obs> Observations { get; set; }
    }

    public class SeriesKey
    {
        [XmlElement(ElementName = "Value", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/data/generic")]
        public required List<SeriesKeyValue> Values { get; set; }
    }

    public class SeriesKeyValue
    {
        [XmlAttribute("id")]
        public required string Id { get; set; }

        [XmlAttribute("value")]
        public required string Value { get; set; }
    }

    public class Obs
    {
        [XmlElement(ElementName = "ObsDimension", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/data/generic")]
        public required ObsDimension Dimension { get; set; }

        [XmlElement(ElementName = "ObsValue", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/data/generic")]
        public required ObsValue Value { get; set; }

        [XmlElement(ElementName = "Attributes", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/data/generic")]
        public required ObsAttributes Attributes { get; set; }
    }

    public class ObsDimension
    {
        [XmlAttribute("value")]
        public required string Value { get; set; }
    }

    public class ObsValue
    {
        [XmlAttribute("value")]
        public required string Value { get; set; }
    }

    public class ObsAttributes
    {
        [XmlElement(ElementName = "Value", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/data/generic")]
        public required List<ObsAttributeValue> Values { get; set; }
    }

    public class ObsAttributeValue
    {
        [XmlAttribute("id")]
        public required string Id { get; set; }

        [XmlAttribute("value")]
        public required string Value { get; set; }
    }
}
