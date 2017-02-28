package searchclient;

import java.util.ArrayList;
import java.util.HashMap;
import java.awt.Point;
import java.util.Collections;
import java.util.LinkedList;
import java.util.Random;

import searchclient.Command.Type;

public class Node {
	private static final Random RND = new Random(1);

	public int MAX_ROW;
	public int MAX_COL;

	public int agentRow;
	public int agentCol;
	private static long idIterator = 0;
	public long id;
	// Arrays are indexed from the top-left of the level, with first index being row and second being column.
	// Row 0: (0,0) (0,1) (0,2) (0,3) ...
	// Row 1: (1,0) (1,1) (1,2) (1,3) ...
	// Row 2: (2,0) (2,1) (2,2) (2,3) ...
	// ...
	// (Start in the top left corner, first go down, then go right)
	// E.g. this.walls[2] is an array of booleans having size MAX_COL.
	// this.walls[row][col] is true if there's a wall at (row, col)
	//

	public boolean[][] walls;
	public HashMap<Byte, ObjectPos> boxes;
	public HashMap<Byte, ObjectPos> goals;

	public Node parent;
	public Command action;

	private int g;
	
	private int _hash = 0;

	public Node(Node parent, int mrow, int mcol, int newg) {
		this.MAX_ROW = mrow;
		this.MAX_COL = mcol;
		this.parent = parent;
		this.g = newg;
		this.id = idIterator++;
	}

	public long getID(){
		return id;
	}
	public int g() {
		return this.g;
	}

	public boolean isInitialState() {
		return this.g == 0;
	}

	public boolean isGoalState() {
		for (HashMap.Entry<Byte, ObjectPos> goal : goals.entrySet()) {
			if (!this.boxes.get(goal.getKey()).containsAll(goal.getValue().getPoints())) {
				return false;
			}
		}
		return true;
	}

	public ArrayList<Node> getExpandedNodes() {
		ArrayList<Node> expandedNodes = new ArrayList<Node>(Command.EVERY.length);
		for (Command c : Command.EVERY) {
			// Determine applicability of action
			int newAgentRow = this.agentRow + Command.dirToRowChange(c.dir1);
			int newAgentCol = this.agentCol + Command.dirToColChange(c.dir1);

			if (c.actionType == Type.Move) {
				// Check if there's a wall or box on the cell to which the agent is moving
				if (this.cellIsFree(newAgentRow, newAgentCol)) {
					Node n = this.ChildNode(false);
					n.action = c;
					n.agentRow = newAgentRow;
					n.agentCol = newAgentCol;
					expandedNodes.add(n);
				}
			} else if (c.actionType == Type.Push) {
				// Make sure that there's actually a box to move
				ObjectPos thebox = this.boxAt(newAgentRow, newAgentCol);
				if (thebox != null) {
					int newBoxRow = newAgentRow + Command.dirToRowChange(c.dir2);
					int newBoxCol = newAgentCol + Command.dirToColChange(c.dir2);
					// .. and that new cell of box is free
					if (this.cellIsFree(newBoxRow, newBoxCol)) {
						Node n = this.ChildNode(true);
						ObjectPos nbox = n.boxAt(newAgentRow, newAgentCol);
						n.action = c;
						n.agentRow = newAgentRow;
						n.agentCol = newAgentCol;
						nbox.moveBox(newAgentRow, newAgentCol, newBoxRow, newBoxCol);
						expandedNodes.add(n);
					}
				}
			} else if (c.actionType == Type.Pull) {
				// Cell is free where agent is going
				if (this.cellIsFree(newAgentRow, newAgentCol)) {
					int boxRow = this.agentRow + Command.dirToRowChange(c.dir2);
					int boxCol = this.agentCol + Command.dirToColChange(c.dir2);
					// .. and there's a box in "dir2" of the agent
					ObjectPos thebox = this.boxAt(boxRow, boxCol);
					if (thebox != null) {
						Node n = this.ChildNode(true);
						ObjectPos nbox = n.boxAt(boxRow, boxCol);
						n.action = c;
						n.agentRow = newAgentRow;
						n.agentCol = newAgentCol;
						nbox.moveBox(boxRow, boxCol, this.agentRow, this.agentCol);
						expandedNodes.add(n);
					}
				}
			}
		}
		Collections.shuffle(expandedNodes, RND);
		return expandedNodes;
	}

	private boolean cellIsFree(int row, int col) {
		return !this.walls[row][col] && this.boxAt(row, col) == null;
	}

	public ObjectPos boxAt(int row, int col) {
		for (ObjectPos box : this.boxes.values()) {
			if (box.contains(row, col))
				return box;
		}
		return null;
	}

	private Node ChildNode(boolean boxMoved) {
		Node copy = new Node(this, MAX_ROW, MAX_COL, this.g + 1);
		if (boxMoved) {
			copy.boxes = new HashMap<Byte, ObjectPos>(this.boxes.size(), 1);
			for (HashMap.Entry<Byte, ObjectPos> box : this.boxes.entrySet()) {
				copy.boxes.put(box.getKey(), box.getValue().clone());
			}
		} else {
			copy.boxes = this.boxes;
		}
		copy.walls = this.walls;
		copy.goals = this.goals;
		return copy;
	}

	public LinkedList<Node> extractPlan() {
		LinkedList<Node> plan = new LinkedList<Node>();
		Node n = this;
		while (!n.isInitialState()) {
			plan.addFirst(n);
			n = n.parent;
		}
		return plan;
	}

	@Override
	public int hashCode() {
		if (this._hash == 0) {
			final int prime = 101;
			int result = 1;
			result = prime * result + this.agentCol;
			result = prime * result + this.agentRow;
			result = prime * result + this.boxes.hashCode();
			this._hash = result;
		}
		return this._hash;
	}

	@Override
	public boolean equals(Object obj) {
		if (this == obj)
			return true;
		if (obj == null)
			return false;
		if (this.getClass() != obj.getClass())
			return false;
		Node other = (Node) obj;
		if (this.agentRow != other.agentRow || this.agentCol != other.agentCol)
			return false;
		if (!other.boxes.equals(this.boxes))
			return false;
		return true;
	}

	@Override
	public String toString() {
		StringBuilder s = new StringBuilder();
		for (int row = 0; row < MAX_ROW; row++) {
			if (!this.walls[row][0]) {
				break;
			}
			for (int col = 0; col < MAX_COL; col++) {
				if (this.walls[row][col]) {
					s.append("+");
				} else if (row == this.agentRow && col == this.agentCol) {
					s.append("0");
				} else {
					s.append(" ");
				}
			}
			s.append("\n");
		}
		for (HashMap.Entry<Byte, ObjectPos> box : this.boxes.entrySet()) {
			for (Point pos : box.getValue().getPoints()) {
				s.setCharAt((int) pos.getX() + (MAX_COL + 1) * (int) pos.getY(), (char) Character.toUpperCase(box.getKey()));
			}
		}
		for (HashMap.Entry<Byte, ObjectPos> goal : this.goals.entrySet()) {
			for (Point pos : goal.getValue().getPoints()) {
				s.setCharAt((int) pos.getX() + (MAX_COL + 1) * (int) pos.getY(), (char) (goal.getKey() & 0xFF));
			}
		}
		return s.toString();
	}
}