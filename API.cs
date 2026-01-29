namespace Neuvolics;

public static class API
{
    private static readonly AtomType[] TrueNeuvolicCycle = { Atoms.Carbonic, Atoms.Bismuth, Atoms.Cobalt, Atoms.Arsenic, Atoms.Platinum };

    public static AtomType GetNeuvolicAtom(int i) => TrueNeuvolicCycle[class_162.method_408(i, 5) /* wrap the index in-bounds */];

    public static int GetNeuvolicIndex(AtomType aT)
    {
        for (int i = 0; i < 5; i++)
        {
            if (TrueNeuvolicCycle[i] == aT)
            {
                return i;
            }
        }
        // if not found
        return -1;
    }
}
