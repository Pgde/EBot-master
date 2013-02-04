using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Controllers;
using Controllers.states;
using System.Diagnostics;

namespace EBotPilot
{
    public partial class Form1 : Form
    {
        EBotManager _manager;

        #region Old Code
        //Frame eveFrame;
        //DateTime pulse;
        //Random random;
        //bool runOnce;
        #endregion

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
         //   MessageBox.Show("bing");
            rgcheck();
            System.Threading.Thread.Sleep(10000);
            _manager = new EBotManager();
            Controllers.Settings.Settings.Instance.LoadSettings();
            System.Threading.Thread.Sleep(10);
            _manager.AddController(new logincontroller());
            _manager.AddController(new MiningController());
            _manager.AddController(new DroneController());
            _manager.AddController(new SkillController());
            _manager.AddController(new TravelController());
            _manager.AddController(new MainController());
            _manager.AddController(new BuyController());
            _manager.AddController(new fittingcontroller());
            _manager.AddController(new MysqlController());
            #region Old Code
            //// Intialize
            //pulse = DateTime.MinValue;
            //random = new Random();

            //// Start up our interrupt of the eve frame
            //eveFrame = new EveModel.Frame();
            //eveFrame.OnFrame += new EventHandler(eveFrame_OnFrame);
            #endregion
        }

        private void button1_Click(object sender, EventArgs e)
        {
        //    _manager.AddController(new TravelController());
        //    TravelController.desti = 60003760;
        //    _States.TravelerState = TravelerState.Initialise;
         //   _States.tutstates = tutstates.wait;
         //   _manager.AddController(new tutcontroller());
            _States.maincontrollerState = maincontrollerStates.endminingcycle;
         //   _States.MiningState = MiningState.unload;
         //   _manager.AddController(new fittingcontroller());
        }

        private void button2_Click(object sender, EventArgs e)
        {
      //      _manager.AddController(new MysqlController());
     //       _manager.AddController(new MiningController());
      //      _manager.AddController(new SkillController());
       //   _manager.AddController(new DroneController());
          //  _States.SkillState = SkillState.Initialise;
         //   _States.MiningState = MiningState.Initialise;
            _manager.AddController(new tutcontroller());
            _manager.AddController(new BuyController());
            _manager.AddController(new TravelController());
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            _manager.AddController(new logincontroller());
            _manager.AddController(new MiningController());
            _manager.AddController(new DroneController());
            _manager.AddController(new SkillController());
            _manager.AddController(new TravelController());
            _manager.AddController(new MainController());
            _manager.AddController(new BuyController());
            _manager.AddController(new fittingcontroller());
            _manager.AddController(new MysqlController());
        }

        private void button4_Click(object sender, EventArgs e)
        {
      //      _States.maincontrollerState = maincontrollerStates.Startup;
            _manager.AddController(new TravelController());
            _manager.AddController(new BuyController());
            _manager.AddController(new tutcontroller());
            _manager.AddController(new SkillController());
            _manager.AddController(new DroneController());
     //       _States.BuyControllerState = BuyControllerStates.done;
     //       _States.SkillState = SkillState.buyskill;
       //     _States.tutstates = tutstates.wait;
            _States.tutstates = tutstates.wait;
       //     _States.SkillState = SkillState.Initialise;
       }

        private void rgcheck()
        {
            bool rginj = false;
            Process currentProcess = Process.GetCurrentProcess();
            int pid = currentProcess.Id;
            ProcessModuleCollection t = currentProcess.Modules;
            foreach (ProcessModule x in t)
            {
                if (x.ModuleName == "rgdll.dll")
                {
                    rginj = true;
                }
            }
              if (rginj == false)
              {
                  if (Process.GetProcessesByName("rg").Any())
                  {
                      Process rg = Process.GetProcessesByName("rg").FirstOrDefault();
                      rg.Kill();
                  }
              /*    System.Threading.Thread.Sleep(10);
                  Process rgbin = new Process();
                  rgbin.StartInfo.FileName = "C:\\rgold\\rg.exe";
                  rgbin.StartInfo.Arguments = "";
                  rgbin.StartInfo.WorkingDirectory = "C:\\rgold";
                  rgbin.StartInfo.UseShellExecute = true;
                  
                  rgbin.Start();
                  System.Threading.Thread.Sleep(100);
                  
                  */
                  System.Threading.Thread.Sleep(6000);
                  currentProcess.Kill();
              }
            
        }
        #region Old Code
        //void eveFrame_OnFrame(object sender, EventArgs e)
        //{
            // Only make the wallet call once
            //if (runOnce)
            //    return;
            //runOnce = true;

            //// Obtain a refrence to the wallet service
            //var wallet = Frame.Client.GetService("wallet");

            //if (wallet.IsValid)
            //{
            //    eveFrame.Log("Found 'wallet' object with Intptr: " + wallet.PointerToObject);
            //    if (wallet["GetWealth"].IsValid)
            //    {
            //        eveFrame.Log("Found a 'GetWealth' attribute of type: " + wallet["GetWealth"].ObjectType);

            //        // Call the GetWealth method synchronously and interpret the result as a double
            //        EveObject balanceObj = wallet.CallMethod("GetWealth", new object[0]);
            //        eveFrame.Log("Found 'GetWealth' return type as: " + wallet.ObjectType);
            //        double balance = balanceObj.GetValueAs<double>();

            //        // Log balance to the innerspace console
            //        eveFrame.Log("Your balance is: " + balance.ToString("N"));
            //    }
            //}
            //else
            //{
            //    eveFrame.Log("Wallet service invalid, please start it first and try again");
            //}
        //}
        #endregion
    }
}
