using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PluginLoader;
using Terraria;

namespace SeptorPlugins
{
    public class NoClip : MarshalByRefObject, IPluginPlayerUpdate, IPluginChatCommand
    {
        private bool noclip = false;
        private Keys noclipKey;
        private int immuneTime;

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

        public void OnPlayerUpdate(Player player)
        {
            player = Main.player[Main.myPlayer];
            immuneTime = player.immuneTime;

            if (noclip)
            {
                float magnitude = 6f;

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

                player.fallStart = (int)player.position.Y;
                player.immune = true;
                player.immuneTime = 1000;
            }
            else
            {
                player.immuneTime = immuneTime;
            }
        }

        public bool OnChatCommand(string command, string[] args)
        {
            if (command != "noclip") return false;
            noclip = !noclip;
            return true;
        }
    }
}