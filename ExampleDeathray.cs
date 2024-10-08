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
    public class ExampleDeathray : ModProjectile
    {
        public float startRot;
        public Vector2 startPos;
        private Texture2D sprite;
        private Texture2D sprite2;
        public override string Texture => "TutorialMod/Assets/Deathray";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 2000;
        }
        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;

            Projectile.aiStyle = -1;
            Projectile.scale = 0.2f;

            Projectile.tileCollide = false;
            Projectile.timeLeft = 70;

            Projectile.penetrate = -1;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
        }
        public override void AI()
        {
            if (Projectile.timeLeft > 50 && Projectile.timeLeft < 70)
                Projectile.scale += 0.05f;

            if (Projectile.timeLeft < 10 && Projectile.scale > 0)
                Projectile.scale -= 0.1f;
        }
        public override void OnSpawn(IEntitySource source)
        {
            startPos = Projectile.Center;
            startRot = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Projectile.velocity = Vector2.Zero;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            //Collision ends after 2000 pixels
            Vector2 endPoint = startPos + Vector2.One.RotatedBy(startRot + MathHelper.PiOver4 + MathHelper.Pi) * 2000;
            float point = float.NaN;

            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), startPos, endPoint, 15, ref point) && Projectile.timeLeft < 50)
            {
                return true;
            }
            else return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (sprite == null)
                sprite = ModContent.Request<Texture2D>(Texture).Value;
            if (sprite2 == null)
                sprite2 = ModContent.Request<Texture2D>(Texture + "_Extension").Value;

            Vector2 pos = startPos - Main.screenPosition;
            int headWidth = (int)(28 * 4 * Projectile.scale);
            int bodyWidth = (int)(24 * 4 * Projectile.scale);
            int height = sprite2.Height * 4;
            float rotation = startRot;
            SpriteBatch spriteBatch = Main.spriteBatch;
            Vector2 origin = sprite.Size() * 0.5f;
            int amount = 40;
            Vector2 dir = Vector2.One.RotatedBy(startRot + MathHelper.PiOver4 + MathHelper.Pi);

            for (float f = 0; f < amount; f++)
            {
                Vector2 pos2 = Vector2.Lerp(pos + dir * 80, pos + dir * 80 + dir * height, f * 0.67f);
                int x = (int)pos2.X;
                int y = (int)pos2.Y;
                spriteBatch.Draw(sprite2, new Rectangle(x, y, bodyWidth, height), null, Color.White, rotation, sprite2.Size() * 0.5f, SpriteEffects.None, 0);
            }
            int x2 = (int)pos.X;
            int y2 = (int)pos.Y;
            spriteBatch.Draw(sprite, new Rectangle(x2, y2, headWidth, sprite.Height * 4), null, Color.White, rotation, origin, SpriteEffects.FlipVertically, 0);

            return false;
        }
    }
}