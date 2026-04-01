namespace Neuvolics;

public static class API
{
    private static readonly AtomType[] NeumetalCycle = { Atoms.Hestium, Atoms.Azulum, Atoms.Taceum, Atoms.Mitrum, Atoms.Iridium };

    public static AtomType GetNeumetalAtom(int i) => NeumetalCycle[class_162.method_408(i, 5) /* wrap the index in-bounds */];

    public static int GetNeumetalIndex(AtomType aT)
    {
        for (int i = 0; i < 5; i++)
        {
            if (NeumetalCycle[i] == aT)
            {
                return i;
            }
        }
        // if not found
        return -1;
    }
}
