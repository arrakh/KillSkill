namespace Arr
{
    public static class StringExtensions
    {
        public static string LogColor(this string text, string color) => $"<color={color}>{text}</color>";
        
        public static string Tag(this string text, string tag, string param = "") 
            => $"<{tag}{(param == "" ? "" : "=" + param)}>{text}</{tag}>";
    }
}