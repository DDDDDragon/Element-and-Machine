using Terraria;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 元素与机械
{
    public static class ModHelper//声明静态类
    {
        /// <summary>
        /// 将世界坐标转换为屏幕坐标
        /// </summary>
        /// <param name="WorldPos">世界坐标</param>
        /// <returns></returns>
        public static Vector2 WorldPos2ScreenPos(this Vector2 WorldPos)
        {
            return WorldPos - Main.screenPosition;
        }
        /// <summary>
        /// 将屏幕坐标转换为世界坐标
        /// </summary>
        /// <param name="ScreenPos">屏幕坐标</param>
        /// <returns></returns>
        public static Vector2 ScreenPos2WorldPos(this Vector2 ScreenPos)
        {
            return ScreenPos + Main.screenPosition;
        }
    }
}
