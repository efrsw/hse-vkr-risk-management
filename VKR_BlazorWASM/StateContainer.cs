using VKR_BlazorWASM.Models;

namespace VKR_BlazorWASM
{
    public class StateContainer
    {
        private string? savedString;
        private Project currentProject = new();
        private PSimulation currentSimulation = new();

        public string Property
        {
            get => savedString ?? string.Empty;
            set
            {
                savedString = value;
                NotifyStateChanged();
            }
        }

        public Project CurrentProject
        {
            get => currentProject ?? new Project();
            set
            {
                currentProject = value;
                NotifyStateChanged();
            }
        }

        public PSimulation CurrentSimulation
        {
            get => currentSimulation ?? new PSimulation();
            set
            {
                currentSimulation = value;
                NotifyStateChanged();
            }
        }

        public event Action? OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
