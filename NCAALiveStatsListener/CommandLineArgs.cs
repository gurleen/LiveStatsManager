using CommandLine;

namespace NCAALiveStatsListener;

public class CommandLineArgs
{
    [Option('a', "address", Required = true, HelpText = "IP address of NCAA Live Stats program.")]
    public required string Address { get; set; }
    
    [Option('p', "port", Required = true, HelpText = "Port of NCAA Live Stats program.")]
    public required int Port { get; set; }
    
    [Option('o', "output", Required = true, HelpText = "Output file.")]
    public required string Output { get; set; }
}