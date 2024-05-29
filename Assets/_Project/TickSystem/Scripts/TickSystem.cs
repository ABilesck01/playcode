using System;
using UnityEngine;

public class TickSystem : MonoBehaviour
{
    public class OnTickEventArgs : EventArgs
    {
        public int tick;
    }

    public static event EventHandler<OnTickEventArgs> OnTick;

    private const float TICK_TIMER_MAX = 0.2f;

    private int tick;
    private float tickTimer;
    private bool countTicks = false;

    private void OnEnable()
    {
        LevelController.OnStartLevel += LevelController_OnStartLevel;
        LevelController.OnStopLevel += LevelController_OnStopLevel;
    }

    

    private void OnDisable()
    {
        LevelController.OnStartLevel -= LevelController_OnStartLevel;
        LevelController.OnStopLevel -= LevelController_OnStopLevel;
    }

    private void LevelController_OnStartLevel(object sender, System.EventArgs e)
    {
        countTicks = true;
    }

    private void LevelController_OnStopLevel(object sender, EventArgs e)
    {
        countTicks = false;
    }

    private void Awake()
    {
        tick = 0;
        tickTimer = 0;
    }

    private void Update()
    {
        if(!countTicks)
        {
            return;
        }

        tickTimer += Time.deltaTime;
        if(tickTimer >= TICK_TIMER_MAX )
        {
            tickTimer -= TICK_TIMER_MAX;
            tick++;
            OnTick?.Invoke(this, new OnTickEventArgs { tick = this.tick });
        }
    }
}
