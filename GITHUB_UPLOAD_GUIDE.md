# 📤 How to Upload to GitHub - Step by Step

## Method 1: Using GitHub Desktop (Easiest)

### Step 1: Install GitHub Desktop
- Download from: https://desktop.github.com/
- Install and sign in with your GitHub account

### Step 2: Create a New Repository
1. Click "File" → "New Repository"
2. Name: `SpacePod_Midterm_Game`
3. Description: "Space Observation Pod - 3D Game Midterm Project"
4. Local Path: Choose where you saved the project
5. Click "Create Repository"

### Step 3: Publish to GitHub
1. Click "Publish repository" button
2. Choose to keep it public or private
3. If private, add your instructor as a collaborator later
4. Click "Publish repository"

### Step 4: Submit
- Copy the repository URL from GitHub Desktop or GitHub.com
- Submit the URL to your instructor

## Method 2: Using Git Command Line

### Prerequisites
```bash
# Install Git if you haven't already
# Windows: https://git-scm.com/download/win
# Mac: brew install git
# Linux: sudo apt-get install git

# Configure Git (first time only)
git config --global user.name "Your Name"
git config --global user.email "your.email@example.com"
```

### Step 1: Navigate to Project Directory
```bash
cd /path/to/SpacePod_Midterm_Game
```

### Step 2: Initialize Git Repository
```bash
# Initialize a new Git repository
git init

# Add all files to staging
git add .

# Commit the files
git commit -m "Initial commit: Space Observation Pod midterm project"
```

### Step 3: Create GitHub Repository
1. Go to https://github.com/new
2. Repository name: `SpacePod_Midterm_Game`
3. Description: "Space Observation Pod - 3D Game Midterm Project"
4. Choose Public or Private
5. **DO NOT** initialize with README, .gitignore, or license (we already have these)
6. Click "Create repository"

### Step 4: Push to GitHub
```bash
# Add the GitHub repository as remote
git remote add origin https://github.com/YOUR_USERNAME/SpacePod_Midterm_Game.git

# Push your code to GitHub
git branch -M main
git push -u origin main
```

### Step 5: If Repository is Private
```bash
# On GitHub.com, go to your repository
# Click Settings → Collaborators
# Click "Add people"
# Enter your instructor's GitHub username
# Click "Add to repository"
```

## Method 3: Upload Directly on GitHub.com (Quick but Basic)

### Step 1: Create Repository on GitHub
1. Go to https://github.com/new
2. Repository name: `SpacePod_Midterm_Game`
3. Choose Public or Private
4. **DO NOT** check "Add a README file"
5. Click "Create repository"

### Step 2: Upload Files
1. Click "uploading an existing file"
2. Drag and drop the entire `SpacePod_Midterm_Game` folder
3. Wait for all files to upload (may take a minute)
4. Add commit message: "Initial commit: Space Observation Pod"
5. Click "Commit changes"

### Step 3: Verify Upload
Check that all these files are present:
- [ ] README.md
- [ ] .gitignore
- [ ] Game.sln
- [ ] Game/ folder with all source files
- [ ] Game/Shaders/ folder with .glsl files
- [ ] Game/Assets/ folder with texture images
- [ ] Game/Graphics/ folder with helper classes

## 🔍 Verification Checklist

After uploading, verify your repository:

### File Structure Check
Your repository should look like this on GitHub:
```
SpacePod_Midterm_Game/
├── README.md ✅
├── .gitignore ✅
├── Game.sln ✅
├── SUBMISSION_GUIDE.md ✅
└── Game/
    ├── Game.csproj ✅
    ├── Program.cs ✅
    ├── Game.cs ✅
    ├── Shaders/
    │   ├── vertex.glsl ✅
    │   └── fragment.glsl ✅
    ├── Assets/
    │   ├── planet_texture.png ✅
    │   ├── metal_texture.png ✅
    │   └── stars_texture.png ✅
    └── Graphics/
        ├── Shader.cs ✅
        ├── Texture.cs ✅
        ├── Mesh.cs ✅
        └── Camera.cs ✅
```

### Content Check
- [ ] README.md displays properly with all sections
- [ ] Images (.png files) are visible in the Assets folder
- [ ] All .cs files have proper syntax highlighting
- [ ] .gitignore is present (bin/ and obj/ folders should NOT be in repo)

## 🎯 Getting the Repository URL

### For Submission:
1. Go to your repository on GitHub.com
2. Click the green "Code" button
3. Copy the HTTPS URL (e.g., `https://github.com/yourusername/SpacePod_Midterm_Game`)
4. Alternatively, just copy the URL from your browser's address bar

## 🔐 If Making Repository Private

If you created a private repository, you MUST add your instructor as a collaborator:

1. Go to your repository on GitHub.com
2. Click "Settings" (top menu)
3. Click "Collaborators" in the left sidebar
4. Click "Add people" button
5. Enter your instructor's GitHub username
6. Click "Add [username] to this repository"
7. They will receive an invitation email

**Note**: Get your instructor's GitHub username from them before doing this!

## ❓ Troubleshooting

### "Repository already exists"
- Choose a different name, or delete the existing repository on GitHub first

### "Permission denied"
- Make sure you're logged into GitHub
- Check if you need to set up SSH keys or use HTTPS with token

### "Large files rejected"
- The texture files should be small enough (<1MB each)
- If you have bin/ or obj/ folders, they shouldn't be committed (check .gitignore)

### "Can't see my files on GitHub"
- Make sure you committed the files: `git status` should show "nothing to commit"
- Check you pushed to the right branch: `git branch` shows current branch
- Refresh your browser on GitHub.com

## 📧 What to Submit

Send your instructor:
1. **GitHub Repository URL** (e.g., `https://github.com/yourusername/SpacePod_Midterm_Game`)
2. If private repository: Confirm you added them as collaborator
3. Optional: Brief note about any bonus features you implemented

## ⏰ Final Reminder

**Deadline: October 30 at 11:59pm**

### Last-Minute Checklist:
- [ ] All files uploaded to GitHub
- [ ] Repository URL copied
- [ ] If private, instructor added as collaborator
- [ ] README.md displays correctly
- [ ] Submission sent to instructor

**Don't wait until the last minute!** Upload early and verify everything works.

---

Good luck with your submission! 🚀
