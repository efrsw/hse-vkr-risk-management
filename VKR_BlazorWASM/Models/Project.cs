using System.Diagnostics;
using System.Threading.Tasks;

namespace VKR_BlazorWASM.Models
{
    public class Project
    {
        public string ProjectName { get; set; } = "";
        public List<PTask> Tasks { get; set; } = new();
        public List<PResource> Resources { get; set; } = new();


        public List<PTask> GetCriticalPath()
        {
            ForwardPass();
            BackPass();
            FindSlack();
            return Tasks.Where(x => x.Slack == 0).ToList();
        }

        void FindSlack()
        {
            Tasks.ForEach(task =>
            {
                task.Slack = task.LatestFinishTime - task.EarliestFinishTime;
            });
        }

        void ForwardPass()
        {
            Tasks.ForEach(task =>
            {
                task.EarliestStartTime = task.Predecessors.Count == 0
                    ? 0
                    : task.Predecessors.Max(p => p.EarliestFinishTime);

                task.EarliestFinishTime = task.EarliestStartTime + task.Duration;
            });
        }

        void BackPass()
        {
            Tasks.AsEnumerable().Reverse().ToList().ForEach(task =>
            {
                task.LatestFinishTime = task.Successors.Count == 0
                    ? Tasks.Max(p => p.EarliestFinishTime)
                    : task.Successors.Min(p => p.LatestStartTime);

                task.LatestStartTime = task.LatestFinishTime - task.Duration;
            });
        }

        public static Project ReadProjectFromCsv(string[] lines, string projectName)
        {
            var project = new Project { ProjectName = projectName };
            var _uTasks = new Dictionary<string, PTask>();

            for (int i = 1; i < lines.Length; i++) // Start from index 1 to skip the header line
            {
                string line = lines[i];
                string[] values = line.Split(',');

                PTask task = new PTask();
                task.Id = values[0];
                task.Description = values[1];
                task.Duration = int.Parse(values[2]);

                _uTasks.Add(task.Id, task);
            }

            for (int i = 1; i < lines.Length; i++) // Start from index 1 to skip the header line
            {
                string line = lines[i];
                string[] values = line.Split(',');

                if (!string.IsNullOrEmpty(values[3]))
                {
                    string[] predIds = values[3].Split(';');
                    foreach (string predId in predIds)
                    {
                        _uTasks[values[0]]
                            .Predecessors.Add(_uTasks[predId]);

                        _uTasks[predId]
                            .Successors.Add(_uTasks[values[0]]);

                    }
                }

                project.Tasks.Add(_uTasks[values[0]]);
            }

            return project;
        }
    }
}
