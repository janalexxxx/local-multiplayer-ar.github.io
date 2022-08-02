<p align="center">
    <h1 align="center">WebSockets, NodeJS & Unity</h1>
    <p align="center">How to setup client-server communication in between your Unity game and your NodeJS server using WebSockets</p>
</p>

# Part 01:
# Understanding WebSockets

Web sockets are defined as a two-way communication between the servers and the clients, which mean both the parties, communicate and exchange data at the same time. This protocol defines a full duplex communication from the ground up. Web sockets take a step forward in bringing desktop rich functionalities to the web browsers. It represents an evolution, which was awaited for a long time in client/server web technology.

![HTTP Requests vs. WebSockets](/img/http-requests-vs-websockets.png)


## Client-side events

There are four events that we need to handle when working with WebSockets on the client side:

`void OnOpen()` -> Server opened connection

`void OnMessage(byte[] inboundBytes)` -> Server sent a message

`void OnClose(WebSocketCloseCode closeCode)` -> Server closed connection

`void OnError(string errorMessage)` -> Something went wrong with the connection


## Server-side events

On the server-side we need to handle three different client events:

`webSocketServer.on('connection', function(client) { ... });` -> Client joined a connection

`client.on('message', function(data) { ... });` -> Client sent a message

`client.on('close', function() { ... });` -> Client closed connection



# Part 02:
# Setup a WebSockets NodeJS server

In the first step we will create a barebone skeleton for a WebSockets NodeJS server. 

All that this server does is allow a WebSocket connection for clients and shares all received data without interference with all currently connected clients.
This is the most minimalistic version of a WebSockets server.

1. Update your `server.js` file with the following content

```javascript
const port = 42660; // REPLACE with your port
const serverAddress = 'argame.uber.space'; // REPLACE with your server's address
const serverDirectory = 'nodejs-server'; // REPLACE with your server's directory

const express = require('express');
const { createServer } = require('http');
const WebSocket = require('ws');

const app = express();

const server = createServer(app);
const webSocketServer = new WebSocket.Server({ server });

var clients = new Map();

webSocketServer.on('connection', function(client) {
  console.log("client joined");
  
  // Store reference to currently connected clients
  clients.set(client, {});

  client.on('message', function(data) {
      // Send update to all connected clients
      [...clients.keys()].forEach((client) => {
        client.send(data);
        console.log("sent update to client");
      })
  });

  client.on('close', function() {
    console.log("client left.");
    clients.delete(client);
  });
});

server.listen(port, function() {
  console.log(`Listening on ws://${serverAddress}:${port}/${serverDirectory}`);
});
```

2. Navigate into your `nodejs-server` directory

3. Install the required `Express` package by executing the following command:

    $ npm install express
    
4. Install the required `WebSocket` package by executing the following command:
    
    $ npm install ws
    
5. Restart the server 



# Part 03:
# Setup a WebSockets Unity client

## Add NativeWebSocket package to your project

The [NativeWebSocket package](https://github.com/endel/NativeWebSocket) makes it possible for you to connect your Unity client to your NodeJS server and communicate via WebSockets.

The first step is to add the NativeWebsocket package to your project using the Unity Package Manager.
Therefore follow these steps:

1. Open `Unity`
2. Open `Package Manager Window`
3. Click `Add Package From Git URL`
4. Enter URL: `https://github.com/endel/NativeWebSocket.git#upm`

If there is an error, when adding the package via Package Manager, just download it directly [here](https://github.com/endel/NativeWebSocket) and drag'n'drop it into your Assets folder in Unity.


## Setup WebSocketConnection

### Establish a connection to your remote server

1. Next create a new script called `WebSocketConnection`.

2. Establish a connection to your WebSockets server and subscribe to all the different available events like so: 

```csharp
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NativeWebSocket;
using System.IO;
using System.Text;

public class WebSocketConnection : MonoBehaviour
{
    private WebSocket _webSocket;
    private string _serverUrl = "ws://[username].uber.space:[port]/nodejs-server"; // REPLACE [username] & [port] with yours
    private int _serverErrorCode;

    async void Start() {
        _webSocket = new WebSocket(_serverUrl);

        _webSocket.OnOpen += OnOpen;
        _webSocket.OnMessage += OnMessage;
        _webSocket.OnClose += OnClose;
        _webSocket.OnError += OnError;

        await _webSocket.Connect();
    }

    void Update() {
        #if !UNITY_WEBGL || UNITY_EDITOR
            _webSocket.DispatchMessageQueue();
        #endif
    }


    // Event Handlers

    private void OnOpen() {
        print("Connection opened");
    }

    private void OnMessage(byte[] incomingBytes) {
        print("Message received");
    }

    private void OnClose(WebSocketCloseCode closeCode) {
        print($"Connection closed: {closeCode}");
    }

    private void OnError(string errorMessage) {
        print($"Connection error: {errorMessage}");
    }

    private async void OnApplicationQuit()
    {
        await _webSocket.Close();
    }
}
```

3. Replace `[username]` & `[port]` inside the `_serverUrl` variable with yours.

4. Create an empty GameObject in your scene & add the `WebSocketConnection` component to it.

5. Start your game. You should see the message `Connection opened` in your console.


Tip: To test the WebSocket connection on device, add a text field to your Canvas and instead of printing directly to the console, add the messages to this text field instead. 


### Sending a simple message to your server

1. Setup a method to send a message

```csharp
private async void SendEmptyMessageToServer() {
    if (_webSocket.State == WebSocketState.Open) {
        byte[] bytes = new byte[1] { 1 };
        await _webSocket.Send(bytes);
    }
}
```

2. Call the created method in your `OnOpen()` event handler after the server opened the connection

```csharp
Invoke("SendEmptyMessageToServer", 0f);
```


# Part 04:
# Setup server-client communication using JSON 

If we want to share data in between our Unity client and our NodeJS server, there are a few things we need to take notice of.

The first thing is that Unity uses C# while NodeJS uses Javscript. So we can't directly transfer any of our objects and classes in between these two instances. Instead we need a data format that can be read and created by both. The most common data format for sharing data in beween such different instances is JSON.

JSON can easily be converted to text to be shared and also easily be converted into classed of any specific programming language. It is also an easily readable format, which makes it thankful to work with. This means in a first step we need to convert our objects to JSON. This process is called Serialization. Deserialization is the other way around, the conversion of JSON to object.

Another thing to notice is that the `NativeWebSocket` plugin for Unity only supports sending `byte[]`. In .NET, a byte is basically a number from 0 to 255 (the numbers that can be represented by eight bits). So, a byte array is just an array of the numbers 0 - 255. This means we can not simply send a JSON string to our server, but also need to convert our JSON to a byte array before we can send it.

Therefore our full process of serialization and deserialization looks like this:

![WebSockets Serialization Overview](/img/websockets-serialization-overview.png)

### Client-Side Sending:
1. Take object of a serializable `class`.
2. Write data to this object.
3. Convert this object to a `JSON string`.
4. Convert this string to `byte[]`.
5. Send the `byte[]` data to server.


### Server-Side Receiving:
1. Receive `byte[]` from server.
2. Convert `byte[]` to `string`.
3. Convert `string` to `JSON object`.
4. Access and use data of `JSON object`.


### Server-Side Sending:
1. Convert `JSON object` to `string`.
2. Convert `string` to `byte[]`.
3. Send `byte[]` to client.


### Client-Side Receiving:
1. Receive `byte[]` from server.
2. Convert `byte[]` into `JSON string`.
3. Convert `JSON string` into object of serializable `class`.
4. Access and use data of object.




## Server

### The complete `server.js` script for serialization and deserialization then looks like this

```javascript
const port = 42660; // REPLACE with your port
const serverAddress = 'argame.uber.space'; // REPLACE with your server's address
const serverDirectory = 'nodejs-server'; // REPLACE with your server's directory

const express = require('express');
const { createServer } = require('http');
const WebSocket = require('ws');

const app = express();

const server = createServer(app);
const webSocketServer = new WebSocket.Server({ server });

var clients = new Map();

webSocketServer.on('connection', function(client) {
  console.log("client joined");
  clients.set(client, {});

  client.on('message', function(data) {
    // Convert binary to string
    var dataString = new TextDecoder().decode(data);

    if (isJSON(dataString)) {
      // Parse the data out of the message received
      const json = JSON.parse(dataString);
      console.log("incoming message");
      console.log(json);

      // Do something with the JSON data received
      // ...

      // Prepare the outgoing data
      const outboundString = JSON.stringify(json);
      const outboundBytes = new TextEncoder().encode(outboundString);

      // Send update to all connected clients
      [...clients.keys()].forEach((client) => {
        client.send(outboundBytes);
        console.log("sent update to client");
      })
    } else {
      console.log("binary received from client -> " + Array.from(data).join(", ") + "");
    }
  });

  client.on('close', function() {
    console.log("client left.");
    clients.delete(client);
  });
});

server.listen(port, function() {
  console.log(`Listening on ws://${serverAddress}:${port}/${serverDirectory}`);
});

function isJSON(string) {
   var isJSON = true;
   try {
      JSON.parse(string);
   }catch(e) {
      isJSON = false;
   }
   return isJSON;
}
```


## Unity

### Create serializable classes

All classes that you want to convert to `byte[]` and share with the server need to serializable. This means that they need to be explicitly labelled as `[Serializable]` and can only contain property types that are universal types like `float`, `int`, `bool`, `string` and `double` or contain property types that are themselve serialized.

Here is an example:

```csharp
using System;

[Serializable]
public class PlayerPosition
{
    public static string classType = "PlayerPosition";
    
    public int playerId;
    public float x;
    public float y;
    public float z;
}

[Serializable]
public class AllPlayerPositions {
  public static string classType = "AllPlayerPositions"

  public PlayerPosition[] playerPositions;
}
```

### Handle incoming data

Important: You need to add `using System.Text` & `using System.IO` to the top of your script to use the serialization and deserialization methods.

1. Convert incoming `byte[]` to `string`:

```csharp
string incomingString = System.Text.Encoding.UTF8.GetString(incomingBytes);
```

2. Check whether the incoming string is an integer or not. If it is an integer, it is not JSON, but a serverError. Handle it accordingly.

```csharp
if (int.TryParse(incomingString, out _serverErrorCode)) {
    // Handle serverError
    print($"Server Error: {serverErrorCode}");
} else {
    // Handle JSON
}
```

3. When you work with different objects of classes that you send around, you will need a way to differentiate in between different types of message, before converting them a `class` object. You need to find out first what type of data you actually received. A simple way to do this, is to add a `string` static property called `classType` to all of serializable classes. Then you can check every JSON object that you receive directly for the value of this property and this way decide on how to proceed.

Here is an example of a serializable class definition.

```csharp
[Serializable]
public class PlayerPosition
{
    public static string classType = "PlayerPosition";
    
    public int playerId;
    public float x;
    public float y;
    public float z;
}
```

Then we need to add [this SimpleJSON script](https://github.com/endel/NativeWebSocket) to our project and add `using SimpleJSON;` to the top of our file.

After adding the `SimpleJSON` script to our project, we can then parse the JSON and check the `classType` property like this:

```csharp
var parsedJson = JSON.Parse(incomingString);
string classType = parsedJson["classType"].Value;

if (classType == PlayerPosition.classType) {
    // Convert incoming JSON to object of type `PlayerPosition`
} else if (classType == ...) {
    // Check for other potential cases
}
```

4. Convert the incoming JSON to a class of your choosing

```csharp
YourClass receivedObject = JsonUtility.FromJson<YourClass>(incomingString);
```

#### Code Example for handling incoming data
An example of how the incoming message from the server can be deserialized within the WebSockets `OnMessage()` event handler:

```csharp
  
  private void OnMessage(byte[] inboundBytes) {
    print("Message received");
    print($"bytes: {inboundBytes}");
    string inboundString = System.Text.Encoding.UTF8.GetString(inboundBytes);
    print($"message: {inboundString}");
    
    
    if (int.TryParse(inboundString, out serverErrorCode)) {
      // If server returns an integer, it is an error
      print($"Server Error: {serverErrorCode}");
    } else {
      JSONNode json = JSON.Parse(inboundString);
      
      if (json["classType"].Value == PlayerDataPackage.classType) {
          PlayerDataPackage playerData = JsonUtility.FromJson<PlayerDataPackage>(inboundString);
          print($"Received PlayerDataPackage || name: {playerData.name}, highScore: {playerData.highScore}");
      } else if (json["classType"].Value == LevelDataPackage.classType) {
          LevelDataPackage levelData = JsonUtility.FromJson<LevelDataPackage>(inboundString);
          print($"Received LevelDataPackage || name: {levelData.mapTitle}, highScore: {levelData.difficultyLevel}");
      }
    }
  }
```



### Handle outgoing data

2. Create an `async` method: `private async void SendMessage() { ... }`

3. Check whether the client is currently connected to the server by checking `webSocket.Statae == WebSocketState.Open`

4. Convert a serializable object to `JSON`: `string json = JsonUtility.ToJson(yourObject);`

5. Convert `JSON` to `byte[]`: `byte[] bytes = Encoding.UTF8.GetBytes(json);`

6. Send bytes to your server using Websocket: `await webSocket.Send(bytes);`

7. Invoke `async` method: `Invoke("SendMessage", 0f);`


Full code:

```csharp
using System.Text;
using System.IO;

if (webSocket.State == WebSocketState.Open) {
    string json = JsonUtility.ToJson(yourObject);
    byte[] bytes = Encoding.UTF8.GetBytes(json);
    await webSocket.Send(bytes);
}
```



### Complete Unity WebSockets implementation example
*Example implementation for handling incoming and outgoing WebSockets data*


```csharp
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using NativeWebSocket; // Source: https://github.com/endel/NativeWebSocket
using SimpleJSON; // Source: https://github.com/Bunny83/SimpleJSON/blob/master/SimpleJSON.cs


[Serializable]
public struct PlayerDataPackage
{
    public static string classType = "PlayerDataPackage";

    public string name;
    public int highScore;

    public PlayerDataPackage(string name, int highScore) {
      this.name = name;
      this.highScore = highScore;
    }
}


[Serializable]
public struct LevelDataPackage
{
    public static string classType = "LevelDataPackage";

    public string mapTitle;
    public int difficultyLevel;

    public LevelDataPackage(string mapTitle, int difficultyLevel) {
      this.mapTitle = mapTitle;
      this.difficultyLevel = difficultyLevel;
    }
}


public class WebSocketsConnection : MonoBehaviour
{
  private WebSocket webSocket;
  private string serverUrl = "ws://my.uber.space:41860/nodejs-server";
  private int serverErrorCode;


  //////////////////////////////////
  // MonoBehaviour Lifecycle Event Handlers
  //////////////////////////////////

  async void Start()
  {
    webSocket = new WebSocket(serverUrl);

    webSocket.OnOpen += OnOpen;
    webSocket.OnMessage += OnMessage;
    webSocket.OnClose += OnClose;
    webSocket.OnError += OnError;

    await webSocket.Connect();
  }

  void Update()
  {
    #if !UNITY_WEBGL || UNITY_EDITOR
      webSocket.DispatchMessageQueue();
    #endif
  }

  private async void OnApplicationQuit()
  {
    await webSocket.Close();
  }


  //////////////////////////////////
  // WebSockets Event Handlers
  //////////////////////////////////

  private void OnOpen() {
    print("Connection opened");
    Invoke("SendPlayerHighScore", 0f);
  }

  private void OnMessage(byte[] inboundBytes) {
    print("Message received");
    print($"bytes: {inboundBytes}");
    string inboundString = System.Text.Encoding.UTF8.GetString(inboundBytes);
    print($"message: {inboundString}");
    
    
    if (int.TryParse(inboundString, out serverErrorCode)) {
      // If server returns an integer, it is an error
      print($"Server Error: {serverErrorCode}");
    } else {
      JSONNode json = JSON.Parse(inboundString);
      
      if (json["classType"].Value == PlayerDataPackage.classType) {
          PlayerDataPackage playerData = JsonUtility.FromJson<PlayerDataPackage>(inboundString);
          print($"Received PlayerDataPackage || name: {playerData.name}, highScore: {playerData.highScore}");
      } else if (json["classType"].Value == LevelDataPackage.classType) {
          LevelDataPackage levelData = JsonUtility.FromJson<LevelDataPackage>(inboundString);
          print($"Received LevelDataPackage || name: {levelData.mapTitle}, highScore: {levelData.difficultyLevel}");
      }
    }
  }

  private void OnClose(WebSocketCloseCode closeCode) {
    print($"Connection closed: {closeCode}");
  }

  private void OnError(string errorMessage) {
    print($"Connection error: {errorMessage}");
  }


  //////////////////////////////////
  // Helper Methods
  //////////////////////////////////

  private async void SendPlayerHighScore() {
      string playerName = "Michael P.";
      int playerHighScore = 12400;
      PlayerDataPackage playerData = new PlayerDataPackage(playerName, playerHighScore);

      if (webSocket.State == WebSocketState.Open) {
          string json = JsonUtility.ToJson(playerData);
          byte[] bytes = Encoding.UTF8.GetBytes(json);
          await webSocket.Send(bytes);
      }
  }
}

```



# Part 05:
# Improve server-client communication

## Make sure to handle offline communication too

It can always happen that your client does not have an active internet connection. In these cases you need to make sure that the game reacts accordingly.
You can differentiate by checking for the `WebSocketState` like this:

```csharp
if (webSocket.State == WebSocketState.Open) {
    // Player is online
} else {
    // Player is offline
}
```
