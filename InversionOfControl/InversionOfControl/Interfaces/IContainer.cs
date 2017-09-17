namespace InversionOfControl
{
	public interface IContainer
    {
		void Clear();
		void Register<T, U>(LifecycleType lifecycleType);
		object Resolve<T>();
	}
}
