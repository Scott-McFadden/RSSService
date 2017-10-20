using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RSSData.data;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RSSService
{
    [Route("api/[controller]")]
    public class RssServicesController : Controller
    {

        RSSData.services.RssService rssService;

        private readonly RssContext rssContext;

        public RssServicesController(RssContext _rssContext)
        {
            rssContext = _rssContext;
            rssService = new RSSData.services.RssService(rssContext);
        }
        // GET: api/values
        [HttpGet]
        [Produces("application/json", "application/xml", "text/html")]

        public async Task<IActionResult> Get()
        {

            List<RssChannel> channels = await rssService.GetChannels();
            foreach (RssChannel channel in channels)
            {
                channel.items = await rssService.GetChannelItems(channel.Id);
            }
            return Ok(channels);
        }
        // GET: api/values
        [HttpGet]
        [Produces("application/json", "application/xml", "application/text", "text/html", "text/xml", "application/rss")]
        [Route("RSS")]

        public async Task<IActionResult> RSS()
        {
            StringBuilder s = new StringBuilder();
            try
            {


                List<RssChannel> channels = await rssService.GetChannels();

                s.Append("<rss versio='2.0'>");
                foreach (RssChannel channel in channels)
                {
                    channel.items = await rssService.GetChannelItems(channel.Id);
                    s.AppendLine("    <channel>");
                    s.AppendLine(channel.RssInnerXml());
                    if (channel.items.Count() > 0)
                    {
                        foreach (RssItem item in channel.items)
                        {
                            s.AppendLine("      <item>");
                            s.AppendLine(item.RssInnerXml());
                            s.AppendLine("      </item>");
                        }
                        s.AppendLine("    </channel>");
                    }

                }
                s.Append("</rss>");
                return this.Content(s.ToString(), "text/xml");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Produces("application/json", "application/xml", "application/text", "text/html", "text/xml", "application/rss")]
        [Route("RSS/{id}")]

        public async Task<IActionResult> RSS(int id)
        {
            StringBuilder s = new StringBuilder();
            try
            {

                RssChannel channel = await rssService.GetChannel(id);
                if (channel == null)
                    return BadRequest($"Channel {id} cannot be found.");
                s.Append("<rss versio='2.0'>");

                channel.items = await rssService.GetChannelItems(channel.Id);
                s.Append("    <channel>");
                s.Append(channel.RssInnerXml());
                if (channel.items.Count() > 0)
                {
                    foreach (RssItem item in channel.items)
                    {
                        s.Append("      <item>");
                        s.Append(item.RssInnerXml());
                        s.Append("      </item>");
                    }

                }

                s.Append("    </channel>");
                s.Append("</rss>");
                return this.Content(s.ToString(), "text/xml");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        
    }
}
