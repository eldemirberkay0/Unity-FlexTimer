# Unity-FlexTimer
A flexible timer package for Unity running on Unity player loop, meaning no MonoBehavior is required. Alternative to Coroutines and spagetti Update() loops with bunch of time variables and if checks.

## Includes
* **TimerManager:** Holds timers in a list and updates them in every frame. Also responsible with some practical event registration.

* **Timer:** The actual timer class. Has various properties:
    * OnTick -> Invokes on each timer tick.
    * OnUpdate -> Invokes on timer update.
    * OnFinished -> Invokes when no ticks left.
    * tickDuration -> Duration of each tick.
    * tickCount -> Number of ticks.
    * IsLooped -> Overrides tickCount and ticks forever if true.
    * IsScaled -> True if timer is using scaled deltaTime.
    * IsRunning -> True if timer is running.
    * TicksRemaining -> Number of ticks left.
    * And more to get timer's current situation like SecondsToTick, SecondsToFinish and normalized versions of these.

## How to Use
### Creating Timer Manually
 Using Timer directly with constructor provides more flexibility. You can set many properties as you like or leave them default.
```csharp
    /// <summary> Creates a timer with various parameters. </summary>
    /// <param name="tickDuration"> Duration (second) of each tick. </param>
    /// <param name="OnTick"> The action invokes on timer tick. Null by default. </param>
    /// <param name="OnFinished"> The action invokes when no ticks left. Null by default. </param>
    /// <param name="OnUpdate"> The action invokes every timer update. Null by default. </param>
    /// <param name="tickCount"> How many times the timer will tick. 1 by default. </param>
    /// <param name="isLooped"> Ticks forever if true. Overrides tickCount if true. False by default. </param>
    /// <param name="isScaled"> Uses Time.unscaledDeltaTime if false. True by default. </param>
    public Timer(float tickDuration, Action OnTick = null, Action OnFinished = null, Action OnUpdate = null, int tickCount = 1, bool isLooped = false, bool isScaled = true)
    {
        this.tickDuration = tickDuration;
        this.OnTick += OnTick;
        this.OnFinished += OnFinished;
        this.OnUpdate += OnUpdate;
        this.tickCount = tickCount;
        IsLooped = isLooped;
        IsScaled = isScaled;
    }
```

While using with constructor you have to use timer.Start() manually to run the timer.
```csharp
    // Note: This function is a member of Timer.
    /// <summary> Registers timer to TimerManager and sets IsRunning true. </summary>
    public void Start()
    {
        if (!TimerManager.timers.Contains(this))
        {
            secondsToTick = tickDuration;
            TimerManager.RegisterTimer(this);
            IsRunning = true;
        }
    }
```

You can attach actions while creating the timer or later.

While creating:
```csharp
    // Create a timer with a duration of 2 seconds and an action attached to it.
    Timer timer = new Timer(2, () => Debug.Log("Ticked")); 

    void Start()
    { 
        // Start the timer
        timer.Start();
        // Logs "Ticked" after 2 seconds
    }
```

After creating:
```csharp
    // Create a timer with a duration of 2 seconds and no actions attached
    Timer timer = new Timer(2);

    void Start()
    {
        // Add an action to timer's OnTick
        timer.OnTick += () => Debug.Log("Ticked"); 
        // Starts the timer
        timer.Start();
        // Logs "Ticked" after 2 seconds
    }
```

### Using TimerManager for basic needs
If you just want to trigger an action after a delay you can use TimerManager.RegisterEvent(). This function creates a basic timer and starts it directly. This method prevents access to timer and reduces control over timer but it is more practical.

TimerManager.RegisterEvent():
```csharp
    /// <summary> Creates a timer with an event attached to it and starts timer directly. </summary>
    /// <param name="duration"> Duration (second) of each tick. </param>
    /// <param name="action"> Invokes on timer tick. </param>
    /// <param name="isScaled"> Uses Time.unscaledDeltaTime if false. True by default. </param>
    public static void RegisterEvent(float duration, Action action, bool isScaled = true)
    {
        Timer timer = new Timer(duration, action, null, null, 1, false, isScaled);
        timer.Start();
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
There are three ways to install this package into your Unity project:

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

### Option 3: Downloading .unitypackage Release
1. Download the latest `.unitypackage` from the [Releases](https://github.com/eldemirberkay0/Unity-FlexTimer/releases) page.
2. Drag and drop it into your project.