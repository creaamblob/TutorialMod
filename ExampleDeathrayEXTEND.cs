using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Security.Cryptography.Xml;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace TutorialMod
{
    public class ExampleDeathrayEXTEND : ModProjectile
    {
        public float startRot;
        public Vector2 startPos;
        private Texture2D sprite;
        public override string Texture => "TutorialMod/Assets/Stretched_Deathray";
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;

            Projectile.aiStyle = -1;
            Projectile.scale = 0.2f;

            Projectile.tileCollide = false;
            Projectile.timeLeft = 90;

            Projectile.penetrate = -1;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
        }
        public override void AI()
        {
            //20 frame delay before becoming big, Growing lasts for 20 frames
            if (Projectile.timeLeft < 70 && Projectile.timeLeft > 50)
            {
                Projectile.scale += 0.4f;
            }
            //Shrink on the last 20 frames
            if (Projectile.timeLeft < 20 && Projectile.scale > 0)
                Projectile.scale -= 0.4f;
        }
        public override void OnSpawn(IEntitySource source)
        {
            startRot = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            startPos = Projectile.Center;
            Projectile.velocity = Vector2.Zero;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            //Collision ends after 1000 pixels
            Vector2 endPoint = startPos + Vector2.One.RotatedBy(startRot + MathHelper.PiOver4 + MathHelper.Pi) * 2000;
            float point = float.NaN;

            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), startPos, endPoint, 15, ref point) && Projectile.timeLeft < 60)
            {
                return true;
            }
            else return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (sprite == null)
            {
                sprite = ModContent.Request<Texture2D>(Texture).Value;
            }
            Vector2 pos = startPos - Main.screenPosition;
            int width = (int)(25 * Projectile.scale);
            int height = 3200;
            float rotation = startRot;
            SpriteBatch spriteBatch = Main.spriteBatch;
            Vector2 origin = sprite.Size() * 0.5f;

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.Additive, default, default,default, null, Main.GameViewMatrix.TransformationMatrix);

            spriteBatch.Draw(sprite, new Rectangle((int)pos.X, (int)pos.Y, width, height), null, Color.Red, rotation, origin, SpriteEffects.None, 0);
            spriteBatch.Draw(sprite, new Rectangle((int)pos.X, (int)pos.Y, (int)(width * 0.7f), height), null, Color.Yellow, rotation, origin, SpriteEffects.None, 0);

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.AlphaBlend, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
    }
}