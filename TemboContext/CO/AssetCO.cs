using System;
using System.Collections.Generic;
using System.Linq;
using TemboContext.Base;
using TemboContext.Connection;
using TemboContext.Models;
using TemboContext.Operations;

namespace TemboContext.CO
{
	
	public class AssetCO : AssetOperation
	{

		public AssetCO()
		{
		}
		public AssetCO(DbBase operationsProvider)
		{
			if (operationsProvider?.ConnectionContainer?.Connection!=null)
			{
				LeaveOpen = operationsProvider.LeaveOpen;
				ConnectionContainer = operationsProvider.ConnectionContainer;
			}
		}
		public AssetCO(ConnectionContainer connectionContainer, bool leaveOpen)
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
		public List<Asset> GetSince(DateTime since, bool mode=false)
        {
			return !mode
				? Query<Asset>(SqlSelectCommand + " WHERE dateCreated>=@DateCreated", new { DateCreated = since }).ToList()
				: Query<Asset>(SqlSelectCommand + " WHERE lastUpdate>=@LastUpdate", new { LastUpdate = since }).ToList();
        }		
	}
}
