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
        public static readonly RoleInfo Crewmate = new RoleInfo { Type = RoleType.Crewmate, Name = "船员", Blurb = "白板瑟瑟发抖", Target = "完成任务 找出内鬼", Color = Color.white };
        public static readonly RoleInfo Sheriff = new RoleInfo { Type = RoleType.Sheriff, Name = "治安官", Blurb = "毙掉内鬼", Target = "杀内鬼 别杀好人", Color = Color.yellow };
        public static readonly RoleInfo Traveller = new RoleInfo { Type = RoleType.Traveller, Name = "旅行家", Blurb = "想去哪就去哪", Target = "使用能力快速移动", Color = Color.cyan };
        public static readonly RoleInfo MechanicExpert = new RoleInfo { Type = RoleType.MechanicExpert, Name = "机械专家", Blurb = "没啥是螺丝刀解决不了的", Target = "快速维修破坏", Color = Color.blue };
        public static readonly RoleInfo LightPrayer = new RoleInfo { Type = RoleType.LightPrayer, Name = "祈光人", Blurb = "赞美太阳!", Target = "不惧黑暗 祝福队友", Color = new Color32(250, 250, 210, byte.MaxValue) };
        public static readonly RoleInfo Judge = new RoleInfo { Type = RoleType.Judge, Name = "法官", Blurb = "掌控投票 就是掌控胜利", Target = "在投票环节大显身手", Color = new Color32(32, 77, 66, byte.MaxValue) };
        public static readonly RoleInfo DoomBait = new RoleInfo { Type = RoleType.DoomBait, Name = "厄运诱饵", Blurb = "画个圈圈诅咒凶手", Target = "杀你的人会被诅咒", Color = new Color32(205, 133, 0, byte.MaxValue) };
        public static readonly RoleInfo Coroner = new RoleInfo { Type = RoleType.Coroner, Name = "验尸官", Blurb = "尸体可不会说谎", Target = "检查尸体 找出线索", Color = new Color32(45, 106, 165, byte.MaxValue) };
        public static readonly RoleInfo Diviner = new RoleInfo { Type = RoleType.Diviner, Name = "占卜家", Blurb = "老兄 要算一卦吗", Target = "通过占卜获得启示", Color = new Color32(171, 230, 255, byte.MaxValue) };
        public static readonly RoleInfo Eavesdropper = new RoleInfo { Type = RoleType.Eavesdropper, Name = "窃听者", Blurb = "嘘! 看眼YY有没有人偷听", Target = "窃听周围的信号", Color = new Color32(207, 207, 207, byte.MaxValue) };
        public static readonly RoleInfo CurseMage = new RoleInfo { Type = RoleType.CurseMage, Name = "咒法师", Blurb = "名字 + 职业 + 记小本本 = ?", Target = "知晓职业发动诅咒", Color = Color.yellow };

        public static readonly RoleInfo ArsonExpert = new RoleInfo { Type = RoleType.ArsonExpert, Name = "纵火家", Blurb = "艺术就是 纵火!", Target = "给所有人涂油", Color = new Color32(250, 106, 106, byte.MaxValue) };
        public static readonly RoleInfo Jester = new RoleInfo { Type = RoleType.Jester, Name = "小丑", Blurb = "得想办法整个节目", Target = "想办法被投出去", Color = new Color32(236, 98, 165, byte.MaxValue) };
        public static readonly RoleInfo Spectator = new RoleInfo { Type = RoleType.Spectator, Name = "观众", Blurb = "人生如看戏", Target = "猜出其他人的职业", Color = new Color32(127, 255, 212, byte.MaxValue) };

        public static readonly RoleInfo Impostor = new RoleInfo { Type = RoleType.Impostor, Name = "伪装者", Blurb = "杀光他们", Target = "杀死好人 破坏设施", Color = Color.red };
        public static readonly RoleInfo NoneFaceMan = new RoleInfo { Type = RoleType.NoneFaceMan, Name = "无面人", Blurb = "没人知道你的真面目", Target = "变脸成其他玩家", Color = Color.red };
        public static readonly RoleInfo SerialKiller = new RoleInfo { Type = RoleType.SerialKiller, Name = "连环杀手", Blurb = "天啊 再不砍点什么我要疯了", Target = "长时间不杀人会发疯", Color = Color.red };
        public static readonly RoleInfo CurseWarlock = new RoleInfo { Type = RoleType.CurseWarlock, Name = "咒术师", Blurb = "名字 + 职业 + 记小本本 = ?", Target = "知晓职业发动诅咒", Color = Color.red };
        public static readonly RoleInfo BloodyHunter = new RoleInfo { Type = RoleType.BloodyHunter, Name = "浴血猎人", Blurb = "杀戮 然后进化", Target = "每次击杀后都能变得更强", Color = Color.red };
        public static readonly RoleInfo Lurker = new RoleInfo { Type = RoleType.Lurker, Name = "潜伏者", Blurb = "藏好自己 逆转局势", Target = "在队友死光前不能击杀", Color = Color.red};


    }

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

    public static class MysteryItemDictionary
    {
        public const string SheriffBadge = "警徽";
        public const string WorkClothes = "工作服";
        public const string CoronerScalpel = "验尸小刀";
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
        public static string[] RoleAssignPriority = { "不分配", "小概率分配", "中概率分配", "大概率分配", "优先分配"};

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

    public static class ConfigKeyDictionary
    {
        public const string UserRecommendConfig = "UserRecommendConfig";
        
        public const string CrewmateCount = "CrewmateCount";
        public const string ImpostorCount = "ImpostorCount";
        public const string ChaosCount = "ChaosCount";
        public const string LurkerCount = "LurkerCount";
        
        public const string CoronerPriority = "CoronerPriority";
        public const string CurseMagePriority = "CurseMagePriority";
        public const string CurseMageTotalCount = "CurseMageTotalCount";
        public const string CurseMageCount = "CurseMageCount";
        public const string DivinerPriority = "DivinerPriority";
        public const string DivinerSkillCD = "DivinerSkillCD";
        public const string DivinerSkillCost = "DivinerSkillCost";
        public const string DoomBaitPriority = "DoomBaitPriority";
        public const string DoomBaitDoomValue = "DoomBaitDoomValue";
        public const string EavesdropperPriority = "EavesdropperPriority";
        
        public const string JudgePriority = "JudgePriority";
        public const string JudgeSeeVoteColor = "JudgeSeeVoteColor";

        public const string LightPrayerPriority = "LightPrayerPriority";
        public const string LightPrayerSkillValue = "LightPrayerSkillValue";
        public const string LightPrayerSkillCD = "LightPrayerSkillCD";
        public const string MechanicExpertPriority = "MechanicExpertPriority";
        public const string MechanicExpertSkillCost = "MechanicExpertSkillCost";
        
        public const string SheriffPriority = "SheriffPriority";
        public const string SheriffSkillCD = "SheriffSkillCD";
        public const string SheriffTotalCount = "SheriffTotalCount";
        public const string SheriffCanKillChaos = "SheriffCanKillChaos";

        public const string TravellerPriority = "TravellerPriority";
        public const string TravellerSkill1CD = "TravellerSkill1CD";
        public const string TravellerSkill1Cost = "TravellerSkill1Cost";
        public const string TravellerSkill2CD = "TravellerSkill2CD";
        public const string TravellerSkill2Cost = "TravellerSkill2Cost";

        public const string BloodyHunterPriority = "BloodyHunterPriority";
        public const string BloodyHunterLevelUpCD = "BloodyHunterLevelUpCD";
        public const string BloodyHunterLevelUpSan = "BloodyHunterLevelUpSan";
        public const string BloodyHunterCurse = "BloodyHunterCurse";
        public const string BloodyHunterInvincible = "BloodyHunterInvincible";
        public const string CurseWarlockPriority = "CurseWarlockPriority";
        public const string CurseWarlockTotalCount = "CurseWarlockTotalCount";
        public const string CurseWarlockCount = "CurseWarlockCount";
        public const string NoneFaceManPriority = "NoneFaceManPriority";
        public const string SerialKillerPriority = "SerialKillerPriority";
        public const string SerialKillerSkillCD = "SerialKillerSkillCD";
        public const string SerialKillerSkillValue = "SerialKillerSkillValue";
        public const string SerialKillerSanTimer = "SerialKillerSanTimer";
        public const string ArsonExpertPriority = "ArsonExpertPriority";
        public const string ArsonExpertSkillCD = "ArsonExpertSkillCD";
        public const string ArsonExpertSkillNeed = "ArsonExpertSkillNeed";
        public const string JesterPriority = "JesterPriority";
        public const string SpectatorPriority = "SpectatorPriority";
        public const string SpectatorWinCount = "SpectatorWinCount";
    }
}
