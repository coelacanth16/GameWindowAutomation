using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GameWindowAutomation.Core
{
    public class GameWindowState
    {
        private static volatile int GlobalIterator = 0;

        public int Id;

        public string Name;

        public Point ScanOffset = new Point(0,0);

        public Bitmap MatchMap = null;

        public int MaxColorThreshold = 300;

        public int MaxOffsetVariance = 20;

        public bool IsStable = true;

        public Action<GameInstanceWindow> NextAction = null;

        public double DelayBeforeActionSeconds = 1;
        
        public SortedList<double, GameStateTransition> ExpectedNextStates = new SortedList<double, GameStateTransition>();

        public GameWindowState() { }

        public static GameWindowState CreateNewGlobalState()
        {
            return new GameWindowState { Id = GlobalIterator++ };
        }
        
        public bool IsMatch(Bitmap scan, CancellationToken cancelToken = default(CancellationToken))
        {            
            for (int scanVariance = 0; scanVariance < MaxOffsetVariance; scanVariance++)
            {
                int threshold1 = MaxOffsetVariance;
                int threshold2 = scanVariance == 0 ? -1 : MaxOffsetVariance;
                for (int srcX = 0; srcX < MatchMap.Size.Width; srcX++)
                {
                    for (int srcY = 0; srcY < MatchMap.Size.Height; srcY++)
                    {
                        if (cancelToken.IsCancellationRequested)
                            return false;
                        var srcPixel = MatchMap.GetPixel(srcX, srcY);
                        var scanPixel = scan.GetPixel(srcX + ScanOffset.X + scanVariance, srcY + ScanOffset.Y + scanVariance);
                        var diff = Math.Abs(srcPixel.A - scanPixel.A) +
                            Math.Abs(srcPixel.B - scanPixel.B) +
                            Math.Abs(srcPixel.G - scanPixel.G) +
                            Math.Abs(srcPixel.R - scanPixel.R);
                        threshold1 -= diff;
                        if (scanVariance != 0)
                        {
                            scanPixel = scan.GetPixel(srcX + ScanOffset.X - scanVariance, srcY + ScanOffset.Y - scanVariance);
                            diff = Math.Abs(srcPixel.A - scanPixel.A) +
                                Math.Abs(srcPixel.B - scanPixel.B) +
                                Math.Abs(srcPixel.G - scanPixel.G) +
                                Math.Abs(srcPixel.R - scanPixel.R);
                            threshold2 -= diff;
                        }
                    }
                }
                if (threshold1 > 0 || threshold2 > 0)
                    return true;                
            }
            return false;
        }                

        public async Task<GameWindowState> PerformActionAndTransition(GameInstanceWindow instance, CancellationToken cancelToken = default(CancellationToken))
        {
            await Task.Delay(TimeSpan.FromSeconds(DelayBeforeActionSeconds), cancelToken);
            NextAction?.Invoke(instance);
            var tasks = new List<Task<GameWindowState>>();
            var childTaskCancel = CancellationTokenSource.CreateLinkedTokenSource(cancelToken);
            foreach(var possibleState in ExpectedNextStates.Values)
            {
                tasks.Add(possibleState.TryTransition(instance, childTaskCancel.Token));
            }
            var firstTask = await Task.WhenAny(tasks);
            var stateResult = await firstTask;
            childTaskCancel.Cancel();
            foreach(var task in tasks) //clean them up
            {
                try
                {
                    await task;
                    task.Dispose();
                }
                catch(OperationCanceledException) { }
                catch(Exception ex)
                {
                    instance.InvokeMessageEvent(this, ex.ToString());
                }
            }
            return stateResult;
        }
    }
}
