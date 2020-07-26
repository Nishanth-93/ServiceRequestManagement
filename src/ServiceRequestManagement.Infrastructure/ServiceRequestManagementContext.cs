using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using ServiceRequestManagement.Domain.Seeds;
using ServiceRequestManagement.Domain.ServiceRequestAggregate;
using ServiceRequestManagement.Infrastructure.EntityConfigurations;
using System;
using System.Data;
using System.Threading.Tasks;

namespace ServiceRequestManagement.Infrastructure
{
    /// <summary>
    /// The Db context for the ServiceRequestManagement API.
    /// </summary>
    public class ServiceRequestManagementContext : DbContext, IUnitOfWork
    {
        /// <summary>
        /// The ServiceRequest entity DbSet. Used to access the ServiceReques entity in the Db.
        /// </summary>
        public DbSet<ServiceRequest> ServiceRequests { get; set; }

        private IDbContextTransaction _currentTransaction;

        /// <summary>
        /// Checks whether the context has a current transaction.
        /// </summary>
        /// /// <returns>true if the IDbContextTransaction is not null; otherwise, false</returns>
        public bool HasActiveTransaction => _currentTransaction != null;

        /// <summary>
        /// Constructor for the ServiceRequestManagementContext.
        /// </summary>
        /// <param name="options"></param>
        public ServiceRequestManagementContext(DbContextOptions<ServiceRequestManagementContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .ApplyConfiguration(new ServiceRequestEntityTypeConfiguration());
        }

        /// <summary>
        /// Begins the Db transaction.
        /// </summary>
        /// <returns>IDbContext transaction if the current transaction is null; otherwise returns null.</returns>
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            if (_currentTransaction != null)
                return null;

            _currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);

            return _currentTransaction;
        }

        /// <summary>
        /// Rolls back the Db transaction.
        /// </summary>
        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
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

        /// <summary>
        /// Commits the Db transaction.
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns>Task</returns>
        public async Task CommitTransactionAsync(IDbContextTransaction transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            if (transaction != _currentTransaction)
                throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

            try
            {
                await SaveChangesAsync();
                transaction.Commit();
            }
            catch
            {
                RollbackTransaction();
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
    }
}
