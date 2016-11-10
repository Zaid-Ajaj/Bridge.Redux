using Bridge.React;
using Bridge.Redux;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bridge.ReactRedux
{
    public class ReduxComponent<TAppState, TProps> : Component<ContainerProps<TAppState, TProps>, TProps>
    {
        public ReduxComponent(ContainerProps<TAppState, TProps> containerProps) : base(containerProps)
        {
            // constructor body should be empty
        }

        protected override void ComponentDidMount()
        {
            var initialState = props.Store.GetState();

            SetState(props.StateToPropsMapper(initialState));

            props.Store.Subscribe(() => SetState(props.StateToPropsMapper(props.Store.GetState())));
        }


        public override ReactElement Render()
        {
            return state == null ? DOM.Div(new Attributes { }) : props.Renderer(state);
        }
    }
}
