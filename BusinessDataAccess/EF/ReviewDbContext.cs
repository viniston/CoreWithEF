using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BusinessDataAccessDefinition.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace BusinessDataAccess.EF
{
    public sealed class ReviewDbContext : DbContext
    {
        private IDbContextTransaction _currentTransaction;

        public ReviewDbContext(DbContextOptions<ReviewDbContext> options) : base(options)
        {
        }

        public DbSet<Review> Review { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // example for seeding data while creating context
            modelBuilder.Entity<Review>().HasData(
                new Review { Id = 1, Name = "Review_OPTIMIZER" }
            );

            modelBuilder.Entity<Review>().HasKey(a => new { a.Id });
        }

        public void BeginTransaction()
        {
            if (_currentTransaction != null)
            {
                return;
            }

            _currentTransaction = Database.BeginTransaction(IsolationLevel.ReadCommitted);
        }

        public void CloseTransaction()
        {
            CloseTransaction(null);
        }

        public void CloseTransaction(Exception exception)
        {
            try
            {
                if (_currentTransaction != null && exception != null)
                {
                    // todo: log exception
                    _currentTransaction.Rollback();
                    return;
                }

                SaveChanges();

                _currentTransaction?.Commit();
            }
            catch (Exception)
            {
                // todo: log exception
                if (_currentTransaction?.GetDbTransaction().Connection != null)
                {
                    _currentTransaction.Rollback();
                }

                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public override int SaveChanges()
        {
            OnBeforeSaving();
            return base.SaveChanges();
        }
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(cancellationToken);
        }

        public void DetachAllEntities()
        {
            var changedEntriesCopy = ChangeTracker.Entries()
                .ToList();

            foreach (var entry in changedEntriesCopy)
                entry.State = EntityState.Detached;
        }

        private void OnBeforeSaving()
        {

        }
    }


}
