namespace InversionOfControl
{
	public interface IContainer
    {
		void Register<T, U>(LifecycleType lifecycleType);
		object Resolve<T>();
	}
}
