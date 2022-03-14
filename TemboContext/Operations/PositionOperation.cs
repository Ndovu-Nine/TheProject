using System;
using System.Collections.Generic;
using System.Linq;
using TemboContext.Base;
using TemboContext.Connection;
using TemboContext.Models;

namespace TemboContext.Operations
{
    public class PositionOperation : DbBase
    {

        public const string SqlTableName = "position";
        public const string SqlSelectCommand = "SELECT * FROM " + SqlTableName;
        public const string SqlInsertCommand = "INSERT INTO " + SqlTableName + " (assetSYSID , strategySYSID , durationOfCandle , openTime , endTime , durationOfTrade , openPrice , closePrice , direction , fractal , macd , rainbow , rsi , stoch , wpr , trendA , trendB , trendC , trendD , trendE , trendF , outcome , isSent , dateCreated , lastUpdate) VALUES (@AssetSYSID , @StrategySYSID , @DurationOfCandle , @OpenTime , @EndTime , @DurationOfTrade , @OpenPrice , @ClosePrice , @Direction , @Fractal , @Macd , @Rainbow , @Rsi , @Stoch , @Wpr , @TrendA , @TrendB , @TrendC , @TrendD , @TrendE , @TrendF , @Outcome , @IsSent , @DateCreated , @LastUpdate) ";
        public const string SqlUpdateCommand = "UPDATE " + SqlTableName + " SET assetSYSID=@AssetSYSID , strategySYSID=@StrategySYSID , durationOfCandle=@DurationOfCandle , openTime=@OpenTime , endTime=@EndTime , durationOfTrade=@DurationOfTrade , openPrice=@OpenPrice , closePrice=@ClosePrice , direction=@Direction , fractal=@Fractal , macd=@Macd , rainbow=@Rainbow , rsi=@Rsi , stoch=@Stoch , wpr=@Wpr , trendA=@TrendA , trendB=@TrendB , trendC=@TrendC , trendD=@TrendD , trendE=@TrendE , trendF=@TrendF , outcome=@Outcome , isSent=@IsSent , dateCreated=@DateCreated , lastUpdate=@LastUpdate WHERE SYSID=@SYSID";
        public const string SqlDeleteCommand = "DELETE FROM " + SqlTableName + " WHERE SYSID=@SYSID";



        public PositionOperation()
        {
        }
        public PositionOperation(DbBase operationsProvider)
        {
            if (operationsProvider?.ConnectionContainer?.Connection!=null)
            {
                LeaveOpen = operationsProvider.LeaveOpen;
                ConnectionContainer = operationsProvider.ConnectionContainer;
            }
        }
        public PositionOperation(ConnectionContainer connectionContainer, bool leaveOpen)
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
        public IEnumerable<Position> All()
        {
            return Query<Position>(SqlSelectCommand);
        }

        /// <summary>
        /// sparkles - get n records from
        ///Beware if you don't have a SYSID column 
        /// </summary>
        /// <returns>Delayed read all records from the database table.</returns>
        public IEnumerable<Position> All(int offset, int count)
        {
            return Query<Position>($"SELECT TOP {count} * FROM {SqlTableName} WHERE sysid>={offset}");
        }
		
		
		
        /// <summary>
        /// position - assetSYSID
        /// </summary>
        public List<Position> GetByAssetSYSID(Int32 assetSYSID)
        {
            return Query<Position>(SqlSelectCommand + " WHERE assetSYSID=@AssetSYSID", new { AssetSYSID = assetSYSID }).ToList();
        }
        /// <summary>
        /// position - strategySYSID
        /// </summary>
        public List<Position> GetByStrategySYSID(Int32 strategySYSID)
        {
            return Query<Position>(SqlSelectCommand + " WHERE strategySYSID=@StrategySYSID", new { StrategySYSID = strategySYSID }).ToList();
        }
		
        public Position GetBySYSID(Int32 sysid)
        {
            return Query<Position>(SqlSelectCommand + " WHERE SYSID=@SYSID", new { SYSID = sysid }).FirstOrDefault();
        }

        public IEnumerable<Position> GetLatest(int count)
        {
            var source = Query<Position>($"SELECT TOP {count} * FROM {SqlTableName} ORDER BY dateCreated DESC");
            return source as List<Position> ?? source.ToList();
        }
/*Insert that bitch*/
        public int Persist(Position model)
        {
            return Execute(SqlInsertCommand, model);
        }
        public int Insert(Position model)
        {
            return Execute(SqlInsertCommand, model);
        }
/*Insert that bitch*/
        public int Persist(IEnumerable<Position> models)
        {
            return Execute(SqlInsertCommand, models);
        }
        public int Insert(IEnumerable<Position> models)
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
        public int Update(Position model)
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
        public int Update(IEnumerable<Position> models)
        {
            return Execute(SqlUpdateCommand, models);
        }
    }
}