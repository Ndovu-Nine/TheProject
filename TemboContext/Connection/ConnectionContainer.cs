
using System;
using System.Data;
namespace TemboContext.Connection
{
	public class ConnectionContainer : IDisposable
	{
		private IDbConnection Conn { get; set; }
		public IDbConnection Connection
		{
			get => Conn;
		    set => Conn = value;
		}
		private IDbTransaction transaction;
		public IDbTransaction Transaction
		{
			get => transaction;
		    set
			{
				transaction = value;
				if (transaction != null)
				    Conn = transaction.Connection;
			}
		}
		public void Dispose()
		{
			transaction?.Dispose();
		    Conn?.Dispose();
			transaction = null;
		    Conn = null;
		}
	}
}
