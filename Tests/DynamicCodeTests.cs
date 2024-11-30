using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.Scripting.Hosting;
using Xunit.Abstractions;

namespace Tests
{
    /// <summary>
    /// https://github.com/dotnet/roslyn/blob/main/docs/wiki/Scripting-API-Samples.md
    /// </summary>
    public class DynamicCodeTests
    {
        [Fact]
        public async Task CompileTest()
        {
            // arrange
            var scriptOptions = ScriptOptions.Default
                .WithImports("System", "System.Diagnostics");

            var code = File.ReadAllText("DynamicCode.script");
            var script = CSharpScript.Create(code, scriptOptions);

            // act
            var compilation = script.Compile();

            // assert
            Assert.True(compilation.IsEmpty);

            var runner = await script.RunAsync();

            Assert.Null(runner.Exception);
        }

        [Fact]
        public async Task RunWithVariables()
        {
            var state = await CSharpScript.RunAsync<int>("int c = 1 + 2;");
            var value = state.Variables[0].Value;
        }

        [Fact]
        public async Task Compilation1()
        {
            using var loader = new InteractiveAssemblyLoader();

            var script = CSharpScript.Create<int>("1", assemblyLoader: loader);
            var state = await script.RunAsync();
            var returnValue = state.ReturnValue;
        }


        private readonly ITestOutputHelper _output;

        public DynamicCodeTests(ITestOutputHelper output)
        {
            _output = output;
        }
    }
}