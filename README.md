# 🛠️ WinOps Toolkit

A modula Powershell-based Windows Configuration app with a clean
WPF GUI frontend. Designed to automate system tweaks, schedulig, and power management using standalone scripts - great for sysadmins, power users, and developers learning Windows internals.

The purpose of this project is to:
- Use C# to create a project that solves a real-world problem
- Help to manage my System
- Learn Powershell scripting

[View Here](https://sammy-john.github.io/WinOps-Toolkit/)

---

## 🚀 What It Does

WinOps Toolkit currently includes:

### 📁 Download Sorter
- Automatically clean and organize your Downloads folder.
- Works on-demand or via Task Scheduler
- Configurable rules for file extensions and log tracking

### ⚡ Power Profiles Manager
- Creates `DayProfile` and `NightProfile` plans for current active power scheme
- Sets screen and sleep timeouts for each
- Supports time-based auto-switching via task scheduler

---

## 🎥 Demo Videos

| Module           | Watch                                                   |
|------------------|----------------------------------------------------------|
| Download Sorter  | 📹 [Download Sorter Demo](#) *(placeholder)*             |
| Power Profiles   | 📹 [Power Profiles Demo](#) *(placeholder)*              |

---

## 🧩 How It Works

- 🧠 Modular scripts stored in `scripts/PowerProfiles/`
- ⚙️ WPF frontend (in `/app`) calls these scripts using a helper class
- 🧭 Users can manually configure scripts or extend the tool with new modules

---

## 📂 Folder Structure

```plaintext
WinOps-Toolkit/
├── app/                  # WPF app (WinOpsToolkit.sln, views, helpers)
├── scripts/              # PowerShell scripts for modules
│   └── PowerProfiles/
├── landing/              # GitHub Pages landing page
├── README.md
├── ROADMAP.md
├── LICENSE (Apache 2.0)
└── .gitignore
```

---

## ⚠️ Setup Instructions

1. 🧪 Clone this repo:
   ```bash
   git clone https://github.com/your-username/WinOps-Toolkit.git
   ```

2. 🛠 Open `app/WinOpsToolkit.sln` in Visual Studio

3. ⚡ Scripts must be placed under:
   ```
   D:\Scripts\PowerProfiles\
   ```

   > Alternatively, modify the paths in `PowerProfilesView.xaml.cs` to match your preferred script directory.

4. ▶️ Build and run the WPF app to launch the toolkit

---

## 🔐 License

Licensed under the [Apache 2.0 License](LICENSE).

---

## 💡 Contributions

This repo is currently for personal use and demonstration. PRs and forks welcome, but major contributions should be discussed first.

---

## ✍️ Author

**Sammy John Rawlinson**  
Contact: [sjr.dev@protonmail.com](sjr.dev@protonmail.com)
