#!/bin/bash

# Script pour créer des pizzas avec des noms variés pour tester le tri
BASE_URL="http://localhost:5033"

echo "🍕 Création de pizzas avec noms variés..."
echo ""

# Créer des pizzas avec des noms différents et des prix variés
curl -s -X POST "$BASE_URL/api/v1/pizzas" \
  -H "Content-Type: application/json" \
  -d '{"name":"Margherita","base":"Tomate","composition":"Mozzarella, Basilic","price":9.50}' > /dev/null
echo "✓ Margherita créée (9.50€)"

curl -s -X POST "$BASE_URL/api/v1/pizzas" \
  -H "Content-Type: application/json" \
  -d '{"name":"4 Fromages","base":"Crème","composition":"Mozzarella, Gorgonzola, Chèvre, Parmesan","price":12.00}' > /dev/null
echo "✓ 4 Fromages créée (12.00€)"

curl -s -X POST "$BASE_URL/api/v1/pizzas" \
  -H "Content-Type: application/json" \
  -d '{"name":"Napolitaine","base":"Tomate","composition":"Anchois, Câpres, Olives","price":11.50}' > /dev/null
echo "✓ Napolitaine créée (11.50€)"

curl -s -X POST "$BASE_URL/api/v1/pizzas" \
  -H "Content-Type: application/json" \
  -d '{"name":"Calzone","base":"Tomate","composition":"Jambon, Champignons, Œuf","price":13.00}' > /dev/null
echo "✓ Calzone créée (13.00€)"

curl -s -X POST "$BASE_URL/api/v1/pizzas" \
  -H "Content-Type: application/json" \
  -d '{"name":"Végétarienne","base":"Tomate","composition":"Légumes grillés, Mozzarella","price":10.50}' > /dev/null
echo "✓ Végétarienne créée (10.50€)"

curl -s -X POST "$BASE_URL/api/v1/pizzas" \
  -H "Content-Type: application/json" \
  -d '{"name":"Reine","base":"Tomate","composition":"Jambon, Champignons, Mozzarella","price":11.00}' > /dev/null
echo "✓ Reine créée (11.00€)"

curl -s -X POST "$BASE_URL/api/v1/pizzas" \
  -H "Content-Type: application/json" \
  -d '{"name":"Hawaïenne","base":"Tomate","composition":"Jambon, Ananas, Mozzarella","price":11.50}' > /dev/null
echo "✓ Hawaïenne créée (11.50€)"

echo ""
echo "✅ Pizzas créées avec succès!"
echo ""
echo "📊 Test du tri par nom (ASC):"
echo "GET $BASE_URL/api/v1/pizzas?asc=name"
echo ""
curl -s "$BASE_URL/api/v1/pizzas?asc=name" | jq -r '.[] | "\(.name) - \(.price)€"'
echo ""

echo "📊 Test du tri par prix (DESC):"
echo "GET $BASE_URL/api/v1/pizzas?desc=price"
echo ""
curl -s "$BASE_URL/api/v1/pizzas?desc=price" | jq -r '.[] | "\(.name) - \(.price)€"'
echo ""

echo "📊 Test du tri par prix (ASC) + Pagination (0-4):"
echo "GET $BASE_URL/api/v1/pizzas?asc=price&range=0-4"
echo ""
curl -s "$BASE_URL/api/v1/pizzas?asc=price&range=0-4" | jq -r '.[] | "\(.name) - \(.price)€"'
