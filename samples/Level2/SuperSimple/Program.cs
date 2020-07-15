using System;
using MSPro.CLArgs;
using SuperSimple;



namespace SuperSimple
{
    class Program
    {
        static void Main(string[] args)
        {
            Arguments arguments = CommandLine.Parse(args);
            Console.WriteLine($"Command-Line: {arguments.CommandLine}");
            ICommand cmd = new MyConverter();
            cmd.Execute(arguments);
        }
    }

    // Todo: Support for OptionName and Tag: ("fileName", "fn")

    class MyParams
    {
        [OptionDescriptor("fileName",  Required = true)]
        public string FileName { get; set; }

        [OptionDescriptor("target", Required = false, Default = "JSON")]
        public string Target { get; set; }
    }


    // MyConverter implements the functionality 
    // and it gets the arguments from the command-line as a typed object.
    class MyConverter : CommandBase<MyParams>
    {
        protected override void OnExecute(MyParams p)
        {
            Console.WriteLine(">>> Start Functionality");
            Console.WriteLine($"fileName='{p.FileName}");
            Console.WriteLine($"target='{p.Target}");
            Console.WriteLine("<<< End Functionality");
        }
    }
}
