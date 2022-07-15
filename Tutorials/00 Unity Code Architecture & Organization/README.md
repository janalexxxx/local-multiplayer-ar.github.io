<p align="center">
    <h1 align="center">Organizing your Unity project and codebase</h1>
    <p align="center">A step-by-step guide to keep your Unity project and codebase organized and easily debuggable</p>
</p>


There is no one way of creating order in your Unity project and there is no one right way of architecting your code. Sometimes you are just building a quick prototype and its okay for the code to be messy, because you dont have much time and just want to try out an idea. Other times you know that you need to extend and scale your codebase quite a bit. In this case it makes sense to take the time to organize your code and your project asset more thoroughly, since it will make the debugging process for you in the future much faster.

In general you need to take care of not overwhelming yourself and not getting lost in the process too. Refactoring and improving your code is an iterative process. In the first iteration you usually focus on the raw implementation itself. The goal here is just to get the desired functionality working without taking much care yet of the architecture. When the desired functionality is working as expected, it is time to refactor the code. Then you know already all the building parts necessary for your feature and you can take the step to create smaller classes or implement code design patterns that work. When you get more proficient with developing in Unity, you can of course skip the first step of a raw implementation completely.

You have probably heard of architecture patterns like MVC, MVVM or VIPER and are wondering if one of these is the way to go for you. In my humble opinion it doesn't make sense for you to focus on such patterns for now. They are extremely useful when working in huge teams, where it is required for all developers to organize their code in exactly the same way, so that even when a lot of people write code in parallel, the software still stays maintainable. There is also no clear answer to which structure is superior, although most developers can get quite emotional talking about this since everybody has a personal favourite. The problem with these structures though is, that they complicate and slow down the development process – a natural drawback for the advantage of a more unified structure.

Since you are usually working in small teams or alone, the advantage for you is not that big actually, but the disadvantage due to your programming level quite high. It will cost you a lot of energy to translate your functionality into one of these structures. Nevertheless when you feel proficient with programming and the basics of organizing your code, then it is an excellent idea to dive deeper into these architecture patterns. But until then, rather focus on the basics first.

With this guide I am aiming to provide you with step-by-step instruction to guide you towards an organized Unity project and an easily debuggable codebase.




# Unity Project Organization
## Keep your Unity project organized
The very first step is to keep the assets in your Unity project and your scene hierarchy organized. Here are a few tips, that should help you with this:

* Organize your assets by keeping all assets of the same type in the same folder. Use subfolders to differentiate assets of different parts of your games.
	* All your scripts should be in the `Scripts` folder.
	* All your materials should be in the `Materials` folder.
	* All your sprites in the `Sprites` folder.
	* etc.

* Keep your hierarchy clean by adding all GameObjects of the same type or functionality to empty GameObjects with a descriptive title
	* Add all of your Managers as children to an empty GameObject called `Managers`
	* Add all of your Canvases as children to an empty GameObject called `Canvases`
	* Add all GameObject related to the setup of the scene, such as Camera & Lights, as children to an empty GameObject called `Setup`
	* etc.
	



# Clean Code
## Keep your code clean
The next step is to keep your code organized. When reading through a class you need to be able to quickly spot what variables it has, what objects it has access to, what it does & what is accessible to the outside.

* Organize your class variables
	* Add all class variables to the top of your class
	* Explicitly label all of your variables as either 'public' or 'private'
	* Organize your variables into chunks of private variables and private variables, also sort them by type or add other subcategories that logically make sense for you
	
* Organize your class methods
	* Explicitly label all of your methods as either 'public' or 'private'
	* Organize similar methods into chunks and separate these chunks in your code using comments
		* One chunk could be `// Unity Event Handlers`
		* Another chunk could be `// Event Handlers`
		* Another chunk could be `// Public Methods`
		* Another chunk could be `// Private Helper Methods`
		* etc.
	
* Properly name your variables, methods and classes
	* Use descriptive names. You should be able to easily refer from the name to its functionality.
	* Use consistent naming patterns
		* All variable names are lowercased (eg `public float waitingTime`)
		* All private variables start with an `_` (eg a private reference to the GameManager would be called `_gameManager`)
		* All boolean variables are named according to the same pattern (eg `private bool _isGameRunning`, `private bool _hasPlayerArrivedAtDestination`)
		* All methods names are uppercased (eg `public void YourMethod()`)
	* Make sure that every method only does one job and that this job can be clearly derived from the name. When debugging, we tend to just cram additional statements into methods that might fix the bug, but actually have nothing to do with the intended functionality of the method. This creates bugs, that are hard to debug in the future. Make sure to add additional statements to places in the code, where you would expect them. Create new methods when necessary.





# Debugging Tips
You will encounter bugs in your game, that you will need to fix. There is no way around this. Here are a few tips to help you with the debugging process



## Debug Printing
Let the code speak to you. Use debug statements to get feedback regarding the state of the current application. Ask it questions and try to get the answers. What can I test? What does this tell me?

* Regularly print out to the console to get feedback on what has happened in your application. Use descriptive print statements hinting at where this code has been executed and what has happened. Use a string literal to log variable values at runtime to know what has been going on under the hood.
	* Printing to the console: `print("GamePhase Attach has started");` || `Debug.Log("GamePhase Attach has started");`
	* Printing variable values: `print($"TouchManager.OnTap: Tap received at {finger.position}");` || `Debug.Log($"TouchManager.OnTap: Tap received at {finger.position}");`
	
* Differentiate in between regular print messages, warnings and errors
	```csharp
		// Print a message to the console
		Debug.Log($"This is a message from {title}");
		print($"This is a message from {title}");
		
		// Print a warning to the console
		Debug.LogWarning($"This is a warning from {title}");
		
		// Print an error to the console
		Debug.LogError($"This is an error from {title}");
	```
	
* When you get an error inside the UnityEditor console, simply double-click on it and VS Studio will open up with your cursor directly at the position in the code. This way you immediately know where an error has been thrown.
	
* When testing your game on a mobile device you will not have direct access to the console log. In this case simple add a `TextMeshProUGUI` or `Text` object to the top of your Canvas and create a separate method to add your log statements to this text.
```csharp
public TextMeshProUGUI textPrintOut;

public void printOut(string text) {
	if (textPrintOut != null && textPrintOut.gameObject.activeSelf) {
		textPrintOut.text = textPrintOut.text + '\n' + text;
	}
}
```


## Developer Menu
### Create a `Developer Menu` with custom actions to simulate different in-game-situations
You can create menu items with custom actions inside of Unity. This way you can easily simulate different in-game-situations in an instant without actually needing to play the game to get until there.

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine

public class Developer {
	[MenuItem("Developer/Clear Saves")]
	public static void ClearSaves() {
		// Clear all of your saved data
		print("All saves have been cleared");
	}
	
	[MenuItem("Developer/Unlock Skins")]
	public static void UnlockSkins() {
		// Unlock all skins for the player
		print("All skins have been unlocked");
	}
}
```

The created actions than appear in the top-bar of the UnityEditor.




# Individual Components
## Separate responsibilities into individual classes
Your game consists of a lot of different building blocks. Make sure to modularize your code and create separate classes for each part. Every class should only have one responsibility that can be deducted from its name.

* Create separate `Manager` classes for everything that needs to be managed
	* `GameManager` that manages the overall game
	* `TouchManager` that manages touch input
	* `NetworkingManager` that manages the communication with the server
	* etc.
	
* Create separate classes for each other part of the game
	* `GameSettings` class that holds all of the setting for your game
	* `Player` class that handles the player logic
	* `NPC` class that handles the NPC logic
	* etc.

* Use inheritance to create different versions of the same class. For example you can create a basic `Player` class that defines the variables and methods that every player in your game has defined. Then you can create separate implementations of players for your game using inheritance (eg. `public class Soldier : Player`, `public class Wizard : Player`)

Aim for keeping classes as small as possible. When you feel that a class starts getting clumsy, think about how you could separate its logic into individual components.




# Prefabs & Prefab Variants
## Use Prefabs and Prefab Variants to simplify the process of altering and reusing your GameObjects
Use Prefabs and prefab variants to organize your GameObjects. Prefabs are reusable GameObjects. A Prefab variant is based on a Prefab, but can override some of its properties. All changes to the original prefab are automatically pushed to all prefab variants except when this prefab variant is overriding this specific property.

A good tutorial describing the use of Prefabs and Prefab Variants can be found [here](https://www.youtube.com/watch?v=S0cjIhI2fIw).





# Scriptable Objects
## Use ScriptableObjects to manage data and settings for your game outside of individual scenes and components
A good tutorial describing the use and implementation of ScriptableObjects can be found [here](https://www.youtube.com/watch?v=WLDgtRNK2VE&list=TLPQMTMwNzIwMjKeshyYMcdc1w&index=5). The video also includes an example project to check out the final implementation.

Another good tutorial can be found [here](https://www.youtube.com/watch?v=aPXvoWVabPY).




# Simple GameManager Architecture
## Implement a simple architecture to control the flow of information
When building your game, your code gets more and more complicated over time. It is absolutely crucial to retain control over your code by managing the flow of information inside of it. An easy way to do that is to lay out a simple architecture that defines how your different classes should communicate with each other.

The most simple architecture is to use the `GameManager` class as the central point of decision-making and event-handling inside of your application. This means that ALL game-relevant events are being handled by the `GameManager`. The `GameManager` is informed about each event and then handles the response by changing the necessary settings and calling all needed methods of other classes to respond.

Here is an illustration of this architecture:

![Simple GameManager Architecture](/img/simple-gameManager-architecture.png)

Make sure that `GameManager` only stores the data and accesses the methods that it actually needs to make the game-relevant decisions. Try to store as little data as possible inside of the `GameManager` and create new individual classes handling individual responsibilities when you feel that the `GameManager` is getting too crowded.

Two simple examples to illustrate:

1. Your `TouchManager` class checks for touch input events. As soon as it identifies a tap on the screen, it will call the GameManager's public `OnTap(Vector2 screenPosition)` method and supply the position of the finger on the screen. The `GameManager` then makes a raycast, checks which GameObject the user tapped on and based on this changes the state of the game and informs other classes that need to know about this event.

2. Your `NetworkingManager` class manages the communication with the server. As soon as the server sends a message, the `NetworkingManager` will receive it, process it and then call the complementary GameManager's method to inform it about this event. So for example the server might have sent an event, that one of the players has just shot using his weapon. The `NetworkingManager` then calls the public method `OnPlayerShot(Vector3 position, Quaternion rotation)` of the `GameManager`. The `GameManager` then takes care of launching the player's bullet in the game and triggers all other needed changes inside the game.






# Single Source of Truth
## Make sure that there is always only one source of truth
All data for the game should be stored at only one place. As soon as multiple classes need the same information, this information should be stored in a separate class, eg `GameSettings`. All classes that need access to this data should access the same variable or method to get it.

For the settings of the game you can for example create a `GameSettings` class that holds reference to all relevant values, materials and prefabs. All classes that need this information communicate with the `GameSettings` object to get it. When you want to change a setting, you will need to change it only in one place.





# Enumerations
## Use enumerations to manage states and make your code more readable
Enumerations are a great way to make your code more readable and to improve the handling of states for you as a developer as well. We touched about this already earlier in the course, but here is a simple overview of how to use `enum`:

```csharp
public enum GameObjectType {
	Player,
	Enemy
}
```

```csharp
public GameObjectType type;
```

```csharp
// Check enum values using a switch statement
switch(type) {
	case GameObjectType.Player:
		// Do something
		break;
	case GameObjectType.Enemy:
		// Do something else
		break;
	default:
		break;
}
```

```csharp
// Check enum using an if statement
if (type == GameObjectType.Player) {
	// Do something
}
```





# Code Design Patterns
Improve your codebase using design patterns



## Service Locator Pattern
### Manage the access of your classes to different services using the ServiceLocator Pattern instead of using the Singleton pattern
The ServiceLocator provides a global point of access to a service without coupling users to the concrete class that implements it. This makes it a great alternative to using the Singleton pattern, which proved a global point of access to a service as well, but couples users to the concreate class that implements it. The Singleton pattern can quickly be overused and create a mess in your project, since you will loose oversight over who accesses what and when quickly. The ServiceLocator is a more organized implmentation and provides you with the possibility to restricting the access to specific services potentially as well.

More information about it can be found [here](http://gameprogrammingpatterns.com/service-locator.html).

How to implement it:

1. Create a new script called ServiceLocator and add this code:
```csharp
public class ServiceLocator : MonoBehaviour
    {
        private static ServiceLocator instance;

        [SerializeField] MonoBehaviour[] autoRegisterServices;

        Dictionary<Type, object> registeredServices = new Dictionary<Type, object>();

        void Awake()
        {
            instance = this;
            foreach (var service in autoRegisterServices)
            {
                Register(service);
            }
        }

        void OnDestroy()
        {
            instance = null;
        }

        public static void Register(object service)
        {
            instance.registeredServices.Add(service.GetType(), service);
        }

        public static bool Get<T>(out T service) where T : class
        {
            service = Get<T>();
            return service != null;
        }

        public static T Get<T>() where T : class
        {
            var type = typeof(T);
            object serviceObject;
            if (instance.registeredServices.TryGetValue(type, out serviceObject))
            {
                return (T)serviceObject;
            }
            else
            {
                Debug.LogError("No service registered of type " + type.FullName);
                return null;
            }
        }
    }
```

2. Create an empty GameObject in the scene and call it "ServiceLocator"

3. Add all services needed as children to the `ServiceLocator` gameObject (eg `GameManager`, `TouchManager`, `GameSettings`, etc.)

4. Add each child of the `ServiceLocator` gameObject then to the public `autoRegisteredServices` property of the `ServiceLocator` gameObject inside the Inspector using drag'n'drop.

5. Then access a service in your `MonoBehaviour` class like this:
```csharp
private GameManager _gameManager;
private GameSettings _gameSettings;

void Awake() {
	ServiceLocator.Get(out _gameManager);
	ServiceLocator.Get(out _gameSettings);
}
```




## Observer Pattern
### Use the Observer Pattern to subscribe and unsuscribe to events inside different classes
The Observer Pattern is an easy way for a class to get notified about events of another class. It can simply subscribe and unsubscribe to these events.

You have been using this pattern already in your `TouchManager` class, when you subscribed to the touch events of the `Lean.Touch` package:

```csharp
public class TouchManager : MonoBehaviour
{
	// ...

    void OnEnable() {
		// Subscribe to the event
        LeanTouch.OnFingerTap += HandleFingerTap;
    }

    void OnDisable() {
		// Unsubscribe from the event
        LeanTouch.OnFingerTap -= HandleFingerTap;
    }

    // Private Methods
    private void HandleFingerTap(LeanFinger finger) {
        // Notify the gameManager about the event
    }
}
```

Here is an example of how to implement this pattern for yourself. In this example we want to play an audio file as soon as a bubble has popped. The `PopAudioPlayer` therefore subscribes to the `OnBubblePopped` event of the `Bubble` object and plays an audio when this event is called.

```csharp
public class PopAudioPlayer : MonoBehaviour {
	private AudioSource _audioSource;
	
	private void Awake() => _audioSource = GetComponent<AudioSource>();
	
	// Subscribe to the event
	private void OnEnable() => Bubble.OnBubblePopped += PlayPopAudio;
	
	// Unsubscribe to the event
	private void OnDisable() => Bubble.OnBubblePopped -= PlayPopAudio;
	
	private void PlayPopAudio() => _audioSource.Play();
}

public class Bubble : MonoBehaviour {
	public static event Action OnBubblePopped;
	
	private void Pop() {
		// Invoke the event for all subscribers
		OnBubblePopped?.Invoke();
	}
}
```


Note: If you are unfamiliar with using `=>` to write a method, compare these two methods below. They are exactly the same. `=>` is simply a shorthand to define a method in one line of code.

```csharp
// Method #1: Uses shorthand writing
private void Awake() => _audioSource = GetComponent<AudioSource>();

// Method #2: Uses classic writing
private void Awake() {
	_audioSource = GetComponent<AudioSource>();
}
```




## State Machine Pattern
### Use that State Machine Pattern to manage everything in your game that has a multitude of potential states and needs to transition in between them
The State Machine Pattern is probably the most impactful pattern to clean up your codebase. Everything in your game that has a set of potential states and needs to manage transitions in between these states can be rewritten using the State Machine Pattern.

![State Machine Apple Example](/img/state-machine-apple-example.png)

For example you might have a simple apple GameObject inside your game. This apple might be spawned at some point, then is growing for a specific time. When it reached its final size it might fall to the ground and can be picked up by the player. Maybe it even starts rottening after it fell to the ground and based on its freshness it might add or deduct health points to the player's health when eaten. You could implement this logic using a bunch of booleans and if statements, but your code will get messy, hard to read and even harder to debug. By implementing this logic using the state machine pattern you can easily separate the logic into indidivual classes and easily manage the transitions in between these states as well.

A great tutorial showing how to use and implement the State Machine Pattern can be found [here](https://www.youtube.com/watch?v=Vt8aZDPzRjI).

Additional information can be found [here](https://refactoring.guru/design-patterns/state) and [here](http://gameprogrammingpatterns.com/state.html).





## Managing Game Phases using State Machine
### Adapt the State Machine Pattern to manage different phases of your game
Most games consist of different phases, this is especially true for local multiplayer games. There might be at first a phase, where all of the players are gathering and selecting their character. Then a first phase in the game, where the goal for the players might be to gather resources or find weapons. Then a second phase in which the players fight each other and then the concluding phase, in which the winner is celebrated and the results are displayed. For managing the logic in between these different phases the State Machine Pattern can be adapted.

This is how it can be implemented:

1. Create an enum `GamePhase` defining all the different phases of your game:
```csharp
public enum GamePhase
{
	Phase1_PreGame,
	Phase2_Introduction,
	Phase3_FirstContact,
	Phase4a_MeetingPlaceRealmOfFreedom,
	Phase4b_RealmOfFreedom,
	Phase5a_MeetingPlaceRebellion,
	Phase5b_Rebellion,
	Phase6a_MeetingPlaceEndgame,
	Phase6b_Endgame,
	Phase7_GameOver
}
```

2. Create a base clase `GameLogicPhase`
```csharp
public abstract class GameLogicPhase : MonoBehaviour
{
	public abstract GamePhase ClassGamePhase { get; }
	
	// Add necessary variables here

	public virtual void OnStart()
	{
		// Add code that should be executed for all GameLogicPhases when starting here
	}

	public virtual void OnEnd()
	{
		// Add code that should be executed for all GameLogicPhases when ending here
	}

	public virtual void UpdateWhileActive()
	{
		// Add code that should be executed for all GameLogicPhases when updating here
	}
}
```

3. Create classes for each GameLogicPhase inheriting from base class `GameLogicPhase`
```csharp
public class GameLogicPhase1 : GameLogicPhase
{
	public override GamePhase ClassGamePhase => GamePhase.Phase1_PreGame;

	// Add necessary variables here

	void Awake()
	{
		// Add code that should be executed on awake here
	}

	public override void OnStart()
	{
		base.OnStart();

		// Add code that should be executed on start here
	}

	public override void OnEnd()
	{
		base.OnEnd();

		// Add code that should be executed on end here
	}
	
	public override void OnUpdateWhileActive()
	{
		base.UpdateWhileActive();
		// Add code that should be executed on update here
	}
}
```

4. Create an empty GameObject called `GameLogicPhases` in your scene. Then create GameObjects in your scene for each GameLogicPhase and add them as children to the object `GameLogicPhases`.

5. Then manage these GameLogicPhases from your `GameManager` script like this:
```csharp
public class GameManager : MonoBehaviour {
	GameLogicPhase currentGameLogicPhase;
	
	GameLogicPhase[] gameLogicPhases;
	
	void Awake() {
		gameLogicPhases = FindObjectsOfType<GameLogicPhase>();
	}
	
	void Update() {
		if (currentGameLogicPhase != null) {
			currentGameLogicPhase.UpdateWhileActive();
		}
	}
	
	void UpdatePhase(GamePhase nextPhase) {
		if (currentGameLogicPhase != null) {
			currentGameLogicPhase.OnEnd();
		}

		currentGameLogicPhase = null;
		foreach (var phase in gameLogicPhases) {
			if (phase.ClassGamePhase == nextPhase) {
				currentGameLogicPhase = phase;
				break;
			}
		}

		if (currentGameLogicPhase != null) {
			currentGameLogicPhase.OnStart();
		}
	}
}
```





## More Design Patterns
### Learn about additional design patterns and apply them to your code, wherever you see it fit
Here are some resources for diving deeper and getting introduced to additional design patterns:

* [Design Patterns in C#](https://refactoring.guru/design-patterns/csharp)
* [Game Programming Patterns](http://gameprogrammingpatterns.com/contents.html)




H A P P Y  C O D I N G !
