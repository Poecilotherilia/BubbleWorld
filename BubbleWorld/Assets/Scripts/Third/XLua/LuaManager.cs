/**
 *  @File: LuaManager.cs
 *  @Author: BoA
 *  @Date: 2023.11.30
 *  @Description: Lua管理器
 */
using System.Text;
using XLua;
using XLua.LuaDLL;
using System.IO;
using UnityEngine;

namespace BWFramework.XLua
{
    public static class LuaManager
    {
#if UNITY_EDITOR
        /// <summary>
        /// 项目路径
        /// </summary>
        private static readonly string ProjectPath = Directory.GetCurrentDirectory().Replace("\\", "/") + "/";
        
#endif
        
        /// <summary>
        /// 全局LuaEnv
        /// </summary>
        private static LuaEnv _luaEnv;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="luaLoader">加载器</param>
        /// <returns></returns>
        public static void Init(LuaEnv.CustomLoader luaLoader = null)
        {
            _luaEnv = new LuaEnv();
            //todu:
            // _luaEnv.AddBuildin("rapidjson", Lua.LoadRapidJson);
            // _luaEnv.AddBuildin("pb", Lua.LoadLuaProtobuf);

            if (luaLoader != null)
            {
                _luaEnv.AddLoader(luaLoader);
            }

            _luaEnv.AddLoader(LuaLoader);
        }
    
        /// <summary>
        /// 获取Lua脚本资源路径
        /// </summary>
        /// <param name="scriptName">Lua脚本名称</param>
        /// <returns></returns>
        public static string GetLuaAssetPath(string scriptName)
        {
            return $"Assets/Lua/{scriptName}.lua.txt";
        }
        
        /// <summary>
        /// 加载Lua脚本资源
        /// </summary>
        /// <param name="luaFile"></param>
        /// <returns></returns>
        private static byte[] LuaLoader(ref string luaFile)
        {
            var luaPath = GetLuaAssetPath(luaFile);
#if UNITY_EDITOR
            if (luaFile.Contains("//"))
            {
                Debug.LogError($"the luaFile error, luaFile = {luaFile}");
            }

            luaFile = ProjectPath + luaPath;

            return File.ReadAllBytes(luaFile);
#else
            //todu:assetbundle写好后写bundle环境的lua加载
#endif
        }
        
        /// <summary>
        /// 执行Lua
        /// </summary>  
        /// <param name="luaString"></param>
        /// <returns></returns>
        public static LuaTable DoString(string luaString)
        {
            var objs = _luaEnv.DoString(luaString);
            return (LuaTable)objs[0];
        }
    }
}