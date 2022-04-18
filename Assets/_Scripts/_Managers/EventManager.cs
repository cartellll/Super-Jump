namespace SuperStars.Events
{
    using System.Collections.Generic;
    using System;
    using UnityEngine;

    public class EventManager : MonoBehaviour
    {
        private static Dictionary<string, object> _0paramEvents = new Dictionary<string, object>();
        private static Dictionary<string, object> _1paramEvents = new Dictionary<string, object>();
        private static Dictionary<string, object> _2paramEvents = new Dictionary<string, object>();
        private static Dictionary<string, object> _3paramEvents = new Dictionary<string, object>();

        #region 0Param

        public static void StartListening(string Event_ID, Action Event_Trigger)
        {
            var hasData = _0paramEvents.TryGetValue(Event_ID, out var actions);

            if (!hasData)
            {
                //Add Data
                List<Action> newList = new List<Action> { Event_Trigger };
                _0paramEvents.Add(Event_ID, newList);
            }
            else
            {
                //Add to Actions list
                if (!((List<Action>)actions).Contains(Event_Trigger)) ((List<Action>)actions).Add(Event_Trigger);
            }
        }

        public static void StopListening(string Event_ID, Action Event_Trigger)
        {
            var hasData = _0paramEvents.TryGetValue(Event_ID, out var actions);

            if (!hasData) return;
            //Remove from list
            if (((List<Action>)actions).Contains(Event_Trigger)) ((List<Action>)actions).Remove(Event_Trigger);
        }

        public static void TriggerEvent(string Event_ID)
        {
            var hasData = _0paramEvents.TryGetValue(Event_ID, out var actions);
            if (!hasData) return;

            Action[] tempActions = new Action[((List<Action>)actions).Count];
            ((List<Action>)actions).CopyTo(tempActions);

            foreach (var action in tempActions)
            {
                action.Invoke();
            }
        }

        #endregion

        #region 1Param

        public static void StartListening<T>(string Event_ID, Action<T> Event_Trigger)
        {
            var hasData = _1paramEvents.TryGetValue(Event_ID, out var actions);

            if (!hasData)
            {
                //Add Data
                List<Action<T>> newList = new List<Action<T>> { Event_Trigger };
                _1paramEvents.Add(Event_ID, newList);
            }
            else
            {
                //Add to Actions list
                if (!((List<Action<T>>)actions).Contains(Event_Trigger)) ((List<Action<T>>)actions).Add(Event_Trigger);
            }
        }

        public static void StopListening<T>(string Event_ID, Action<T> Event_Trigger)
        {
            var hasData = _1paramEvents.TryGetValue(Event_ID, out var actions);

            if (!hasData) return;
            //Remove from list
            if (((List<Action<T>>)actions).Contains(Event_Trigger)) ((List<Action<T>>)actions).Remove(Event_Trigger);
        }

        public static void TriggerEvent<T>(string Event_ID, T val)
        {
            var hasData = _1paramEvents.TryGetValue(Event_ID, out var actions);
            if (!hasData) return;

            Action<T>[] tempActions = new Action<T>[((List<Action<T>>)actions).Count];
            ((List<Action<T>>)actions).CopyTo(tempActions);

            foreach (var action in tempActions)
            {
                action.Invoke(val);
            }
        }

        #endregion

        #region 2Params

        public static void StartListening<T, Y>(string Event_ID, Action<T, Y> Event_Trigger)
        {
            var hasData = _2paramEvents.TryGetValue(Event_ID, out var actions);

            if (!hasData)
            {
                //Add Data
                List<Action<T, Y>> newList = new List<Action<T, Y>> { Event_Trigger };
                _2paramEvents.Add(Event_ID, newList);
            }
            else
            {
                //Add to Actions list
                if (!((List<Action<T, Y>>)actions).Contains(Event_Trigger)) ((List<Action<T, Y>>)actions).Add(Event_Trigger);
            }
        }

        public static void StopListening<T, Y>(string Event_ID, Action<T, Y> Event_Trigger)
        {
            var hasData = _2paramEvents.TryGetValue(Event_ID, out var actions);

            if (!hasData) return;
            //Remove from list
            if (((List<Action<T, Y>>)actions).Contains(Event_Trigger)) ((List<Action<T, Y>>)actions).Remove(Event_Trigger);
        }

        public static void TriggerEvent<T, Y>(string Event_ID, T val, Y val1)
        {
            var hasData = _2paramEvents.TryGetValue(Event_ID, out var actions);
            if (!hasData) return;
            Action<T, Y>[] tempActions = new Action<T, Y>[((List<Action<T, Y>>)actions).Count];
            ((List<Action<T, Y>>)actions).CopyTo(tempActions);
            foreach (var action in tempActions)
            {
                action.Invoke(val, val1);
            }
        }

        #endregion


        #region 3Params

        public static void StartListening<T, Y, X>(string Event_ID, Action<T, Y, X> Event_Trigger)
        {
            var hasData = _3paramEvents.TryGetValue(Event_ID, out var actions);

            if (!hasData)
            {
                //Add Data
                List<Action<T, Y, X>> newList = new List<Action<T, Y, X>> { Event_Trigger };
                _3paramEvents.Add(Event_ID, newList);
            }
            else
            {
                //Add to Actions list
                if (!((List<Action<T, Y, X>>)actions).Contains(Event_Trigger)) ((List<Action<T, Y, X>>)actions).Add(Event_Trigger);
            }
        }

        public static void StopListening<T, Y, X>(string Event_ID, Action<T, Y, X> Event_Trigger)
        {
            var hasData = _3paramEvents.TryGetValue(Event_ID, out var actions);

            if (!hasData) return;
            //Remove from list
            if (((List<Action<T, Y, X>>)actions).Contains(Event_Trigger)) ((List<Action<T, Y, X>>)actions).Remove(Event_Trigger);
        }

        public static void TriggerEvent<T, Y, X>(string Event_ID, T val, Y val1, X val2)
        {
            var hasData = _3paramEvents.TryGetValue(Event_ID, out var actions);
            if (!hasData) return;
            foreach (var action in (List<Action<T, Y, X>>)actions)
            {
                action.Invoke(val, val1, val2);
            }

            /* Action<T, Y, X>[] tempActions = new Action<T, Y, X>[((List<Action<T, Y, X>>)actions).Count];
            ((List<Action<T, Y, X>>)actions).CopyTo(tempActions);
            foreach (var action in tempActions)
            {
                action.Invoke(val, val1, val2);
            }*/
        }

        #endregion
    }

}
