using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Timing.Common
{
    public class Event : IPoolObject
    {
        public Delegate Action;
        public bool Removed;
        public void Reset()
        {
            Action = default;
            Removed = default;
        }
    }
    public abstract class EventManager<T> : Singleton<T> where T : new()
    {
        private readonly Dictionary<int, List<Event>> mEventListeners = new Dictionary<int, List<Event>>();
        private readonly Dictionary<int, List<Event>> mWaitRemoveListener = new Dictionary<int, List<Event>>();
        private int mDispatchRef;

        protected abstract Event GetEvent();
        protected abstract void ReturnEvent(Event e);
        
        private void AddEventListener(int key, Delegate callback)
        {
            var evt = GetEvent();
            evt.Action = callback;
            evt.Removed = false;

            if (!mEventListeners.TryGetValue(key, out var listener)) {
                listener = new List<Event>();
                mEventListeners.Add(key, listener);
            }

            listener.Add(evt);
        }
        private void RemoveEventListener(int key, Delegate callback)
        {
            if (!mEventListeners.TryGetValue(key, out var events)) { return; }

            for (var i = 0; i < events.Count; i++) {
                var evt = events[i];
                if (!evt.Removed && evt.Action == callback) {
                    if (mDispatchRef <= 0) {
                        events.RemoveAt(i);
                        ReturnEvent(evt);
                    }
                    else {
                        evt.Removed = true;
                        if (!mWaitRemoveListener.TryGetValue(key, out var listener)) {
                            listener = new List<Event>();
                            mWaitRemoveListener.Add(key, listener);
                        }

                        listener.Add(evt);
                    }

                    break;
                }
            }
        }

        #region add & remove
        public void AddEventListener(int key, Action callBack) { AddEventListener(key, (Delegate)callBack); }
        public void AddEventListener<T1>(int key, Action<T1> callBack) { AddEventListener(key, (Delegate)callBack); }
        public void AddEventListener<T1, T2>(int key, Action<T1, T2> callBack)
        {
            AddEventListener(key, (Delegate)callBack);
        }
        public void AddEventListener<T1, T2, T3>(int key, Action<T1, T2, T3> callBack)
        {
            AddEventListener(key, (Delegate)callBack);
        }
        public void AddEventListener<T1, T2, T3, T4>(int key, Action<T1, T2, T3, T4> callBack)
        {
            AddEventListener(key, (Delegate)callBack);
        }
        public void AddEventListener<T1, T2, T3, T4, T5>(int key, Action<T1, T2, T3, T4, T5> callBack)
        {
            AddEventListener(key, (Delegate)callBack);
        }
        public void AddEventListener<T1, T2, T3, T4, T5, T6>(int key, Action<T1, T2, T3, T4, T5, T6> callBack)
        {
            AddEventListener(key, (Delegate)callBack);
        }
        public void AddEventListener<T1, T2, T3, T4, T5, T6, T7>(int key, Action<T1, T2, T3, T4, T5, T6, T7> callBack)
        {
            AddEventListener(key, (Delegate)callBack);
        }
        public void RemoveListener(int key, Action callBack) { RemoveEventListener(key, callBack); }
        public void RemoveListener<T1>(int key, Action<T1> callBack) { RemoveEventListener(key, callBack); }
        public void RemoveListener<T1, T2>(int key, Action<T1, T2> callBack) { RemoveEventListener(key, callBack); }
        public void RemoveListener<T1, T2, T3>(int key, Action<T1, T2, T3> callBack)
        {
            RemoveEventListener(key, callBack);
        }
        public void RemoveListener<T1, T2, T3, T4>(int key, Action<T1, T2, T3, T4> callBack)
        {
            RemoveEventListener(key, callBack);
        }
        public void RemoveListener<T1, T2, T3, T4, T5>(int key, Action<T1, T2, T3, T4, T5> callBack)
        {
            RemoveEventListener(key, callBack);
        }
        public void RemoveListener<T1, T2, T3, T4, T5, T6>(int key, Action<T1, T2, T3, T4, T5, T6> callBack)
        {
            RemoveEventListener(key, callBack);
        }
        #endregion
        #region dispatch
        public void DispatchEvent<T1, T2, T3, T4, T5, T6>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            if (mEventListeners.TryGetValue(key, out var events)) {
                mDispatchRef++;
                for (var i = 0; i < events.Count; i++) {
                    var evt = events[i];
                    if (!evt.Removed) {
                        if (evt.Action is Action<T1, T2, T3, T4, T5, T6> action) {
                            action(arg1, arg2, arg3, arg4, arg5, arg6);
                        }
                        else { SLogger.Error($"Listener Method{evt.Action.GetMethodInfo()} Not Match Event:{key} "); }
                    }
                }

                mDispatchRef--;
            }
        }
        public void DispatchEvent<T1, T2, T3, T4, T5>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            if (mEventListeners.TryGetValue(key, out var events)) {
                mDispatchRef++;
                for (var i = 0; i < events.Count; i++) {
                    var evt = events[i];
                    if (!evt.Removed) {
                        if (evt.Action is Action<T1, T2, T3, T4, T5> action) { action(arg1, arg2, arg3, arg4, arg5); }
                        else { SLogger.Error($"Listener Method{evt.Action.GetMethodInfo()} Not Match Event:{key} "); }
                    }
                }

                mDispatchRef--;
            }
        }
        public void DispatchEvent<T1, T2, T3, T4>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            if (mEventListeners.TryGetValue(key, out var events)) {
                mDispatchRef++;
                for (var i = 0; i < events.Count; i++) {
                    var evt = events[i];
                    if (!evt.Removed) {
                        if (evt.Action is Action<T1, T2, T3, T4> action) { action(arg1, arg2, arg3, arg4); }
                        else { SLogger.Error($"Listener Method{evt.Action.GetMethodInfo()} Not Match Event:{key} "); }
                    }
                }

                mDispatchRef--;
            }
        }
        public void DispatchEvent<T1, T2, T3>(int key, T1 arg1, T2 arg2, T3 arg3)
        {
            if (mEventListeners.TryGetValue(key, out var events)) {
                mDispatchRef++;
                for (var i = 0; i < events.Count; i++) {
                    var evt = events[i];
                    if (!evt.Removed) {
                        if (evt.Action is Action<T1, T2, T3> action) { action(arg1, arg2, arg3); }
                        else { SLogger.Error($"Listener Method{evt.Action.GetMethodInfo()} Not Match Event:{key} "); }
                    }
                }

                mDispatchRef--;
            }
        }
        public void DispatchEvent<T1, T2>(int key, T1 arg1, T2 arg2)
        {
            if (mEventListeners.TryGetValue(key, out var events)) {
                mDispatchRef++;
                for (var i = 0; i < events.Count; i++) {
                    var evt = events[i];
                    if (!evt.Removed) {
                        if (evt.Action is Action<T1, T2> action) { action(arg1, arg2); }
                        else { SLogger.Error($"Listener Method{evt.Action.GetMethodInfo()} Not Match Event:{key} "); }
                    }
                }

                mDispatchRef--;
            }
        }
        public void DispatchEvent<T1>(int key, T1 arg1)
        {
            if (mEventListeners.TryGetValue(key, out var events)) {
                mDispatchRef++;
                for (var i = 0; i < events.Count; i++) {
                    var evt = events[i];
                    if (!evt.Removed) {
                        if (evt.Action is Action<T1> action) { action(arg1); }
                        else { SLogger.Error($"Listener Method{evt.Action.GetMethodInfo()} Not Match Event:{key} "); }
                    }
                }

                mDispatchRef--;
            }
        }
        public void DispatchEventWithArgInt(int key, int arg1)
        {
            if (mEventListeners.TryGetValue(key, out var events)) {
                mDispatchRef++;
                for (var i = 0; i < events.Count; i++) {
                    var evt = events[i];
                    if (!evt.Removed) {
                        if (evt.Action is Action<int> action) { action(arg1); }
                        else { SLogger.Error($"Listener Method{evt.Action.GetMethodInfo()} Not Match Event:{key} "); }
                    }
                }

                mDispatchRef--;
            }
        }
        public void DispatchEvent(int key)
        {
            if (mEventListeners.TryGetValue(key, out var events)) {
                mDispatchRef++;
                for (var i = 0; i < events.Count; i++) {
                    var evt = events[i];
                    if (!evt.Removed) {
                        if (evt.Action is Action action) { action(); }
                        else { SLogger.Error($"Listener Method{evt.Action.GetMethodInfo()} Not Match Event:{key} "); }
                    }
                }
                mDispatchRef--;
            }
        }
        #endregion
        public virtual void LateUpdate()
        {
            foreach (var pair in mWaitRemoveListener) {
                if (mEventListeners.TryGetValue(pair.Key, out var listeners)) {
                    foreach (var evt in pair.Value) {
                        listeners.Remove(evt);
                        ReturnEvent(evt);
                    }
                }
            }

            mWaitRemoveListener.Clear();
        }

        public virtual void Release()
        {
#if UNITY_EDITOR
            DumpAllListener();
#endif
        }

        private void DumpAllListener()
        {
            using (var enumerator = mEventListeners.GetEnumerator()) {
                while (enumerator.MoveNext()) {
                    var keyValuePair = enumerator.Current;
                    for (var i = 0; i < keyValuePair.Value.Count; i++) {
                        SLogger.Error($"局内事件未清除：{keyValuePair.Key}:{keyValuePair.Value[i].Action.GetMethodInfo()}");
                    }
                }
            }
        }
    }
}