﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TravianManager.Data.Sql.Context
{
    using Microsoft.EntityFrameworkCore;
    using System.Threading.Tasks;
    using TravianManager.Core.Context;
    using TravianManager.Core.Data;

    /// <summary>
    /// The EntityFramework Database Context.
    /// </summary>
    public class EntityFrameworkDbContext : DbContext, IEntityFrameworkDbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkDbContext"/> class.
        /// </summary>
        /// <param name="options"> The options for DataContext.</param>
        public EntityFrameworkDbContext(DbContextOptions options) : base(options)
        {
            Database.SetCommandTimeout(0);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Attacker>()
                .HasOne(c => c.Account)
                .WithOne();

            modelBuilder.Entity<Attacker>()
                .HasMany(c => c.Defender);

            modelBuilder.Entity<Defender>()
                .HasOne(c => c.Account)
                .WithMany();
        }

        public Task<int> SaveChangesAsync() => base.SaveChangesAsync();

        public DbSet<User> Users { get; set; }

        public DbSet<Attacker> Attackers { get; set; }

        public DbSet<Defender> Defenders { get; set; }

        public DbSet<Account> Accounts { get; set; }
    }
}
