#!/bin/bash

BASE_URL="http://localhost:5033/api"

echo "✂️ Test des Réponses Partielles API Archi"
echo "====================================="
echo ""

# Test 1: Sélection d'un seul champ
echo "1. Un seul champ: fields=name"
echo "GET $BASE_URL/pizzas?fields=name"
curl -s "$BASE_URL/pizzas?fields=name" | jq .
echo ""

# Test 2: Sélection de plusieurs champs
echo "2. Plusieurs champs: fields=id,name,price"
echo "GET $BASE_URL/pizzas?fields=id,name,price"
curl -s "$BASE_URL/pizzas?fields=id,name,price" | jq .
echo ""

# Test 3: Champs insensibles à la casse
echo "3. Casse ignorée: fields=ID,NaMe"
echo "GET $BASE_URL/pizzas?fields=ID,NaMe"
curl -s "$BASE_URL/pizzas?fields=ID,NaMe" | jq .
echo ""

# Test 4: Champ inexistant (doit être ignoré)
echo "4. Champ inexistant: fields=name,fakeField"
echo "GET $BASE_URL/pizzas?fields=name,fakeField"
curl -s "$BASE_URL/pizzas?fields=name,fakeField" | jq .
echo ""

# Test 5: Combinaison Totale (Tri + Recherche + Pagination + Champs)
echo "5. LA TOTALE: search=name:*a* & asc=price & range=0-1 & fields=id,name"
echo "GET $BASE_URL/pizzas?search=name:*a*&asc=price&range=0-1&fields=id,name"
curl -g -s "$BASE_URL/pizzas?search=name:*a*&asc=price&range=0-1&fields=id,name" | jq .
echo ""

echo "✅ Tests de réponses partielles terminés !"
