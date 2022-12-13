using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PrimitivesTests.Primitives;

namespace PrimitivesTests
{
   
    
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D texture;
        private Texture2D texCrircle;

        private OBB obb;    // oriented bounding box
        private AABB aabb; //axis-aligned bounding box

        private AABB aabb2; //axis-aligned bounding box

        private Circle circle;
        private Color color;

        private float rotation;
        private bool overlapObb;
        private bool overlapAabb;
        private bool overlapAabbAabb;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            obb = new OBB(Vector2.One * 200, new Vector2(24, 36));
            aabb = new AABB(new Vector2(400, 200), new Vector2(50, 30));
            circle = new Circle(Vector2.Zero, 24);

            aabb2 = new AABB(Vector2.Zero, new Vector2(30, 25));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            texture = new Texture2D(GraphicsDevice, 1, 1);
            Color[] colors = new Color[1] { Color.White };
            texture.SetData(colors);
            GenerateCircleTexture(GraphicsDevice, 64);
        }

        protected override void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            circle.center = Mouse.GetState().Position.ToVector2();
            aabb2.center = circle.center + new Vector2(0, 150);
            rotation = MathHelper.WrapAngle(rotation + dt);

            obb.Rotate(rotation);

            overlapObb = ShapeTests.Overlap(circle, obb);
            overlapAabb = ShapeTests.Overlap(circle, aabb);
            color = overlapObb ? Color.Red : overlapAabb ? Color.Blue: Color.White;

            overlapAabbAabb = ShapeTests.Overlap(aabb2, aabb);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();   
            FillRectangle(_spriteBatch, obb.center, obb.extents, overlapObb ? Color.Red : Color.White, rotation);
            FillCircle(_spriteBatch, circle.center, circle.radius, color);
            FillRectangle(_spriteBatch, aabb.center, aabb.extents, overlapAabb ? Color.Blue : Color.White, 0);
            FillRectangle(_spriteBatch, aabb2.center, aabb2.extents, overlapAabbAabb ? Color.Green : Color.White, 0);
            _spriteBatch.End(); 
            base.Draw(gameTime);
        }

        public void FillRectangle(SpriteBatch spriteBatch, Vector2 position, Vector2 extents, Color color, float rotation)
        {
            int x = (int)(position.X - extents.X);
            int y = (int)(position.Y - extents.Y);
            int width = (int)(extents.X + extents.X);
            int height = (int)(extents.Y + extents.Y);
            spriteBatch.Draw(texture, position, new Rectangle(x, y, width, height), color, rotation, extents, 1f, SpriteEffects.None, 0f);
        }
        public void FillCircle(SpriteBatch spriteBatch, Vector2 position, float radius, Color color)
        {
            Rectangle dest = new Rectangle((int)(position.X - radius), (int)(position.Y - radius), (int)(radius + radius), (int)(radius + radius));
            spriteBatch.Draw(texCrircle, dest, color);
        }

        public Texture2D GenerateCircleTexture(GraphicsDevice graphicsDevice, int radius)
        {
            int diameter = radius * 2 + 1;
            Vector2 origin = new Vector2(radius, radius);
            Texture2D tex = new Texture2D(graphicsDevice, diameter, diameter);
            Color[] colors = new Color[diameter * diameter];

            int index = 0;
            for (int i = 0; i < diameter; i++)
            {
                for (int j = 0; j < diameter; j++)
                {
                    float distSq = Vector2.DistanceSquared(origin, new Vector2(i, j));
                    if (distSq < radius * radius)
                        colors[index] = Color.White;
                    else
                        colors[index] = Color.Transparent;
                    index++;
                }
            }
            tex.SetData(colors);
            texCrircle = tex;
            return tex;
        }

    }
}