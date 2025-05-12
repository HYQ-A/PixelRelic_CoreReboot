# PixelRelic_CoreReboot (像素遗迹)

![Unity Version](https://img.shields.io/badge/Unity-2021.3%2B-blue)
![License](https://img.shields.io/badge/License-MIT-green)

一款基于Unity3D实现的2.5D动作冒险游戏，融合探索、战斗与叙事元素。项目采用模块化设计，支持扩展性强的高效开发模式。

---

## 🌟 核心特性

### 1. **2.5D 动态视角系统**
- **摄像机智能跟随**：通过`RotatingCamera`实现平滑视角旋转（Q/E键控制），保持玩家始终居中。
- **Sprite自适应朝向**：`FacingCamera`确保场景物体始终面向摄像机，增强2.5D沉浸感。

### 2. **战斗与AI系统**
- **武器连击机制**：`WeaponController`支持多方向攻击判定，使用`Physics2D.OverlapBox`精准检测伤害区域。
- **怪物行为树**：`MonsterObj`实现巡逻、追击、攻击逻辑，包含转向平滑插值和协程控制的等待/返回逻辑。
- **玩家战斗反馈**：受击硬直、血量管理（`PlayerObj`）与技能冷却UI(待完善)（`GamePanel`）。

### 3. **交互与叙事系统**
- **动态对话系统**：`NpcOneObj`解析CSV对话数据，支持多角色对话位置切换，触发式交互（OnTriggerEnter2D）。
- **UI面板管理**：通过`UIManager`统一控制背包(`BagPanel`)、对话(`TalkPanel`)、游戏主界面(`GamePanel`)的显隐与数据绑定。

### 4. **数据驱动的系统设计**
- **LitJson数据存储**：角色属性（`PlayerInfo`/`MonsterInfo`）与游戏配置通过JSON序列化/反序列化实现动态加载。
- **模块化UI组件**：技能图标、血条等UI元素通过List动态管理，支持灵活配置。

---

## 🛠️ 技术实现亮点

- **协程优化**：怪物巡逻、摄像机旋转等耗时操作通过协程实现非阻塞平滑过渡。
- **动画状态机**：玩家/怪物动画通过Animator参数精确控制（如`Walk`、`Attack`状态切换）。
- **碰撞检测优化**：采用LayerMask分层检测，减少性能开销。
- **单例模式**：`PlayerObj`与`UIManager`使用单例确保全局唯一访问。

---

## 📦 系统模块

| 模块          | 关键脚本                     | 功能描述                              |
|---------------|------------------------------|-------------------------------------|
| 角色控制      | `PlayerObj`, `WeaponController` | 移动、攻击、受击反馈与动画控制        |
| 怪物AI        | `MonsterObj`                 | 巡逻、追击、攻击逻辑与状态管理        |
| 摄像机        | `RotatingCamera`, `FacingCamera` | 动态视角控制与2.5D效果实现           |
| UI系统        | `BagPanel`, `GamePanel`      | 背包物品展示、技能冷却与血条管理      |
| 对话系统      | `NpcOneObj`, `TalkPanel`     | CSV对话解析、多角色位置自适应文本显示 |

---

## 🚀 快速开始

1. **环境要求**  
   - Unity 2021.3+
   - LitJson插件（已集成数据序列化功能）

2. **克隆项目**  
   ```bash
   git clone https://github.com/your-username/PixelRelic_CoreReboot.git
