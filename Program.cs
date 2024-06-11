using System.Threading;

class Program
{
    private static readonly object locker = new object();

    static void Main()
    {
        string filePathOut = "output.txt";

        File.WriteAllText(filePathOut, "");

        Thread thread1 = new Thread(new ParameterizedThreadStart(ReadAndWrite));
        Thread thread2 = new Thread(new ParameterizedThreadStart(ReadAndWrite));

        thread1.Start(new string[] { "TextDocumentThread1.txt", "output.txt" });
        thread2.Start(new string[] { "TextDocumentThread2.txt", "output.txt" });

        thread1.Join();
        thread2.Join();
        using (StreamReader reader = new StreamReader("output.txt"))
        {
            Console.WriteLine(reader.ReadToEnd());
        }
    }

    static void ReadAndWrite(object paths)
    {
        string[] filePaths = (string[])paths;
        string content = File.ReadAllText(filePaths[0]);

        lock (locker)
        {
            Console.WriteLine($"Поток {Thread.CurrentThread.ManagedThreadId} записывает данные...");
            File.AppendAllText(filePaths[1], content);
        }
    }
}
