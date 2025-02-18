# KeyFrame Animator (After Effects)

![Unity](https://img.shields.io/badge/Unity-2022%2B-blue.svg)
![License](https://img.shields.io/badge/License-MIT-green.svg)

## 📌 Overview
Keyframe Animator is a Unity plugin that enables UI animation using keyframes exported from After Effects. It simplifies UI animation by directly converting AE coordinates into Unity positions while providing an intuitive editor for configuring and previewing animations effortlessly.




## 🛠 Installation
### Option 1: Unity Package Manager (UPM)
1. Open Unity and go to `Window > Package Manager`.
2. Click on `+` and select `Add package from git URL...`.
3. Enter: https://github.com/TheoLardeurGersztein/com.theolardeurgersztein.keyframeanimator




## 🪄​ How to 

### Creating a Keyframe Config

1. **Open the Keyframe Animator Tool**: In Unity's top menu, go to **Tools** > **Keyframe Animation Config**.
   
2. **Create a New Keyframe Config**:
   - The tool will automatically create a new **KeyframeConfig** asset for you.
   - You can also manually create the asset by clicking **Create KeyframeConfig** in the window.

> **⚠️ Warning:**
> Creating and modifying KeyFrameConfig must be done in the **KeyFrame Animation Config menu** in Unity and not the **inspector**.

3. **Assign Your UI Element and Set Update Rate**:
   - Drag and drop the target UI element (e.g., a `RectTransform` of a UI object) into the **Target UI Object** field.
   - Adjust the **Update Rate** slider to control how fast the keyframes should update during the animation (e.g., 0.04).

5. **Enter Keyframe Data**:
   - Copy and paste your After Effects keyframe data into the **Keyframe Input** field.
   - Example format for After Effects keyframes:

    ```markdown
    Transform	Position
	Frame	X pixels	Y pixels	Z pixels
     0    100    200    0
     1    120    220    0
     2    140    240    0
     ```

6. **Load Keyframes**: Click **Load Keyframes** to convert the data into a list of keyframes.

### Applying the Animation

1. **Add the Keyframe UI Animator Component**:
   - On the **Raw Image** you wish to animate, add the `KeyframeUIAnimator` script.
   - Drag the **KeyframeConfig** asset(s) you created into the **Config** field of the component.

2. **Play the Animation**:
   - Run your scene, and the UI element will animate according to the keyframe data you've set up.


## 📂 Plugin Structure

```markdown
📦 YourPlugin
├── 📂 Editor
|   ├── KeyFrameSetup.cs
├── 📂 Runtime
│   ├── KeyFrameConfig.cs
│   ├── KeyFrameUIAnimator.cs
├── README.md
```