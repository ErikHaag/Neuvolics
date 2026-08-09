using MonoMod.ModInterop;
using MonoMod.Utils;
using System;
using PartType = class_139;
using Texture = class_256;

namespace Neuvolics;

public class Exports
{

    public static void ExportAtoms()
    {
        typeof(AtomExports).ModInterop();
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
}
