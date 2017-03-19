using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Drawing;

namespace HAL_Solver
{
    class Map
    {
        private static bool[] wallMap;
        private static byte height;
        private static byte width;

        private static Collection<byte>[] boxesOfColor;
        private static byte[] colorOfBox;
        private static Collection<byte>[] agentsOfColor;
        private static byte[] colorOfAgent;

        private static byte maxAgent;
        private static Collection<byte> availableAgents;
        private static Collection<byte> availableBoxes;
        private static Collection<byte> availableGoals;

        static Collection<byte[]>[] goals;
        Collection<byte[]>[] boxes;
        byte[][] agents;

        public Map(bool[] newwallMap, byte[][] newagents,
            Collection<byte[]>[] newboxes, Collection<byte[]>[] newgoals,
            Collection<byte> aa, Collection<byte> ab, Collection<byte> ag,
            Collection<byte>[] boc, byte[] cob, Collection<byte>[] aoc,
            byte[] coa, byte h, byte w)
        {
            wallMap = newwallMap;
            height = h;
            width = w;

            boxesOfColor = boc;
            colorOfBox = cob;
            agentsOfColor = aoc;
            colorOfAgent = coa;

            maxAgent = aa.Max();
            availableAgents = aa;
            availableBoxes = ab;
            availableGoals = ag;

            goals = newgoals;
            boxes = newboxes;
            agents = newagents;
        }

        public Map(byte[][] newagents, Collection<byte[]>[] newboxes)
        {
            boxes = newboxes;
            agents = newagents;
        }

        public string[] getMovements()
        {
            List<string> possibleMoves = new List<string>();
            string[] moves = Enumerable.Repeat(string.Empty, maxAgent).ToArray();

            foreach (byte agent in availableAgents)
            {
                possibleMoves = AgentMovement.getActions(agent, this);

                // Some heuristic stuff to isolate moves, otherwise we get a billion nodes.

                moves[agent] = possibleMoves[0]; // Should be something found obviously.
            }
            return moves;
        }

        internal bool isBox(byte[] pos, byte color)
        {
            Collection<byte> checklist = boxesOfColor[color];
            foreach (byte n in checklist)
            {
                foreach (byte[] box in boxes[n])
                {
                    if (box[0] == pos[0] && box[1] == pos[1]) { return true; }
                }
            }
            return false;
        }

        internal bool isBox(int x, int y, byte color)
        {
            return isBox(new byte[2] { (byte)x, (byte)y });
        }

        internal bool isBox(byte[] pos)
        {
            foreach (byte n in availableBoxes)
            {
                foreach (byte[] box in boxes[n])
                {
                    if (box[0] == pos[0] && box[1] == pos[1]) { return true; }
                }
            }
            return false;
        }

        internal bool isAgent(byte[] pos)
        {
            foreach (byte n in availableAgents)
            {
                byte[] agentPos = agents[n];
                if (agentPos[0] == pos[0] && agentPos[1] == pos[1]) { return true; }
            }
            return false;
        }

        internal bool isEmptySpace(byte[] pos)
        {
            if (isWall(pos)) { return false; }
            if (isBox(pos)) { return false; }
            if (isAgent(pos)) { return false; }
            return true;
        }

        internal bool isEmptySpace(int x, int y)
        {
            return isEmptySpace(new byte[2] { (byte)x, (byte)y });
        }

        public bool isWall(byte[] pos) { return wallMap[pos[0] + pos[1] * width]; }
        public bool isWall(int x, int y) { return wallMap[x + y * width]; }

        public byte getColorOfAgent(byte id) { return colorOfAgent[id]; }
        public byte[] getAgentPos(byte id) { return agents[id]; }
    }
}
