#!/bin/bash

BASE_URL="http://localhost:5033/api"

echo "🧪 Test des Filtres API Archi"
echo "=============================="
echo ""

# Test 1: Filtre Exact sur chaîne
echo "1. Filtre Exact: name=Margherita"
echo "GET $BASE_URL/pizzas?name=Margherita"
curl -g -s "$BASE_URL/pizzas?name=Margherita" | jq -r '.[] | "- \(.name) (\(.price)€)"'
echo ""

# Test 2: Filtre Multiple sur chaîne
echo "2. Filtre Multiple: name=Margherita,Reine"
echo "GET $BASE_URL/pizzas?name=Margherita,Reine"
curl -g -s "$BASE_URL/pizzas?name=Margherita,Reine" | jq -r '.[] | "- \(.name) (\(.price)€)"'
echo ""

# Test 3: Filtre Range sur prix
echo "3. Filtre Range: price=[10.5,12]"
echo "GET $BASE_URL/pizzas?price=[10.5,12]"
curl -g -s "$BASE_URL/pizzas?price=[10.5,12]" | jq -r '.[] | "- \(.name) (\(.price)€)"'
echo ""

# Test 4: Filtre GreaterThan sur prix
echo "4. Filtre GreaterThan: price=[13,]"
echo "GET $BASE_URL/pizzas?price=[13,]"
curl -g -s "$BASE_URL/pizzas?price=[13,]" | jq -r '.[] | "- \(.name) (\(.price)€)"'
echo ""

# Test 5: Filtre LessThan sur prix
echo "5. Filtre LessThan: price=[,10]"
echo "GET $BASE_URL/pizzas?price=[,10]"
curl -g -s "$BASE_URL/pizzas?price=[,10]" | jq -r '.[] | "- \(.name) (\(.price)€)"'
echo ""

# Test 6: Combinaison Filtre + Tri dynamique + Pagination
echo "6. Combinaison: price=[,12] & asc=price & range=0-2"
echo "GET $BASE_URL/pizzas?price=[,12]&asc=price&range=0-2"
curl -g -s "$BASE_URL/pizzas?price=[,12]&asc=price&range=0-2" | jq -r '.[] | "- \(.name) (\(.price)€)"'
echo ""

echo "✅ Tests terminés !"
