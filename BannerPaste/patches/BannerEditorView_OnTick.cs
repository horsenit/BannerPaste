using HarmonyLib;
using SandBox.GauntletUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View;

namespace BannerPaste.patches
{
    [HarmonyPatch(typeof(BannerEditorView), "OnTick")]
    class BannerEditorView_OnTick
    {
        static void Postfix(BannerEditorView __instance)
        {
            if (__instance.SceneLayer.Input.IsHotKeyPressed("Copy") || __instance.GauntletLayer.Input.IsHotKeyPressed("Copy"))
            {
                try
                {
                    Input.SetClipboardText(__instance.DataSource.BannerVM.Banner.Serialize());
                }
                catch (Exception ex)
                {
                    Log.write("Error copying banner code");
                    InformationManager.DisplayMessage(new InformationMessage("Error copying banner code", new Color(1f, 0, 0)));
                    Log.write(ex);
                }
            }
            else if (__instance.SceneLayer.Input.IsHotKeyPressed("Paste") || __instance.GauntletLayer.Input.IsHotKeyPressed("Paste"))
            {
                try
                {
                    // test banner deserialization first
                    var bannerCode = Input.GetClipboardText();
                    var banner = new Banner(bannerCode);
                    // then try to draw it
                    banner.ConvertToMultiMesh();
                    // hopefully we have thrown by now if we're gonna
                    __instance.DataSource.BannerVM.BannerCode = bannerCode;
                    Traverse.Create(__instance).Method("RefreshShieldAndCharacter").GetValue();
                }
                catch (Exception ex)
                {
                    Log.write("Error deserializing banner code");
                    InformationManager.DisplayMessage(new InformationMessage($"Error pasting banner code", new Color(1f, 0, 0)));
                    Log.write(ex);
                }
            }
        }
    }
}
