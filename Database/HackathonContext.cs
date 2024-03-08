using hackathon.Database.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace hackathon.Database
{

    public class HackathonContext : DbContext
    {
        public DbSet<ImportedShell> ImportedShells { get; set; }

        public string DbPath { get; }

        public HackathonContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "hackathon.db");
        }

        public HackathonContext(DbContextOptions<HackathonContext> options) : base(options)
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "hackathon.db");
        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
    }
}