namespace InversionOfControl
{
	public interface IContainer
    {
		void Register<T, U> ();
		void Register<T, U>(LifecycleType lifecycleType);
		T Resolve<T>();
	}
}
