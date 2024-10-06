namespace Console
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            System.Console.Title = "Marcin's challenge";

            var token = CancellationToken.None;

            await Task.CompletedTask;
        }

    }
}