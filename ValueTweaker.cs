using Quintessential;

namespace Neuvolics;

public class ValueTweaker
{
    public int V1 = 0;
    public int V2 = 0;
    public int V3 = 0;
    public int V4 = 0;

    public void Update()
    {
        if (class_115.method_200(SDL2.SDL.enum_160.SDLK_y))
        {
            V1 += 1;
        }
        else if (class_115.method_200(SDL2.SDL.enum_160.SDLK_h))
        {
            V1 -= 1;
        }

        if (class_115.method_200(SDL2.SDL.enum_160.SDLK_u))
        {
            V2 += 1;
        }
        else if (class_115.method_200(SDL2.SDL.enum_160.SDLK_j))
        {
            V2 -= 1;
        }

        if (class_115.method_200(SDL2.SDL.enum_160.SDLK_i))
        {
            V3 += 1;
        }
        else if (class_115.method_200(SDL2.SDL.enum_160.SDLK_k))
        {
            V3 -= 1;
        }

        if (class_115.method_200(SDL2.SDL.enum_160.SDLK_o))
        {
            V4 += 1;
        }
        else if (class_115.method_200(SDL2.SDL.enum_160.SDLK_l))
        {
            V4 -= 1;
        }
    }

    public void Display(Vector2 pos)
    {
        UI.DrawText($"{V1}, {V2}, {V3}, {V4}", pos, UI.Text, UI.TextColor, TextAlignment.Centred);
    }
}
