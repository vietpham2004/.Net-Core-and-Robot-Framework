using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace DotNetCore_RobotFrameWork.Communicate.SqlHelper
{
    public class UnitOfWork
    {
        private SqlTransaction _transaction;
        private readonly Action<UnitOfWork> _rolledBack;
        private readonly Action<UnitOfWork> _committed;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="rolledBack"></param>
        /// <param name="committed"></param>
		public UnitOfWork(SqlTransaction transaction, Action<UnitOfWork> rolledBack, Action<UnitOfWork> committed)
        {
            Transaction = transaction;
            _transaction = transaction;
            _rolledBack = rolledBack;
            _committed = committed;
        }

        /// <summary>
        /// Get/Set Transaction
        /// </summary>
		public SqlTransaction Transaction { get; private set; }

        /// <summary>
        /// Dispose object
        /// </summary>
		public void Dispose()
        {
            if (_transaction == null)
                return;

            _transaction.Rollback();
            _transaction.Dispose();
            _rolledBack(this);
            _transaction = null;
        }

        /// <summary>
        /// Save Changes
        /// </summary>
		public void SaveChanges()
        {
            if (_transaction == null)
                throw new InvalidOperationException("May not call save changes twice.");

            _transaction.Commit();
            _committed(this);
            _transaction = null;
        }
    }
}
