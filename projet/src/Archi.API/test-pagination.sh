#!/bin/bash

# Script pour tester la pagination de l'API Archi
BASE_URL="http://localhost:5033"

echo "🚀 Test de la pagination - API Archi"
echo "======================================"
echo ""

# 1. Créer des pizzas de test
echo "📝 Étape 1: Création de 15 pizzas de test..."
for i in {1..15}; do
  curl -s -X POST "$BASE_URL/api/pizzas" \
    -H "Content-Type: application/json" \
    -d "{\"name\":\"Pizza $i\",\"base\":\"Tomate\",\"composition\":\"Fromage\",\"price\":$((10 + i))}" > /dev/null
  echo "  ✓ Pizza $i créée"
done

echo ""
echo "✅ 15 pizzas créées avec succès!"
echo ""

# 2. Tester la pagination par défaut
echo "📊 Étape 2: Test pagination par défaut (sans range)"
echo "GET $BASE_URL/api/pizzas"
echo ""
response=$(curl -s -i "$BASE_URL/api/pizzas")
echo "$response" | grep -E "Content-Range|Accept-Range|Link"
echo ""

# 3. Tester première page (0-4)
echo "📊 Étape 3: Test première page (0-4)"
echo "GET $BASE_URL/api/pizzas?range=0-4"
echo ""
response=$(curl -s -i "$BASE_URL/api/pizzas?range=0-4")
echo "$response" | grep -E "Content-Range|Accept-Range|Link"
count=$(echo "$response" | grep -o '"id"' | wc -l | tr -d ' ')
echo "Nombre d'éléments retournés: $count"
echo ""

# 4. Tester deuxième page (5-9)
echo "📊 Étape 4: Test deuxième page (5-9)"
echo "GET $BASE_URL/api/pizzas?range=5-9"
echo ""
response=$(curl -s -i "$BASE_URL/api/pizzas?range=5-9")
echo "$response" | grep -E "Content-Range|Accept-Range|Link"
count=$(echo "$response" | grep -o '"id"' | wc -l | tr -d ' ')
echo "Nombre d'éléments retournés: $count"
echo ""

# 5. Tester dernière page (10-14)
echo "📊 Étape 5: Test dernière page (10-14)"
echo "GET $BASE_URL/api/pizzas?range=10-14"
echo ""
response=$(curl -s -i "$BASE_URL/api/pizzas?range=10-14")
echo "$response" | grep -E "Content-Range|Accept-Range|Link"
count=$(echo "$response" | grep -o '"id"' | wc -l | tr -d ' ')
echo "Nombre d'éléments retournés: $count"
echo ""

# 6. Tester range au-delà des données (15-20)
echo "📊 Étape 6: Test range au-delà des données (15-20)"
echo "GET $BASE_URL/api/pizzas?range=15-20"
echo ""
response=$(curl -s -i "$BASE_URL/api/pizzas?range=15-20")
echo "$response" | grep -E "Content-Range|Accept-Range|Link"
count=$(echo "$response" | grep -o '"id"' | wc -l | tr -d ' ')
echo "Nombre d'éléments retournés: $count"
echo ""

echo "✅ Tests de pagination terminés!"
echo ""
echo "💡 Tu peux aussi tester manuellement avec:"
echo "   - GET $BASE_URL/api/pizzas?range=0-9"
echo "   - GET $BASE_URL/api/pizzas?range=10-19"
echo "   - Regarde les headers Content-Range, Accept-Range, et Link"
