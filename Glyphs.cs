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
    public static PartType Putrefaction;

    #endregion

    #region Hexes

    public static readonly HexIndex SeparationHoleHex = new(0, 0);
    public static readonly HexIndex SeparationGelaronIrisHex = new(1, 0);
    public static readonly HexIndex SeparationFrixonIrisHex = new(-1, 0);

    public static readonly HexIndex FixationHole1Hex = new(-1, 0);
    public static readonly HexIndex FixationHole2Hex = new(0, 0);
    public static readonly HexIndex FixationHole3Hex = new(1, 0);
    public static readonly HexIndex FixationZephironIrisHex = new(1, -2);
    public static readonly HexIndex FixationNeumetalIrisHex = new(-1, 2);

    public static readonly HexIndex ConsolidationHole1Hex = new(0, 1);
    public static readonly HexIndex ConsolidationHole2Hex = new(1, -1);
    public static readonly HexIndex ConsolidationZephironIrisHex = new(0, 0);

    public static readonly HexIndex PutrefactionBowlHex = new(1, 0);
    public static readonly HexIndex PutrefactionHole1Hex = new(0, -1);
    public static readonly HexIndex PutrefactionHole2Hex = new(-1, 1);

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
            description: "The glyph of separation divides an atom of zephiron in an atom of frixon and gelaron.",
            cost: 20,
            glow: Textures.Select.TrilineGlow,
            stroke: Textures.Select.TrilineStroke,
            icon: Textures.Icon.Separation,
            hoveredIcon: Textures.Icon.SeparationHover,
            usedHexes: new HexIndex[] {
                SeparationGelaronIrisHex,
                SeparationHoleHex,
                SeparationFrixonIrisHex
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
                FixationZephironIrisHex,
                FixationNeumetalIrisHex
            },
            customPermission: MainClass.FixationPermission
        );

        Consolidation = Brimstone.API.CreateSimpleGlyph(
            ID: "neuvolics-consolidation",
            name: "Glyph of Consolidation",
            description: "The glyph of consolidation consumes an atom of frixon and gelaron to combine them into zephiron.",
            cost: 10,
            glow: Textures.Select.BendGlow,
            stroke: Textures.Select.BendStroke,
            icon: Textures.Icon.Consolidation,
            hoveredIcon: Textures.Icon.ConsolidationHover,
            usedHexes: new HexIndex[] {
                ConsolidationHole1Hex,
                ConsolidationHole2Hex,
                ConsolidationZephironIrisHex
            },
            customPermission: MainClass.ConsolidationPermission
        );

        Putrefaction = Brimstone.API.CreateSimpleGlyph(
            ID: "neuvolics-putrefaction",
            name: "Glyph of Putrefaction",
            description: "The glyph of putrefaction consumes one or two atoms of frixon or gelaron to transmute a neumetal to an adjacent form.",
            cost: 10,
            glow: class_238.field_1989.field_97.field_384,
            stroke: class_238.field_1989.field_97.field_385,
            icon: Textures.Icon.Putrefaction,
            hoveredIcon: Textures.Icon.PutrefactionHover,
            usedHexes: new HexIndex[]
            {
                new(0,0),
                PutrefactionBowlHex,
                PutrefactionHole1Hex,
                PutrefactionHole2Hex
            },
            customPermission: MainClass.PutrefactionPermission
        );

        #endregion

        QApi.AddPartTypeToPanel(Separation, false);
        QApi.AddPartTypeToPanel(Fixation, false);
        QApi.AddPartTypeToPanel(Consolidation, false);
        QApi.AddPartTypeToPanel(Putrefaction, false);

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
            renderer.method_530(Textures.Separation.ZephironInput, SeparationHoleHex, 0);
            class_135.method_272(Textures.HoleSymbol.Zephiron, (class_187.field_1742.method_491(SeparationHoleHex, Vector2.Zero).Rotated(uco.field_1985) + uco.field_1984 - Textures.HoleSymbol.Zephiron.field_2056.ToVector2() / 2).Rounded());
            int irisFrame = 15;
            bool afterIrisOpens = false;
            Molecule risingAtom = null;
            Vector2 risingOffset = uco.field_1984 + class_187.field_1742.method_492(SeparationGelaronIrisHex).Rotated(uco.field_1985);

            renderer.method_528(Textures.Separation.GelaronIrisBase, SeparationGelaronIrisHex, Vector2.Zero);
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
            renderer.method_529(Textures.Irises.Gelaron[irisFrame], SeparationGelaronIrisHex, Vector2.Zero);
            renderer.method_528(Textures.Separation.GelaronIrisLip, SeparationGelaronIrisHex, Vector2.Zero);
            if (pss.field_2743 && afterIrisOpens)
            {
                // show atom rising infront of iris
                Editor.method_925(risingAtom, risingOffset, new HexIndex(0, 0), 0f, 1f, time, 1f, false, null);
            }

            renderer.method_528(Textures.Separation.FrixonIrisBase, SeparationFrixonIrisHex, Vector2.Zero);
            if (pss.field_2743)
            {
                risingOffset = uco.field_1984 + class_187.field_1742.method_492(SeparationFrixonIrisHex).Rotated(uco.field_1985);
                risingAtom = Molecule.method_1121(pss.field_2744[1]);
                if (!afterIrisOpens)
                {
                    // show atom rising behind iris
                    Editor.method_925(risingAtom, risingOffset, new HexIndex(0, 0), 0f, 1f, time, 1f, false, null);
                }
            }

            renderer.method_529(Textures.Irises.Frixon[irisFrame], SeparationFrixonIrisHex, Vector2.Zero);
            renderer.method_528(Textures.Separation.FrixonIrisLip, SeparationFrixonIrisHex, Vector2.Zero);
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
                        if (aT == Atoms.Gelaron)
                        {
                            atomsPresent |= (atomsPresent & 1) != 0 ? 4 : 1;
                        }
                        else if (aT == Atoms.Frixon)
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
                    // both frixon and gelaron are present, invalid.
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
            Vector2 risingOffset = uco.field_1984 + class_187.field_1742.method_492(FixationZephironIrisHex).Rotated(uco.field_1985);

            renderer.method_528(class_238.field_1989.field_90.field_228.field_272, FixationZephironIrisHex, Vector2.Zero);
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
            renderer.method_529(Textures.Irises.Zephiron[irisFrame], FixationZephironIrisHex, Vector2.Zero);
            renderer.method_528(class_238.field_1989.field_90.field_228.field_271, FixationZephironIrisHex, Vector2.Zero);
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
                        if (aT == Atoms.Gelaron)
                        {
                            atomsPresent |= 1;
                        }
                        else if (aT == Atoms.Frixon)
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
                renderer.method_529(Textures.Consolidation.HoleBar, h, Vector2.Zero);
                renderer.method_529((atomsPresent & 1) == 0 ? Textures.Consolidation.HoleGelaronActive : Textures.Consolidation.HoleGelaronInactive, h, Vector2.Zero);
                renderer.method_529((atomsPresent & 2) == 0 ? Textures.Consolidation.HoleFrixonActive : Textures.Consolidation.HoleFrixonInactive, h, Vector2.Zero);
            }

            // iris
            int irisFrame = 15;
            bool afterIrisOpens = false;
            Molecule risingAtom = null;
            Vector2 risingOffset = uco.field_1984 + class_187.field_1742.method_492(ConsolidationZephironIrisHex).Rotated(uco.field_1985);

            renderer.method_528(Textures.Consolidation.IrisBase, ConsolidationZephironIrisHex, Vector2.Zero);
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
            renderer.method_529(Textures.Irises.Zephiron[irisFrame], ConsolidationZephironIrisHex, Vector2.Zero);
            renderer.method_528(Textures.Consolidation.IrisLip, ConsolidationZephironIrisHex, Vector2.Zero);
            if (pss.field_2743 && afterIrisOpens)
            {
                // show atom rising infront of iris
                Editor.method_925(risingAtom, risingOffset, new HexIndex(0, 0), 0f, 1f, time, 1f, false, null);
            }
            renderer.method_523(Textures.Consolidation.Connectors, offset, pivot, 0);
        });

        QApi.AddPartType(Putrefaction, static (part, pos, editor, renderer) =>
        {
            //PartSimState pss = editor.method_507().method_481(part);
            //class_236 uco = editor.method_1989(part, pos);
            //float time = editor.method_504();

            Vector2 pivot = new(83f, 119f);
            Vector2 offset = new(0, -1f);
            renderer.method_523(Textures.Putrefaction.Base, offset, pivot, 0);

            HexIndex[] holes = new HexIndex[]
            {
                PutrefactionHole1Hex, PutrefactionHole2Hex
            };

            foreach (HexIndex h in holes)
            {
                renderer.method_530(class_238.field_1989.field_90.field_255.field_293, h, 0);
                renderer.method_529(Textures.HoleSymbol.Volic, h, Vector2.Zero);
            }

            renderer.method_530(class_238.field_1989.field_90.field_170, PutrefactionBowlHex, 0);
            renderer.method_529(Textures.BowlSymbol.Neumetal, PutrefactionBowlHex, Vector2.Zero);

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
                        HexIndex iF = part.method_1184(SeparationFrixonIrisHex);
                        HexIndex iP = part.method_1184(SeparationGelaronIrisHex);

                        if (sim.FindAtom(iF).method_1085() || sim.FindAtom(iP).method_1085())
                        {
                            // blocked!
                            continue;
                        }

                        if (!sim.FindAtom(h).method_99(out AtomReference holeAtom))
                        {
                            // no atom above hole
                            continue;
                        }

                        if (holeAtom.field_2280 != Atoms.Zephiron || holeAtom.field_2281 || holeAtom.field_2282)
                        {
                            continue;
                        }

                        // an unheld, elemental zephiron atom is above the hole, and neither iris is covered
                        Brimstone.API.RemoveAtom(holeAtom);
                        Brimstone.API.DrawFallingAtom(seb, holeAtom);

                        Brimstone.API.AddSmallCollider(sim, part, SeparationGelaronIrisHex);
                        Brimstone.API.AddSmallCollider(sim, part, SeparationFrixonIrisHex);

                        pss[part].field_2743 = true;
                        pss[part].field_2744 = new AtomType[2] { Atoms.Gelaron, Atoms.Frixon };

                        Brimstone.API.PlaySound(sim, SeparationSound);
                    }
                    else if (pss[part].field_2743)
                    {
                        Brimstone.API.AddAtom(sim, part, SeparationGelaronIrisHex, pss[part].field_2744[0]);
                        Brimstone.API.AddAtom(sim, part, SeparationFrixonIrisHex, pss[part].field_2744[1]);
                    }
                }
                else if (type == Fixation)
                {
                    if (first)
                    {
                        HexIndex hLeft = part.method_1184(FixationHole1Hex);
                        HexIndex hCenter = part.method_1184(FixationHole2Hex);
                        HexIndex hRight = part.method_1184(FixationHole3Hex);
                        HexIndex iZ = part.method_1184(FixationZephironIrisHex);
                        HexIndex iN = part.method_1184(FixationNeumetalIrisHex);

                        if (sim.FindAtom(iZ).method_1085() || sim.FindAtom(iN).method_1085())
                        {
                            // blocked!
                            continue;
                        }

                        AtomReference neumetal = null;
                        AtomReference volic1 = null;
                        AtomReference volic2 = null;

                        int neumetalIndex = -1;

                        HexIndex[] holes = new HexIndex[] { hLeft, hCenter, hRight };
                        foreach (HexIndex h in holes)
                        {
                            if (!sim.FindAtom(h).method_99(out AtomReference r) || r.field_2281 || r.field_2282)
                            {
                                // atom not present, not a singleton, or is held
                                goto nextGlyph;
                            }
                            if (r.field_2280 == Atoms.Gelaron || r.field_2280 == Atoms.Frixon)
                            {
                                if (volic1 == null)
                                {
                                    volic1 = r;
                                    continue;
                                }
                                if (r.field_2280 != volic1.field_2280)
                                {
                                    // second FN is different from the first
                                    goto nextGlyph;
                                }
                                if (volic2 == null)
                                {
                                    volic2 = r;
                                    continue;
                                }

                                goto nextGlyph;
                            }
                            if (neumetalIndex != -1)
                            {
                                // second neumetal, or invalid atom
                                goto nextGlyph;
                            }
                            neumetalIndex = API.GetNeumetalIndex(r.field_2280);
                            if (neumetalIndex == -1)
                            {
                                // invalid atom present
                                goto nextGlyph;
                            }
                            neumetal = r;
                        }


                        int delta = volic1.field_2280 == Atoms.Gelaron ? -1 : 1;

                        Brimstone.API.RemoveAtom(neumetal);
                        Brimstone.API.RemoveAtom(volic1);
                        Brimstone.API.RemoveAtom(volic2);

                        Brimstone.API.DrawFallingAtom(seb, neumetal);
                        Brimstone.API.DrawFallingAtom(seb, volic1);
                        Brimstone.API.DrawFallingAtom(seb, volic2);

                        Brimstone.API.AddSmallCollider(sim, part, FixationZephironIrisHex);
                        Brimstone.API.AddSmallCollider(sim, part, FixationNeumetalIrisHex);

                        pss[part].field_2743 = true;
                        pss[part].field_2744 = new AtomType[2] { Atoms.Zephiron, API.GetNeumetalAtom(neumetalIndex + delta) };

                        Brimstone.API.PlaySound(sim, FixationSound);
                    }
                    else if (pss[part].field_2743)
                    {
                        Brimstone.API.AddAtom(sim, part, FixationZephironIrisHex, pss[part].field_2744[0]);
                        Brimstone.API.AddAtom(sim, part, FixationNeumetalIrisHex, pss[part].field_2744[1]);
                    }
                }
                else if (type == Consolidation)
                {
                    if (first)
                    {
                        HexIndex h1 = part.method_1184(ConsolidationHole1Hex);
                        HexIndex h2 = part.method_1184(ConsolidationHole2Hex);
                        HexIndex ia = part.method_1184(ConsolidationZephironIrisHex);

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

                        if ((a1.field_2280 != Atoms.Frixon || a2.field_2280 != Atoms.Gelaron) && (a1.field_2280 != Atoms.Gelaron || a2.field_2280 != Atoms.Frixon))
                        {
                            continue;
                        }

                        Brimstone.API.RemoveAtom(a1);
                        Brimstone.API.RemoveAtom(a2);

                        Brimstone.API.DrawFallingAtom(seb, a1);
                        Brimstone.API.DrawFallingAtom(seb, a2);

                        Brimstone.API.AddSmallCollider(sim, part, ConsolidationZephironIrisHex);
                        pss[part].field_2743 = true;
                        pss[part].field_2744 = new AtomType[1] { Atoms.Zephiron };

                        Brimstone.API.PlaySound(sim, ConsolidationSound);
                    }
                    else if (pss[part].field_2743)
                    {
                        Brimstone.API.AddAtom(sim, part, ConsolidationZephironIrisHex, pss[part].field_2744[0]);
                    }
                }
                else if (type == Putrefaction)
                {
                    HexIndex bowl = part.method_1184(PutrefactionBowlHex);
                    HexIndex hole1 = part.method_1184(PutrefactionHole1Hex);
                    HexIndex hole2 = part.method_1184(PutrefactionHole2Hex);
                    if (!sim.FindAtom(bowl).method_99(out AtomReference bowlAtom))
                    {
                        //bowl empty
                        continue;
                    }

                    int neumetalIndex = API.GetNeumetalIndex(bowlAtom.field_2280);
                    if (neumetalIndex == -1) {
                        // invalid atom
                        continue;
                    }

                    bool consumeHole1 = sim.FindAtom(hole1).method_99(out AtomReference hole1Atom) && !hole1Atom.field_2281 && !hole1Atom.field_2282;
                    bool consumeHole2 = sim.FindAtom(hole2).method_99(out AtomReference hole2Atom) && !hole2Atom.field_2281 && !hole2Atom.field_2282;

                    if (consumeHole1)
                    {
                        if (hole1Atom.field_2280 == Atoms.Frixon)
                        {
                            neumetalIndex += 1;
                        }
                        else if (hole1Atom.field_2280 == Atoms.Gelaron)
                        {
                            neumetalIndex -= 1;
                        }
                        else
                        {
                            consumeHole1 = false;
                        }
                    }
                    if (consumeHole2)
                    {
                        if (hole2Atom.field_2280 == Atoms.Frixon)
                        {
                            neumetalIndex += 1;
                        }
                        else if (hole2Atom.field_2280 == Atoms.Gelaron)
                        {
                            neumetalIndex -= 1;
                        }
                        else
                        {
                            consumeHole2 = false;
                        }
                    }

                    if (!consumeHole1 && !consumeHole2)
                    {
                        // neither atom could be consumed
                        continue;
                    }

                    if (consumeHole1)
                    {
                        Brimstone.API.RemoveAtom(hole1Atom);
                        Brimstone.API.DrawFallingAtom(seb, hole1Atom);
                    }
                    if (consumeHole2)
                    {
                        Brimstone.API.RemoveAtom(hole2Atom);
                        Brimstone.API.DrawFallingAtom(seb, hole2Atom);
                    }

                    Brimstone.API.ChangeAtom(bowlAtom, API.GetNeumetalAtom(neumetalIndex));
                    bowlAtom.field_2279.field_2276 = new class_168(seb, 0, (enum_132)1, bowlAtom.field_2280, class_238.field_1989.field_81.field_614, 30f);

                }
                else if (type == class_191.field_1775) // triplex bonder
                {
                    foreach (class_222 bonder in type.field_1538)
                    {
                        if (!sim.FindAtomRelative(part, bonder.field_1920).method_99(out AtomReference leftAtom) || !sim.FindAtomRelative(part, bonder.field_1921).method_99(out AtomReference rightAtom))
                        {
                            continue;
                        }
                        bool isLeftFire = leftAtom.field_2280 == Brimstone.API.VanillaAtoms.fire;
                        bool isLeftVolic = leftAtom.field_2280 == Atoms.Frixon || leftAtom.field_2280 == Atoms.Gelaron;
                        bool isRightFire = rightAtom.field_2280 == Brimstone.API.VanillaAtoms.fire;
                        bool isRightVolic = rightAtom.field_2280 == Atoms.Frixon || rightAtom.field_2280 == Atoms.Gelaron;

                        if ((isLeftFire && isRightVolic) || (isLeftVolic && isRightFire) || (isLeftVolic && isRightVolic))
                        {
                            Brimstone.API.JoinMoleculesAtHexes(sim, part, bonder.field_1920, bonder.field_1921);
                            Brimstone.API.AddBond(sim, part, bonder.field_1920, bonder.field_1921, bonder.field_1922);
                        }
                    }
                }
            nextGlyph:;
            }
        });
    }

}
