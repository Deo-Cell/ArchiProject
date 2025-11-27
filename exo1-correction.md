/app
  /presentation           # Ce qui parle au monde extérieur (API / Web)
    app.py                # Point d'entrée de l'application (routes, serveur)
    html_renderer.py      # Génération de HTML ou adaptation pour la vue

  /domain                 # Cœur métier : règles, logique, modèles
    cart.py               # Logique métier du panier
    product.py            # Logique métier liée aux produits
    services.py           # Orchestration métier (cas d’usage)

  /infrastructure         # Accès aux ressources techniques
    db.py                 # Connexion à la base / repository
    logging_adapter.py    # Gestion des logs
    session_store.py      # Gestion des sessions / état persistant

  /shared                 # Outils génériques, utilitaires communs
    utils.py

  /admin                  # Fonctions d’administration
    admin_tools.py

  /legacy                 # Code ancien à retraiter / à supprimer
    old_backup_code_final_v3(1).py

  /var
    logs.txt              # Fichier de log déplacé hors du code source
