using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace HAL_Solver
{
    public class GoalList
    {
        Dictionary<char,GoalGroup> goalgroups;
        Collection<char> names;
        
        public GoalList()
        {
            goalgroups = new Dictionary<char,GoalGroup>();
            names = new Collection<char>();
        }
        public void Add(byte x, byte y, char name)
        {
            if (!goalgroups.ContainsKey(name)) { goalgroups[name] = new GoalGroup(name); names.Add(name); }
            goalgroups[name].addgoal(x, y);
        }
        class GoalGroup
        {
            char name;
            Collection<Node> goals;
            public GoalGroup(char name)
            {
                this.name = name;
                goals = new Collection<Node>();
            }
            public void addgoal(byte x, byte y)
            {
                goals.Add(new Node(x, y));
            }
            public bool IsInGoal(BoxList boxlist, Collection<int> boxes)
            {
                
                foreach (Node goal in goals)
                {
                    bool res = false;
                    foreach (int boxnumber in boxes)
                    {
                        if (boxlist[boxnumber] == goal) { res = true; break; }
                    }
                    if (res == false) { return false; }
                }
                return true;
            }
        }
        public bool IsInGoal(BoxList boxlist)
        {
            foreach (char name in names)
            {
                if (!goalgroups[name].IsInGoal(boxlist, BoxList.boxNameGroups[name])) { return false; }
            }
            return true;
        }
    }
}
