namespace ReflexDI.GameLoop
{
    using System;
    using UnityEngine;

    internal class GameLoopRunner : MonoBehaviour
    {
        private static GameLoopRunner instanceCached;

        internal static GameLoopRunner Instance
        {
            get
            {
                if (instanceCached != null) return instanceCached;
                instanceCached = new GameObject(nameof(GameLoopRunner)).AddComponent<GameLoopRunner>();
                DontDestroyOnLoad(instanceCached.gameObject);

                return instanceCached;
            }
        }

        private event Action FixedTickableAction;
        private event Action LateTickableAction;
        private event Action TickableAction;

        internal void RegisterFixedTickable(IFixedTickable fixedTickable)
        {
            this.FixedTickableAction += fixedTickable.FixedTick;
        }

        internal void RegisterTickable(ITickable tickable)
        {
            this.TickableAction += tickable.Tick;
        }

        internal void RegisterLateTickable(ILateTickable lateTickable)
        {
            this.LateTickableAction += lateTickable.LateTick;
        }

        internal void UnRegisterFixedTickable(IFixedTickable fixedTickable)
        {
            this.FixedTickableAction -= fixedTickable.FixedTick;
        }

        internal void UnRegisterTickable(ITickable tickable)
        {
            this.TickableAction -= tickable.Tick;
        }

        internal void UnRegisterLateTickable(ILateTickable lateTickable)
        {
            this.LateTickableAction -= lateTickable.LateTick;
        }

#region Mono Event

        private void FixedUpdate()
        {
            this.FixedTickableAction?.Invoke();
        }

        private void Update()
        {
            this.TickableAction?.Invoke();
        }

        private void LateUpdate()
        {
            this.LateTickableAction?.Invoke();
        }

#endregion
    }
}