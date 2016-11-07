using System;
using Bridge.React;
using Bridge.Redux;

namespace Bridge.ReactRedux
{
    [ObjectLiteral]
    public class ContainerProps<TState, TStatePart>
    {
        public Store<TState> Store { get; set; }
        public Func<TState, TStatePart> StateToPropsMapper { get; set; }
        public Func<TStatePart, ReactElement> Renderer { get; set; }
    }
}
