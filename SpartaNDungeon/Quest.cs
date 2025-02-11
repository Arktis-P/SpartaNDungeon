using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaNDungeon
{
    public enum QuestType { Kill = 1, Collect };
    abstract class Quest
    {
        public string Name { get; }  // name of the quest
        public string Desc { get; }  // description of the quest
        public QuestType Type { get; }  // type of the quest (kill / collect)
        public int Stage { get; }  // which stage of kill or collect quest

        public Quest(string name, string desc, int typeInt, int stage)
        {
            Name = name; Desc = desc; Type = (QuestType)typeInt; Stage = stage;
        }
    }

    interface IKillQeust
    {
        public string TargetMonster { get; }
        public int TargetCount { get; }
    }

    interface ICollectQuest
    {
        public string TargetItem { get; }
        public int TargetCount { get; }
    }

    class KillQuest : Quest, IKillQeust
    {
        public string TargetMonster { get; }
        public int TargetCount { get; }

        public KillQuest(string name, string desc, int typeInt, int stage, string targetMonster, int targetCount) : base(name, desc, typeInt, stage)
        {
            TargetMonster = targetMonster; TargetCount = targetCount;
        }
    }

    class CollectQuest : Quest, ICollectQuest
    {
        public string TargetItem { get; }
        public int TargetCount { get; }

        public CollectQuest(string name, string desc, int typeInt, int stage, string targetItem, int targetCount) : base(name, desc, typeInt, stage)
        {
            TargetItem = targetItem; TargetCount = targetCount;
        }
    }

    static class QuestData
    {
        static Dictionary<string, Quest> quests;

        static List<string> killQuestNames = new List<string>();
        static List<string> collectQuestNames = new List<string>();

        static QuestData()  // initializing
        {
            quests = new Dictionary<string, Quest>()
            {
                { "미니언 처치", new KillQuest("미니언 처치", "전투에서 미니언을 5마리 처치하십시오.",1,1,"미니언", 5) },
                { "공허충 처치", new KillQuest("공허충 처치", "전투에서 공허충을 5마리 처치하십시오.",1,2,"공허충", 5) },
                { "대포 미니언 처치", new KillQuest("대포 미니언 처치", "전투에서 대포 미니언을 5마리 처치하십시오.",1,3,"대포 미니언", 5) },
                { "외곽 포탑 파괴", new KillQuest("외곽 포탑 처치", "전투에서 외곽 포탑을 3회 파괴하십시오.",1,4,"외곽 포탑", 3) },
                { "내부 포탑 파괴", new KillQuest("내부 포탑 처치", "전투에서 내부 포탑을 3회 파괴하십시오.",1,5,"내부 포탑", 3) },
                { "억제기 포탑 파괴", new KillQuest("억제기 포탑 파괴", "전투에서 억제기 포탑을 3회 파괴하십시오.",1,6,"억제기 포탑", 3) },
                { "슈퍼 미니언 처치", new KillQuest("슈퍼 미니언 처치", "전투에서 슈퍼 미니언을 5마리 처치하십시오.",1,7,"슈퍼 미니언", 5) },
                { "넥서스 포탑 파괴", new KillQuest("넥서스 포탑 파괴", "전투에서 넥서스 포탑을 3회 파괴하십시오.",1,8,"넥서스 포탑", 3) },
                { "도란의 방패 획득", new CollectQuest("도란의 방패 획득", "전투에서 도란의 방패를 획득하십시오.",2,1,"도란의 방패", 1) },
                { "도란의 반지 획득", new CollectQuest("도란의 반지 획득", "전투에서 도란의 반지를 획득하십시오.",2,2,"도란의 반지", 1) },
                { "태양불꽃 망토 획득", new CollectQuest("태양불꽃 망토 획득", "전투에서 태양불꽃 망토를 획득하십시오.",2,3,"태양불꽃 망토", 1) },
                { "티아맷 획득", new CollectQuest("티아맷 획득", "전투에서 티아맷을 획득하십시오.",2,4,"티아맷", 1) },
                { "라바돈의 죽음 모자", new CollectQuest("라바돈의 죽음 모자 획득", "전투에서 라바돈의 죽음 모자를 획득하십시오.",2,5,"라바돈의 죽음 모자", 1) },
                { "스태틱의 단검 획득", new CollectQuest("스태틱의 단검 획득", "전투에서 스태틱의 단검을 획득하십시오.",2,1,"스태틱의 단검", 1) }
            };

            foreach (var q in quests)
            {
                if (quests[q.Key] is IKillQeust) { killQuestNames.Add(quests[q.Key].Name); }
                else if (quests[q.Key] is ICollectQuest) { collectQuestNames.Add(quests[q.Key].Name); }
            }
        }

        // return individual quest
        public static Quest GetQuest(string name)
        {
            if (quests.ContainsKey(name)) { return quests[name]; }
            return null;
        }
        // return a set of quest
        public static Quest[] GetQeustSet(int killStage, int collectStage)
        {
            Quest[] questSet = new Quest[2];
            // use index to find quests from quests
            Quest killQuest = GetQuest(killQuestNames[killStage]);
            Quest collectQuest = GetQuest(collectQuestNames[collectStage]);
            // add two quests in questSet
            questSet[0] = killQuest; questSet[1] = collectQuest;

            return questSet;
        }
    }
}
