using System;
using System.Collections.Generic;
using TemboContext.Base;
using TemboContext.Operations;

namespace TemboContext.Models
{
    public class Strategy : BaseModel
    {
		
        public int SYSID { get; set; }

        public string Name { get; set; }

        public string Summary { get; set; }

        public string Details { get; set; }

        public bool IsIndicator { get; set; }

        public string IndicatorName { get; set; }

        public string IndicatorConfiguration { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime LastUpdate { get; set; }

		
        /// <summary>
        /// position - strategySYSID
        /// </summary>
        public List<Position> Position()
        {
            using (var ops = new PositionOperation(DbConnection, true))
            {
                return ops.GetByStrategySYSID(SYSID);
            }
        }
    }
}