using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimitivesTests.Inputs
{
    internal static class MouseInput
    {

        static MouseState prevState;
        static MouseState currState;

        public static void UpdateStates()
        {
            prevState = currState;
            currState = Mouse.GetState();
        }

        public static Vector2 Position => currState.Position.ToVector2();

        public static bool LeftClicked => currState.LeftButton == ButtonState.Pressed && prevState.LeftButton == ButtonState.Released;
        public static bool LeftPressed => currState.LeftButton == ButtonState.Pressed;
        public static bool LeftReleased => currState.LeftButton == ButtonState.Released;

        public static bool RightClicked => currState.RightButton == ButtonState.Pressed && prevState.RightButton == ButtonState.Released;
        public static bool RightPressed => currState.RightButton == ButtonState.Pressed;
        public static bool RightReleased => currState.RightButton == ButtonState.Released;

    }
}
