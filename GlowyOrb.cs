using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TutorialMod
{
    public class GlowyOrb : ModProjectile
    {
        private Texture2D sprite;
        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 28;

            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 300;
        }
        //Make the hitbox be a 12x12 circle
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (targetHitbox.Distance(Projectile.Center) < 12)
                return base.Colliding(projHitbox, targetHitbox);
            else return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (sprite == null)
            {
                sprite = ModContent.Request<Texture2D>("TutorialMod/Assets/GlowBloom").Value;
            }

            Vector2 pos = Projectile.Center - Main.screenPosition;
            Vector2 origin = sprite.Size() * 0.5f;
            Color col = new Color(0f, 0.2f, 1f, 0f);
            float scale = 0.5f;

            Main.EntitySpriteDraw(sprite, pos, null, col, 0, origin, scale, SpriteEffects.None);

            return true;
        }
    }
}