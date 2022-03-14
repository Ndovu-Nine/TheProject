
using System;
using System.Data;
using TemboContext.CO;
using TemboContext.Connection; 
namespace TemboContext.Context
{
/// <summary>
    /// A collection of all custom operations
    /// </summary>
	public class DbContext : IDisposable
	{
		private ConnectionContainer connectionContainer;

		public ConnectionContainer DbConnection
		{
			get => connectionContainer;
		    set => connectionContainer = value;
		}
		
		private AssetCO asset;
		public AssetCO Asset => asset ??= new AssetCO(connectionContainer, true);
		
		private StrategyCO strategy;
		public StrategyCO Strategy => strategy ??= new StrategyCO(connectionContainer, true);
		
		private PositionCO position;
		public PositionCO Position => position ??= new PositionCO(connectionContainer, true);
		

		public DbContext()
		{
			DbConnection = new ConnectionContainer()
			{
				Connection = Connection.DbConnection.CreateConnection()
			};
		}
		/// <summary>
        /// 
		/// other_page
        /// </summary>
		public IDbTransaction BeginTransaction()
		{
			DbConnection.Transaction = DbConnection.Connection.BeginTransaction();
			return DbConnection.Transaction;
		}

		public IDbTransaction BeginTransaction(IsolationLevel isolationLevel)
		{
			DbConnection.Transaction = DbConnection.Connection.BeginTransaction(isolationLevel);
			return DbConnection.Transaction;
		}

		public void CommitTransaction()
		{
			DbConnection.Transaction.Commit();
		}

		public void Dispose()
		{
			
			asset?.Dispose();
			asset = null;
			
			strategy?.Dispose();
			strategy = null;
			
			position?.Dispose();
			position = null;
			
			connectionContainer?.Dispose();
			connectionContainer = null;
		}
	}
}
