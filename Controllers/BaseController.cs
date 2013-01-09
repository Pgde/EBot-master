using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EveModel;

namespace Controllers
{
    public abstract class BaseController : IController
    {
        Random _random;

        protected BaseController()
        {
            _random = new Random();
        }
        /// <summary>
        /// Holds the controllers own pulse for local delays
        /// </summary>
        protected DateTime _localPulse;

        protected bool CanWork { get { return DateTime.Now > _localPulse | DateTime.Now < Frame.Client.Session.NextSessionChange; } }

        public abstract void DoWork();
        /// <summary>
        /// Returns whether the controller has finished its work
        /// </summary>
        public bool IsWorkDone { get; set; }

        protected int GetRandom(int minValue, int maxValue)
        {
            return _random.Next(minValue, maxValue);
        }
        protected DateTime GetDelay(int minDelayInSeconds, int maxDelayInSeconds)
        {
            return DateTime.Now.AddMilliseconds(GetRandom(minDelayInSeconds * 1000, maxDelayInSeconds * 1000));
        }
    }
}
