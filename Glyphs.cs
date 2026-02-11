//#define tweaker

using Mono.Cecil.Cil;
using MonoMod.Cil;
using Quintessential;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PartType = class_139;
using Texture = class_256;

namespace Neuvolics;

public static class Glyphs
{
    #region Glyphs

    public static PartType Separation;
    public static PartType Fixation;
    public static PartType Consolidation;

    #endregion

    #region Textures

    public static readonly string glyphTexturePath = "textures/parts/erikhaag/Neuvolics/";

    #region Icons

    public static readonly Texture SeparationIcon = Brimstone.API.GetTexture(glyphTexturePath + "icons/separation");
    public static readonly Texture SeparationIconHover = Brimstone.API.GetTexture(glyphTexturePath + "icons/separation_hover");

    public static readonly Texture FixationIcon = Brimstone.API.GetTexture(glyphTexturePath + "icons/fixation");
    public static readonly Texture FixationIconHover = Brimstone.API.GetTexture(glyphTexturePath + "icons/fixation_hover");

    #endregion

    public static readonly Texture SeparationBase = Brimstone.API.GetTexture(glyphTexturePath + "Separation/base");
    public static readonly Texture SeparationAntimonyInput = Brimstone.API.GetTexture(glyphTexturePath + "Separation/antimony_input");
    public static readonly Texture SeparationPotassiumIrisLip = Brimstone.API.GetTexture(glyphTexturePath + "Separation/potassium_output_above_iris");
    public static readonly Texture SeparationLithiumIrisLip = Brimstone.API.GetTexture(glyphTexturePath + "Separation/lithium_output_above_iris");
    public static readonly Texture SeparationConnectors = Brimstone.API.GetTexture(glyphTexturePath + "Separation/connectors");

    public static readonly Texture FixationBase = Brimstone.API.GetTexture(glyphTexturePath + "Fixation/base");
    public static readonly Texture[] FixationNets = new Texture[] { Brimstone.API.GetTexture(glyphTexturePath + "Fixation/base_tr"), Brimstone.API.GetTexture(glyphTexturePath + "Fixation/base_tl"), Brimstone.API.GetTexture(glyphTexturePath + "Fixation/base_bl"), Brimstone.API.GetTexture(glyphTexturePath + "Fixation/base_br") };
    public static readonly Texture FixationConnectors = Brimstone.API.GetTexture(glyphTexturePath + "Fixation/connectors");

    public static readonly Texture AntimonyHoleSymbol = Brimstone.API.GetTexture(glyphTexturePath + "antimony_hole");
    public static readonly Texture[] PotassiumIris = Brimstone.API.GetAnimation(glyphTexturePath + "iris_full_potassium.array", "iris_full_potassium", 16);
    public static readonly Texture[] LithiumIris = Brimstone.API.GetAnimation(glyphTexturePath + "iris_full_lithium.array", "iris_full_lithium", 16);

    public static readonly Texture FixationHoleBar = Brimstone.API.GetTexture(glyphTexturePath + "Fixation/separator");
    public static readonly Texture FixationHoleCarbonicInactive = Brimstone.API.GetTexture(glyphTexturePath + "Fixation/carbonic_inactive");
    public static readonly Texture FixationHoleCarbonicActive = Brimstone.API.GetTexture(glyphTexturePath + "Fixation/carbonic_active");
    public static readonly Texture FixationHoleFalseNeuvolicInactive = Brimstone.API.GetTexture(glyphTexturePath + "Fixation/false_neuvolic_inactive");
    public static readonly Texture FixationHoleFalseNeuvolicHalfActive = Brimstone.API.GetTexture(glyphTexturePath + "Fixation/false_neuvolic_half_active");
    public static readonly Texture FixationHoleFalseNeuvolicActive = Brimstone.API.GetTexture(glyphTexturePath + "Fixation/false_neuvolic_active");

    public static readonly Texture[] AntimonyIris = Brimstone.API.GetAnimation(glyphTexturePath + "iris_full_antimony.array", "iris_full_antimony", 16);
    public static readonly Texture[] TrueNeuvolicIris = Brimstone.API.GetAnimation(glyphTexturePath + "iris_full_true_neuvolic.array", "iris_full_true_neuvolics", 16);

    #endregion

    #region Hexes

    public static readonly HexIndex SeparationHoleHex = new(0, 0);
    public static readonly HexIndex SeparationPotassiumIrisHex = new(1, 0);
    public static readonly HexIndex SeparationLithiumIrisHex = new(-1, 0);

    public static readonly HexIndex FixationHole1Hex = new(-1, 0);
    public static readonly HexIndex FixationHole2Hex = new(0, 0);
    public static readonly HexIndex FixationHole3Hex = new(1, 0);
    public static readonly HexIndex FixationAntimonyIrisHex = new(1, -2);
    public static readonly HexIndex FixationTrueNeuvolicIrisHex = new(-1, 2);


    #endregion

    #region Sounds
    public static Sound SeparationSound, FixationSound, ConsolidationSound;

    public static void LoadSounds()
    {
        string contentDir = Brimstone.API.GetContentPath("Neuvolics").method_1087();

        SeparationSound = Brimstone.API.GetSound(contentDir, "sounds/separation").method_1087();
        FixationSound = Brimstone.API.GetSound(contentDir, "sounds/fixation").method_1087();
        //FixationSound = Brimstone.API.GetSound(contentDir, "sounds/consolidation").method_1087();

        FieldInfo field = typeof(class_11).GetField("field_52", BindingFlags.Static | BindingFlags.NonPublic);
        Dictionary<string, float> volumeDictionary = (Dictionary<string, float>)field.GetValue(null);

        volumeDictionary.Add("separation", 0.5f);
        volumeDictionary.Add("fixation", 0.5f);
        //volumeDictionary.Add("consolidation", 0.5f);
    }

    #endregion

    #region Hooks

#if tweaker
    private static ValueTweaker tweaker;
#endif
    public static void AddHooks()
    {
        // If you should get any mail with the subject "STINKY CHEESE", delete it immediately.

        Quintessential.Logger.Log(MainClass.LogPrefix + "Hooking");

#if tweaker
        tweaker = new ValueTweaker();
        IL.SolutionEditorBase.method_1984 += ValueTweakerPhage;
#endif
        IL.class_201.method_540 += SoundPhage;
        
        // move part
        IL.Solution.method_1939 += OHSUpdatePhage;
        // remove part
        IL.Solution.method_1940 += OHSUpdatePhage;
        // undo/redo
        IL.Solution.method_1963 += OHSUpdatePhage;

        // extend track start
        IL.Part.method_1191 += OHSUpdatePhage;
        // extend track end
        IL.Part.method_1192 += OHSUpdatePhage;
        // retreat track start
        IL.Part.method_1193 += OHSUpdatePhage;
        // retreat track end
        IL.Part.method_1194 += OHSUpdatePhage;

        //IL.class_282.method_0 += OHSUpdateFromRotationPhage;
    }

    public static void RemoveHooks()
    {
        Quintessential.Logger.Log(MainClass.LogPrefix + "Unhooking");

        //IL.class_282.method_0 -= OHSUpdateFromRotationPhage;

        IL.Part.method_1194 -= OHSUpdatePhage;
        IL.Part.method_1193 -= OHSUpdatePhage;
        IL.Part.method_1192 -= OHSUpdatePhage;
        IL.Part.method_1191 -= OHSUpdatePhage;

        IL.Solution.method_1963 -= OHSUpdatePhage;
        IL.Solution.method_1940 -= OHSUpdatePhage;
        IL.Solution.method_1939 -= OHSUpdatePhage;
        IL.class_201.method_540 -= SoundPhage;

#if tweaker
        IL.SolutionEditorBase.method_1984 -= ValueTweakerPhage;
#endif

    }

    private static void SoundPhage(ILContext context)
    {
        ILCursor gremlin = new(context);

        if (!gremlin.TryGotoNext(MoveType.Before,
            instr => instr.OpCode == OpCodes.Ret
            ))
        {
            throw new Exception("Could not find end of startup function");
        }

        gremlin.EmitDelegate(static () =>
        {
            SeparationSound.field_4062 = false;
            FixationSound.field_4062 = false;
            //ConsolidationSound.field_4062 = false;
        });
    }

    public static void StaleOH()
    {
        OccupiedHexesStale = true;
    }

    private static void OHSUpdatePhage(ILContext context)
    {
        ILCursor gremlin = new(context);
        gremlin.EmitDelegate(StaleOH);
    }

    // Cursed, doesn't work, no clue why.
    //private static void OHSUpdateFromRotationPhage(ILContext context)
    //{
    //    ILCursor gremlin = new(context);
    //    if (!gremlin.TryGotoNext(MoveType.After,
    //        instr => instr.MatchLdcI4(1),
    //        instr => instr.MatchCall("class_115", "method_205"),
    //        instr => instr.OpCode == OpCodes.Brtrue
    //    ))
    //    {
    //        throw new Exception("Couldn't find left click release!");
    //    }

    //    gremlin.EmitDelegate(StaleOH);
    //}


    internal static HashSet<HexIndex> OccupiedHexes = new();
    public static bool OccupiedHexesStale = true;

#if tweaker
    private static void ValueTweakerPhage(ILContext context)
    {
        ILCursor gremlin = new(context);

        if (!gremlin.TryGotoNext(MoveType.After,
            instr => instr.MatchLdloc(4),
            instr => instr.MatchCallvirt("SolutionEditorBase", "method_1993"),
            instr => instr.MatchLdloc(9)))
        {
            throw new Exception("Could not find part draw loop");
        }

        if (!gremlin.TryGotoNext(MoveType.After,
            instr => instr.OpCode == OpCodes.Blt_S,
            instr => instr.MatchLdloc(3),
            instr => instr.MatchStloc(26))) {
            throw new Exception("Could not find end of loop"); 
        }
        gremlin.EmitDelegate(() =>
        {
            tweaker.Update();
            tweaker.Display(new(500, 500));
        });

    }
#endif



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
            description: "The glyph of fixation consumes a pair of false neuvolics to transmute a true neuvolic to an adjacent form.",
            cost: 20,
            glow: class_238.field_1989.field_97.field_382,
            stroke: class_238.field_1989.field_97.field_383,
            icon: FixationIcon,
            hoveredIcon: FixationIconHover,
            usedHexes: new HexIndex[] {
                FixationHole1Hex,
                FixationHole2Hex,
                FixationHole3Hex,
                FixationAntimonyIrisHex,
                FixationTrueNeuvolicIrisHex
            },
            customPermission: MainClass.FixationPermission
        );

        /*
        Consolidation = Brimstone.API.CreateSimpleGlyph(
            ID: "neuvolics-consolidation",
            name: "Glyph of Consolidation",
            description: "The glyph of consolidation consumes an atom of lithium and potassium to combine them into antimony.",
            cost: 20,
            glow: class_238.field_1989.field_97.field_382,
            stroke: class_238.field_1989.field_97.field_383,
            icon: FixationIcon,
            hoveredIcon: FixationIconHover,
            usedHexes: new HexIndex[] {
                FixationHole1Hex,
                FixationHole2Hex,
                FixationHole3Hex,
                FixationAntimonyIrisHex,
                FixationTrueNeuvolicIrisHex
            },
            customPermission: MainClass.FixationPermission
        );
        */

        #endregion

        QApi.AddPartTypeToPanel(Separation, false);
        QApi.AddPartTypeToPanel(Fixation, false);

        #region Glyph renderers

        QApi.AddPartType(Separation, static (part, pos, editor, renderer) =>
        {
            PartSimState pss = editor.method_507().method_481(part);
            class_236 uco = editor.method_1989(part, pos);
            float time = editor.method_504();
            Vector2 offset = new(123f, 48f);

            renderer.method_523(SeparationBase, new(0, -1f), offset, 0);
            // input
            renderer.method_530(SeparationAntimonyInput, SeparationHoleHex, 0);
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
            renderer.method_528(SeparationPotassiumIrisLip, SeparationPotassiumIrisHex, Vector2.Zero);
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
            renderer.method_528(SeparationLithiumIrisLip, SeparationLithiumIrisHex, Vector2.Zero);
            if (pss.field_2743 && afterIrisOpens)
            {
                // show atom rising infront of iris
                Editor.method_925(risingAtom, risingOffset, new HexIndex(0, 0), 0f, 1f, time, 1f, false, null);
            }
            renderer.method_523(SeparationConnectors, new(0, -1f), offset, 0);
        });

        QApi.AddPartType(Fixation, static (part, pos, editor, renderer) =>
        {
            // hold on to your butts!
            PartSimState pss = editor.method_507().method_481(part);
            class_236 uco = editor.method_1989(part, pos);
            float time = editor.method_504();

            if (OccupiedHexesStale)
            {
                OccupiedHexes.Clear();
                OccupiedHexes.UnionWith(editor.method_502().method_1947(struct_18.field_1431, (enum_137)0));
                OccupiedHexesStale = false;
            }

            int invertedNetMask = 0;

            HexIndex[] nettingHexes = new HexIndex[4] { new(0, 1), new(-1, 1), new(0, -1), new(1, -1) };


            IScreen TOS = GameLogic.field_2434.method_938();

            if (TOS is not SolutionEditorScreen)
            {
                // Not interacting with it, not dragging
                goto NetRemoval;
            }

            interface_0 mode = ((SolutionEditorScreen)TOS).field_4010;

            if (mode is not PartDraggingInputMode)
            {
                // not dragging at all
                goto NetRemoval;
            }


            PartDraggingInputMode drag = (PartDraggingInputMode)mode;

            if (((List<PartDraggingInputMode.DraggedPart>)typeof(PartDraggingInputMode).GetField("field_2712", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(drag)).Any((d) => d.field_2722 == part))
            {
                // we are dragging, and this part is in it.
                goto FixationDrawing;
            }

            NetRemoval: for (int i = 0; i < nettingHexes.Length; i++)
            {
                if (OccupiedHexes.Contains(part.method_1184(nettingHexes[i])))
                {
                    // if hex contains a part, disable it
                    invertedNetMask |= (1 << i);
                }
            }


        FixationDrawing: int atomsPresent = 0;
            HexIndex[] holes = new HexIndex[] { FixationHole1Hex, FixationHole2Hex, FixationHole3Hex };
            foreach (HexIndex h in holes)
            {
                foreach (Molecule m in editor.method_507().method_483())
                {
                    if (m.method_1100().Count == 1 && m.method_1100().TryGetValue(part.method_1184(h), out Atom a))
                    {
                        AtomType aT = a.field_2275;
                        if (aT == Atoms.Potassium)
                        {
                            atomsPresent |= (atomsPresent & 1) != 0 ? 4 : 1;
                        }
                        else if (aT == Atoms.Lithium)
                        {
                            atomsPresent |= (atomsPresent & 2) != 0 ? 4 : 2;
                        }
                        else if (API.GetNeuvolicIndex(aT) != -1)
                        {
                            atomsPresent |= 8;
                        }
                    }
                }
            }

            switch (atomsPresent & 3)
            {
                case 3:
                    // both potassium and lithium are present, invalid.
                    atomsPresent &= ~3;
                    break;
                case 1:
                    atomsPresent |= 2;
                    break;
                default:
                    break;
            }

            atomsPresent >>= 1;

            Vector2 offset = new(122, 190);
            renderer.method_523(FixationBase, new(-1, -1), offset, 0);

            for (int i = 0; i < nettingHexes.Length; i++)
            {
                if ((invertedNetMask & (1 << i)) == 0)
                {
                    renderer.method_523(FixationNets[i], new(-1, -1), offset, 0);
                }
            }

            // input rendering
            foreach (HexIndex h in holes)
            {
                renderer.method_530(class_238.field_1989.field_90.field_255.field_293, h, 0);
                renderer.method_529(FixationHoleBar, h, Vector2.Zero);
                renderer.method_529((atomsPresent & 4) != 0 ? FixationHoleCarbonicActive : FixationHoleCarbonicInactive, h, Vector2.Zero);
                renderer.method_529((atomsPresent & 1) != 0 ? ((atomsPresent & 2) != 0 ? FixationHoleFalseNeuvolicActive : FixationHoleFalseNeuvolicHalfActive) : FixationHoleFalseNeuvolicInactive, h, Vector2.Zero);
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

            renderer.method_523(FixationConnectors, new(-1, -1), offset, 0);

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

                        Brimstone.API.PlaySound(sim, SeparationSound);
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
                        HexIndex hc = part.method_1184(FixationHole2Hex);
                        HexIndex hr = part.method_1184(FixationHole3Hex);
                        HexIndex ia = part.method_1184(FixationAntimonyIrisHex);
                        HexIndex it = part.method_1184(FixationTrueNeuvolicIrisHex);

                        if (sim.FindAtom(ia).method_1085() || sim.FindAtom(it).method_1085())
                        {
                            // blocked!
                            continue;
                        }

                        AtomReference trueNeuvolic = null;
                        AtomReference falseNeuvolic1 = null;
                        AtomReference falseNeuvolic2 = null;

                        int trueNeuIndex = -1;

                        HexIndex[] holes = new HexIndex[] { hl, hc, hr };
                        foreach (HexIndex h in holes)
                        {
                            if (!sim.FindAtom(h).method_99(out AtomReference r) || r.field_2281 || r.field_2282)
                            {
                                // atom not present, not a singleton, or is held
                                goto nextGlyph;
                            }
                            if (r.field_2280 == Atoms.Potassium || r.field_2280 == Atoms.Lithium)
                            {
                                if (falseNeuvolic1 == null)
                                {
                                    falseNeuvolic1 = r;
                                    continue;
                                }
                                if (r.field_2280 != falseNeuvolic1.field_2280)
                                {
                                    // second FN is different from the first
                                    goto nextGlyph;
                                }
                                if (falseNeuvolic2 == null)
                                {
                                    falseNeuvolic2 = r;
                                    continue;
                                }

                                goto nextGlyph;
                            }
                            if (trueNeuIndex != -1)
                            {
                                // second true neuvolic, or invalid atom
                                goto nextGlyph;
                            }
                            trueNeuIndex = API.GetNeuvolicIndex(r.field_2280);
                            if (trueNeuIndex == -1)
                            {
                                // invalid atom present
                                goto nextGlyph;
                            }
                            trueNeuvolic = r;
                        }


                        int delta = falseNeuvolic1.field_2280 == Atoms.Potassium ? -1 : 1;

                        Brimstone.API.RemoveAtom(trueNeuvolic);
                        Brimstone.API.RemoveAtom(falseNeuvolic1);
                        Brimstone.API.RemoveAtom(falseNeuvolic2);

                        Brimstone.API.DrawFallingAtom(seb, trueNeuvolic);
                        Brimstone.API.DrawFallingAtom(seb, falseNeuvolic1);
                        Brimstone.API.DrawFallingAtom(seb, falseNeuvolic2);

                        Brimstone.API.AddSmallCollider(sim, part, FixationAntimonyIrisHex);
                        Brimstone.API.AddSmallCollider(sim, part, FixationTrueNeuvolicIrisHex);

                        pss[part].field_2743 = true;
                        pss[part].field_2744 = new AtomType[2] { Atoms.Antimony, API.GetNeuvolicAtom(trueNeuIndex + delta) };

                        Brimstone.API.PlaySound(sim, FixationSound);
                    }
                    else if (pss[part].field_2743)
                    {
                        Brimstone.API.AddAtom(sim, part, FixationAntimonyIrisHex, pss[part].field_2744[0]);
                        Brimstone.API.AddAtom(sim, part, FixationTrueNeuvolicIrisHex, pss[part].field_2744[1]);
                    }
                }
            nextGlyph:;
            }
        });
    }

}
