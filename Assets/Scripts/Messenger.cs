using System.Collections.Generic;

namespace HyperGames {

    // TODO: Make this an interface instead?
    public class Message {}

    public class Messenger {
        public delegate void MessageDelegate<T> (T msg) where T : Message;
        private delegate void MessageDelegate (Message msg);

        static private Dictionary<System.Type, MessageDelegate> delegates = new Dictionary<System.Type, MessageDelegate>();
        static private Dictionary<System.Delegate, MessageDelegate> delegateLookup = new Dictionary<System.Delegate, MessageDelegate>();

        static public void AddListener<T> (MessageDelegate<T> del) where T : Message {
            // Early-out if we've already registered this delegate
            if (delegateLookup.ContainsKey(del)) {
                return;
            }

            // Create a new non-generic delegate which calls our generic one.
            // This is the delegate we actually invoke.
            MessageDelegate internalDelegate = (msg) => del((T)msg);
            delegateLookup[del] = internalDelegate;

            MessageDelegate tempDel;
            if (delegates.TryGetValue(typeof(T), out tempDel)) {
                delegates[typeof(T)] = tempDel += internalDelegate;
            } else {
                delegates[typeof(T)] = internalDelegate;
            }
        }

        static public void RemoveListener<T> (MessageDelegate<T> del) where T : Message {

            MessageDelegate internalDelegate;
            if (delegateLookup.TryGetValue(del, out internalDelegate)) {

                MessageDelegate tempDel;
                if (delegates.TryGetValue(typeof(T), out tempDel)) {

                    tempDel -= internalDelegate;
                    if (tempDel == null) {
                        delegates.Remove(typeof(T));
                    } else {
                        delegates[typeof(T)] = tempDel;
                    }
                }

                delegateLookup.Remove(del);
            }
        }

        static public void Dispatch(Message msg) {
            MessageDelegate del;
            if (delegates.TryGetValue(msg.GetType(), out del)) {
                del.Invoke(msg);
            }
        }
    }
}