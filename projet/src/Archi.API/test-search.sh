#!/bin/bash

BASE_URL="http://localhost:5033/api"

echo "🔍 Test de Recherche API Archi"
echo "=============================="
echo ""

# Test 1: Recherche Contains (mot à l'intérieur)
echo "1. Recherche Contains (*ari* dans nom)"
echo "GET $BASE_URL/pizzas?search=name:*ari*"
curl -g -s "$BASE_URL/pizzas?search=name:*ari*" | jq -r '.[] | "- \(.name) (\(.price)€)"'
echo ""

# Test 2: Recherche StartsWith (commence par)
echo "2. Recherche StartsWith (M* dans nom)"
echo "GET $BASE_URL/pizzas?search=name:M*"
curl -g -s "$BASE_URL/pizzas?search=name:M*" | jq -r '.[] | "- \(.name) (\(.price)€)"'
echo ""

# Test 3: Recherche EndsWith (finit par)
echo "3. Recherche EndsWith (*ne dans nom)"
echo "GET $BASE_URL/pizzas?search=name:*ne"
curl -g -s "$BASE_URL/pizzas?search=name:*ne" | jq -r '.[] | "- \(.name) (\(.price)€)"'
echo ""

# Test 4: Recherche exacte (sans wildcard)
echo "4. Recherche Exacte (Calzone dans nom)"
echo "GET $BASE_URL/pizzas?search=name:Calzone"
curl -g -s "$BASE_URL/pizzas?search=name:Calzone" | jq -r '.[] | "- \(.name) (\(.price)€)"'
echo ""

# Test 5: Combinaison Recherche + Filtre + Tri dynamique
echo "5. Combinaison: search=name:*a* & price=[,11.5] & asc=price"
echo "GET $BASE_URL/pizzas?search=name:*a*&price=[,11.5]&asc=price"
curl -g -s "$BASE_URL/pizzas?search=name:*a*&price=[,11.5]&asc=price" | jq -r '.[] | "- \(.name) (\(.price)€)"'
echo ""

echo "✅ Tests de recherche terminés !"
