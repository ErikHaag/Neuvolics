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

    public static void ExportTextures()
    {
        typeof(FixationTextureExports).ModInterop();
    }

    public static void ExportSounds()
    {
        typeof(SoundExports).ModInterop();
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
        public static int GetNeumetalIndex(AtomType neumetal) => API.GetNeumetalIndex(neumetal);
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

    [ModExportName("Neuvolics.Textures.Fixation")]
    public static class FixationTextureExports
    {
        public static Texture GetFixationBase() => Textures.Fixation.Base;
        public static Texture[] GetFixationNets() => Textures.Fixation.Nets;
        public static Texture GetFixationConnectors() => Textures.Fixation.Connectors;
        public static Texture GetFixationHoleBar() => Textures.Fixation.HoleBar;
        public static Texture GetFixationHoleNeumetalActive() => Textures.Fixation.HoleNeumetalActive;
        public static Texture GetFixationHoleNeumetalInactive() => Textures.Fixation.HoleNeumetalInactive;
        public static Texture GetFixationHoleVolicActive() => Textures.Fixation.HoleVolicActive;
        public static Texture GetFixationHoleFrixonHalfActive() => Textures.Fixation.HoleFrixonHalfActive;
        public static Texture GetFixationHoleFrixonInactive() => Textures.Fixation.HoleFrixonInactive;
        public static Texture GetFixationHoleGelaronHalfActive() => Textures.Fixation.HoleGelaronHalfActive;
        public static Texture GetFixationHoleGelaronInactive() => Textures.Fixation.HoleGelaronInactive;

        public static Texture[] GetFixationZephironIris() => Textures.Irises.Zephiron;
        public static Texture[] GetFixationNeumetalIris() => Textures.Irises.Neumetal;
    }

    [ModExportName("Neuvolics.Sounds")]
    public static class SoundExports
    {
        public static Sound GetFixationSound() => Glyphs.FixationSound;
    }

}
