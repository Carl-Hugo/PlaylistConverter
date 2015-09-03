using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PlaylistConverter.Core
{
    public class XmlConversionService : IConversionService
    {
        public string Convert(string playlist)
        {
            var doc = new XmlDocument();
            doc.InnerXml = playlist;
            var songs = ExtractSongs(doc);
            var output = new StringBuilder();
            foreach(var song in songs)
            {
                output.AppendLine(song);
            }
            return output.ToString();
        }

        public IEnumerable<string> ExtractSongs(XmlDocument playlist)
        {
            var medias = playlist.GetElementsByTagName("media");
            foreach(XmlNode media in medias)
            {
                yield return media.Attributes.GetNamedItem("src").Value;
            }
        }
    }
}
