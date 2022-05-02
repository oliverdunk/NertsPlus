using System;
using System.IO;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace NertsPlusPatcher
{
    public class NertsPlusPatcher
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Called using reflection")]
        public static void LoadMod()
        {
            Console.WriteLine("Attempting to inject NertsPlus...");

            Assembly mod = Assembly.LoadFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NertsPlus.dll"));

            Type modType = mod.GetType("NertsPlus.NertsPlus");
            Object modInstance = mod.CreateInstance("NertsPlus.NertsPlus");
            modType.InvokeMember("Load", BindingFlags.InvokeMethod, null, modInstance, new Object[] { });
        }

        public static void Main()
        {
            AssemblyDefinition original = AssemblyDefinition.ReadAssembly("../bin/NertsOnline-cleaned.exe");

            MethodInfo loadMethod = typeof(NertsPlusPatcher).GetMethod("LoadMod", BindingFlags.Static | BindingFlags.Public);
            Mono.Cecil.Cil.MethodBody body = original.EntryPoint.Body;

            Instruction callInstruction = Instruction.Create(OpCodes.Call, original.MainModule.ImportReference(loadMethod));
            body.GetILProcessor().InsertBefore(body.Instructions[0], callInstruction);

            original.Write("../bin/NertsOnline-patched.exe");
        }
    }
}
