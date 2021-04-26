using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSPro.CLArgs;
using Xunit;
using Xunit.Abstractions;



namespace XUnitTest
{
    public class SplitCommandLineTest
    {
        private readonly ITestOutputHelper _log;



        public SplitCommandLineTest(ITestOutputHelper log)
        {
            _log = log;
        }



        [Fact]
        public void Test01()
        {
            const string CMD_LINE = "Deploy /p1=\"2021\" /p3=\"abc def\" --p2=abd --p4=\"abd\" ";
            string[] args1 = Win32.CommandLineToArgs(CMD_LINE);

            string[] args = Helper.SplitCommandLine(CMD_LINE);
            Assert.NotEmpty(args);
            Assert.Equal(5, args.Length);
            Assert.Equal("Deploy", args[0]);
            Assert.Equal("/p1=2021", args[1]);
            Assert.Equal("/p3=abc def", args[2]);
            Assert.Equal("--p2=abd", args[3]);
            Assert.Equal("--p4=abd", args[4]);
        }
    }
}
