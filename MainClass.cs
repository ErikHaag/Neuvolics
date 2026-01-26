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
            // second, import your assets, glyphs, transmutations, hooks, and detours
        }

        public override void PostLoad()
        {
            // third, follow through with various hand-offs
        }

        public override void Unload()
        {
            // lastly, remove all the hooks you made in LoadPuzzleContent
        }
    }
}