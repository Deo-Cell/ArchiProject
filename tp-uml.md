# 📘 ÉTUDE DE CAS UML – MASTER 1  
## Plateforme de covoiturage – Modélisation UML

---

## 🎯 Objectifs pédagogiques

À l’issue de cette étude de cas, l’étudiant devra être capable de :

- Analyser un besoin fonctionnel complexe
- Identifier les acteurs et fonctionnalités d’un système
- Modéliser un système à l’aide des principaux diagrammes UML
- Assurer la cohérence entre les diagrammes structurels et comportementaux
- Justifier ses choix de modélisation

---

## 🧭 Contexte général

Une startup souhaite développer une **plateforme de covoiturage** accessible via **application web et mobile**.

La plateforme permet à des utilisateurs de :
- proposer des trajets,
- rechercher et réserver des trajets,
- effectuer des paiements en ligne,
- annuler des réservations,
- laisser des avis après un trajet.

Les paiements sont gérés par un **service externe**.

Un utilisateur peut être **conducteur**, **passager**, ou les deux.

---

## 📌 Contraintes générales

- Le système doit vérifier la disponibilité des places avant toute réservation.
- Un paiement valide est obligatoire pour confirmer une réservation.
- Les annulations peuvent entraîner des pénalités.
- Les avis ne sont possibles qu’après un trajet terminé.

---

# 🧩 PARTIE 1 – Diagramme de cas d’utilisation

## 📄 Description fonctionnelle

### Fonctions principales
- Création de compte
- Authentification
- Gestion du profil
- Proposition de trajet
- Recherche de trajet
- Réservation de place
- Paiement en ligne
- Annulation de réservation
- Consultation de l’historique
- Dépôt d’avis

### Acteurs possibles
- Utilisateur
- Conducteur
- Passager
- Service de paiement (acteur secondaire)

---

## 🔧 Travail demandé

1. Identifier les acteurs du système  
2. Délimiter le périmètre du système  
3. Réaliser un **diagramme de cas d’utilisation UML** :
   - Inclure les relations `<<include>>` et `<<extend>>`
   - Utiliser la généralisation d’acteurs si pertinente
   - Nommer les cas d’utilisation avec des verbes d’action

---

## ✅ Attendus

- Diagramme lisible et structuré
- Cas d’utilisation non redondants
- Bonne séparation système / acteurs

---

# 🧩 PARTIE 2 – Diagramme de classes

## 📄 Contraintes métier

- Un **trajet** possède :
  - une date
  - une ville de départ
  - une ville d’arrivée
  - un nombre de places disponibles
  - un prix par place
- Une **réservation** :
  - concerne un seul passager
  - est associée à un seul trajet
- Un **paiement** :
  - est lié à une réservation
  - possède un statut (`en attente`, `validé`, `refusé`)
- Un **avis** :
  - est rédigé par un utilisateur
  - concerne un trajet
- Un **utilisateur** peut être conducteur et/ou passager

---

## 🔧 Travail demandé

Réaliser un **diagramme de classes UML** comprenant au minimum :

### Classes obligatoires
- Utilisateur
- Trajet
- Réservation
- Paiement
- Avis

### Éléments attendus
- Attributs pertinents
- Méthodes principales (sans implémentation)
- Relations UML appropriées
- Multiplicités
- Visibilité des attributs et méthodes

---

## ❓ Questions de réflexion

- Conducteur et Passager doivent-ils être des classes ou des rôles ?
- Une réservation peut-elle exister sans paiement ?
- Paiement est-il une composition ou une association ?

---

## ✅ Attendus

- Modèle fidèle au métier
- Relations cohérentes
- Diagramme exploitable pour une implémentation future

---

# 🧩 PARTIE 3 – Diagramme de séquence

## 📄 Scénario imposé

**Un passager réserve un trajet et effectue un paiement en ligne**

### Étapes
1. Recherche d’un trajet  
2. Vérification des places disponibles  
3. Création de la réservation  
4. Déclenchement du paiement  
5. Validation du paiement par un service externe  
6. Confirmation de la réservation  

---

## 🔧 Travail demandé

Réaliser un **diagramme de séquence UML** incluant :

- Acteur : Passager
- Objets `boundary` (interface utilisateur)
- Objets `control` (logique applicative)
- Objets `entity` (données métier)
- Service de paiement externe

Inclure :
- Messages synchrones
- Retours
- Gestion d’un cas d’échec du paiement

---

## ✅ Attendus

- Séquence logique et complète
- Responsabilités bien réparties
- Diagramme aligné avec le diagramme de classes

---

# 🧩 PARTIE 4 – Diagramme d’activités

## 📄 Processus à modéliser

**Processus global de réservation d’un trajet**

Inclure :
- Recherche
- Sélection du trajet
- Confirmation
- Paiement
- Validation ou annulation

---

## 🔧 Travail demandé

Réaliser un **diagramme d’activités UML** comprenant :

- Actions
- Nœuds de décision
- Flux alternatifs (annulation, paiement refusé)
- Début et fin du processus

---

## ✅ Attendus

- Workflow clair
- Cas alternatifs visibles
- Diagramme compréhensible sans explication orale

---

# 🧩 PARTIE 5 – Diagramme d’états

## 📄 Objet étudié

**Réservation**

### États possibles
- Créée
- En attente de paiement
- Confirmée
- Annulée
- Terminée

---

## 🔧 Travail demandé

Réaliser un **diagramme d’états UML** montrant :

- États
- Transitions
- Événements déclencheurs

---

## ✅ Attendus

- Cycle de vie complet
- Transitions pertinentes
- Diagramme cohérent avec les autres modèles

---

# 📦 Livrables attendus

- Un document PDF contenant l’ensemble des diagrammes UML
- Nommage clair et homogène
- Outils recommandés : Draw.io, StarUML, Modelio, Lucidchart

---

# 📊 Critères d’évaluation (indicatif)

- Pertinence fonctionnelle
- Qualité de modélisation UML
- Cohérence entre les diagrammes
- Clarté et lisibilité
- Respect des conventions UML
