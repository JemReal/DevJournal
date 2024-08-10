﻿using DevJournal.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace DevJournal.Web.Data
{
    public class DevjournalDbContext : DbContext
    {
        public DevjournalDbContext(DbContextOptions<DevjournalDbContext> options) : base(options)
        {

        }

        public DbSet<BlogPost> BlogPosts { get; set; }

        public DbSet<Tag> Tags { get; set; }
    }
}
