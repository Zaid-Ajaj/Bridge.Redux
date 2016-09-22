using Bridge.React;
using System;
using System.Collections.Generic;

namespace Bridge.Redux.Examples
{
    //public class TodoListContainer : Component<TodoListContainer.Props, IEnumerable<TodoItem>>
    //{
    //    public TodoListContainer(Store<IEnumerable<TodoItem>> store)
    //        : base(new Props { Store = store }) { }

    //    protected override void ComponentDidMount()
    //    {
    //        props.Store.Subscribe(() => SetState(props.Store.GetState()));
    //    }

    //    protected override IEnumerable<TodoItem> GetInitialState()
    //    {
    //        return props.Store.GetState();
    //    }

    //    public override ReactElement Render()
    //    {
    //        return Components.TodoList(new TodoListProps
    //        {
    //            Todos = state,
    //            AddTodo = text => props.Store.Dispatch(Actions.AddTodo(text)),
    //            ToggleTodo = id => props.Store.Dispatch(Actions.ToggleTodo(id))
    //        });
    //    }

    //    public class Props
    //    {
    //        public Store<IEnumerable<TodoItem>> Store;
    //    }
    //}


    public static class ReduxContainers
    {
        public static ReduxContainer<TAppState, TProps> Create<TAppState, TProps>(ContainerProps<TAppState, TProps> container)
        {
            return new ReduxContainer<TAppState, TProps>(container.Store, container.StateToPropsMapper, container.Renderer);
        }
    }



    public class ReduxContainer<TAppState, TProps> : Component<ContainerProps<TAppState, TProps>, TProps>
    {
        public ReduxContainer(Store<TAppState> store, Func<TAppState, TProps> stateMapper, Func<TProps, ReactElement> renderer) : base(new ContainerProps<TAppState, TProps>
        {
            Store = store,
            StateToPropsMapper = stateMapper,
            Renderer = renderer } )
        {

        }

        protected override void ComponentDidMount()
        {
            props.Store.Subscribe(() => SetState(props.StateToPropsMapper(props.Store.GetState())));
        }


        public override ReactElement Render()
        {
            return props.Renderer(state);
        }
    }

    public class ContainerProps<TState, TProps>
    {
        public Store<TState> Store { get; set; }
        public Func<TState, TProps> StateToPropsMapper { get; set; }
        public Func<TProps, ReactElement> Renderer { get; set; }
    }
}