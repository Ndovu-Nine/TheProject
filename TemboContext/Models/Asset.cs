
using System;
using System.Collections.Generic;
using TemboContext.Base;
using TemboContext.Operations;

namespace TemboContext.Models
{
	
	public class Asset : BaseModel
	{
		
		public int SYSID { get; set; }

		public string Name { get; set; }

		public DateTime DateCreated { get; set; }

		public DateTime LastUpdate { get; set; }

		
		/// <summary>
		/// position - assetSYSID
		/// </summary>
		public List<Position> Position()
		{
			using (var ops = new PositionOperation(DbConnection, true))
			{
				return ops.GetByAssetSYSID(SYSID);
			}
		}
	}
}
