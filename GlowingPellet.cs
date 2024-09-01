using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TutorialMod
{
    public class GlowingPellet : ModProjectile
    {
        private Texture2D sprite;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 20;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 22;

            Projectile.tileCollide = false;
            Projectile.timeLeft = 300;

            Projectile.hostile = true;
            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;
            ProjectileID.Sets.TrailCacheLength[Type] = 10;
        }
        //Make the hitbox be a 8x8 square instead of 16x22 rectangle
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (targetHitbox.Intersects(new Rectangle((int)(Projectile.Center.X - 4), (int)(Projectile.Center.Y - 4), 8, 8)))
                return base.Colliding(projHitbox, targetHitbox);
            else return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (sprite == null)
            {
                sprite = ModContent.Request<Texture2D>(Texture).Value;
            }

            int lenght = Projectile.oldPos.Length;
            Vector2 origin = sprite.Size() * 0.5f;

            for (int i = 0; i < lenght; i++)
            {
                Color col = Color.Gray * ((lenght - i) / (float)lenght);
                Vector2 pos = Projectile.oldPos[i] + (Projectile.Size * 0.5f) - Main.screenPosition;
                Main.EntitySpriteDraw(sprite, pos, null, col, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None);
            }

            return base.PreDraw(ref lightColor);
        }
    }
}