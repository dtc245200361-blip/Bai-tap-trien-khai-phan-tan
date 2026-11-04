
using System.Collections.Concurrent;

class Program
{
    static async Task Main(string[] args)
    {
        string choice = args.FirstOrDefault() ?? "all";

        if (choice == "all")
        {
            await RunRequestResponse();
            await RunOneWay();
            await RunQueue();
            await RunPublishSubscribe();
            await RunStreaming();
        }
        else
        {
            switch (choice)
            {
                case "1": await RunRequestResponse(); break;
                case "2": await RunOneWay(); break;
                case "3": await RunQueue(); break;
                case "4": await RunPublishSubscribe(); break;
                case "5": await RunStreaming(); break;
                default: Console.WriteLine("Lua chon khong hop le!"); break;
            }
        }
    }

    // MEP 1: Request - Response
    static async Task RunRequestResponse()
    {
        Console.WriteLine("\n=== MEP 1: REQUEST - RESPONSE ===");

        Console.WriteLine("Client: Gui yeu cau den Server");

        await Task.Delay(500);

        Console.WriteLine("Server: Nhan yeu cau");
        Console.WriteLine("Server: Xu ly yeu cau");

        await Task.Delay(500);

        Console.WriteLine("Server: Tra ket qua");
        Console.WriteLine("Client: Nhan phan hoi");
    }

    // MEP 2: One-way
    static async Task RunOneWay()
    {
        Console.WriteLine("\n=== MEP 2: ONE-WAY ===");

        Console.WriteLine("Client: Gui thong diep");

        await Task.Delay(500);

        Console.WriteLine("Server: Da nhan thong diep");
        Console.WriteLine("Client: Khong cho phan hoi nghiep vu");
    }

    // MEP 3: Queue-based
    static async Task RunQueue()
    {
        Console.WriteLine("\n=== MEP 3: QUEUE-BASED ===");

        var queue = new ConcurrentQueue<string>();

        for (int i = 1; i <= 5; i++)
            queue.Enqueue($"Sub-{i}");

        async Task Worker(string name)
        {
            while (queue.TryDequeue(out string? job))
            {
                Console.WriteLine($"{name} dang xu ly {job}");

                await Task.Delay(500);

                Console.WriteLine($"{name} da xu ly xong {job}");
            }
        }

        await Task.WhenAll(
            Worker("Worker 1"),
            Worker("Worker 2")
        );
    }

    // MEP 4: Publish - Subscribe
    static async Task RunPublishSubscribe()
    {
        Console.WriteLine("\n=== MEP 4: PUBLISH - SUBSCRIBE ===");

        string message = "Co bai nop moi!";

        string[] services =
        {
            "Notification Service",
            "Email Service",
            "Statistics Service"
        };

        Console.WriteLine($"Publisher: {message}");

        foreach (string service in services)
        {
            await Task.Delay(300);
            Console.WriteLine($"{service}: Nhan su kien");
        }
    }

    // MEP 5: Streaming
    static async Task RunStreaming()
    {
        Console.WriteLine("\n=== MEP 5: STREAMING ===");

        await foreach (int data in GenerateData())
        {
            Console.WriteLine($"Nhan du lieu: {data}");
        }
    }

    static async IAsyncEnumerable<int> GenerateData()
    {
        for (int i = 1; i <= 5; i++)
        {
            await Task.Delay(500);
            yield return i;
        }
    }
}