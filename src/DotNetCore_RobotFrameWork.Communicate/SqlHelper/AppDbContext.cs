using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;

namespace DotNetCore_RobotFrameWork.Communicate.SqlHelper
{
    public partial class AppDbContext
    {
        private readonly SqlConnection _connection;
        private readonly ReaderWriterLockSlim _rwLock = new ReaderWriterLockSlim();
        private readonly LinkedList<UnitOfWork> _uows = new LinkedList<UnitOfWork>();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectionString"></param>
        public AppDbContext(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
            _connection.Open();
        }

        /// <summary>
        /// Create Unit Of Work
        /// </summary>
        /// <returns></returns>
        public UnitOfWork CreateUnitOfWork()
        {
            var transaction = _connection.BeginTransaction();
            var uow = new UnitOfWork(transaction, RemoveTransaction, RemoveTransaction);

            _rwLock.EnterWriteLock();
            _uows.AddLast(uow);
            _rwLock.ExitWriteLock();

            return uow;
        }

        /// <summary>
        /// CreateCommand
        /// </summary>
        /// <returns></returns>
        public SqlCommand CreateCommand()
        {
            var cmd = _connection.CreateCommand();

            _rwLock.EnterReadLock();
            if (_uows.Count > 0)
                cmd.Transaction = _uows.First.Value.Transaction;
            _rwLock.ExitReadLock();

            return cmd;
        }

        private void RemoveTransaction(UnitOfWork obj)
        {
            _rwLock.EnterWriteLock();
            _uows.Remove(obj);
            _rwLock.ExitWriteLock();
        }

        /// <summary>
        /// Dispose object
        /// </summary>
        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}
