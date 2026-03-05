# 🍕 Archi.API — API REST en C# .NET

API REST complète pour la gestion de produits alimentaires (Pizzas, Tacos, Burgers), développée en **C# .NET 10** avec **Entity Framework Core** et **SQLite**.

## 📋 Fonctionnalités

| Fonctionnalité | Description | Exemple |
|---|---|---|
| **CRUD complet** | Create, Read, Update, Delete (soft delete) | `POST /api/v1/pizzas` |
| **Pagination** | Navigation par plages avec headers HTTP | `?range=0-25` |
| **Tri** | Tri ascendant / descendant multi-colonnes | `?asc=name&desc=price` |
| **Filtrage** | Exact, multiple, range, greater/less than | `?price=[10,20]` |
| **Recherche** | Recherche partielle avec wildcards | `?search=name:*napoli*` |
| **Réponses partielles** | Sélection des champs retournés | `?fields=id,name,price` |
| **Versioning** | URL versionnée | `/api/v1/[controller]` |
| **Logging** | Logs structurés (SeriLog) | Console + fichier |
| **Swagger** | Documentation interactive | `/swagger` |
| **Tests** | 36 tests unitaires (xUnit) | `dotnet test` |

## 🏗️ Architecture

```
src/
├── Archi.API/              # Projet API (Controllers, Models, Data)
│   ├── Controllers/
│   │   ├── PizzasController.cs
│   │   └── TacosController.cs
│   ├── Models/
│   │   ├── PizzaModel.cs
│   │   └── TacosModel.cs
│   ├── Data/
│   │   └── ArchiDbContext.cs
│   ├── Properties/
│   │   └── launchSettings.json
│   ├── Logs/
│   │   └── error-*.txt, log-*.txt
│   ├── appsettings.json
│   ├── Program.cs
│   └── Archi.API.http
├── Archi.Library/          # Bibliothèque partagée (logique métier)
│   ├── Controllers/
│   │   └── BaseController.cs     # CRUD générique
│   ├── Data/
│   │   └── BaseDbContext.cs
│   ├── Models/
│   │   └── BaseModel.cs
│   ├── Pagination/
│   │   ├── PaginationHelper.cs
│   │   └── PaginationHeaders.cs
│   ├── Sorting/
│   │   ├── SortingParser.cs
│   │   └── SortingExtensions.cs
│   ├── Filters/
│   │   ├── QueryFilterParser.cs
│   │   ├── FilterExpression.cs
│   │   ├── Filter.cs
│   │   └── FilterType.cs
│   ├── Search/
│   │   ├── SearchParser.cs
│   │   ├── SearchQuery.cs
│   │   └── SearchExtensions.cs
│   ├── PartialResponse/
│   │   ├── FieldSelector.cs
│   │   └── DynamicProjection.cs
│   ├── Logging/
│   │   └── SerilogConfig.cs
│   └── Versioning/
│       └── ApiVersionConfig.cs
├── Archi.Tests/            # Tests unitaires (xUnit)
│   ├── PaginationHelperTests.cs
│   ├── SortingParserTests.cs
│   ├── QueryFilterParserTests.cs
│   ├── SearchParserTests.cs
│   ├── FieldSelectorTests.cs
│   └── UnitTest1.cs
└── ArchiLog.sln
```

## 🚀 Prérequis

- [.NET 10 SDK](https://dotnet.microsoft.com/download)

## ⚡ Démarrage rapide

```bash
# Cloner le projet
git clone <url-du-repo>
cd src/Archi.API

# Lancer l'API
dotnet run
```

L'API démarre sur `http://localhost:5033`.
Swagger UI : `http://localhost:5033/swagger`

## 📖 Exemples d'utilisation

### CRUD

```bash
# Lister toutes les pizzas
curl http://localhost:5033/api/v1/pizzas

# Récupérer une pizza par ID
curl http://localhost:5033/api/v1/pizzas/1

# Créer une pizza
curl -X POST http://localhost:5033/api/v1/pizzas \
  -H "Content-Type: application/json" \
  -d '{"name": "Margherita", "price": 12.5}'

# Modifier une pizza
curl -X PUT http://localhost:5033/api/v1/pizzas/1 \
  -H "Content-Type: application/json" \
  -d '{"id": 1, "name": "Margherita XXL", "price": 15.0}'

# Supprimer une pizza (soft delete)
curl -X DELETE http://localhost:5033/api/v1/pizzas/1
```

### Pagination

```bash
# 10 premières pizzas
curl http://localhost:5033/api/v1/pizzas?range=0-9

# Pizzas 11 à 20
curl http://localhost:5033/api/v1/pizzas?range=10-19
```

### Tri

```bash
# Trier par nom (A → Z)
curl http://localhost:5033/api/v1/pizzas?asc=name

# Trier par prix décroissant
curl http://localhost:5033/api/v1/pizzas?desc=price

# Multi-tri
curl http://localhost:5033/api/v1/pizzas?asc=name&desc=price
```

### Filtrage

```bash
# Filtre exact
curl http://localhost:5033/api/v1/pizzas?name=Margherita

# Filtre multiple
curl "http://localhost:5033/api/v1/pizzas?name=Margherita,Royale"

# Filtre range (entre 10 et 20€)
curl "http://localhost:5033/api/v1/pizzas?price=[10,20]"

# Plus de 10€
curl "http://localhost:5033/api/v1/pizzas?price=[10,]"

# Moins de 15€
curl "http://localhost:5033/api/v1/pizzas?price=[,15]"
```

### Recherche

```bash
curl "http://localhost:5033/api/v1/pizzas?search=name:*napoli*"
```

### Réponses partielles

```bash
curl "http://localhost:5033/api/v1/pizzas?fields=id,name,price"
```

## 📜 Scripts de test

Des scripts shell et HTTP sont fournis dans `Archi.API/` pour tester chaque fonctionnalité :

```bash
# Peupler la base avec des pizzas variées
./create-varied-pizzas.sh

# Tester chaque fonctionnalité individuellement
./test-pagination.sh     # Pagination avec headers Content-Range
./test-sorting.sh        # Tri asc/desc
./test-filtering.sh      # Filtres exact, multiple, range
./test-search.sh         # Recherche partielle
./test-partial.sh        # Réponses partielles (?fields=...)
```

| Script | Fonctionnalité testée |
|---|---|
| `create-varied-pizzas.sh` | Crée un jeu de données de pizzas variées |
| `test-pagination.sh` | Pagination `?range=` + vérification des headers |
| `test-sorting.sh` | Tri `?asc=` / `?desc=` |
| `test-filtering.sh` | Filtres exact, multiple, range |
| `test-search.sh` | Recherche `?search=name:*...*` |
| `test-partial.sh` | Réponses partielles `?fields=` |
| `test-api.http` | Fichier HTTP pour tester dans VS Code / Rider |
| `Archi.API.http` | Requêtes HTTP générées par défaut |

> **Note** : Lancer `chmod +x *.sh` avant la première utilisation.

## 🧪 Tests unitaires

```bash
cd src/Archi.Tests
dotnet test --verbosity normal
```

**36 tests unitaires** couvrant :
- `PaginationHelper` — ParseRange + ApplyPagination (10 tests)
- `SortingParser` — Tri asc/desc (7 tests)
- `QueryFilterParser` — Filtres exact, multiple, range (8 tests)
- `SearchParser` — Recherche partielle (5 tests)
- `FieldSelector` — Réponses partielles (5 tests)

## 🛠️ Stack technique

| Technologie | Version | Usage |
|---|---|---|
| .NET | 10.0 | Framework |
| Entity Framework Core | 9.0.12 | ORM / Base de données |
| SQLite | — | Base de données locale |
| SeriLog | 10.0.0 | Logging structuré |
| Asp.Versioning | 8.1.1 | API Versioning |
| Swashbuckle | 7.2.0 | Swagger / OpenAPI |
| xUnit | 3.1.4 | Tests unitaires |

## 👤 Auteur

**Deo-Gratias PATINVOH** — YNOV Paris, Master Développement Web & Mobile
