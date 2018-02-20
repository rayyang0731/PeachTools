public interface INoticeObj {
	int Number { get; }
	object Body { get; }

	bool TryGetBody<T> (out T body) where T : class;

	T GetBody<T> ();
}