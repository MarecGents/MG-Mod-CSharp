# MG-Mod-CSharp

**MG-Mod 服务端 Mod 核心逻辑库** — 基于 C# 的 SPTarkov Mod 框架

[![SPT Version](https://img.shields.io/badge/SPT-4.1.0-blue)](https://dev.sp-tarkov.com/)
[![MGMod Version](https://img.shields.io/badge/version-v0.9.0.040100-green)]()
[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/License-CC%20BY--NC--ND%204.0-lightgrey)](LICENSE)

---

## 📖 简介

**MG-Mod-CSharp** 是 MG-Mod 家族的核心项目，使用 C# 编写的 SPTarkov 服务端 Mod。它通过依赖注入（DI）架构深度集成 SPTarkov 框架，实现对游戏服务器的全面配置与功能扩展。

本仓库编译产出的 DLL 为 MG-Mod 提供全部后端逻辑支持，是 MG-Mod 功能体系的技术基石。

---

## 🏗️ 技术架构

```
MG-Mod-CSharp/
├── MGmod.cs              ← Mod 入口（IOnLoad 加载器）
├── types/
│   ├── models/           ← 数据模型定义
│   │   ├── Custom/       ← 自定义类型（配置/键映射）
│   │   ├── EFT/          ← EFT 游戏类型映射
│   │   └── Paths/        ← 路径管理
│   ├── server/           ← 服务器模块（8 个子系统）
│   ├── services/         ← 业务服务层（7 个服务）
│   └── utils/            ← 工具类
├── db/                   ← 游戏数据库覆盖
├── res/                  ← 运行时配置和资源
├── traders/              ← 自定义商人数据
├── images/               ← 图片资源
├── bundles/              ← 资源包（编译时输出）
└── Logg/                 ← 运行日志目录
```

### 🔩 技术栈

| 组件 | 技术 |
|------|------|
| 语言 | C# 14 |
| 运行时 | .NET 10.0 |
| 项目类型 | 类库 (Library) |
| 框架依赖 | SPTarkov.Common 4.1.0, SPTarkov.DI 4.1.0, SPTarkov.Server.Core 4.1.0 |
| 架构模式 | 依赖注入 (DI) + 服务层 |
| 版本 | v0.9.0.040100 |

---

## 🧩 模块体系

### Server 层（8 个服务器模块）

| 模块 | 职能 |
|------|------|
| `BotsServer` | AI 机器人参数配置 |
| `ConfigsServer` | 全局配置（空投、AI生成、战局默认、搜刮倍率等） |
| `GlobalsServer` | 全局游戏参数（跳蚤、撤离、装载速度、装备增益） |
| `HideoutServer` | 藏身处系统（建造/生产/Scav箱/燃料/QTE） |
| `LocalesServer` | 本地化/翻译 |
| `LocationsServer` | 战局地图（时间/Boss刷新/撤离/通过率） |
| `TemplatesServer` | 模板数据（物品鉴定、容器扩容、堆叠、过滤） |
| `TradersServer` | 商人系统（保险、交易） |

### Services 层（7 个业务服务）

| 服务 | 职能 |
|------|------|
| `ConfigSettingServices` | 配置加载与分发（核心调度） |
| `CustomItemServices` | 自定义物品管理 |
| `CustomTraderServices` | 自定义商人管理 |
| `CustomAssortServices` | 自定义商品配置 |
| `CustomProfileServices` | 自定义存档管理 |
| `KeyClassfyServices` | 钥匙分类管理 |
| `SyncFleaMarketServices` | **实时跳蚤同步** — 从 MG-FleaMarket 仓库获取最新价格 |
| `TestServices` | 测试服务 |

---

## 🔄 与其他项目的关系

```
                    ┌─────────────────┐
                    │   MGModEditor   │  ← 可视化配置 GUI（WPF）
                    └────────┬────────┘
                             │ 编辑 config.json
                             ▼
┌──────────────┐     ┌─────────────────┐     ┌──────────────────┐
│ MG-FleaMarket│ ←── │ MG-Mod-CSharp   │ ──→ │   MG-Mod（发布） │
│ (价格数据源)  │ ←Sync│ (核心逻辑引擎)   │ →Bin│   (整合发布仓库)  │
└──────────────┘     └────────┬────────┘     └──────────────────┘
                             │ 共享独立商人逻辑
                             ▼
                    ┌─────────────────┐
                    │  MGGTMod-CSharp │  ← 独立发布的通用商人 Mod
                    └─────────────────┘
```

---

## 🚀 构建与使用

### 前置要求
- .NET 10.0 SDK
- Visual Studio 2022 或 JetBrains Rider

### 构建
```bash
dotnet build MGmod/MGMod.csproj -c Release
```

### 安装
构建输出的 `MGMod.dll` 与其他资源文件已整合至 [MG-Mod 发布仓库](https://github.com/MarecGents/MG-Mod/releases)，无需单独部署。

---

## 📄 许可证

本项目采用 **CC BY-NC-ND 4.0** 协议。

- ✅ 允许免费使用和分享（需署名）
- ❌ **禁止商业用途**
- ❌ **禁止修改后重新发布**

保留所有版权。详见 [LICENSE](LICENSE) 文件。

---

## 🔗 相关链接

- [ODDBA 发布帖](https://sns.oddba.cn/183236.html)
- [MG-Mod（发布仓库）](https://github.com/MarecGents/MG-Mod)
- [MGModEditor（配置编辑器）](https://github.com/MarecGents/MGModEditor)
- [MG-FleaMarket（实时跳蚤同步）](https://github.com/MarecGents/MG-FleaMarket)
- [MGGTMod-CSharp（独立商人 Mod）](https://github.com/MarecGents/MGGTMod-CSharp)
- [MG-GT-Mod（独立商人发布仓库）](https://github.com/MarecGents/MG-GT-Mod)
- 作者：[MarecGents](https://sns.oddba.cn/author/92586) | [爱发电](https://ifdian.net/a/MarecGents)
