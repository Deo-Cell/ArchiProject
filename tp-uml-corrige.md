# ✅ CORRIGÉ – ÉTUDE DE CAS UML – MASTER 1  
## Plateforme de covoiturage – Modélisation UML

---

## 🧭 Rappel du contexte

La plateforme permet :
- la gestion des utilisateurs (conducteurs et passagers),
- la proposition et la réservation de trajets,
- le paiement via un service externe,
- la gestion des annulations,
- le dépôt d’avis après réalisation d’un trajet.

Un utilisateur peut cumuler les rôles de conducteur et de passager.

---

# 🧩 PARTIE 1 – Diagramme de cas d’utilisation (corrigé)

## 🎭 Acteurs identifiés

### Acteurs principaux
- **Utilisateur**
- **Conducteur**
- **Passager**

> Conducteur et Passager sont des spécialisations de l’acteur Utilisateur.

### Acteur secondaire
- **Service de paiement externe**

---

## 📌 Cas d’utilisation retenus

### Cas communs (Utilisateur)
- Créer un compte
- S’authentifier
- Gérer son profil
- Consulter l’historique

### Cas spécifiques Conducteur
- Proposer un trajet
- Modifier un trajet
- Annuler un trajet

### Cas spécifiques Passager
- Rechercher un trajet
- Réserver un trajet
- Annuler une réservation
- Payer une réservation
- Laisser un avis

---

## 🔗 Relations UML

- `Réserver un trajet` **inclut** `Vérifier disponibilité`
- `Payer une réservation` **inclut** `Effectuer paiement`
- `Annuler une réservation` **étend** `Réserver un trajet`
- Généralisation :
  - Conducteur ⟶ Utilisateur
  - Passager ⟶ Utilisateur

---

## ✅ Bonnes pratiques respectées

- Cas d’utilisation orientés action
- Mutualisation des comportements communs
- Frontière du système clairement définie

<img width="975" height="991" alt="usecase" src="https://github.com/user-attachments/assets/efe6e28e-8e17-47c9-b858-8982d3d7894c" />

---

# 🧩 PARTIE 2 – Diagramme de classes (corrigé)

## 📦 Classes et responsabilités

### Utilisateur
- id
- nom
- email
- motDePasse
- noteMoyenne
+ sInscrire()
+ seConnecter()
+ laisserAvis()

---

### Trajet
- id
- date
- villeDepart
- villeArrivee
- prix
- placesDisponibles
+ verifierDisponibilite()
+ annulerTrajet()

---

### Réservation
- id
- dateReservation
- statut
+ confirmer()
+ annuler()

---

### Paiement
- id
- montant
- statut
- datePaiement
+ effectuerPaiement()
+ validerPaiement()

---

### Avis
- id
- note
- commentaire
- dateAvis

---

## 🔗 Relations et multiplicités

- Utilisateur (1) —— (0..*) Trajet  
  *(un conducteur peut proposer plusieurs trajets)*

- Utilisateur (1) —— (0..*) Réservation  
  *(un passager peut faire plusieurs réservations)*

- Trajet (1) —— (0..*) Réservation  
  *(un trajet peut avoir plusieurs réservations)*

- Réservation (1) —— (1) Paiement  
  *(composition : le paiement n’existe pas sans réservation)*

- Utilisateur (1) —— (0..*) Avis  
- Trajet (1) —— (0..*) Avis  

---

## ❓ Réponses aux questions de réflexion

- **Conducteur / Passager** :  
  👉 Modélisés comme rôles (ou spécialisations d’Utilisateur), pas comme entités indépendantes.

- **Réservation sans paiement ?**  
  👉 Possible temporairement (statut *en attente*), mais non confirmée.

- **Paiement : composition ou association ?**  
  👉 Composition, car le paiement dépend strictement de la réservation.

  <img width="722" height="1099" alt="class" src="https://github.com/user-attachments/assets/555a9e02-0a9a-448d-ae98-1ea410df1a5d" />

---

# 🧩 PARTIE 3 – Diagramme de séquence (corrigé)

## 🎬 Scénario : Réserver un trajet et payer

### Participants
- Passager (acteur)
- InterfaceUtilisateur (boundary)
- ReservationController (control)
- Trajet
- Réservation
- Paiement
- ServicePaiementExterne

---

## 🔄 Séquence logique

1. Passager → Interface : rechercherTrajet()
2. Interface → Controller : rechercherTrajet()
3. Controller → Trajet : verifierDisponibilite()
4. Controller → Réservation : creerReservation()
5. Controller → Paiement : initierPaiement()
6. Paiement → ServicePaiementExterne : effectuerPaiement()
7. ServicePaiementExterne → Paiement : confirmation
8. Paiement → Controller : paiementValidé
9. Controller → Réservation : confirmer()
10. Interface → Passager : confirmationReservation

---

## 🔀 Cas alternatif

- Paiement refusé :
  - Paiement → Controller : paiementRefusé
  - Controller → Réservation : annuler()
  - Message d’erreur retourné au passager

---

## ✅ Points clés

- Séparation claire boundary / control / entity
- Respect du diagramme de classes
- Gestion d’un scénario alternatif

<img width="1289" height="1117" alt="sequence" src="https://github.com/user-attachments/assets/b409ce90-08dd-48f0-845f-7689a9054595" />

---

# 🧩 PARTIE 4 – Diagramme d’activités (corrigé)

## 🔄 Processus global de réservation

### Flux principal
- Début
- Rechercher trajet
- Sélectionner trajet
- Vérifier disponibilité
- Confirmer réservation
- Effectuer paiement
- Réservation validée
- Fin

---

### Flux alternatifs
- Paiement refusé → Annulation
- Annulation volontaire → Fin

---

## ✅ Bonnes pratiques

- Décisions explicites
- Flux alternatifs visibles
- Processus compréhensible sans texte

<img width="545" height="695" alt="activite" src="https://github.com/user-attachments/assets/51a7f24c-55ef-4366-a645-1bf43c98ba9d" />


---

# 🧩 PARTIE 5 – Diagramme d’états (corrigé)

## 🔄 Objet : Réservation

### États
- Créée
- En attente de paiement
- Confirmée
- Annulée
- Terminée

---

## 🔁 Transitions

- Créée → En attente de paiement  
  *(création réservation)*

- En attente de paiement → Confirmée  
  *(paiement validé)*

- En attente de paiement → Annulée  
  *(paiement refusé / annulation utilisateur)*

- Confirmée → Terminée  
  *(trajet effectué)*


---

## ✅ Cohérence globale

- Le cycle de vie respecte le processus métier
- Les transitions correspondent aux cas d’utilisation
- Alignement avec le diagramme de séquence

<img width="554" height="637" alt="state" src="https://github.com/user-attachments/assets/4344a3d8-7a82-493f-a8b2-9c42e8b864e4" />

---

# 🧠 SYNTHÈSE PÉDAGOGIQUE

| Diagramme | Apport |
|---------|-------|
| Cas d’utilisation | Vision fonctionnelle |
| Classes | Structure métier |
| Séquence | Interaction dynamique |
| Activités | Workflow |
| États | Cycle de vie |

---

## ❌ Erreurs fréquentes à éviter

- Mélanger diagramme de classes et diagramme technique
- Oublier les multiplicités
- Créer des cas d’utilisation trop techniques
- Mettre de la logique métier dans les acteurs

---

