namespace Arr.ViewModuleSystem
{
    public struct EventOpenView<T> {}
    public struct EventCloseView<T> {}
    public struct EventOnViewOpened { public View view; }
    public struct EventOnViewClosed { public View view; }
}