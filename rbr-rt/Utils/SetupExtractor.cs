using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rbr_rt.Utils
{
    public class SetupExtractor
    {
        private int startIndex;
        private int endIndex;
        private byte[] replayByteArray;

        public SetupExtractor()
        {
            startIndex = 0;
            endIndex = 0;
        }

        public bool FindSetup(string replayPath)
        {
            replayByteArray = File.ReadAllBytes(replayPath);
            (startIndex, endIndex) = MiscTools.FindSetupIndex(ref replayByteArray);
            return (startIndex != 0 && endIndex != 0);

        }

        public bool SaveSetup(string setupPath)
        {
            byte[] setup = replayByteArray.Skip(startIndex).Take(endIndex - startIndex + 1).ToArray();
            try
            {
                File.WriteAllBytes(setupPath, setup);
                return true;
            }
            catch
            {
                return false;
            }

        }
    }
}
