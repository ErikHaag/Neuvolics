using System;
using Quintessential;

namespace Neuvolics
{
    public class MainClass : QuintessentialMod
    {
        const string LogPrefix = "Neuvolics: ";


        public override void Load()
        {
            Console.WriteLine(LogPrefix + "Loaded!");
        }

        public override void LoadPuzzleContent()
        {
            Atoms.AddAtomTypes();
        }

        public override void PostLoad()
        {

        }

        public override void Unload()
        {

        }
    }
}