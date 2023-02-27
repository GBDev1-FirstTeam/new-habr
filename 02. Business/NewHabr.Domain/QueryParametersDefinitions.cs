namespace NewHabr.Domain;

public struct QueryParametersDefinitions
{
    public struct OrderingTypes
    {
        public const string Ascending = "ascending";
        public const string Descending = "descending";
    }

    public enum RatingOrderBy
    {
        None,
        Ascending,
        Descending,
    }

    public const string PageNumber = "pageNumber";
    public const string PageSize = "pageSize";
    public const string OrderBy = "orderBy";
    public const string FromDateTime = "from";
    public const string ToDateTime = "to";
}
