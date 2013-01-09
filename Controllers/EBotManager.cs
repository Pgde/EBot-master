using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Controllers
{
    public class EBotManager : IDisposable
    {
        EveModel.Frame _eveFrame;
        DateTime _controllerPulse;
        Random _random;
        int _minPulse, _maxpulse;
        List<IController> _controllerList;

        public EBotManager(int minPulse = 1500, int maxPulse = 4000)
        {
            _minPulse = minPulse;
            _maxpulse = maxPulse;
            _controllerPulse = DateTime.MinValue;
            _random = new Random();
            _controllerList = new List<IController>();

            _eveFrame = new EveModel.Frame();
            _eveFrame.OnFrame += new EventHandler(_eveFrame_OnFrame);
        }

        void _eveFrame_OnFrame(object sender, EventArgs e)
        {
            if (DateTime.Now < NextPulse)
                return;
            var modal = EveModel.Frame.Client.GetWindows.Where(w => w.IsModal).FirstOrDefault();
            if (modal != null)
            {
                EveModel.Frame.Log("Closing modal window: " + modal.GetPopUpMessage);
                modal.Close();
                return;
            }
            foreach (var controller in _controllerList)
            {
                if (controller != null && !controller.IsWorkDone)
                {
                    controller.DoWork();
                }
            }
        }
        public void AddController(IController controller)
        {
            if (!_controllerList.Contains(controller))
                _controllerList.Add(controller);
        }
        public void RemoveController(IController controller)
        {
            _controllerList.Remove(controller);
        }
        public void Dispose()
        {
            _eveFrame.OnFrame -= new EventHandler(_eveFrame_OnFrame);
            _eveFrame.Dispose();
        }
        public DateTime NextPulse
        {
            set { _controllerPulse = value; }
            get
            {
                DateTime returnValue = _controllerPulse;
                if (DateTime.Now >= _controllerPulse)
                {
                    _controllerPulse = DateTime.Now.AddMilliseconds(_random.Next(_minPulse, _maxpulse));
                }
                return returnValue;
            }
        }
    }
}
