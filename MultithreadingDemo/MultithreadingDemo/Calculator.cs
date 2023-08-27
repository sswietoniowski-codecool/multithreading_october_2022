namespace MultithreadingDemo;

public class Calculator
{
    private readonly object _lock = new object();

    public int Add(int a, int b)
    {
        return a + b;
    }

    public int AddSplitWork(int a, int b)
    {
        var sum = a;
        var (counter, increment) = b >= 0 ? (b, 1) : (-b, -1);

        for (int i = 0; i < counter; i++)
        {
            sum += increment;
        }

        return sum;
    }

    public int AddMultithreadedWithRaceCondition(int a, int b)
    {
        var sum = a;
        var (counter, increment) = b >= 0 ? (b, 1) : (-b, -1);
        var threads = new List<Thread>();

        for (int i = 0; i < counter; i++)
        {
            // create a new thread for each increment
            var thread = new Thread(() =>
            {
                var result = sum;
                // simulate some work
                Thread.Sleep(new Random().Next(1, 10)); // 1-9 ms
                result += increment;
                sum = result;
            });
            thread.Start();
            threads.Add(thread);
        }

        // wait for all threads to finish
        foreach (var thread in threads)
        {
            thread.Join();
        }

        return sum;
    }

    public int AddMultithreadedAtomic(int a, int b)
    {
        var sum = a;
        var (counter, increment) = b >= 0 ? (b, 1) : (-b, -1);
        var threads = new List<Thread>();

        for (int i = 0; i < counter; i++)
        {
            // create a new thread for each increment
            var thread = new Thread(() =>
            {
                // simulate some work
                Thread.Sleep(new Random().Next(1, 10)); // 1-9 ms

                //sum += increment;

                Interlocked.Add(ref sum, increment);
            });
            thread.Start();
            threads.Add(thread);
        }

        // wait for all threads to finish
        foreach (var thread in threads)
        {
            thread.Join();
        }

        return sum;
    }

    public int AddMultithreadedLock(int a, int b)
    {
        var sum = a;
        var (counter, increment) = b >= 0 ? (b, 1) : (-b, -1);
        var threads = new List<Thread>();

        for (int i = 0; i < counter; i++)
        {
            // create a new thread for each increment
            var thread = new Thread(() =>
            {
                // simulate some work
                Thread.Sleep(new Random().Next(1, 10)); // 1-9 ms

                // lock the critical section
                lock (_lock)
                {
                    sum += increment; // critical section    
                }
            });
            thread.Start();
            threads.Add(thread);
        }

        // wait for all threads to finish
        foreach (var thread in threads)
        {
            thread.Join();
        }

        return sum;
    }

    public int AddMultithreadedMutex(int a, int b)
    {
        var sum = a;
        var (counter, increment) = b >= 0 ? (b, 1) : (-b, -1);
        var threads = new List<Thread>();

        using var mutex = new Mutex();

        for (int i = 0; i < counter; i++)
        {
            // create a new thread for each increment
            var thread = new Thread(() =>
            {
                // simulate some work
                Thread.Sleep(new Random().Next(1, 10)); // 1-9 ms

                mutex.WaitOne();
                sum += increment; // critical section    
                mutex.ReleaseMutex();
            });
            thread.Start();
            threads.Add(thread);
        }

        // wait for all threads to finish
        foreach (var thread in threads)
        {
            thread.Join();
        }

        return sum;
    }
}