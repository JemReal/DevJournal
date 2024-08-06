using DevJournal.Web.Data;
using DevJournal.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace DevJournal.Web.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly DevjournalDbContext devjournalDbContext;

        public TagRepository(DevjournalDbContext devjournalDbContext)
        {
            this.devjournalDbContext = devjournalDbContext;
        }

        public async Task<Tag> AddAsync(Tag tag)
        {
            await devjournalDbContext.Tags.AddAsync(tag);
            await devjournalDbContext.SaveChangesAsync();

            return tag;
        }

        public async Task<Tag?> DeleteAsync(Guid id)
        {
            var existingTag = await devjournalDbContext.Tags.FindAsync(id);

            if (existingTag != null)
            {
                devjournalDbContext.Tags.Remove(existingTag);
                await devjournalDbContext.SaveChangesAsync();
                return existingTag;
            }

            return null;
        }

        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
            return await devjournalDbContext.Tags.ToListAsync();
        }

        public Task<Tag?> GetAsync(Guid id)
        {
            return devjournalDbContext.Tags.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Tag?> UpdateAsync(Tag tag)
        {
            var existingTag = await devjournalDbContext.Tags.FindAsync(tag.Id);

            if (existingTag != null)
            {
                existingTag.Name = tag.Name;
                existingTag.DisplayName = tag.DisplayName;

                await devjournalDbContext.SaveChangesAsync();

                return existingTag;
            }

            return null;
        }
    }
}
