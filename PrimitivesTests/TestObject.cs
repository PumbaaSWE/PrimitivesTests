using Microsoft.Xna.Framework.Graphics;
using PrimitivesTests.Primitives;
using EzGraphics;
using Microsoft.Xna.Framework;

namespace PrimitivesTests
{
    public abstract class TestObject
    {
        public abstract Shape Shape { get; }

        protected Vector2 position;
        protected Vector2 velocity;
        protected float rotation;
        protected Color color;

        public Vector2 Position { get { return position; } set { SetPosition(value); } }
        public float Rotation { get { return rotation; } set { SetRotation(value); } }
        public virtual void SetPosition(Vector2 position)
        {
            this.position = position;
        }

        public virtual void SetRotation(float rotation)
        {
            this.rotation = rotation;
        }


        public virtual void Update(float dt)
        {
            position += velocity * dt;
        }

        public virtual bool Intersect(TestObject other)
        {
            return ShapeTests.Overlap(Shape, other.Shape);
        }

        public virtual void Draw(SpriteBatch spriteBatch, ShapeBatch shapeBatch)
        {
            shapeBatch.DrawShape(Shape, color);
        }
    }
}
