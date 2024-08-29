using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TutorialMod
{
    public class ProjectileRingEnemy : ModNPC
    {
        int ProjectileCD;
        float RotationTimer;
        int Timer;
        public override void SetDefaults()
        {
            NPC.lifeMax = 500;
            NPC.dontTakeDamage = true;
            NPC.damage = 0;
            NPC.width = 16;
            NPC.height = 16;
            NPC.noGravity = true;
        }
        public override void AI()
        {
            switch (NPC.ai[1])
            {
                case 0: ShootBasicRing(); break;
                case 1: DustAura(); break;
                case 2: CreateProjectileRingAroundItself(); break;
                case 3: NPC.ai[1] = 0; goto case 0;
            }
            //Swap modes every 3 seconds
            if (++Timer > 240)
            {
                NPC.ai[1]++;
                Timer = 0;
            }

            NPC.TargetClosest();
            RotationTimer += 0.1f;

            void ShootBasicRing()
            {
                int amount = 16;
                //Shoot thrice a second
                if (++ProjectileCD % 15 == 0)
                {
                    for (int i = 0; i < amount; i++)
                    {
                        Vector2 velocity = new Vector2(0, 3).RotatedBy(MathHelper.TwoPi * i / amount + RotationTimer);
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity, ProjectileID.DeathLaser, 50, 1);
                    }
                }
            }
            void CreateProjectileRingAroundItself()
            {
                int amount = 8;
                float radius = (float)(Math.Sin(RotationTimer * 5) * 100f);

                //Shoot every 5 frames
                if (++ProjectileCD % 5 == 0)
                {
                    for (int i = 0; i < amount; i++)
                    {
                        float rot = MathHelper.TwoPi * i / amount + RotationTimer;

                        Vector2 pos = NPC.Center + Vector2.One.RotatedBy(rot) * radius;
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), pos, Vector2.Zero, ProjectileID.DD2DarkMageBolt, 50, 0);
                    }
                }
            }
            void DustAura()
            {
                int amount = 8;
                float radius = (float)(Math.Sin(Main.GlobalTimeWrappedHourly * 2) * 400);

                for (int i = 0; i < amount; i++)
                {
                    float rot = MathHelper.TwoPi * i / amount + (float)Math.Sin(Main.GlobalTimeWrappedHourly * 2) * 5;

                    int d = Dust.NewDust(NPC.Center + Vector2.One.RotatedBy(rot)
                        * radius, 1, 1, DustID.SandSpray, 0, 0, 0, default, 2f);
                    Main.dust[d].velocity = Vector2.Zero;
                }
            }
        }
    }
}