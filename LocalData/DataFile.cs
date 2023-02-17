using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LocalData
{
    internal class DataFile
    {
        private static readonly CryptFileHandler MyEncryptedFile = new CryptFileHandler("date.bin", "A 32 CHARACTER PASSWORD !@#$%¨&*", "16CHARACTERPASSW");
        private static readonly string VariablePrefix = "#54FV_";

        public static void Clear() => System.IO.File.Delete(MyEncryptedFile.FilePath);

        public static bool verifyExistCFG(string variable)
        {          
            variable = VariablePrefix + variable + "=";
            if (!System.IO.File.Exists(MyEncryptedFile.FilePath))
            {
                return false;
            }
            string ss = MyEncryptedFile.LoadString();
            if (!ss.Contains(variable))
            {
                return false;
            }
            return true;
        }

        public static string loadCfg(string variable, string valueDefull = "")
        {
            variable = VariablePrefix + variable + "=";
            if (!verifyExistCFG(variable.Replace(VariablePrefix, "").Replace("=", "").Trim()))
            {
                Save(variable.Replace(VariablePrefix, "").Replace("=", "").Trim(), valueDefull);
                return valueDefull;
            }
            string[] MyCfg = MyEncryptedFile.LoadString().Replace("\0", "").Split('\n');
            string resut = MyCfg.ToList().FirstOrDefault(x => x.Contains(variable)).Split('=')[1];
            return resut;
        }

        public static bool loadCfg(string variable, bool valueDefull)
        {
            string res = loadCfg(variable, valueDefull ? "1" : "0");
            return res.Contains('1');
        }
        public static int loadCfg(string variable, int valueDefull)
        {
            try
            {
                string res = loadCfg(variable, valueDefull.ToString());
                return Convert.ToInt32(res);
            }
            catch
            {
                return valueDefull;
            }
        }

        private static string GetVariableKey(string variable)
        {
            return variable = VariablePrefix + variable + "=";
        }
        public static bool Save(string variable, string value)
        {
            string variableKey = GetVariableKey(variable);

            if (!System.IO.File.Exists(MyEncryptedFile.FilePath))
            {
                MyEncryptedFile.SaveString(variableKey + "=" + value);
                return true;
            }

            string[] lines = MyEncryptedFile.LoadString().Split('\n');
            int index = Array.FindIndex(lines, line => line.StartsWith(variableKey));

            if (index == -1)
            {
                MyEncryptedFile.SaveString(variableKey + "=" + value + "\n" + MyEncryptedFile.LoadString());
                return true;
            }

            lines[index] = variableKey + "=" + value;
            MyEncryptedFile.SaveString(string.Join("\n", lines));
            return true;
        }

        public static bool Save(string variable, bool value) => Save(variable, value ? "1" : "0");

        public static bool Save(string variable, int value) => Save(variable, value.ToString());
    }
}
