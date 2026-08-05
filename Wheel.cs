using MonoMod.Utils;
using Quintessential;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PartType = class_139;

namespace Neuvolics;

// Borrowed from Reductive Metallurgy
public static class Wheel
{
    const string SoriaStateString = "Neuvoics_MoroWheelAtoms";
    const float sixtyDegrees = (float)Math.PI / 3f;

    static Molecule MaroMolecule()
    {
        Molecule molecule = new();
        molecule.method_1105(new(Atoms.Gelaron), new HexIndex(0, 1));
        molecule.method_1105(new(Atoms.Zephiron), new HexIndex(1, 0));
        molecule.method_1105(new(Atoms.Frixon), new HexIndex(1, -1));
        molecule.method_1105(new(Atoms.Gelaron), new HexIndex(0, -1));
        molecule.method_1105(new(Atoms.Zephiron), new HexIndex(-1, 0));
        molecule.method_1105(new(Atoms.Frixon), new HexIndex(-1, 1));
        return molecule;
    }

    public static PartType Maro;

    public static void LoadWheel()
    {
        Maro = new()
        {
            /*ID*/
            field_1528 = "neuvolics-maro",
            /*Name*/
            field_1529 = class_134.method_253("Maro's Wheel", string.Empty),
            /*Desc*/
            field_1530 = class_134.method_253("By using Maro's wheel with the glyph of cataclysm, the volics may be freely interchanged", string.Empty),
            /*Cost*/
            field_1531 = 25,
            /*Type*/
            field_1532 = (enum_2)1,
            /*Programmable?*/
            field_1533 = true,
            /*Force-rotatable*/
            field_1536 = true,
            /*Berlo Atoms*/
            field_1544 = new Dictionary<HexIndex, AtomType>(),
            /*Icon*/
            field_1547 = Textures.Icon.Maro,
            /*Hover Icon*/
            field_1548 = Textures.Icon.MaroHover,
            /*Only One Allowed?*/
            field_1552 = true,
            CustomPermissionCheck = perms => perms.Contains(MainClass.MaroPermission)
        };
        foreach (HexIndex hex in HexIndex.AdjacentOffsets)
            Maro.field_1544.Add(hex, Brimstone.API.VanillaAtoms.quicksilver);


        QApi.AddPartTypeToPanel(Maro, class_191.field_1771);
        QApi.AddPartType(Maro, DrawMaroWheel);
    }

    private static void SetMaroWheelData<T>(PartSimState state, string field, T data) => new DynamicData(state).Set(field, data);
    private static T GetMaroWheelData<T>(PartSimState state, string field, T initial)
    {
        var data = new DynamicData(state).Get(field);
        if (data == null)
        {
            SetMaroWheelData(state, field, initial);
            return initial;
        }
        else
        {
            return (T)data;
        }
    }

    public static void DrawSelectionGlow(SolutionEditorBase seb_self, Part part, Vector2 pos, float alpha)
    {
        var cageSelectGlowTexture = class_238.field_1989.field_97.field_367;
        int armLength = 1; // part.method_1165()
        class_236 class236 = seb_self.method_1989(part, pos);
        Color color = Color.White.WithAlpha(alpha);

        typeof(SolutionEditorBase).GetMethod("method_2006", BindingFlags.NonPublic | BindingFlags.Static).Invoke(seb_self, new object[] { armLength, class_191.field_1767.field_1534, class236, color });
        for (int index = 0; index < 6; ++index)
        {
            float num = index * sixtyDegrees;
            typeof(SolutionEditorBase).GetMethod("method_2016", BindingFlags.NonPublic | BindingFlags.Static).Invoke(seb_self, new object[] { cageSelectGlowTexture, color, class236.field_1984, class236.field_1985 + num });
        }
    }

    public static void DrawMaroAtoms(SolutionEditorBase seb_self, Part part, Vector2 pos, bool active = false)
    {
        if (part.method_1159() != Maro)
            return;
        PartSimState partSimState = seb_self.method_507().method_481(part);

        class_236 class236 = seb_self.method_1989(part, pos);
        Molecule molecule = GetMaroWheelAtoms(partSimState);
        Editor.method_925(molecule, class236.field_1984, new HexIndex(0, 0), class236.field_1985, 1f, 1f, 1f, false, seb_self);
    }

    public static void DrawMaroFlash(SolutionEditorBase seb, Part part, HexIndex hex)
    {
        DrawMaroFlash(seb, part.method_1184(hex));
    }

    public static void DrawMaroFlash(SolutionEditorBase seb, HexIndex hex)
    {
        // todo,
        //seb.field_3935.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(hex), Textures.Soria.Flash, 30f, Vector2.Zero, 0f));
    }

    private static Molecule GetMaroWheelAtoms(PartSimState state) => GetMaroWheelData(state, SoriaStateString, MaroMolecule());

    static void DrawMaroWheel(Part part, Vector2 pos, SolutionEditorBase editor, class_195 renderer)
    {
        // draw atoms, if the simulation is stopped - otherwise, the running simulation will draw them
        if (editor.method_503() == enum_128.Stopped)
        {
            DrawMaroAtoms(editor, part, pos);
        }

        // draw arm stubs
        class_236 class236 = editor.method_1989(part, pos);
        typeof(SolutionEditorBase).GetMethod("method_2005", BindingFlags.NonPublic | BindingFlags.Static).Invoke(editor, new object[] { part.method_1165(), class_191.field_1767.field_1534, class236 });

        // draw cages
        PartSimState partSimState = editor.method_507().method_481(part);
        for (int i = 0; i < 6; i++)
        {
            float radians = renderer.field_1798 + (i * sixtyDegrees);
            Vector2 vector2_9 = renderer.field_1797 + class_187.field_1742.method_492(new HexIndex(1, 0)).Rotated(radians);
            typeof(SolutionEditorBase).GetMethod("method_2003", BindingFlags.NonPublic | BindingFlags.Static).Invoke(editor, new object[] { class_238.field_1989.field_90.field_232, vector2_9, new Vector2(39f, 33f), radians });
        }
    }

    public static Maybe<AtomReference> MaybeFindMaroWheelAtom(Sim sim_self, Part part, HexIndex offset) => MaybeFindMaroWheelAtom(sim_self, part.method_1184(offset));

    public static Maybe<AtomReference> MaybeFindMaroWheelAtom(Sim sim_self, HexIndex hex)
    {
        var SEB = sim_self.field_3818;
        var solution = SEB.method_502();
        var partList = solution.field_3919;
        var partSimStates = sim_self.field_3821;

        foreach (var maro in partList.Where(x => x.method_1159() == Maro))
        {
            var partSimState = partSimStates[maro];
            Molecule maroAtoms = GetMaroWheelAtoms(partSimState);
            var hexIndex = partSimState.field_2724;
            var rotation = partSimState.field_2726;
            var hexKey = (hex - hexIndex).Rotated(rotation.Negative());

            if (maroAtoms.method_1100().TryGetValue(hexKey, out Atom atom))
            {
                return new AtomReference(maroAtoms, hexKey, atom.field_2275, atom, true);
            }
        }
        return struct_18.field_1431;
    }

}