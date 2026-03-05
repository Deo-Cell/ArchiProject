using Microsoft.AspNetCore.Http;

namespace Archi.Library.PartialResponse
{
    /// <summary>
    /// Parser pour extraire les champs demandés de l'URL
    /// Exemple: ?fields=id,name,price
    /// </summary>
    public static class FieldSelector
    {
        public static List<string> ParseFields(IQueryCollection queryParams)
        {
            var fields = new List<string>();

            if (queryParams.TryGetValue("fields", out var fieldsValues))
            {
                foreach (var value in fieldsValues)
                {
                    if (string.IsNullOrWhiteSpace(value)) continue;

                    var parts = value.Split(',');
                    foreach (var part in parts)
                    {
                        var trimmed = part.Trim();
                        if (!string.IsNullOrEmpty(trimmed))
                        {
                            fields.Add(trimmed);
                        }
                    }
                }
            }

            return fields;
        }
    }
}
