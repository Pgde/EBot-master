using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LavishVMAPI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Reflection;

namespace EveModel
{
    public class Frame
    {
        public delegate void Message(string msg);
        public static event Message OnMessage;

        public event EventHandler OnFrame;
        public static EveClient Client { get; private set; }


        public static bool LogToInnerspaceConsole { get; set; }
        static Frame()
        {
            LogToInnerspaceConsole = true;
        }

        public Frame()
        {
            LavishScriptAPI.LavishScript.Events.AttachEventTarget("OnFrame", OnInnerspaceFrame);
        }
        /// <summary>
        /// This method will be called for every frame captured by Innerspace
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="lse"></param>
        void OnInnerspaceFrame(object sender, LavishScriptAPI.LSEventArgs lse)
        {
            using (new FrameLock(true))
            {
                using (EveClient _client = new EveClient())
                {
                    Client = _client;
                    if (OnFrame != null)
                        OnFrame(this, new EventArgs());
                    Client = null;
                }
            }
        }
        public void Dispose()
        {
            LavishScriptAPI.LavishScript.Events.DetachEventTarget("OnFrame", OnInnerspaceFrame);
        }
        /// <summary>
        /// Log to Innerspace Console
        /// </summary>
        /// <param name="text">Text to log</param>
        public static void Log(object text)
        {
            if (OnMessage != null)
            {
                OnMessage(DateTime.Now.ToString("hh:mm:ss") + " " + text.ToString());
            }
            if (LogToInnerspaceConsole)
            {
                InnerSpaceAPI.InnerSpace.Echo(DateTime.Now.ToString("hh:mm:ss") + " " + text.ToString());
            }
        }
        /// <summary>
        /// Logs an objects property names and values
        /// </summary>
        /// <param name="obj">Object whos properties should be exposed</param>
        public static void ExposeObject(object obj)
        {
            Log("Exposing: " + obj.GetType().Name);
            foreach (var item in obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                var property = obj.GetType().GetProperty(item.Name);
                var index = property.GetIndexParameters().Count();
                if (index == 0)
                {
                    Log(item.Name + ": " + obj.GetType().GetProperty(item.Name).GetValue(obj, null));
                }
                else
                {
                    Log(item.Name + ": " + obj.GetType().GetProperty(item.Name).PropertyType + "[" + index + "]");
                }
            }
        }
    }
}
