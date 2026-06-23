# 汉化分支合并上游时的保留说明

## 合并时必须优先保留

### 汉化仓库加载来源限制

相关文件：

- `Questionable/QuestionablePlugin.cs`
- `Questionable/Configuration.cs`
- `Questionable/Questionable.csproj`
- `Questionable/packages.lock.json`

目的：

- 非 `DEBUG` 构建下检查插件来源仓库，只允许指定的汉化作者仓库或个人插件源加载。
- `Configuration.SetupToken` 使用 Windows DPAPI 保存设置完成标记，避免简单复制配置绕过首次设置或来源限制。
- 失败时通过 toast 和 Dalamud 通知明确提示“本地加载或安装来源不正确”。

保留理由：

- 这是汉化版的发行控制和反倒卖保护逻辑，上游不会包含。
- 合并上游时如果构造函数参数、Dalamud 通知 API 或服务注册方式变化，应按新版 API 重新接入，而不是直接删除。

注意：

- 这会影响本地调试和开发加载。保留时必须确认 `#if !DEBUG` 仍能让 Debug 构建正常调试。
- 如果以后更换插件源地址，需要同步更新 `RepoCheck()`。

### 汉化版发布流程和插件元数据

相关文件：

- `.github/workflows/release.yml`
- `.gitignore`
- `Questionable/Questionable.json`

目的：

- 只在 `*-cn` 标签推送时构建发布汉化版。
- 构建时从标签去掉 `-cn` 得到版本号，并把完整标签写入 InformationalVersion。
- 使用汉化仓库的作者、描述、标签和 `RepoUrl`。
- 忽略本地 `Directory.Build.props`，避免个人 Dalamud 路径被提交。

保留理由：

- 上游工作流面向 PunishXIV 官方发布、Dynamis、Discord 通知等，不适合汉化仓库直接使用。
- 汉化仓库需要自己的 release 产物、仓库地址和说明。

处理建议：

- 合并新版时优先保留“按 `*-cn` 标签发布”和“汉化仓库元数据”这两个行为。
- GitHub Actions 的版本号、actions 版本、Dalamud 下载地址可以跟随上游或当前可用环境调整。

### AEAssist 战斗模块支持

相关文件：

- `Questionable/Controller/CombatModules/AEAssistModule.cs`
- `Questionable/Configuration.cs`
- `Questionable/QuestionablePlugin.cs`
- `Questionable/Windows/ConfigComponents/PluginConfigComponent.cs`

目的：

- 在战斗模块枚举中加入 `AEAssist`。
- 开始战斗时执行 `/aeTargetSelector off`、`/aestop off`、`/aepull on`。
- 停止时执行 `/aepull off`、`/aestop on`。
- 在依赖/战斗模块设置页展示 AEAssist 选项。

保留理由：

- 上游最新没有 AEAssist 集成。
- AEAssist/AEAssistV3 是国服用户常用的战斗自动化方案，属于实用功能差异。

处理建议：

- 合并时如果上游的 `ICombatModule` 接口或 `CombatController.CombatData` 变化，应重写适配层，但保留“可选择 AEAssist 并通过命令开关拉怪/停手”的行为。

### DailyRoutines 兼容和传送联动

相关文件：

- `Questionable/External/DailyRoutinesIpc.cs`
- `Questionable/Configuration.cs`
- `Questionable/QuestionablePlugin.cs`
- `Questionable/Windows/ConfigComponents/GeneralConfigComponent.cs`
- `Questionable/Controller/Steps/Shared/AethernetShortcut.cs`
- `Questionable.Model/Common/EAetheryteLocation.cs`

目的：

- 在 Questionable 自动任务运行时，临时卸载 DailyRoutines 中容易冲突的模块。
- 停止或退出时恢复被临时卸载的模块。
- 可选使用 DailyRoutines 的 `BetterTeleport`/`/pdrtelepo` 进行主城小水晶传送。
- 用中文地名把 `EAetheryteLocation` 转成 DailyRoutines 能识别的传送名称。

当前冲突模块列表：

- `AutoTalkSkip`
- `AutoCutsceneSkip`
- `OptimizedInteraction`
- `AutoQuestComplete`
- `AutoFateSync`
- `InstantDismount`
- `AutoHideGameObjects`
- `AutoUnlockAllContents`

保留理由：

- 上游最新没有 DailyRoutines IPC。
- 这些是国服环境里减少插件互相抢操作、减少卡任务的重要兼容逻辑。
- 小水晶传送是明显功能增强，但风险高，必须保留开关和提示。

处理建议：

- 合并时优先保留“自动任务时临时禁用冲突模块”的主行为。
- 如果上游改动了小水晶/大水晶任务结构，重新检查 `AethernetShortcut.Executor` 的注入参数和传送触发条件。

### 内置 AutoSnipe 处理

相关文件：

- `Questionable/Tweak/AutoSnipeHandler.cs`
- `Questionable/QuestionablePlugin.cs`

目的：

- Hook 游戏内狙击小游戏相关函数。
- 当 Questionable 正在运行时自动返回完成状态，避免依赖 Automaton 的 AutoSnipe 功能。

保留理由：

- 上游最新仍主要通过 Automaton 相关检测提示处理狙击小游戏，没有汉化版这个内置 hook。
- 这是减少用户额外安装和配置 Automaton 的实用改动。

处理建议：

- 这是签名 hook，FF14/Dalamud/ClientStructs 更新后最容易失效。合并或更新 API 后必须实际测试。
- 如果上游以后提供官方内置处理，可优先采用上游实现，删除本地 hook。

### 国服 vnavmesh 版本兼容补丁

相关文件：

- `Questionable/External/NavmeshIpc.cs`

目的：

- 注释掉上游对 vnavmesh 版本 `< 1.2.3.2` 的强制报错。

保留理由：

- 提交 `5feac8e7` 和 `2312b3df` 都是为国服 navmesh 兼容处理。
- 国服插件源里的 vnavmesh 版本号可能落后、重打包或与国际服不同，但 IPC 仍可用；保留检查会导致插件误判不可用。

处理建议：

- 合并冲突时不要无脑恢复上游的强制版本报错，否则国服用户可能无法寻路。

### 中文地名、职业名、版本名和友好名称

相关文件：

- `Questionable.Model/Common/EAetheryteLocation.cs`
- `Questionable.Model/EExpansionVersion.cs`
- `Questionable/Data/JobExtensions.cs`
- `Questionable/Model/EAlliedSociety.cs`

目的：

- 把水晶、资料片、职业、友好部族等枚举显示为中文。
- `EAetheryteLocation.ToFriendlyString()` 同时服务 DailyRoutines `/pdrtelepo`，不只是界面显示。

保留理由：

- 对普通 UI 来说，上游 i18n 可以替代硬编码中文。
- 但 DailyRoutines 传送需要中文地名作为命令参数，这部分仍是功能依赖。

处理建议：

- 合并上游 i18n 后，优先把纯显示文本迁移到 `I18N.xml`。
- `EAetheryteLocation.ToFriendlyString()` 只要仍被 DailyRoutines 传送使用，就不能直接删除。

### 友好部族坐骑 NPC 数据补充

相关文件：

- `Questionable/Data/AlliedSocietyData.cs`

目的：

- 为 `Qitari` 和 `Pelupelu` 增加 `mountNpcs` 数据。

保留理由：

- 这是行为数据补充，不是翻译。
- 合并时如果上游仍缺这些 NPC，删除会影响对应部族任务或坐骑相关处理。

处理建议：

- 先对照上游最新是否已经加入同等数据。
- 如果上游已经有更完整数据，以新版为准；否则保留本地补充。

### 默认关闭上游问题上报

相关文件：

- `Questionable/Configuration.cs`

目的：

- `DismissedReportWarning` 默认 `true`。
- `ReportsDisabled` 默认 `true`。

保留理由：

- 汉化版用户的问题多数和国服、汉化分支、插件源差异有关，不应默认发到上游。
- 这也避免用户误把汉化版问题报告给原作者。

处理建议：

- 如果上游报告系统迁移到 i18n 或新配置结构，继续保持汉化版默认不向上游报告。

## 可以保留，但需要酌情改造

### 关于页和汉化作者反馈入口

相关文件：

- `Questionable/Windows/ConfigComponents/AboutConfigComponent.cs`
- `Questionable/QuestionablePlugin.cs`

目的：

- 显示汉化版免费声明、原作者、汉化作者、汉化版 issue 地址和插件源地址。

保留理由：

- 这不是核心功能，但对汉化版分发、反倒卖、反馈分流有实际价值。

处理建议：

- 合并上游 i18n 后可以继续保留为汉化版专属页。
- 如果上游配置页结构变化，只要保留同等入口即可，不必保留原文件结构。

### Pandora's Box IPC 报错降噪

相关文件：

- `Questionable/External/PandorasBoxIpc.cs`

目的：

- 注释掉 Pandora IPC 查询、暂停、恢复失败时的 warning 日志。

保留理由：

- 对国服用户来说，没装 Pandora 或 IPC 不匹配时可能反复刷日志，影响排查真正的问题。


### 高级设置里的 NPC 高亮默认关闭

相关文件：

- `Questionable/Configuration.cs`

目的：

- `Advanced.HighlightSelectedNpc` 从默认 `true` 改为默认 `false`。

保留理由：

- 这属于汉化版默认体验偏好，可能是为了减少视觉干扰或插件特征。

### LLib 子模块和依赖锁定

相关文件：

- `.gitmodules`
- `LLib`
- `packages.lock.json`
- 各项目 `packages.lock.json`

目的：

- 曾经改过 LLib 子模块 URL 和版本。

保留理由：

- 只有在汉化仓库构建确实依赖该 URL 或版本时才需要保留。

处理建议：

- 合并上游新版时优先采用上游依赖版本。
- 如果国服构建失败，再单独调整锁文件和子模块。

## 建议迁移到上游 i18n，不建议继续硬编码保留

这些差异主要是中文 UI、命令帮助、提示语、按钮、tooltip、任务状态文本、窗口标题、任务步骤 `ToString()` 等。它们对用户体验有价值，但随着上游引入 i18n，不应继续大量散落在 `.cs` 文件里。

涉及文件包括但不限于：

- `Questionable/Controller/CommandHandler.cs`
- `Questionable/Controller/MiniTaskController.cs`
- `Questionable/Controller/QuestController.cs`
- `Questionable/Controller/QuestRegistry.cs`
- `Questionable/Controller/Steps/**`
- `Questionable/Windows/ConfigComponents/**`
- `Questionable/Windows/JournalComponents/**`
- `Questionable/Windows/QuestComponents/**`
- `Questionable/Windows/PriorityWindow.cs`
- `Questionable/Windows/QuestWindow.cs`
- `Questionable/Windows/UiUtils.cs`
- `Questionable/Windows/Utils/QuestSelector.cs`

处理原则：

- 如果上游 `Questionable/Resources/I18N.xml` 已经有对应 zh-cn 字符串，优先使用上游 i18n，不保留硬编码中文。
- 如果上游缺中文字符串，应把汉化版文本补进 i18n，而不是继续直接改 C# 字符串。
- 任务步骤 `ToString()` 可能用于调试、日志、界面状态。若上游暂未 i18n 化，可以临时保留中文；但长期仍建议资源化。
- 命令 `/qst help`、错误提示、配置页说明这些用户直接可见的文本，应优先保证中文可读。

## 不必保留或只作历史参考

### 2026 已过期季节活动提示

相关提交：

- `fa54fc15`：降神节 2026
- `241d5c15`：恋人节 2026
- `888d079e`：女儿节 2026
- `cde0a7af`：彩蛋狩猎 2026

原因：

- 这些活动在当前日期 `2026-06-23` 已经过期。
- 提交只是维护当时的活动提示列表，不属于长期功能。

处理建议：

- 合并新版时不必为了这些旧活动解决冲突。
- 以后如需活动提示，应按当期活动重新添加，不要恢复旧列表。

### 纯拼写、排版、代码风格或无实际行为差异

例子：

- 只改变英文提示为中文但上游 i18n 已覆盖的行。
- 只改变空格、标点、using、锁文件噪音且不影响构建的差异。
- `Questionable/Data/JournalData.cs` 中未见明确行为作用的额外 using。

处理建议：

- 合并冲突时优先选上游新版。
- 只有在确认会影响汉化版构建或用户可见中文时再保留。

## 合并冲突处理顺序建议

1. 先合并上游代码结构，确保能编译。
2. 再恢复汉化版必须保留的功能：来源限制、AEAssist、DailyRoutines、AutoSnipe、vnavmesh 兼容、发布流程。
3. 再处理中文：优先补 `I18N.xml`，不要急着恢复旧的硬编码中文。
4. 最后处理可选项：关于页、日志降噪、默认设置偏好。
5. 每次解决冲突后至少检查：
   - 插件能否在 Debug 构建加载。
   - Release 构建是否仍有来源限制。
   - AEAssist 选项是否能出现在配置页并能启停。
   - DailyRoutines 冲突模块是否会在任务运行时临时关闭并在停止后恢复。
   - vnavmesh 在国服环境不会被版本号误拦截。
   - `*-cn` 标签是否能触发发布工作流。

## 简短结论

- “翻译文本”本身已经不再是汉化分支最重要的差异，因为上游已有 i18n 和 zh-cn。
- 真正需要保留的是汉化版围绕国服环境做的功能性和兼容性改动：发行来源限制、AEAssist、DailyRoutines、AutoSnipe、vnavmesh 兼容、汉化版发布流程。
- 硬编码中文后续应逐步迁移到上游 i18n，减少每次合并上游时的冲突量。
