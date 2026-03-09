using Quintessential;

namespace Neuvolics;

public static class Atoms
{
    private static readonly string atomPath = "textures/atoms/erikhaag/Neuvolics/";

    public static AtomType Carbonic, Bismuth, Cobalt, Arsenic, Platinum, Lithium, Antimony, Potassium;

    public static void AddAtomTypes()
    {
        // 115 - 123
        Carbonic = Brimstone.API.CreateMetalAtom(
            ID: 115,
            modName: "Neuvolics",
            name: "Carbonic",
            pathToSymbol: atomPath + "carbonic_symbol",
            pathToLightramp: atomPath + "carbonic_lightramp",
            pathToRimlight: atomPath + "carbonic_rimlight"
        );

        Bismuth = Brimstone.API.CreateMetalAtom(
            ID: 116,
            modName: "Neuvolics",
            name: "Bismuth",
            pathToSymbol: atomPath + "bismuth_symbol",
            pathToLightramp: atomPath + "bismuth_lightramp",
            pathToRimlight: atomPath + "bismuth_rimlight"
        );

        Cobalt = Brimstone.API.CreateMetalAtom(
            ID: 117,
            modName: "Neuvolics",
            name: "Cobalt",
            pathToSymbol: atomPath + "cobalt_symbol",
            pathToLightramp: atomPath + "cobalt_lightramp",
            pathToRimlight: atomPath + "cobalt_rimlight"
        );

        Arsenic = Brimstone.API.CreateMetalAtom(
            ID: 118,
            modName: "Neuvolics",
            name: "Arsenic",
            pathToSymbol: atomPath + "arsenic_symbol",
            pathToLightramp: atomPath + "arsenic_lightramp",
            pathToRimlight: atomPath + "arsenic_rimlight"
        );

        Platinum = Brimstone.API.CreateMetalAtom(
            ID: 119,
            modName: "Neuvolics",
            name: "Platinum",
            pathToSymbol: atomPath + "platinum_symbol",
            pathToLightramp: atomPath + "platinum_lightramp",
            pathToRimlight: atomPath + "platinum_rimlight"
        );

        Lithium = Brimstone.API.CreateNormalAtom(
            ID: 120,
            modName: "Neuvolics",
            name: "Lithium",
            pathToSymbol: atomPath + "lithium_symbol",
            pathToDiffuse: atomPath + "lithium_diffuse",
            pathToShade: atomPath + "lithium_shade"
        );

        Antimony = Brimstone.API.CreateNormalAtom(
            ID: 121,
            modName: "Neuvolics",
            name: "Antimony",
            pathToSymbol: atomPath + "antimony_symbol",
            pathToDiffuse: atomPath + "antimony_diffuse",
            pathToShade: atomPath + "antimony_shade"
        );

        Potassium = Brimstone.API.CreateNormalAtom(
            ID: 122,
            modName: "Neuvolics",
            name: "Potassium",
            pathToSymbol: atomPath + "potassium_symbol",
            pathToDiffuse: atomPath + "potassium_diffuse",
            pathToShade: atomPath + "potassium_shade"
        );

        QApi.AddAtomType(Lithium);
        QApi.AddAtomType(Antimony);
        QApi.AddAtomType(Potassium);
        QApi.AddAtomType(Carbonic);
        QApi.AddAtomType(Bismuth);
        QApi.AddAtomType(Cobalt);
        QApi.AddAtomType(Arsenic);
        QApi.AddAtomType(Platinum);
    }
}