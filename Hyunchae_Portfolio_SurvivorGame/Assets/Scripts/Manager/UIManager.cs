using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    private const int DEFAULTLAYER = 100;

    private Dictionary<Type, UIBaseController> uiPanelDict = new Dictionary<Type, UIBaseController>();
    
    private Stack<UIBaseController> uiPanelStack = new Stack<UIBaseController>();

    private int curLayer = 0;

    public T Show<T>(string _panelPath = "") where T : UIBaseController
    {
        var panel = GetCachedPanel<T>(_panelPath);

        uiPanelStack.Push(panel);
        panel.Show();

        panel.SetSortOrder(DEFAULTLAYER + curLayer);
        curLayer++;

        return (T)panel;
    }

    public void Hide()
    {
        if(uiPanelStack.Count > 0)
        {
            UIBaseController ui = uiPanelStack.Pop();
            ui.Hide();

            curLayer--;
        }
        else
        {
            Debug.Log("Not exist Showing UIPanel");
        }
    }

    public T GetCachedPanel<T>(string _panelPath = "") where T : UIBaseController
    {
        uiPanelDict.TryGetValue(typeof(T), out var panel);

        if(panel == null)
        {
            panel = AddCachePanel<T>(_panelPath);
        }

        return (T)panel;
    }

    public T AddCachePanel<T>(string _panelPath = "") where T : UIBaseController
    {
        var panel = Find<T>();
        if(panel == null)
        {
            panel = CreatePanel<T>(_panelPath);
        }

        return panel;
    }

    public T CreatePanel<T>(string _panelPath = "") where T : UIBaseController
    {
        var originPrefab = Resources.Load(_panelPath, typeof(GameObject)) as GameObject;
        var panelObj = GameObject.Instantiate(originPrefab);

        return panelObj.GetComponent<T>();
    }

    public void UnloadScene()
    {
        uiPanelStack.Clear();
        uiPanelDict.Clear();
    }

    public T Find<T>() where T : UIBaseController
    {
        T ui = UnityEngine.GameObject.FindObjectOfType<T>();

        return ui;
    }
}