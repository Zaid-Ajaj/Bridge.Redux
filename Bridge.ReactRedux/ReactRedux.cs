using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bridge.ReactRedux
{
    public static class ReactRedux
    {
        public static ReduxComponent<TAppState, TProps> Component<TAppState, TProps>(ContainerProps<TAppState, TProps> container)
        {
            return new ReduxComponent<TAppState, TProps>(container);
        }
    }
}