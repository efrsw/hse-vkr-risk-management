using MathNet.Numerics.Distributions;
using MathNet.Numerics.Random;
using System.Collections.Generic;
using System.Data;

namespace VKR_BlazorWASM.Models
{
    public class PSimulation
    {
        public int NumberOfSimulations { get; set; } = 1000;
        public Project SimulationProject { get; set; } = new();
        public int DistributionType { get; set; } = 1;
        public int NumberOfBins { get; set; } = 15;
        public Dictionary<string, double[]> SimulationDict { get; set; } = new();
        public List<DataItem> SimulationData { get; set; } = new();
        public int[] SimulationHistogram { get; set; }
        public List<int> SimulationDurations { get; set; } = new();
        public double SuccessProbability { get; set; }

        public void SimulateOutcomes()
        {
            switch (DistributionType)
            {
                case 1:
                    SimulationHistogram = new int[NumberOfBins];

                    foreach (var ptask in SimulationProject.Tasks)
                    {
                        if (ptask.Slack == 0)
                        {
                            var random = new Normal(
                                                    (ptask.ExpectedDurationLowerBound + ptask.ExpectedDurationUpperBound) / 2,
                                                    (double)((ptask.ExpectedDurationUpperBound - ptask.ExpectedDurationLowerBound) / 3.29));

                            SimulationDict.Add(ptask.Id, new double[NumberOfSimulations]);
                            random.Samples(SimulationDict[ptask.Id]);
                        }
                    }

                    for (int i = 0; i < NumberOfSimulations; i++)
                    {
                        var _t = 0d;
                        foreach (var key in SimulationDict.Keys)
                        {
                            _t += SimulationDict[key][i];
                        }

                        SimulationDurations.Add((int)_t);
                    }

                    for(int i = 0; i < NumberOfBins; i++)
                    {
                        SimulationHistogram[i] = i * (SimulationDurations.Max() / NumberOfBins);
                    }

                    for (int i = 0; i < SimulationHistogram.Length - 1; i++)
                    {
                        var lowerBound = SimulationHistogram[i];
                        var upperBound = SimulationHistogram[i + 1];

                        SimulationData.Add(new DataItem
                        {
                            Bin = lowerBound,
                            Amount = SimulationDurations.Where(x => x > lowerBound && x < upperBound).Count(),
                        });
                    }

                    var expectedDuration = SimulationProject.Tasks
                        .Where(x => x.Slack == 0)
                        .Sum(x => x.Duration);

                    SuccessProbability = SimulationDurations
                        .Where(x => x <= expectedDuration)
                        .Count();

                    SuccessProbability /= NumberOfSimulations;

                    Console.Write(SuccessProbability);

                    break;

                default:
                    break;
            }
        }
    }
}
