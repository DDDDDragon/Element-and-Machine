using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;
using 元素与机械.UI;
using 元素与机械;
using Steamworks;

namespace 元素与机械
{
	public class 元素与机械 : Mod
	{
        Mod desolation = ModLoader.GetMod("Desolation");
        public bool Desolation = false;
        Mod depravityMod = ModLoader.GetMod("DepravityMod");
        public bool Depravity = false;
        public static bool FightForLife = false;
        public override void PostUpdateEverything()
        {
            if(desolation != null)
            {
                Desolation = true;
            }
            if (depravityMod != null)
            {
                Depravity = true;
            }
            else Depravity = false;
            base.PostUpdateEverything();
        }
        internal ExampleUI exampleUI;
        internal UserInterface exampleUserInterface;
        public ModHotKey Key;
        public ModHotKey Key2;
        public static MachineSlot machineSlot;
        private static 元素与机械 instance;
        public 元素与机械()
        {
        }
        public override void Load()
        {
            exampleUI = new ExampleUI();
            exampleUI.Activate();
            exampleUserInterface = new UserInterface();
            exampleUserInterface.SetState(exampleUI);
            Key = RegisterHotKey("Key", "Y");
            Key2 = RegisterHotKey("Key2", "U");
        }
        public override void UpdateUI(GameTime gameTime)
        {
            machineSlot.Update();
            if (ExampleUI.Visible == true)
            {
                exampleUserInterface?.Update(gameTime);
            }

            base.UpdateUI(gameTime);
        }
        public override void Unload()
        {
            Key = null;
            instance = null;
            base.Unload();
        }
        public static 元素与机械 Instance
        {
            get;
        }
        public override void PostSetupContent()
        {
            machineSlot = new MachineSlot(new Vector2(Main.screenWidth, Main.screenHeight) / 2, ModContent.GetTexture("元素与机械/UI/Images/MachineSlot"));
            base.PostSetupContent();
        }
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            //寻找一个名字为Vanilla: Mouse Text的绘制层，也就是绘制鼠标字体的那一层，并且返回那一层的索引
            int MouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            //寻找到索引时
            if (MouseTextIndex != -1)
            {
                //往绘制层集合插入一个成员，第一个参数是插入的地方的索引，第二个参数是绘制层
                layers.Insert(MouseTextIndex, new LegacyGameInterfaceLayer(
                   //这里是绘制层的名字
                   "Machine : ExampleUI",
                   //这里是匿名方法
                   delegate
                   {
               //当Visible开启时（当UI开启时）
               if (ExampleUI.Visible)
                   //绘制UI（运行exampleUI的Draw方法）
                   exampleUI.Draw(Main.spriteBatch);
                       return true;
                   },
                   //这里是绘制层的类型
                   InterfaceScaleType.UI)
               );
            }
            base.ModifyInterfaceLayers(layers);
        }
        public override void PostDrawInterface(SpriteBatch spriteBatch)/*将int转换为string有两种办法*/
        {
            if (机械师Player.OpenNoMana == true)
            {
                Utils.DrawBorderStringFourWay(spriteBatch, Main.fontMouseText, "你的械力为"+Main.LocalPlayer.GetModPlayer<机械师Player>().RealEnergy.ToString()/*你这里没有把int转化为string*/, Main.LocalPlayer.Center.WorldPos2ScreenPos().X, Main.LocalPlayer.Center.WorldPos2ScreenPos().Y, Color.White, Color.Black, Vector2.Zero);
                //还有一种是$"{Main.LocalPlayer.GetModPlayer<机械师Player>().RealEnergy}"s
            }
            base.PostDrawInterface(spriteBatch);
        }
        public override void HotKeyPressed(string name)
        {
            if (name == "Key")
            {
                Main.NewText(Main.LocalPlayer.GetModPlayer<机械师Player>().DirtSuit);
            }
            if (name == "Key2")
            {
            }
                base.HotKeyPressed(name);
        }
    }
}