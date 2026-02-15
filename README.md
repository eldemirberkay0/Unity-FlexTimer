# Unity-FlexTimer
A flexible timer package for Unity running on Unity player loop, meaning no MonoBehavior is required.

## Includes
* **TimerManager:** Holds timers in a list and updates them in every frame. Also responsible with some practical event registration.

* **Timer:** The actual timer class. Has various properties:
    * OnTick -> Invokes on each timer tick.
    * OnUpdate -> Invokes on timer update.
    * OnFinished -> An action invokes when no ticks left.
    * tickDuration -> Duration of each tick.
    * tickCount -> Number of ticks.
    * IsLooped -> Overrides tickCount and ticks forever if true.
    * IsScaled -> True if timer is using scaled deltaTime.
    * IsRunning -> True if timer is running.
    * TicksRemaining -> Number of ticks left.
    * And more to get timer's current situation like SecondsToTick, SecondsToFinish and normalized versions of these.

## How to Use
There are several options to use timer class. If you just want to trigger an action after a delay you can use TimerManager.RegisterEvent(). This function creates a timer and starts it directly. This method prevents access to timer and reduces control over timer but it is more practical.
```csharp
    void Start()
    {
        TimerManager.RegisterEvent(2, () => Debug.Log("Ticked"));
    } 

```