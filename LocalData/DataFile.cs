using System;
using System.Collections.Generic;
using System.Linq;

namespace LocalData
{
    public class DataFile
    {
        private readonly CryptFileHandler MyEncryptedFile;
        private readonly string VariablePrefix;
        public DataFile()
        {
            MyEncryptedFile = new CryptFileHandler("date.bin", "A-32-CHARACTER-PASSWORD-!@#$%¨&*", "16CHARACTERPASSW");
            VariablePrefix = "#PRE_";
        }
        public DataFile(string filePath, string key, string iv, string customPrefix = null)
        {
            MyEncryptedFile = new CryptFileHandler(filePath, key, iv);
            if (customPrefix == null)
                VariablePrefix = "#PRE_";
            else
                VariablePrefix = customPrefix;
        }
        public void Clear() => System.IO.File.Delete(MyEncryptedFile.FilePath);

        public bool VerifyExistCFG(string variable)
        {
            variable = _getVariableKey(variable);
            if (!System.IO.File.Exists(MyEncryptedFile.FilePath))
            {
                return false;
            }
            string fileContent = MyEncryptedFile.LoadString();
            return fileContent.Contains(variable);
        }

        public T LoadCfg<T>(string variable, T defaultValue)
        {
            string result = _loadCfg(variable, defaultValue.ToString());
            return (T)Convert.ChangeType(result, typeof(T));
        }

        public bool SaveCfg<T>(string variable, T value)
        {
            string stringValue = Convert.ToString(value);
            return _save(variable, stringValue);
        }
        public IEnumerable<string> GetAllVariableNames()
        {
            string[] lines = MyEncryptedFile.LoadString().Replace("\0", "").Split('\n');
            foreach (string line in lines)
            {
                int index = line.IndexOf('=');
                if (index > 0 && line.StartsWith(VariablePrefix))
                {
                    yield return line.Substring(VariablePrefix.Length, index - VariablePrefix.Length);
                }
            }
        }

        private string _getVariableKey(string variable)
        {
            return VariablePrefix + variable + "=";
        }

        private bool _save(string variable, string value)
        {
            string variableKey = _getVariableKey(variable);

            if (!System.IO.File.Exists(MyEncryptedFile.FilePath))
            {
                MyEncryptedFile.SaveString(variableKey + value);
                return true;
            }

            string[] lines = MyEncryptedFile.LoadString().Split('\n');
            int index = Array.FindIndex(lines, line => line.StartsWith(variableKey));

            if (index == -1)
            {
                MyEncryptedFile.SaveString(variableKey + value + "\n" + MyEncryptedFile.LoadString());
                return true;
            }

            lines[index] = variableKey + value;
            MyEncryptedFile.SaveString(string.Join("\n", lines));
            return true;
        }
        private string _loadCfg(string variable, string defaultValue = "")
        {
            variable = _getVariableKey(variable);
            if (!VerifyExistCFG(variable.Replace(VariablePrefix, "").Replace("=", "").Trim()))
            {
                _save(variable.Replace(VariablePrefix, "").Replace("=", "").Trim(), defaultValue);
                return defaultValue;
            }
            string[] cfgLines = MyEncryptedFile.LoadString().Replace("\0", "").Split('\n');
            string result = cfgLines.FirstOrDefault(x => x.Contains(variable)).Split('=')[1];
            return result;
        }
    }
}
