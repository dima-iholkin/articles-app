namespace ArticlesApp.Core.Orchestrators.Infrastructure.EventBrokers;



public interface IEvent<TEventArgs> where TEventArgs : EventArgs
{
    //public event AsyncEventHandler<TEventArgs> Handlers;
    public event EventHandler<TEventArgs> Handlers;



    public void Publish(object sender, TEventArgs eventArgs);
}