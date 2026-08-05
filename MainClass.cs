using Quintessential;
using System;

namespace Neuvolics;

public class MainClass : QuintessentialMod
{
    public const string LogPrefix = "Neuvolics: ";
    public const string SeparationPermission = "Neuvolics:separation";
    public const string FixationPermission = "Neuvolics:fixation";
    public const string ConsolidationPermission = "Neuvolics:consolidation";
    public const string PutrefactionPermission = "Neuvolics:putrefaction";
    public const string CataclysmPermission = "Neuvolics:cataclysm";
    public const string MaroPermission = "Neuvolics:maro";

    public static bool FTSIGCTULoaded = Brimstone.API.IsModLoaded("FTSIGCTU");
    public static bool HalvingMetallurgyLoaded = Brimstone.API.IsModLoaded("HalvingMetallurgy");

    public override void Load()
    {
        Quintessential.Logger.Log(LogPrefix + "Loaded!");
        if (FTSIGCTULoaded)
        {
            Quintessential.Logger.Log(LogPrefix + "Found FTSIGCTU!");
        }
        if (HalvingMetallurgyLoaded)
        {
            Quintessential.Logger.Log(LogPrefix + "Found Halving Metallurgy!");
        }
    }

    public override void LoadPuzzleContent()
    {
        Quintessential.Logger.Log(LogPrefix + "Initializing...");
        Atoms.AddAtomTypes();
        Wheel.LoadWheel();
        Glyphs.LoadSounds();
        Glyphs.AddHooks();
        Glyphs.AddGlyphs();

        QApi.AddPuzzlePermission(PutrefactionPermission, "Glyph of Putrefaction", "Neuvolics");
        QApi.AddPuzzlePermission(ConsolidationPermission, "Glyph of Consolidation", "Neuvolics");
        QApi.AddPuzzlePermission(SeparationPermission, "Glyph of Separation", "Neuvolics");
        QApi.AddPuzzlePermission(FixationPermission, "Glyph of Fixation", "Neuvolics");
        QApi.AddPuzzlePermission(CataclysmPermission, "Glyph of Cataclysm", "Neuvolics");
        QApi.AddPuzzlePermission(MaroPermission, "Maro's Wheel", "Neuvolics");

        if (HalvingMetallurgyLoaded)
        {
            ImportManager.ImportHM();
        }
    }

    private void AddMapRules()
    {
        foreach (class_139 p in new class_139[] { Glyphs.Putrefaction, Glyphs.Consolidation, Glyphs.Separation, Glyphs.Fixation, Glyphs.Cataclysm })
        {
            FTSIGCTU.Navigation.PartsMap.addPartHexRule(p, FTSIGCTU.Navigation.PartsMap.glyphRule);
        }
    }

    private void AddReflectionRules()
    {
        FTSIGCTU.MirrorTool.addRule(Glyphs.Putrefaction, FTSIGCTU.MirrorTool.mirrorSimplePart);
        FTSIGCTU.MirrorTool.addRule(Glyphs.Consolidation, FTSIGCTU.MirrorTool.mirrorSimplePart);
        FTSIGCTU.MirrorTool.addRule(Glyphs.Separation, FTSIGCTU.MirrorTool.mirrorSimplePart);
        FTSIGCTU.MirrorTool.addRule(Glyphs.Fixation, FTSIGCTU.MirrorTool.mirrorVerticalPart0_0);
        FTSIGCTU.MirrorTool.addRule(Glyphs.Cataclysm, static (ses, part, vert, origin) =>
        {
            FTSIGCTU.MirrorTool.shiftRotation(part, HexRotation.Clockwise);
            FTSIGCTU.MirrorTool.mirrorSimplePart(ses, part, vert, origin);
            FTSIGCTU.MirrorTool.shiftRotation(part, HexRotation.Counterclockwise);
            return true;
        });
        FTSIGCTU.MirrorTool.addRule(Wheel.Maro, FTSIGCTU.MirrorTool.mirrorSimplePart);
    }

    public override void PostLoad()
    {
        if (FTSIGCTULoaded)
        {
            AddMapRules();
            AddReflectionRules();
        }
    }

    public override void Unload()
    {
        Glyphs.RemoveHooks();
    }
}