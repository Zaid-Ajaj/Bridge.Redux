using Bridge.React;

namespace Bridge.Redux.Examples
{
    public class Styles
    {
        public static readonly ReactStyle Done = new ReactStyle
        {
            Color = "red",
            TextDecorationLine = Html5.TextDecorationLine.LineThrough
        };

        public static readonly ReactStyle NotDone = new ReactStyle
        {
            Color = "green",
        };


    }
}