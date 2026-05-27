namespace API.Helpers;

public class BlocksParams : PagingParams
{
    public string MemeberId { get; set; } = "";
    public string Predicate { get; set; } = "blocked";
}