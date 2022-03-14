
using System;
using System.Collections.Generic;
using System.Data;
using TemboContext.Connection;
using TemboContext.Dapper;
namespace TemboContext.Base
{
	public abstract class DbBase : IDisposable
	{
		protected internal DbBase()
		{
			ConnectionContainer = new ConnectionContainer()
			{
				Connection = DbConnection.CreateConnection()
			};
		}
		protected internal DbBase(ConnectionContainer container, bool leaveOpen)
		{
			LeaveOpen = leaveOpen;
			ConnectionContainer = container;
		}
        public ConnectionContainer ConnectionContainer { get; protected set; }
        protected internal bool LeaveOpen { get; set; }
        public IDbTransaction BeginTransaction()
		{
			ConnectionContainer.Transaction = ConnectionContainer.Connection.BeginTransaction();
			return ConnectionContainer.Transaction;
		}
		public IDbTransaction BeginTransaction(IsolationLevel isolationLevel)
		{
			ConnectionContainer.Transaction = ConnectionContainer.Connection.BeginTransaction(isolationLevel);
			return ConnectionContainer.Transaction;
		}
		/// <summary>
		/// Execute parameterized SQL  
		/// When does this guy commit?
		/// other_page
		/// </summary>
		/// <returns>Number of rows affected</returns>
		public int Execute(string sql, dynamic param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
		{
			try
			{
				if (transaction == null)
					transaction = ConnectionContainer.Transaction;
				OpenConnection();
				var query = SqlMapper.Execute(ConnectionContainer.Connection, sql, param, transaction, commandTimeout, commandType);
				return query;
			}
			catch (Exception ex)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine(ex.Message);
				Console.ForegroundColor = ConsoleColor.White;
				return -999;
			}
		}
		/// <summary>
		/// Return a list of dynamic objects, reader is closed after the call
		/// </summary>
		public IEnumerable<dynamic> Query(string sql, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
		{
			if (transaction == null)
				transaction = ConnectionContainer.Transaction;
			OpenConnection();
			var query = SqlMapper.Query(ConnectionContainer.Connection, sql, param, transaction, buffered, commandTimeout, commandType);
			return AssignOwner(query);
		}
		/// <summary>
		/// Executes a query, returning the data typed as per T
		/// </summary>
		/// <remarks>the dynamic param may seem a bit odd, but this works around a major usability issue in vs, if it is Object vs completion gets annoying. Eg type new [space] get new object</remarks>
		/// <returns>A sequence of data of the supplied type; if a basic type (int, string, etc) is queried then the data from the first column in assumed, otherwise an instance is
		/// created per row, and a direct column-name===member-name mapping is assumed (case insensitive).
		/// </returns>
		public IEnumerable<T> Query<T>(string sql, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
		{
			if (transaction == null)
				transaction = ConnectionContainer.Transaction;
			OpenConnection();
			var query = SqlMapper.Query<T>(ConnectionContainer.Connection, sql, param, transaction, buffered, commandTimeout, commandType);
			return AssignOwner(query);
		}
		/// <summary>
		/// Maps a query to objects
		/// </summary>
		public IEnumerable<TReturn> Query<TFirst, TSecond, TReturn>(string sql, Func<TFirst, TSecond, TReturn> map, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
		{
			if (transaction == null)
				transaction = ConnectionContainer.Transaction;
			OpenConnection();
			var query = SqlMapper.Query<TFirst, TSecond, TReturn>(ConnectionContainer.Connection, sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
			return AssignOwner(query);
		}
		/// <summary>
		/// Perform a multi mapping query with 5 input parameters
		/// </summary>
		public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
		{
			if (transaction == null)
				transaction = ConnectionContainer.Transaction;
			OpenConnection();
			var query = SqlMapper.Query<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(ConnectionContainer.Connection, sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
			return AssignOwner(query);
		}
		/// <summary>
		/// Perform a multi mapping query with 4 input parameters
		/// </summary>
		public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
		{
			if (transaction == null)
				transaction = ConnectionContainer.Transaction;
			OpenConnection();
			var query = SqlMapper.Query<TFirst, TSecond, TThird, TFourth, TReturn>(ConnectionContainer.Connection, sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
			return AssignOwner(query);
		}
		/// <summary>
		/// Maps a query to objects
		/// </summary>
		public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TReturn>(string sql, Func<TFirst, TSecond, TThird, TReturn> map, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
		{
			if (transaction == null)
				transaction = ConnectionContainer.Transaction;
			OpenConnection();
			var query = SqlMapper.Query<TFirst, TSecond, TThird, TReturn>(ConnectionContainer.Connection, sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
			return AssignOwner(query);
		}
		/// <summary>
		/// Execute a command that returns multiple result sets, and access each in turn
		/// </summary>
		public SqlMapper.GridReader QueryMultiple(string sql, dynamic param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
		{
			if (transaction == null)
				transaction = ConnectionContainer.Transaction;
			OpenConnection();
			return SqlMapper.QueryMultiple(ConnectionContainer.Connection, sql, param, transaction, commandTimeout, commandType);
		}
		protected void OpenConnection()
		{
			if (ConnectionContainer.Connection != null && ConnectionContainer.Connection.State != ConnectionState.Open)
				ConnectionContainer.Connection.Open();
		}
		protected void CloseConnection()
		{
			if (ConnectionContainer.Connection != null)
				ConnectionContainer.Connection.Close();
		}
		protected IEnumerable<T> AssignOwner<T>(IEnumerable<T> data)
		{
			return !typeof(T).IsSubclassOf(typeof(BaseModel)) ? data : new DapperModelQuery<T>(data, ConnectionContainer);
		}
		protected IList<T> AssignOwner<T>(IList<T> data)
		{
			if (!typeof(T).IsSubclassOf(typeof(BaseModel)))
			{
				return data;
			}
			for (int i = 0; i < data.Count; i++)
			{
				var baseModel = data[i] as BaseModel;
				if (baseModel != null)
					baseModel.DbConnection = ConnectionContainer;
			}
			return data;
		}
		protected void AssignListOwner<T>(IList<T> data) where T : BaseModel
		{
			for (int i = 0; i < data.Count; i++)
			{
				data[i].DbConnection = ConnectionContainer;
			}
		}
		protected void AssignModelOwner<T>(T data) where T : BaseModel
		{
			if (data != null)
				data.DbConnection = ConnectionContainer;
		}
		public void Dispose()
		{
			if (!LeaveOpen)
			{
				ConnectionContainer.Dispose();
			}
			ConnectionContainer = null;
		}
	}
}
