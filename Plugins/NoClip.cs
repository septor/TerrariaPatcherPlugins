using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PluginLoader;
using Terraria;

namespace SeptorPlugins
{
    public class NoClip : MarshalByRefObject, IPluginUpdate
    {
        private bool noclip = false;
        private Keys noclipKey;
        
        public NoClip()
        {
            if (!Keys.TryParse(IniAPI.ReadIni("NoClip", "NoclipKey", "Q", writeIt: true), out noclipKey))
                noclipKey = Keys.Q;

            Color green = Color.Green;
            Loader.RegisterHotkey(() =>
            {
                noclip = !noclip;
                Main.NewText("NoClip " + (noclip ? "Enabled" : "Disabled"), green.R, green.G, green.B, false);
            }, noclipKey);
        }

        public void OnUpdate()
        {
            var player = Main.player[Main.myPlayer];

            if (noclip)
            {
                float magnitude = 6f;

                player.fallStart = (int)player.position.Y;
                player.immune = true;
                player.immuneTime = 1000;

                if (player.controlUp || player.controlJump)
                {
                    player.position = new Vector2(player.position.X, player.position.Y - magnitude);
                }
                if (player.controlDown)
                {
                    player.position = new Vector2(player.position.X, player.position.Y + magnitude);
                }
                if (player.controlLeft)
                {
                    player.position = new Vector2(player.position.X - magnitude, player.position.Y);
                }
                if (player.controlRight)
                {
                    player.position = new Vector2(player.position.X + magnitude, player.position.Y);
                }
            }
            else
            {
                player.immune = false;
            }
        }
    }
}