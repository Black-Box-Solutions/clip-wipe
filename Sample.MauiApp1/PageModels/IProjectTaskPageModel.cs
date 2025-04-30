using CommunityToolkit.Mvvm.Input;
using Sample.MauiApp1.Models;

namespace Sample.MauiApp1.PageModels
{
    public interface IProjectTaskPageModel
    {
        IAsyncRelayCommand<ProjectTask> NavigateToTaskCommand { get; }
        bool IsBusy { get; }
    }
}