using ShaderIncludePreprocessor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ShaderIncludePreprocessor
{
    public class Shader
    {

        public static string ShaderInclude(string code, string codeSourcePath)
        {
                while (code.Contains("#include"))
                {

                    string includeToReplace = "";
                    string codeToInclude = "";
                    string importedShaderPath = "";

                    (includeToReplace, codeToInclude, importedShaderPath) = GetCodeToInclude(code, codeSourcePath);

                    if (codeToInclude.Contains("#include"))
                    {
                        codeToInclude = ShaderInclude(codeToInclude, importedShaderPath);
                    }

                    code = code.Replace(includeToReplace, codeToInclude);
                }

                return code;
            return code;
        }
        public static (string, string, string) GetCodeToInclude(string code, string codeSourcePath)
        {

            int importStartLoc = code.IndexOf("#include") + 8; // +8 because of "#include"

            int importEndLoc = code.IndexOf("\n", importStartLoc);

            string includeFilePath = code.Substring(importStartLoc, importEndLoc - importStartLoc)
                    .Replace("\"", "")
                    .Replace(@"\\", "/")
                    .Replace(" ", "")
                    .Replace("\r", "");

            string[] spliten = codeSourcePath.Split("\\");
            if (spliten.Length==1)
            {
                spliten = codeSourcePath.Split("/");
            }
            List<string> pathway = new(spliten);
            
            if (pathway[pathway.Count() - 1].Contains('.') && pathway.Count > 1)
            {
                pathway.RemoveAt(pathway.Count - 1);
            }

            pathway.Remove("");

            string importedShaderPath = RelativePathToAbsolutePath(CombinePath(pathway), includeFilePath);

            string codeToInclude = ReadFile(importedShaderPath);

            string includeToReplace = code.Substring(importStartLoc - 8, importEndLoc - (importStartLoc - 8)).Replace("\n", "");

            return (includeToReplace, codeToInclude, importedShaderPath);
        }
        public static string ReadFile(string importedShaderPath)
        {
            string result;
            string path = importedShaderPath.Replace("/", @"\");
            using (StreamReader reader = new(path))
            {
                result = reader.ReadToEnd();
            }

            return result;
        }
        public static string RelativePathToAbsolutePath(string absolutePath, string relativePath)
        {
            string result = "";

            List<string> absolutes = new();
            absolutes.AddRange(absolutePath.Split("/"));
            List<string> relatives = new();
            relatives.AddRange(relativePath.Split("/"));
            //        if (relatives.size() == 1) {
            //            relatives.add(0, "..");
            //        }

            foreach (string pathFragment in relatives)
            {
                if (pathFragment == "..")
                {
                    int indexToRemove = absolutes.Count - 1;
                    absolutes.RemoveAt(indexToRemove);
                }
                else
                {
                    absolutes.Add(pathFragment);
                }
            }

            result = CombinePath(absolutes);

            return result;
        }
        public static string CombinePath(List<string> path)
        {
            string result = "";

            foreach (string pathFragment in path)
            {
                result += pathFragment + "/";
            }

            return Regex.Replace(result, ".$", "");
        }

    }
}
