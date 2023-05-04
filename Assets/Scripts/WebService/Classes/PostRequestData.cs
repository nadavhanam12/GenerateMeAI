public class PostRequestData
{
    public string msg { get; set; }
    public PostRequestData(string prompt)
    {
        this.msg = prompt;
    }
}