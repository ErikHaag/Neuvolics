using MonoMod.ModInterop;
using System;

namespace Neuvolics;

internal class ImportManager
{
    internal static void ImportHM()
    {
        typeof(HalvingMetallurgyAtoms).ModInterop();
        Fields.HMVulcan = HalvingMetallurgyAtoms.GetVulcan();
    }

    internal static class Fields {
        internal static AtomType HMVulcan = null;
    }

    [ModImportName("HalvingMetallurgy.Atoms")]
    internal static class HalvingMetallurgyAtoms
    {
        public static Func<AtomType> GetVulcan;
    }
}
