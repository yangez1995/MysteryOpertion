using MysteryOpertion.Model;
using Hazel;
using Il2CppSystem.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using UnhollowerBaseLib;
using UnityEngine;

namespace MysteryOpertion
{
    public static class ToolBox
    {
        internal delegate bool d_LoadImage(IntPtr tex, IntPtr data, bool markNonReadable);
        internal static d_LoadImage iCall_LoadImage;
        public static System.Random random = new System.Random((int)DateTime.Now.Ticks);

        public static PlayerControl GetPlayerControlById(byte id)
        {
            foreach (PlayerControl player in PlayerControl.AllPlayerControls)
                if (player.PlayerId == id)
                    return player;
            return null;
        }

        public static Sprite loadSpriteFromResources(string path, float pixelsPerUnit)
        {
            try
            {
                Texture2D texture = loadTextureFromResources(path);
                return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), pixelsPerUnit);
            }
            catch
            {
                Debug.Log("Error loading sprite from path: " + path);
            }
            return null;
        }

        public static Texture2D loadTextureFromResources(string path)
        {
            try
            {
                Texture2D texture = new Texture2D(2, 2, TextureFormat.ARGB32, true);
                Assembly assembly = Assembly.GetExecutingAssembly();
                
                Stream stream = assembly.GetManifestResourceStream(path);
                var byteTexture = new byte[stream.Length];
                var read = stream.Read(byteTexture, 0, (int)stream.Length);
                LoadImage(texture, byteTexture, false);
                return texture;
            }
            catch
            {
                System.Console.WriteLine("Error loading texture from resources: " + path);
            }
            return null;
        }

        private static bool LoadImage(Texture2D tex, byte[] data, bool markNonReadable)
        {
            if (iCall_LoadImage == null)
                iCall_LoadImage = IL2CPP.ResolveICall<d_LoadImage>("UnityEngine.ImageConversion::LoadImage");
            var il2cppArray = (Il2CppStructArray<byte>)data;
            return iCall_LoadImage.Invoke(tex.Pointer, il2cppArray.Pointer, markNonReadable);
        }

        public static void setLook(this PlayerControl target, String playerName, int colorId, string hatId, string visorId, string skinId, string petId) {
            target.RawSetColor(colorId);
            target.RawSetVisor(visorId);
            target.RawSetHat(hatId, colorId);
            target.RawSetName(hidePlayerName(PlayerControl.LocalPlayer, target) ? "" : playerName);

            SkinViewData nextSkin = DestroyableSingleton<HatManager>.Instance.GetSkinById(skinId).viewData.viewData;
            PlayerPhysics playerPhysics = target.MyPhysics;
            AnimationClip clip = null;
            var spriteAnim = playerPhysics.Skin.animator;
            var currentPhysicsAnim = playerPhysics.Animator.GetCurrentAnimation();
            if (currentPhysicsAnim == playerPhysics.CurrentAnimationGroup.RunAnim) clip = nextSkin.RunAnim;
            else if (currentPhysicsAnim == playerPhysics.CurrentAnimationGroup.SpawnAnim) clip = nextSkin.SpawnAnim;
            else if (currentPhysicsAnim == playerPhysics.CurrentAnimationGroup.EnterVentAnim) clip = nextSkin.EnterVentAnim;
            else if (currentPhysicsAnim == playerPhysics.CurrentAnimationGroup.ExitVentAnim) clip = nextSkin.ExitVentAnim;
            else if (currentPhysicsAnim == playerPhysics.CurrentAnimationGroup.IdleAnim) clip = nextSkin.IdleAnim;
            else clip = nextSkin.IdleAnim;
            float progress = playerPhysics.Animator.m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            playerPhysics.Skin.skin = nextSkin;
            spriteAnim.Play(clip, 1f);
            spriteAnim.m_animator.Play("a", 0, progress % 1);
            spriteAnim.m_animator.Update(0f);

            if (target.CurrentPet) UnityEngine.Object.Destroy(target.CurrentPet.gameObject);
            target.CurrentPet = UnityEngine.Object.Instantiate<PetBehaviour>(DestroyableSingleton<HatManager>.Instance.GetPetById(petId).viewData.viewData);
            target.CurrentPet.transform.position = target.transform.position;
            target.CurrentPet.Source = target;
            target.CurrentPet.Visible = target.Visible;
            PlayerControl.SetPlayerMaterialColors(colorId, target.CurrentPet.rend);
        }

        public static bool hidePlayerName(PlayerControl source, PlayerControl target)
        {
            if (source == null || target == null) return true;
            else if (source == target) return false; // Player sees his own name
            else if (source.Data.Role.IsImpostor && (target.Data.Role.IsImpostor)) return false; // Members of team Impostors see the names of Impostors/Spies

            return true;
        }

        //设置玩家图标亮灭
        public static void setSemiTransparent(this PoolablePlayer player, bool value)
        {
            float alpha = value ? 0.25f : 1f;
            foreach (SpriteRenderer r in player.gameObject.GetComponentsInChildren<SpriteRenderer>())
                r.color = new Color(r.color.r, r.color.g, r.color.b, alpha);
            player.NameText.color = new Color(player.NameText.color.r, player.NameText.color.g, player.NameText.color.b, alpha);
        }

        public static void ReSetPlayerTask(byte playerId, int numCommon, int numShort, int numLong)
        {
            Il2CppSystem.Collections.Generic.List<byte> tasks = GenerateTasks(numCommon, numShort, numLong);
            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)RPCFuncType.ReSetTasks, Hazel.SendOption.Reliable);
            writer.Write(playerId);
            writer.WriteBytesAndSize((Il2CppStructArray<byte>)tasks.ToArray());
            RPCFunctions.ReSetTasks(playerId, tasks.ToArray());
        }

        //根据长中短任务数生成任务列表
        public static Il2CppSystem.Collections.Generic.List<byte> GenerateTasks(int numCommon, int numShort, int numLong)
        {
            var list = new Il2CppSystem.Collections.Generic.List<byte>();

            int sum = numCommon + numShort + numLong;
            if (sum <= 0) return list;
            
            Il2CppSystem.Collections.Generic.List<NormalPlayerTask> tasks = new Il2CppSystem.Collections.Generic.List<NormalPlayerTask>();

            int commonTaskCount = ShipStatus.Instance.CommonTasks.Count;
            List<int> randomList = GetRandomNumberListUnrepeat(0, commonTaskCount, numCommon);
            foreach (var index in randomList)
            {
                tasks.Add(ShipStatus.Instance.CommonTasks[index]);
            }

            int normalTaskCount = ShipStatus.Instance.NormalTasks.Count;
            randomList = GetRandomNumberListUnrepeat(0, normalTaskCount, numShort);
            foreach (var index in randomList)
            {
                tasks.Add(ShipStatus.Instance.NormalTasks[index]);
            }

            int longTaskCount = ShipStatus.Instance.LongTasks.Count;
            randomList = GetRandomNumberListUnrepeat(0, longTaskCount, numLong);
            foreach (var index in randomList)
            {
                tasks.Add(ShipStatus.Instance.LongTasks[index]);
            }

            int num = 0;
            ShipStatus.Instance.AddTasksFromList(ref num, sum, list, new Il2CppSystem.Collections.Generic.HashSet<TaskTypes>(), tasks);

            return list;
        }

        public static List<int> GetRandomNumberListUnrepeat(int min, int max, int count)
        {
            List<int> list = new List<int>();
            bool[] flags = new bool[max - min];

            for (int i = 0; i < count; i++)
            {
                int num;
                do
                {
                    num = random.Next(min, max);
                } while(flags[num - min]);
                list.Add(num);
                flags[num - min] = true;
            }

            return list;
        }

        //清空玩家任务
        public static void clearAllTasks(this PlayerControl player)
        {
            if (player == null) return;
            for (int i = 0; i < player.myTasks.Count; i++)
            {
                PlayerTask playerTask = player.myTasks[i];
                playerTask.OnRemove();
                UnityEngine.Object.Destroy(playerTask.gameObject);
            }
            player.myTasks.Clear();

            if (player.Data != null && player.Data.Tasks != null)
                player.Data.Tasks.Clear();
        }

        //设置任务栏角色描述颜色
        public static string FormatRoleTaskText(Color color, string text)
        {
            return string.Format("<color=#{0:X2}{1:X2}{2:X2}{3:X2}>{4}</color>", ToByte(color.r), ToByte(color.g), ToByte(color.b), ToByte(color.a), text);
        }
        private static byte ToByte(float f)
        {
            f = Mathf.Clamp01(f);
            return (byte)(f * 255);
        }

        //屏幕闪光
        public static void showFlash(Color color, float duration = 1f)
        {
            if (HudManager.Instance == null || HudManager.Instance.FullScreen == null) return;
            HudManager.Instance.FullScreen.gameObject.SetActive(true);
            HudManager.Instance.FullScreen.enabled = true;
            HudManager.Instance.StartCoroutine(Effects.Lerp(duration, new Action<float>((p) => {
                var renderer = HudManager.Instance.FullScreen;

                if (p < 0.5)
                {
                    if (renderer != null)
                        renderer.color = new Color(color.r, color.g, color.b, Mathf.Clamp01(p * 2 * 0.75f));
                }
                else
                {
                    if (renderer != null)
                        renderer.color = new Color(color.r, color.g, color.b, Mathf.Clamp01((1 - p) * 2 * 0.75f));
                }
                if (p == 1f && renderer != null) renderer.enabled = false;
            })));
        }
    }
}
