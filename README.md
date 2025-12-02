# 🏺 Le Gardien du Cristal

> Un jeu de défense 2D développé avec Unity - Protège l'artefact magique contre les monstres !

![Unity](https://img.shields.io/badge/Unity-2022.3.62f3-black?logo=unity)
![C#](https://img.shields.io/badge/C%23-Programming-239120?logo=csharp)
![URP](https://img.shields.io/badge/URP-14.0.12-blue)
![Status](https://img.shields.io/badge/Status-En%20développement-yellow)

---

## 📖 À propos du jeu

**Le Gardien du Cristal** est un jeu de défense en 2D où le joueur doit protéger un artefact magique qui perd progressivement sa vie. Pour le sauver, ramassez des gemmes et nourrissez l'artefact tout en combattant des monstres qui tentent de le détruire.

### 🎮 Gameplay

- 💎 **Ramassez des gemmes** dispersées sur la carte
- 🏺 **Nourrissez l'artefact** pour restaurer sa vie (touche E)
- ⚔️ **Combattez les monstres** en cliquant dessus
- ⏱️ **Survivez** le plus longtemps possible !

---

## ✨ Fonctionnalités actuelles

### ✅ Implémentées (Étapes 1-3)

- [x] Mouvement du joueur (WASD/Flèches)
- [x] Sprites directionnels du joueur (4 directions)
- [x] Caméra qui suit le joueur avec limites (X et Y)
- [x] Système de collecte de gemmes
- [x] Inventaire et affichage UI (compteur + icône)
- [x] Artefact qui perd de la vie automatiquement (1 HP/s)
- [x] Système de nourrissage (gemmes → +10 HP artefact)
- [x] Barre de vie de l'artefact (UI Slider)
- [x] Déplacement automatique des monstres

### 🔜 À venir (Étapes 4-6)

- [ ] Système de combat (clic sur les monstres)
- [ ] Monstres qui attaquent l'artefact
- [ ] Spawn automatique des monstres
- [ ] Difficulté progressive
- [ ] Menu principal et Game Over
- [ ] Système de score (temps de survie)
- [ ] Animations et effets visuels
- [ ] Sons et musique

---

## 🎨 Assets

Tous les assets graphiques ont été créés avec **Aseprite** :

- Sprites du joueur (4 frames d'animation)
- Sprite du monstre
- Gemmes (rouge et normale)
- Artefact magique
- Décors (arbres, pierres, terre)

---

## 🛠️ Technologies utilisées

| Technologie     | Version           | Usage                 |
| --------------- | ----------------- | --------------------- |
| **Unity**       | 2022.3.62f3       | Moteur de jeu         |
| **C#**          | .NET Standard 2.1 | Programmation         |
| **URP**         | 14.0.12           | Rendu graphique       |
| **TextMeshPro** | 3.0.7             | Interface utilisateur |
| **Aseprite**    | -                 | Création des sprites  |

---

## 📂 Structure du projet

```
Assets/
├── Scripts/              # Tous les scripts C#
│   ├── PlayerController.cs
│   ├── PlayerInventory.cs
│   ├── CameraController.cs
│   ├── MonsterController.cs
│   ├── GemmeController.cs
│   ├── ArtefactHealth.cs
│   └── ArtefactFeeder.cs
├── Prefabs/              # Prefabs réutilisables
│   ├── Player.prefab
│   ├── Monster.prefab
│   └── Gemme_rouge.prefab
├── Sprites/              # Tous les assets graphiques
├── Animations/           # Contrôleurs d'animation
├── Scenes/               # Scène principale
└── Tilemaps/             # Système de terrain
```

---

## 🚀 Installation et lancement

### Prérequis

- Unity 2022.3.62f3 (LTS)
- Visual Studio 2022 ou JetBrains Rider

### Étapes

1. **Clone le repository**

   ```bash
   git clone https://github.com/Lysdora/le_gardien_du_cristal.git
   ```

2. **Ouvre le projet dans Unity Hub**

   - Add → Sélectionne le dossier du projet
   - Ouvre avec Unity 2022.3.62f3

3. **Lance la scène**
   - Ouvre `Assets/Scenes/SampleScene.unity`
   - Appuie sur Play ▶️

---

## 🎮 Contrôles

| Touche                  | Action                           |
| ----------------------- | -------------------------------- |
| **WASD** ou **Flèches** | Déplacer le joueur               |
| **E**                   | Nourrir l'artefact (près de lui) |
| **Clic gauche**         | Attaquer un monstre _(à venir)_  |
| **Échap**               | Pause _(à venir)_                |

---

## 📚 Documentation

Le projet contient un **guide complet de développement** dans `GUIDE_COMPLET.md` qui explique :

- Le code ligne par ligne
- Les concepts Unity importants
- Les bonnes pratiques de programmation
- Les étapes de développement détaillées

Parfait pour apprendre Unity et C# !

---

## 🎓 Apprentissage

Ce projet est développé dans un but pédagogique pour apprendre :

- ✅ Les bases de Unity 2D
- ✅ La programmation en C#
- ✅ Les systèmes de gameplay (mouvement, collecte, combat)
- ✅ L'interface utilisateur (UI)
- ✅ Les triggers et collisions 2D
- ✅ La gestion d'état et le Game Manager

---

## 📈 Progression du développement

```
Étape 1 : Configuration de base ████████████████████ 100%
Étape 2 : Système de gemmes    ████████████████████ 100%
Étape 3 : Système artefact     ████████████████████ 100%
Étape 4 : Combat               ░░░░░░░░░░░░░░░░░░░░   0%
Étape 5 : Spawn automatique    ░░░░░░░░░░░░░░░░░░░░   0%
Étape 6 : Game Manager         ░░░░░░░░░░░░░░░░░░░░   0%
```

---

## 🤝 Contribution

Ce projet est personnel et à but éducatif. Les suggestions et feedbacks sont les bienvenus !

---

## 📝 Licence

Ce projet est développé à des fins d'apprentissage. Les assets graphiques sont originaux.

---

## 👤 Auteur

**Lysdora**

- GitHub: [@Lysdora](https://github.com/Lysdora)
- Projet créé en décembre 2025

---

## 🙏 Remerciements

- Unity Technologies pour le moteur et la documentation
- La communauté Unity pour les tutoriels
- Aseprite pour l'outil de pixel art

---

<div align="center">

**⭐ Si ce projet t'aide à apprendre Unity, n'hésite pas à laisser une étoile ! ⭐**

Fait avec ❤️ et beaucoup de ☕

</div>
