using Microsoft.EntityFrameworkCore;
using RSSData.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSSData.services
{
    public class RssService
    {
        private readonly RssContext rSSContext;

        public RssService(RssContext rssContext)
        {
            rSSContext = rssContext;
        }

        public async Task<List<RssChannel>> GetChannels()
        {
            return await rSSContext.Channels.ToListAsync();

        }

        public async Task<RssChannel> GetChannel(int id)
        {
            return await rSSContext.Channels.FindAsync(id);
        }
        public async Task<List<RssItem>> GetChannelItems(int ChannelId)
        {
            return await rSSContext.Items.Where(a => a.RssChannelId == ChannelId).ToListAsync();
        }

        public async Task AddChannelAsync(RssChannel newChannel)
        {
            await rSSContext.AddAsync(newChannel);
            await rSSContext.SaveChangesAsync();
        }

        public async Task RemoveChannelAsync(int ChannelId)
        {
            List<RssItem> entitiesToRemove = await GetChannelItems(ChannelId);
            rSSContext.Items.RemoveRange(entitiesToRemove);
            rSSContext.Channels.Remove( await rSSContext.Channels.FindAsync (ChannelId));
            await rSSContext.SaveChangesAsync();

        }

        public async Task RemoveItem(int ItemId)
        {
            RssItem i = await rSSContext.Items.FindAsync(ItemId);
            if(i != null)
                rSSContext.Items.Remove(i);
            await rSSContext.SaveChangesAsync();
        }

        public async Task AddItemAsync(RssItem NewItem)
        {
            if (NewItem == null)
                throw new ArgumentNullException("NewItem");

             rSSContext.Items.Add(NewItem);
            await rSSContext.SaveChangesAsync();
        }

        public async Task UpdateItem(RssItem NewItem)
        {
            if (NewItem == null)
                throw new ArgumentNullException("NewItem");
            RssItem item  = await rSSContext.Items.FindAsync(NewItem.Id);
            if (item == null)
                throw new Exception("The RSSItem does not exist.");
            rSSContext.Update(NewItem); 
            await rSSContext.SaveChangesAsync();
        }

        public async Task UpdateChannel(RssChannel NewItem)
        {
            if (NewItem == null)
                throw new ArgumentNullException("NewItem");
            RssChannel item = await rSSContext.Channels.FindAsync(NewItem.Id);
            if (item == null)
                throw new Exception("The RSSChannel does not exist.");
            rSSContext.Update(NewItem);
            await rSSContext.SaveChangesAsync();
        }
    }
}
