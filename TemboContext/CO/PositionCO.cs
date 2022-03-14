using System;
using System.Collections.Generic;
using System.Linq;
using TemboContext.Base;
using TemboContext.Connection;
using TemboContext.Models;
using TemboContext.Operations;

namespace TemboContext.CO
{
    public class PositionCO : PositionOperation
    {

        public PositionCO()
        {
        }
        public PositionCO(DbBase operationsProvider)
        {
            if (operationsProvider?.ConnectionContainer?.Connection!=null)
            {
                LeaveOpen = operationsProvider.LeaveOpen;
                ConnectionContainer = operationsProvider.ConnectionContainer;
            }
        }
        public PositionCO(ConnectionContainer connectionContainer, bool leaveOpen)
        {
            if (connectionContainer?.Connection != null)
            {
                LeaveOpen = leaveOpen;
                ConnectionContainer = connectionContainer;
            }
        }
        /// <summary>
        /// if your data tables have a column called dateCreated, this is for you my darling*
        /// other_page
        /// </summary>
        /// <param name="since">it's in the name baby girl</param>
        /// <param name="mode">true - you want last update, false - you want date created, default- false</param>
        /// <returns></returns>
        public List<Position> GetSince(DateTime since, bool mode=false)
        {
            return !mode
                ? Query<Position>(SqlSelectCommand + " WHERE dateCreated>=@DateCreated", new { DateCreated = since }).ToList()
                : Query<Position>(SqlSelectCommand + " WHERE lastUpdate>=@LastUpdate", new { LastUpdate = since }).ToList();
        }
        public List<Position> GetByAssetAndDuration(int assetId, string duration, int count = 0, bool isSent=false)
        {
            if (count > 0)
            {
                if (isSent)
                {
                    var query = $"SELECT TOP {count} * FROM {SqlTableName} WHERE isSent=1 AND assetSYSID={assetId} AND durationOfCandle='{duration}' ORDER BY dateCreated desc";
                    return Query<Position>(query).OrderBy(t => t.DateCreated).ToList();
                }
                else
                {
                    var query = $"SELECT TOP {count} * FROM {SqlTableName} WHERE assetSYSID={assetId} AND durationOfCandle='{duration}' ORDER BY dateCreated desc";
                    return Query<Position>(query).OrderBy(t => t.DateCreated).ToList();
                }
            }
            else
            {
                if (isSent)
                {
                    var query = $"SELECT * FROM {SqlTableName} WHERE isSent=1 AND assetSYSID={assetId} AND durationOfCandle='{duration}'";
                    return Query<Position>(query).ToList();
                }
                else
                {
                    var query = $"SELECT * FROM {SqlTableName} WHERE assetSYSID={assetId} AND durationOfCandle='{duration}'";
                    return Query<Position>(query).ToList();
                }
            }
        }
    }
}