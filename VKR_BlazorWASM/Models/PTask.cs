using System.Diagnostics;

namespace VKR_BlazorWASM.Models
{
    public class PTask
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public int ExpectedDuration { get; set; }
        public int ExpectedDurationLowerBound { get; set; }
        public int ExpectedDurationUpperBound { get; set; }
        public int EarliestFinishTime { get; set; }
        public int LatestFinishTime { get; set; }
        public int EarliestStartTime { get; set; }
        public int LatestStartTime { get; set; }
        public int Slack { get; set; } = -1;
        public List<PTask> Successors { get; set; } = new();
        public List<PTask> Predecessors { get; set; } = new();
        public List<PResource> Resources { get; set; } = new();
    }
}
