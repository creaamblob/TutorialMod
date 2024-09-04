using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TutorialMod
{
    public class BigOrb : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 256;
            Projectile.height = 256;

            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 150;
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (targetHitbox.Distance(Projectile.Center) < 128)
                return true;
            else return false;
        }
        public override void AI()
        {
            //Projectile.scale += 0.01f;
        }
    }
}