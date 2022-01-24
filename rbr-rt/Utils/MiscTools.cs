using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rbr_rt.Utils
{
    public static class MiscTools
    {
        //public static byte[]? ReadSetupFromReplay(string replayPath)
        //{
        //    byte[] replayByteArray = File.ReadAllBytes(replayPath);
        //    var indexList = FindSetupIndex(ref replayByteArray);
        //    if (!indexList.Contains(0))
        //    {
        //        return replayByteArray;
        //    }
        //    return null;
        //}

        /// <summary> Returns indexes of bytes starting and ending setup part in replay </summary>
        public static (int, int) FindSetupIndex(ref byte[] bytes)
        {
            var currentStart = new byte[3];
            var currentEnd = new byte[5];
            var startBytes = new byte[] { 0x28, 0x28, 0x22 };
            var endBytes = new byte[] { 0x29, 0x0A, 0x20, 0x29, 0x29 };
            var maxSearchRange = bytes.Length - 1;

            var startIndex = 0;
            var endIndex = 0;

            for (var index = 0; index < maxSearchRange; index++)
            {
                for (var i = 0; i < 3; i++)
                {
                    currentStart[i] = bytes[index + i];
                }

                if ((startBytes).SequenceEqual(currentStart))
                {
                    startIndex = index;
                    break;
                }

            }

            for (var index = startIndex; index < maxSearchRange; index++)
            {
                for (var i = 0; i < 5; i++)
                {
                    currentEnd[i] = bytes[index + i];
                }

                if ((endBytes).SequenceEqual(currentEnd))
                {
                    endIndex = index + 4;
                    break;
                }
            }

            return (startIndex, endIndex);
        }

    }


}
