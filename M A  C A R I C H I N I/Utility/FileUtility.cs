using M_A__C_A_R_I_C_H_I_N_I.Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace M_A__C_A_R_I_C_H_I_N_I.Utility
{
    public static class FileUtility
    {
        public static string FilePath { get; set; }
        public static IList<TntModel> OpenFile()
        {
            //FilePath = Path.Combine(FilePath);
            return File.ReadAllLines(FilePath)
                                        .Skip(1)
                                        .Select(v => TntModel.FromCsv(v))
                                        .OrderBy(x=>x.TITOLO)
                                        .ToList();
            //return values;
        }
    }
}
