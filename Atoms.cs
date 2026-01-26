using Quintessential;

namespace Neuvolics;

public static class Atoms
{
    private static readonly string atomPath = "textures/atoms/erikhaag/Neuvolics/";

    public static AtomType Carbonic, Bismuth, Cobalt, Arsenic, Platinum, Lithium, Potassium;

    public static void AddAtomTypes()
    {
        // 116 - 123
        Carbonic = Brimstone.API.CreateMetalAtom(
            ID: 116,
            modName: "Neuvolics",
            name: "Carbonic",
            pathToSymbol: atomPath + "carbonic_symbol",
            pathToLightramp: atomPath + "carbonic_lightramp"
        );
        
        Bismuth = Brimstone.API.CreateMetalAtom(
            ID: 117,
            modName: "Neuvolics",
            name: "Bismuth",
            pathToSymbol: atomPath + "bismuth_symbol",
            pathToLightramp: atomPath + "bismuth_lightramp"
        );

        Cobalt = Brimstone.API.CreateMetalAtom(
            ID: 118,
            modName: "Neuvolics",
            name: "Cobalt",
            pathToSymbol: atomPath + "cobalt_symbol",
            pathToLightramp: atomPath + "cobalt_lightramp"
        );

        Arsenic = Brimstone.API.CreateMetalAtom(
            ID: 119,
            modName: "Neuvolics",
            name: "Arsenic",
            pathToSymbol: atomPath + "arsenic_symbol",
            pathToLightramp: atomPath + "arsenic_lightramp"
        );

        Platinum = Brimstone.API.CreateMetalAtom(
            ID: 120,
            modName: "Neuvolics",
            name: "Platinum",
            pathToSymbol: atomPath + "platinum_symbol",
            pathToLightramp: atomPath + "platinum_lightramp"
        );

        Lithium = Brimstone.API.CreateNormalAtom(
            ID: 121,
            modName: "Neuvolics",
            name: "Lithium",
            pathToSymbol: atomPath + "lithium_symbol",
            pathToDiffuse: atomPath + "lithium_diffuse"
        );

        Potassium = Brimstone.API.CreateNormalAtom(
            ID: 122,
            modName: "Neuvolics",
            name: "Potassium",
            pathToSymbol: atomPath + "potassium_symbol",
            pathToDiffuse: atomPath + "potassium_diffuse"
        );

        QApi.AddAtomType(Carbonic);
        QApi.AddAtomType(Bismuth);
        QApi.AddAtomType(Cobalt);
        QApi.AddAtomType(Arsenic);
        QApi.AddAtomType(Platinum);
        QApi.AddAtomType(Lithium);
        QApi.AddAtomType(Potassium);
    }
}

