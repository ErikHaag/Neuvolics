using Quintessential;

namespace Neuvolics;

public class MainClass : QuintessentialMod
{
    public const string LogPrefix = "Neuvolics: ";
    public const string SeparationPermission = "Neuvolics:separation";
    public const string FixationPermission = "Neuvolics:fixation";
    public const string ConsolidationPermission = "Neuvolics:consolidation";
    public const string PutrefactionPermission = "Neuvolics:putrefaction";

    public static bool HalvingMetallurgyLoaded = Brimstone.API.IsModLoaded("HalvingMetallurgy");

    public override void Load()
    {
        Quintessential.Logger.Log(LogPrefix + "Loaded!");
        if (HalvingMetallurgyLoaded)
        {
            Quintessential.Logger.Log(LogPrefix + "Found HM!");
        }
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
        QApi.AddPuzzlePermission(ConsolidationPermission, "Glyph of Consolidation", "Neuvolics");
        QApi.AddPuzzlePermission(PutrefactionPermission, "Glyph of Putrefaction", "Neuvolics");

        if (HalvingMetallurgyLoaded)
        {
            ImportManager.ImportHM();
        }
    }

    public override void PostLoad()
    {

    }

    public override void Unload()
    {
        Glyphs.RemoveHooks();
    }
}