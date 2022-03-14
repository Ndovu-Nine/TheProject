using System;
using System.Collections.Generic;
using System.Linq;
using TemboContext.Base;
using TemboContext.Connection;
using TemboContext.Models;
using TemboContext.Operations;

namespace TemboContext.CO
{
    public class StrategyCO : StrategyOperation
    {

        public StrategyCO()
        {
        }
        public StrategyCO(DbBase operationsProvider)
        {
            if (operationsProvider?.ConnectionContainer?.Connection!=null)
            {
                LeaveOpen = operationsProvider.LeaveOpen;
                ConnectionContainer = operationsProvider.ConnectionContainer;
            }
        }
        public StrategyCO(ConnectionContainer connectionContainer, bool leaveOpen)
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
        public List<Strategy> GetSince(DateTime since, bool mode=false)
        {
            return !mode
                ? Query<Strategy>(SqlSelectCommand + " WHERE dateCreated>=@DateCreated", new { DateCreated = since }).ToList()
                : Query<Strategy>(SqlSelectCommand + " WHERE lastUpdate>=@LastUpdate", new { LastUpdate = since }).ToList();
        }		
    }
}