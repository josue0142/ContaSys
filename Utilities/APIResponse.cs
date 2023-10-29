namespace ContaSys.Utilities
{
    public class APIResponse<T>
    {
        public T data { get; set; }
        public bool success { get; set; } = true;
        public IList<string> messageList { get; set; } = new List<string>();
    }
}
