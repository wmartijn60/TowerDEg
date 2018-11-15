/// <summary>
/// A Point with x and y coordinate.
/// </summary>
public struct Point{
	
	/// <summary>
	/// Gets or sets the x.
	/// </summary>
	/// <value>The x.</value>
	public int X { get; set; }

	/// <summary>
	/// Gets or sets the y.
	/// </summary>
	/// <value>The y.</value>
	public int Y { get; set; }

	/// <summary>
	/// Initializes a new instance of the <see cref="Point"/> struct.
	/// </summary>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	public Point(int x, int y){
		this.X = x;
		this.Y = y;
	}

	public static bool operator ==(Point p1, Point p2){
		return p1.X == p2.X && p1.Y == p2.Y;
	}

	public static bool operator !=(Point p1, Point p2){
		return p1.X != p2.X || p1.Y != p2.Y;
	}

	public static Point operator -(Point p1, Point p2){
		return new Point (p1.X - p2.X, p1.Y - p2.Y);
	}
}
