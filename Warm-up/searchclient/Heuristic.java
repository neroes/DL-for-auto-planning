package searchclient;

import java.util.Comparator;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.HashSet;
import java.awt.Point;

import searchclient.NotImplementedException;

public abstract class Heuristic implements Comparator<Node> {
	HashMap<Byte, ObjectPos> goals;
	HashMap<Integer, Point> combi;

	public Heuristic(Node initialState) {
		this.goals = initialState.goals;

		this.initClosest(initialState);
	}

	private void initClosest(Node n) {
		this.combi = new HashMap<Integer, Point>();
		for (HashMap.Entry<Byte, ObjectPos> goal : this.goals.entrySet()) {
			HashSet<Point> gps = new HashSet<Point>(goal.getValue().getPoints());
			HashMap<Integer, Point> bps = new HashMap<Integer, Point>(n.boxes.get(goal.getKey()).getMap());
			HashMap<Integer, Point> boxpos = n.boxes.get(goal.getKey()).getMap();
			HashSet<Point> redo = new HashSet<Point>();
			do {
				for (Point gp : gps) {
					int closest = shortestDist(bps, gp);
					Point closestp = bps.get(closest);
					int dist = this.distance(closestp, gp);
					if (this.combi.containsKey(closest)) {
						Point gclosest = this.combi.get(closest);
						if (dist < this.distance(closestp, gclosest)) {
							redo.add(gclosest);
							this.combi.put(closest, gp);
							redo.remove(gp);
						} else {
							redo.add(gp);
						}
					} else {
						redo.remove(gp);
						this.combi.put(closest, gp);
					}
				}
				bps.keySet().removeAll(this.combi.keySet());
				gps.clear();
				gps.addAll(redo);
			} while(redo.size() > 0);
		}
	}

	private int combiDist(Node n, boolean aDist, boolean avgADist) {
		int dist = 0;
		int aDistance = 0;
		int aDistMult = 1;
		if (aDist && !avgADist)
			aDistance = Integer.MAX_VALUE;
		Point aPoint = new Point(n.agentRow, n.agentCol);
		for (HashMap.Entry<Byte, ObjectPos> goal : this.goals.entrySet()) {
			HashMap<Integer, Point> bps = new HashMap<Integer, Point>(n.boxes.get(goal.getKey()).getMap());
			for (HashMap.Entry<Integer, Point> bp : bps.entrySet()) {
				int bk = bp.getKey();
				Point bv = bp.getValue();
				if (this.combi.containsKey(bk)) {
					Point gp = this.combi.get(bk);
					int tdist = this.distance(bv, gp);
					dist = dist + tdist;
					if (aDist && tdist > 0)
						if (avgADist) {
							aDistance = aDistance + this.distance(bv, aPoint);
							aDistMult++;
						}
						else
							aDistance = Math.min(aDistance, this.distance(bv, aPoint));
				}			
			}
		}
		return dist + aDistance / aDistMult;
	}

	private int totalDist(Node n) {
		int dist = 0;
		for (HashMap.Entry<Byte, ObjectPos> goal : this.goals.entrySet()) {
			HashSet<Point> bps = new HashSet<Point>(n.boxes.get(goal.getKey()).getPoints());
			HashSet<Point> gps = new HashSet<Point>(goal.getValue().getPoints());
			for (Point bp : bps) {
				for (Point gp : gps) {
					dist = dist + this.distance(gp, bp);
				}		
			}
		}
		return dist;
	}

	public int notInGoal(Node n) {
		int i = 0;
		for (HashMap.Entry<Byte, ObjectPos> goal : this.goals.entrySet()) {
			if (!n.boxes.get(goal.getKey()).containsAll(goal.getValue().getPoints())) {
				i++;
			}
		}
		return i;
	}

	private int shortestDist(HashSet<Point> points, Point comp, boolean remove){
		int minDist = Integer.MAX_VALUE;
		Point minbox = null;
		for (Point point : points) {
			int dist = this.distance(point, comp);
			if (dist < minDist) {
				minDist = dist;
				if (remove)
					minbox = point;
			}
		}
		if (remove)
			points.remove(minbox);
		return minDist;
	}

	private int shortestDist(HashMap<Integer, Point> points, Point comp){
		int minDist = Integer.MAX_VALUE;
		int minbox = 0;
		for (HashMap.Entry<Integer, Point> point : points.entrySet()) {
			int dist = this.distance(point.getValue(), comp);
			if (dist < minDist) {
				minDist = dist;
				minbox = point.getKey();
			}
		}
		return minbox;
	}

	private Point shortestDist(HashSet<Point> points, Point comp, boolean remove, Point aPoint){
		int minDist = Integer.MAX_VALUE;
		Point minbox = null;
		int aDist = 0;
		for (Point point : points) {
			int dist = this.distance(point, comp);
			if (dist < minDist) {
				minDist = dist;
				minbox = point;
			}
		}
		points.remove(minbox);
		return minbox;
	}

	private int distance(Point a, Point b) {
		return Math.abs(a.x - b.x) + Math.abs(a.y - b.y);
	}

	private int minBoxDist(Node n, boolean aDist) {
		int dist = 0;
		Point aPoint = new Point(n.agentRow, n.agentCol);
		int aDistance = 0;
		for (HashMap.Entry<Byte, ObjectPos> goal : this.goals.entrySet()) {
			int tdist = 0;
			HashSet<Point> gps = new HashSet<Point>(goal.getValue().getPoints());
			HashSet<Point> bps = new HashSet<Point>(n.boxes.get(goal.getKey()).getPoints());
			gps.removeAll(bps);
			bps.removeAll(goal.getValue().getPoints());
			byte distMult = 1;
			if (gps.size() == bps.size() && gps.size() > 1) {
				distMult = 2;
				for (Point bp : bps) {
					int sdist = this.shortestDist(gps, bp, true);
					tdist = tdist + sdist;
				}
			}
			if (gps.size() > 0 && aDist)
				aDistance = Integer.MAX_VALUE;
			for (Point gp : gps) {
				int sdist = 0;
				if (aDist) {
					Point cbox = this.shortestDist(bps, gp, true, aPoint);
					tdist = tdist + this.distance(cbox, gp);
					aDistance = Math.min(aDistance, this.distance(aPoint, cbox));
				}
				else
					sdist = this.shortestDist(bps, gp, true);
				tdist = tdist + sdist;
			}
			dist = dist + tdist / distMult;
		}
		return dist + aDistance;
	}

	public int h(Node n) {
		//return this.notInGoal(n); // Kind of bad.
		//return this.minBoxDist(n, true); // Fastest which is always optimal with astar.
		//return this.totalDist(n); // Really bad.
		//return this.combiDist(n, true, false); // Fastest which is also optimal with astar almost all the time.
		//return this.combiDist(n, false, false); // Very fast but suboptimal with many states before reaching closest box eg. SAD2.lvl
		return this.combiDist(n, true, true); // Best with greedy most of the time.
	}

	public abstract int f(Node n);

	@Override
	public int compare(Node n1, Node n2) {
		int res = Integer.compare(this.f(n1), this.f(n2));
		if (res == 0)
			res = Long.compare(n1.getID(), n2.getID());
		return res;
	}

	public static class AStar extends Heuristic {
		public AStar(Node initialState) {
			super(initialState);
		}

		@Override
		public int f(Node n) {
			return n.g() + this.h(n);
		}

		@Override
		public String toString() {
			return "A* evaluation";
		}
	}

	public static class WeightedAStar extends Heuristic {
		private int W;

		public WeightedAStar(Node initialState, int W) {
			super(initialState);
			this.W = W;
		}

		@Override
		public int f(Node n) {
			return n.g() + this.W * this.h(n);
		}

		@Override
		public String toString() {
			return String.format("WA*(%d) evaluation", this.W);
		}
	}

	public static class Greedy extends Heuristic {
		public Greedy(Node initialState) {
			super(initialState);
		}

		@Override
		public int f(Node n) {
			return this.h(n);
		}

		@Override
		public String toString() {
			return "Greedy evaluation";
		}
	}
}
