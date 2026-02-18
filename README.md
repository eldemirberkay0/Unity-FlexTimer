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
### Creating Timer Manually With Constructor
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
    /// <param name="attachedTo"> MonoBehaviour that timer attaches to. If this MonoBehaviour is destroyed, timer will cancel itself. </param>
    public Timer(float tickDuration, Action OnTick = null, Action OnFinished = null, Action OnUpdate = null, int tickCount = 1, bool isLooped = false, bool isScaled = true, MonoBehaviour attachedTo = null)
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
            this.OnUpdate += CheckAttachedObject;
        }
    }
```

While using with constructor you have to use timer.Start() manually to run the timer.

Timer.Start():
```csharp
    // Note: This function is a member of Timer.
    /// <summary> Registers timer to TimerManager and sets IsRunning true. </summary>
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
***
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
***
Attach timer to a MonoBehaviour if timer's actions have something to do with it. Because if MonoBehaviour is destroyed, timer can't access it and this will result with an error. Or you can call timer.Cancel() manually, which is *a bit* faster.

Example (Practical):
```csharp
    void Start()
    {
        // Create a timer attached to a MonoBehaviour so that if MonoBehaviour is destroyed, timer cancels itself.
        Timer timer = new Timer(2, () => gameObject.SetActive(false), attachedTo: this);
        // Starts the timer.
        timer.Start();
        // Destroy the gameObject that the timer is attached to. If the timer were not attached, it would throw a MissingReferenceException error when timer ticked.
        Destroy(gameObject);
    }
```
Example (Advanced):
```csharp
    Timer timer;

    void Start()
    {
        // Create the timer that is not attached to a MonoBehaviour, timer.Cancel() should be called manually
        timer = new Timer(2, () => gameObject.SetActive(false));
        // Start the timer
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
### Using TimerManager for Basic Needs
If you just want to trigger an action once after a delay you can use TimerManager.RegisterEvent(). This function creates a basic timer and starts it directly. This method prevents access to timer and reduces control over timer but it is more practical.

TimerManager.RegisterEvent():
```csharp
    /// <summary> Creates a timer with an event attached to it and starts timer directly. Practical use for basic needs. </summary>
    /// <param name="duration"> Duration (second) of timer. </param>
    /// <param name="action"> Invokes on timer tick. </param>
    /// <param name="attachedTo"> MonoBehaviour that timer attaches to. If this MonoBehaviour is destroyed, timer will cancel itself. </param>
    public static void RegisterEvent(float duration, Action action, MonoBehaviour attachedTo = null)
    {
        Timer timer = new Timer(duration, action, null, null, 1, false, true, attachedTo);
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