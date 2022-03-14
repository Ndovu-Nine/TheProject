using System;
using System.Collections.Generic;
using System.Linq;
using TemboContext.Base;
using TemboContext.Connection;
using TemboContext.Models;

namespace TemboContext.Operations
{

	public class AssetOperation : DbBase
	{
		
		public const string SqlTableName = "asset";
		public const string SqlSelectCommand = "SELECT * FROM " + SqlTableName;
		public const string SqlInsertCommand = "INSERT INTO " + SqlTableName + " (name , dateCreated , lastUpdate) VALUES (@Name , @DateCreated , @LastUpdate) ";
		public const string SqlUpdateCommand = "UPDATE " + SqlTableName + " SET name=@Name , dateCreated=@DateCreated , lastUpdate=@LastUpdate WHERE SYSID=@SYSID";
		public const string SqlDeleteCommand = "DELETE FROM " + SqlTableName + " WHERE SYSID=@SYSID";
		

		public AssetOperation()
		{
		}
		public AssetOperation(DbBase operationsProvider)
		{
			if (operationsProvider?.ConnectionContainer?.Connection!=null)
			{
				LeaveOpen = operationsProvider.LeaveOpen;
				ConnectionContainer = operationsProvider.ConnectionContainer;
			}
		}
		public AssetOperation(ConnectionContainer connectionContainer, bool leaveOpen)
		{
			if (connectionContainer?.Connection != null)
			{
				LeaveOpen = leaveOpen;
				ConnectionContainer = connectionContainer;
			}
		}
		/// <summary>
		/// Returns all records from the table.
		/// Please be aware that any predicate cannot be applied to the returned IEnumurable and it will allways read all records.
		/// </summary>
		/// <returns>Delayed read all records from the database table.</returns>
		public IEnumerable<Asset> All()
		{
			return Query<Asset>(SqlSelectCommand);
		}

    /// <summary>
        /// sparkles - get n records from
        ///Beware if you don't have a SYSID column 
        /// </summary>
        /// <returns>Delayed read all records from the database table.</returns>
        public IEnumerable<Asset> All(int offset, int count)
        {
            return Query<Asset>($"SELECT TOP {count} * FROM {SqlTableName} WHERE sysid>={offset}");
        }
		

		public Asset GetByNameIndex(String name)
		{
			return Query<Asset>(SqlSelectCommand + " WHERE name=@Name", new { Name = name }).FirstOrDefault();
		}

		public int DeleteByName(String name)
		{
			return Execute(SqlDeleteCommand, new { Name = name });
		}
		
		
		
		public Asset GetBySYSID(Int32 sysid)
		{
			return Query<Asset>(SqlSelectCommand + " WHERE SYSID=@SYSID", new { SYSID = sysid }).FirstOrDefault();
		}

		public IEnumerable<Asset> GetLatest(int count)
		{
		var source = Query<Asset>($"SELECT TOP {count} * FROM {SqlTableName} ORDER BY dateCreated DESC");
		return source as List<Asset> ?? source.ToList();
		}
/*Insert that bitch*/
		public int Persist(Asset model)
		{
			return Execute(SqlInsertCommand, model);
		}
    public int Insert(Asset model)
		{
			return Execute(SqlInsertCommand, model);
		}
/*Insert that bitch*/
		public int Persist(IEnumerable<Asset> models)
		{
			return Execute(SqlInsertCommand, models);
		}
    public int Insert(IEnumerable<Asset> models)
		{
			return Execute(SqlInsertCommand, models);
		}
/*delete that bitch*/
		public int Forget(Int32 sysid)
		{
			return Execute(SqlDeleteCommand, new { SYSID = sysid});
		}
    public int Delete(Int32 sysid)
		{
			return Execute(SqlDeleteCommand, new { SYSID = sysid});
		}
/*I bet 950k that this(update) will stay language neutral 
  * until human extinction, when metal is the 
  * only thing that can survive outside!
  ---------------------------------------------------------
  * Since you won't be there build something that will roam
  * that world in your spirit and essence
  * other_page
  * btw in the currency that will rule that metal extinction
  */
		public int Update(Asset model)
		{
			return Execute(SqlUpdateCommand, model);
		}
/*I bet 950k that this(update) will stay language neutral 
  * until human extinction, when metal is the 
  * only thing that can survive outside!
  ---------------------------------------------------------
  * Since you won't be there build something that will roam
  * that world in your spirit and essence
  * other_page
  * btw in the currency that will rule that metal extinction
  */
		public int Update(IEnumerable<Asset> models)
		{
			return Execute(SqlUpdateCommand, models);
		}
	}
}
