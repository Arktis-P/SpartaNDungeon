using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaNDungeon
{
    public class Quest1
    {
        public string name;
        public string targetMonster;
        public int targetCount;
        public int currentCount;
        public string decrip;
        public int rewardGold;
        public bool isCleared;

        public Quest1(string name, string targetMonster, int targetCount, string decrip, int reward)
        {
            this.name = name;
            this.targetMonster = targetMonster;
            this.targetCount = targetCount;
            currentCount = 0;
            this.decrip = decrip;
            this.rewardGold = reward;
            isCleared = false;
        }
        public void DisplayQuest()
        {
             string status = isCleared ? "완료" : $"진행 중 ({currentCount}/{targetCount})";
             Console.WriteLine($"퀘스트명: {name}, 상태: {status}, 보상: {rewardGold} 골드");
           
        }

    }
    public class Quest1Manager
    {
        List<Quest1> quests = new List<Quest1>();
        Player player;
        MonsterManager manager;
        public Quest1Manager(Player player, MonsterManager manager)
        {
            this.player = player;
            this.manager = manager;
            quests.Add(new Quest1("미니언 처치", manager.monsterList[0].Name, 5, "미니언 5마리를 처치하십시오.", 1000));
            quests.Add(new Quest1("슈퍼 미니언 처치", manager.monsterList[3].Name, 5, "슈퍼 미니언 5마리를 처치하십시오.", 1000));
        }

        public void CheckQuest(List<Quest1> activeQuest)
        {
            foreach (Quest1 quest in activeQuest)
            {
                foreach (Monster monster in player.questMonsters)
                {
                    if (monster.Name == quest.name)
                    {
                        quest.currentCount++;
                    }
                }
            }
        }

        public void DisplayQuests()
        {
            for (int i = 0; i < quests.Count; i++)
            {
                Console.Write($"{i+1}. ");
                quests[i].DisplayQuest();
            }
        }
    }
}
