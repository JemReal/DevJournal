using DevJournal.Web.Data;
using DevJournal.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace DevJournal.Web.Repositories
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly DevjournalDbContext devjournalDbContext;

        public BlogPostRepository(DevjournalDbContext devjournalDbContext)
        {
            this.devjournalDbContext = devjournalDbContext;
        }

        public async Task<BlogPost> AddAsync(BlogPost blogPost)
        {
            await devjournalDbContext.AddAsync(blogPost);
            await devjournalDbContext.SaveChangesAsync();
            return blogPost;
        }

        public async Task<BlogPost?> DeleteAsync(Guid id)
        {
            var existingBlog = await devjournalDbContext.BlogPosts.FindAsync(id);

            if (existingBlog != null)
            {
                devjournalDbContext.BlogPosts.Remove(existingBlog);
                await devjournalDbContext.SaveChangesAsync();
                return existingBlog;
            }

            return null;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await devjournalDbContext.BlogPosts.Include(x => x.Tags).ToListAsync();
        }

        public async Task<BlogPost?> GetAsync(Guid id)
        {
            return await devjournalDbContext.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<BlogPost?> GetByUrlHandleAsync(string urlHandle)
        {
            return await devjournalDbContext.BlogPosts.Include(x => x.Tags)
                .FirstOrDefaultAsync(x => x.UrlHandle == urlHandle);
        }

        public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
            var existingBlog = await devjournalDbContext.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == blogPost.Id);

            if (existingBlog != null)
            {
                existingBlog.Id = blogPost.Id;
                existingBlog.Heading = blogPost.Heading;
                existingBlog.PageTitle = blogPost.PageTitle;
                existingBlog.Content = blogPost.Content;
                existingBlog.ShortDescription = blogPost.ShortDescription;
                existingBlog.Author = blogPost.Author;
                existingBlog.FeaturedImageUrl = blogPost.FeaturedImageUrl;
                existingBlog.UrlHandle = blogPost.UrlHandle;
                existingBlog.PublishedDate = blogPost.PublishedDate;
                existingBlog.Visible = blogPost.Visible;
                existingBlog.Tags = blogPost.Tags;

                await devjournalDbContext.SaveChangesAsync();
                return existingBlog;
            }

            return null;
        }
    }
}
