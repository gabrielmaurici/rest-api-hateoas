namespace rest_api_hateoas.Hateoas;

public class LinkModel
{
    public LinkModel(string href, string rel, string method)
    {
        Href = href;
        Rel = rel;
        Method = method;
    }

    public string Href { get; private set; }
    public string Rel { get; private set; }
    public string Method { get; private set; }
}