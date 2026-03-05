namespace Archi.Library.Filters
{
    /// <summary>
    /// Représente un filtre parsé depuis l'URL
    /// </summary>
    public class Filter
    {
        /// <summary>
        /// Nom de la propriété sur laquelle appliquer le filtre
        /// </summary>
        public string PropertyName { get; set; } = string.Empty;

        /// <summary>
        /// Type d'opération de filtrage
        /// </summary>
        public FilterType Type { get; set; }

        /// <summary>
        /// Valeurs associées au filtre
        /// </summary>
        public List<string> Values { get; set; } = new();
    }
}
