public class BaseClass {
    public BaseClass () {
        UpdateManager.Instance.Add (this);
    }~BaseClass () {
        UpdateManager.Instance.Remove (this);
    }
}