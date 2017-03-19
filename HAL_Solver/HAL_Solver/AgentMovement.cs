using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace HAL_Solver
{
    static class AgentMovement
    {
        private enum Actions { Push, Move, Pull }
        private enum Dir { N, S, E, W}

        private static void getXY(Dir dir, out int x, out int y)
        {
            x = 0;
            y = 0;
            switch(dir)
            {
                case Dir.N:
                    y = 1;
                    break;
                case Dir.S:
                    y = -1;
                    break;
                case Dir.E:
                    x = 1;
                    break;
                case Dir.W:
                    x = -1;
                    break;
            }
        }

        public static List<string> getActions(byte id, Map currentMap)
        {
            byte color = currentMap.getColorOfAgent(id);
            byte[] pos = currentMap.getAgentPos(id);
            int x = pos[0];
            int y = pos[1];
            int px, py, bx, by, npx, npy;
            List<string> actionList = new List<string>();
            Array actions = Enum.GetValues(typeof(Actions));
            Array dirs = Enum.GetValues(typeof(Dir));
            actionList.Add("NoOp");
            foreach (Action action in actions)
            {
                if (action.Equals(Actions.Move))
                {
                    foreach (Dir dir in dirs)
                    {
                        getXY(dir, out px, out py);
                        if (currentMap.isEmptySpace(x + px, y + py)) {
                            actionList.Add(string.Format("Move({0})", dir.ToString()));
                            // Map stuff.
                        }
                    }
                } else {
                    foreach (Dir dir1 in dirs)
                    {
                        getXY(dir1, out px, out py);
                        npx = x + px;
                        npy = y + py;
                        if (action.Equals(Actions.Push))
                        {
                            if (currentMap.isBox(npx, npy, color))
                            {
                                foreach (Dir dir2 in dirs)
                                {
                                    getXY(dir2, out bx, out by);
                                    if (currentMap.isEmptySpace(bx + npx, by + npy)) {
                                        actionList.Add(string.Format("Push({0},{1})",
                                            dir1.ToString(), dir2.ToString()));
                                        // Map stuff
                                    }
                                }
                            }
                        } else // If pull
                        {
                            if (!currentMap.isEmptySpace(npx, npy))
                            {
                                foreach (Dir dir2 in dirs)
                                {
                                    getXY(dir2, out bx, out by);
                                    if (currentMap.isBox(px + bx, py + by, color))
                                    {
                                        actionList.Add(string.Format("Pull({0},{1})",
                                        dir1.ToString(), dir2.ToString()));
                                        // Map stuff
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return actionList;
        }
    }
}
