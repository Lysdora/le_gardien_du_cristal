# 🎮 ARTEFACT DEFENSE - Guide Complet de Développement

## 📋 Table des Matières

1. [Vue d'ensemble du projet](#vue-densemble-du-projet)
2. [Étape 1 : Configuration de base](#étape-1--configuration-de-base)
3. [Étape 2 : Système de gemmes](#étape-2--système-de-gemmes)
4. [Étape 3 : Système de l'artefact](#étape-3--système-de-lartefact)
5. [Étape 4 : Système de combat](#étape-4--système-de-combat)
6. [Étape 5 : Spawn des monstres](#étape-5--spawn-des-monstres)
7. [Étape 6 : Game Manager et UI](#étape-6--game-manager-et-ui)
8. [Concepts Unity importants](#concepts-unity-importants)

---

## 🎯 Vue d'ensemble du projet

**Artefact Defense** est un jeu 2D où :

- Le joueur doit protéger un artefact magique
- L'artefact perd de la vie chaque seconde
- Le joueur ramasse des gemmes pour nourrir l'artefact
- Des monstres apparaissent et attaquent l'artefact
- Le joueur clique sur les monstres pour les éliminer
- Objectif : Survivre le plus longtemps possible !

**Technologies :**

- Unity 2022.3.62f3
- C# (langage de programmation)
- Universal Render Pipeline (URP)
- 2D Physics

---

## ✅ Étape 1 : Configuration de base

### 🎯 Objectif

Mettre en place les bases : joueur, caméra et mouvement.

### 📝 Scripts créés

- `PlayerController.cs`
- `CameraController.cs`
- `MonsterController.cs`

---

### 1.1 PlayerController.cs - Mouvement du joueur

**Concept clé :** Le joueur utilise un `Rigidbody2D` pour un mouvement physique réaliste.

```csharp
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 6f;
    private Vector2 moveInput;
    private Rigidbody2D rb;

    private void Awake()
    {
        // Récupère le composant Rigidbody2D attaché au GameObject
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Lit les inputs (WASD ou flèches)
        moveInput.x = Input.GetAxisRaw("Horizontal"); // -1, 0, ou 1
        moveInput.y = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        // Normalise pour éviter le speed boost en diagonal
        // sqrMagnitude > 1 signifie qu'on va en diagonal
        rb.velocity = moveInput.sqrMagnitude > 1f
            ? moveInput.normalized * moveSpeed
            : moveInput * moveSpeed;
    }
}
```

**🎓 Explications :**

- `Awake()` : S'exécute au démarrage, avant `Start()`
- `Update()` : S'exécute chaque frame (~60 fois/seconde) - parfait pour lire les inputs
- `FixedUpdate()` : S'exécute à intervalle fixe - parfait pour la physique
- `Input.GetAxisRaw()` : Retourne -1, 0, ou 1 (pas de smooth)
- `moveInput.normalized` : Réduit le vecteur à une longueur de 1 (évite d'aller plus vite en diagonal)

**⚙️ Configuration Unity :**

1. Attache ce script au GameObject `Player`
2. Ajoute un composant `Rigidbody2D` :
   - Gravity Scale : 0 (pas de gravité pour un jeu vu de haut)
   - Freeze Rotation Z : ✅ (évite que le player tourne)
3. Tag : "Player" (important pour les collisions !)

---

### 1.2 CameraController.cs - Caméra qui suit le joueur

**Concept clé :** La caméra suit le joueur avec des limites (pour ne pas sortir de la map).

```csharp
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform playerTransform;

    [Header("Limites de la caméra")]
    [SerializeField] private float minX = -7f;
    [SerializeField] private float maxX = 40f;
    [SerializeField] private float minY = -5f;
    [SerializeField] private float maxY = 5f;

    private Vector3 tempPos;

    void Start()
    {
        // Trouve le player via son tag
        playerTransform = GameObject.FindWithTag("Player").transform;

        if (playerTransform == null)
            Debug.Log("Ajoute le tag 'Player' à ton player !");
    }

    private void LateUpdate()
    {
        if (playerTransform == null) return;

        // Suit le player en X et Y, garde le Z de la caméra
        tempPos = new Vector3(
            playerTransform.position.x,
            playerTransform.position.y,
            transform.position.z
        );

        // Limite la position pour rester dans la map
        tempPos.x = Mathf.Clamp(tempPos.x, minX, maxX);
        tempPos.y = Mathf.Clamp(tempPos.y, minY, maxY);

        transform.position = tempPos;
    }
}
```

**🎓 Explications :**

- `[SerializeField]` : Rend une variable privée visible dans l'Inspector
- `[Header("...")]` : Ajoute un titre dans l'Inspector (organisation)
- `LateUpdate()` : S'exécute après tous les `Update()` - évite le jitter (saccades)
- `Mathf.Clamp(value, min, max)` : Force une valeur entre min et max
- `GameObject.FindWithTag()` : Trouve un objet par son tag (pratique mais lent, à faire dans `Start()`)

**⚙️ Configuration Unity :**

1. Attache ce script à la `Main Camera`
2. Ajuste les limites dans l'Inspector selon la taille de ta map

---

### 1.3 MonsterController.cs - Mouvement simple du monstre

**Concept clé :** Les monstres marchent vers la gauche (vers l'artefact).

```csharp
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    float speed = 2f;

    private void FixedUpdate()
    {
        // Déplace le monstre vers la gauche
        transform.position += new Vector3(-speed * Time.deltaTime, 0, 0);
    }
}
```

**🎓 Explications :**

- `transform.position` : Position du GameObject (manipulation directe, pas de physique)
- `Time.deltaTime` : Temps écoulé depuis la dernière frame (~0.016s à 60 FPS)
- Multiplier par `Time.deltaTime` rend le mouvement indépendant du framerate
- **Pas de Rigidbody2D** : Le monstre ne réagit pas à la physique (voulu)

**⚙️ Configuration Unity :**

1. Attache ce script au prefab `Monster`

---

## 💎 Étape 2 : Système de gemmes

### 🎯 Objectif

Le joueur peut ramasser des gemmes et voir le compteur à l'écran.

### 📝 Scripts créés

- `GemmeController.cs`
- `PlayerInventory.cs`

---

### 2.1 GemmeController.cs - Détection et collecte

**Concept clé :** Utilisation des **Triggers** pour détecter les collisions sans physique.

```csharp
using UnityEngine;

public class GemmeController : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Vérifie si c'est le player qui touche
        if (collision.gameObject.CompareTag("Player"))
        {
            // Récupère le script PlayerInventory du player
            PlayerInventory playerInventory = collision.gameObject.GetComponent<PlayerInventory>();

            // Sécurité : vérifie que le script existe
            if (playerInventory != null)
            {
                playerInventory.AddGemme();
            }

            Debug.Log("Gemme collectée !");
            Destroy(gameObject); // Détruit la gemme
        }
    }
}
```

**🎓 Explications :**

- `OnTriggerEnter2D()` : Fonction automatique appelée quand un objet entre dans le trigger
- `collision.gameObject` : L'objet qui a touché la gemme
- `CompareTag()` : Compare le tag (plus rapide que `==`)
- `GetComponent<T>()` : Récupère un script attaché à un GameObject
- `Destroy(gameObject)` : Détruit l'objet (ici, la gemme elle-même)

**⚙️ Configuration Unity :**

1. Sur le prefab `Gemme_rouge` :
   - Ajoute `Circle Collider 2D`
   - **Coche "Is Trigger"** ✅ (important !)
   - Ajuste le radius pour la zone de collecte
2. Attache le script `GemmeController`

**💡 Trigger vs Collision :**

- **Trigger** : Détecte le contact mais ne bloque pas (passe à travers)
- **Collision** : Détecte ET bloque physiquement
- Pour ramasser des objets → Utilise Trigger !

---

### 2.2 PlayerInventory.cs - Compteur et UI

**Concept clé :** Communication avec l'UI (TextMeshPro).

```csharp
using UnityEngine;
using TMPro; // Import pour TextMeshPro

public class PlayerInventory : MonoBehaviour
{
    public TextMeshProUGUI gemmeCountText; // Référence à l'UI
    int gemmeCount = 0;

    // Fonction publique pour récupérer le nombre de gemmes
    public int GetGemmeCount()
    {
        return gemmeCount;
    }

    // Fonction publique pour retirer des gemmes
    public void RemoveGemme(int amount)
    {
        gemmeCount -= amount;
        if (gemmeCount < 0)
            gemmeCount = 0; // Sécurité

        Debug.Log("Gemme retirée ! Total : " + gemmeCount);
        gemmeCountText.text = gemmeCount.ToString();
    }

    // Fonction publique appelée par GemmeController
    public void AddGemme()
    {
        gemmeCount++;
        Debug.Log("Gemme ajoutée ! Total : " + gemmeCount);
        gemmeCountText.text = gemmeCount.ToString();
    }
}
```

**🎓 Explications :**

- `using TMPro` : Namespace pour utiliser TextMeshPro
- `public` : Les autres scripts peuvent appeler ces fonctions
- `private` (par défaut) : Seulement ce script peut y accéder
- `.ToString()` : Convertit un nombre en texte
- Encapsulation : `gemmeCount` est privé, on accède via `GetGemmeCount()`

**⚙️ Configuration Unity :**

**Créer l'UI :**

1. Hierarchy → Clic droit → UI → Canvas (si pas déjà créé)
2. Clic droit sur Canvas → UI → Image (pour l'icône de gemme)
3. Clic droit sur Canvas → UI → Text - TextMeshPro
   - Renomme en "GemmeCountText"
   - Position : Top-Left
   - Texte : "0"

**Connecter au script :**

1. Sélectionne le `Player`
2. Attache le script `PlayerInventory`
3. Glisse `GemmeCountText` (Hierarchy) vers le champ `Gemme Count Text` (Inspector)

---

## 🏺 Étape 3 : Système de l'artefact

### 🎯 Objectif

L'artefact perd de la vie automatiquement et peut être nourri avec des gemmes.

### 📝 Scripts créés

- `ArtefactHealth.cs`
- `ArtefactFeeder.cs`

---

### 3.1 ArtefactHealth.cs - Vie et barre de vie

**Concept clé :** Utilisation d'`InvokeRepeating()` pour des actions répétées.

```csharp
using UnityEngine;
using UnityEngine.UI; // Pour le Slider

public class ArtefactHealth : MonoBehaviour
{
    public Slider healthBar; // Référence à la barre de vie UI

    float health = 100f;
    float decreaseRate = 1f; // Perte de vie par seconde
    float max_health = 100f;

    void Start()
    {
        health = max_health;
        healthBar.value = health; // Initialise la barre

        // Appelle DecreaseHealth toutes les 1 seconde
        InvokeRepeating("DecreaseHealth", 1f, 1f);
    }

    void DecreaseHealth()
    {
        health -= decreaseRate;
        healthBar.value = health;
        Debug.Log("Vie de l'artefact : " + health);

        if (health <= 0)
        {
            Debug.Log("GAME OVER");
            Destroy(gameObject);
        }
    }

    // Fonction publique pour restaurer la vie
    public void RestoreHealth(float amount)
    {
        health += amount;

        // Ne pas dépasser le maximum
        if (health > max_health)
            health = max_health;

        healthBar.value = health; // Met à jour la barre
        Debug.Log("Artefact nourri ! Vie : " + health);
    }
}
```

**🎓 Explications :**

- `InvokeRepeating(nomFonction, délai, intervalle)` :
  - `nomFonction` : Nom de la fonction (en string)
  - `délai` : Temps avant le premier appel
  - `intervalle` : Temps entre chaque appel
- `Slider` : Composant UI pour les barres (vie, mana, etc.)
- `healthBar.value` : Valeur du slider (entre min et max)

**⚙️ Configuration Unity :**

**Créer la barre de vie :**

1. Canvas → UI → Slider
   - Renomme en "ArtefactHealthBar"
   - Position : Bottom-Center
   - Min Value : 0, Max Value : 100, Value : 100
2. Supprime "Handle Slide Area" (pas besoin)
3. Ajuste les couleurs :
   - Background → Image → Color : Gris (80, 80, 80)
   - Fill → Image → Color : Vert (50, 205, 50)
4. Ajuste Fill Area et Fill → Rect Transform → Tout à 0

**Connecter au script :**

1. Sélectionne l'`Artefact`
2. Attache `ArtefactHealth.cs`
3. Glisse `ArtefactHealthBar` vers le champ `Health Bar`

---

### 3.2 ArtefactFeeder.cs - Nourrir l'artefact

**Concept clé :** Détection de zone (OnTrigger) + Input clavier.

```csharp
using UnityEngine;

public class ArtefactFeeder : MonoBehaviour
{
    private bool playerInRange = false;
    private PlayerInventory playerInventory;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
            playerInventory = collision.GetComponent<PlayerInventory>();
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
            playerInventory = null;
        }
    }

    void Update()
    {
        // Si le player est dans la zone ET appuie sur E
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            // Vérifie s'il a au moins 1 gemme
            if (playerInventory.GetGemmeCount() >= 1)
            {
                playerInventory.RemoveGemme(1);
                GetComponent<ArtefactHealth>().RestoreHealth(10);
                Debug.Log("Artefact nourri ! +10 PV");
            }
            else
            {
                Debug.Log("Pas assez de gemmes !");
            }
        }
    }
}
```

**🎓 Explications :**

- `OnTriggerEnter2D()` : Le player ENTRE dans la zone
- `OnTriggerExit2D()` : Le player SORT de la zone
- `Input.GetKeyDown(KeyCode.E)` : Détecte UNE pression (pas maintenue)
- Variables de classe : `playerInRange` et `playerInventory` conservent leur valeur entre les frames
- Communication entre scripts : `GetComponent<ArtefactHealth>().RestoreHealth(10)`

**⚙️ Configuration Unity :**

1. Sur l'`Artefact` :
   - Ajoute `Circle Collider 2D`
   - **Coche "Is Trigger"** ✅
   - Radius : 2-3 (zone d'interaction)
2. Attache `ArtefactFeeder.cs` sur l'Artefact

**💡 Pattern important :**

```
OnTriggerEnter → Active un flag
Update → Vérifie le flag + input
OnTriggerExit → Désactive le flag
```

C'est le pattern standard pour les zones interactives !

---

## ⚔️ Étape 4 : Système de combat

### 🎯 Objectif

- Les monstres ont de la vie
- Le player clique sur les monstres pour les attaquer
- Les monstres infligent des dégâts à l'artefact

### 📝 Scripts à créer

- `MonsterHealth.cs`
- `PlayerAttack.cs`
- `MonsterAttack.cs`

---

### 4.1 MonsterHealth.cs - Vie du monstre

**Concept clé :** Système de dégâts réutilisable.

```csharp
using UnityEngine;

public class MonsterHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 30f;
    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    // Fonction publique pour infliger des dégâts
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log("Monstre touché ! Vie : " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Monstre éliminé !");
        // TODO : Particules, son, animation
        Destroy(gameObject);
    }
}
```

**🎓 Explications :**

- `[SerializeField]` sur une variable privée : Visible dans l'Inspector mais protégée
- Séparation en fonctions : `TakeDamage()` et `Die()` (clean code)
- `public void TakeDamage()` : Les autres scripts peuvent appeler cette fonction

**⚙️ Configuration Unity :**

1. Attache `MonsterHealth.cs` au prefab `Monster`
2. Ajuste `Max Health` dans l'Inspector (30 par défaut)

---

### 4.2 PlayerAttack.cs - Attaque par clic

**Concept clé :** Raycast 2D pour détecter le clic sur un monstre.

```csharp
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float attackCooldown = 0.5f;
    private float nextAttackTime = 0f;

    void Update()
    {
        // Si clic gauche ET cooldown fini
        if (Input.GetMouseButtonDown(0) && Time.time >= nextAttackTime)
        {
            AttackMonster();
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    void AttackMonster()
    {
        // Convertit la position de la souris en position monde
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Lance un raycast à cette position
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        // Si on a touché quelque chose
        if (hit.collider != null)
        {
            // Vérifie si c'est un monstre
            MonsterHealth monster = hit.collider.GetComponent<MonsterHealth>();

            if (monster != null)
            {
                monster.TakeDamage(attackDamage);
                Debug.Log("Attaque réussie !");
            }
        }
    }
}
```

**🎓 Explications :**

- `Input.GetMouseButtonDown(0)` : Clic gauche (0), droit (1), milieu (2)
- `Time.time` : Temps écoulé depuis le début du jeu
- `Camera.main.ScreenToWorldPoint()` : Convertit pixels écran → coordonnées monde
- `Physics2D.Raycast()` : Lance un "rayon" pour détecter les collisions
- `RaycastHit2D` : Contient les infos de ce qui a été touché
- Cooldown : Évite de spammer les attaques

**⚙️ Configuration Unity :**

1. Attache `PlayerAttack.cs` sur le `Player`
2. Sur le prefab `Monster` :
   - Ajoute `Box Collider 2D` ou `Circle Collider 2D`
   - **NE COCHE PAS "Is Trigger"** (doit bloquer le raycast)

**💡 Comprendre les Raycasts :**

```
Souris (pixels écran)
    → ScreenToWorldPoint
    → Position monde (x, y)
    → Raycast
    → Détecte les colliders à cette position
```

---

### 4.3 MonsterAttack.cs - Monstre attaque l'artefact

**Concept clé :** Dégâts au contact avec l'artefact.

```csharp
using UnityEngine;

public class MonsterAttack : MonoBehaviour
{
    [SerializeField] private float damage = 5f;
    [SerializeField] private float attackInterval = 1f;

    private bool isAttacking = false;
    private ArtefactHealth artefact;

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Si le monstre touche l'artefact
        if (collision.CompareTag("Artefact"))
        {
            artefact = collision.GetComponent<ArtefactHealth>();

            if (artefact != null && !isAttacking)
            {
                isAttacking = true;
                InvokeRepeating("AttackArtefact", 0f, attackInterval);
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        // Si le monstre s'éloigne de l'artefact
        if (collision.CompareTag("Artefact"))
        {
            isAttacking = false;
            CancelInvoke("AttackArtefact");
        }
    }

    void AttackArtefact()
    {
        if (artefact != null)
        {
            artefact.RestoreHealth(-damage); // Dégâts = vie négative
            Debug.Log("Monstre attaque l'artefact !");
        }
    }

    void OnDestroy()
    {
        // Annule les attaques si le monstre meurt
        CancelInvoke("AttackArtefact");
    }
}
```

**🎓 Explications :**

- `InvokeRepeating()` : Pour des attaques répétées tant que le monstre est au contact
- `CancelInvoke()` : Arrête les appels répétés
- `OnDestroy()` : Appelé quand l'objet est détruit (nettoyage)
- Astuce : `RestoreHealth(-5)` = infliger 5 dégâts (réutilise la fonction existante)

**⚙️ Configuration Unity :**

1. Attache `MonsterAttack.cs` au prefab `Monster`
2. Sur l'`Artefact` :
   - Tag : "Artefact"
   - Le Collider doit avoir "Is Trigger" ✅
3. Sur le `Monster` :
   - Ajoute un deuxième `Circle Collider 2D` (ou utilise l'existant)
   - **Coche "Is Trigger"** ✅

**💡 Astuce multi-colliders :**
Un GameObject peut avoir plusieurs Colliders :

- Un pour les raycasts du player (pas trigger)
- Un pour détecter l'artefact (trigger)

---

## 🎲 Étape 5 : Spawn des monstres

### 🎯 Objectif

Les monstres apparaissent automatiquement derrière les arbres.

### 📝 Script à créer

- `MonsterSpawner.cs`

---

### 5.1 MonsterSpawner.cs - Apparition automatique

**Concept clé :** Instantiate() pour créer des objets à partir de prefabs.

```csharp
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private GameObject monsterPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private float difficultyIncrease = 0.1f;

    private float currentInterval;

    void Start()
    {
        currentInterval = spawnInterval;
        InvokeRepeating("SpawnMonster", 2f, currentInterval);
    }

    void SpawnMonster()
    {
        // Choisit un point de spawn aléatoire
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[randomIndex];

        // Crée un monstre à cette position
        Instantiate(monsterPrefab, spawnPoint.position, Quaternion.identity);

        Debug.Log("Monstre spawné à " + spawnPoint.name);

        // Augmente la difficulté (spawn plus rapide)
        currentInterval = Mathf.Max(1f, currentInterval - difficultyIncrease);

        // Redémarre avec le nouvel intervalle
        CancelInvoke("SpawnMonster");
        InvokeRepeating("SpawnMonster", currentInterval, currentInterval);
    }
}
```

**🎓 Explications :**

- `GameObject` : Type pour stocker un prefab
- `Transform[]` : Tableau de positions (plusieurs points de spawn)
- `Random.Range(min, max)` : Nombre aléatoire entre min (inclus) et max (exclus)
- `Instantiate(prefab, position, rotation)` : Crée une copie du prefab dans la scène
- `Quaternion.identity` : Rotation par défaut (0°)
- `Mathf.Max(a, b)` : Retourne la plus grande valeur (ici, minimum 1 seconde)

**⚙️ Configuration Unity :**

**Créer les points de spawn :**

1. Hierarchy → Create Empty → Renomme "MonsterSpawner"
2. Create Empty (enfant de MonsterSpawner) → "SpawnPoint1"
   - Place-le derrière un arbre (X positif, hors écran)
3. Duplique pour créer SpawnPoint2, SpawnPoint3, etc.

**Configurer le spawner :**

1. Attache `MonsterSpawner.cs` sur `MonsterSpawner`
2. Glisse le prefab `Monster` vers `Monster Prefab`
3. Définis la taille du tableau `Spawn Points` (ex: 3)
4. Glisse SpawnPoint1, SpawnPoint2, SpawnPoint3 dans le tableau

**💡 Pourquoi des Transform[] ?**

```csharp
Transform[] spawnPoints; // Tableau de positions
// spawnPoints[0] = premier point
// spawnPoints[1] = deuxième point
// spawnPoints.Length = nombre total de points
```

---

## 🎮 Étape 6 : Game Manager et UI

### 🎯 Objectif

Gérer les états du jeu (Menu, Jeu, Game Over) et afficher les UI.

### 📝 Scripts à créer

- `GameManager.cs`
- `UIManager.cs`

---

### 6.1 GameManager.cs - Gestion des états

**Concept clé :** Singleton pattern + Enum pour les états.

```csharp
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Singleton : une seule instance accessible partout
    public static GameManager Instance { get; private set; }

    // États possibles du jeu
    public enum GameState { Menu, Playing, Paused, GameOver }
    public GameState currentState = GameState.Playing;

    [Header("Score")]
    public float survivalTime = 0f;
    private bool isGameActive = false;

    void Awake()
    {
        // Pattern Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persiste entre les scènes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        StartGame();
    }

    void Update()
    {
        // Compte le temps de survie
        if (isGameActive && currentState == GameState.Playing)
        {
            survivalTime += Time.deltaTime;
        }

        // Pause avec Échap
        if (Input.GetKeyDown(KeyCode.Escape) && currentState == GameState.Playing)
        {
            PauseGame();
        }
    }

    public void StartGame()
    {
        currentState = GameState.Playing;
        isGameActive = true;
        survivalTime = 0f;
        Time.timeScale = 1f; // Vitesse normale
    }

    public void PauseGame()
    {
        currentState = GameState.Paused;
        Time.timeScale = 0f; // Fige le jeu
        // TODO : Afficher menu pause
    }

    public void ResumeGame()
    {
        currentState = GameState.Playing;
        Time.timeScale = 1f;
    }

    public void GameOver()
    {
        currentState = GameState.GameOver;
        isGameActive = false;
        Time.timeScale = 0f;

        Debug.Log("GAME OVER ! Temps de survie : " + survivalTime + " secondes");
        // TODO : Afficher écran Game Over
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
```

**🎓 Explications :**

- **Singleton** : Pattern pour avoir une seule instance accessible via `GameManager.Instance`
- `enum` : Type personnalisé pour définir des états limités
- `Time.timeScale` : 0 = pause, 1 = normal, 2 = 2x plus rapide
- `DontDestroyOnLoad()` : L'objet survit au changement de scène
- `SceneManager.LoadScene()` : Recharge la scène (restart)

**⚙️ Configuration Unity :**

1. Create Empty → "GameManager"
2. Attache `GameManager.cs`

**💡 Utilisation depuis d'autres scripts :**

```csharp
// Dans ArtefactHealth.cs, remplace Destroy() par :
if (health <= 0)
{
    GameManager.Instance.GameOver();
    Destroy(gameObject);
}
```

---

### 6.2 UIManager.cs - Gestion de l'interface

```csharp
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject gameOverPanel;

    [Header("Game Over")]
    [SerializeField] private TextMeshProUGUI finalScoreText;

    void Start()
    {
        ShowGameUI();
    }

    void Update()
    {
        // Affiche l'UI correspondant à l'état du jeu
        switch (GameManager.Instance.currentState)
        {
            case GameManager.GameState.Menu:
                ShowMenuUI();
                break;
            case GameManager.GameState.Playing:
                ShowGameUI();
                break;
            case GameManager.GameState.Paused:
                ShowPauseUI();
                break;
            case GameManager.GameState.GameOver:
                ShowGameOverUI();
                break;
        }
    }

    void ShowMenuUI()
    {
        menuPanel.SetActive(true);
        gamePanel.SetActive(false);
        pausePanel.SetActive(false);
        gameOverPanel.SetActive(false);
    }

    void ShowGameUI()
    {
        menuPanel.SetActive(false);
        gamePanel.SetActive(true);
        pausePanel.SetActive(false);
        gameOverPanel.SetActive(false);
    }

    void ShowPauseUI()
    {
        pausePanel.SetActive(true);
    }

    void ShowGameOverUI()
    {
        gameOverPanel.SetActive(true);
        gamePanel.SetActive(false);

        // Affiche le score final
        int minutes = Mathf.FloorToInt(GameManager.Instance.survivalTime / 60f);
        int seconds = Mathf.FloorToInt(GameManager.Instance.survivalTime % 60f);
        finalScoreText.text = $"Temps de survie : {minutes:00}:{seconds:00}";
    }

    // Fonctions pour les boutons
    public void OnPlayButton()
    {
        GameManager.Instance.StartGame();
    }

    public void OnResumeButton()
    {
        GameManager.Instance.ResumeGame();
    }

    public void OnRestartButton()
    {
        GameManager.Instance.RestartGame();
    }

    public void OnQuitButton()
    {
        Application.Quit();
        Debug.Log("Quit game");
    }
}
```

**🎓 Explications :**

- `GameObject.SetActive(true/false)` : Affiche/cache un objet
- `switch` : Comme plusieurs `if/else` mais plus lisible
- `$"..."` : String interpolation (C# 6.0+) pour insérer des variables
- `{minutes:00}` : Format à 2 chiffres (ex: 05 au lieu de 5)
- Fonctions publiques : Appelées par les boutons UI

---

## 📚 Concepts Unity importants

### 🔄 Cycle de vie d'un MonoBehaviour

```
Création de l'objet
    ↓
Awake() ← Initialisation interne
    ↓
OnEnable() ← Quand l'objet devient actif
    ↓
Start() ← Initialisation externe (références)
    ↓
Update() ← Chaque frame (~60/s)
FixedUpdate() ← Intervalle fixe (physique)
LateUpdate() ← Après tous les Update (caméra)
    ↓ (boucle)

OnDisable() ← Quand l'objet devient inactif
OnDestroy() ← Avant destruction
```

### 🎯 Triggers vs Collisions

| Aspect           | Trigger             | Collision          |
| ---------------- | ------------------- | ------------------ |
| Blocage physique | ❌ Non              | ✅ Oui             |
| Détection        | ✅ Oui              | ✅ Oui             |
| Fonctions        | OnTriggerEnter2D    | OnCollisionEnter2D |
| Usage            | Zones, collectibles | Murs, obstacles    |

### 🔍 GetComponent vs Find

```csharp
// RAPIDE ✅ - Utilise une référence directe
public MonsterHealth monster;
monster.TakeDamage(10);

// LENT ❌ - Cherche dans toute la scène
GameObject.Find("Monster").GetComponent<MonsterHealth>().TakeDamage(10);

// BON COMPROMIS - Stocke la référence
private MonsterHealth cachedMonster;
void Start() {
    cachedMonster = GetComponent<MonsterHealth>();
}
```

### 🎨 Organisation du code

**Ordre recommandé dans un script :**

```csharp
// 1. Variables publiques (Inspector)
public float speed = 5f;

// 2. Variables privées sérialisées
[SerializeField] private GameObject prefab;

// 3. Variables privées
private Rigidbody2D rb;

// 4. Fonctions Unity (ordre de vie)
void Awake() { }
void Start() { }
void Update() { }
void FixedUpdate() { }

// 5. Fonctions publiques (API)
public void TakeDamage(float amount) { }

// 6. Fonctions privées (helpers)
private void Die() { }
```

### 💾 Bonnes pratiques

✅ **À FAIRE :**

- Utiliser `[SerializeField]` pour les variables privées que tu veux voir
- Cacher les variables qui ne doivent pas être modifiées
- Vérifier `!= null` avant d'utiliser `GetComponent`
- Utiliser `CompareTag()` au lieu de `== "Tag"`
- Mettre les `Find` dans `Start()`, pas dans `Update()`

❌ **À ÉVITER :**

- Tout mettre en `public` (exposer inutilement)
- `GameObject.Find()` dans `Update()` (trop lent)
- Oublier `Time.deltaTime` pour le mouvement
- Laisser des `Debug.Log()` dans le code final

---

## 🎓 Ressources pour continuer

### 📖 Documentation

- [Unity Manual](https://docs.unity3d.com/Manual/index.html)
- [C# Programming Guide](https://learn.microsoft.com/fr-fr/dotnet/csharp/)
- [Unity Learn](https://learn.unity.com/)

### 🎥 Tutoriels recommandés

- Brackeys (YouTube) - Débutant
- Code Monkey (YouTube) - Intermédiaire
- Sebastian Lague (YouTube) - Avancé

### 🛠️ Outils utiles

- Visual Studio / Rider - IDE
- Aseprite - Pixel art
- Audacity - Sons
- Git - Versioning

---

## 🏆 Prochaines étapes pour améliorer le jeu

1. **Animations** : Ajouter des Animators pour player/monster
2. **Particules** : Effets visuels (mort, collecte, etc.)
3. **Sons** : Musique de fond et SFX
4. **Power-ups** : Gemmes spéciales, boost de vitesse
5. **Vagues** : Système de vagues progressives
6. **High Score** : Sauvegarde avec PlayerPrefs
7. **Mobile** : Adapter les contrôles tactiles
8. **Polish** : Screen shake, feedback, juice

---

**Bon courage pour la suite de ton apprentissage Unity ! 🚀**

_Document créé le 2 décembre 2025_
