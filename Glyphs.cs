using Quintessential;
using System.Collections.Generic;
using PartType = class_139;
using Permissions = enum_149;
using Texture = class_256;

namespace Neuvolics;

public static class Glyphs
{
    #region Glyphs

    public static PartType Separation;
    //public static PartType Fixation;

    #endregion

    #region Textures
    #region Icons

    public static readonly Texture SeparationIcon = Brimstone.API.GetTexture("textures/parts/erikhaag/Neuvolics/icons/separation");
    public static readonly Texture SeparationIconHover = Brimstone.API.GetTexture("textures/parts/erikhaag/Neuvolics/icons/separation_hover");

    #endregion

    public static readonly Texture AntimonyHoleSymbol = Brimstone.API.GetTexture("textures/parts/erikhaag/Neuvolics/antimony_hole");

    public static readonly Texture[] PotassiumIris = Brimstone.API.GetAnimation("textures/parts/erikhaag/Neuvolics/iris_full_potassium.array", "iris_full_potassium", 16);
    //public static readonly Texture[] AntimonyIris = Brimstone.API.GetAnimation("textures/parts/erikhaag/Neuvolics/iris_full_antimony.array", "iris_full_antimony", 16);
    public static readonly Texture[] LithiumIris = Brimstone.API.GetAnimation("textures/parts/erikhaag/Neuvolics/iris_full_lithium.array", "iris_full_lithium", 16);

    #endregion

    #region Hexes

    public static readonly HexIndex SeparationHole = new(0, 0);
    public static readonly HexIndex SeparationPotassiumIris = new(1, 0);
    public static readonly HexIndex SeparationLithiumIris = new(-1, 0);

    #endregion

    public static void AddGlyphs()
    {
        #region Glyph definitions

        Separation = Brimstone.API.CreateSimpleGlyph(
            ID: "neuvolics-separation",
            name: "Glyph of Separation",
            description: "The glyph of separation divides an atom of antimony in an atom of potassium and lithium.",
            cost: 10,
            glow: class_238.field_1989.field_97.field_382,
            stroke: class_238.field_1989.field_97.field_383,
            icon: SeparationIcon,
            hoveredIcon: SeparationIconHover,
            usedHexes: new HexIndex[] {
                SeparationPotassiumIris,
                SeparationHole,
                SeparationLithiumIris
            },
            customPermission: MainClass.SeparationPermission
        );

        #endregion

        QApi.AddPartTypeToPanel(Separation, false);

        #region Glyph renderers

        QApi.AddPartType(Separation, static (part, pos, editor, renderer) =>
        {
            PartSimState pss = editor.method_507().method_481(part);
            class_236 uco = editor.method_1989(part, pos);
            float time = editor.method_504();
            renderer.method_521(class_238.field_1989.field_90.field_169, (class_238.field_1989.field_90.field_169.field_2056.ToVector2() / 2).Rounded() + new Vector2(0f, 1f));
            // input
            renderer.method_530(class_238.field_1989.field_90.field_255.field_293, SeparationHole, 0);
            class_135.method_272(AntimonyHoleSymbol, (class_187.field_1742.method_491(SeparationHole, Vector2.Zero).Rotated(uco.field_1985) + uco.field_1984 - AntimonyHoleSymbol.field_2056.ToVector2() / 2).Rounded());
            int irisFrame = 15;
            bool afterIrisOpens = false;
            Molecule risingAtom = null;
            Vector2 risingOffset = uco.field_1984 + class_187.field_1742.method_492(SeparationPotassiumIris).Rotated(uco.field_1985);

            renderer.method_528(class_238.field_1989.field_90.field_228.field_272, SeparationPotassiumIris, Vector2.Zero);
            if (pss.field_2743)
            {
                irisFrame = class_162.method_404((int)(class_162.method_411(1f, -1f, time) * 16f), 0, 15);
                afterIrisOpens = time > 0.5f;
                risingAtom = Molecule.method_1121(pss.field_2744[0]);
                if (!afterIrisOpens)
                {
                    // show atom rising behind iris
                    Editor.method_925(risingAtom, risingOffset, new HexIndex(0, 0), 0f, 1f, time, 1f, false, null);
                }
            }
            renderer.method_529(PotassiumIris[irisFrame], SeparationPotassiumIris, Vector2.Zero);
            renderer.method_528(class_238.field_1989.field_90.field_228.field_271, SeparationPotassiumIris, Vector2.Zero);
            if (pss.field_2743 && afterIrisOpens)
            {
                // show atom rising infront of iris
                Editor.method_925(risingAtom, risingOffset, new HexIndex(0, 0), 0f, 1f, time, 1f, false, null);
            }

            renderer.method_528(class_238.field_1989.field_90.field_228.field_272, SeparationLithiumIris, Vector2.Zero);
            if (pss.field_2743)
            {
                risingOffset = uco.field_1984 + class_187.field_1742.method_492(SeparationLithiumIris).Rotated(uco.field_1985);
                risingAtom = Molecule.method_1121(pss.field_2744[1]);
                if (!afterIrisOpens)
                {
                    // show atom rising behind iris
                    Editor.method_925(risingAtom, risingOffset, new HexIndex(0, 0), 0f, 1f, time, 1f, false, null);
                }
            }

            renderer.method_529(LithiumIris[irisFrame], SeparationLithiumIris, Vector2.Zero);
            renderer.method_528(class_238.field_1989.field_90.field_228.field_271, SeparationLithiumIris, Vector2.Zero);
            if (pss.field_2743 && afterIrisOpens)
            {
                // show atom rising infront of iris
                Editor.method_925(risingAtom, risingOffset, new HexIndex(0, 0), 0f, 1f, time, 1f, false, null);
            }
        });

        #endregion

        QApi.RunAfterCycle(static (sim, first) =>
        {
            SolutionEditorBase seb = sim.field_3818;
            Dictionary<Part, PartSimState> pss = sim.field_3821;
            List<Part> parts = seb.method_502().field_3919;

            foreach (var part in parts)
            {
                PartType type = part.method_1159();
                if (type == Separation)
                {
                    HexIndex h = part.method_1184(SeparationHole);
                    HexIndex il = part.method_1184(SeparationLithiumIris);
                    HexIndex ip = part.method_1184(SeparationPotassiumIris);

                    if (first)
                    {
                        if (sim.FindAtom(il).method_1085() || sim.FindAtom(ip).method_1085())
                        {
                            // blocked!
                            continue;
                        }

                        if (!sim.FindAtom(h).method_99(out AtomReference holeAtom))
                        {
                            // no atom above hole
                            continue;
                        }

                        if (holeAtom.field_2280 != Atoms.Antimony || holeAtom.field_2281 || holeAtom.field_2282)
                        {
                            continue;
                        }

                        // an unheld, elemental antimony atom is above the hole, and neither iris is covered
                        Brimstone.API.RemoveAtom(holeAtom);
                        Brimstone.API.DrawFallingAtom(seb, holeAtom);

                        Brimstone.API.AddSmallCollider(sim, part, ip);
                        Brimstone.API.AddSmallCollider(sim, part, il);

                        pss[part].field_2743 = true;
                        pss[part].field_2744 = new AtomType[2] { Atoms.Potassium, Atoms.Lithium };
                    }
                    else if (pss[part].field_2743)
                    {
                        Brimstone.API.AddAtom(sim, part, SeparationPotassiumIris, pss[part].field_2744[0]);
                        Brimstone.API.AddAtom(sim, part, SeparationLithiumIris, pss[part].field_2744[1]);
                    }
                }
            }

        });
    }

}
