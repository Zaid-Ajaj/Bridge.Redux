﻿using System.Collections.Generic;
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
                    var id = state.Count() + 1;
                    var todo = new TodoItem
                    {
                        Id = id,
                        IsDone = false,
                        Text = $"{action.TodoText} #{id}"
                    };

                    return state.Concat(new TodoItem[] { todo });
                }
                else if (action.Type == ActionTypes.ToggleTodo)
                {
                    return state.Select(todo =>
                    {
                        if (todo.Id == action.TodoId)
                        {
                            return new TodoItem
                            {
                                Id = todo.Id,
                                IsDone = !todo.IsDone,
                                Text = todo.Text
                            };
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