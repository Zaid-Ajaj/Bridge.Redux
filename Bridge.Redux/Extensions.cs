namespace Bridge.Redux
{
    public static class Extensions
    {
        public static bool IsUndefined<T>(this T value)
        {
            return Script.Write<bool>("value === undefined");
        }

        // Works only with object literals
        public static T MergeWith<T>(this T value, T otherValue)
        {
            var result = Script.Write<T>("{ }");
            /*@
            for (var key in value) { 
                if (typeof value[key] === 'function') {
                    continue;
                }
                
                result[key] = value[key];
            
            }
           for (var key in otherValue) { 
                if (typeof otherValue[key] === 'function') {
                    continue;
                }
                
                result[key] = otherValue[key];
            
            }
             */
            return result;
        }
    }
}