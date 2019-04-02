using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.ServiceModel.Syndication;
using System.IO;
using System.Xml;
using System.Text;

namespace RequestBuffering.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // POST api/values
        [HttpPost]
        public void Post([FromBody] SyndicationItem value)
        {
            Console.WriteLine("POST CONTROLLER");
            try {
                Console.WriteLine("GOT VALUE: {0}", value.Title.Text);
                var sink = new StringWriter();
                value.SaveAsAtom10(GetSerializer(sink));
                Console.WriteLine("GOT XML: {0}", sink.ToString());
            } catch (Exception e) {
                Console.WriteLine("CAUGHT EXCEPTION: {0}", e.Message);
                throw e;
            }
        }

        [HttpPut]
        public void Put([FromBody] XmlElement value)
        {
            Console.WriteLine("PUT CONTROLLER");
            Console.WriteLine("GOT XML: {0}", value.OuterXml);
        }

        private XmlWriter GetSerializer(TextWriter sink)
        {
            var xmlw = XmlWriter.Create(sink, new XmlWriterSettings {
                Encoding = new UTF8Encoding(false),
                Indent = true,
                OmitXmlDeclaration = true
            });
            return xmlw;
        }
    }
}
