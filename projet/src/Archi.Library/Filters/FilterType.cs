namespace Archi.Library.Filters
{
    /// <summary>
    /// Type de filtre à appliquer
    /// </summary>
    public enum FilterType
    {
        Exact,        // ex: type=pizza (égalité simple)
        Multiple,     // ex: type=pizza,pates (sélection parmi plusieurs valeurs)
        Range,        // ex: price=[10,20] (entre deux valeurs incluses)
        GreaterThan,  // ex: price=[10,] (supérieur ou égal à)
        LessThan      // ex: price=[,20] (inférieur ou égal à)
    }
}
