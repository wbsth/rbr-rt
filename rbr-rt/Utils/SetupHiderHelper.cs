using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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

            (startIndex, endIndex) = MiscTools.FindSetupIndex(ref replayByteArray);
            string replayString = Encoding.ASCII.GetString(replayByteArray.Skip(startIndex).Take(endIndex - startIndex + 1).ToArray());
            string setupString = Encoding.ASCII.GetString(setupByteArray.ToArray());
            var replayValues = ExtractIntegers(replayString, ref valuesIndexes);
            var setValues = ExtractIntegers(setupString, ref setupIndexes);
            //Replace(replayValues, setValues, valuesIndexes, setupIndexes, outPath);
            var valuesRead = ReadValues(setupString);
            var replayValuesRead = ReadValues(replayString);
            if (valuesRead.Count > 0)
            {
                ReplaceValues(valuesRead, replayString);
                return true;
            }

            return false;
        }

        private List<(string, float)> ReadValues(string inputString)
        {
            var tupleList = new List<(string, float)>();

            var splitted = Regex.Split(inputString, "\r\n|\r|\n");
            foreach (var line in splitted)
            {
                var removedTabsLine = Regex.Split(line,"\\t|\\s");
                var valuesList = removedTabsLine.Where(s => !string.IsNullOrWhiteSpace(s))
                    .Select(x=>x.Trim())
                    .ToList();

                if (valuesList.Count == 2)
                {
                    float settingValue;
                    string settingName = valuesList[0];
                    if (float.TryParse(valuesList[1], out settingValue))
                    {
                        // parse success
                        tupleList.Add((settingName, settingValue));
                    }
                }
            }

            return tupleList;
        }

        private void ReplaceValues(List<(string, float)> sourceValues, string replaySetupString)
        {
            foreach (var item in sourceValues)
            {
                // search for item in replay setup file
                var indexFound = replaySetupString.IndexOf(item.Item1);
                if (indexFound != -1)
                {

                }
            }
        }
        //private void Replace(List<string> replay, List<string> setup, List<int> repInd, List<int> setInd, string outPath)
        //{
        //    for (int i = 0; i < replay.Count; i++)
        //    {
        //        if (replay[i] != setup[i])
        //        {
        //            if (replay[i].Length == setup[i].Length)
        //            {
        //                byte[] temp = Encoding.ASCII.GetBytes(setup[i]);
        //                Array.Copy(temp, 0, replayByteArray, startIndex + repInd[i], temp.Length);
        //            }
        //            else if (setup[i].Contains('.') && replay[i].Contains('.'))
        //            {
        //                int lengthDifference = setup[i].Length - replay[i].Length;
        //                int zeroes = setup[i].Split('.')[1].Length;
        //                float parsed = float.Parse(setup[i]);
        //                var formatted = parsed.ToString($"F{zeroes - lengthDifference}");
        //                byte[] temp = Encoding.ASCII.GetBytes(formatted);
        //                Array.Copy(temp, 0, replayByteArray, startIndex + repInd[i], temp.Length);
        //            }
        //        }

        //    }
        //    File.WriteAllBytes(outPath, replayByteArray);
        //}

        private static List<string> ExtractIntegers(string text, ref List<int> indexes)
        {
            List<string> tempList = new List<string> { };
            string tempString = "";
            int indexStart = -1;
            for (int i = 0; i < text.Length; i++)
            {
                if (char.IsDigit(text[i]) || text[i] == '.')
                {
                    tempString += text[i];
                    if (indexStart == -1)
                        indexStart = i;
                }
                else if (tempString.Length > 0)
                {
                    if (!char.IsLetter(text[indexStart - 1]))
                    {
                        tempList.Add(tempString);
                        indexes.Add(indexStart);
                    }
                    tempString = "";
                    indexStart = -1;
                }
            }
            return tempList;
        }

    }
}
