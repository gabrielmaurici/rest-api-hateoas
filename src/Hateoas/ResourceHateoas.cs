namespace rest_api_hateoas.Hateoas;

public class ResourceHateoas<TModel> where TModel : class
{
    public object GenerateLinksHateoas(TModel? model, LinkModel[] links, string propertyName) 
    {
        return new Dictionary<string, object?>
        {
            [propertyName] = model,
            ["_links"] = links
        };
    }
}