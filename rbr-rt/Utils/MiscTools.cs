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
        public static byte[]? ReadSetupFromReplay(string replayPath)
        {
            byte[] replayByteArray = File.ReadAllBytes(replayPath);
            var indexList = FindSetupIndex(ref replayByteArray);
            if (!indexList.Contains(0))
            {
                return replayByteArray;
            }
            return null;
        }

        /// <summary> Returns indexes of bytes starting and ending setup part in replay </summary>
        public static List<int> FindSetupIndex(ref byte[] bytes)
        {
            byte[] currentStart = new byte[3];
            byte[] currentEnd = new byte[5];
            byte[] startbytes = new byte[] { 0x28, 0x28, 0x22 };
            byte[] endbytes = new byte[] { 0x29, 0x0A, 0x20, 0x29, 0x29 };
            var maxSearchRange = bytes.Length - 1;
            List<int> indexList = new List<int> { 0, 0 };

            for (var index = 0; index < maxSearchRange; index++)
            {
                for (var i = 0; i < 3; i++)
                {
                    currentStart[i] = bytes[index + i];
                }

                if ((startbytes).SequenceEqual(currentStart))
                {
                    indexList[0] = index;
                    break;
                }

            }

            for (var index = indexList[0]; index < maxSearchRange; index++)
            {
                for (var i = 0; i < 5; i++)
                {
                    currentEnd[i] = bytes[index + i];
                }

                if ((endbytes).SequenceEqual(currentEnd))
                {
                    indexList[1] = index + 4;
                    break;
                }
            }

            return indexList;
        }

    }


}
