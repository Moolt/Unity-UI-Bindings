using System;

namespace UiBinding.Core
{
    public class DisposableAction : IDisposable
    {
        private readonly Action _action;

        public DisposableAction(Action action)
        {
            _action = action;
        }

        public void Dispose()
        {
            _action?.Invoke();
        }

        public static IDisposable Empty => new DisposableAction(() => { });
    }
}