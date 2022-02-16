using Amqp;
using CommandLine;
using static System.String;

Parser.Default.ParseArguments<Options>( args )
  .WithParsed(
    o =>
      RunRawClient( o ).Wait() );

async Task RunRawClient( Options options ) {
  var factory = new ConnectionFactory();

  if ( options.Trace ) {
    Trace.TraceLevel = TraceLevel.Frame;
    Trace.TraceListener = ( _, format, objects ) => Console.WriteLine( Format( format, objects ) );
  }

  var connection = await factory.CreateAsync( new Address( options.Uri ) );

  var session = new Session( connection );

  var receiver = new ReceiverLink(
    session,
    "receiver",
    options.Key );

  var sender = new SenderLink( session, "sender", options.Key );

  void OnMessage( IReceiverLink link, Message message ) {
    Console.WriteLine( $"{DateTime.Now.ToLongTimeString()} > INCOMING:{message.GetBody<string>()}" );
    link.Accept( message );
  }

  receiver.Start( 100, OnMessage );

  while ( true ) {
    Console.WriteLine( "Enter a message, 'exit' to quit" );
    var message = Console.ReadLine();
    if ( IsNullOrWhiteSpace( message ) ) {
      Console.Error.WriteLine( "Try again!" );
      continue;
    }

    if ( message == "exit" ) {
      break;
    }

    await sender.SendAsync( new Message( message ), TimeSpan.FromSeconds( 5 ) );
    Console.WriteLine( $"{DateTime.Now.ToLongTimeString()} > SENT: {message}" );
  }
}
