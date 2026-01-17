# 🍓 Celeste Bridge - Unity Port

<p align="center">
  <img alt="Celeste" src="https://assets.nintendo.com/image/upload/c_fill,w_1200/q_auto:best/f_auto/dpr_2.0/ncom/fr_CA/games/switch/c/celeste-switch/hero" width="800">
</p>

<p align="center">
  <strong>A Unity port bridging the gap between classic Celeste and modern game development</strong>
</p>

<p align="center">
  <a href="#-introduction">Introduction</a> •
  <a href="#-disclaimer">Disclaimer</a> •
  <a href="#-team--tasks">Team & Tasks</a> •
  <a href="#-features">Features</a> •
  <a href="#-getting-started">Getting Started</a> •
  <a href="#-contributing">Contributing</a>
</p>

---

## 📖 Introduction

**Celeste Bridge** is a comprehensive Unity port of the beloved indie platformer Celeste. This project aims to recreate the original game's mechanics, feel, and charm within the Unity engine while maintaining compatibility with the original XNA Framework, Monocle engine, MonoGame, and FNA implementations.

The goal of this project is threefold:
1. **Educational:** Provide a learning resource for game developers interested in understanding Celeste's architecture
2. **Preservation:** Create a cross-platform version that can run on modern systems through Unity
3. **Modding:** Enable new modding possibilities through Unity's ecosystem and tooling

This is a passion project born from deep appreciation for the original game's design and the desire to explore its systems in a different engine context.

---

## ⚠️ Disclaimer

> **This project is NOT official and is NOT endorsed by Extremely OK Games (EXOK) or any affiliated parties.**

This is an **unofficial**, **fan-made**, community-driven port created purely for educational and research purposes. 

### Important Points:

- ❌ This project is **not affiliated** with Maddy Makes Games, Extremely OK Games, or any official Celeste development team
- ❌ This repository does **not include** any original game assets (sprites, audio, maps, etc.)
- ❌ This is **not a replacement** for the original game
- ✅ You **must own** the original Celeste game to use any assets with this project
- ✅ Please **support the original developers** by purchasing the game legally

### Original Game Credits

| Role | Credit |
|------|--------|
| **Creators** | Maddy Thorson & Noel Berry |
| **Studio** | Extremely OK Games (EXOK) |
| **Original Engine** | XNA Framework / MonoGame / FNA |
| **Music** | Lena Raine |

**Buy the original game:** [Steam](https://store.steampowered.com/app/504230/Celeste/) • [itch.io](https://maddymakesgames.itch.io/celeste) • [Nintendo Switch](https://www.nintendo.com/games/detail/celeste-switch/) • [PlayStation](https://store.playstation.com/en-us/product/UP2120-CUSA11302_00-CELESTEXXPS4FG01) • [Xbox](https://www.xbox.com/en-US/games/store/celeste/BZ18WQHFSWW8)

---

## 👥 Team & Tasks

### Project Structure

This project is organized into several key areas, each with specific goals and responsibilities:

| Area | Description | Status |
|------|-------------|--------|
| **Core Engine** | Monocle engine port and Unity integration | 🔄 In Progress |
| **Gameplay Systems** | Player mechanics, entities, collision | 🔄 In Progress |
| **Rendering** | Graphics pipeline, shaders, effects | 🔄 In Progress |
| **Audio** | FMOD integration, music, sound effects | 📋 Planned |
| **Level Loading** | Map parsing, room transitions | 🔄 In Progress |
| **UI/Menus** | Menu systems, HUD, overlays | 📋 Planned |
| **Modding Support** | Everest compatibility, BepInEx | 📋 Planned |

### Task Priorities

#### 🔴 High Priority
- [ ] Complete player movement and physics
- [ ] Implement core entity systems (spikes, springs, platforms)
- [ ] Room loading and transitions
- [ ] Basic collision detection

#### 🟡 Medium Priority
- [ ] Visual effects and particles
- [ ] Audio system integration
- [ ] Save/load system
- [ ] All chapter content

#### 🟢 Low Priority
- [ ] Modding API
- [ ] Map editor integration
- [ ] Achievement system
- [ ] Speedrun features

### How to Contribute

We welcome contributors! See the [Contributing](#-contributing) section below for guidelines on how to get involved.

---

## 🎮 Features

### Core Features
- **Unity 6 Compatible** — Full compatibility with Unity 6 (6000.0) or later
- **Multi-Framework Support** — Maintains XNA, Monocle, MonoGame, and FNA compatibility
- **Asset Pipeline** — Converts original Content to Unity Assets (Scenes, Sprites, Audio)
- **Modular Architecture** — Clean separation between engine, gameplay, and Unity layers

### Unity Integration
- **Physics Integration** — Optional Unity physics mode alongside original collision
- **Room Wrapper System** — Seamless room transitions with loading/unloading
- **Advanced Camera** — Celeste-style camera with look-ahead and smoothing
- **Tweening System** — High-performance animation and interpolation

### Planned Features
- **3D Overworld** — Mountain, bird, moon, building, and heart 3D models
- **Framework Conversion** — Automatic conversion between Monocle/XNA and Unity
- **BepInEx Integration** — Unity-specific modding support
- **Loenn Compatibility** — Map editor integration

---

## 🔧 Requirements

| Requirement | Version |
|-------------|---------|
| Unity | 6 (6000.0+) |
| .NET | Framework 4.8 or .NET 6+ |
| FMOD | Required for audio |
| Original Game | Required for assets |

---

## 🚀 Getting Started

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/BloodLantern/Celeste.git
   cd Celeste
   ```

2. **Open in Unity**
   - Launch Unity Hub
   - Click "Add" and select the cloned folder
   - Open with Unity 6

3. **Install dependencies**
   - Open Package Manager (Window > Package Manager)
   - Install required packages

4. **Configure FMOD** (for audio)
   - Follow FMOD Unity integration guide
   - Set up audio banks

5. **Copy game assets** (requires original game ownership)
   - Locate your Celeste installation
   - Copy Content folder to project

6. **Build and run**

---

## 🔨 Modding Support

This port includes compatibility layers for popular Celeste modding tools:

| Tool | Description | Link |
|------|-------------|------|
| **EverestAPI** | Celeste modding framework | [GitHub](https://github.com/EverestAPI/Everest) |
| **Loenn** | Map editor | [GitHub](https://github.com/CelestialCartographers/Loenn) |
| **BepInEx** | Unity modding framework | [GitHub](https://github.com/BepInEx/BepInEx) |

---

## 📖 Documentation

- 📘 [Setup Guide](Documentation/Setup-Guide.md) — Complete setup and usage guide
- 📗 [Unity Integration](Documentation/Unity-Integration.md) — Unity-specific documentation
- 📙 [Architecture Overview](Documentation/Architecture.md) — System design and structure

---

## 🤝 Contributing

Contributions are welcome and appreciated! Here's how you can help:

### Ways to Contribute
- 🐛 **Bug Reports** — Found a bug? Open an issue with details
- 💡 **Feature Requests** — Have an idea? Share it in discussions
- 🔧 **Code Contributions** — Submit PRs for improvements
- 📝 **Documentation** — Help improve guides and docs
- 🧪 **Testing** — Test builds and report issues

### Guidelines
1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

Please ensure all changes maintain compatibility with the original frameworks.

---

## 📜 License & Legal

### Legal Notice

> **This is an unofficial port. All original assets, code, and intellectual property belong to Extremely OK Games.**

- This project is for **educational purposes only**
- **No original game assets** are included in this repository
- Users must **own the original game** to use any Celeste assets
- Please **support the original developers** by purchasing the game

### Support the Developers

🎮 **Buy Celeste:** https://www.celestegame.com/

---

<p align="center">
  Made with ❤️ by the community, for the community
</p>

<p align="center">
  <em>"You can do this."</em> — Celeste
</p>
