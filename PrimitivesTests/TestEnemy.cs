using Microsoft.Xna.Framework;
using MissileMath;
using PrimitivesTests.Primitives;

namespace PrimitivesTests
{
    public class TestEnemy : TestObject
    {
        public override Shape Shape => hull;

        private Hull hull;
        private float rotationSpeed;
        private Circle range;

        public TestEnemy(Vector2 startPos)
        {
            Vector2[] model = new Vector2[4];
            model[0] = new Vector2(-30, -30);
            model[1] = new Vector2(30, -30);
            model[2] = new Vector2(30, 30);
            model[3] = new Vector2(-30, 30);
            hull = new Hull(model);
            range = new Circle(startPos, 50);
            Position = startPos;
            color = Color.Red;
            rotationSpeed = RandomUtils.RandomFloat(-3.5f, 3.5f);
            Rotation = rotationSpeed;
        }

        public override void Update(float dt)
        {
            Rotation += rotationSpeed * dt;
            base.Update(dt);
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
    }
}
