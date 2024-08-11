
using DevJournal.Web.Data;
using DevJournal.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace DevJournal.Web.Repositories
{
    public class BlogPostLikeRepository : IBlogPostLikeRepository
    {
        private readonly DevjournalDbContext devjournalDbContext;

        public BlogPostLikeRepository(DevjournalDbContext devjournalDbContext)
        {
            this.devjournalDbContext = devjournalDbContext;
        }

        public async Task<BlogPostLike> AddLikeForBlog(BlogPostLike blogPostLike)
        {
            await devjournalDbContext.BlogPostLike.AddAsync(blogPostLike);
            await devjournalDbContext.SaveChangesAsync();
            return blogPostLike;
        }

        public async Task<IEnumerable<BlogPostLike>> GetLikesForBlog(Guid blogPostId)
        {
            return await devjournalDbContext.BlogPostLike.Where(x => x.BlogPostId == blogPostId)
                .ToListAsync();
        }

        public async Task<int> GetTotalLikes(Guid blogPostId)
        {
            return await devjournalDbContext.BlogPostLike.CountAsync(x => x.BlogPostId == blogPostId);
        }
    }
}
