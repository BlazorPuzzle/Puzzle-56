# Blazor Puzzle #56

## The Case of the Curious Counter

YouTube Video: https://youtu.be/jKiQUx5xH4Y

Blazor Puzzle Home Page: https://blazorpuzzle.com

### The Challenge:

This is a simple application that demonstrates the use of a counter.

To see the counter in action, navigate to the counter page.

There's something curious about this counter though. For some reason, even though it's using the MyCounter field, it isn't actually incrementing and reflecting the entries in the counter. Your job, detectives, is to find out why my counter won't increment.

### The Solution:

There were two red herrings in this puzzle.

First, take a look at the interfaces in *ICounter.cs*:

```c#
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
```

Notice that there is a NullCounter implementation that does nothing.

Now look at line 10 in *Program.cs*:

```c#
builder.Services.AddScoped<ICounter, NullCounter>();
```

You might think that's it. But it's a red herring.

Look at *Counter.razor*:

```c#
@page "/counter"

<PageTitle>Counter</PageTitle>

<h1>Counter</h1>

<p role="status">Current count: @MyCounter.Count</p>

<button class="btn btn-primary" @onclick="IncrementCount">Click me</button>

@code {

	private ICounter MyCountеr = new TheCounter();

	private void IncrementCount()
	{
		MyCounter.Increment();
	}
}
```

We are not using an injected service here. We are instantiating a new `MyCounter`, which is not the one that does nothing.

However, look at this line in *_Imports.razor*:

```c#
@inject ICounter MyCounter
```

So far we have an injected `NullCounter` and an injected `MyCounter`, and yet we are using neither of them.

The REAL problem is on line 13 of *Counter.razor*:

```c#
private ICounter MyCountеr = new TheCounter();
```

Here's the secret: The "e" in the variable name `MyCounter` is not an ASCII e.

Go ahead. Inspect it. Double click on the variable name and you'll see it only selects `MyCount`

So, that is NOT the same `MyCounter` that was injected.

To fix the problem, replace the  e in the `MyCounter` variable and remove the injected version from *_Imports.razor*.

Boom!
