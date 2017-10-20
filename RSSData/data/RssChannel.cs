using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSSData.data
{

    public class RssContext : DbContext
    {
        public RssContext(DbContextOptions<RssContext> options) : base(options)
        {

        }

        public DbSet<RssChannel> Channels { get; set; }
        public DbSet<RssItem> Items { get; set; }
    }
    public class RssChannel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(250)]
        public string title { get; set; }
        public string link { get; set; }
        public string description { get; set; }
        public string language { get; set; } = "en-us";
        public DateTime pubDate { get; set; } = DateTime.UtcNow;
        public DateTime lastBuildDate { get; set; } = DateTime.UtcNow;

        public string docs { get; set; }
        public string Generator { get; set; } = "Scott's RSS";
        public string managingEditor { get; set; }
        public string webMaster { get; set; }

        public virtual IEnumerable<RssItem> items { get; set; }

        public string RssInnerXml()
        {
            StringBuilder s = new StringBuilder();

            s.Append(XmlHelper.Xmlline("title", title));
            s.Append(XmlHelper.Xmlline("link", link));
            s.Append(XmlHelper.Xmlline("description", description));
            s.Append(XmlHelper.Xmlline("language", language));
            s.Append(XmlHelper.Xmlline("pubDate", pubDate));
            s.Append(XmlHelper.Xmlline("lastBuildDate", lastBuildDate));
            s.Append(XmlHelper.Xmlline("docs", docs));
            s.Append(XmlHelper.Xmlline("Generator", Generator));
            s.Append(XmlHelper.Xmlline("managingEditor", managingEditor));
            s.Append(XmlHelper.Xmlline("webMaster", webMaster));



            return s.ToString();

        }
 
    }

    public class RssItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string title { get; set; }
        public string link { get; set; }
        public string description { get; set; }
        public DateTime pubDate { get; set; } = DateTime.UtcNow;
        public string guid { get; set; }

        public int RssChannelId { get; set; }
        public string RssInnerXml()
        {
            StringBuilder s = new StringBuilder();

            s.Append(XmlHelper.Xmlline("title", title));
            s.Append(XmlHelper.Xmlline("link", link));
            s.Append(XmlHelper.Xmlline("description", description));
            s.Append(XmlHelper.Xmlline("pubDate", pubDate));
            s.Append(XmlHelper.Xmlline("guid", guid));

            return s.ToString();

        }


    }
    public static class XmlHelper
    {
        public static string Xmlline(string tag, string data)
        {
            string ret = "";
            if (!String.IsNullOrEmpty(data))
                ret = $"<{tag}><![CDATA[{data}]]></{tag}>";
            return ret;
        }

        public static string Xmlline(string tag, DateTime data)
        {
            return $"<{tag}>{data.ToLongDateString()}</{tag}>";
        }

        public static string Xmlline(string tag, int data)
        {
            return $"<{tag}>{data}</{tag}>";
        }
    }
}
