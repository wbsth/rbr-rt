using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace rbr_rt.Utils
{
    public class SetupHiderHelper
    {
        public SetupHiderHelper()
        {
            startIndex = 0;
            endIndex = 0;
        }

        private int startIndex;
        private int endIndex;
        private byte[] replayByteArray;
        private byte[] setupByteArray;

        public bool HideSetup(string replayPath, string setupPath, string outPath)
        {
            List<int> valuesIndexes = new List<int> { };
            List<int> setupIndexes = new List<int> { };

            // loading replay and setups files into arrays
            replayByteArray = File.ReadAllBytes(replayPath);
            setupByteArray = File.ReadAllBytes(setupPath);

            // find setup file byte index (where it starts, where it ends)
            (startIndex, endIndex) = MiscTools.FindSetupIndex(ref replayByteArray);
            
            // loads setup from replay file into string
            string replayString = Encoding.ASCII.GetString(replayByteArray.Skip(startIndex).Take(endIndex - startIndex + 1).ToArray());
            
            // load setup from setup file into string
            string setupString = Encoding.ASCII.GetString(setupByteArray.ToArray());
            
            // extract values from both setups
            var replayValues = ExtractIntegers(replayString, ref valuesIndexes);
            var setValues = ExtractIntegers(setupString, ref setupIndexes);
            
            // check if number of extracted values matches
            if (replayValues.Count != setValues.Count) return false;
            
            // replace replay setup, with provided one
            Replace(replayValues, setValues, valuesIndexes, setupIndexes, outPath);
            return true;
        }
        
        private void Replace(List<string> replay, List<string> setup, List<int> repInd, List<int> setInd, string outPath)
        {
            // iterate through values extracted from replay file
            for (int i = 0; i < replay.Count; i++)
            {
                // if value should be changed (because it differs from the provided setup value)
                if (replay[i] != setup[i])
                {
                    // if values are of the same length, simply copy it
                    if (replay[i].Length == setup[i].Length)
                    {
                        byte[] temp = Encoding.ASCII.GetBytes(setup[i]);
                        Array.Copy(temp, 0, replayByteArray, startIndex + repInd[i], temp.Length);
                    }
                    // if values length is different, some parsing is needed
                    else if (setup[i].Contains('.') && replay[i].Contains('.'))
                    {
                        int lengthDifference = setup[i].Length - replay[i].Length;
                        int zeroes = setup[i].Split('.')[1].Length;
                        float parsed = float.Parse(setup[i], CultureInfo.InvariantCulture);
                        var formatted = parsed.ToString($"F{zeroes - lengthDifference}");
                        byte[] temp = Encoding.ASCII.GetBytes(formatted);
                        Array.Copy(temp, 0, replayByteArray, startIndex + repInd[i], temp.Length);
                    }
                }

            }
            File.WriteAllBytes(outPath, replayByteArray);
        }

        private static List<string> ExtractIntegers(string text, ref List<int> indexes)
        {
            List<string> extractedValuesList = new List<string> { };
            string tempString = "";
            int indexStart = -1;
            
            // iterate through provided string
            for (int i = 0; i < text.Length; i++)
            {
                // if character is a digit, or a decimal sign, write it to temporary string
                if (char.IsDigit(text[i]) || text[i] == '.')
                {
                    tempString += text[i];
                    if (indexStart == -1)
                        indexStart = i;
                }
                // if not, check if there is any data in temporary string
                else if (tempString.Length > 0)
                {
                    // make sure that numerical value is not a part of text
                    if (!char.IsLetter(text[indexStart - 1]) && text[indexStart - 1] != '_')
                    {
                        extractedValuesList.Add(tempString);
                        indexes.Add(indexStart);
                    }
                    tempString = "";
                    indexStart = -1;
                }
            }
            return extractedValuesList;
        }

    }
}
