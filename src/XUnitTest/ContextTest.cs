using System.Collections.Generic;
using JetBrains.Annotations;
using MSPro.CLArgs;
using Xunit;
using Xunit.Abstractions;



namespace XUnitTest
{
    public class ContextTest
    {
        private readonly ITestOutputHelper _log;



        public ContextTest(ITestOutputHelper log)
        {
            _log = log;
        }



        [Fact]
        public void Test1()
        {
            const string CMD_LINE = "DEPLOY /Package=\"Sprint 03\" --dst-env 01-DEV --DST-env 02-DEV /flag /note:\"Fixed API\"  \"  Another target\"";
            string[] args = Win32.CommandLineToArgs(CMD_LINE);

            //
            // Define CLArgs behavior
            //
            Settings settings = new () {IgnoreCase=true };

            var clArgs = CommandLineParser.Parse(args, settings);

            //
            // A list of option descriptors is required to convert arguments.
            // The option descriptors determine how each option is handled.
            // Now, that we do have a 'Context' we can simply get the option descriptors
            // from the context's attributes.
            //
            OptionDescriptorFromTypeProvider<Context01> optionDescriptorProvider = new();
            var optionDescriptors = optionDescriptorProvider.Get();

            // 
            // Having the option descriptors, we can convert the arguments 
            // a create a Context01 out of it.
            //
            ArgumentConverter<Context01> argumentConverter = new(settings);
            var errors = argumentConverter.TryConvert(
                clArgs, optionDescriptors,
                out Context01 executionContext,
                out HashSet<string> unresolvedPropertyNames);

            Assert.False(errors.HasErrors(), errors.ToString());

            Assert.Equal("Sprint 03", executionContext.Package);
            Assert.True( executionContext.Flag);
            Assert.Equal(2, executionContext.DstEnvironments.Count);
            Assert.Equal("01-DEV", executionContext.DstEnvironments[0]);
            Assert.Equal("02-DEV", executionContext.DstEnvironments[1]);

            Assert.NotEmpty(executionContext.Targets);
            Assert.Equal("  Another target", executionContext.Targets[0]);
        }



        private class Context01
        {
            [OptionDescriptor("package")]
            public string Package { get; set; }

            [OptionDescriptor("Note")]
            public string Note { get; set; }

            [OptionDescriptor("Flag")]
            public bool Flag { get; set; }

            [OptionDescriptor("dst-env", AllowMultiple = nameof(DstEnvironments))]
            [UsedImplicitly]
            public string DstEnvironment { get; set; }


            public List<string> DstEnvironments { get; } = new();

            [Targets]
            [UsedImplicitly]
            public List<string> Targets { get;  }= new();
        }
    }
}