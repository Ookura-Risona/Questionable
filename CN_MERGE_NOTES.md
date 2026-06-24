# 汉化分支合并上游保留清单

本文件只记录后续合并上游时仍有用的约束。合并前先读本文件；合并后如果某项已经被上游吸收、实现改变或不再需要，必须同步更新本文件。

## 必须保留

### 汉化仓库加载来源限制

相关文件：

- `Questionable/QuestionablePlugin.cs`
- `Questionable/Configuration.cs`
- `Questionable/Questionable.csproj`
- `Questionable/packages.lock.json`

保留内容：

- Release 构建下检查插件来源，只允许指定汉化插件源加载。
- `Configuration.SetupToken` 使用 Windows DPAPI 保存设置完成标记。
- 来源不正确时用 toast 和 Dalamud 通知阻止加载。

注意：

- Debug 构建必须仍可本地调试。
- 如果插件源地址变更，需要同步更新 `RepoCheck()`。

### 汉化版发布流程和插件元数据

相关文件：

- `.github/workflows/release.yml`
- `.gitignore`
- `Questionable/Questionable.json`

保留内容：

- `release.yml` 按 `*-cn` 标签发布汉化版。
- 从标签去掉 `-cn` 得到版本号，完整标签写入 InformationalVersion。
- `Questionable.json` 使用汉化版作者、说明、标签和仓库地址。
- `.gitignore` 忽略 `Directory.Build.props`。

### AEAssist 战斗模块

相关文件：

- `Questionable/Controller/CombatModules/AEAssistModule.cs`
- `Questionable/Configuration.cs`
- `Questionable/QuestionablePlugin.cs`
- `Questionable/Windows/ConfigComponents/PluginConfigComponent.cs`

保留内容：

- `Configuration.ECombatModule.AEAssist`。
- DI 中注册 `ICombatModule, AeAssistModule`。
- 配置页能选择 AEAssist。
- 开始战斗时执行 AEAssist 拉怪相关命令，停止时恢复停手命令。

### DailyRoutines 兼容

相关文件：

- `Questionable/External/DailyRoutinesIpc.cs`
- `Questionable/Configuration.cs`
- `Questionable/QuestionablePlugin.cs`
- `Questionable/Windows/ConfigComponents/GeneralConfigComponent.cs`
- `Questionable/Controller/Steps/Shared/AethernetShortcut.cs`

保留内容：

- 自动任务运行时临时卸载 DailyRoutines 冲突模块。
- 停止或退出时恢复被临时卸载的模块。
- 可选使用 DailyRoutines BetterTeleport/`/pdrtelepo` 处理主城小水晶传送。
- 传送功能必须保留开关，默认不开启。

当前冲突模块列表：

- `AutoTalkSkip`
- `AutoCutsceneSkip`
- `OptimizedInteraction`
- `AutoQuestComplete`
- `AutoFateSync`
- `InstantDismount`
- `AutoHideGameObjects`
- `AutoUnlockAllContents`

### 内置 AutoSnipe 处理

相关文件：

- `Questionable/Tweak/AutoSnipeHandler.cs`
- `Questionable/QuestionablePlugin.cs`

保留内容：

- 在 Questionable 运行时自动处理狙击小游戏，减少对 Automaton/CBT AutoSnipe 的依赖。
- 签名 hook 更新后必须实际测试；如果上游未来提供等价内置实现，可改用上游实现。

### 默认关闭上游问题上报

相关文件：

- `Questionable/Configuration.cs`

保留内容：

- `DismissedReportWarning` 默认 `true`。
- `ReportsDisabled` 默认 `true`。

原因：

- 汉化版问题通常应反馈到汉化分支，不应默认发给上游。

### 汉化版关于页和反馈入口

相关文件：

- `Questionable/Windows/ConfigComponents/AboutConfigComponent.cs`
- `Questionable/Windows/ConfigWindow.cs`
- `Questionable/QuestionablePlugin.cs`

保留内容：

- 汉化版免费声明。
- 原作者和汉化作者信息。
- 汉化分支 issue 与插件源入口。

## 可酌情保留

### Pandora's Box IPC 报错降噪

相关文件：

- `Questionable/External/PandorasBoxIpc.cs`

当前做法是压低 Pandora IPC 查询、暂停、恢复失败时的 warning。可保留，但如果上游已有更好的日志策略，应采用上游实现。

### NPC 高亮默认关闭

相关文件：

- `Questionable/Configuration.cs`

`Advanced.HighlightSelectedNpc` 默认关闭。不是核心功能，冲突严重时可接受上游默认值。

## 已被上游吸收或不再保留

### vnavmesh 版本兼容

上游新版 `NavmeshIpc.Pathfind` 已不再进行旧的 `< 1.2.3.2` 强制版本拦截，因此不需要继续保留旧的注释补丁。以后如果上游重新加入严格版本拦截，再重新评估国服兼容。

### 硬编码中文 UI 文本

上游已有 i18n 和 zh-cn 翻译。合并时不要恢复旧的硬编码中文 UI 文本；缺失翻译应补 `Questionable/Resources/I18N.xml` 或对应 i18n 资源。

### 手写水晶中文名全集

DailyRoutines 传送不再要求在 `EAetheryteLocation` 中维护整段手写中文名。优先从 Lumina 数据读取名称，只对特殊地点保留小映射。
