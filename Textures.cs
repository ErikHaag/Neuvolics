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

        public static readonly Texture Putrefaction = Brimstone.API.GetTexture(glyphTexturePath + "icon/putrefaction");
        public static readonly Texture PutrefactionHover = Brimstone.API.GetTexture(glyphTexturePath + "icon/putrefaction_hover");
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
        public static readonly Texture ZephironInput = Brimstone.API.GetTexture(glyphTexturePath + "Separation/zephiron_input");
        public static readonly Texture FrixonIrisLip = Brimstone.API.GetTexture(glyphTexturePath + "Separation/frixon_output_above_iris");
        public static readonly Texture FrixonIrisBase = Brimstone.API.GetTexture(glyphTexturePath + "Separation/frixon_output_under_iris");
        public static readonly Texture GelaronIrisBase = Brimstone.API.GetTexture(glyphTexturePath + "Separation/gelaron_output_under_iris");
        public static readonly Texture GelaronIrisLip = Brimstone.API.GetTexture(glyphTexturePath + "Separation/gelaron_output_above_iris");
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
        public static Texture HoleBar = Brimstone.API.GetTexture(glyphTexturePath + "Consolidation/separator");
        public static Texture HoleFrixonActive = Brimstone.API.GetTexture(glyphTexturePath + "Consolidation/frixon_active");
        public static Texture HoleFrixonInactive = Brimstone.API.GetTexture(glyphTexturePath + "Consolidation/frixon_inactive");
        public static Texture HoleGelaronActive = Brimstone.API.GetTexture(glyphTexturePath + "Consolidation/gelaron_active");
        public static Texture HoleGelaronInactive = Brimstone.API.GetTexture(glyphTexturePath + "Consolidation/gelaron_inactive");
    }

    public static class Putrefaction
    {
        public static readonly Texture Base = class_238.field_1989.field_90.field_213;
    }

    public static class HoleSymbol
    {
        public static readonly Texture Zephiron = Brimstone.API.GetTexture(glyphTexturePath + "zephiron_hole");
        public static readonly Texture Volic = Brimstone.API.GetTexture(glyphTexturePath + "volic_hole");
    }

    public static class BowlSymbol
    {
        public static readonly Texture Neumetal = Brimstone.API.GetTexture(glyphTexturePath + "neumetal_bowl");
    }

    public static class Irises
    {
        public static readonly Texture[] Frixon = Brimstone.API.GetAnimation(glyphTexturePath + "iris_full_frixon.array", "iris_full_frixon", 16);
        public static readonly Texture[] Neumetal = Brimstone.API.GetAnimation(glyphTexturePath + "iris_full_neumetal.array", "iris_full_neumetal", 16);
        public static readonly Texture[] Gelaron = Brimstone.API.GetAnimation(glyphTexturePath + "iris_full_gelaron.array", "iris_full_gelaron", 16);
        public static readonly Texture[] Zephiron = Brimstone.API.GetAnimation(glyphTexturePath + "iris_full_zephiron.array", "iris_full_zephiron", 16);
    }
}