## Text 'chat' client for showing a problem with message delivery
The example code represents a simple text chat client. It connects to a topic / pubsub, and is intended to
broadcast messages to all subscribed clients.

### Repro steps:

1. Run two copies of the included client, e.g.
`.\TestClient.exe -u amqp://guest:secret@localhost:5672 -k /topic/foo`
2. In the first client send a few message
3. Move to the second client and note if messages have been received, then send messages from the second client.
4. Move to the first client and note if messages have been received.

The behavior observed differs a bit based on broker but in all cases clients reach a point where they only receive
messages if they send a message.

### Brokers
Tested against the following docker images:

`configit/rabbitmq` - Public image based on the base rabbitmq image, but with AMQP and MQTT plugins enabled.

Behavior with RabbitMQ:

Client 1 (sending C1.x message)
``` 
Enter a message, 'exit' to quit
C1.1
12.38.27 > INCOMING:C1.1
12.38.27 > SENT: C1.1
Enter a message, 'exit' to quit
C1.2
12.38.31 > INCOMING:C1.2
12.38.31 > SENT: C1.2
Enter a message, 'exit' to quit
C1.3
12.38.45 > INCOMING:C2.1
12.38.45 > INCOMING:C2.2
12.38.45 > INCOMING:C1.3
12.38.45 > SENT: C1.3
```

Client 2 (sending C2.x messages)
```
Enter a message, 'exit' to quit
12.38.27 > INCOMING:C1.1
12.38.31 > INCOMING:C1.2
C2.1
12.38.37 > INCOMING:C2.1
12.38.37 > SENT: C2.1
Enter a message, 'exit' to quit
C2.2
12.38.39 > INCOMING:C2.2
12.38.39 > SENT: C2.2
```

`symptoma/activemq` - ActiveMQ 5.16.x

Behavior with activemq:

Client 1 (sending C1.x message)
``` 
Enter a message, 'exit' to quit
C1.1
12.35.09 > SENT: C1.1
Enter a message, 'exit' to quit
C1.2
12.35.15 > SENT: C1.2
Enter a message, 'exit' to quit
C1.3
12.35.39 > INCOMING:C1.2
12.35.39 > INCOMING:C2.2
12.35.39 > SENT: C1.3
```

Client 2 (sending C2.x messages)
```
12.35.09 > INCOMING:C1.1
C2.1
12.35.29 > SENT: C2.1
Enter a message, 'exit' to quit
C2.2
12.35.33 > INCOMING:C2.1
12.35.33 > SENT: C2.2
Enter a message, 'exit' to quit
```
