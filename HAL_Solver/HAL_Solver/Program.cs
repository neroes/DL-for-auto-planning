using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace HAL_Solver
{
    class Program
    {
        static void Main(string[] args)
        {
            Map map = null;
            MapLoad.loadMap("MAsimple1.lvl", out map);/*
            Map map2 = new Map(map);
            act[] actions = new act[1];
            actions[0] = new act(Interact.MOVE, Direction.E);
            map2.PerformActions(actions);
            map2.PerformActions(actions);
            map2.PerformActions(actions);*/
            Search search = new Search(new Astar(map));
            Map finalmap = solver(search, map);

            //map.GetHashCode();
            /*Map map = null;
            MapLoad.loadMap(args[0], out map);
            switch (args[1])
            {
                case "BFS":
                case "DFS":
                case "Astar":
                case "AWstar":
                case "Greedy":
                    break;
            }*/
            Map printmap = finalmap;
            while (true)
            {
                // Only prints a-boxes. This is only temporary testing though. It needs to input to the server.
                HashSet<Node> aBoxes = printmap.getBoxGroup('a');
                Actor[] actors = printmap.getActors();
                foreach (Actor actor in actors)
                {
                    System.Console.Write("Actor{0} Position: {1},{2}\n", actor.id, actor.x, actor.y);
                }
                int count = 1;
                foreach (Node box in aBoxes)
                {
                    System.Console.Write("Box{0} Position: {1},{2}\n", count, box.x, box.y);
                    count++;
                }
                System.Console.Write("Steps: {0}\n\n", printmap.steps);
                if (printmap.parent == null) { break; }
                else { printmap = printmap.parent; }
            }
            LinkedList<act[]> actionlist = restoreactions(finalmap);
            foreach(act[] actiongroup in actionlist)
            {
                string line = "[";
                for (int i = 0; i < actiongroup.Count()-1; i++)
                {
                    line = line + actiongroup[i].ToString();
                    line = line + ", ";
                }
                line = line + actiongroup[actiongroup.Count()-1].ToString();
                line = line + "]";
                System.Console.WriteLine(line);
            }
            System.Console.Write("pizza");
            
        }
        public static LinkedList<act[]> restoreactions(Map map)
        {
            LinkedList<act[]> actions;
            if (map.parent != null) { actions = restoreactions(map.parent); }
            else { actions = new LinkedList<act[]>(); return actions; }

            Actor[] actors = map.getActors();
            Actor[] parentactors = map.parent.getActors();
            
            int actorcount = actors.Count();
            act[] actiongroup = new act[actorcount];

            for (int i = 0; i< actorcount; i++)
            {
                if (actors[i].y < parentactors[i].y) { actiongroup[i] = new act(Interact.MOVE,Direction.N); }
                else if (actors[i].y > parentactors[i].y) { actiongroup[i] = new act(Interact.MOVE, Direction.S); }
                else if (actors[i].x < parentactors[i].x) { actiongroup[i] = new act(Interact.MOVE, Direction.E); }
                else if (actors[i].x > parentactors[i].x) { actiongroup[i] = new act(Interact.MOVE, Direction.W); }
                else { actiongroup[i] = new act(Interact.WAIT); }
                int box;
                if (map.parent.isBox(actors[i].x,actors[i].y,actors[i].getcolor(),out box))
                {
                    Node newbox = map.parent.getbox(box);
                    Node oldbox = map.getbox(box);
                    if (oldbox.y < newbox.y) { actiongroup[i] = new act(Interact.PUSH, actiongroup[i].dir, Direction.N, box); }
                    else if (oldbox.y > newbox.y) { actiongroup[i] = new act(Interact.PUSH, actiongroup[i].dir, Direction.S, box); }
                    else if (oldbox.x < newbox.x) { actiongroup[i] = new act(Interact.PUSH, actiongroup[i].dir, Direction.E, box); }
                    else if (oldbox.x > newbox.x) { actiongroup[i] = new act(Interact.PUSH, actiongroup[i].dir, Direction.W, box); }
                }
                else if (map.isBox(parentactors[i].x, parentactors[i].y, parentactors[i].getcolor(), out box))
                {
                    Node newbox = map.parent.getbox(box);
                    Node oldbox = map.getbox(box);
                    if (oldbox.y < newbox.y) { actiongroup[i] = new act(Interact.PULL, actiongroup[i].dir, Direction.N, box); }
                    else if (oldbox.y > newbox.y) { actiongroup[i] = new act(Interact.PULL, actiongroup[i].dir, Direction.S, box); }
                    else if (oldbox.x < newbox.x) { actiongroup[i] = new act(Interact.PULL, actiongroup[i].dir, Direction.E, box); }
                    else if (oldbox.x > newbox.x) { actiongroup[i] = new act(Interact.PULL, actiongroup[i].dir, Direction.W, box); }
                }
            }
            
            actions.AddLast(actiongroup);
            return actions;
        }
        public static Map solver(Search search, Map map)
        {
            search.addToFrontier(map);

            int i = -1;

            while (true)
            {
                i++;
                if (i == 1000)
                {
                    System.Console.Write("Explored: {0}\t Frontier: {1}\n", search.exploredSize(), search.frontierSize());
                    i = 0;
                }
                Map smap = search.getFromFrontier();
                if (smap.isGoal()) { return smap; }
                HashSet<act>[] actionlist = smap.getAllActions();
                foreach (HashSet<act> actorlist in actionlist)
                {
                    foreach (act action in actorlist)
                    {
                        Map nmap = new Map(smap);
                        act[] actions = new act[1];
                        actions[0] = action;
                        if (nmap.PerformActions(actions))
                        {
                            if (nmap.isGoal()) { return nmap; }
                            search.addToFrontier(nmap);
                        }
                    }
                }
            }
        }
    }
}
