using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaNDungeon
{
    using System;
    using System.Collections.Generic;

    public enum QuestStatus
    {

    }


    public class Quest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int Goal { get; set; }
        public string Reward { get; set; }


        public QuestStatus Status { get; set; }



        public Quest(string title, string description, int goal, string reward)
        {
            Title = title;
            Description = description;
            Goal = goal;
            Reward = reward;
        }

        public void StartQuest()
        {
        }


        public void UpdateProgress()
        {

        }

        public void CompleteQuest()
        {

        }
    }

    public class QuestManager
    {
    }

}
       