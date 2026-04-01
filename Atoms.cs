using Quintessential;

namespace Neuvolics;

public static class Atoms
{
    private static readonly string atomPath = "textures/atoms/erikhaag/Neuvolics/";

    public static AtomType Mitrum, Iridium, Azulum, Taceum, Hestium, Frixon, Zephiron, Gelaron;

    public static void AddAtomTypes()
    {
        // 115 - 123
        Mitrum = Brimstone.API.CreateMetalAtom(
            ID: 115,
            modName: "Neuvolics",
            name: "Mitrum",
            pathToSymbol: atomPath + "mitrum_symbol",
            pathToLightramp: atomPath + "mitrum_lightramp",
            pathToRimlight: atomPath + "mitrum_rimlight"
        );

        Iridium = Brimstone.API.CreateMetalAtom(
            ID: 116,
            modName: "Neuvolics",
            name: "Iridium",
            pathToSymbol: atomPath + "iridium_symbol",
            pathToLightramp: atomPath + "iridium_lightramp",
            pathToRimlight: atomPath + "iridium_rimlight"
        );

        Azulum = Brimstone.API.CreateMetalAtom(
            ID: 117,
            modName: "Neuvolics",
            name: "Azulum",
            pathToSymbol: atomPath + "azulum_symbol",
            pathToLightramp: atomPath + "azulum_lightramp",
            pathToRimlight: atomPath + "azulum_rimlight"
        );

        Taceum = Brimstone.API.CreateMetalAtom(
            ID: 118,
            modName: "Neuvolics",
            name: "Taceum",
            pathToSymbol: atomPath + "taceum_symbol",
            pathToLightramp: atomPath + "taceum_lightramp",
            pathToRimlight: atomPath + "taceum_rimlight"
        );

        Hestium = Brimstone.API.CreateMetalAtom(
            ID: 119,
            modName: "Neuvolics",
            name: "Hestium",
            pathToSymbol: atomPath + "hestium_symbol",
            pathToLightramp: atomPath + "hestium_lightramp",
            pathToRimlight: atomPath + "hestium_rimlight"
        );

        Frixon = Brimstone.API.CreateNormalAtom(
            ID: 120,
            modName: "Neuvolics",
            name: "Frixon",
            pathToSymbol: atomPath + "frixon_symbol",
            pathToDiffuse: atomPath + "frixon_diffuse",
            pathToShade: atomPath + "frixon_shade"
        );

        Zephiron = Brimstone.API.CreateNormalAtom(
            ID: 121,
            modName: "Neuvolics",
            name: "Zephiron",
            pathToSymbol: atomPath + "zephiron_symbol",
            pathToDiffuse: atomPath + "zephiron_diffuse",
            pathToShade: atomPath + "zephiron_shade"
        );

        Gelaron = Brimstone.API.CreateNormalAtom(
            ID: 122,
            modName: "Neuvolics",
            name: "Gelaron",
            pathToSymbol: atomPath + "gelaron_symbol",
            pathToDiffuse: atomPath + "gelaron_diffuse",
            pathToShade: atomPath + "gelaron_shade"
        );

        QApi.AddAtomType(Frixon);
        QApi.AddAtomType(Zephiron);
        QApi.AddAtomType(Gelaron);
        QApi.AddAtomType(Mitrum);
        QApi.AddAtomType(Iridium);
        QApi.AddAtomType(Azulum);
        QApi.AddAtomType(Taceum);
        QApi.AddAtomType(Hestium);
    }
}