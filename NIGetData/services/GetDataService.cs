using NIGetData.models;
using System.Collections.Generic;
using Vertica.Data.VerticaClient;
using System;

namespace NIGetData.services
{
    public class GetDataService
    {  
        private List<DataModel> GetDataFromReader(VerticaDataReader dataR)
        {
            var result = new List<DataModel>();

            while (dataR.Read())
            {
                var rowResult = new DataModel
                {
                    //DATETIME_KEY = dataR.GetDateTime(dataR.GetOrdinal("DATETIME_KEY")),
                    //NETWORK_SID = dataR.GetInt32(dataR.GetOrdinal("NETWORK_SID")),
                    Time = dataR.GetDateTime(dataR.GetOrdinal("Time")),
                    NeAlias = dataR.IsDBNull(dataR.GetOrdinal("NeAlias")) ? null : dataR.GetString(dataR.GetOrdinal("NeAlias")),
                    NeType = dataR.IsDBNull(dataR.GetOrdinal("NeType")) ? null : dataR.GetString(dataR.GetOrdinal("NeType")),
                    RSL_INPUT_POWER = dataR.GetFloat(dataR.GetOrdinal("RSL_INPUT_POWER")),
                    MaxRxLevel = dataR.GetFloat(dataR.GetOrdinal("MaxRxLevel")),
                    RSL_Deviation = dataR.GetFloat(dataR.GetOrdinal("RSL_Deviation")),
                };

                result.Add(rowResult);
            }

            return result;
        }

        public List<DataModel> fetchdata(BodyDataModel bodyDataModel)
        {
            VerticaConnectionStringBuilder builder = new();

            builder.Host = "10.10.4.231";
            builder.Database = "test";
            builder.Port = 5433;
            builder.User = "bootcamp9";
            builder.Password = "bootcamp92023";

            bool isAlias = bodyDataModel.Ne.Equals("NeAlias") ?   true :  false;

            string query2 = isAlias ? "null as NeType," : "null as NeAlias,";

                     string query = $@"
            SELECT 
                {bodyDataModel.Ne},
                {query2}
                Time,
                Max(RSL_INPUT_POWER) as RSL_INPUT_POWER,
                Max(MaxRxLevel) as MaxRxLevel,
                ABS(Max(MaxRxLevel)) - ABS(Max(RSL_INPUT_POWER)) as RSL_Deviation
                
            FROM  
                {bodyDataModel.table}
where
Time between '{bodyDataModel.timeFrom}' AND '{bodyDataModel.timeTo}'
            GROUP BY 
                Time, {bodyDataModel.Ne}
;";

        VerticaConnection _conn = new(builder.ToString());
            _conn.Open();


            VerticaCommand command = _conn.CreateCommand();
            command.CommandText = query;
            VerticaDataReader dataR = command.ExecuteReader();
            var result = GetDataFromReader(dataR);


            _conn.Close();

            // Combine the results from all queries

            return result;
        }
    }
}
