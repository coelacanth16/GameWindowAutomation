using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GameWindowAutomation.Core
{
    public class GameStateTransition
    {
        public static double LearningPressure = .05;

        public double ExpectedDelaySeconds = .5;

        public double ReCheckSeconds = .2;

        public double ProbabilityToEnter = 1;

        public GameWindowState GameState;

        public bool DynamicallyAdjustDelays = true;
        
        public async Task<GameWindowState> TryTransition(GameInstanceWindow instance, CancellationToken cancelToken)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            await Task.Delay(TimeSpan.FromSeconds(ExpectedDelaySeconds), cancelToken);
            int cycleNum = 0;
            do
            {
                if (GameState.IsMatch(instance.LastFullScan))
                {
                    stopWatch.Stop();
                    if (DynamicallyAdjustDelays)
                    {
                        if (cycleNum > 0)
                            ExpectedDelaySeconds += ExpectedDelaySeconds * LearningPressure * cycleNum;
                        else
                            ExpectedDelaySeconds -= ExpectedDelaySeconds * LearningPressure;
                    }
                    return GameState;
                }
                instance.NextScan.Wait(cancelToken);
                cycleNum++;
            } while (!cancelToken.IsCancellationRequested);
            throw new OperationCanceledException();
        }
    }
}
