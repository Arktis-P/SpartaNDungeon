using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaNDungeon
{
    public class Quest
    {
        public string Name { get; set; }  // quest name
        public string Desc { get; set; }  // quest description
        public string TargetMonster { get; set; }  // target monster for the qeust
        public int TargetCount { get; set; }  // how many monsters needed to be defeated
        public int CurrentCount { get; set; }  // how many monsters defeated at the moment
        public int Reward { get; set; }  // reward (mostly gold) as quest reward 

        public bool IsSelected { get; set; } 
        public bool IsSatisfied => CurrentCount >= TargetCount;  // if satisfied the clear condition
        public bool IsCompleted { get; set; }  // if quest is already completed


        // public List<Quest> Quests;  // list of quests

        public Quest(string name, string desc, string targetMonster, int targetCount, int reward)
        {
            Name = name; Desc = desc;
            TargetMonster = targetMonster; TargetCount = targetCount;
            Reward = reward;
            IsSelected = false; IsCompleted = false;
            // get quset list
            // Quests = QuestList.GetAllQuests();
        }

        // convert selected state if selected
        public void SelectQuest()
        {
            // if not selected, convert selected state
            if (!IsSelected)
            {
                IsSelected = true; return;
            }
        }

        // convert completed stated if chosen to be completed
        public void CompleteQuest()
        {
            // check if satisfied, and convert complete state
            if (IsSelected && IsSatisfied)
            {
                IsSelected = false;
                IsCompleted = true;
                Player.Gold += Reward;
            }
            // if not satisfied, show cannot complete caution msg
        }
        
    }

    class QuestList
    {
        static List<Quest> questList;

        static QuestList()
        {
            MonsterManager manager = new MonsterManager(0);
            questList = new List<Quest>
            {
                new Quest("미니언 처치","미니언을 5마리 처치하십시오.",manager.monsterList[0].Name,5,25),
                new Quest("공허충 처치","공허충을 5마리 처치하십시오.",manager.monsterList[1].Name,5,50),
                new Quest("대포미니언 처치","대포미니언을 5마리 처치하십시오.",manager.monsterList[2].Name,5,100),
                new Quest("슈퍼 미니언 처치","슈퍼 미니언을 5마리 처치하십시오.",manager.monsterList[3].Name,5,150)
            };
        }
        public Quest GetQuest(int index)
        {
            Quest quest;
            if (index >= 0 && index < questList.Count) { return quest = questList[index]; }
            return null;
        }
        public static List<Quest> GetAllQuests()
        {
            return questList;
        }
    }

    static class QuestManager
    {
        public static bool isQuest = false;
        private static UI ui = new UI();
        private static Quest quest;
        public static int reward;
        public static List<Quest> questList = QuestList.GetAllQuests();
        static QuestManager() { }

        public static void DisplayQuest()
        {
            if (questList.Count == 0) { Console.WriteLine("  퀘스트 목록이 비어 있습니다."); return; }

            // preparation for list-up
            List<string> questNames = new List<string>();
            List<string> questDescs = new List<string>();
            foreach (Quest q in questList)
            {
                questNames.Add(q.Name); questDescs.Add(q.Desc);
            }
            // decide spacings
            int nameMax = ConsoleUtil.CalcuatedMaxNumber(questNames);
            int descMax = ConsoleUtil.CalcuatedMaxNumber(questDescs);

            // show list of quests
            string str;
            int i = 0;
            foreach (Quest q in questList)
            {
                str = "";  // initializing
                str += String.Format("{0,2}. ", i + 1) + (q.IsSelected ? "(V)" : "   ");
                str += ConsoleUtil.WriteSpace(q.Name, nameMax) + "| " + ConsoleUtil.WriteSpace(q.Desc, descMax);
                if (!q.IsCompleted)
                {
                    str += "| " + $"{q.CurrentCount} / {q.TargetCount}마리";
                    str += q.IsSatisfied ? " | 완료 가능" : "";
                }
                i++;

                // check if selected, display in blue
                // check if satisfied, display in green
                // check if already completed, to disply in gray
                if (q.IsCompleted) { ConsoleUtil.ColorWrite(str, ConsoleColor.Gray); }
                else if (q.IsSatisfied) { ConsoleUtil.ColorWrite(str, ConsoleColor.Green); }
                else if (q.IsSelected) { ConsoleUtil.ColorWrite(str, ConsoleColor.DarkCyan); }
                else { Console.WriteLine(str); }
            }
        }
        public static void SelectQuest(int input)
        {
            isQuest = !isQuest;
            questList[input - 1].SelectQuest();
            quest = questList[input - 1];
            reward = quest.Reward;
        }
        public static bool CheckQuestCompleted(int input)
        {
            bool isCompleted = false;
            Quest temp = questList[input - 1];
            if (temp.IsCompleted) { isCompleted = true; }
            return isCompleted;
        }
        public static bool CheckQuestSatisfie(int input)
        {
            bool isSatisfied = false;
            Quest temp = questList[input - 1];
            if (temp.IsSatisfied) { isSatisfied = true; }
            return isSatisfied;
        }
        public static void CompleteQuest(int input)
        {
            questList[input - 1].CompleteQuest();
            quest = null;
            isQuest = false;
        }
        
        public static void QuestCountUp(Monster monster)
        {
            if (!isQuest) { return; }
            if (monster.Name == quest.TargetMonster) { quest.CurrentCount++; }
        }
    }
}
