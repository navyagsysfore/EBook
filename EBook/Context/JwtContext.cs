using EBook.Models;
using Microsoft.EntityFrameworkCore;

namespace EBook.Context
{
    public class JwtContext : DbContext
    {
        public JwtContext(DbContextOptions<JwtContext> options) : base(options)
        {
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<AuthorBookMappings> AuthorBookMappings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

         
            modelBuilder.Entity<Author>().HasKey(a => a.ID);
            modelBuilder.Entity<Book>().HasKey(b => b.BookID);
            modelBuilder.Entity<Genre>().HasKey(g => g.GenreID);
            modelBuilder.Entity<Users>().HasKey(u => u.Username);

            modelBuilder.Entity<Users>().Property(u => u.Role).HasColumnName("Role");
            modelBuilder.Entity<Genre>()
                .Property(g => g.GenreID)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Author>()
                .HasMany(a => a.Books)
                .WithMany(b => b.Authors)
                .UsingEntity<AuthorBookMappings>(
                    j => j
                        .HasOne(ab => ab.Book)
                        .WithMany(b => b.AuthorBookMappings)
                        .HasForeignKey(ab => ab.BookID),
                    j => j
                        .HasOne(ab => ab.Author)
                        .WithMany(a => a.AuthorBookMappings)
                        .HasForeignKey(ab => ab.AuthorID),
                    j =>
                    {
                        j.HasKey(t => new { t.AuthorID, t.BookID });
                    });
        }
    }
}
