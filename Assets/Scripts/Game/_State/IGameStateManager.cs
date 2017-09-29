using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public abstract class IGameStateManager : MonoBehaviour {
    internal System.EventHandler<MessageEventArgs> onMessage;
    internal System.EventHandler<CloseEventArgs> onClose;

    internal AsyncOperation sceneLoad;

    private void OnEnable () {
        if (onMessage == null || onClose == null) SetupHandlers();
        if (onMessage != null) WebSocketManager.ws.OnMessage += onMessage;
        if (onClose != null) WebSocketManager.ws.OnClose += onClose;
    }

    internal abstract void SetupHandlers (); //used for registering handlers

    private void OnDisable () {
        WebSocketManager.ws.OnMessage -= onMessage;
        WebSocketManager.ws.OnClose -= onClose;
    }

    private void OnApplicationQuit () {
        WebSocketManager.Shutdown();
    }
}
