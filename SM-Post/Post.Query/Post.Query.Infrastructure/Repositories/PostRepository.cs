using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;
using Post.Query.Infrastructure.DataAcess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Post.Query.Infrastructure.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly DatabaseContextFactory _databaseContextFactory;
        public PostRepository(DatabaseContextFactory databaseContextFactory)
        {
            _databaseContextFactory = databaseContextFactory;
        }

        public async Task CreateAsync(PostEntity post)
        {
            using DatabaseContext context = _databaseContextFactory.CreateDbContext();
            context.Posts.Add(post);

            _ = await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid postId)
        {
            using DatabaseContext context = _databaseContextFactory.CreateDbContext();
            var post = await GetByIdAsync(postId);
            if (post == null) return;

            context.Posts.Remove(post);
            _ = await context.SaveChangesAsync();
        }

        public Task<PostEntity?> GetByIdAsync(Guid postId)
        {
            using DatabaseContext context = _databaseContextFactory.CreateDbContext();
            return context.Posts.Include(p => p.Comments)
                .FirstOrDefaultAsync(x => x.PostId == postId);
        }

        public Task<List<PostEntity>> ListAllAsync()
        {
            using DatabaseContext context = _databaseContextFactory.CreateDbContext();
            return context.Posts.AsNoTracking()
                .Include(p => p.Comments).AsNoTracking()
                .ToListAsync();
        }

        public Task<List<PostEntity>> ListByAuthorAsync(string Author)
        {
            using DatabaseContext context = _databaseContextFactory.CreateDbContext();
            return context.Posts.AsNoTracking()
                .Include(p => p.Comments).AsNoTracking()
                .Where(x => x.Author.Contains(Author))
                .ToListAsync();
        }

        public Task<List<PostEntity>> ListWithCommentsAsync()
        {
            using DatabaseContext context = _databaseContextFactory.CreateDbContext();
            return context.Posts.AsNoTracking()
                .Include(p => p.Comments).AsNoTracking()
                .Where(x => x.Comments != null && x.Comments.Any())
                .ToListAsync();
        }

        public Task<List<PostEntity>> ListWithLikesAsync(int numberOfLikes)
        {
            using DatabaseContext context = _databaseContextFactory.CreateDbContext();
            return context.Posts.AsNoTracking()
                .Include(p => p.Comments).AsNoTracking()
                .Where(x => x.Likes >= numberOfLikes)
                .ToListAsync();
        }

        public async Task UpdateAsync(PostEntity post)
        {
            using DatabaseContext context = _databaseContextFactory.CreateDbContext();
            context.Posts.Update(post);

            _ = await context.SaveChangesAsync();
        }
    }
}
