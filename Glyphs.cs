//#define tweaker

using Mono.Cecil.Cil;
using MonoMod.Cil;
using Quintessential;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PartType = class_139;

namespace Neuvolics;

public static class Glyphs
{
    #region Glyphs

    public static PartType Separation;
    public static PartType Fixation;
    public static PartType Consolidation;

    #endregion

    #region Hexes

    public static readonly HexIndex SeparationHoleHex = new(0, 0);
    public static readonly HexIndex SeparationPotassiumIrisHex = new(1, 0);
    public static readonly HexIndex SeparationLithiumIrisHex = new(-1, 0);

    public static readonly HexIndex FixationHole1Hex = new(-1, 0);
    public static readonly HexIndex FixationHole2Hex = new(0, 0);
    public static readonly HexIndex FixationHole3Hex = new(1, 0);
    public static readonly HexIndex FixationAntimonyIrisHex = new(1, -2);
    public static readonly HexIndex FixationNeumetalIrisHex = new(-1, 2);

    public static readonly HexIndex ConsolidationHole1Hex = new(0, 1);
    public static readonly HexIndex ConsolidationHole2Hex = new(1, -1);
    public static readonly HexIndex ConsolidationAntimonyIrisHex = new(0, 0);

    #endregion

    #region Sounds
    public static Sound SeparationSound, FixationSound, ConsolidationSound;

    public static void LoadSounds()
    {
        string contentDir = Brimstone.API.GetContentPath("Neuvolics").method_1087();

        SeparationSound = Brimstone.API.GetSound(contentDir, "sounds/separation").method_1087();
        FixationSound = Brimstone.API.GetSound(contentDir, "sounds/fixation").method_1087();
        ConsolidationSound = Brimstone.API.GetSound(contentDir, "sounds/consolidation").method_1087();

        FieldInfo field = typeof(class_11).GetField("field_52", BindingFlags.Static | BindingFlags.NonPublic);
        Dictionary<string, float> volumeDictionary = (Dictionary<string, float>)field.GetValue(null);

        volumeDictionary.Add("separation", 0.5f);
        volumeDictionary.Add("fixation", 0.5f);
        volumeDictionary.Add("consolidation", 0.5f);
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
            ConsolidationSound.field_4062 = false;
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
            cost: 20,
            glow: Textures.Select.TrilineGlow,
            stroke: Textures.Select.TrilineStroke,
            icon: Textures.Icon.Separation,
            hoveredIcon: Textures.Icon.SeparationHover,
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
            description: "The glyph of fixation consumes a pair of the same volics to transmute a neumetal to an adjacent form.",
            cost: 15,
            glow: Textures.Select.ParallelogramGlow,
            stroke: Textures.Select.ParallelogramStroke,
            icon: Textures.Icon.Fixation,
            hoveredIcon: Textures.Icon.FixationHover,
            usedHexes: new HexIndex[] {
                FixationHole1Hex,
                FixationHole2Hex,
                FixationHole3Hex,
                FixationAntimonyIrisHex,
                FixationNeumetalIrisHex
            },
            customPermission: MainClass.FixationPermission
        );

        Consolidation = Brimstone.API.CreateSimpleGlyph(
            ID: "neuvolics-consolidation",
            name: "Glyph of Consolidation",
            description: "The glyph of consolidation consumes an atom of lithium and potassium to combine them into antimony.",
            cost: 10,
            glow: Textures.Select.BendGlow,
            stroke: Textures.Select.BendStroke,
            icon: Textures.Icon.Consolidation,
            hoveredIcon: Textures.Icon.ConsolidationHover,
            usedHexes: new HexIndex[] {
                ConsolidationHole1Hex,
                ConsolidationHole2Hex,
                ConsolidationAntimonyIrisHex
            },
            customPermission: MainClass.ConsolidationPermission
        );

        #endregion

        QApi.AddPartTypeToPanel(Separation, false);
        QApi.AddPartTypeToPanel(Fixation, false);
        QApi.AddPartTypeToPanel(Consolidation, false);

        #region Glyph renderers

        QApi.AddPartType(Separation, static (part, pos, editor, renderer) =>
        {
            PartSimState pss = editor.method_507().method_481(part);
            class_236 uco = editor.method_1989(part, pos);
            float time = editor.method_504();
            Vector2 pivot = new(123f, 48f);

            Vector2 offset = new(0, -1f);
            renderer.method_523(Textures.Separation.Base, offset, pivot, 0);
            // input
            renderer.method_530(Textures.Separation.AntimonyInput, SeparationHoleHex, 0);
            class_135.method_272(Textures.HoleSymbol.Antimony, (class_187.field_1742.method_491(SeparationHoleHex, Vector2.Zero).Rotated(uco.field_1985) + uco.field_1984 - Textures.HoleSymbol.Antimony.field_2056.ToVector2() / 2).Rounded());
            int irisFrame = 15;
            bool afterIrisOpens = false;
            Molecule risingAtom = null;
            Vector2 risingOffset = uco.field_1984 + class_187.field_1742.method_492(SeparationPotassiumIrisHex).Rotated(uco.field_1985);

            renderer.method_528(Textures.Separation.PotassiumIrisBase, SeparationPotassiumIrisHex, Vector2.Zero);
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
            renderer.method_529(Textures.Irises.Potassium[irisFrame], SeparationPotassiumIrisHex, Vector2.Zero);
            renderer.method_528(Textures.Separation.PotassiumIrisLip, SeparationPotassiumIrisHex, Vector2.Zero);
            if (pss.field_2743 && afterIrisOpens)
            {
                // show atom rising infront of iris
                Editor.method_925(risingAtom, risingOffset, new HexIndex(0, 0), 0f, 1f, time, 1f, false, null);
            }

            renderer.method_528(Textures.Separation.LithiumIrisBase, SeparationLithiumIrisHex, Vector2.Zero);
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

            renderer.method_529(Textures.Irises.Lithium[irisFrame], SeparationLithiumIrisHex, Vector2.Zero);
            renderer.method_528(Textures.Separation.LithiumIrisLip, SeparationLithiumIrisHex, Vector2.Zero);
            if (pss.field_2743 && afterIrisOpens)
            {
                // show atom rising infront of iris
                Editor.method_925(risingAtom, risingOffset, new HexIndex(0, 0), 0f, 1f, time, 1f, false, null);
            }
            renderer.method_523(Textures.Separation.Connectors, offset, pivot, 0);
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
                        else if (API.GetNeumetalIndex(aT) != -1)
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

            Vector2 pivot = new(122f, 191f);
            Vector2 offset = new(-1f, -1f);
            renderer.method_523(Textures.Fixation.Base, offset, pivot, 0);

            for (int i = 0; i < nettingHexes.Length; i++)
            {
                if (((invertedNetMask >> i) & 1) == 0)
                {
                    renderer.method_523(Textures.Fixation.Nets[i], offset, pivot, 0);
                }
            }

            // input rendering
            foreach (HexIndex h in holes)
            {
                renderer.method_530(class_238.field_1989.field_90.field_255.field_293, h, 0);
                renderer.method_529(Textures.Fixation.HoleBar, h, Vector2.Zero);
                renderer.method_529((atomsPresent & 4) != 0 ? Textures.Fixation.HoleNeumetalInactive : Textures.Fixation.HoleNeumetalActive, h, Vector2.Zero);
                renderer.method_529((atomsPresent & 1) != 0 ? ((atomsPresent & 2) != 0 ? Textures.Fixation.HoleVolicInactive : Textures.Fixation.HoleVolicHalfActive) : Textures.Fixation.HoleVolicActive, h, Vector2.Zero);
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
            renderer.method_529(Textures.Irises.Antimony[irisFrame], FixationAntimonyIrisHex, Vector2.Zero);
            renderer.method_528(class_238.field_1989.field_90.field_228.field_271, FixationAntimonyIrisHex, Vector2.Zero);
            if (pss.field_2743 && afterIrisOpens)
            {
                // show atom rising infront of iris
                Editor.method_925(risingAtom, risingOffset, new HexIndex(0, 0), 0f, 1f, time, 1f, false, null);
            }

            renderer.method_528(class_238.field_1989.field_90.field_228.field_272, FixationNeumetalIrisHex, Vector2.Zero);
            if (pss.field_2743)
            {
                risingOffset = uco.field_1984 + class_187.field_1742.method_492(FixationNeumetalIrisHex).Rotated(uco.field_1985);
                risingAtom = Molecule.method_1121(pss.field_2744[1]);
                if (!afterIrisOpens)
                {
                    // show atom rising behind iris
                    Editor.method_925(risingAtom, risingOffset, new HexIndex(0, 0), 0f, 1f, time, 1f, false, null);
                }
            }

            renderer.method_529(Textures.Irises.Neumetal[irisFrame], FixationNeumetalIrisHex, Vector2.Zero);
            renderer.method_528(class_238.field_1989.field_90.field_228.field_271, FixationNeumetalIrisHex, Vector2.Zero);
            if (pss.field_2743 && afterIrisOpens)
            {
                // show atom rising infront of iris
                Editor.method_925(risingAtom, risingOffset, new HexIndex(0, 0), 0f, 1f, time, 1f, false, null);
            }

            renderer.method_523(Textures.Fixation.Connectors, offset, pivot, 0);

        });

        QApi.AddPartType(Consolidation, static (part, pos, editor, renderer) =>
        {
            PartSimState pss = editor.method_507().method_481(part);
            class_236 uco = editor.method_1989(part, pos);
            float time = editor.method_504();

            int atomsPresent = 0;

            HexIndex[] holes = new HexIndex[] { ConsolidationHole1Hex, ConsolidationHole2Hex };
            foreach (HexIndex h in holes)
            {
                foreach (Molecule m in editor.method_507().method_483())
                {
                    if (m.method_1100().Count == 1 && m.method_1100().TryGetValue(part.method_1184(h), out Atom a))
                    {
                        AtomType aT = a.field_2275;
                        if (aT == Atoms.Potassium)
                        {
                            atomsPresent |= 1;
                        }
                        else if (aT == Atoms.Lithium)
                        {
                            atomsPresent |= 2;
                        }
                    }
                }
            }

            Vector2 pivot = new(41f, 119f);
            Vector2 offset = new(-1f, -1f);
            renderer.method_523(Textures.Consolidation.Base, offset, pivot, 0f);

            foreach (HexIndex h in holes)
            {
                renderer.method_530(Textures.Consolidation.VolicInput, h, 0f);
            }

            // iris
            int irisFrame = 15;
            bool afterIrisOpens = false;
            Molecule risingAtom = null;
            Vector2 risingOffset = uco.field_1984 + class_187.field_1742.method_492(ConsolidationAntimonyIrisHex).Rotated(uco.field_1985);

            renderer.method_528(Textures.Consolidation.IrisBase, ConsolidationAntimonyIrisHex, Vector2.Zero);
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
            renderer.method_529(Textures.Irises.Antimony[irisFrame], ConsolidationAntimonyIrisHex, Vector2.Zero);
            renderer.method_528(Textures.Consolidation.IrisLip, ConsolidationAntimonyIrisHex, Vector2.Zero);
            if (pss.field_2743 && afterIrisOpens)
            {
                // show atom rising infront of iris
                Editor.method_925(risingAtom, risingOffset, new HexIndex(0, 0), 0f, 1f, time, 1f, false, null);
            }
            renderer.method_523(Textures.Consolidation.Connectors, offset, pivot, 0);
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
                        HexIndex it = part.method_1184(FixationNeumetalIrisHex);

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
                            trueNeuIndex = API.GetNeumetalIndex(r.field_2280);
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
                        Brimstone.API.AddSmallCollider(sim, part, FixationNeumetalIrisHex);

                        pss[part].field_2743 = true;
                        pss[part].field_2744 = new AtomType[2] { Atoms.Antimony, API.GetNeuvolicAtom(trueNeuIndex + delta) };

                        Brimstone.API.PlaySound(sim, FixationSound);
                    }
                    else if (pss[part].field_2743)
                    {
                        Brimstone.API.AddAtom(sim, part, FixationAntimonyIrisHex, pss[part].field_2744[0]);
                        Brimstone.API.AddAtom(sim, part, FixationNeumetalIrisHex, pss[part].field_2744[1]);
                    }
                }
                else if (type == Consolidation)
                {
                    if (first)
                    {
                        HexIndex h1 = part.method_1184(ConsolidationHole1Hex);
                        HexIndex h2 = part.method_1184(ConsolidationHole2Hex);
                        HexIndex ia = part.method_1184(ConsolidationAntimonyIrisHex);

                        if (sim.FindAtom(ia).method_1085())
                        {
                            // blocked!
                            continue;
                        }
                        if (!sim.FindAtom(h1).method_99(out AtomReference a1) || a1.field_2281 || a1.field_2282)
                        {
                            continue;
                        }
                        if (!sim.FindAtom(h2).method_99(out AtomReference a2) || a2.field_2281 || a2.field_2282)
                        {
                            continue;
                        }

                        if ((a1.field_2280 != Atoms.Lithium || a2.field_2280 != Atoms.Potassium) && (a1.field_2280 != Atoms.Potassium || a2.field_2280 != Atoms.Lithium))
                        {
                            continue;
                        }

                        Brimstone.API.RemoveAtom(a1);
                        Brimstone.API.RemoveAtom(a2);

                        Brimstone.API.DrawFallingAtom(seb, a1);
                        Brimstone.API.DrawFallingAtom(seb, a2);

                        Brimstone.API.AddSmallCollider(sim, part, ConsolidationAntimonyIrisHex);
                        pss[part].field_2743 = true;
                        pss[part].field_2744 = new AtomType[1] { Atoms.Antimony };

                        Brimstone.API.PlaySound(sim, ConsolidationSound);
                    }
                    else if (pss[part].field_2743)
                    {
                        Brimstone.API.AddAtom(sim, part, ConsolidationAntimonyIrisHex, pss[part].field_2744[0]);
                    }
                }
            nextGlyph:;
            }
        });
    }

}
