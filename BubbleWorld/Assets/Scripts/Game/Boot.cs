/**
 *  @File: Boot.cs
 *  @Author: BoA
 *  @Date: 2023.11.30
 *  @Description: 启动脚本
 */

using System;
using System.Collections;
using BWFramework.XLua;
using Unity.VisualScripting;
using UnityEngine;

namespace Game
{
    public class Boot : MonoBehaviour
    {
        
        private Action _onStart;
        private Action _onUpdate;
        private Action _onApplicationQuit;
        
        private IEnumerator Start()
        {
            LuaManager.Init();

            var boot = LuaManager.DoString("return require 'game/boot'");
            Debug.LogError(boot);
            _onStart = boot.Get<Action>("on_start");
            Debug.LogError(_onStart);
            _onUpdate = boot.Get<Action>("on_update");
            _onApplicationQuit = boot.Get<Action>("on_application_quit");
            _onStart?.Invoke();
            yield return 0;
        }
    }
}