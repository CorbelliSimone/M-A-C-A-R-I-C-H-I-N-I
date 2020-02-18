using System;

namespace M_A__C_A_R_I_C_H_I_N_I.Model
{
    public class TntModel
    {
        public string DATA { get; set; }
        public string HASH { get; set; }
        public int TOPIC { get; set; }
        public int POST { get; set; }
        public string AUTORE { get; set; }
        public string TITOLO { get; set; }
        public string DESCRIZIONE { get; set; }
        public long DIMENSIONE { get; set; }
        public int CATEGORIA { get; set; }
        public string TorrentLink { get { return "magnet:?xt=urn:btih:"+HASH; } }

        public static TntModel FromCsv(string csvLine)
        {
            string[] values = csvLine.Split('~');//~
            TntModel tntModel = new TntModel();
            //tntModel.DATA = Convert.ToString(values[0]);
            tntModel.HASH = Convert.ToString(values[1]);
            //tntModel.TOPIC = Convert.ToInt32(values[2]);
            //tntModel.POST = Convert.ToInt32(values[3]);
            //tntModel.AUTORE = Convert.ToString(values[4]);
            tntModel.TITOLO = Convert.ToString(values[5]);
            tntModel.DESCRIZIONE = Convert.ToString(values[6]);
            //tntModel.DIMENSIONE = Convert.ToInt64(values[7]);
            //tntModel.CATEGORIA = Convert.ToInt32(values[8]);
            return tntModel;
        }
    }
}
