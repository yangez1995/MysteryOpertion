using MysteryOpertion.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MysteryOpertion
{
    public static class ConfigLoader
    {
        public static Dictionary<string, ConfigSelecter> selecters = new Dictionary<string, ConfigSelecter>();

        public static void Load()
        {
            var UserRecommendConfig = ConfigSelecterFactory.Produce("UserRecommendConfig", "使用推荐配置", new string[] { "是", "否" });

            ConfigSelecterFactory.Produce("CrewmateCount", "船员阵营人数", 2, 14, 1, parent: UserRecommendConfig, marginTop: true);
            ConfigSelecterFactory.Produce("ImpostorCount", "伪装者阵营人数", 1, 5, 1, parent: UserRecommendConfig);
            ConfigSelecterFactory.Produce("ChaosCount", "混沌阵营人数", 0, 14, 1, parent: UserRecommendConfig);
            ConfigSelecterFactory.Produce("LurkerCount", "潜伏者人数", 0, 5, 1, parent: UserRecommendConfig);

            var CoronerPriority = ConfigSelecterFactory.Produce("CoronerPriority", "验尸官", TextDictionary.RoleAssignPriority, parent: UserRecommendConfig, marginTop: true);
            
            var CurseMagePriority = ConfigSelecterFactory.Produce("CurseMagePriority", "咒法师", TextDictionary.RoleAssignPriority, parent: UserRecommendConfig, marginTop: true);
            ConfigSelecterFactory.Produce("CurseMageTotalCount", "咒杀总次数", 1, 14, 1, parent: CurseMagePriority);
            ConfigSelecterFactory.Produce("CurseMageCount", "每回合可咒杀次数", 1, 14, 1, parent: CurseMagePriority);

            var DivinerPriority = ConfigSelecterFactory.Produce("DivinerPriority", "占卜家", TextDictionary.RoleAssignPriority, parent: UserRecommendConfig, marginTop: true);
            ConfigSelecterFactory.Produce("DivinerSkillCD", "占卜冷却时间", 0, 30, 2, parent: DivinerPriority);
            ConfigSelecterFactory.Produce("DivinerSkillCost", "占卜精神消耗", 0, 30, 2, parent: DivinerPriority);

            var DoomBaitPriority = ConfigSelecterFactory.Produce("DoomBaitPriority", "厄运诱饵", TextDictionary.RoleAssignPriority, parent: UserRecommendConfig, marginTop: true);
            ConfigSelecterFactory.Produce("DoomBaitDoomValue", "凶手精神减少", 0, 30, 2, parent: DoomBaitPriority);

            var EavesdropperPriority = ConfigSelecterFactory.Produce("EavesdropperPriority", "窃听者", TextDictionary.RoleAssignPriority, parent: UserRecommendConfig, marginTop: true);
            
            var JudgePriority = ConfigSelecterFactory.Produce(ConfigKeyDictionary.JudgePriority, "法官", TextDictionary.RoleAssignPriority, parent: UserRecommendConfig, marginTop: true);
            ConfigSelecterFactory.Produce(ConfigKeyDictionary.JudgeSeeVoteColor, "法官能看到投票颜色", new string[] { "否", "是" }, parent: JudgePriority, defaultValue: 1);

            var LightPrayerPriority = ConfigSelecterFactory.Produce("LightPrayerPriority", "祈光人", TextDictionary.RoleAssignPriority, parent: UserRecommendConfig, marginTop: true);
            ConfigSelecterFactory.Produce("LightPrayerSkillValue", "祝福精神增加", 0, 30, 2, parent: LightPrayerPriority);
            ConfigSelecterFactory.Produce("LightPrayerSkillCD", "祝福冷却时间", 0, 30, 2, parent: LightPrayerPriority);

            var MechanicExpertPriority = ConfigSelecterFactory.Produce("MechanicExpertPriority", "机械专家", TextDictionary.RoleAssignPriority, parent: UserRecommendConfig, marginTop: true);
            ConfigSelecterFactory.Produce("MechanicExpertSkillCost", "快速维修精神消耗", 0, 30, 2, parent: MechanicExpertPriority);

            var SheriffPriority = ConfigSelecterFactory.Produce(ConfigKeyDictionary.SheriffPriority, "治安官", TextDictionary.RoleAssignPriority, parent: UserRecommendConfig, marginTop: true);
            ConfigSelecterFactory.Produce(ConfigKeyDictionary.SheriffSkillCD, "击杀冷却时间", 0, 60, 2.5f, parent: SheriffPriority, defaultValue: 12);
            ConfigSelecterFactory.Produce(ConfigKeyDictionary.SheriffTotalCount, "击杀可用次数", 0, 14, 1, parent: SheriffPriority, defaultValue: 2);
            ConfigSelecterFactory.Produce(ConfigKeyDictionary.SheriffCanKillChaos, "可以击杀混沌阵营玩家", new string[] { "否", "是" }, parent: SheriffPriority);

            var TravellerPriority = ConfigSelecterFactory.Produce(ConfigKeyDictionary.TravellerPriority, "旅行家", TextDictionary.RoleAssignPriority, parent: UserRecommendConfig, marginTop: true);
            ConfigSelecterFactory.Produce(ConfigKeyDictionary.TravellerSkill1CD, "地点旅行冷却时间", 0, 60, 2.5f, parent: TravellerPriority, defaultValue: 4);
            ConfigSelecterFactory.Produce(ConfigKeyDictionary.TravellerSkill1Cost, "地点旅行精神消耗", 0, 20, 2, parent: TravellerPriority, defaultValue: 4);
            ConfigSelecterFactory.Produce(ConfigKeyDictionary.TravellerSkill2CD, "玩家旅行冷却时间", 0, 60, 2.5f, parent: TravellerPriority, defaultValue: 4);
            ConfigSelecterFactory.Produce(ConfigKeyDictionary.TravellerSkill2Cost, "玩家旅行精神消耗", 0, 20, 2, parent: TravellerPriority, defaultValue: 4);

            var BloodyHunterPriority = ConfigSelecterFactory.Produce("BloodyHunterPriority", "浴血猎人", TextDictionary.RoleAssignPriority, parent: UserRecommendConfig, marginTop: true);
            ConfigSelecterFactory.Produce("BloodyHunterLevelUpCD", "每次击杀后CD减少秒数", 0, 30, 2, parent: BloodyHunterPriority);
            ConfigSelecterFactory.Produce("BloodyHunterLevelUpSan", "每次击杀后精神最大值增加数", 0, 30, 2, parent: BloodyHunterPriority);
            ConfigSelecterFactory.Produce("BloodyHunterCurse", "几次击杀后获得咒杀能力", 0, 5, 1, parent: BloodyHunterPriority);
            ConfigSelecterFactory.Produce("BloodyHunterInvincible", "几次击杀后获得无敌能力", 0, 5, 1, parent: BloodyHunterPriority);

            var CurseWarlockPriority = ConfigSelecterFactory.Produce("CurseWarlockPriority", "咒术师", TextDictionary.RoleAssignPriority, parent: UserRecommendConfig, marginTop: true);
            ConfigSelecterFactory.Produce("CurseWarlockTotalCount", "咒杀总次数", 1, 14, 1, parent: CurseWarlockPriority);
            ConfigSelecterFactory.Produce("CurseWarlockCount", "每回合可咒杀次数", 1, 14, 1, parent: CurseWarlockPriority);

            var NoneFaceManPriority = ConfigSelecterFactory.Produce("NoneFaceManPriority", "无面人", TextDictionary.RoleAssignPriority, parent: UserRecommendConfig, marginTop: true);
            
            var SerialKillerPriority = ConfigSelecterFactory.Produce("SerialKillerPriority", "连环杀手", TextDictionary.RoleAssignPriority, parent: UserRecommendConfig, marginTop: true);
            ConfigSelecterFactory.Produce("SerialKillerSkillCD", "击杀CD", 0, 40, 3, parent: SerialKillerPriority);
            ConfigSelecterFactory.Produce("SerialKillerSkillValue", "击杀精神值奖励", 0, 40, 5, parent: SerialKillerPriority);
            ConfigSelecterFactory.Produce("SerialKillerSanTimer", "每隔几秒减少1精神值", 0, 5, 1, parent: SerialKillerPriority);

            var ArsonExpertPriority = ConfigSelecterFactory.Produce("ArsonExpertPriority", "纵火家", TextDictionary.RoleAssignPriority, parent: UserRecommendConfig, marginTop: true);
            ConfigSelecterFactory.Produce("ArsonExpertSkillCD", "涂油冷却时间", 0, 30, 2, parent: ArsonExpertPriority);
            ConfigSelecterFactory.Produce("ArsonExpertSkillNeed", "涂油所需时间", 0, 5, 1, parent: ArsonExpertPriority);

            var JesterPriority = ConfigSelecterFactory.Produce("JesterPriority", "小丑", TextDictionary.RoleAssignPriority, parent: UserRecommendConfig, marginTop: true);
            
            var SpectatorPriority = ConfigSelecterFactory.Produce("SpectatorPriority", "观众", TextDictionary.RoleAssignPriority, parent: UserRecommendConfig, marginTop: true);
            ConfigSelecterFactory.Produce("SpectatorWinCount", "胜利需要猜对职业数", 0, 10, 1, parent: SpectatorPriority);
        }

        public static void UseRecommendConfig(int playerCount)
        {

        }
    }
}
