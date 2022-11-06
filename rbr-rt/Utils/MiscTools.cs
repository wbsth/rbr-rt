using System;
using System.Linq;

namespace rbr_rt.Utils
{
    public static class MiscTools
    {
        /// <summary> Returns indexes of bytes starting and ending setup part in replay </summary>
        public static (int, int) FindSetupIndex(ref byte[] bytes)
        {
            var currentStart = new byte?[3];
            var currentEnd = new byte?[5];
            var startBytes = new byte?[] { 0x28, 0x28, 0x22 };               // bytes at the beginning of the setup file
            var endBytes = new byte?[] { 0x29, 0x0A, 0x20, 0x29, 0x29 };     // bytes at the end of the setup file
            var maxSearchRange = bytes.Length - 1;

            var startIndex = 0;
            var endIndex = 0;
            
            // iterate through file bytes to find where setup part starts
            for (var index = 0; index < maxSearchRange; index++)
            {
                // shift array one element to the left
                Array.Copy(currentStart, 1, currentStart, 0, currentStart.Length - 1);
                
                // and write current byte to last place
                currentStart[^1] = bytes[index];
                
                // if current bytes sequence equals to the setup start sequence, success
                if ((startBytes).SequenceEqual(currentStart))
                {
                    startIndex = index - startBytes.Length + 1;
                    break;
                }

            }

            // iterate through file bytes to find where setup part ends
            for (var index = startIndex; index < maxSearchRange; index++)
            {
                // shift array one element to the left
                Array.Copy(currentEnd, 1, currentEnd, 0, currentEnd.Length - 1);
                
                // and write current byte to last place
                currentEnd[^1] = bytes[index];
                
                if ((endBytes).SequenceEqual(currentEnd))
                {
                    endIndex = index;
                    break;
                }
            }

            return (startIndex, endIndex);
        }

    }


}
