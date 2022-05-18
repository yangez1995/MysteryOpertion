using MysteryOpertion.Model;
using MysteryOpertion.Model.Roles;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MysteryOpertion
{
    public static class RoleInfoDictionary
    {
        public static readonly RoleInfo Crewmate = new RoleInfo { Name = "船员", Blurb = "白板瑟瑟发抖", Target = "完成任务 找出内鬼", Color = Color.white };
        public static readonly RoleInfo Sheriff = new RoleInfo { Name = "治安官", Blurb = "毙掉内鬼", Target = "杀内鬼 别杀好人", Color = Color.yellow };
        public static readonly RoleInfo Traveller = new RoleInfo { Name = "旅行家", Blurb = "想去哪就去哪", Target = "使用能力快速移动", Color = Color.cyan };
        public static readonly RoleInfo MechanicExpert = new RoleInfo { Name = "机械专家", Blurb = "没啥是螺丝刀解决不了的", Target = "快速维修破坏", Color = Color.blue };
        public static readonly RoleInfo LightPrayer = new RoleInfo { Name = "祈光人", Blurb = "赞美太阳!", Target = "不惧黑暗 祝福队友", Color = new Color32(250, 250, 210, byte.MaxValue) };
        public static readonly RoleInfo Judge = new RoleInfo { Name = "法官", Blurb = "掌控投票 就是掌控胜利", Target = "在投票环节大显身手", Color = new Color32(32, 77, 66, byte.MaxValue) };
        public static readonly RoleInfo DoomBait = new RoleInfo { Name = "厄运诱饵", Blurb = "画个圈圈诅咒凶手", Target = "杀你的人会被诅咒", Color = new Color32(205, 133, 0, byte.MaxValue) };
        public static readonly RoleInfo Coroner = new RoleInfo { Name = "验尸官", Blurb = "尸体可不会说谎", Target = "检查尸体 找出线索", Color = new Color32(45, 106, 165, byte.MaxValue) };
        public static readonly RoleInfo Diviner = new RoleInfo { Name = "占卜家", Blurb = "老兄 要算一卦吗", Target = "通过占卜获得启示", Color = new Color32(171, 230, 255, byte.MaxValue) };
        public static readonly RoleInfo Eavesdropper = new RoleInfo { Name = "窃听者", Blurb = "嘘! 看眼YY有没有人偷听", Target = "窃听周围的信号", Color = new Color32(207, 207, 207, byte.MaxValue) };
        public static readonly RoleInfo CurseMage = new RoleInfo { Name = "咒法师", Blurb = "名字 + 职业 + 记小本本 = ?", Target = "知晓职业发动诅咒", Color = Color.yellow };

        public static readonly RoleInfo ArsonExpert = new RoleInfo { Name = "纵火家", Blurb = "艺术就是 纵火!", Target = "给所有人涂油", Color = new Color32(250, 106, 106, byte.MaxValue) };

        public static readonly RoleInfo Impostor = new RoleInfo { Name = "伪装者", Blurb = "杀光他们", Target = "杀死好人 破坏设施", Color = Color.red };
        public static readonly RoleInfo NoneFaceMan = new RoleInfo { Name = "无面人", Blurb = "没人知道你的真面目", Target = "变脸成其他玩家", Color = Color.red };
        public static readonly RoleInfo SerialKiller = new RoleInfo { Name = "连环杀手", Blurb = "天啊 再不砍点什么我要疯了", Target = "长时间不杀人会发疯", Color = Color.red };
        public static readonly RoleInfo CurseWarlock = new RoleInfo { Name = "咒术师", Blurb = "名字 + 职业 + 记小本本 = ?", Target = "知晓职业发动诅咒", Color = Color.red };
    }

    //public static class RoleNameDictionary
    //{
    //    public const string Crewmate = "船员";
    //    public const string Sheriff = "治安官";
    //    public const string Traveller = "旅行家";
    //    public const string MechanicExpert = "机械专家";
    //    public const string LightPrayer = "祈光人";
    //    public const string Judge = "法官";
    //    public const string DoomBait = "厄运诱饵";
    //    public const string Coroner = "验尸官";
    //    public const string Diviner = "占卜家";
    //    public const string Eavesdropper = "窃听者";
    //    public const string CurseMage = "咒法师";

    //    public const string ArsonExpert = "纵火家";

    //    public const string Impostor = "伪装者";
    //    public const string NoneFaceMan = "无面人";
    //    public const string SerialKiller = "连环杀手";
    //    public const string CurseWarlock = "咒术师";
    //}

    //public static class RoleBlurbDictionary
    //{
    //    public const string CrewmateBlurb = "白板瑟瑟发抖";
    //    public const string SheriffBlurb = "毙掉内鬼";
    //    public const string TravellerBlurb = "想去哪就去哪";
    //    public const string MechanicExpertBlurb = "你有菜刀 我有螺丝刀";
    //    public const string LightPrayerBlurb = "赞美太阳!";
    //    public const string JudgeBlurb = "掌控投票 就是掌控胜利";
    //    public const string DoomBaitBlurb = "画个圈圈诅咒凶手";
    //    public const string CoronerBlurb = "尸体可不会说谎";
    //    public const string DivinerBlurb = "老兄 要算一卦吗";
    //    public const string EavesdropperBlurb = "嘘! 看眼YY有没有人偷听";
    //    public const string CurseMageBlurb = "名字 + 职业 + 记小本本 = ？";

    //    public const string ArsonExpertBlurb = "艺术就是 纵火!";

    //    public const string ImpostorBlurb = "杀光他们";
    //    public const string NoneFaceManBlurb = "没人知道你的真面目";
    //    public const string SerialKillerBlurb = "天啊 再不砍点什么我要疯了";
    //    public const string CurseWarlockBlurb = "名字 + 职业 + 记小本本 = ？";
    //}

    public static class ButtonTextDictionary
    {
        public const string KillButtonText = "击杀";
        public const string SheriffKillButtonText = "执法";
        public const string TravelButtonText_Mark = "标记地点";
        public const string TravelButtonText_Travel = "地点旅行";
        public const string TravelPlayerButtonText_Mark = "标记玩家";
        public const string TravelPlayerButtonText_Travel = "玩家旅行";
        public const string RepairButtonText = "维修";
        public const string BlessButtonText = "祝福";
        public const string NoneFaceManButtonText_Sampling = "取样";
        public const string NoneFaceManButtonText_Morph = "变形";
        public const string NoneFaceManButtonText_Relieve = "解除";
        public const string ArsonExpertOiledButtonText = "涂油";
        public const string ArsonExpertPlaceDollButton = "人偶陷阱";
        public const string DivineButtonText = "占卜";
    }

    public static class TaskDictionary
    {
        public static string GetTaskName(TaskTypes type)
        {
            switch (type)
            {
                case TaskTypes.None:
                    return type.ToString();
                default:
                    return type.ToString();
            }
        }
    }

    public static class TextDictionary
    {
        public const string Role = "职业";
        public const string DivineNonDead = "本回合没人死亡";
        public const string DivineHasDead = "本回合已经有人死亡";

        public static string DivineCrewmate(string playerName)
        {
            return $"{playerName}是船员";
        }

        public static string DivineExistent(string roleName)
        {
            return $"当前场上还有{roleName}存在";
        }

        public static string DivineNonExistent(string roleName)
        {
            return $"本局游戏中没有{roleName}";
        }

        public static string GenerateCoronerReport(DeathRecord record)
        {
            var deathElapsedTime = (DateTime.UtcNow - record.deadTime).TotalMilliseconds;
            string causeOfDeath;
            switch (record.Cause)
            {
                case CauseOfDeath.CommonImpostorKill:
                    causeOfDeath = "割伤（伪装者击杀）";
                    break;
                case CauseOfDeath.Sheriffkill:
                    causeOfDeath = "枪伤（治安官击杀）";
                    break;
                case CauseOfDeath.Suicide:
                    causeOfDeath = "自杀";
                    break;
                default:
                    causeOfDeath = "未知";
                    break;
            }
            return $"尸检报告：[{record.deadPlayer.PlayerControl.Data.PlayerName}]死于{Math.Round(deathElapsedTime / 1000)}秒前，死亡原因为[{causeOfDeath}]，凶案现场当时有{record.MurderScenePeopleCount}人。";
        }

        public static string AllTaskComplete(string playerName)
        {
            return $"{playerName} 已经完成了所有任务";
        }

        public static string LastTaskComplete(string taskName)
        {
            return $"最近被完成的任务是 {taskName}";
        }
    }
}
