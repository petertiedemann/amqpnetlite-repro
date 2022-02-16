using CommandLine;

public class Options {
  [Option( 'u', "uri", Required = true, HelpText = "Connection uri" )]
  public string? Uri { get; set; }

  [Option( 'k', "key", Required = true, HelpText = "queue/node/routing key" )]
  public string? Key { get; set; }

  [Option( "trace", Default = false, Required = false )]
  public bool Trace { get; set; }
}
