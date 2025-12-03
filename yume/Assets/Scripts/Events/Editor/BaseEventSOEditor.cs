// 引入必要的命名空间：泛型集合、Unity编辑器工具、Unity引擎核心功能
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// 为泛型类BaseEventSO<T>创建自定义编辑器，使其在Inspector面板中显示自定义界面
[CustomEditor(typeof(BaseEventSO<>))]
public class BaseEventSOEditor<T> : Editor
{
    // 存储当前编辑的BaseEventSO<T>实例引用
    private BaseEventSO<T> baseEventSO;

    // 当编辑器启用时调用（如选中对象时）
    private void OnEnable()
    {
        // 如果尚未获取实例引用，则将目标对象转换为BaseEventSO<T>类型并赋值
        if(baseEventSO == null)
            baseEventSO = target as BaseEventSO<T>;
    }

    // 重写Inspector面板的绘制逻辑
    public override void OnInspectorGUI()
    {
        // 先绘制默认的Inspector界面（显示BaseEventSO<T>的公共字段，如description等）
        base.OnInspectorGUI();
        
        // 绘制标签显示当前事件的订阅者数量
        EditorGUILayout.LabelField("订阅数量：" + GetListeners().Count);

        // 遍历所有订阅者，在Inspector中显示每个订阅者的信息
        foreach (var listener in GetListeners())
        {
            // 以对象字段形式显示订阅者（MonoBehaviour类型），显示其名称和引用
            EditorGUILayout.ObjectField(listener.ToString(), listener, typeof(MonoBehaviour), true);
        }
    }
    
    // 获取所有订阅了当前事件的监听器（MonoBehaviour实例）
    private List<MonoBehaviour> GetListeners()
    {
        // 初始化一个空列表用于存储监听器
        List<MonoBehaviour> listeners = new List<MonoBehaviour>();
        
        // 安全检查：如果事件实例为空或没有订阅者，则直接返回空列表
        if(baseEventSO == null || baseEventSO.onEventRaised == null)
            return listeners;

        // 获取事件回调（onEventRaised）的所有订阅方法列表
        var subscribers = baseEventSO.onEventRaised.GetInvocationList();
        
        // 遍历所有订阅方法，提取其所属的MonoBehaviour实例
        foreach (var subscriber in subscribers)
        {
            // 将订阅方法的目标对象转换为MonoBehaviour（监听器通常挂载在MonoBehaviour上）
            var obj = subscriber.Target as MonoBehaviour;
            
            // 避免重复添加同一监听器，确保列表中只包含唯一实例
            if(!listeners.Contains(obj))
                listeners.Add(obj);
        }

        // 返回整理后的监听器列表
        return listeners;
    }
}