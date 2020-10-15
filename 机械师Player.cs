using Terraria;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ReLogic.Utilities;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.GameContent.Achievements;
using Terraria.GameContent.Events;
using Terraria.GameContent.Tile_Entities;
using Terraria.GameContent.UI;
using Terraria.GameInput;
using Terraria.Graphics.Capture;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ObjectData;
using Terraria.Social;
using Terraria.UI;
using Terraria.UI.Chat;
using Terraria.UI.Gamepad;
using Terraria.Utilities;
using Terraria.World.Generation;
using Terraria.ModLoader;
using 元素与机械.Buffs;
using 元素与机械;
using 元素与机械.UI;
using 元素与机械.Items;
using Terraria.ModLoader.IO;
using 元素与机械.伤害类型;

namespace 元素与机械
{

    public class 机械师Player : ModPlayer
    {
        #region 地元素伤害
        public float 大地战士伤害Add = 0f;
        public float 大地战士伤害Mult = 1f;
        public float 大地战士Knockback;
        public int 大地战士Crit;
        #endregion
        #region 武器效果
        public int dirtHit=0;
        #endregion
        #region 套装效果
        public bool DirtSuit = false;
        #endregion
        #region 人物基础属性
        public int point = 10;//总点数
        public int power;//力量
        public int wisdom;//智力
        public int luck;//幸运
        public int agile;//敏捷
        public bool AddLast = false;//判定是否保存
        #endregion
        #region 初始化
        public override void SetupStartInventory(IList<Item> items, bool mediumcoreDeath)
        {
            base.SetupStartInventory(items, mediumcoreDeath);
        }
        public override void Initialize()
        {
            point = 10;
            power = 0;
            agile = 0;
            wisdom = 0;
            luck = 0;
            player.ClearBuff(ModContent.BuffType<羸弱>());
            player.ClearBuff(ModContent.BuffType<痴傻>());
            player.ClearBuff(ModContent.BuffType<跛脚>());
            player.ClearBuff(ModContent.BuffType<霉运>());
        }
        public override void LoadLegacy(BinaryReader reader)
        {
            point = reader.ReadInt32();
        }
        public static 机械师Player ModPlayer(Player player)
        {
            return player.GetModPlayer<机械师Player>();
        }
        #endregion
        #region 械力
        public int EnergyMax = 20;
        public int RealEnergy = 20;
        public int EnergyMax2 = 20;
        public int Time = 0;
        public override void PreUpdate()
        {
            if (OpenNoMana == true)
            {
                Time++;
                if (Time == 60)
                {
                    Time = 0;
                    if (RealEnergy < EnergyMax2)
                    {
                        RealEnergy += 1;
                    }
                }
                if (RealEnergy < 0)
                {
                    RealEnergy = 0;
                }
                if (EnergyMax2 > 400)
                {
                    EnergyMax2 = 400;
                }
                if (EnergyMax > 200)
                {
                    EnergyMax = 200;
                }
                if (RealEnergy > EnergyMax2)
                {
                    RealEnergy = EnergyMax2;
                }
                if (IsAccessoryMagicToEnergy == false)
                {
                    EnergyMax2 = 20;
                }
                else EnergyMax2 = 40;
            }
        }
        #endregion
        #region 杂项
        public bool strongBeesUpgrade;
        public bool IsAccessoryMagicToEnergy = false;
        public string worldName = "新大陆";
        public static int miss = 5;
        public static bool OpenNoMana = false;
        #endregion
        #region 闪避设定
        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if(Main.rand.Next(100) <= luck)
            {
                CombatText.NewText(player.getRect(), Color.White, "Miss!");
                return false;
            }
            return true;
        }
        #endregion
        #region 各项基本属性的效果及buff设定,天赋设定
        int iii = 0;
        public override void ResetEffects()
        {
            DirtSuit = false;
            if (AddLast == true)
            {
                if (power >= 4 && power < 10)
                {
                    player.AddBuff(ModContent.BuffType<初级战斗精通>(), 180);
                    player.AddBuff(ModContent.BuffType<初级体质强悍>(), 180);
                }
                player.meleeDamage += 0.01f * power;
                player.rangedDamage += 0.01f * power;
                player.magicDamage += 0.01f * wisdom;
                player.minionDamage += 0.01f * wisdom;
                player.moveSpeed += 0.02f * agile;
                if (power == 0)
                {
                    player.AddBuff(ModContent.BuffType<羸弱>(), 180);
                }
                else
                {
                    player.ClearBuff(ModContent.BuffType<羸弱>());
                }
                if (wisdom == 0)
                {
                    player.AddBuff(ModContent.BuffType<痴傻>(), 180);
                }
                else
                {
                    player.ClearBuff(ModContent.BuffType<痴傻>());
                }
                if (agile == 0)
                {
                    player.AddBuff(ModContent.BuffType<跛脚>(), 180);
                }
                else
                {
                    player.ClearBuff(ModContent.BuffType<跛脚>());
                }
                if (luck == 0)
                {
                    player.AddBuff(ModContent.BuffType<霉运>(), 180);
                }
                else
                {
                    player.ClearBuff(ModContent.BuffType<霉运>());
                }
            }
            ResetVariables();
        }
        private void ResetVariables()
        {
            大地战士伤害Add = 0f;
            大地战士伤害Mult = 1f;
            大地战士Knockback = 0f;
            大地战士Crit = 0;
        }
        #endregion
        #region 保存和载入
        public override TagCompound Save()
        {
             return new TagCompound
             {
                  { "point", point },
                  { "power", power },
                  {"wisdom",wisdom },
                  {"luck",luck },
                  {"agile",agile },
                  {"AddLast",AddLast }
             };
        }
        public override void Load(TagCompound tag)
        {
            AddLast = tag.GetBool("AddLast");
            point = tag.GetInt("point");
            power = tag.GetInt("power");
            wisdom = tag.GetInt("wisdom");
            luck = tag.GetInt("luck");
            agile = tag.GetInt("agile");   
        }
        #endregion
        #region 进入世界时
        public override void OnEnterWorld(Player player)
        {
            ExampleUI.Visible = true;
        }
        #endregion
    }
}

