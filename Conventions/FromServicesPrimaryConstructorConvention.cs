using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace ColdTrack_Back.Conventions;

/// <summary>
/// 修复 Swashbuckle 6.6.2 主构造函数参数被误当作 Action 参数的已知问题。
/// 将 Controller 主构造函数参数从 Action 参数列表中移除，避免 Swagger 将其显示为查询参数。
/// </summary>
public class FromServicesPrimaryConstructorConvention : IActionModelConvention
{
    public void Apply(ActionModel action)
    {
        var ctorParamNames = action.Controller.ControllerType.GetConstructors()
            .FirstOrDefault(c => c.GetParameters().Length > 0)?
            .GetParameters()
            .Select(p => p.Name)
            .ToHashSet();

        if (ctorParamNames == null || ctorParamNames.Count == 0)
            return;

        var paramsToRemove = action.Parameters
            .Where(p => ctorParamNames.Contains(p.ParameterName))
            .ToList();

        foreach (var p in paramsToRemove)
            action.Parameters.Remove(p);
    }
}
