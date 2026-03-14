using Texture = class_256;

namespace Neuvolics;

public static class Textures
{
    private static readonly string modPath = "/erikhaag/Neuvolics/";
    private static readonly string glyphTexturePath = "textures/parts" + modPath;
    private static readonly string selectTexturePath = "textures/select" + modPath;

    public static class Icon
    {
        public static readonly Texture Separation = Brimstone.API.GetTexture(glyphTexturePath + "icons/separation");
        public static readonly Texture SeparationHover = Brimstone.API.GetTexture(glyphTexturePath + "icons/separation_hover");

        public static readonly Texture Fixation = Brimstone.API.GetTexture(glyphTexturePath + "icons/fixation");
        public static readonly Texture FixationHover = Brimstone.API.GetTexture(glyphTexturePath + "icons/fixation_hover");

        public static readonly Texture Consolidation = Brimstone.API.GetTexture(glyphTexturePath + "icons/consolidation");
        public static readonly Texture ConsolidationHover = Brimstone.API.GetTexture(glyphTexturePath + "icons/consolidation_hover");
    }

    public static class Select
    {
        public static readonly Texture ParallelogramGlow = Brimstone.API.GetTexture(selectTexturePath + "parallelogram_3_glow");
        public static readonly Texture ParallelogramStroke = Brimstone.API.GetTexture(selectTexturePath + "parallelogram_3_stroke");
        public static readonly Texture TrilineGlow = Brimstone.API.GetTexture(selectTexturePath + "triline_glow");
        public static readonly Texture TrilineStroke = Brimstone.API.GetTexture(selectTexturePath + "triline_stroke");
        public static readonly Texture BendGlow = Brimstone.API.GetTexture(selectTexturePath + "bend_glow");
        public static readonly Texture BendStroke = Brimstone.API.GetTexture(selectTexturePath + "bend_stroke");
    }

    public static class Separation
    {
        public static readonly Texture Base = Brimstone.API.GetTexture(glyphTexturePath + "Separation/base");
        public static readonly Texture AntimonyInput = Brimstone.API.GetTexture(glyphTexturePath + "Separation/antimony_input");
        public static readonly Texture PotassiumIrisBase = Brimstone.API.GetTexture(glyphTexturePath + "Separation/potassium_output_under_iris");
        public static readonly Texture PotassiumIrisLip = Brimstone.API.GetTexture(glyphTexturePath + "Separation/potassium_output_above_iris");
        public static readonly Texture LithiumIrisBase = Brimstone.API.GetTexture(glyphTexturePath + "Separation/lithium_output_under_iris");
        public static readonly Texture LithiumIrisLip = Brimstone.API.GetTexture(glyphTexturePath + "Separation/lithium_output_above_iris");
        public static readonly Texture Connectors = Brimstone.API.GetTexture(glyphTexturePath + "Separation/connectors");
    }

    public static class Fixation
    {
        public static readonly Texture Base = Brimstone.API.GetTexture(glyphTexturePath + "Fixation/base");
        public static readonly Texture[] Nets = new Texture[] { Brimstone.API.GetTexture(glyphTexturePath + "Fixation/base_tr"), Brimstone.API.GetTexture(glyphTexturePath + "Fixation/base_tl"), Brimstone.API.GetTexture(glyphTexturePath + "Fixation/base_bl"), Brimstone.API.GetTexture(glyphTexturePath + "Fixation/base_br") };
        public static readonly Texture Connectors = Brimstone.API.GetTexture(glyphTexturePath + "Fixation/connectors");
        public static readonly Texture HoleBar = Brimstone.API.GetTexture(glyphTexturePath + "Fixation/separator");
        public static readonly Texture HoleNeumetalInactive = Brimstone.API.GetTexture(glyphTexturePath + "Fixation/neumetal_inactive");
        public static readonly Texture HoleNeumetalActive = Brimstone.API.GetTexture(glyphTexturePath + "Fixation/neumetal_active");
        public static readonly Texture HoleVolicInactive = Brimstone.API.GetTexture(glyphTexturePath + "Fixation/volic_inactive");
        public static readonly Texture HoleVolicHalfActive = Brimstone.API.GetTexture(glyphTexturePath + "Fixation/volic_half_active");
        public static readonly Texture HoleVolicActive = Brimstone.API.GetTexture(glyphTexturePath + "Fixation/volic_active");
    }

    public static class Consolidation
    {
        public static Texture Base = Brimstone.API.GetTexture(glyphTexturePath + "Consolidation/base");
        public static Texture VolicInput = Brimstone.API.GetTexture(glyphTexturePath + "Consolidation/volic_input");
        public static Texture IrisBase = Brimstone.API.GetTexture(glyphTexturePath + "Consolidation/output_under_iris");
        public static Texture IrisLip = Brimstone.API.GetTexture(glyphTexturePath + "Consolidation/output_above_iris");
        public static Texture Connectors = Brimstone.API.GetTexture(glyphTexturePath + "Consolidation/connectors");
    }

    public static class HoleSymbol
    {
        public static readonly Texture Antimony = Brimstone.API.GetTexture(glyphTexturePath + "antimony_hole");
    }

    public static class Irises
    {
        public static readonly Texture[] Antimony = Brimstone.API.GetAnimation(glyphTexturePath + "iris_full_antimony.array", "iris_full_antimony", 16);
        public static readonly Texture[] Neumetal = Brimstone.API.GetAnimation(glyphTexturePath + "iris_full_neumetal.array", "iris_full_neumetal", 16);
        public static readonly Texture[] Potassium = Brimstone.API.GetAnimation(glyphTexturePath + "iris_full_potassium.array", "iris_full_potassium", 16);
        public static readonly Texture[] Lithium = Brimstone.API.GetAnimation(glyphTexturePath + "iris_full_lithium.array", "iris_full_lithium", 16);
    }
}