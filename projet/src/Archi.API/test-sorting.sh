#!/bin/bash

# Script pour tester le tri de l'API Archi
BASE_URL="http://localhost:5033"

echo "🚀 Test du tri - API Archi"
echo "======================================"
echo ""

# 1. Tri ascendant par nom
echo "📊 Test 1: Tri ascendant par nom"
echo "GET $BASE_URL/api/v1/pizzas?asc=name&range=0-4"
echo ""
response=$(curl -s "$BASE_URL/api/v1/pizzas?asc=name&range=0-4")
echo "$response" | jq -r '.[] | "\(.id): \(.name) - \(.price)€"'
echo ""

# 2. Tri descendant par prix
echo "📊 Test 2: Tri descendant par prix"
echo "GET $BASE_URL/api/v1/pizzas?desc=price&range=0-4"
echo ""
response=$(curl -s "$BASE_URL/api/v1/pizzas?desc=price&range=0-4")
echo "$response" | jq -r '.[] | "\(.id): \(.name) - \(.price)€"'
echo ""

# 3. Tri ascendant par prix
echo "📊 Test 3: Tri ascendant par prix"
echo "GET $BASE_URL/api/v1/pizzas?asc=price&range=0-4"
echo ""
response=$(curl -s "$BASE_URL/api/v1/pizzas?asc=price&range=0-4")
echo "$response" | jq -r '.[] | "\(.id): \(.name) - \(.price)€"'
echo ""

# 4. Tri multiple: nom ASC puis prix DESC
echo "📊 Test 4: Tri multiple (nom ASC, puis prix DESC)"
echo "GET $BASE_URL/api/v1/pizzas?asc=name&desc=price&range=0-4"
echo ""
response=$(curl -s "$BASE_URL/api/v1/pizzas?asc=name&desc=price&range=0-4")
echo "$response" | jq -r '.[] | "\(.id): \(.name) - \(.price)€"'
echo ""

# 5. Tri + Pagination
echo "📊 Test 5: Tri par prix DESC + Pagination (5-9)"
echo "GET $BASE_URL/api/v1/pizzas?desc=price&range=5-9"
echo ""
response=$(curl -s -i "$BASE_URL/api/v1/pizzas?desc=price&range=5-9")
echo "$response" | grep -E "Content-Range"
echo ""
echo "Données:"
echo "$response" | tail -1 | jq -r '.[] | "\(.id): \(.name) - \(.price)€"'
echo ""

# 6. Sans tri (ordre par défaut)
echo "📊 Test 6: Sans tri (ordre par défaut - ID)"
echo "GET $BASE_URL/api/v1/pizzas?range=0-4"
echo ""
response=$(curl -s "$BASE_URL/api/v1/pizzas?range=0-4")
echo "$response" | jq -r '.[] | "\(.id): \(.name) - \(.price)€"'
echo ""

echo "✅ Tests de tri terminés!"
echo ""
echo "💡 Observations:"
echo "   - Le tri est appliqué AVANT la pagination"
echo "   - On peut combiner plusieurs critères de tri"
echo "   - Les propriétés sont insensibles à la casse (name = Name)"
