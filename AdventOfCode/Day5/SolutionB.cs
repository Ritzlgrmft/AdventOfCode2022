using System;
namespace AdventOfCode.Day5
{
	public class SolutionB
	{
		public static void DoWork()
		{
			var stacks = new Stack<char>[9];
			stacks[0] = initStack("FDBZTJRN");
			stacks[1] = initStack("RSNJH");
			stacks[2] = initStack("CRNJGZFQ");
			stacks[3] = initStack("FVNGRTQ");
			stacks[4] = initStack("LTQF");
			stacks[5] = initStack("QCWZBRGN");
			stacks[6] = initStack("FCLSNHM");
			stacks[7] = initStack("DNQMTJ");
			stacks[8] = initStack("PGS");


			foreach (string line in System.IO.File.ReadLines(@"../../../Day5/Input.txt"))
			{
				var lineParts = line.Split(' ');
				var count = int.Parse(lineParts[1]);
				var from = int.Parse(lineParts[3]) - 1;
				var to = int.Parse(lineParts[5]) - 1;

				var buffer = new char[count];
				for (int i = 0; i < count; i++)
				{
					buffer[i] = stacks[from].Pop();
				}
				for (int i = count - 1; i >= 0; i--)
				{
					stacks[to].Push(buffer[i]);
				}
			}

			foreach (var stack in stacks)
			{
				Console.Write(stack.Peek());
			}
			Console.WriteLine();
		}

		static Stack<char> initStack(String values)
		{
			var stack = new Stack<char>();
			foreach (var c in values.ToCharArray())
			{
				stack.Push(c);
			}
			return stack;
		}

		class Stack<T>
		{
			static readonly int MAX = 1000;
			int top;
			T[] stack = new T[MAX];

			public Stack()
			{
				top = -1;
			}

			bool IsEmpty()
			{
				return (top < 0);
			}

			internal bool Push(T data)
			{
				if (top >= MAX)
				{
					throw new Exception("Stack Overflow");
				}
				else
				{
					stack[++top] = data;
					return true;
				}
			}

			internal T Pop()
			{
				if (top < 0)
				{
					throw new Exception("Stack Underflow");
				}
				else
				{
					T value = stack[top--];
					return value;
				}
			}

			internal T Peek()
			{
				if (top < 0)
				{
					throw new Exception("Stack Underflow");
				}
				else
				{
					T value = stack[top];
					return value;
				}
			}

		}

	}
}