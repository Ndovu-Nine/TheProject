using System.Collections;
using System.Collections.Generic;
using TemboContext.Connection;
using TemboContext.Base;
namespace TemboContext.Dapper
{
	/// <summary>
	/// Assigns the owner property of the results
	/// </summary>
	class DapperModelQuery<T> : IEnumerable<T>, IEnumerator<T>
	{
		private readonly IEnumerable<T> query;
		private readonly ConnectionContainer dbConnection;
		private IEnumerator<T> queryEnumerator;

		public DapperModelQuery(IEnumerable<T> query, ConnectionContainer dbConnection)
		{
			this.query = query;
			this.dbConnection = dbConnection;
		}

		public IEnumerator<T> GetEnumerator()
		{
			queryEnumerator = query.GetEnumerator();
			return this;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void Dispose()
		{
		    queryEnumerator?.Dispose();
		}

		public bool MoveNext()
		{
			return queryEnumerator.MoveNext();
		}

		public void Reset()
		{
			queryEnumerator.Reset();
		}

		public T Current
		{
			get
			{
				var c = queryEnumerator.Current;
                if (c is BaseModel cBase)
					cBase.DbConnection = dbConnection;
				return c;
			}
		}

		object IEnumerator.Current => Current;
	}
	}
