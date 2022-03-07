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
            _replayByteArray = File.ReadAllBytes(replayPath);
            (_startIndex, _endIndex) = MiscTools.FindSetupIndex(ref _replayByteArray);
            return (_startIndex != 0 && _endIndex != 0);

        }

        public bool SaveSetup(string setupPath)
        {
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
