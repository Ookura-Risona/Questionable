using Dalamud.Interface.Colors;
using Dalamud.Interface.Utility.Raii;
using Dalamud.Plugin;
using Dalamud.Utility;
using Dalamud.Bindings.ImGui;

namespace Questionable.Windows.ConfigComponents;

internal sealed class AboutConfigComponent : ConfigComponent
{
    public AboutConfigComponent(IDalamudPluginInterface pluginInterface, Configuration configuration) : base(pluginInterface, configuration)
    {
    }

    public override void DrawTab()
    {
        using var tab = ImRaii.TabItem("关于###About");
        if (!tab)
            return;

        DrawContent();
    }

    public static void DrawContent()
    {
        ImGui.Separator();
        ImGui.Spacing();
        ImGui.TextColored(ImGuiColors.DalamudRed, "本汉化插件完全开源免费，从未委托任何人在任何渠道售卖。");
        ImGui.TextColored(ImGuiColors.DalamudRed, "如果你是付费购买的本汉化插件，请立即退款并差评举报。");

        // plugin origin author
        ImGui.Text("插件作者：");
        ImGui.SameLine();
        ImGui.TextColored(ImGuiColors.DalamudOrange, "Liza Carvelli");
        
        ImGui.Text("原汉化&国服特供：");
        ImGui.SameLine();
        ImGui.TextColored(ImGuiColors.DalamudOrange, "decorwdyun");

        ImGui.Text("当前国服维护：");
        ImGui.SameLine();
        ImGui.TextColored(ImGuiColors.DalamudOrange, "Ookura-Risona");
        
        if (ImGui.Button("问题反馈（只处理汉化&国服特有的问题）"))
        {
            Util.OpenLink("https://github.com/Ookura-Risona/Questionable/issues/new");
        }
        ImGui.SameLine();
        if (ImGui.Button("国服维护者的插件源"))
        {
            Util.OpenLink("https://github.com/Ookura-Risona/DalamudPlugins");
        }
    }
}
