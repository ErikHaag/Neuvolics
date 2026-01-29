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
    public static PartType Fixation;

    #endregion

    #region Textures
    #region Icons

    public static readonly Texture SeparationIcon = Brimstone.API.GetTexture("textures/parts/erikhaag/Neuvolics/icons/separation");
    public static readonly Texture SeparationIconHover = Brimstone.API.GetTexture("textures/parts/erikhaag/Neuvolics/icons/separation_hover");

    public static readonly Texture FixationIcon = Brimstone.API.GetTexture("textures/parts/erikhaag/Neuvolics/icons/fixation");
    public static readonly Texture FixationIconHover = Brimstone.API.GetTexture("textures/parts/erikhaag/Neuvolics/icons/fixation_hover");

    #endregion

    public static readonly Texture AntimonyHoleSymbol = Brimstone.API.GetTexture("textures/parts/erikhaag/Neuvolics/antimony_hole");

    public static readonly Texture[] PotassiumIris = Brimstone.API.GetAnimation("textures/parts/erikhaag/Neuvolics/iris_full_potassium.array", "iris_full_potassium", 16);
    public static readonly Texture[] LithiumIris = Brimstone.API.GetAnimation("textures/parts/erikhaag/Neuvolics/iris_full_lithium.array", "iris_full_lithium", 16);

    public static readonly Texture FixationHoleBar = Brimstone.API.GetTexture("textures/parts/erikhaag/Neuvolics/Fixation/separator");
    public static readonly Texture FixationHoleCarbonicInactive = Brimstone.API.GetTexture("textures/parts/erikhaag/Neuvolics/Fixation/carbonic_inactive");
    public static readonly Texture FixationHoleFalseNeuvolicInactive = Brimstone.API.GetTexture("textures/parts/erikhaag/Neuvolics/Fixation/false_neuvolic_inactive");
    public static readonly Texture FixationHoleCarbonicActive = Brimstone.API.GetTexture("textures/parts/erikhaag/Neuvolics/Fixation/carbonic_active");
    public static readonly Texture FixationHoleFalseNeuvolicActive = Brimstone.API.GetTexture("textures/parts/erikhaag/Neuvolics/Fixation/false_neuvolic_active");

    public static readonly Texture[] AntimonyIris = Brimstone.API.GetAnimation("textures/parts/erikhaag/Neuvolics/iris_full_antimony.array", "iris_full_antimony", 16);
    public static readonly Texture[] TrueNeuvolicIris = Brimstone.API.GetAnimation("textures/parts/erikhaag/Neuvolics/iris_full_true_neuvolic.array", "iris_full_true_neuvolics", 16);

    #endregion

    #region Hexes

    public static readonly HexIndex SeparationHoleHex = new(0, 0);
    public static readonly HexIndex SeparationPotassiumIrisHex = new(1, 0);
    public static readonly HexIndex SeparationLithiumIrisHex = new(-1, 0);

    public static readonly HexIndex FixationHole1Hex = new(0, 0);
    public static readonly HexIndex FixationHole2Hex = new(1, 0);
    public static readonly HexIndex FixationAntimonyIrisHex = new(1, -1);
    public static readonly HexIndex FixationTrueNeuvolicIrisHex = new(0, 1);


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
                SeparationPotassiumIrisHex,
                SeparationHoleHex,
                SeparationLithiumIrisHex
            },
            customPermission: MainClass.SeparationPermission
        );

        Fixation = Brimstone.API.CreateSimpleGlyph(
            ID: "neuvolics-fixation",
            name: "Glyph of Fixation",
            description: "The glyph of fixation consumes a false neuvolic to transmute a true neuvolic to an adjacent form.",
            cost: 10,
            glow: class_238.field_1989.field_97.field_382,
            stroke: class_238.field_1989.field_97.field_383,
            icon: FixationIcon,
            hoveredIcon: FixationIconHover,
            usedHexes: new HexIndex[] {
                FixationHole1Hex,
                FixationHole2Hex,
                FixationAntimonyIrisHex,
                FixationTrueNeuvolicIrisHex
            },
            customPermission: MainClass.FixationPermission
        );

        #endregion

        QApi.AddPartTypeToPanel(Separation, false);
        QApi.AddPartTypeToPanel(Fixation, false);

        #region Glyph renderers

        QApi.AddPartType(Separation, static (part, pos, editor, renderer) =>
        {
            PartSimState pss = editor.method_507().method_481(part);
            class_236 uco = editor.method_1989(part, pos);
            float time = editor.method_504();
            renderer.method_521(class_238.field_1989.field_90.field_169, (class_238.field_1989.field_90.field_169.field_2056.ToVector2() / 2).Rounded() + new Vector2(0f, 1f));
            // input
            renderer.method_530(class_238.field_1989.field_90.field_255.field_293, SeparationHoleHex, 0);
            class_135.method_272(AntimonyHoleSymbol, (class_187.field_1742.method_491(SeparationHoleHex, Vector2.Zero).Rotated(uco.field_1985) + uco.field_1984 - AntimonyHoleSymbol.field_2056.ToVector2() / 2).Rounded());
            int irisFrame = 15;
            bool afterIrisOpens = false;
            Molecule risingAtom = null;
            Vector2 risingOffset = uco.field_1984 + class_187.field_1742.method_492(SeparationPotassiumIrisHex).Rotated(uco.field_1985);

            renderer.method_528(class_238.field_1989.field_90.field_228.field_272, SeparationPotassiumIrisHex, Vector2.Zero);
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
            renderer.method_529(PotassiumIris[irisFrame], SeparationPotassiumIrisHex, Vector2.Zero);
            renderer.method_528(class_238.field_1989.field_90.field_228.field_271, SeparationPotassiumIrisHex, Vector2.Zero);
            if (pss.field_2743 && afterIrisOpens)
            {
                // show atom rising infront of iris
                Editor.method_925(risingAtom, risingOffset, new HexIndex(0, 0), 0f, 1f, time, 1f, false, null);
            }

            renderer.method_528(class_238.field_1989.field_90.field_228.field_272, SeparationLithiumIrisHex, Vector2.Zero);
            if (pss.field_2743)
            {
                risingOffset = uco.field_1984 + class_187.field_1742.method_492(SeparationLithiumIrisHex).Rotated(uco.field_1985);
                risingAtom = Molecule.method_1121(pss.field_2744[1]);
                if (!afterIrisOpens)
                {
                    // show atom rising behind iris
                    Editor.method_925(risingAtom, risingOffset, new HexIndex(0, 0), 0f, 1f, time, 1f, false, null);
                }
            }

            renderer.method_529(LithiumIris[irisFrame], SeparationLithiumIrisHex, Vector2.Zero);
            renderer.method_528(class_238.field_1989.field_90.field_228.field_271, SeparationLithiumIrisHex, Vector2.Zero);
            if (pss.field_2743 && afterIrisOpens)
            {
                // show atom rising infront of iris
                Editor.method_925(risingAtom, risingOffset, new HexIndex(0, 0), 0f, 1f, time, 1f, false, null);
            }
        });

        QApi.AddPartType(Fixation, static (part, pos, editor, renderer) =>
        {
            // hold on to your butts!
            PartSimState pss = editor.method_507().method_481(part);
            class_236 uco = editor.method_1989(part, pos);
            float time = editor.method_504();

            int atomsPresent = 0;
            HexIndex[] holes = new HexIndex[2] { FixationHole1Hex, FixationHole2Hex };
            foreach (HexIndex h in holes)
            {
                foreach (Molecule m in editor.method_507().method_483())
                {
                    if (m.method_1100().Count == 1 && m.method_1100().TryGetValue(part.method_1184(h), out Atom a))
                    {
                        AtomType aT = a.field_2275;
                        if (aT == Atoms.Potassium || aT == Atoms.Lithium)
                        {
                            atomsPresent |= 2;
                        }
                        else if (API.GetNeuvolicIndex(aT) != -1)
                        {
                            atomsPresent |= 1;
                        }
                    }
                }
            }

            renderer.method_523(class_238.field_1989.field_90.field_228.field_265, new(-1, -1), new(41, 120), 0);

            // input rendering
            foreach (HexIndex h in holes)
            {
                renderer.method_530(class_238.field_1989.field_90.field_255.field_293, h, 0);
                renderer.method_529(FixationHoleBar, h, Vector2.Zero);
                renderer.method_529((atomsPresent & 1) != 0 ? FixationHoleCarbonicActive : FixationHoleCarbonicInactive, h, Vector2.Zero);
                renderer.method_529((atomsPresent & 2) != 0 ? FixationHoleFalseNeuvolicActive : FixationHoleFalseNeuvolicInactive, h, Vector2.Zero);
            }
            // irises
            int irisFrame = 15;
            bool afterIrisOpens = false;
            Molecule risingAtom = null;
            Vector2 risingOffset = uco.field_1984 + class_187.field_1742.method_492(FixationAntimonyIrisHex).Rotated(uco.field_1985);

            renderer.method_528(class_238.field_1989.field_90.field_228.field_272, FixationAntimonyIrisHex, Vector2.Zero);
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
            renderer.method_529(AntimonyIris[irisFrame], FixationAntimonyIrisHex, Vector2.Zero);
            renderer.method_528(class_238.field_1989.field_90.field_228.field_271, FixationAntimonyIrisHex, Vector2.Zero);
            if (pss.field_2743 && afterIrisOpens)
            {
                // show atom rising infront of iris
                Editor.method_925(risingAtom, risingOffset, new HexIndex(0, 0), 0f, 1f, time, 1f, false, null);
            }

            renderer.method_528(class_238.field_1989.field_90.field_228.field_272, FixationTrueNeuvolicIrisHex, Vector2.Zero);
            if (pss.field_2743)
            {
                risingOffset = uco.field_1984 + class_187.field_1742.method_492(FixationTrueNeuvolicIrisHex).Rotated(uco.field_1985);
                risingAtom = Molecule.method_1121(pss.field_2744[1]);
                if (!afterIrisOpens)
                {
                    // show atom rising behind iris
                    Editor.method_925(risingAtom, risingOffset, new HexIndex(0, 0), 0f, 1f, time, 1f, false, null);
                }
            }

            renderer.method_529(TrueNeuvolicIris[irisFrame], FixationTrueNeuvolicIrisHex, Vector2.Zero);
            renderer.method_528(class_238.field_1989.field_90.field_228.field_271, FixationTrueNeuvolicIrisHex, Vector2.Zero);
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
                    if (first)
                    {
                        HexIndex h = part.method_1184(SeparationHoleHex);
                        HexIndex il = part.method_1184(SeparationLithiumIrisHex);
                        HexIndex ip = part.method_1184(SeparationPotassiumIrisHex);

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

                        Brimstone.API.AddSmallCollider(sim, part, SeparationPotassiumIrisHex);
                        Brimstone.API.AddSmallCollider(sim, part, SeparationLithiumIrisHex);

                        pss[part].field_2743 = true;
                        pss[part].field_2744 = new AtomType[2] { Atoms.Potassium, Atoms.Lithium };
                    }
                    else if (pss[part].field_2743)
                    {
                        Brimstone.API.AddAtom(sim, part, SeparationPotassiumIrisHex, pss[part].field_2744[0]);
                        Brimstone.API.AddAtom(sim, part, SeparationLithiumIrisHex, pss[part].field_2744[1]);
                    }
                }
                else if (type == Fixation)
                {
                    if (first)
                    {
                        HexIndex hl = part.method_1184(FixationHole1Hex);
                        HexIndex hr = part.method_1184(FixationHole2Hex);
                        HexIndex ia = part.method_1184(FixationAntimonyIrisHex);
                        HexIndex it = part.method_1184(FixationTrueNeuvolicIrisHex);

                        if (sim.FindAtom(ia).method_1085() || sim.FindAtom(it).method_1085())
                        {
                            // blocked!
                            continue;
                        }

                        if (!sim.FindAtom(hl).method_99(out AtomReference al) || al.field_2281 || al.field_2282)
                        {
                            continue;
                        }

                        if (!sim.FindAtom(hr).method_99(out AtomReference ar) || ar.field_2281 || ar.field_2282)
                        {
                            continue;
                        }

                        AtomType falseNeu = null;
                        int trueNeuIndex = -1;

                        if (al.field_2280 == Atoms.Potassium || al.field_2280 == Atoms.Lithium)
                        {
                            falseNeu = al.field_2280;
                            trueNeuIndex = API.GetNeuvolicIndex(ar.field_2280);
                        }
                        else if (ar.field_2280 == Atoms.Potassium || ar.field_2280 == Atoms.Lithium)
                        {
                            falseNeu = ar.field_2280;
                            trueNeuIndex = API.GetNeuvolicIndex(al.field_2280);
                        }
                        else
                        {
                            continue;
                        }

                        if (trueNeuIndex == -1)
                        {
                            continue;
                        }

                        int delta = falseNeu == Atoms.Potassium ? -1 : 1;

                        Brimstone.API.RemoveAtom(al);
                        Brimstone.API.RemoveAtom(ar);

                        Brimstone.API.DrawFallingAtom(seb, al);
                        Brimstone.API.DrawFallingAtom(seb, ar);

                        Brimstone.API.AddSmallCollider(sim, part, FixationAntimonyIrisHex);
                        Brimstone.API.AddSmallCollider(sim, part, FixationTrueNeuvolicIrisHex);

                        pss[part].field_2743 = true;
                        pss[part].field_2744 = new AtomType[2] { Atoms.Antimony, API.GetNeuvolicAtom(trueNeuIndex + delta) };
                    }
                    else if (pss[part].field_2743)
                    {
                        Brimstone.API.AddAtom(sim, part, FixationAntimonyIrisHex, pss[part].field_2744[0]);
                        Brimstone.API.AddAtom(sim, part, FixationTrueNeuvolicIrisHex, pss[part].field_2744[1]);
                    }
                }
            }
        });
    }

}
