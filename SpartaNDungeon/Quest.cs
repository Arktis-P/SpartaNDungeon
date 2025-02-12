using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaNDungeon
{
    public class Quest
    {
        public string QuestName { get; set; }
        public string Description { get; set; }
        public Monster TargetMonster { get; set; }  // 몬스터 객체로 변경
        public int TargetCount { get; set; }  // 몇 마리 처치해야 하는지
        public int CurrentCount { get; set; }  // 현재 처치한 몬스터 수
        public int RewardGold { get; set; }  // 보상 골드       

        public bool IsCompleted => CurrentCount >= TargetCount;  // 퀘스트 완료 여부 확인

        public Quest(string questName, string description, Monster targetMonster, int targetCount, int rewardGold)
        {
            QuestName = questName;
            Description = description;
            TargetMonster = targetMonster;
            TargetCount = targetCount;
            RewardGold = rewardGold;
            CurrentCount = 0;
        }

        public void UpdateQuest(Monster monster)
        {
            if (monster.Name == TargetMonster.Name)  // 몬스터 이름을 비교하여 목표 몬스터인지 확인
            {
                CurrentCount++;
                Console.WriteLine($"퀘스트 진행 중: {CurrentCount}/{TargetCount} 마리 처치!");
            }
        }

        public void ClaimReward(Player player)
        {
            if (IsCompleted)
            {
                Player.Gold += RewardGold;
                Console.WriteLine($"퀘스트 완료! 보상으로 {RewardGold}골드를 받았습니다.");
            }
        }
    }
    public class QuestList
    {
        private List<Quest> quests;

        public QuestList()
        {
            quests = new List<Quest>();
        }

        // 퀘스트 추가
        public void AddQuest(Quest quest)
        {
            quests.Add(quest);
            Console.WriteLine($"퀘스트 '{quest.QuestName}' 가 추가되었습니다.");
        }

        // 퀘스트 진행 상황 업데이트
        public void UpdateQuestProgress(List<Monster> monsters)
        {
            foreach (var quest in quests)
            {
                foreach (var monster in monsters)
                {
                    quest.UpdateQuest(monster);
                }
            }
        }

        // 완료된 퀘스트 보상 지급
        public void ClaimRewards(Player player)
        {
            foreach (var quest in quests)
            {
                if (quest.IsCompleted)
                {
                    quest.ClaimReward(player);
                }
            }
        }

        // 퀘스트 목록 출력
        public void DisplayQuests()
        {
            Console.WriteLine("현재 퀘스트 목록:");
            foreach (var quest in quests)
            {
                string status = quest.IsCompleted ? "완료" : $"진행 중 ({quest.CurrentCount}/{quest.TargetCount})";
                Console.WriteLine($"퀘스트명: {quest.QuestName}, 상태: {status}, 보상: {quest.RewardGold} 골드");
            }
        }
    }


    public class QuestManager
    {
        public List<Quest> Quests { get; set; }

        public QuestManager(List<Monster> monsterList,List<Monster> towerList,List<Monster> eliteList)
        {
            Quests = new List<Quest>();

            // 퀘스트 1: 미니언 5마리 처치
            Quest quest1 = new Quest("미니언 처치", "미니언 5마리를 처치하십시오.", monsterList[0].Name, 5, 100);

            // 퀘스트 2: 슈퍼 미니언 3마리 처치
            Quest quest2 = new Quest("슈퍼 미니언 처치", "슈퍼 미니언 3마리를 처치하십시오.", monsterList[3].Name, 3, 200);

            // 퀘스트를 리스트에 추가
            Quests.Add(quest1);
            Quests.Add(quest2);
        }
    }


}

