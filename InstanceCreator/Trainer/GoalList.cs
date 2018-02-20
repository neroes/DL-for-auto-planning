using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Trainer
{
    // The container for our goals along with the check to see if all goals are meet
    public class GoalList
    {
        Dictionary<char,GoalGroup> goalgroups;
        HashSet<char> names;
        
        public GoalList()
        {
            goalgroups = new Dictionary<char,GoalGroup>();
            names = new HashSet<char>();
        }
        public void Add(byte x, byte y, char name)
        {
            if (!goalgroups.ContainsKey(name)) { goalgroups[name] = new GoalGroup(name); names.Add(name); }
            goalgroups[name].addgoal(x, y);
        }
        class GoalGroup
        {
            char name;
            HashSet<Node> goals;
            public GoalGroup(char name)
            {
                this.name = name;
                goals = new HashSet<Node>();
            }
            public void addgoal(byte x, byte y)
            {
                goals.Add(new Node(x, y));
            }
            public int ManDist(BoxList boxlist, HashSet<int> boxes) // Manhattan Distance
            {
                int totaldist = 0;

                foreach (Node goal in goals)
                {
                    int minDistToGoal = 1000000;
                    foreach (int boxnumber in boxes)
                    {
                        int dist = boxlist[boxnumber] - goal;
                        if (dist < minDistToGoal) { minDistToGoal = dist; }
                    }
                    totaldist += minDistToGoal;
                }
                return totaldist;
            }
            public bool IsInGoal(BoxList boxlist, HashSet<int> boxes)
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

            public HashSet<Node> getGoals()
            {
                return goals;
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
        public int ManDist(BoxList boxlist)
        {
            int goaldist = 0;
            foreach (char name in names)
            {
                goaldist += goalgroups[name].ManDist(boxlist, BoxList.boxNameGroups[name]);
            }
            return goaldist;
        }

        public HashSet<char> getGoalNames()
        {
            return names;
        }

        public HashSet<Node> getGoals(char name)
        {
            return goalgroups[name].getGoals();
        }
    }
}
