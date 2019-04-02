using System;
using System.IO;
using System.Reflection;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace RequestBuffering
{
    public class AtomEntryInputFormatter : XmlSerializerInputFormatter
    {
        public AtomEntryInputFormatter(MvcOptions options) : base(options)
        {
            SupportedMediaTypes.Clear();
            SupportedMediaTypes.Add("application/atom+xml;type=entry");
        }

        protected override bool CanReadType(Type dataType)
        {
            Console.WriteLine("CAN READ TYPE? {0} --> {1}", dataType, typeof(SyndicationItem).IsAssignableFrom(dataType));
            return typeof(SyndicationItem).IsAssignableFrom(dataType);
        }

        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context,
            Encoding encoding)
        {
            try {
                Console.WriteLine("PARSING STREAM {0}", context.HttpContext.Request.Body.GetType());
                using (XmlReader reader = XmlReader.Create(context.HttpContext.Request.Body))
                {
                    SyndicationItem item = SyndicationItem.Load(reader);
                    Console.WriteLine("SUCCESSFULLY PARSED: {0}", item.Title.Text);
                    return await InputFormatterResult.SuccessAsync(item);
                }
            } catch (Exception e) {
                Console.WriteLine("CAUGHT EXCEPTION WHILE PARSING: {0}", e.Message);
                throw e;
            }
        }
    }
}
