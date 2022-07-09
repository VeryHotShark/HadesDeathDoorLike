using System.Collections.Generic;

public class TimerManager : Singleton<TimerManager>, IUpdateListener {
    private List<Timer> _timers = new List<Timer>();
    
    private void OnEnable() => UpdateManager.AddUpdateListener(this);
    private void OnDisable() => UpdateManager.RemoveUpdateListener(this);

    public static void AddTimer(Timer timer ) {
        if(!Instance._timers.Contains(timer))
            Instance._timers.Add(timer);
    }
    
    public static void RemoveTimer(Timer timer ) => Instance._timers.Remove(timer);

    public void OnUpdate(float deltaTime) {
        for (int i = _timers.Count - 1; i >= 0; i--) {
            if(_timers[i].CheckTimerEnd())
                _timers.RemoveAt(i);
        }
    }
}