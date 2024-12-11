namespace Console
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            System.Console.Title = "DevOnBike console of doom";

            var token = CancellationToken.None;

            await Task.CompletedTask;
        }

    }
}