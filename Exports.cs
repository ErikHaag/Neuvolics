using MonoMod.ModInterop;
using PartType = class_139;
using Texture = class_256;

namespace Neuvolics;

public class Exports
{

    public static void ExportAtoms()
    {
        typeof(AtomExports).ModInterop();
    }

    public static void ExportGlyphs()
    {
        typeof(GlyphExports).ModInterop();
    }

    [ModExportName("Neuvolics.Atoms")]
    public static class AtomExports
    {
        public static AtomType GetAzulum() => Atoms.Azulum;
        public static AtomType GetFrixon() => Atoms.Frixon;
        public static AtomType GetGelaron() => Atoms.Gelaron;
        public static AtomType GetHestium() => Atoms.Hestium;
        public static AtomType GetIridium() => Atoms.Iridium;
        public static AtomType GetMitrum() => Atoms.Mitrum;
        public static AtomType GetTaceum() => Atoms.Taceum;
        public static AtomType GetZephiron() => Atoms.Zephiron;

        public static AtomType GetNeumetalAtom(int index) => API.GetNeumetalAtom(index);
        public static int GetNeumetalAtom(AtomType neumetal) => API.GetNeumetalIndex(neumetal);
    }

    [ModExportName("Neuvolics.Glyphs")]
    public static class GlyphExports
    {
        public static PartType GetPutrefaction() => Glyphs.Putrefaction;
        public static PartType GetConsolidation() => Glyphs.Consolidation;
        public static PartType GetSeparation() => Glyphs.Separation;
        public static PartType GetFixation() => Glyphs.Fixation;
        public static PartType GetCataclysm() => Glyphs.Cataclysm;
    }


}
