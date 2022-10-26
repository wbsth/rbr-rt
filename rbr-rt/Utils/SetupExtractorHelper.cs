using System.IO;
using System.Linq;

namespace rbr_rt.Utils
{
    public class SetupExtractor
    {
        private int _startIndex;
        private int _endIndex;
        private byte[] _replayByteArray;

        public SetupExtractor()
        {
            _startIndex = 0;
            _endIndex = 0;
        }

        public bool FindSetup(string replayPath)
        {
            // load file into byte array
            _replayByteArray = File.ReadAllBytes(replayPath);
            
            // find start and beginning of setup part
            (_startIndex, _endIndex) = MiscTools.FindSetupIndex(ref _replayByteArray);
            
            // return true if setup was found
            return (_startIndex != 0 && _endIndex != 0);

        }

        public bool SaveSetup(string setupPath)
        {
            // loads setup from replay file, and write it to output path
            byte[] setup = _replayByteArray.Skip(_startIndex).Take(_endIndex - _startIndex + 1).ToArray();
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
