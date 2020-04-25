using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade;
using HarmonyLib;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.InputSystem;

namespace BannerPaste
{
    public class BannerPasteModule : MBSubModuleBase
    {
        public static BannerPasteModule Instance;

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            try
            {
                Instance = this;
                var harmony = new Harmony("BannerPaste stuff");
                harmony.PatchAll();
            }
            catch (Exception ex)
            {
                DelayMessage("BannerPaste encountered an error while initializing, details copied to clipboard.", Color.FromUint(0xffff0000));
                Input.SetClipboardText(FormatException(ex));
            }
        }

        public static string FormatException(Exception ex)
        {
            return $"{ex.GetType().Name}: {ex.Message}\r\n{ex.StackTrace}" + (ex.InnerException != null ? "\r\n" + FormatException(ex.InnerException) : "");
        }

        protected override void OnSubModuleUnloaded()
        {
            Instance = null;
        }

        protected static void DisplayMessage(string message, Color? color = null)
        {
            InformationManager.DisplayMessage(new InformationMessage(message, color ?? Color.White));
        }

        static bool initialized = false;
        static List<(string, Color)> messages = new List<(string, Color)>();
        public static void DelayMessage(string message, Color? color = null)
        {
            if (initialized)
                DisplayMessage(message, color);
            else
                messages.Add((message, color ?? Color.White));
        }

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            initialized = true;
            foreach (var (message, color) in messages)
            {
                DisplayMessage(message, color);
            }
            messages.Clear();
        }
    }
}
