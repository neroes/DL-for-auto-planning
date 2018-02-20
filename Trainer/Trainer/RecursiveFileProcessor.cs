// For Directory.GetFiles and Directory.GetDirectories
// For File.Exists, Directory.Exists
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trainer
{
    // Based on recusiveFileProecessor found at https://msdn.microsoft.com/en-us/library/c1sez4sc(v=vs.110).aspx
    // It finds all files and for each case it runs a search to find the goal state
    public class RecursiveFileProcessor
    {
        public static void RFPMain(string[] args)
        {
            foreach (string path in args)
            {
                if (File.Exists(path))
                {
                    // This path is a file
                    ProcessFile(path);
                }
                else if (Directory.Exists(path))
                {
                    // This path is a directory
                    ProcessDirectory(path);
                }
                else
                {
                    Console.WriteLine("{0} is not a valid file or directory.", path);
                }
            }
        }


        // Process all files in the directory passed in, recurse on any directories 
        // that are found, and process the files they contain.
        public static void ProcessDirectory(string targetDirectory)
        {
            // Process the list of files found in the directory.
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
                ProcessFile(fileName);

            // Recurse into subdirectories of this directory.
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
                ProcessDirectory(subdirectory);
        }

        // Insert logic for processing found files here.
        public static void ProcessFile(string path)
        {
            MLInput.mapName = path;
            System.Console.WriteLine(path);
            StreamReader sr = new StreamReader(path);
            //System.Console.WriteLine("1");

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            //System.Console.WriteLine("1");

            Map map = null;
            MapLoad.loadMap(sr, out map);
            //System.Console.WriteLine("1");

            

            Heuristic h = new BFS(map);

            Search search = new Search(h);

            Console.Error.WriteLine("Initialized after {0:0.000}", stopwatch.Elapsed.TotalSeconds);

            Map finalmap = solver(search, map, stopwatch);
            stopwatch.Stop();
            //System.Console.WriteLine("1");
            if (finalmap == null)
            {
                Console.Error.WriteLine("Frontier was emptied! No solution found. Explored: {0}", search.exploredSize());
            }
            else
            {
                Console.Error.WriteLine("Finished!");
                Console.Error.Write("Time: {0:0.000}\t Steps: {1}\t Explored: {2}\t Frontier: {3}\n\n", stopwatch.Elapsed.TotalSeconds, finalmap.steps, search.exploredSize() - search.frontierSize(), search.frontierSize());
                MLInput mlin = new MLInput(map);
                mlin.run(finalmap.steps);
                /*
                Map printmap = finalmap;
                while (true) // Outputs posistions for debugging.
                {
                    Node[] aBoxes = printmap.getAllBoxes();
                    Actor[] actors = printmap.getActors();
                    foreach (Actor actor in actors)
                    {
                        Console.Error.Write("Act{0} Pos: {1},{2}\n", actor.id, actor.x, actor.y);
                    }
                    int count = 1;
                    foreach (Node box in aBoxes)
                    {
                        Console.Error.Write("Box{0} Pos: {1},{2}\n", count, box.x, box.y);
                        count++;
                    }
                    Console.Error.Write("Steps: {0}\n\n", printmap.steps);
                    if (printmap.parent == null) { break; }
                    else { printmap = printmap.parent; }
                }*/
                LinkedList<act[]> actionlist = restoreactions(finalmap);
                foreach (act[] actiongroup in actionlist)
                {
                    string line = "[";
                    for (int i = 0; i < actiongroup.Count() - 1; i++)
                    {
                        line = line + actiongroup[i].ToString();
                        line = line + ", ";
                    }
                    line = line + actiongroup[actiongroup.Count() - 1].ToString();
                    line = line + "]";
                    // Console.Error.WriteLine(line); // Debug.
                    //System.Console.WriteLine(line);
                }
            }
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

            for (int i = 0; i < actorcount; i++)
            {
                if (actors[i].y < parentactors[i].y) { actiongroup[i] = new act(Interact.MOVE, Direction.N); }
                else if (actors[i].y > parentactors[i].y) { actiongroup[i] = new act(Interact.MOVE, Direction.S); }
                else if (actors[i].x < parentactors[i].x) { actiongroup[i] = new act(Interact.MOVE, Direction.W); }
                else if (actors[i].x > parentactors[i].x) { actiongroup[i] = new act(Interact.MOVE, Direction.E); }
                else { actiongroup[i] = new act(Interact.WAIT); }
                int box;
                if (map.parent.isBox(actors[i].x, actors[i].y, actors[i].getcolor(), out box))
                {
                    Node newbox = map.parent.getbox(box);
                    Node oldbox = map.getbox(box);
                    if (oldbox.y < newbox.y) { actiongroup[i] = new act(Interact.PUSH, actiongroup[i].dir, Direction.N, box); }
                    else if (oldbox.y > newbox.y) { actiongroup[i] = new act(Interact.PUSH, actiongroup[i].dir, Direction.S, box); }
                    else if (oldbox.x < newbox.x) { actiongroup[i] = new act(Interact.PUSH, actiongroup[i].dir, Direction.W, box); }
                    else if (oldbox.x > newbox.x) { actiongroup[i] = new act(Interact.PUSH, actiongroup[i].dir, Direction.E, box); }
                }
                else if (map.isBox(parentactors[i].x, parentactors[i].y, parentactors[i].getcolor(), out box))
                {
                    Node newbox = map.parent.getbox(box);
                    Node oldbox = map.getbox(box);

                    // Opposite directions for pull. It's what the server wants!
                    if (oldbox.y < newbox.y) { actiongroup[i] = new act(Interact.PULL, actiongroup[i].dir, Direction.S, box); }
                    else if (oldbox.y > newbox.y) { actiongroup[i] = new act(Interact.PULL, actiongroup[i].dir, Direction.N, box); }
                    else if (oldbox.x < newbox.x) { actiongroup[i] = new act(Interact.PULL, actiongroup[i].dir, Direction.E, box); }
                    else if (oldbox.x > newbox.x) { actiongroup[i] = new act(Interact.PULL, actiongroup[i].dir, Direction.W, box); }

                }
            }

            actions.AddLast(actiongroup);
            return actions;
        }
        public static Map solver(Search search, Map map, Stopwatch stopwatch)
        {
            search.addToFrontier(map);

            int i = -1;

            while (search.frontierSize() > 0)
            {
                i++;
                if (i == 10000)
                {
                    Console.Error.Write("Time:  {0:0.000}\t Explored: {1}\t Frontier: {2}\t Current steps: {3}\n", stopwatch.Elapsed.TotalSeconds, search.exploredSize() - search.frontierSize(), search.frontierSize(), search.currentSteps());
                    i = 0;
                }
                Map smap = search.getFromFrontier();
                if (smap.isGoal()) { return smap; }

                HashSet<act>[] actionlist = smap.getAllActions();


                IEnumerator<act>[] enumerators = new IEnumerator<act>[actionlist.Count()];
                for (int j = 0; j < actionlist.Count(); j++)
                {
                    enumerators[j] = actionlist[j].GetEnumerator();
                    enumerators[j].MoveNext();
                }
                bool run = true;
                while (run)
                {
                    act[] actions = new act[enumerators.Count()];
                    for (int j = 0; j < enumerators.Count(); j++)
                    {
                        actions[j] = enumerators[j].Current;
                    }
                    int k = 0;
                    while (!enumerators[k].MoveNext())
                    {
                        enumerators[k].Reset();
                        enumerators[k].MoveNext();
                        k++;
                        if (k == enumerators.Count()) { run = false; break; }
                    }
                    Map nmap = new Map(smap);
                    if (nmap.PerformActions(actions))
                    {
                        if (nmap.isGoal()) { return nmap; }
                        search.addToFrontier(nmap);
                    }
                }


                /*foreach (HashSet<act> actorlist in actionlist)
                {
                    foreach (act action in actorlist)
                    {
                        
                        act[] actions = new act[1];
                        actions[0] = action;
                        if (nmap.PerformActions(actions))
                        {
                            if (nmap.isGoal()) { return nmap; }
                            search.addToFrontier(nmap);
                        }
                    }
                }*/
            }
            return null;
        }
    }
}
