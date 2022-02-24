namespace ArticlesApp.Core.Orchestrators.Infrastructure.EventBrokers;



public delegate Task AsyncEventHandler<TEventArgs>(object? sender, TEventArgs eventArgs) where TEventArgs : EventArgs;