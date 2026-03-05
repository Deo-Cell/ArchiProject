namespace Archi.Library.Search
{
    /// <summary>
    /// Représente une requête de recherche avec support des wildcards (*)
    /// </summary>
    public class SearchQuery
    {
        public string PropertyName { get; set; } = string.Empty;
        public string SearchTerm { get; set; } = string.Empty;

        // Options dérivées de la présence de '*'
        public bool StartsWithWildcard => SearchTerm.StartsWith("*");
        public bool EndsWithWildcard => SearchTerm.EndsWith("*");

        // Le terme nettoyé (sans les '*')
        public string CleanTerm => SearchTerm.Trim('*');
    }
}
