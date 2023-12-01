using System.Security.Principal;

namespace NIGetData.models
{
    public class DataModel
    {
         
       //public int NETWORK_SID { get; set;}
       //public DateTime DATETIME_KEY { get; set; }

        public DateTime Time { get; set; }

        public string? NeAlias { get; set; }

        public string? NeType { get; set; }
        public float RSL_INPUT_POWER { get; set; }
        public float MaxRxLevel { get; set;}

        public float RSL_Deviation { get; set;}
    }
}
