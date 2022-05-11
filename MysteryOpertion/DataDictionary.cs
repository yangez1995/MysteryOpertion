using MysteryOpertion.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MysteryOpertion
{
    public static class RoleNameDictionary
    {
        public const string Crewmate = "船员";
        public const string Sheriff = "治安官";
        public const string Traveller = "旅行家";
        public const string MechanicExpert = "机械专家";
        public const string LightPrayer = "祈光人";
        public const string Judge = "法官";
        public const string DoomBait = "厄运诱饵";
        public const string Coroner = "验尸官";

        public const string ArsonExpert = "纵火家";

        public const string Impostor = "伪装者";
        public const string NoneFaceMan = "无面人";
    }

    public static class RoleBlurbDictionary
    {
        public const string CrewmateBlurb = "白板瑟瑟发抖";
        public const string SheriffBlurb = "毙掉内鬼";
        public const string TravellerBlurb = "想去哪就去哪";
        public const string MechanicExpertBlurb = "你有菜刀 我有螺丝刀";
        public const string LightPrayerBlurb = "赞美太阳!";
        public const string JudgeBlurb = "掌控投票 就是掌控胜利";
        public const string DoomBaitBlurb = "画个圈圈诅咒凶手";
        public const string CoronerBlurb = "尸体可不会说谎";

        public const string ArsonExpertBlurb = "艺术就是 纵火!";

        public const string ImpostorBlurb = "杀光他们";
        public const string NoneFaceManBlurb = "没人知道你的真面目";
    }

    public static class ButtonTextDictionary
    {
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
    }

    public static class TextDictionary
    {
        public const string Role = "职业";

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
            return $"尸检报告：[{record.deadPlayer.playerControl.Data.PlayerName}]死于{Math.Round(deathElapsedTime / 1000)}秒前，死亡原因为[{causeOfDeath}]，凶案现场当时有{record.MurderScenePeopleCount}人。";
        }
    }
}
