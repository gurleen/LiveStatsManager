using CommandLine;

namespace AllSportListener;

// ReSharper disable once ClassNeverInstantiated.Global
public class CommandLineArgs
{
    [Option('c', "ComPort", Required = true, HelpText = "The COM port to connect to.")]
    public required string ComPort { get; set; }
    
    [Option('o', "OutputFile", Required = true, HelpText = "The file to write the data to.")]
    public required string OutputFile { get; set; }
}