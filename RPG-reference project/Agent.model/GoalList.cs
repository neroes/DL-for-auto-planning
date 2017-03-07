using System.Collections.Generic;
using System.Drawing;
using System.Collections.ObjectModel;

namespace Agent.model
{
    class GoalList
    {
        Dictionary<char,GoalGroup> goalgroups;
        
        public GoalList()
        {
            goalgroups = new Dictionary<char,GoalGroup>();
        }
        public void Add(int x, int y, char name)
        {
            if (!goalgroups.ContainsKey(name)) { goalgroups[name] = new GoalGroup(name); }
            goalgroups[name].addgoal(x, y);
        }
        public class GoalGroup
        {
            char name;
            Collection<Node> goals;
            public GoalGroup(char name)
            {
                this.name = name;
                goals = new Collection<Node>();
            }
            public void addgoal(int x, int y)
            {
                goals.Add(new Node(x, y));
            }
        }
    }
}
