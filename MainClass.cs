using System;
using Quintessential;

namespace Neuvolics;

public class MainClass : QuintessentialMod
{
    public const string LogPrefix = "Neuvolics: ";
    public const string SeparationPermission = "Neuvolics:separation";
    public const string FixationPermission = "Neuvolics:fixation";

    public override void Load()
    {
        Console.WriteLine(LogPrefix + "Loaded!");
    }

    public override void LoadPuzzleContent()
    {
        Atoms.AddAtomTypes();
        Glyphs.AddGlyphs();

        QApi.AddPuzzlePermission(SeparationPermission, "Glyph of Separation", "Neuvolics");
        QApi.AddPuzzlePermission(FixationPermission, "Glyph of Fixation", "Neuvolics");
    }

    public override void PostLoad()
    {

    }

    public override void Unload()
    {

    }
}