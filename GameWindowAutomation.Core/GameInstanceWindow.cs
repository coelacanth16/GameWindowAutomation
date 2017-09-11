using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using WindowScrape.Types;

namespace GameWindowAutomation.Core
{
    public class GameInstanceWindow
    {
        public Dictionary<int, GameWindowState> AllStates = new Dictionary<int, GameWindowState>();

        public event EventHandler<string> MessageEvent;

        private HwndObject hwndObj = null;

        public HwndObject WindowInfo { get { return hwndObj; } }
        
        public Process WinProcess { get; private set; }

        private Bitmap fullScan = null;
        public Bitmap LastFullScan { get { return fullScan; } }

        private TaskCompletionSource<Bitmap> scanTaskSource = new TaskCompletionSource<Bitmap>();
        public Task NextScan { get { return scanTaskSource.Task; } }

        public GameWindowState CurrentState { get; private set; }

        public GameWindowState InitialState { get; set; }

        public Task WorkingTask { get; private set; }

        public double ScanRateSeconds { get; set; }
        
        private CancellationTokenSource cancelTokenSource = new CancellationTokenSource();

        public GameInstanceWindow(Process process)
        {
            hwndObj = new HwndObject(process.MainWindowHandle);            
            WorkingTask = null;
            CurrentState = null;
        }

        public void BeginMonitor()
        {
            if(WorkingTask != null)
                WorkingTask = Task.WhenAll(Monitor(), InitialState.PerformActionAndTransition(this, cancelTokenSource.Token));
        }

        public void InvokeMessageEvent(object sender, string message)
        {
            MessageEvent?.Invoke(sender, message);
        }

        public Bitmap DoScan()
        {
            lock (this)
            {
                if (fullScan == null || fullScan.Width != WindowInfo.Size.Width || fullScan.Height != WindowInfo.Size.Height)
                    fullScan = new Bitmap(WindowInfo.Size.Width, WindowInfo.Size.Height);
                Utilities.GetImageAt(new Point(WindowInfo.Location.X, WindowInfo.Location.Y), ref fullScan);
                scanTaskSource.TrySetResult(fullScan);
                scanTaskSource = new TaskCompletionSource<Bitmap>();
                return fullScan;
            }
        }

        private async Task Monitor()
        {
            try
            {
                while (!cancelTokenSource.IsCancellationRequested)
                {
                    DoScan();
                    await Task.Delay(TimeSpan.FromSeconds(ScanRateSeconds), cancelTokenSource.Token);
                }
            }
            catch(OperationCanceledException)
            {
            }
            catch(Exception ex)
            {
                InvokeMessageEvent(this, ex.ToString());
            }
        }

        public async Task Transition()
        {
            while(!cancelTokenSource.IsCancellationRequested)
                CurrentState = await CurrentState.PerformActionAndTransition(this, cancelTokenSource.Token);
        }

        public static List<GameInstanceWindow> GetWindowsByProcessName(string processName)
        {
            var list = new List<GameInstanceWindow>();
            Process[] processes = Process.GetProcessesByName(processName);            
            foreach (var process in processes)
                list.Add(new GameInstanceWindow(process));
            return list;
        }

        public static GameInstanceWindow GetWindowAtMouseCursor(Point point)
        {
            foreach(var process in Process.GetProcesses())
            {
                var hwndObj = new HwndObject(process.MainWindowHandle);
                if(hwndObj.Location.X < point.X && point.X < hwndObj.Location.X + hwndObj.Size.Width &&
                   hwndObj.Location.Y < point.Y && point.Y < hwndObj.Location.Y + hwndObj.Size.Height)
                {
                    return new GameInstanceWindow(process);
                }
            }
            return null;
        }
    }
}
