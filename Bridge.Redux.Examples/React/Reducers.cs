using System.Collections.Generic;
using System.Linq;

namespace Bridge.Redux.Examples
{
    public class Reducers
    {
        public static Reducer<IEnumerable<TodoItem>> ItemsReducer()
        {
            return Reducer<IEnumerable<TodoItem>>.Create<SimpleAction>((state, action) =>
            {
                if (action.Type == ActionTypes.AddTodo)
                {
                    var todo = new TodoItem
                    {
                        Id = state.Count() + 1,
                        IsDone = false,
                        Text = action.TodoText
                    };

                    return state.Concat(new TodoItem[] { todo  });
                }
                else if (action.Type == ActionTypes.ToggleTodo)
                {
                    return state.Select(todo =>
                    {
                        if (todo.Id == action.TodoId)
                        {
                            todo.IsDone = !todo.IsDone;
                            return todo;
                        }
                        else
                        {
                            return todo;
                        }
                    });
                } 
                else
                {
                    return state;
                }
            });
        }
    }
}