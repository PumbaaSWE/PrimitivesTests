using EzGraphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MissileMath;
using PrimitivesTests.Inputs;
using PrimitivesTests.Primitives;
using System.Collections.Generic;

namespace PrimitivesTests
{
    internal class TestPlayer : TestObject
    {
        public override Shape Shape => hull;

        private Hull hull;
        private Circle range;
        private Color inRangeColor = Color.GreenYellow;
        private float inRangeThickness = 1;

        public TestPlayer(Vector2 startPos)
        {
            Vector2[] model = new Vector2[4];
            model[0] = new Vector2(-30, -20);
            model[1] = new Vector2(20, -20);
            model[2] = new Vector2(20, 40);
            model[3] = new Vector2(-20, 20);
            hull = new Hull(model);
            range = new Circle(startPos, 55);
            Position = startPos;
            color = Color.BlueViolet;
        }

        public override void Update(float dt)
        {
            Position = MouseInput.Position;
            Rotation += MouseInput.LeftClicked?1:0;
        }

        public void Interact(List<TestEnemy> enemies)
        {
            color = Color.Black;
            inRangeThickness = 1;
            inRangeColor = Color.Black;
            for (int i = 0; i < enemies.Count; i++)
            {
                if (Intersect(enemies[i]))
                {
                    color = Color.Purple;
                }
                if (ShapeTests.Overlap(range, enemies[i].Shape))
                {
                    inRangeThickness = 4;
                    inRangeColor = Color.GreenYellow;
                }
            }
        }

        public override void SetPosition(Vector2 position)
        {
            hull.Center = position;
            range.center = position;
            base.SetPosition(position);

        }

        public override void SetRotation(float rotation)
        {
            hull.Rotation = rotation;
            base.SetRotation(rotation);
        }

        public override void Draw(SpriteBatch spriteBatch, ShapeBatch shapeBatch)
        {
            shapeBatch.DrawCircle(range, inRangeColor, inRangeThickness);
            base.Draw(spriteBatch, shapeBatch);
        }
    }
}
