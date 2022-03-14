//*****************************************************************************************
//*                                                                                       *
//* This is an auto-generated file by Microsoft ML.NET CLI (Command-Line Interface) tool. *
//*                                                                                       *
//*****************************************************************************************

using Microsoft.ML.Data;

namespace TickServerML.Model.DataModels
{
    public class ModelInput
    {
        [ColumnName("SYSID"), LoadColumn(0)]
        public float SYSID { get; set; }


        [ColumnName("assetSYSID"), LoadColumn(1)]
        public float AssetSYSID { get; set; }


        [ColumnName("strategySYSID"), LoadColumn(2)]
        public float StrategySYSID { get; set; }


        [ColumnName("direction"), LoadColumn(3)]
        public bool Direction { get; set; }


        [ColumnName("openTime"), LoadColumn(4)]
        public string OpenTime { get; set; }


        [ColumnName("endTime"), LoadColumn(5)]
        public string EndTime { get; set; }


        [ColumnName("openPrice"), LoadColumn(6)]
        public float OpenPrice { get; set; }


        [ColumnName("closePrice"), LoadColumn(7)]
        public float ClosePrice { get; set; }


        [ColumnName("volume"), LoadColumn(8)]
        public float Volume { get; set; }


        [ColumnName("volumeLabel"), LoadColumn(9)]
        public float VolumeLabel { get; set; }


        [ColumnName("spreadLabel"), LoadColumn(10)]
        public float SpreadLabel { get; set; }


        [ColumnName("readingAtOpen"), LoadColumn(11)]
        public float ReadingAtOpen { get; set; }


        [ColumnName("multiplier"), LoadColumn(12)]
        public float Multiplier { get; set; }


        [ColumnName("outcome"), LoadColumn(13)]
        public bool Outcome { get; set; }


        [ColumnName("dateCreated"), LoadColumn(14)]
        public string DateCreated { get; set; }


        [ColumnName("lastUpdate"), LoadColumn(15)]
        public string LastUpdate { get; set; }


    }
}
