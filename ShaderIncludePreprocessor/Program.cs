namespace ShaderIncludePreprocessor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                args[i] = Path.GetFileName(args[i]);
            }
            string commandLoc = Environment.CurrentDirectory + "\\";
            string sourceFile = args[0];
            string targetFile = args[1];


            string code = Shader.ReadFile(commandLoc + sourceFile);

            string result = Shader.ShaderInclude(code, commandLoc);

            using (StreamWriter stream = new(File.OpenWrite(targetFile)))
            {
                stream.Write(result);
            }
            Console.WriteLine("done");
        }
    }
}
