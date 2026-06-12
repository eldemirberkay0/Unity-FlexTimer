# Unity-FlexTimer
A flexible timer package with zero update allocation for Unity running on Unity player loop, meaning no MonoBehaviour is required. Alternative to Coroutines and messy Update() loops with a bunch of time variables and if checks.

## Includes
* **TimerManager:** Holds timers in a list and updates them in every frame. Also responsible for some practical event registration.

* **Timer:** The actual timer class. Has various properties:
    * OnTick -> Invokes on each timer tick.
    * OnUpdate -> Invokes on timer update.
    * OnFinished -> Invokes when no ticks left.
    * tickDuration -> Duration of each tick.
    * tickCount -> Number of ticks.
    * IsLooped -> Overrides tickCount and ticks forever if true.
    * IsScaled -> True if timer is using scaled deltaTime.
    * IsRunning -> True if timer is running.
    * TicksPassed -> Number of ticks passed since timer's start.
    * And more to get timer's current situation like SecondsToTick, SecondsToFinish and normalized versions of these.

## How to Use
### Creating Timer With Constructor
 Using Timer directly with constructor provides more flexibility. You can set many properties as you like or leave them default.
```csharp
    public Timer(float tickDuration, Action OnTick = null, Action OnFinished = null, Action OnUpdate = null, int tickCount = 1, bool isLooped = false, bool isScaled = true, GameObject attachedTo = null)
    {
        this.tickDuration = tickDuration;
        this.OnTick += OnTick;
        this.OnFinished += OnFinished;
        this.OnUpdate += OnUpdate;
        this.tickCount = tickCount;
        IsLooped = isLooped;
        IsScaled = isScaled;
        if (attachedTo != null)
        {
            this.attachedTo = attachedTo;
            isAttached = true;
        }
    }
```

While using with constructor you have to use timer.Start() to run the timer.

Timer.Start():
```csharp
    public void Start()
    {
        if (!TimerManager.timers.Contains(this))
        {
            TicksPassed = 0;
            secondsToTick = tickDuration;
            TimerManager.RegisterTimer(this);
        }
        IsRunning = true;
    }
```

You can attach actions while creating the timer or later.

While creating:
```csharp
    Timer timer = new Timer(2, () => Debug.Log("Ticked")); 

    void Start()
    { 
        timer.Start();
        // Logs "Ticked" after 2 seconds
    }
```

After creating:
```csharp
    Timer timer = new Timer(2);

    void Start()
    {
        timer.OnTick += () => Debug.Log("Ticked"); 
        timer.Start();
        // Logs "Ticked" after 2 seconds
    }
```
***
Attach timer to a GameObject if timer's actions have something to do with it. Because if Gameobject is destroyed, timer can't access it and this will result with an error. Or you can call timer.Cancel() manually when object is destroyed (avoids checking if attachedObject is destroyed on every update).

Example (Practical):
```csharp
    void Start()
    {
        Timer timer = new Timer(2, () => gameObject.SetActive(false), attachedTo: this.gameObject);
        timer.Start();
        // If the timer were not attached, it would throw a MissingReferenceException error when timer ticked.
        Destroy(gameObject);
    }
```
Example (Advanced):
```csharp
    Timer timer;

    void Start()
    {
        timer = new Timer(2, () => gameObject.SetActive(false));
        timer.Start();
        Destroy(gameObject);
    }

    // Add timer.Cancel() to OnDestroy to cancel the timer when object is destroyed
    void OnDestroy()
    {
        timer.Cancel();
    }
```
Note that deleting timer reference when you are done with timer is also a good practice.

*There are more examples at [Samples Folder](/Samples~).*
***
### Using TimerManager for Quick Needs
If you want to trigger an action once after a delay you can use TimerManager.RegisterEvent(). This function creates a basic timer and starts it directly. This method is just a wrapper and designed for practical use.

TimerManager.RegisterEvent():
```csharp
    public static Timer RegisterEvent(float duration, Action action, MonoBehaviour attachedTo = null)
    {
        Timer timer = new Timer(duration, action, null, null, 1, false, true, attachedTo);
        timer.Start();
        return timer;
    }
```

Example:
```csharp
    void Start()
    {
        // Logs "Ticked" after 2 seconds
        TimerManager.RegisterEvent(2, () => Debug.Log("Ticked"));
    } 
```

## How to Install
There are two ways to install this package into your Unity project:

### Option 1: Via Unity Package Manager (UPM)
1. Open your Unity project.
2. Go to **Window > Package Manager**.
3. Click the **+** (plus) button in the top-left corner of the window.
4. Select **Install package from git URL**.
5. Paste `https://github.com/eldemirberkay0/Unity-FlexTimer.git` and click **Install**.

### Option 2: Editing manifest.json
1. Navigate to your project's `Packages` folder.
2. Open `manifest.json`.
3. Add the following line to dependencies:
```json
"com.eldemirberkay0.flextimer": "https://github.com/eldemirberkay0/Unity-FlexTimer.git"
```