using EzGraphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PrimitivesTests.Inputs;
using PrimitivesTests.Primitives;
using System.Collections.Generic;

namespace PrimitivesTests
{
   
    
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private ShapeBatch shapeBatch;
        private List<TestEnemy> enemyList = new();
        private TestPlayer player;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            enemyList.Add(new TestEnemy(new Vector2(100, 100)));
            enemyList.Add(new TestEnemy(new Vector2(200, 200)));
            enemyList.Add(new TestEnemy(new Vector2(200, 100)));
            player = new(new Vector2(300));
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            shapeBatch = new ShapeBatch(GraphicsDevice);

        }

        protected override void Update(GameTime gameTime)
        {
            MouseInput.UpdateStates();
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            player.Update(dt);
            foreach (TestEnemy enemy in enemyList)
            {
                enemy.Update(dt);
            }
            player.Interact(enemyList);
            

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            shapeBatch.Begin();
            foreach (TestEnemy enemy in enemyList)
            {
                enemy.Draw(spriteBatch, shapeBatch);
            }
            player.Draw(spriteBatch, shapeBatch);

            DebugDraw.Draw(shapeBatch);
            spriteBatch.End();
            shapeBatch.End();

            base.Draw(gameTime);
        }

        //public void FillRectangle(SpriteBatch spriteBatch, Vector2 position, Vector2 extents, Color color, float rotation)
        //{
        //    int x = (int)(position.X - extents.X);
        //    int y = (int)(position.Y - extents.Y);
        //    int width = (int)(extents.X + extents.X);
        //    int height = (int)(extents.Y + extents.Y);
        //    spriteBatch.Draw(texture, position, new Rectangle(x, y, width, height), color, rotation, extents, 1f, SpriteEffects.None, 0f);
        //}
        //public void FillCircle(SpriteBatch spriteBatch, Vector2 position, float radius, Color color)
        //{
        //    Rectangle dest = new Rectangle((int)(position.X - radius), (int)(position.Y - radius), (int)(radius + radius), (int)(radius + radius));
        //    spriteBatch.Draw(texCrircle, dest, color);
        //}

        //public Texture2D GenerateCircleTexture(GraphicsDevice graphicsDevice, int radius)
        //{
        //    int diameter = radius * 2 + 1;
        //    Vector2 origin = new Vector2(radius, radius);
        //    Texture2D tex = new Texture2D(graphicsDevice, diameter, diameter);
        //    Color[] colors = new Color[diameter * diameter];

        //    int index = 0;
        //    for (int i = 0; i < diameter; i++)
        //    {
        //        for (int j = 0; j < diameter; j++)
        //        {
        //            float distSq = Vector2.DistanceSquared(origin, new Vector2(i, j));
        //            if (distSq < radius * radius)
        //                colors[index] = Color.White;
        //            else
        //                colors[index] = Color.Transparent;
        //            index++;
        //        }
        //    }
        //    tex.SetData(colors);
        //    texCrircle = tex;
        //    return tex;
        //}

    }
}