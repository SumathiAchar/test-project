using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace SSI.ContractManagement.Shared.Helpers
{
    public static class Serializer
    {
        // ReSharper disable once UnusedMember.Global
        public static T FromXml<T>(string xml)
        {
            using (StringReader stringReader = new StringReader(xml))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(stringReader);
            }
        }

        public static string ToXml<T>(T obj)
        {
            using (StringWriter stringWriter = new StringWriter(new StringBuilder()))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                xmlSerializer.Serialize(stringWriter, obj, new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty }));
                return stringWriter.ToString();
            }
        }
    }
}
