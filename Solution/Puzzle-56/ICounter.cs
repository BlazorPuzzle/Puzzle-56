namespace Puzzle_56;

public interface ICounter
{
	int Count { get; }
	void Increment();
}

public class TheCounter : ICounter
{
	private int _count;
	public int Count => _count;
	public void Increment() => _count++;
}

public class NullCounter : ICounter
{
	public int Count => 0;
	public void Increment() { }
}
