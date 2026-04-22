namespace API.Helpers;

public class LikesParams : PagingParams
{
    public string MemeberId { get; set; } = "";
    public string Predicate { get; set; } = "liked";
}
