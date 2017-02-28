package searchclient;

import java.util.HashSet;
import java.util.HashMap;
import java.awt.Point;

public class ObjectPos {
	private HashMap<Integer, Point> points;
	private int _hash = 0;
	private boolean changed = false;
	private static int id = 0;

	public ObjectPos(int row, int col) {
		this.points = new HashMap<Integer, Point>(1);
		if (row >= 0 && col >= 0) 
			this.addPos(row, col);
	}

	public ObjectPos(HashMap<Integer, Point> plist) {
		this.points = plist;
	}

	public void addPos(int row, int col) {
		id++;
		points.put(id, new Point(row, col));
		changed = true;
	}

	public ObjectPos clone() {
		HashMap<Integer, Point> newpoints = new HashMap<Integer, Point>(this.points.size(), 1);
		for (HashMap.Entry<Integer, Point> ps : this.points.entrySet())
			newpoints.put(ps.getKey(), ps.getValue().getLocation());
		return new ObjectPos(newpoints);
	}

	public HashSet<Point> getPoints() {
		return new HashSet<Point>(this.points.values());
	}

	public HashMap<Integer, Point> getMap() {
		return this.points;
	}

	@Override
	public int hashCode() {
		if (this._hash == 0 || changed) {
			int x = 1;
			int y = 1;
			int wprod = 1;
			for (Point point : this.points.values()) {
				x = x * (point.x + 2);
				y = y * (point.y + 2);
				wprod = wprod + powx(point.x, 12) * powx(point.y, 2);
			}
			this._hash = 101 * x + 31 * y + 73 * wprod;
		}
		return this._hash;
	}

	public int powx(int in, int iter) {
		int out = in;
		for (int i = 0; i < iter; i++) {
			out = out * in;
		}
		return out;
	}

	@Override
	public boolean equals(Object obj) {
		if (this == obj)
			return true;
		if (obj == null)
			return false;
		if (this.getClass() != obj.getClass())
			return false;
		ObjectPos other = (ObjectPos) obj;
		if (!this.getPoints().equals(other.getPoints()))
			return false;
		return true;
	}

	public boolean containsAll(HashSet<Point> req) {
		return this.getPoints().containsAll(req);
	}

	public boolean contains(int row, int col) {
		return this.getPoints().contains(new Point(row, col));
	}

	public void moveBox(int oldrow, int oldcol, int newrow, int newcol) {
		for (Point point : this.getPoints()) {
			if (point.x == oldrow && point.y == oldcol) {
				point.setLocation(newrow, newcol);
				break;
			}
		}
		changed = true;
	}

	@Override
	public String toString() {
		return this.points.toString();
	}
}