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
        public float rot;
        public Vector2 Center;
        private Texture2D sprite;
        public override string Texture => "TutorialMod/Assets/BloomLine2";
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;

            Projectile.aiStyle = -1;
            Projectile.scale = 0;

            Projectile.tileCollide = false;
            Projectile.timeLeft = 40;

            Projectile.penetrate = -1;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
        }
        public override void AI()
        {
            if (Projectile.timeLeft > 10)
                Projectile.scale += 0.4f;

            if (Projectile.timeLeft < 20 && Projectile.scale > 0)
                Projectile.scale -= 0.4f;
        }
        public override void OnSpawn(IEntitySource source)
        {
            Center = Projectile.Center;
            rot = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Projectile.velocity = Vector2.Zero;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Vector2 endPos = Projectile.position + Vector2.One.RotatedBy(rot + MathHelper.PiOver4 + MathHelper.Pi) * 2000;
            float point = float.NaN;

            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.position, endPos, Projectile.scale, ref point) && Projectile.timeLeft < 30)
                return true;
            else return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 2000;
            SpriteBatch sb = Main.spriteBatch;

            if (sprite == null)
                sprite = ModContent.Request<Texture2D>(Texture).Value;

            sb.End();
            sb.Begin(default, BlendState.NonPremultiplied, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            sb.Draw(sprite, new Rectangle(
                (int)(Projectile.Center.X - Main.screenPosition.X), (int)(Projectile.Center.Y - Main.screenPosition.Y), (int)(10 * Projectile.scale), 5000), null,
                Color.DeepSkyBlue, rot, sprite.Size() * 0.5f, SpriteEffects.None, 0);

            sb.Draw(sprite, new Rectangle(
                (int)(Projectile.Center.X - Main.screenPosition.X), (int)(Projectile.Center.Y - Main.screenPosition.Y), (int)(5 * Projectile.scale), 5000), null,
                Color.White, rot, sprite.Size() * 0.5f, SpriteEffects.None, 0);

            sb.End();
            sb.Begin(default, BlendState.AlphaBlend, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
    }
}