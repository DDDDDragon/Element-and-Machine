using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using 元素与机械.Prefix;
using 元素与机械.Items;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace 元素与机械
{
    public class MyGlobalItem : GlobalItem
    {
        public byte 大地;
        public MyGlobalItem()
        {
            大地 = 0;
        }
        public override bool InstancePerEntity => true;
        public override GlobalItem Clone(Item item, Item itemClone)
        {
            MyGlobalItem myClone = (MyGlobalItem)base.Clone(item, itemClone);
            myClone.大地 = 大地;
            return myClone;
        }
        public override void OnHitNPC(Item item, Player player, NPC target, int damage, float knockBack, bool crit)
        {
        }
        public override void Update(Item item, ref float gravity, ref float maxFallSpeed)
        {
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (!item.social && item.prefix > 0)
            {
                if (item.prefix == ModContent.PrefixType<大地属性>())
                {
                    TooltipLine line = new TooltipLine(mod, "大地", "使地元素武器增加25%的元素伤害\n对于每件地元素武器特殊增幅不同\n普通武器增加25%的伤害")
                    {
                        isModifier = true
                    };
                    tooltips.Add(line);
                }
            }
        }
    }
}
