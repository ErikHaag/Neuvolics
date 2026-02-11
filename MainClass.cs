using System;
using Quintessential;

namespace Neuvolics;

public class MainClass : QuintessentialMod
{
    public const string LogPrefix = "Neuvolics: ";
    public const string SeparationPermission = "Neuvolics:separation";
    public const string FixationPermission = "Neuvolics:fixation";
    //public const string ConsolidationPermission = "Neuvolics:consolidation";

    public override void Load()
    {
        Console.WriteLine(LogPrefix + "Loaded!");
    }

    public override void LoadPuzzleContent()
    {
        Quintessential.Logger.Log(MainClass.LogPrefix + "Initializing...");
        Atoms.AddAtomTypes();
        Glyphs.LoadSounds();
        Glyphs.AddHooks();
        Glyphs.AddGlyphs();


        QApi.AddPuzzlePermission(SeparationPermission, "Glyph of Separation", "Neuvolics");
        QApi.AddPuzzlePermission(FixationPermission, "Glyph of Fixation", "Neuvolics");
        //QApi.AddPuzzlePermission(ConsolidationPermission, "Glyph of Consolidation", "Neuvolics");
    }

    public override void PostLoad()
    {

    }

    public override void Unload()
    {
        Glyphs.RemoveHooks();
    }
}