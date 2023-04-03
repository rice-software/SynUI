using System;
using CommunityToolkit.Mvvm.ComponentModel;
using SynUI.ViewModels;

namespace SynUI.Services;

public interface INavigationService
{
    ViewModelBase? CurrentView { get; }
    void NavigateTo<T>() where T : ViewModelBase;
}

public class NavigationService : ObservableObject, INavigationService
{
    private readonly Func<Type, ViewModelBase> viewModelFactory;
    private ViewModelBase? currentView;

    public NavigationService(Func<Type, ViewModelBase> viewModelFactory)
    {
        this.viewModelFactory = viewModelFactory;
    }

    public ViewModelBase? CurrentView
    {
        get => currentView;
        private set => SetProperty(ref currentView, value);
    }

    public void NavigateTo<TViewModel>() where TViewModel : ViewModelBase
    {
        var viewModel = viewModelFactory.Invoke(typeof(TViewModel));
        CurrentView = viewModel;
    }
}