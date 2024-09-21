using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TutorialMod
{
    public class ProjectileRingEnemy : ModNPC
    {
        int ProjectileCD;
        int ProjectileCD2;
        float RotationTimer;
        int Timer;
        private Texture2D sprite;

        public override void SetDefaults()
        {
            NPC.lifeMax = 500;
            NPC.dontTakeDamage = true;
            NPC.damage = 0;
            NPC.width = 16;
            NPC.height = 16;
            NPC.noGravity = true;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (sprite == null)
            {
                sprite = ModContent.Request<Texture2D>("TutorialMod/ProjectileRingEnemy_Glow").Value;
            }
            Vector2 position = NPC.Center - new Vector2(0, 4) - Main.screenPosition;
            Rectangle frame = NPC.frame;
            Vector2 origin = frame.Size() * 0.5f;
            Color color = Main.DiscoColor;
            float rotation = NPC.rotation;
            SpriteEffects se = NPC.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            Main.EntitySpriteDraw(sprite, position, frame, color, rotation, origin, NPC.scale, se);
        }
        public override void AI()
        {
            /*switch (NPC.ai[1])
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
            }*/
            //ShootBasicRing();

            //CreateProjectileRingAroundItself();
            NPC.TargetClosest();
            RotationTimer += 0.2f;
            if (++ProjectileCD2 % 90 == 0)
            {
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.DirectionTo(Main.player[NPC.target].Center) * 7, ModContent.ProjectileType<ExampleDeathray>(), 50, 1f);
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.DirectionTo(Main.player[NPC.target].Center).RotatedBy(MathHelper.PiOver4 / 2) * 7, ModContent.ProjectileType<ExampleDeathrayEXTEND>(), 50, 1f);
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.DirectionTo(Main.player[NPC.target].Center).RotatedBy(MathHelper.PiOver4 / -2) * 7, ModContent.ProjectileType<ExampleDeathrayEXTEND>(), 50, 1f);
            }

            void ShootBasicRing()
            {
                int amount = 4;
                //Shoot thrice a second
                if (++ProjectileCD % 150 == 0)
                {
                    for (int i = 0; i < amount; i++)
                    {
                        Vector2 velocity = new Vector2(0, 12).RotatedBy(MathHelper.TwoPi * i / amount);
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity, ModContent.ProjectileType<ExampleDeathray>(), 50, 1);
                    }
                }
                /*if (++ProjectileCD2 < 30 && ++ProjectileCD % 5 == 0)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        float rot = MathHelper.TwoPi * i / 5 - RotationTimer;
                        Vector2 pos = NPC.Center + Vector2.One.RotatedBy(rot) * (float)(Math.Sin(ProjectileCD / 2f) * 50);
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), pos, pos.DirectionTo(NPC.Center) * -3, ModContent.ProjectileType<GlowyOrb>(), 50, 1);
                    }
                }*/
                //if (ProjectileCD2 > 60) ProjectileCD2 = 0;
            }
            void CreateProjectileRingAroundItself()
            {
                int amount = 8;
                float radius = (float)(Math.Sin(RotationTimer * 5) * 100f);

                //Shoot every 5 frames
                if (++ProjectileCD % 125 == 0)
                {
                    for (int i = 0; i < amount; i++)
                    {
                        float rot = MathHelper.TwoPi * i / amount + RotationTimer;

                        Vector2 pos = NPC.Center + Vector2.One.RotatedBy(rot) * 200;
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), pos, pos.DirectionTo(NPC.Center) * 1, ModContent.ProjectileType<ExampleDeathrayEXTEND>(), 50, 0);
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