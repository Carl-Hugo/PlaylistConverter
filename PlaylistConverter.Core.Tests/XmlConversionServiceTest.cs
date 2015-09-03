using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaylistConverter.Core.Tests
{
    [TestClass]
    public class XmlConversionServiceTest
    {
        [TestMethod]
        public void ConvertTest()
        {
            // Arange
            string wplPlaylistData = @"<?wpl version=""1.0""?>
<smil>
    <head>
        <meta name=""Generator"" content=""Microsoft Windows Media Player -- 12.0.10240.16397""/>
        <meta name=""ItemCount"" content=""34""/>
        <title>Eluveitie - Playlist</title>
    </head>
    <body>
        <seq>
            <media src=""a""/>
            <media src=""b""/>
            <media src=""c""/>
        </seq>
    </body>
</smil>";
            string expectedM3UplaylistData = @"a
b
c
";

            var service = new XmlConversionService();

            // Act
            var results = service.Convert(wplPlaylistData);

            // Assert
            Assert.AreEqual(expectedM3UplaylistData, results);
        }
    }
}
